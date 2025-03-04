using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ClinicBooking.
namespace ClinicBooking.Controllers
{
    public class AuthController : Controller
    {
        private readonly AuthService _authService;

        public AuthController(AuthService authService)
        {
            _authService = authService;
        }
        // GET: AuthController
        public ActionResult Index()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Login(string email, string password)
        {
            var user = await _authService.AuthenticateUser(email, password);
            var userRole = await _authService.GetIsActiveByEmail(email);
            if (user == null)
            {
                ViewBag.Error = "Invalid email or password";
                return View();
            }
            if (userRole == 1)
            {
                HttpContext.Session.SetInt32("UserId", user.AccountId);
                HttpContext.Session.SetInt32("UserRole", user.AccountRole ?? -1);

                if (user.AccountRole == 0)
                {
                    return RedirectToAction("Index", "Account");
                }
                if (user.AccountRole == 1)
                {
                    return RedirectToAction("Index", "Category");
                }
                if (user.AccountRole == 2)
                {
                    return RedirectToAction("Manage", "NewsArticle");
                }

            }
            else
            {
                TempData["ErrorMessage"] = "Your account has been locked";
                return View("CannotLogin");

            }
            return RedirectToAction("Index", "NewsArticle");
        }
     
        public ActionResult Details(int id)
        {
            return View();
        }

        public ActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(IFormCollection collection)
        {
            try
            {
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        // GET: AuthController/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: AuthController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int id, IFormCollection collection)
        {
            try
            {
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        // GET: AuthController/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: AuthController/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int id, IFormCollection collection)
        {
            try
            {
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }
    }
}
