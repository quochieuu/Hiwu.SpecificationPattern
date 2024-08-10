using Hiwu.SpecificationPattern.Application.Enums;
using Hiwu.SpecificationPattern.Application.Interfaces.Caching;
using Hiwu.SpecificationPattern.Domain.Settings;
using RedLockNet.SERedis;
using RedLockNet.SERedis.Configuration;
using StackExchange.Redis;
using System.Net;

namespace Hiwu.SpecificationPattern.Caching.Redis
{
    /// <summary>
    /// Manages the Redis connection and provides Redis-related operations, including distributed locking using RedLock.
    /// </summary>
    public class RedisConnectionWrapper : IRedisConnectionWrapper, IConnectionLocker
    {
        private readonly RedisSettings _redisSettings;
        private readonly object _connectionLock = new object();
        private volatile ConnectionMultiplexer _connection;
        private readonly Lazy<string> _connectionString;
        private volatile RedLockFactory _redisLockFactory;

        public RedisConnectionWrapper(RedisSettings redisSettings)
        {
            _redisSettings = redisSettings;
            _connectionString = new Lazy<string>(GetConnectionString);
            _redisLockFactory = CreateRedisLockFactory();
        }

        /// <summary>
        /// Retrieves the Redis connection string from the configuration.
        /// </summary>
        /// <returns></returns>
        protected string GetConnectionString()
        {
            return _redisSettings.RedisConnectionString;
        }

        /// <summary>
        /// Gets the current Redis connection, creating a new one if necessary.
        /// </summary>
        /// <returns></returns>
        protected ConnectionMultiplexer GetConnection()
        {
            if (_connection != null && _connection.IsConnected) return _connection;

            lock (_connectionLock)
            {
                if (_connection != null && _connection.IsConnected) return _connection;

                // Connection disconnected. Disposing connection...
                _connection?.Dispose();

                // Creating new instance of Redis Connection
                _connection = ConnectionMultiplexer.Connect(_connectionString.Value);
            }

            return _connection;
        }

        /// <summary>
        /// Creates a RedLockFactory instance to manage distributed locks.
        /// </summary>
        /// <returns></returns>
        protected RedLockFactory CreateRedisLockFactory()
        {
            // Get RedLock endpoints
            var configurationOptions = ConfigurationOptions.Parse(_connectionString.Value);
            var redLockEndPoints = GetEndPoints().Select(endPoint => new RedLockEndPoint
            {
                EndPoint = endPoint,
                Password = configurationOptions.Password,
                Ssl = configurationOptions.Ssl,
                RedisDatabase = configurationOptions.DefaultDatabase,
                ConfigCheckSeconds = configurationOptions.ConfigCheckSeconds,
                ConnectionTimeout = configurationOptions.ConnectTimeout,
                SyncTimeout = configurationOptions.SyncTimeout
            }).ToList();

            // Create RedLock factory to use RedLock distributed lock algorithm
            return RedLockFactory.Create(redLockEndPoints);
        }

        /// <summary>
        /// Retrieves a Redis database by its identifier.
        /// </summary>
        /// <param name="db"></param>
        /// <returns></returns>
        public IDatabase GetDatabase(int db)
        {
            return GetConnection().GetDatabase(db);
        }

        /// <summary>
        /// Retrieves a Redis server instance by its endpoint.
        /// </summary>
        /// <param name="endPoint"></param>
        /// <returns></returns>
        public IServer GetServer(EndPoint endPoint)
        {
            return GetConnection().GetServer(endPoint);
        }

        /// <summary>
        /// Retrieves all endpoints of the connected Redis servers.
        /// </summary>
        /// <returns></returns>
        public EndPoint[] GetEndPoints()
        {
            return GetConnection().GetEndPoints();
        }

        /// <summary>
        /// Flushes the specified Redis database on all connected servers.
        /// </summary>
        /// <param name="db"></param>
        public void FlushDatabase(RedisDatabaseNumber db)
        {
            var endPoints = GetEndPoints();

            foreach (var endPoint in endPoints)
            {
                GetServer(endPoint).FlushDatabase((int)db);
            }
        }

        /// <summary>
        /// Performs an action with a distributed lock.
        /// </summary>
        /// <param name="resource"></param>
        /// <param name="expirationTime"></param>
        /// <param name="action"></param>
        /// <returns></returns>
        public bool PerformActionWithLock(string resource, TimeSpan expirationTime, Action action)
        {
            // Use RedLock library
            using (var redisLock = _redisLockFactory.CreateLock(resource, expirationTime))
            {
                // Ensure that lock is acquired
                if (!redisLock.IsAcquired)
                    return false;

                // Perform action
                action();

                return true;
            }
        }

        /// <summary>
        /// Disposes of the resources used by the <see cref="RedisConnectionWrapper"/>.
        /// </summary>
        public void Dispose()
        {
            // Dispose ConnectionMultiplexer
            _connection?.Dispose();

            // Dispose RedLock factory
            _redisLockFactory?.Dispose();
        }
    }
}
