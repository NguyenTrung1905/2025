using _2025.Web.TaskManager.DTO.Config;

namespace _2025.Web.TaskManager.Services
{
    public class AppSettings
    {
        private IConfiguration _configuration;

        public AppSettings(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        private T Get<T>(string key = null)
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

        public ServicesDTO Services => Get<ServicesDTO>("Services");
    }
}
