using _2025.Services.AuthAPI.Core;

namespace _2025.Services.AuthAPI.Services
{
    public interface IInitService
    {
        void InitFirstUser();
    }

    public class InitService : IInitService
    {
        private IdentityContext _identityContext;
        private IUserService _userService;
        private AppSettings _appSettings;

        public InitService(IdentityContext identityContext, IUserService userService, AppSettings appSettings)
        {
            _identityContext = identityContext;
            _userService = userService;
            _appSettings = appSettings;
        }
        public void InitFirstUser() 
        {
            var admin = _identityContext.Users.FirstOrDefault(t => t.Email == _appSettings.AdminAccount.Email);

            if (admin == null)
            {
                admin = new Core.Entities.User()
                {
                    Id = 0,
                    Email = _appSettings.AdminAccount.Email,
                    UserName = _appSettings.AdminAccount.UserName,
                    Title = _appSettings.AdminAccount.FullName,
                    FullName = _appSettings.AdminAccount.FullName,
                    Role = Core.Enum.RoleEnum.Admin,
                    Status = Core.Enum.UserStatusEnum.Active,
                    CreatedOn = DateTime.SpecifyKind(DateTime.UtcNow, DateTimeKind.Unspecified),
                    Delete = false,
                };


                admin = _userService.SetPassword(admin, _appSettings.AdminAccount.Password);

                _identityContext.Users.Add(admin);
                _identityContext.SaveChanges();
            }
        }
    }
}
