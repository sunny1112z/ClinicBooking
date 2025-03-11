using ClinicBooking.Entities;
using ClinicBooking.Services;
using Microsoft.AspNetCore.Mvc;

namespace ClinicBooking.Controllers
{
    public class AccountController : Controller
    {
        private readonly AccountService _accountService;
        private readonly JwtService _jwtService;

        public AccountController(AccountService accountService, JwtService jwtService)
        {
            _accountService = accountService;
            _jwtService = jwtService;
        }

        [HttpGet]
        public async Task<IActionResult> View()
        {
            var users = await _accountService.GetAllUsersAsync();
            return View(users);
        }

       
        [HttpGet]
        public IActionResult Create()
        {
            return PartialView("_CreateUser", new User());
        }

       
        [HttpPost]
        public async Task<IActionResult> Create(User model)
        {
            if (!ModelState.IsValid)
            {
                return PartialView("_CreateUser", model);
            }
            model.PasswordHash = BCrypt.Net.BCrypt.HashPassword(model.PasswordHash);
            await _accountService.AddUserAsync(model);
            return Json(new { success = true });
        }
    }
}
