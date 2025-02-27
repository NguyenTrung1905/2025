using Microsoft.AspNetCore.Mvc;

namespace _2025.Web.TaskManager.Controllers
{
    public class AuthController : Controller
    {
        public IActionResult Login()
        {
            return View();
        }
    }
}
