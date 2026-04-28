using BadmintonEcommerce.Application.Abstraction.Messaging;
using BadmintonEcommerce.Application.Abstraction.Repositories;
using BadmintonEcommerce.Contracts.API.Presentation.ProductCategory.Responses;
using BadmintonEcommerce.Domain.Errors;
using BadmintonEcommerce.Mapper.Abstractions;
using SharedKernel.Patterns;

namespace BadmintonEcommerce.Application.Features.ProductCategory.GetById;

public class GetProductCategoryByIdQueryHandler(IProductCategoryRepository productCategoryRepository,
    IMapper mapper)   
    : IQueryHandler<GetProductCategoryByIdQuery, ProductCategoryByIdResponse>
{
    public async Task<Result<ProductCategoryByIdResponse>> Handle(GetProductCategoryByIdQuery query, CancellationToken cancellationToken)
    {
        Domain.Entities.Catalog.ProductCategory category = productCategoryRepository.GetById(query.ProductCategoryId);
        if (category == null)
            return Result.Failure<ProductCategoryByIdResponse>(ProductCategoryError.NotFound(query.ProductCategoryId));

        ProductCategoryByIdResponse response = mapper.Map<ProductCategoryByIdResponse>(category);

        return response;
    }
}