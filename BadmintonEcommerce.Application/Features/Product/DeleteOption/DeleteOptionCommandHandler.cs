using System.Diagnostics.CodeAnalysis;
using BadmintonEcommerce.Application.Abstraction.Messaging;
using BadmintonEcommerce.Application.Abstraction.Repositories;
using SharedKernel.Patterns;

namespace BadmintonEcommerce.Application.Features.Product.DeleteOption;

[ExcludeFromCodeCoverage]
public class DeleteOptionCommandHandler(
    IProductOptionRepository productOptionRepository,
    IProductOptionValueRepository productOptionValueRepository) : ICommandHandler<DeleteOptionCommand>
{
    public async Task<Result> Handle(DeleteOptionCommand command, CancellationToken cancellationToken)
    {
        return Result.Success();
    }
}