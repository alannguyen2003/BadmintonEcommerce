namespace BadmintonEcommerce.Domain.Abstraction.Errors;

public static class AuthenticationErrorCommand
{
    public static class EmailNotUnique
    {
        public const string Code = "Authentication.EmailNotUnique";
        public const string Description = "The email to register is not unique!";
    }
    
    public static class EmailNotExists
    {
        public const string Code = "Authentication.EmailNotExists";
        public const string Description = "The email address not exists. Id: ";
    }

    public static class EmailOrPasswordIsWrong
    {
        public const string Code = "Authentication.EmailOrPasswordIsWrong";
        public const string Description = "The email or password is wrong.";
    }
}