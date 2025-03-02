using _2025.Web.TaskManager.DTO.User;
using _2025.Web.TaskManager.Services;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace _2025.Web.TaskManager.Controllers
{
    public class AuthController : Controller
    {
        private IAuthApiService _authApiService;

        public AuthController(IAuthApiService authApiService)
        {
            _authApiService = authApiService;
        }

        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Login(LogonDTO model)
        {
            try
            {
                var authenTokenResponse = await _authApiService.Logon(model);

                if (!authenTokenResponse.Success)
                {
                    throw new Exception(authenTokenResponse.Message);
                }

                await SignInUser(authenTokenResponse.Data);

                return RedirectToAction("index", "home");
            }
            catch (Exception ex)
            {
                return RedirectToAction("login", "auth");
            }
        }

        private async Task SignInUser(string jwtToken)
        {
            var handle = new JwtSecurityTokenHandler();
            var jwt = handle.ReadJwtToken(jwtToken);
            var identity = new ClaimsIdentity(CookieAuthenticationDefaults.AuthenticationScheme);

            identity.AddClaim(new Claim(JwtRegisteredClaimNames.Sub, jwt.Claims.FirstOrDefault(t => t.Type == JwtRegisteredClaimNames.Sub).Value));
            identity.AddClaim(new Claim("UserId", jwt.Claims.FirstOrDefault(t => t.Type == "UserId").Value));
            identity.AddClaim(new Claim("UserName", jwt.Claims.FirstOrDefault(t => t.Type == "UserName").Value));
            identity.AddClaim(new Claim("Email", jwt.Claims.FirstOrDefault(t => t.Type == "Email").Value));
            identity.AddClaim(new Claim("FullName", jwt.Claims.FirstOrDefault(t => t.Type == "FullName").Value));
            identity.AddClaim(new Claim("Roles", jwt.Claims.FirstOrDefault(t => t.Type == "Roles").Value));

            var principal = new ClaimsPrincipal(identity);

            await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, principal);

        }
    }
}
