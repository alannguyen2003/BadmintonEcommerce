using BadmintonEcommerce.API.Extensions;
using BadmintonEcommerce.API.Infrastructure;
using BadmintonEcommerce.Application.Abstraction.Messaging;
using BadmintonEcommerce.Application.Features.Authentication.Login;
using BadmintonEcommerce.Contracts.API.Presentation.Authentication.Request;
using BadmintonEcommerce.Contracts.API.Presentation.Authentication.Response;
using BadmintonEcommerce.Mapper.Abstractions;
using Microsoft.AspNetCore.Mvc;
using SharedKernel.Patterns;

namespace BadmintonEcommerce.API.Endpoints.Authentication;

public class Login : IEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapPost("auth/login", async (
            [FromBody] SignInRequest request,
            [FromServices] ICommandHandler<LoginCommand, string> handler,
            [FromServices] IMapper mapper,
            CancellationToken cancellationToken) =>
        {
            Result<string> result = await handler.Handle(
                mapper.Map<LoginCommand>(request), cancellationToken);

            return result.Match(Results.Ok, CustomResult.Problem);
        })
        .WithTags(Tags.Authentication);
    }
}