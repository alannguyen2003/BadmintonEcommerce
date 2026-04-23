namespace BadmintonEcommerce.Infrastructure.Abstractions;

public static class Message
{
    public static class Authentication
    {
        public static class ClaimsPrincipalMessage
        {
            public const string UserIdIsNotAvailable = "UserId from the token is not available!";
        }
    }

    
}