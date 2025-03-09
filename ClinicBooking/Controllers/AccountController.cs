
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
namespace ClinicBooking.Controllers
{
    

    public class AccountController : Controller
    {
        private readonly AccountService _userService;
        private readonly JwtService _jwtService;

        public AccountController(AccountService userService, JwtService jwtService)
        {
            _userService = userService;
            _jwtService = jwtService;
        }

        [HttpPost]
        public IActionResult Login(LoginModel model)
        {
            var user = _userService.Authenticate(model.Username, model.Password);
            if (user == null)
            {
                return Unauthorized("Sai tài khoản hoặc mật khẩu");
            }

           
            string role = user.Username == "admin" ? "Admin" : "User";

            
            var token = _jwtService.GenerateToken(user, role);

            return Ok(new { Token = token });
        }
    }

}
