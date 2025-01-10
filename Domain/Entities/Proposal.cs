using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace SagaAserhi.Domain.Entities
{
    [BsonIgnoreExtraElements]
    public class Proposal
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; } = string.Empty;
        public string Title { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public decimal Amount { get; set; }
        public string Status { get; set; } = string.Empty;
        public DateTime CreationDate { get; set; }
        public string PotentialClientId { get; set; } = string.Empty;

        [BsonElement("companyBusinessName")]
        public string CompanyBusinessName { get; set; } = string.Empty;

        [BsonElement("siteId")]
        public string? SiteId { get; set; }

        [BsonElement("hasSite")]
        public bool HasSite { get; set; } = false;

        [BsonElement("lastModified")]
        public DateTime LastModified { get; set; } = DateTime.UtcNow;
    }
}