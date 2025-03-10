using _2025.Services.AuthAPI.DTO;
using _2025.Services.AuthAPI.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace _2025.Services.AuthAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : BaseApiController
    {
        private IUserService _userService;

        public UserController(IUserService userService)
        {
            _userService = userService;
        }

        [HttpPut("add")]
        public async Task<ResponseDataDTO<bool>> Add([FromBody] AddUserDTO model)
        {
            return await HandleException(_userService.Add(model));
        }
    }
}
