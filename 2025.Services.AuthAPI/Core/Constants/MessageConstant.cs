namespace _2025.Services.AuthAPI.Core.Constants
{
    public static class MessageConstant // static vi class nay se k thay doi trong suot vong doi chuong trinh
    {
        public static class CommonMessage {
            public const string NOT_ALLOWED = "You are not allowed!";
            public const string NOT_AUTHEN = "Not authen!";
            public const string NOT_FOUND = "Not found!";
            public const string ERROR_HAPPEND = "An error occurred, please contact the admin or try again later!";
            public const string MISSING_PARAM = "Missing input parameters. Please check again!"; //Thieu tham so dau vao
        }

        public static class UserMessage
        {
            public const string LOGIN_FAIL = "Login failed! Please check your email or password again!";
            public const string CONFIRM_PASSWORD_NOT_CORRECT = "Confirm password is not correct!";
            public const string USERNAME_REQUIRED = "Username is required!";
            public const string EMAIL_REQUIRED = "Email is required!";
            public const string PASSWORD_REQUIRED = "Password is required!";
            public const string USERNAME_EXIST = "Username already exists!";
            public const string EMAIL_EXIST = "Email already exists!";
            public const string INVALIDID_USERNAME = "Username is invalid: Contain only letters (a-z, A-Z), numbers (0-9) and underscore (_). Length from 3 to 30 characters. Not start or end with underscore (_).";
            public const string INVALIDID_EMAIL = "Email is invalid: Start with one or more letters, numbers or special characters like . and _. Have a @ to separate the login name and domain name. The domain name must contain at least one dot (.). The domain extension can be 2 to 4 characters (eg com, net, info, ...).";
            public const string INVALIDID_PASSWORD = "Password is invalid: Length from 8 to 20 characters. Contain at least one lowercase letter, one uppercase letter, one number and one special character.";
        }
    }
}
