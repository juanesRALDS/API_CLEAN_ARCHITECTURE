using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System;
using System.Collections.Generic;

namespace SagaAserhi.Domain.Entities
{

    public class ProposalStatus
    {
        [BsonElement("proposal")]
        public string Proposal { get; set; } = string.Empty;

        [BsonElement("sending")]
        public string Sending { get; set; } = string.Empty;

        [BsonElement("review")]
        public string Review { get; set; } = string.Empty;
    }

    public class ProposalHistory
    {
        [BsonElement("action")]
        public string Action { get; set; } = string.Empty;

        [BsonElement("date")]
        public DateTime Date { get; set; }

        [BsonElement("potentialClientId")]
        [BsonRepresentation(BsonType.ObjectId)]
        public string PotentialClientId { get; set; } = string.Empty;
    }



    public class Proposal
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; } = ObjectId.GenerateNewId().ToString();

        [BsonElement("clientId")]
        [BsonRepresentation(BsonType.ObjectId)]
        public string ClientId { get; set; } = string.Empty;

        [BsonElement("number")]
        public string Number { get; set; } = string.Empty;

        [BsonElement("status")]
        public ProposalStatus Status { get; set; } = new ProposalStatus();

        [BsonElement("sites")]
        public List<Site> Sites { get; set; } = new List<Site>();

        [BsonElement("history")]
        public List<ProposalHistory> History { get; set; } = new List<ProposalHistory>();

        [BsonElement("createdAt")]
        public DateTime CreatedAt { get; set; }

        [BsonElement("updatedAt")]
        public DateTime UpdatedAt { get; set; }

        [BsonIgnore]
        public virtual PotentialClient PotentialClient { get; set; } = new PotentialClient();
    }
}