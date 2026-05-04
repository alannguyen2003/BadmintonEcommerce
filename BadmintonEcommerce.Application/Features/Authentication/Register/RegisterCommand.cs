using BadmintonEcommerce.Application.Abstraction.Messaging;

namespace BadmintonEcommerce.Application.Features.Authentication.Register;

public class RegisterCommand(
    string Fullname,
    string Email,
    string Username,
    string Password) : ICommand<Guid>;