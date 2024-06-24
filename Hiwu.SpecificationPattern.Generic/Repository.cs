using Hiwu.SpecificationPattern.Abstractions;
using Microsoft.EntityFrameworkCore;

namespace Hiwu.SpecificationPattern.Generic
{
    /// <summary>
    /// This class contains implementations of repository functions
    /// </summary>
    internal sealed class Repository : IRepository
    {
        private readonly DbContext _context;

        public Repository(DbContext context)
        {
            this._context = context;
        }

        #region Add
        public TEntity Add<TEntity>(TEntity entity) where TEntity : class
        {
            _context.Set<TEntity>().Add(entity);
            return entity;
        }

        public TEntity Add<TEntity, TPrimaryKey>(TEntity entity) where TEntity : EasyBaseEntity<TPrimaryKey>
        {
            entity.CreationDate = DateTime.UtcNow;
            _context.Set<TEntity>().Add(entity);
            return entity;
        }

        public async Task<TEntity> AddAsync<TEntity>(TEntity entity, CancellationToken cancellationToken = default) where TEntity : class
        {
            await _context.Set<TEntity>().AddAsync(entity, cancellationToken).ConfigureAwait(false);
            return entity;
        }

        public async Task<TEntity> AddAsync<TEntity, TPrimaryKey>(TEntity entity, CancellationToken cancellationToken = default) where TEntity : EasyBaseEntity<TPrimaryKey>
        {
            entity.CreationDate = DateTime.UtcNow;
            await _context.Set<TEntity>().AddAsync(entity, cancellationToken).ConfigureAwait(false);
            return entity;
        }

        public IEnumerable<TEntity> AddRange<TEntity>(IEnumerable<TEntity> entities) where TEntity : class
        {
            _context.Set<TEntity>().AddRange(entities);
            return entities;
        }

        public IEnumerable<TEntity> AddRange<TEntity, TPrimaryKey>(IEnumerable<TEntity> entities) where TEntity : EasyBaseEntity<TPrimaryKey>
        {
            entities.ToList().ForEach(x => x.CreationDate = DateTime.UtcNow);
            _context.Set<TEntity>().AddRange(entities);
            return entities;
        }

        public async Task<IEnumerable<TEntity>> AddRangeAsync<TEntity>(IEnumerable<TEntity> entities, CancellationToken cancellationToken = default) where TEntity : class
        {
            await _context.Set<TEntity>().AddRangeAsync(entities, cancellationToken).ConfigureAwait(false);
            return entities;
        }

        public async Task<IEnumerable<TEntity>> AddRangeAsync<TEntity, TPrimaryKey>(IEnumerable<TEntity> entities, CancellationToken cancellationToken = default) where TEntity : EasyBaseEntity<TPrimaryKey>
        {
            entities.ToList().ForEach(x => x.CreationDate = DateTime.UtcNow);
            await _context.Set<TEntity>().AddRangeAsync(entities, cancellationToken).ConfigureAwait(false);
            return entities;
        }
        #endregion
    }
}
