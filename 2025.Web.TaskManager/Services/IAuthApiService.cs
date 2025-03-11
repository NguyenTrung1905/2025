using _2025.Web.TaskManager.DTO.Common;
using _2025.Web.TaskManager.DTO.User;
using Newtonsoft.Json;
using System.Text;

namespace _2025.Web.TaskManager.Services
{
    public interface IAuthApiService
    {
        Task<ResponseDataDTO<string>> Logon(LogonDTO model);
        
        Task<ResponseDataDTO<bool>> Add(AddUserDTO model);
        Task<ResponseDataDTO<bool>> Update(UpdateUserDTO model);
    }

    public class AuthApiService : IAuthApiService
    {
        private ILogger<AuthApiService> _logger;
        private AppSettings _appSettings;

        public AuthApiService(ILogger<AuthApiService> logger, AppSettings appSettings)
        {
            _logger = logger;
            _appSettings = appSettings;
        }

        public async Task<ResponseDataDTO<bool>> Add(AddUserDTO model)
        {
            var url = $"{_appSettings.Services.AuthApiUri}/api/user/add";
            using (var client = new HttpClient())
            {
                try
                {
                    var jsonData = JsonConvert.SerializeObject(model);
                    var requestContent = new StringContent(jsonData, Encoding.UTF8, "application/json");
                    var response = await client.PutAsync(url, requestContent);

                    if (!response.IsSuccessStatusCode)
                    {
                        _logger.LogError("Add StatusCode = " + response.StatusCode);
                    }

                    var data = await response.Content.ReadAsStringAsync();
                    var result = JsonConvert.DeserializeObject<ResponseDataDTO<bool>>(data);

                    return result;
                }
                catch (Exception ex)
                {
                    _logger.LogError("add ex url = " + url);
                    _logger.LogError("add ex message = " + ex.Message);
                }
                finally
                {
                    client.Dispose();
                }
            }

            return null;
        }

        public async Task<ResponseDataDTO<string>> Logon(LogonDTO model)
        {
            var url = $"{_appSettings.Services.AuthApiUri}/api/authentication/logon";
            using (var client = new HttpClient())
            {
                try
                {
                    var jsonData = JsonConvert.SerializeObject(model);
                    var requestContent = new StringContent(jsonData, Encoding.UTF8, "application/json");
                    var response = await client.PostAsync(url, requestContent);

                    if (!response.IsSuccessStatusCode)
                    {
                        _logger.LogError("Logon StatusCode = " + response.StatusCode);
                    }

                    var data = await response.Content.ReadAsStringAsync();
                    var result = JsonConvert.DeserializeObject<ResponseDataDTO<string>>(data);

                    return result;
                }
                catch (Exception ex)
                {
                    _logger.LogError("logon ex url = " + url);
                    _logger.LogError("logon ex message = " + ex.Message);
                }
                finally
                {
                    client.Dispose();
                }
            }

            return null;
        }

        public async Task<ResponseDataDTO<bool>> Update(UpdateUserDTO model)
        {
            var url = $"{_appSettings.Services.AuthApiUri}/api/user/update";
            using (var client = new HttpClient())
            {
                try
                {
                    var jsonData = JsonConvert.SerializeObject(model);
                    var requestContent = new StringContent(jsonData, Encoding.UTF8, "application/json");
                    var response = await client.PostAsync(url, requestContent);

                    if (!response.IsSuccessStatusCode)
                    {
                        _logger.LogError("Update StatusCode = " + response.StatusCode);
                    }

                    var data = await response.Content.ReadAsStringAsync();
                    var result = JsonConvert.DeserializeObject<ResponseDataDTO<bool>>(data);

                    return result;
                }
                catch (Exception ex)
                {
                    _logger.LogError("update ex url = " + url);
                    _logger.LogError("update ex message = " + ex.Message);
                }
                finally
                {
                    client.Dispose();
                }
            }

            return null;
        }
    }
}
