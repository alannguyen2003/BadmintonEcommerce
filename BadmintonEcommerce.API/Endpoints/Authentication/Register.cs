using BadmintonEcommerce.API.Extensions;
using BadmintonEcommerce.API.Infrastructure;
using BadmintonEcommerce.Application.Abstraction.Messaging;
using BadmintonEcommerce.Application.Features.Authentication.Register;
using BadmintonEcommerce.Contracts.API.Presentation.Authentication;
using BadmintonEcommerce.Contracts.API.Presentation.Authentication.Request;
using BadmintonEcommerce.Mapper.Abstractions;
using Microsoft.AspNetCore.Mvc;
using SharedKernel.Patterns;

namespace BadmintonEcommerce.API.Endpoints.Authentication;

public class Register : IEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapPost("auth/register", async (
            [FromBody] RegisterRequest request,
            [FromServices] ICommandHandler<RegisterCommand, string> handler,
            [FromServices] IMapper mapper,
            CancellationToken cancellationToken) =>
        {
            Result<string> result = await handler.Handle(mapper.Map<RegisterCommand>(request), cancellationToken);
            
            return result.Match(Results.Ok, CustomResult.Problem);
        })
        .WithTags(Tags.Authentication);
    }
}