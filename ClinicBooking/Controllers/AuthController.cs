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

        public AuthController(AuthService authService, IUserRepository userRepository , AccountService accountService)
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
                
            }

            var token = _authService.GenerateJwtToken(user.Username, user.Role.RoleId);

            Console.WriteLine($"Token: {token}");

            Response.Cookies.Append("JwtToken", token, new CookieOptions
            {
                HttpOnly = true,
                Secure = Request.IsHttps, 
                Expires = DateTime.UtcNow.AddMinutes(60)
            });

            Console.WriteLine("Token set in cookies.");

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
        public async Task<IActionResult> Register(User user)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return View(user);
                }

                var newUser = await _accountService.RegisterAsync(
                    user.FullName,
                    user.Email,
                    user.Phone,
                    user.Username,
                    user.PasswordHash 
                );

                return RedirectToAction("Login");
            }
            catch (Exception ex)
            {
                ViewBag.Error = ex.Message;
                return View(user);
            }
        }


    }
}
