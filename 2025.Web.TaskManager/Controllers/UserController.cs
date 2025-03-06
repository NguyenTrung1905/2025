using Microsoft.AspNetCore.Mvc;

namespace _2025.Web.TaskManager.Controllers
{
    public class UserController : BaseController
    {
        public IActionResult List()
        {
            if (!IsAdmin)
            {
                return RedirectToAction("accessDenied", "auth");
            }

            return View();
        }
    }
}
