using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace api_completa_mongodb_net_6_0.Domain.Entities
{
    public class Shift
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; } = null!;
        public string Title { get; set; } = null!;
        public DateTime Start { get; set; }
        public DateTime End { get; set; }
    }
}