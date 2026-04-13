using Products.Infrastructure.Contracts.Common;
using Products.Infrastructure.Contracts.Repositories;
using Products.Infrastructure.Implementation.Context;
using Microsoft.EntityFrameworkCore.Storage;

namespace Products.Infrastructure.Implementation.Common;

public class UnitOfWork(
    ProductsDbContext dbContext,
    IProductRepository productRepository) : IUnitOfWork
{
    private readonly ProductsDbContext _dbContext = dbContext;
    private IDbContextTransaction? _transaction;

    public IProductRepository Products { get; } = productRepository;

    public async Task<int> SaveChangesAsync()
        => await _dbContext.SaveChangesAsync();

    public async Task BeginTransactionAsync()
        => _transaction = await _dbContext.Database.BeginTransactionAsync();

    public async Task CommitTransactionAsync()
    {
        try
        {
            await SaveChangesAsync();
            await _transaction!.CommitAsync();
        }
        catch
        {
            await RollbackTransactionAsync();
            throw;
        }
        finally
        {
            _transaction?.Dispose();
            _transaction = null;
        }
    }

    public async Task RollbackTransactionAsync()
    {
        if (_transaction != null)
        {
            await _transaction.RollbackAsync();
            _transaction.Dispose();
            _transaction = null;
        }
    }

    public void Dispose()
    {
        _transaction?.Dispose();
        _dbContext.Dispose();
    }
}
