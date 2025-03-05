using _2025.Services.AuthAPI.Core;
using _2025.Services.AuthAPI.Core.Entities;
using _2025.Services.AuthAPI.Core.Helper;
using _2025.Services.AuthAPI.DTO;
using Microsoft.EntityFrameworkCore;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;
using static _2025.Services.AuthAPI.Core.Constants.MessageConstant;

namespace _2025.Services.AuthAPI.Services
{
    public interface IUserService
    {
        Task<User> GetByEmail(string email);
        User SetPassword(User user, string password);
        byte[] CombineSaltAndPassword(byte[] saltBytes, string password);
        Task<User> SignUp(LogonDTO model);
    }

    public class UserService : IUserService
    {
        // Chỉ chứa chữ cái (a-z, A-Z), số (0-9) và dấu gạch dưới (_).
        // Độ dài từ 3 đến 30 ký tự.
        // Không bắt đầu hoặc kết thúc bởi dấu gạch duới (_).
        public const string UserNamePattern = @"^(?=.{3,30}$)(?![_])[a-zA-Z0-9_]+(?<![_])$";

        // Bắt đầu bằng một hoặc nhiều chữ cái, chữ số hoặc các ký tự đặc biệt như . và _.
        // Có một ký tự @ để phân cách giữa tên đăng nhập và tên miền.
        // Tên miền phải chứa ít nhất một dấu chấm (.)
        // Phần mở rộng tên miền có thể là 2 đến 4 ký tự (ví dụ: com, net, info,...).
        public const string EmailPattern = @"^[a-zA-Z0-9._%+-]+@[a-zA-Z0-9.-]+\.[a-zA-Z]{2,4}$";

        // Độ dài từ 8 đến 20 ký tự.
        // Có ít nhất một chữ cái viết thường, một chữ cái viết hoa, một số và một ký tự đặc biệt.
        public const string PasswordPattern = @"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)(?=.*[@$!%*?&])[A-Za-z\d@$!%*?&]{8,20}$";

        private IdentityContext _identityContext;

        public UserService(IdentityContext identityContext)
        {/*Khắc phục lỗi identity trả về null, bởi ở đây, biến _identityContext (là thành viên của lớp) không được khởi tạo vì ta lại gán giá trị của nó cho tham số identityContext
          *thay vì ngược lại. Do đó, sau khi constructor chạy, _identityContext vẫn là null, dẫn đến lỗi khi sử dụng sau này.
          Cách khắc phục: đổi từ identityContext = _identityContext; thành  _identityContext = identityContext;
          Việc này đảm bảo rằng đối tượng IdentityContext được DI (Dependency Injection) tiêm vào được gán cho biến _identityContext của lớp, từ đó khi gọi các phương thức như 
          GetByEmail thì _identityContext không còn là null.*/
            _identityContext = identityContext;
        }
        public User SetPassword(User user, string password)
        {
            // Tạo mảng byte để chứa "muối" (salt)
            var saltBytes = new byte[16];

            // Sử dụng RandomNumberGenerator thay thế cho RNGCryptoServiceProvider
            using var random = RandomNumberGenerator.Create();
            random.GetBytes(saltBytes);

            // Mã hóa mảng salt thành chuỗi Base64 để lưu trong user.PasswordSalt
            user.PasswordSalt = Convert.ToBase64String(saltBytes);

            // Tạo password hash (giả sử ComputeHashBase64 là hàm băm + Base64)
            user.Password = ComputeHashBase64(saltBytes, password);

            return user;
        }

        private string ComputeHashBase64(byte[] saltBytes, string password)
        {
            var combinedBytes = CombineSaltAndPassword(saltBytes, password);

            return CryptoHelper.HashPassword(Encoding.Unicode.GetString(combinedBytes));
        }

        public byte[] CombineSaltAndPassword(byte[] saltBytes, string password)
        {
            var passwordBytes = Encoding.Unicode.GetBytes(password);
            return saltBytes.Concat(passwordBytes).ToArray();
        }

        public async Task<User> GetByEmail(string email)
        {
            if (string.IsNullOrWhiteSpace(email))
            {
                throw new ApplicationException(CommonMessage.MISSING_PARAM);
            }

            email = email.Trim().ToLower();

            return await _identityContext.Users.FirstOrDefaultAsync(t=> t.Email.ToLower() == email && !t.Delete && t.Status == Core.Enum.UserStatusEnum.Active);
        }

        public async Task<User> SignUp(LogonDTO model)
        {
            if(model.Password != model.ConfirmPassword)
            {
                throw new ApplicationException(UserMessage.CONFIRM_PASSWORD_NOT_CORRECT);
            }
            
            ValidateUserInfo(model.UserName, model.Email, model.Password);

            if(await _identityContext.Users.AnyAsync(t => t.UserName.ToLower() == model.UserName.ToLower() && !t.Delete))
            {
                throw new ApplicationException(UserMessage.USERNAME_EXIST);
            }

            if (await _identityContext.Users.AnyAsync(t => t.Email.ToLower() == model.Email.ToLower() && !t.Delete))
            {
                throw new ApplicationException(UserMessage.EMAIL_EXIST);
            }

            var user = new User
            {
                Id = 0,
                UserName = model.UserName,
                Email = model.Email,
                Role = Core.Enum.RoleEnum.User,
                Status = Core.Enum.UserStatusEnum.Active,
                CreatedOn = DateTime.UtcNow,
                Delete = false,
            };

            user = SetPassword(user, model.Password);

            await _identityContext.Users.AddAsync(user);
            await _identityContext.SaveChangesAsync();

            return user;
        }

        private void ValidateUserInfo(string userName, string email, string password)
        {
            if (string.IsNullOrWhiteSpace(userName))
            {
                throw new ApplicationException(UserMessage.USERNAME_REQUIRED);
            }
            if (string.IsNullOrWhiteSpace(email))
            {
                throw new ApplicationException(UserMessage.EMAIL_REQUIRED);
            }
            if (string.IsNullOrWhiteSpace(password))
            {
                throw new ApplicationException(UserMessage.PASSWORD_REQUIRED);
            }
            if (!Regex.IsMatch(userName, UserNamePattern))
            {
                throw new ApplicationException(UserMessage.INVALIDID_USERNAME);
            }
            if (!Regex.IsMatch(email, EmailPattern))
            {
                throw new ApplicationException(UserMessage.INVALIDID_EMAIL);
            }
            if (!Regex.IsMatch(password, PasswordPattern))
            {
                throw new ApplicationException(UserMessage.INVALIDID_PASSWORD);
            }
        }
    }   
}
