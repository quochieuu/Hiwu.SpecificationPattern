using Hiwu.SpecificationPattern.Application.Interfaces.Repositories;

namespace Hiwu.SpecificationPattern.Persistence.Repositories
{
    /// <summary>
    /// Implementation of Unit of work pattern
    /// </summary>
    public class UnitOfWork : IUnitOfWork
    {
        public UnitOfWork(IRepository repository)
        {
            Repository = repository;
        }
        public IRepository Repository { get; }
    }
}
