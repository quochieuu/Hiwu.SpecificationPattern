using Hiwu.SpecificationPattern.Domain.Common;

namespace Hiwu.SpecificationPattern.Application.Interfaces.Repositories
{
    /// <summary>
    /// Abstraction of Unit Of Work pattern
    /// </summary>
    public interface IUnitOfWork : IDisposable
    {
        IGenericRepository<TEntity> Repository<TEntity>() where TEntity : class;
        Task<int> CompleteAsync();
    }
}
