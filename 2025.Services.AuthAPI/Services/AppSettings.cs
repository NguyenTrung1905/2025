using _2025.Services.AuthAPI.DTO;

namespace _2025.Services.AuthAPI.Services
{
    public class AppSettings
    {
        private IConfiguration _configuration;

        public AppSettings(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        private T GetT<T>(string key)
        {
            if (string.IsNullOrWhiteSpace(key))
            {
                return _configuration.Get<T>();
            }
            if (_configuration.GetSection(key) == null)
            {
                return default;
            }
            return _configuration.GetSection(key).Get<T>();
        }

        //public List<int> SampleConfig => GetT<List<int>>("SampleConfig");

        public AdminAccountDTO AdminAccount => GetT<AdminAccountDTO>("AdminAccount");
    }
}
