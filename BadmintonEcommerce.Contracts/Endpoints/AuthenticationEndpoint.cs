namespace BadmintonEcommerce.Contracts.Endpoints;

public class AuthenticationEndpoint
{
    public static readonly string EndpointUrl = "/auth";

    public static readonly string Login = $"{EndpointUrl}/login";

    public static readonly string Register = $"{EndpointUrl}/register";
}