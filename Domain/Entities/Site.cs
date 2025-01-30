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

        [BsonElement("name")]
        public string Name { get; set; } = string.Empty;

        [BsonElement("address")]
        public string Address { get; set; } = string.Empty;

        [BsonElement("city")]
        public string City { get; set; } = string.Empty;

        [BsonElement("department")]
        public string Department { get; set; } = string.Empty;

        [BsonElement("phone")]
        public string Phone { get; set; } = string.Empty;

        [BsonElement("proposalId")]
        [BsonRepresentation(BsonType.ObjectId)]
        public string ProposalId { get; set; } = string.Empty;

        [BsonElement("wastes")]
        public List<Waste> Wastes { get; set; } = new List<Waste>();

        [BsonElement("frequencies")]
        public Frequency Frequencies { get; set; } = new Frequency();

        [BsonElement("TotalPrice")]
        public decimal TotalPrice { get; set; }

        [BsonElement("createdAt")]
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    }

    public class Frequency 
    {

        [BsonElement("FrequencyOfTravel")]
        public string FrequencyOfTravel { get; set; } = string.Empty;

        [BsonElement("amount")]
        public decimal Amount { get; set; }
    }

    public class Waste
    {
        [BsonElement("type")]
        public string Type { get; set; } = string.Empty;

        [BsonElement("classification")]
        public string Classification { get; set; } = string.Empty;

        [BsonElement("treatment")]
        public string Treatment { get; set; } = string.Empty;

        [BsonElement("price")]
        public decimal Price { get; set; }
    }
}
