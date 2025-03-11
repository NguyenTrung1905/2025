using _2025.Web.TaskManager.DTO.User;
using _2025.Web.TaskManager.Services;
using Microsoft.AspNetCore.Mvc;

namespace _2025.Web.TaskManager.Controllers
{
    public class UserController : BaseController
    {
        private ILogger<UserController> _logger;
        private IAuthApiService _authApiService;

        public UserController(ILogger<UserController> logger, IAuthApiService authApiService)
        {
            _logger = logger;
            _authApiService = authApiService;
        }

        public IActionResult List()
        {
            if (!IsAdmin)
            {
                return RedirectToAction("accessDenied", "auth");
            }

            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Save([FromForm] SaveUserDTO model)
        {
            if (!IsAdmin)
            {
                return RedirectToAction("accessDenied", "auth");
            }

            try
            {
                if (model.Id == 0)
                {
                    var addUserResponse = await _authApiService.Add(new AddUserDTO()
                    {
                        UserName = model.UserName,
                        Email = model.Email,
                        FullName = model.FullName,
                        Password = model.Password,
                        Role = model.Role,
                        Status = model.Status,
                        Title = model.Title
                    });

                    if (!addUserResponse.Success)
                    {
                        throw new Exception(addUserResponse.Message);
                    }

                    TempData["Message"] = "Add user successfully!";
                }
                else
                {
                    var UpdateUserResponse = await _authApiService.Update(new UpdateUserDTO()
                    {
                        Id = model.Id,
                        UserName = model.UserName,
                        Email = model.Email,
                        FullName = model.FullName,
                        Role = model.Role,
                        Status = model.Status,
                        Title = model.Title
                    });

                    if (!UpdateUserResponse.Success)
                    {
                        throw new Exception(UpdateUserResponse.Message);
                    }

                    TempData["Message"] = "Update user successfully!";
                }
            }
            catch (Exception ex)
            {
                _logger.LogError($"Failed: {ex.Message} \n {ex.StackTrace}");

                TempData["Message"] = ex.Message;
            }

            return RedirectToAction("List", "user");
        }
    }
}
