namespace Hiwu.SpecificationPattern.Application.Interfaces.Caching
{
    public interface IConnectionLocker
    {
        bool PerformActionWithLock(string resource, TimeSpan expirationTime, Action action);
    }
}
