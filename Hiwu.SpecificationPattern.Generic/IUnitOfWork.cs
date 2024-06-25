using Hiwu.SpecificationPattern.Abstractions;

namespace Hiwu.SpecificationPattern.Generic
{
    /// <summary>
    /// Abstraction of Unit Of Work pattern
    /// </summary>
    public interface IUnitOfWork
    {
        IRepository Repository { get; }
    }
}
