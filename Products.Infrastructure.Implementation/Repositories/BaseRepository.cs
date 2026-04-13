using Products.Infrastructure.Contracts.Repositories;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace Products.Infrastructure.Implementation.Repositories;

public abstract class BaseRepository<TContext, T>(TContext dbContext) : IBaseRepository<T>
    where TContext : DbContext
    where T : class
{
    protected readonly TContext _dbContext = dbContext;
    protected readonly DbSet<T> _entitySet = dbContext.Set<T>();

    public virtual async Task<IEnumerable<T>> GetAllAsync()
        => await _entitySet.AsNoTracking().ToListAsync();

    public virtual async Task<T?> GetByIdAsync(object id)
        => await _entitySet.FindAsync(id);

    public virtual async Task<T?> GetFirstOrDefaultAsync(Expression<Func<T, bool>> predicate)
        => await _entitySet.AsNoTracking().FirstOrDefaultAsync(predicate);

    public virtual async Task<IEnumerable<T>> GetWhereAsync(Expression<Func<T, bool>> predicate)
        => await _entitySet.AsNoTracking().Where(predicate).ToListAsync();

    public virtual async Task AddAsync(T entity)
        => await _entitySet.AddAsync(entity);

    public virtual void Update(T entity)
        => _entitySet.Update(entity);

    public virtual void Remove(T entity)
        => _entitySet.Remove(entity);

    public virtual async Task<bool> AnyAsync(Expression<Func<T, bool>> predicate)
        => await _entitySet.AnyAsync(predicate);

    public virtual async Task<int> SaveChangesAsync()
        => await _dbContext.SaveChangesAsync();

    protected async Task<IEnumerable<T>> ExecuteStoredProcedureQueryAsync(string sql, params SqlParameter[] parameters)
        => await _entitySet
            .FromSqlRaw(sql, parameters)
            .AsNoTracking()
            .ToListAsync();

    protected async Task ExecuteStoredProcedureAsync(string sql, params SqlParameter[] parameters)
        => await _dbContext.Database.ExecuteSqlRawAsync(sql, parameters);
}
