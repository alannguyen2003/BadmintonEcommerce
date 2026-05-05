using BadmintonEcommerce.Application.Abstraction.Messaging;
using BadmintonEcommerce.Application.Abstraction.Repositories;
using BadmintonEcommerce.Contracts.API.Presentation.Client.Category;
using BadmintonEcommerce.Mapper.Abstractions;
using SharedKernel.Patterns;

namespace BadmintonEcommerce.Application.Features.Client.GetCategories;

public class GetCategoriesQueryHandler(
    IProductCategoryRepository productCategoryRepository,
    IMapper mapper
    ) : IQueryHandler<GetCategoriesQuery, List<CategoryResponse>>
{
    public async Task<Result<List<CategoryResponse>>> Handle(GetCategoriesQuery query, CancellationToken cancellationToken)
    {
        IEnumerable<Domain.Entities.Catalog.ProductCategory> categories = await productCategoryRepository
            .Get(filter: null,
                orderBy: null,
                "ChildCategories,ParentCategory");
        List<CategoryResponse> responses = this.CategoryRecursion(categories.ToList(), null);
        return responses;
    }

    public List<CategoryResponse> CategoryRecursion(List<Domain.Entities.Catalog.ProductCategory>  categories,
        Guid? parentCategoryId)
    {
        return categories
            .Where(item => item.ParentCategoryId == parentCategoryId)
            .Select(item => new CategoryResponse()
            {
                Id = item.Id,
                Name = item.CategoryName,
                ChildCategories = CategoryRecursion(categories, item.Id)
            }).ToList();
    }
}