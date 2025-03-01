using _2025.Web.TaskManager.DTO.User;
using _2025.Web.TaskManager.Services;
using Microsoft.AspNetCore.Mvc;

namespace _2025.Web.TaskManager.Controllers
{
    public class AuthController : Controller
    {
        private IAuthApiService _authApiService;

        public AuthController(IAuthApiService authApiService)
        {
            _authApiService = authApiService;
        }

        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Login(LogonDTO model)
        {
            try
            {
                var authenTokenResponse = await _authApiService.Logon(model);

                if (!authenTokenResponse.Success)
                {
                    throw new Exception(authenTokenResponse.Message);
                }
                return RedirectToAction("index", "home");
            }
            catch (Exception ex)
            {
                return RedirectToAction("login", "auth");
            }
        }
    }
}
