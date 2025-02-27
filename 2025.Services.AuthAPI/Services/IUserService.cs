using _2025.Services.AuthAPI.Core.Entities;
using _2025.Services.AuthAPI.Core.Helper;
using System.Security.Cryptography;
using System.Text;

namespace _2025.Services.AuthAPI.Services
{
    public interface IUserService
    {
        User SetPassword(User user, string password);
    }

    public class UserService : IUserService
    {
        public UserService() 
        {
                    
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

        private byte[] CombineSaltAndPassword(byte[] saltBytes, string password)
        {
            var passwordBytes = Encoding.Unicode.GetBytes(password);
            return saltBytes.Concat(passwordBytes).ToArray();
        }
    }   
}
