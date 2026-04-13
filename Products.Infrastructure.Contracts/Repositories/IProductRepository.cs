using Products.Infrastructure.Contracts.Entities;

namespace Products.Infrastructure.Contracts.Repositories;

public interface IProductRepository : IBaseRepository<Product>
{
    Task<IEnumerable<Product>> GetAllViaSPAsync();
    Task<Product?> GetByIdViaSPAsync(int id);
    Task CreateViaSPAsync(string name, string? description, decimal price);
    Task UpdateViaSPAsync(int id, string name, string? description, decimal price);
    Task DeleteViaSPAsync(int id);
}
