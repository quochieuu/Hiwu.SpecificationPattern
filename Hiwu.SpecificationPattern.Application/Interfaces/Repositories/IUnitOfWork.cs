namespace Hiwu.SpecificationPattern.Application.Interfaces.Repositories
{
    /// <summary>
    /// Abstraction of Unit Of Work pattern
    /// </summary>
    public interface IUnitOfWork
    {
        IRepository Repository { get; }
    }
}
