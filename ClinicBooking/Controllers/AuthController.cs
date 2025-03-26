using Microsoft.AspNetCore.Mvc;
using ClinicBooking.Services;
using ClinicBooking.Models;
using Microsoft.AspNetCore.Http;
using System.Threading.Tasks;
using ClinicBooking_Data.Repositories.Interfaces;
using ClinicBooking.Entities;
using Microsoft.EntityFrameworkCore;


namespace ClinicBooking.Controllers
{
    public class AuthController : Controller
    {
        private readonly AuthService _authService;
        private readonly IUserRepository _userRepository;
        private readonly AccountService _accountService;

        public AuthController(AuthService authService, IUserRepository userRepository, AccountService accountService)
        {
            _authService = authService;
            _userRepository = userRepository;
            _accountService = accountService;
        }

        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }


        [HttpPost]
        public async Task<IActionResult> Login(LoginModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            var user = await _userRepository.GetByUsernameAsync(model.Username);
            if (user == null || !_authService.VerifyPassword(model.Password, user.PasswordHash))
            {
                ModelState.AddModelError("", "Invalid username or password");
                return View(model);
            }

            var token = _authService.GenerateJwtToken(user.Username, user.Role?.RoleId ?? 0);

            Response.Cookies.Append("JwtToken", token, new CookieOptions
            {
                HttpOnly = true,
                Secure = Request.IsHttps,
                Expires = DateTime.UtcNow.AddMinutes(60)
            });
            int roleId = user.Role?.RoleId ?? 0;
            if (roleId == 3)
                return RedirectToAction("View", "Account");

            if (roleId == 1)
                return RedirectToAction("Index", "Home");

            return RedirectToAction("Index", "Home");
           
        }




        [HttpPost]
        public IActionResult Logout()
        {
            Response.Cookies.Delete("JwtToken");
            return RedirectToAction("Login");
        }
        [HttpGet]
        public IActionResult Register()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Register(RegisterModel model)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return View(model);
                }

                    var newUser = await _accountService.RegisterAsync(
                        model.FullName,
                        model.Email,
                        model.Phone,
                        model.Username,
                        model.Password
                    );

                    return RedirectToAction("Login");
                }
                catch (DbUpdateException ex) // Bắt lỗi khi lưu database
                {
                    ViewBag.Error = ex.InnerException?.Message ?? ex.Message;
                    return View(model);
                }
                catch (Exception ex) 
                {
                    ViewBag.Error = ex.Message;
                    return View(model);
                }
            
            }


        [HttpGet]
        public IActionResult ForgotPassword()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> ForgotPassword(ForgotPasswordModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            bool result = await _authService.ForgotPasswordAsync(model.Email);
            if (!result)
            {
                ViewBag.Error = "Email không tồn tại!";
                return View(model);
            }

            ViewBag.Message = "Email đặt lại mật khẩu đã được gửi!";
            return View();
        }

        //  đặt lại mật khẩu
        [HttpGet]
        public async Task<IActionResult> ResetPassword(string token)
        {
            if (string.IsNullOrEmpty(token))
            {
                return NotFound(); 
            }

            
            var user = await _userRepository.GetByResetTokenAsync(token);
            if (user == null || user.ResetTokenExpiry < DateTime.UtcNow)
            {
                return NotFound(); 
            }

            var model = new ResetPasswordModel
            {
                Token = token,
                Email = user.Email 
            };

            return View(model);
        }



        [HttpPost]
        public async Task<IActionResult> ResetPassword(ResetPasswordModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model); 
            }

            bool result = await _authService.ResetPasswordAsync(model.Token, model.NewPassword);
            if (!result)
            {
                ViewBag.Error = "Liên kết đặt lại mật khẩu không hợp lệ hoặc đã hết hạn!";
                return View(model); // Trả lại lỗi nếu token không hợp lệ
            }

            TempData["Message"] = "Mật khẩu đã được đặt lại thành công!";
            return RedirectToAction("Login", "Auth");
        }



    }
}