using _2025.Services.AuthAPI.Core;
using _2025.Services.AuthAPI.Core.Entities;
using _2025.Services.AuthAPI.Core.Helper;
using _2025.Services.AuthAPI.DTO;
using System.Text;
using static _2025.Services.AuthAPI.Core.Constants.MessageConstant;

namespace _2025.Services.AuthAPI.Services
{
    public interface IAuthenticationService
    {
        Task<string> Logon(LogonDTO logonDTO);
    }

    public class AuthenticationService : IAuthenticationService
    {
        private IdentityContext _identitycontext;
        private IUserService _userService;
        private AppSettings _appSettings;

        public AuthenticationService( IdentityContext identityContext, IUserService userService, AppSettings appSettings)
        {
            identityContext = _identitycontext;
            userService = _userService;
            _appSettings = appSettings;
        }

        public async Task<string> Logon(LogonDTO model)
        {
            var user = await _userService.GetByEmail(model.Email);

            if (user == null)
            {
                throw new ApplicationException(CommonMessage.NOT_FOUND);
            }

            var saltBytes = Convert.FromBase64String(user.PasswordSalt);
            var password = _userService.CombineSaltAndPassword(saltBytes, model.Password);

            if ( CryptoHelper.VerifyHashedPassword(user.Password, Encoding.Unicode.GetString(password)))
            {
                throw new ApplicationException(UserMessage.LOGIN_FAIL);
            }

            var authenToken = GenerateJwtToken(user);

            return authenToken;
        }

        private string GenerateJwtToken(User user)
        {
            throw new NotImplementedException();
        }
    }
}
