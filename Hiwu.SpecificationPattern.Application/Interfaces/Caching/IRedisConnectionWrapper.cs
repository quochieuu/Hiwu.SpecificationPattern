using Hiwu.SpecificationPattern.Application.Enums;
using StackExchange.Redis;
using System.Net;

namespace Hiwu.SpecificationPattern.Application.Interfaces.Caching
{
    public interface IRedisConnectionWrapper : IDisposable
    {
        IDatabase GetDatabase(int db);

        IServer GetServer(EndPoint endPoint);

        EndPoint[] GetEndPoints();

        void FlushDatabase(RedisDatabaseNumber db);
    }
}
