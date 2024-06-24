using AutoFilterer.Types;
using System.Linq.Expressions;

namespace Hiwu.SpecificationPattern.Abstractions
{
    public interface IRepository
    {
        TEntity Add<TEntity>(TEntity entity) where TEntity : class;
        Task<TEntity> AddAsync<TEntity>(TEntity entity, CancellationToken cancellationToken = default) where TEntity : class;
        TEntity Add<TEntity, TPrimaryKey>(TEntity entity) where TEntity : EasyBaseEntity<TPrimaryKey>;
        Task<TEntity> AddAsync<TEntity, TPrimaryKey>(TEntity entity, CancellationToken cancellationToken = default) where TEntity : EasyBaseEntity<TPrimaryKey>;
        IEnumerable<TEntity> AddRange<TEntity>(IEnumerable<TEntity> entities) where TEntity : class;
        Task<IEnumerable<TEntity>> AddRangeAsync<TEntity>(IEnumerable<TEntity> entities, CancellationToken cancellationToken = default) where TEntity : class;
        IEnumerable<TEntity> AddRange<TEntity, TPrimaryKey>(IEnumerable<TEntity> entities) where TEntity : EasyBaseEntity<TPrimaryKey>;
        Task<IEnumerable<TEntity>> AddRangeAsync<TEntity, TPrimaryKey>(IEnumerable<TEntity> entites, CancellationToken cancellationToken = default) where TEntity : EasyBaseEntity<TPrimaryKey>;

        bool Any<TEntity>(Expression<Func<TEntity, bool>> anyExpression) where TEntity : class;
        Task<bool> AnyAsync<TEntity>(Expression<Func<TEntity, bool>> anyExpression, CancellationToken cancellationToken = default) where TEntity : class;

        int Count<TEntity>() where TEntity : class;
        int Count<TEntity>(Expression<Func<TEntity, bool>> whereExpression) where TEntity : class;
        Task<int> Count<TEntity>(Expression<Func<TEntity, bool>> whereExpression, CancellationToken cancellationToken = default) where TEntity : class;
        int Count<TEntity, TFilter>(TFilter filter) where TEntity : class where TFilter : FilterBase;
        Task<int> CountAsync<TEntity>(CancellationToken cancellationToken = default) where TEntity : class;
        Task<int> CountAsync<TEntity, TFilter>(TFilter filter, CancellationToken cancellationToken = default) where TEntity : class where TFilter : FilterBase;
    }
}
