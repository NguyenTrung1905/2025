using _2025.Services.AuthAPI.Core.Entities;
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

        [HttpPost("update")]
        public async Task<ResponseDataDTO<bool>> Update([FromBody] UpdateUserDTO model)
        {
            return await HandleException(_userService.Update(model));
        }

        [HttpGet("Search")]
        public async Task<ResponseDataDTO<List<User>>> Search([FromQuery] BaseSearchDTO model)
        {
            var result = await HandleException(_userService.Search(model));

            result.metaData = new MetaData() { PageIndex = model.PageIndex, PageSize = model.PageSize, Total =model.Total };

            return result;
        }
    }
}
