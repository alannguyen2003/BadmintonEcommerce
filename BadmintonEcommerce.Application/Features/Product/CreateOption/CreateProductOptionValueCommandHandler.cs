using BadmintonEcommerce.Application.Abstraction.Messaging;
using BadmintonEcommerce.Application.Abstraction.Repositories;
using BadmintonEcommerce.Domain.Entities.Catalog;
using BadmintonEcommerce.Domain.Enums;
using BadmintonEcommerce.Domain.Errors;
using BadmintonEcommerce.Mapper.Abstractions;
using SharedKernel.Patterns;
using SharedKernel.Services;

namespace BadmintonEcommerce.Application.Features.Product.CreateOption;

public class CreateProductOptionValueCommandHandler(
    IMapper mapper,
    IProductRepository productRepository,
    IProductOptionRepository productOptionRepository,
    IProductOptionValueRepository productOptionValueRepository,
    IDateTimeProvider dateTimeProvider) : ICommandHandler<CreateProductOptionValueCommand>
{
    public async Task<Result> Handle(CreateProductOptionValueCommand command, CancellationToken cancellationToken)
    {
        //check if product exists
        Domain.Entities.Catalog.Product product = productRepository.GetById(command.ProductId);

        if (product == null)
            return Result.Failure(ProductError.NotFound(command.ProductId));

        ProductOption productOption = new ProductOption()
        {
            ProductId = product.Id,
            DataType = OptionValueDataType.String,
            OptionName = command.OptionName,
            OptionValues = new List<ProductOptionValue>(),
            CreatedOnUtc = dateTimeProvider.UtcNow
        };
        foreach (var item in command.OptionValues)
        {
            productOption.OptionValues.Add(new ProductOptionValue()
            {
                Value = item,
                CreatedOnUtc = dateTimeProvider.UtcNow
            });
        }
        productOptionRepository.Insert(productOption);
        await productOptionRepository.SaveChangesAsync();
        return Result.Success();
    }
}