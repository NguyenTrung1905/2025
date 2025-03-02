using Microsoft.AspNetCore.Mvc;

namespace _2025.Web.TaskManager.Controllers
{
    public class UserController : Controller
    {
        public IActionResult List()
        {
            return View();
        }
    }
}
