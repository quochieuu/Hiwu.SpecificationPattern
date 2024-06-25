using Hiwu.SpecificationPattern.Abstractions;

namespace Hiwu.SpecificationPattern.Generic
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
