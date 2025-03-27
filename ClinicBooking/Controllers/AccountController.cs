using ClinicBooking.Entities;
using ClinicBooking.Services;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

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
        //Create Account 
       
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
                var errors = ModelState
                    .Where(x => x.Value.Errors.Any())
                    .ToDictionary(
                        kvp => kvp.Key,
                        kvp => kvp.Value.Errors.Select(e => e.ErrorMessage).ToList()
                    );

                return BadRequest(new { success = false, errors });
            }

            try
            {
                var existingUser = await _accountService.GetUserByEmailAsync(model.Email);
                if (existingUser != null)
                {
                    return BadRequest(new { success = false, message = "Email đã tồn tại, vui lòng chọn email khác!" });
                }
                model.PasswordHash = BCrypt.Net.BCrypt.HashPassword(model.PasswordHash);
                await _accountService.AddUserAsync(model);
                return Json(new { success = true });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { success = false, message = "Lỗi hệ thống: " + ex.Message });
            }
        }

        // Update Account
        [HttpGet]
        public async Task<IActionResult> Edit(int id)
        {
            var user = await _accountService.GetUserByIdAsync(id);
            if (user == null)
            {
                return NotFound();
            }
            return PartialView("_EditUser", user);
        }

        [HttpPost]
        public async Task<IActionResult> Update(User model)
        {
            if (!ModelState.IsValid)
            {
                var errors = ModelState
                    .Where(x => x.Value.Errors.Any())
                    .ToDictionary(
                        kvp => kvp.Key,
                        kvp => kvp.Value.Errors.Select(e => e.ErrorMessage).ToList()
                    );

                return BadRequest(new { success = false, errors });
            }

            var user = await _accountService.GetUserByIdAsync(model.UserId);
            if (user == null)
            {
                return NotFound();
            }

            user.FullName = model.FullName;
            user.Email = model.Email;
            user.Phone = model.Phone;
            user.Address = model.Address;
            user.RoleId = model.RoleId;
            user.GenderId = model.GenderId;

            var result = await _accountService.UpdateUserAsync(user);

            if (result)
            {
                return Json(new { success = true, message = "Cập nhật thành công!" });
            }

            return StatusCode(500, "Có lỗi xảy ra khi cập nhật.");
        }
    }
}
