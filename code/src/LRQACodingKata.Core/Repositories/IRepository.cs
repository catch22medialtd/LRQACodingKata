using System.Linq.Expressions;

namespace LRQACodingKata.Core.Repositories
{
    public interface IRepository<TEntity> where TEntity : class
    {
        Task<TEntity?> GetByIdAsync(int id, CancellationToken cancellationToken = default);

        Task<IEnumerable<TResult>> GetAllProjectedAsync<TResult>(
            Expression<Func<TEntity, TResult>> selector,
            Func<IQueryable<TResult>,
            IOrderedQueryable<TResult>>? orderBy = null,
            CancellationToken cancellationToken = default);

        Task<TEntity> AddAsync(TEntity entity, CancellationToken cancellationToken = default);
        
        void Update(TEntity entity);
        
        void Delete(TEntity entity);
        
        Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
    }
}