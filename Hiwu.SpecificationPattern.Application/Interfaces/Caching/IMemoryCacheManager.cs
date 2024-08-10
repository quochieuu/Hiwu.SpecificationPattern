namespace Hiwu.SpecificationPattern.Application.Interfaces.Caching
{
    public interface IMemoryCacheManager
    {
        Task<string> GetAsync(string cacheKey);
        Task SetAsync(string key, object data, int cacheTime);
    }
}
