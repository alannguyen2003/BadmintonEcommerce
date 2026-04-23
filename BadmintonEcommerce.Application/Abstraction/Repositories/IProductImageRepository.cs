using BadmintonEcommerce.Domain.Entities.Catalog;

namespace BadmintonEcommerce.Application.Abstraction.Repositories;

public interface IProductImageRepository
{
    public Task<List<ProductImage>> GetProductImagesOfProduct(Guid productId);
    public Task<ProductImage> GetProductImage(Guid productImageId);
    public Task AddProductImage(ProductImage productImage);
    public Task UpdateProductImage(ProductImage productImage);
    public Task DeleteProductImage(ProductImage productImage);
}