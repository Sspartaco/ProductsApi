using Products.Infrastructure.Contracts.Repositories;

namespace Products.Infrastructure.Contracts.Common;

public interface IUnitOfWork : IDisposable
{
    IProductRepository Products { get; }

    Task<int> SaveChangesAsync();
    Task BeginTransactionAsync();
    Task CommitTransactionAsync();
    Task RollbackTransactionAsync();
}
