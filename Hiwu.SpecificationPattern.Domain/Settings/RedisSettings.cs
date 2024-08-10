namespace Hiwu.SpecificationPattern.Domain.Settings
{
    public class RedisSettings
    {
        public string RedisDataProtectionKey { get; set; }
        public int CacheTime { get; set; }
        public string RedisConnectionString { get; set; }
        public int? RedisDatabaseId { get; set; }
    }
}
