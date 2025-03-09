using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using System.Security.Claims;

public class AuthController : Controller
{
    private readonly AuthService _authService;
    private readonly AccountService _accountService;

    public AuthController(AuthService authService, AccountService accountService)
    {
        _authService = authService;
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
        var user = _accountService.GetUserByUsername(model.Username);
        if (user == null || !_authService.VerifyPassword(model.Password, user.PasswordHash))
        {
            ModelState.AddModelError("", "Invalid username or password");
            return View(model);
        }

        // ✅ Lấy RoleName từ bảng Role
        var roleName = user.Role.RoleName;

        // ✅ Tạo JWT Token
        var token = _authService.GenerateJwtToken(user.Username, roleName);

        // ✅ Lưu JWT vào Cookie
        Response.Cookies.Append("JwtToken", token, new CookieOptions
        {
            HttpOnly = true,
            Secure = true,
            Expires = DateTime.UtcNow.AddMinutes(60)
        });

        return RedirectToAction("Index", "Home");
    }

    [HttpPost]
    public IActionResult Logout()
    {
        Response.Cookies.Delete("JwtToken");
        return RedirectToAction("Login");
    }
}
