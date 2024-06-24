using AutoFilterer.Extensions;
using AutoFilterer.Types;
using Hiwu.SpecificationPattern.Abstractions;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

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

        #region Any
        public bool Any<TEntity>(Expression<Func<TEntity, bool>> anyExpression) where TEntity : class
        {
            return _context.Set<TEntity>().Any(anyExpression);
        }

        public async Task<bool> AnyAsync<TEntity>(Expression<Func<TEntity, bool>> anyExpression, CancellationToken cancellationToken = default) where TEntity : class
        {
            bool result = await _context.Set<TEntity>().AnyAsync(anyExpression, cancellationToken).ConfigureAwait(false);
            return result;
        }
        #endregion

        #region Count
        public int Count<TEntity>() where TEntity : class
        {
            return _context.Set<TEntity>().Count();
        }

        public int Count<TEntity>(Expression<Func<TEntity, bool>> whereExpression) where TEntity : class
        {
            return _context.Set<TEntity>().Where(whereExpression).Count();
        }

        public async Task<int> Count<TEntity>(Expression<Func<TEntity, bool>> whereExpression, CancellationToken cancellationToken = default) where TEntity : class
        {
            int count = await _context.Set<TEntity>().Where(whereExpression).CountAsync(cancellationToken).ConfigureAwait(false);
            return count;
        }

        public int Count<TEntity, TFilter>(TFilter filter)
            where TEntity : class
            where TFilter : FilterBase
        {
            return _context.Set<TEntity>().ApplyFilter(filter).Count();
        }

        public async Task<int> CountAsync<TEntity>(CancellationToken cancellationToken = default) where TEntity : class
        {
            int count = await _context.Set<TEntity>().CountAsync(cancellationToken).ConfigureAwait(false);
            return count;
        }

        public async Task<int> CountAsync<TEntity, TFilter>(TFilter filter, CancellationToken cancellationToken = default)
            where TEntity : class
            where TFilter : FilterBase
        {
            int count = await _context.Set<TEntity>().ApplyFilter(filter).CountAsync(cancellationToken).ConfigureAwait(false);
            return count;
        }
        #endregion
    }
}
