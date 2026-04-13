using Products.Infrastructure.Contracts.Entities;
using Products.Infrastructure.Contracts.Repositories;
using Products.Infrastructure.Implementation.Context;
using Microsoft.Data.SqlClient;

namespace Products.Infrastructure.Implementation.Repositories;

public class ProductRepository(ProductsDbContext dbContext)
    : BaseRepository<ProductsDbContext, Product>(dbContext), IProductRepository
{
    public async Task<IEnumerable<Product>> GetAllViaSPAsync()
        => await ExecuteStoredProcedureQueryAsync("EXEC sp_GetAllProducts");

    public async Task<Product?> GetByIdViaSPAsync(int id)
    {
        var param = new SqlParameter("@Id", id);
        var results = await ExecuteStoredProcedureQueryAsync("EXEC sp_GetProductById @Id", param);
        return results.FirstOrDefault();
    }

    public async Task CreateViaSPAsync(string name, string? description, decimal price)
        => await ExecuteStoredProcedureAsync(
            "EXEC sp_CreateProduct @Name, @Description, @Price",
            new SqlParameter("@Name", name),
            new SqlParameter("@Description", (object?)description ?? DBNull.Value),
            new SqlParameter("@Price", price));

    public async Task UpdateViaSPAsync(int id, string name, string? description, decimal price)
        => await ExecuteStoredProcedureAsync(
            "EXEC sp_UpdateProduct @Id, @Name, @Description, @Price",
            new SqlParameter("@Id", id),
            new SqlParameter("@Name", name),
            new SqlParameter("@Description", (object?)description ?? DBNull.Value),
            new SqlParameter("@Price", price));

    public async Task DeleteViaSPAsync(int id)
        => await ExecuteStoredProcedureAsync(
            "EXEC sp_DeleteProduct @Id",
            new SqlParameter("@Id", id));
}
