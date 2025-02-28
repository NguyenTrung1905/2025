using _2025.Services.AuthAPI.Core;
using _2025.Services.AuthAPI.Core.Entities;
using _2025.Services.AuthAPI.Core.Helper;
using Microsoft.EntityFrameworkCore;
using System.Security.Cryptography;
using System.Text;
using static _2025.Services.AuthAPI.Core.Constants.MessageConstant;

namespace _2025.Services.AuthAPI.Services
{
    public interface IUserService
    {
        Task<User> GetByEmail(string email);
        User SetPassword(User user, string password);
        byte[] CombineSaltAndPassword(byte[] saltBytes, string password);
    }

    public class UserService : IUserService
    {
        private IdentityContext _identityContext;
        public UserService(IdentityContext identityContext) 
        {
            identityContext = _identityContext;
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

        public Task<User> GetByEmail(string email)
        {
            if (string.IsNullOrWhiteSpace(email))
            {
                throw new ApplicationException(CommonMessage.MISSING_PARAM);
            }

            // Kiểm tra _identityContext có null hay không
            if (_identityContext == null)
            {
                throw new ApplicationException("Identity context is not initialized.");
            }

            email = email.Trim().ToLower();

            return _identityContext.Users.FirstOrDefaultAsync(t => t.Email.ToLower() == email && !t.Delete && t.Status == Core.Enum.UserStatusEnum.Active);
        }
    }   
}
