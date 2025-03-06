using _2025.Web.TaskManager.Core.Enum;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace _2025.Web.TaskManager.Controllers
{
    [Authorize]
    public class BaseController : Controller
    {
        public List<string> Roles
        {
            get
            {
                return User?.Claims?.Where(t=>t.Type == "Roles")?.Select(t=> t.Value)?.ToList();
            }
        }

        public bool IsAdmin
        {
            get
            {
                return Roles?.Any(t=> t == RoleEnum.Admin.ToString()) ?? false;
            }
        }

        public int UserId
        {
            get
            {
                return int.Parse(User?.Claims?.FirstOrDefault(t => t.Type == "UserId")?.Value?? "0");
            }
        }
    }
}
