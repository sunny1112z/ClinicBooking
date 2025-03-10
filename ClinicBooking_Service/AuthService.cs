using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using BCrypt.Net;
using ClinicBooking_Data.Repositories.Interfaces;

namespace ClinicBooking.Services
{
    public class AuthService
    {
        private readonly IConfiguration _configuration;
        private readonly IUserRepository _userRepository;
        private readonly EmailService _emailService;
        public AuthService(IConfiguration configuration, IUserRepository userRepository, EmailService emailService)
        {
            _configuration = configuration;
            _userRepository = userRepository;  
            _emailService = emailService;      
        }


        public string HashPassword(string password)
        {
            return BCrypt.Net.BCrypt.HashPassword(password);
        }

        public bool VerifyPassword(string password, string hashedPassword)
        {
            return BCrypt.Net.BCrypt.Verify(password, hashedPassword);
        }

        public string GenerateJwtToken(string username, int role)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var keyString = _configuration["Jwt:Key"];

            if (string.IsNullOrEmpty(keyString))
            {
                throw new Exception("JWT Key is missing from configuration.");
            }

            var key = Encoding.UTF8.GetBytes(keyString);

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new[]
                {
            new Claim(ClaimTypes.Name, username),
            new Claim(ClaimTypes.Role, role.ToString())
        }),
                Expires = DateTime.UtcNow.AddHours(1),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };

            var token = tokenHandler.CreateToken(tokenDescriptor);
            var jwt = tokenHandler.WriteToken(token);

            Console.WriteLine($"Generated JWT: {jwt}"); 

            return jwt;
        }
        public async Task<bool> ForgotPasswordAsync(string email)
        {
            var user = await _userRepository.GetByEmailAsync(email);

            if (user == null)
            {
                throw new Exception("Không tìm thấy người dùng");
            }

            // Tạo token (mã xác nhận) và thời gian hết hạn cho token
            string token = Guid.NewGuid().ToString(); // Token duy nhất
            DateTime expiry = DateTime.UtcNow.AddMinutes(30); // Token có hạn 30 phút

            // Lưu token vào cơ sở dữ liệu cùng với thời gian hết hạn
            await _userRepository.SaveResetTokenAsync(user.UserId, token, expiry);

            // Tạo URL reset mật khẩu và gửi qua email
            string resetLink = $"https://localhost:7278/Auth/ResetPassword?token={token}";
            await _emailService.SendEmailAsync(user.Email, "Reset Password", $"Click here to reset: {resetLink}");

            return true;
        }



        // 🟢 Đặt lại mật khẩu: Kiểm tra token, cập nhật mật khẩu mới
        public async Task<bool> ResetPasswordAsync(string token, string newPassword)
        {
            var user = await _userRepository.GetByResetTokenAsync(token);
            if (user == null || user.ResetTokenExpiry < DateTime.UtcNow)
            {
                return false;
            }

            user.PasswordHash = BCrypt.Net.BCrypt.HashPassword(newPassword);
            user.ResetToken = null;
            user.ResetTokenExpiry = null;

            await _userRepository.UpdateAsync(user);
            return true;
        }
    }
}
