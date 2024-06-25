using AutoFilterer.Extensions;
using AutoFilterer.Types;
using Hiwu.SpecificationPattern.Abstractions;
using Microsoft.EntityFrameworkCore;
using System.Globalization;
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

        #region Complete / Save changes
        public void Complete()
        {
            _context.SaveChanges();
        }

        public async Task CompleteAsync(CancellationToken cancellationToken = default)
        {
            await _context.SaveChangesAsync(cancellationToken).ConfigureAwait(false);
        }
        #endregion

        #region Hard delete
        public void HardDelete<TEntity>(TEntity entity) where TEntity : class
        {
            _context.Set<TEntity>().Remove(entity);
        }

        public void HardDelete<TEntity>(object id) where TEntity : class
        {
            var entity = _context.Set<TEntity>().Find(id);
            _context.Set<TEntity>().Remove(entity);
        }

        public void HardDelete<TEntity, TPrimaryKey>(TEntity entity) where TEntity : EasyBaseEntity<TPrimaryKey>
        {
            _context.Set<TEntity>().Remove(entity);
        }

        public void HardDelete<TEntity, TPrimaryKey>(TPrimaryKey id) where TEntity : EasyBaseEntity<TPrimaryKey>
        {
            var entity = _context.Set<TEntity>().FirstOrDefault(GenerateExpression<TEntity>(id));
            _context.Set<TEntity>().Remove(entity);
        }

        public Task HardDeleteAsync<TEntity>(TEntity entity, CancellationToken cancellationToken = default) where TEntity : class
        {
            _context.Set<TEntity>().Remove(entity);
            return Task.CompletedTask;
        }

        public async Task HardDeleteAsync<TEntity>(object id, CancellationToken cancellationToken = default) where TEntity : class
        {
            var entity = await _context.Set<TEntity>().FirstOrDefaultAsync(GenerateExpression<TEntity>(id), cancellationToken).ConfigureAwait(false);
            _context.Set<TEntity>().Remove(entity);
        }

        public Task HardDeleteAsync<TEntity, TPrimaryKey>(TEntity entity, CancellationToken cancellationToken = default) where TEntity : EasyBaseEntity<TPrimaryKey>
        {
            _context.Set<TEntity>().Remove(entity);
            return Task.CompletedTask;
        }

        public async Task HardDeleteAsync<TEntity, TPrimaryKey>(TPrimaryKey id, CancellationToken cancellationToken = default) where TEntity : EasyBaseEntity<TPrimaryKey>
        {
            var entity = await _context.Set<TEntity>().FirstOrDefaultAsync(GenerateExpression<TEntity>(id), cancellationToken).ConfigureAwait(false);
            _context.Set<TEntity>().Remove(entity);
        }
        #endregion

        #region Replace 
        public TEntity Replace<TEntity>(TEntity entity) where TEntity : class
        {
            _context.Entry(entity).State = EntityState.Modified;
            return entity;
        }

        public TEntity Replace<TEntity, TPrimaryKey>(TEntity entity) where TEntity : EasyBaseEntity<TPrimaryKey>
        {
            entity.ModificationDate = DateTime.UtcNow;
            _context.Entry(entity).State = EntityState.Modified;
            return entity;
        }

        public Task<TEntity> ReplaceAsync<TEntity>(TEntity entity, CancellationToken cancellationToken = default) where TEntity : class
        {
            _context.Entry(entity).State = EntityState.Modified;
            return Task.FromResult(entity);
        }

        public Task<TEntity> ReplaceAsync<TEntity, TPrimaryKey>(TEntity entity, CancellationToken cancellationToken = default) where TEntity : EasyBaseEntity<TPrimaryKey>
        {
            entity.ModificationDate = DateTime.UtcNow;
            _context.Entry(entity).State = EntityState.Modified;
            return Task.FromResult(entity);
        }
        #endregion

        private Expression<Func<TEntity, bool>> GenerateExpression<TEntity>(object id)
        {
            var type = _context.Model.FindEntityType(typeof(TEntity));
            string pk = type.FindPrimaryKey().Properties.Select(s => s.Name).FirstOrDefault();
            Type pkType = type.FindPrimaryKey().Properties.Select(p => p.ClrType).FirstOrDefault();

            object value = Convert.ChangeType(id, pkType, CultureInfo.InvariantCulture);

            ParameterExpression pe = Expression.Parameter(typeof(TEntity), "entity");
            MemberExpression me = Expression.Property(pe, pk);
            ConstantExpression constant = Expression.Constant(value, pkType);
            BinaryExpression body = Expression.Equal(me, constant);
            Expression<Func<TEntity, bool>> expression = Expression.Lambda<Func<TEntity, bool>>(body, new[] { pe });

            return expression;
        }
    }
}
