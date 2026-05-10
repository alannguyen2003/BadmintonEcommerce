namespace BadmintonEcommerce.Tests.Helpers.Constants;

public static class Authentication
{
    public static class Login
    {
        public const string EmailField = "Email";
        public const string PasswordField = "Password";

        public static class LoginValid
        {
            public const string Email = "admin@gmail.com";
            public const string Password = "12345678";
        }
    }

    public static class Register
    {
        public const string EmailField = "Email";
        public const string PasswordField = "Password";
        public const string UsernameField = "Username";
        public const string Fullname = "Fullname";
    }
}