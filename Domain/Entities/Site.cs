using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace SagaAserhi.Domain.Entities
{
    public class Site
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; } = ObjectId.GenerateNewId().ToString();
        public string Name { get; set; } = string.Empty;
        public string Address { get; set; } = string.Empty;
        public string City { get; set; } = string.Empty;
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public string PotentialClientId { get; set; } = ObjectId.GenerateNewId().ToString();

        [BsonElement("wastes")]
        public List<Waste> Wastes { get; set; } = new List<Waste>();
    }


    public class Waste
    {
        [BsonElement("type")]
        public string Type { get; set; } = string.Empty;

        [BsonElement("classification")]
        public string Classification { get; set; } = string.Empty;

        [BsonElement("treatment")]
        public string Treatment { get; set; } = string.Empty;

        [BsonElement("frequency")]
        public string Frequency { get; set; } = string.Empty;

        [BsonElement("price")]
        public decimal Price { get; set; }
    }


}
