using _2025.Services.AuthAPI.Core.Enum;

namespace _2025.Services.AuthAPI.DTO
{
    public class UpdateUserDTO
    {
        public int Id { get; set; }
        public string UserName { get; set; }
        public string Email { get; set; }
        public string FullName { get; set; }
        public string Title { get; set; }
        public UserStatusEnum Status { get; set; }
        public RoleEnum Role { get; set; }
    }
}
