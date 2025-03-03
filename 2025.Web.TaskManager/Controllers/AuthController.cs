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

        [HttpGet]
        public IActionResult SignUp()
        {
            return View();
        }

        [HttpGet]
        public IActionResult AccessDenied()
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

                TempData["Message"] = "Login successully!";

                return RedirectToAction("list", "taskitem");
            }
            catch (Exception ex)
            {
                TempData["Message"] = ex.Message;

                return RedirectToAction("login", "auth");
            }
        }

        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            return RedirectToAction("login");
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
