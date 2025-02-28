using _2025.Services.AuthAPI.DTO;
using _2025.Services.AuthAPI.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace _2025.Services.AuthAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthenticationController : BaseApiController
    {
        private IAuthenticationService _authenticationService;

        public AuthenticationController(IAuthenticationService authenticationService)
        {
            _authenticationService = authenticationService;
        }

        [HttpPost("logon")]
        public async Task<ResponseDataDTO<string>> Logon([FromBody] LogonDTO model)
        {
            return await HandleException(_authenticationService.Logon(model));
        }
    }
}
