namespace _2025.Web.TaskManager.DTO.User
{
    public class LogonDTO
    {
        public string Email { get; set; }
        public string Password { get; set; }

        public bool IsSignUp { get; set; }

        public string UserName { get; set; }

        public string ConfirmPassword { get; set; }
    }
}
