using LRQACodingKata.Core.Repositories;
using Microsoft.EntityFrameworkCore;

namespace LRQACodingKata.Infrastructure.Data.Repositories
{
    public class EfCoreRepository<TEntity, TKey>(DbContext context) : IRepository<TEntity> where TEntity : class
    {
        public Task<IEnumerable<TEntity>> GetAllAsync(CancellationToken cancellationToken = default)
        {
            throw new NotImplementedException();
        }

        public Task<TEntity?> GetByIdAsync(int id, CancellationToken cancellationToken = default)
        {
            throw new NotImplementedException();
        }

        public Task<TEntity> AddAsync(TEntity entity, CancellationToken cancellationToken = default)
        {
            throw new NotImplementedException();
        }

        public void Update(TEntity entity)
        {
            throw new NotImplementedException();
        }
        public void Delete(TEntity entity)
        {
            throw new NotImplementedException();
        }
    }
}