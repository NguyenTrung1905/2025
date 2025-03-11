using _2025.Web.TaskManager.Core.Enum;

namespace _2025.Web.TaskManager.DTO.User
{
    public class AddUserDTO
    {
        public string UserName { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public string FullName { get; set; }
        public string Title { get; set; }
        public UserStatusEnum Status { get; set; }
        public RoleEnum Role { get; set; }
    }
}
