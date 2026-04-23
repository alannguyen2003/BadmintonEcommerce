using BadmintonEcommerce.Domain.Entities.Catalog;

namespace BadmintonEcommerce.Application.Abstraction.Repositories;

public interface IProductRepository
{
    public Task<List<Product>> GetAllProducts();
    public Task<Product?> GetProduct(Guid productId);
    public Task AddProduct(Product product);
    public Task UpdateProduct(Product product);
    public Task DeleteProduct(Guid productId);
}