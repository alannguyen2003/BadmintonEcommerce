using BadmintonEcommerce.Application.Abstraction.Messaging;
using BadmintonEcommerce.Contracts.API.Presentation.Client.Category;

namespace BadmintonEcommerce.Application.Features.Client.GetCategories;

public sealed record GetCategoriesQuery() : IQuery<List<CategoryResponse>>;