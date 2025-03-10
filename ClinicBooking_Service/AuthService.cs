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
        public AuthService(IConfiguration configuration , IUserRepository userRepository , EmailService emailService)
        {
            _configuration = configuration;
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
            if (user == null) return false;

            string verificationCode = Guid.NewGuid().ToString().Substring(0, 8);
            DateTime expiry = DateTime.UtcNow.AddMinutes(30);
            await _userRepository.SaveResetTokenAsync(user.UserId, verificationCode, expiry);

            // Tạo reset link sử dụng verificationCode
            string resetLink = $"https://yourwebsite.com/Auth/ResetPassword?code={verificationCode}";

            // Gửi email reset password với reset link
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
