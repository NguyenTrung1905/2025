using Microsoft.AspNetCore.Mvc;

namespace _2025.Web.TaskManager.Controllers
{
    public class TaskItemController : BaseController
    {
        public IActionResult List()
        {
            return View();
        }
    }
}
