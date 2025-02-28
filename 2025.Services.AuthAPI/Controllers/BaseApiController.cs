using _2025.Services.AuthAPI.DTO;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using static _2025.Services.AuthAPI.Core.Constants.MessageConstant;

namespace _2025.Services.AuthAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BaseApiController : ControllerBase
    {
        public BaseApiController() { }

        public async Task<ResponseDataDTO<T>> HandleException<T>(Task<T> task)
        {
            try
            {
                var data = await task;
                return new ResponseDataDTO<T> {Success = true, Data = data };
            }
            catch (ApplicationException ex)
            {
                return new ResponseDataDTO<T> { Success = false, Message = ex.Message };
            }
            catch (Exception ex)
            {
                Serilog.Log.Error(ex, ex.Message);
                return new ResponseDataDTO<T> { Success = false, Message = CommonMessage.ERROR_HAPPEND };
            }
        }
    }
}
