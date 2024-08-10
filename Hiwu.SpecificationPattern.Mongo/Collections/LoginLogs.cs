using Hiwu.SpecificationPattern.Mongo.Attributes;
using MongoDB.Bson.Serialization.Attributes;

namespace Hiwu.SpecificationPattern.Mongo.Collections
{
    [BsonCollection("loginlogs")]
    [BsonIgnoreExtraElements]
    public class LoginLog : MongoBaseDocument
    {
        public string UserEmail { get; set; }
        public DateTime LoginTime { get; set; }
    }
}
