using System.Data;
using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using Microsoft.Extensions.Logging;
using Ozbul.Application.Portal.Repository.Interfaces;

namespace Ozbul.Application.Portal.Repository;

public class UnitOfWork<TContext> : IUnitOfWork where TContext : DbContext
{
    private bool _disposed;
    private readonly TContext _context;
    private readonly ILogger<UnitOfWork<TContext>> _logger;

    public UnitOfWork(TContext dbContext, ILogger<UnitOfWork<TContext>> logger)
    {
        _context = dbContext;
        _logger = logger;
    }

    public virtual List<T> Get<T>() where T : class, IBaseEntity => Query<T>().ToList();

    public virtual List<T> Get<T>(Expression<Func<T, bool>> expression) where T : class, IBaseEntity => Query(expression).ToList();

    public virtual void Add<T>(T entity) where T : class, IBaseEntity
    {        
        _context.Add(entity);
    }

    public Task<List<T>> GetAsync<T>() where T : class, IBaseEntity => Query<T>().ToListAsync();

    public virtual void Update<T>(T entity) where T : class, IBaseEntity => _context.Update(entity);

    public virtual void Delete<T>(T entity) where T : class, IBaseEntity => _context.Remove(entity);

    public Task<List<T>> GetAsync<T>(Expression<Func<T, bool>> expression) where T : class, IBaseEntity =>
        Query(expression).ToListAsync();

    public virtual void SoftDelete<T>(T entity) where T : class, IBaseEntity
    {
        entity.IsDeleted = true;
        Update(entity);
    }

    public virtual T? FirstOrDefault<T>(Expression<Func<T, bool>> expression) where T : class, IBaseEntity =>
        Query(expression).FirstOrDefault();

    public virtual Task<T?> FirstOrDefaultAsync<T>(Expression<Func<T, bool>> expression)
        where T : class, IBaseEntity => Query(expression).FirstOrDefaultAsync();

    public virtual IQueryable<T> Query<T>(Expression<Func<T, bool>> expression) where T : class, IBaseEntity
    {
        return _context.Set<T>().Where(e => e.IsDeleted == false).Where(expression);
    }

    public virtual IQueryable<T> Query<T>() where T : class, IBaseEntity => Query<T>(e => true);

    public Task<bool> AnyAsync<T>(Expression<Func<T, bool>> expression) where T : class, IBaseEntity =>
        Query<T>().AnyAsync(expression);

    public void Save() => _context.SaveChanges();

    public virtual Task SaveAsync() => _context.SaveChangesAsync();

    public virtual Task ExecuteInTransactionAsync(Func<IUnitOfWork, Task> action) => ExecuteInTransactionAsync(action, IsolationLevel.ReadCommitted);

    public virtual Task ExecuteInTransactionAsync(Func<IUnitOfWork, Task> action, IsolationLevel isolationLevel)
    {
        IExecutionStrategy strategy = _context.Database.CreateExecutionStrategy();

        return strategy.ExecuteAsync(async () =>
        {
            using IDbContextTransaction transaction = await _context.Database.BeginTransactionAsync(CancellationToken.None);

            try
            {
                // Execute the action itself
                await action(this);

                // save changes.
                await SaveAsync();

                transaction.Commit();
            }
            catch (Exception e)
            {
                _logger.LogError(e, "Transaction failed while creating an execution strategy.");
                _logger.LogWarning("Trying to rollback...");
                transaction.Rollback();
                _logger.LogWarning("Rollback successfully!");
                throw;
            }
        });
    }

    public Task AddRangeAsync<T>(IEnumerable<T> entities) where T : class, IBaseEntity
    {
        return _context.AddRangeAsync(entities);
    }

    public void Dispose()
    {
        Dispose(true);
        GC.SuppressFinalize(this);
    }

    protected virtual void Dispose(bool disposing)
    {
        if (!_disposed && disposing)
        {
            _disposed = true;
            _context.Dispose();
        }
    }
}