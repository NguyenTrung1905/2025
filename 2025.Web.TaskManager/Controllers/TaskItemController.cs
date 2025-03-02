using Microsoft.AspNetCore.Mvc;

namespace _2025.Web.TaskManager.Controllers
{
    public class TaskItemController : Controller
    {
        public IActionResult List()
        {
            return View();
        }
    }
}
