using _2025.Services.AuthAPI.Core;
using _2025.Services.AuthAPI.Core.Entities;
using _2025.Services.AuthAPI.Core.Helper;
using _2025.Services.AuthAPI.DTO;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using static _2025.Services.AuthAPI.Core.Constants.MessageConstant;

namespace _2025.Services.AuthAPI.Services
{
    public interface IAuthenticationService
    {
        Task<string> Logon(LogonDTO model);
    }

    public class AuthenticationService : IAuthenticationService
    {
        private IdentityContext _identityContext;
        private IUserService _userService;
        private AppSettings _appSettings;

        public AuthenticationService(IdentityContext identityContext, IUserService userService, AppSettings appSettings)
        {
            _identityContext = identityContext;
            _userService = userService;
            _appSettings = appSettings;
        }

        public async Task<string> Logon(LogonDTO model)
        {
            User user = null;

            if (model.IsSignUp)
            {
                user = await _userService.SignUp(model);
            }
            else
            {
                user = await _userService.GetByEmail(model.Email);

            }

            if (user == null)
            {
                throw new ApplicationException(CommonMessage.NOT_FOUND);
            }

            var saltBytes = Convert.FromBase64String(user.PasswordSalt);
            var password = _userService.CombineSaltAndPassword(saltBytes, model.Password);

            if (!CryptoHelper.VerifyHashedPassword(user.Password, Encoding.Unicode.GetString(password)))
            {
                throw new ApplicationException(UserMessage.LOGIN_FAIL);
            }

            if(user != null)
            {
                var authenToken = GenerateJwtToken(user);

                return authenToken;
            }

            return string.Empty;
        }

        private string GenerateJwtToken(User user)
        {
            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_appSettings.JwtSecurityKey));
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

            var jwtTokenHandler = new JwtSecurityTokenHandler();
            var tokenDescriptor = new SecurityTokenDescriptor()
            {
                Subject = new System.Security.Claims.ClaimsIdentity(new[]
                {
                    new Claim(JwtRegisteredClaimNames.Sub, user.Id.ToString()),
                    new Claim("UserId", user.Id.ToString()),
                    new Claim("UserName", user.UserName),
                    new Claim("Email", user.Email),
                    new Claim("FullName", user.FullName ?? string.Empty),
                    new Claim("Roles", user.Role.ToString())
                }),
                TokenType = "at+jwt",
                Expires = DateTime.UtcNow.AddHours(72),
                SigningCredentials = credentials,

            };

            var Token = jwtTokenHandler.CreateJwtSecurityToken(tokenDescriptor);
            var jwtToken = jwtTokenHandler.WriteToken(Token);

            return jwtToken;
        }
    }
}
