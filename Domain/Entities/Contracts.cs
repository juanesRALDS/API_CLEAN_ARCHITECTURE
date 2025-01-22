using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System;
using System.Collections.Generic;

namespace SagaAserhi.Domain.Entities
{
    public class ContractDates
    {
        [BsonElement("start")]
        public DateTime Start { get; set; }

        [BsonElement("end")]
        public DateTime End { get; set; }
    }



    public class Annex
    {
        [BsonElement("AnenxId")]
        public string AnnexId { get; set; } = ObjectId.GenerateNewId().ToString();

        [BsonElement("name")]
        public string Title { get; set; } = string.Empty;

        [BsonElement("path")]
        public string Path { get; set; } = string.Empty;

        [BsonElement("uploadDate")]
        public DateTime UploadDate { get; set; }
    }

    public class Clause
    {
        [BsonElement("title")]
        public string Title { get; set; } = string.Empty;

        [BsonElement("content")]
        public string Content { get; set; } = string.Empty;
    }

    public class Documents
    {
        [BsonElement("annexes")]
        public List<Annex> Annexes { get; set; } = new List<Annex>();

        [BsonElement("clauses")]
        public List<Clause> Clauses { get; set; } = new List<Clause>();
    }

    public class ContractHistory
    {
        [BsonElement("status")]
        public string Status { get; set; } = string.Empty;

        [BsonElement("date")]
        public DateTime Date { get; set; }

        [BsonElement("observation")]
        public string Observation { get; set; } = string.Empty;

        [BsonElement("userId")]
        public string UserId { get; set; } = string.Empty;
    }

    public class Contract
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; } = ObjectId.GenerateNewId().ToString();

        [BsonElement("proposalId")]
        [BsonRepresentation(BsonType.ObjectId)]
        public string ProposalId { get; set; } = string.Empty;

        [BsonElement("clientId")]
        [BsonRepresentation(BsonType.ObjectId)]
        public string ClientId { get; set; } = string.Empty;

        [BsonElement("number")]
        public string Number { get; set; } = string.Empty;

        [BsonElement("status")]
        public string Status { get; set; } = string.Empty;

        [BsonElement("dates")]
        public ContractDates Dates { get; set; } = new ContractDates();

        [BsonElement("documents")]
        public Documents Documents { get; set; } = new Documents();

        [BsonElement("history")]
        public List<ContractHistory> History { get; set; } = new List<ContractHistory>();

        [BsonElement("createdAt")]
        public DateTime CreatedAt { get; set; }

        [BsonElement("updatedAt")]
        public DateTime UpdatedAt { get; set; }

        // Propiedades de navegación
        [BsonIgnore]
        public virtual Proposal Proposal { get; set; } = new Proposal();

        [BsonIgnore]
        public virtual PotentialClient Client { get; set; } = new PotentialClient();
    }
}