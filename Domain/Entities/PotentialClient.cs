using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System;
using System.Collections.Generic;

namespace SagaAserhi.Domain.Entities
{
    [BsonIgnoreExtraElements]

    public class Identification
    {
        [BsonElement("type")]
        public string Type { get; set; } = string.Empty;

        [BsonElement("number")]
        public string Number { get; set; } = string.Empty;
    }

    [BsonIgnoreExtraElements]
    public class BusinessInfo
    {

        [BsonElement("tradeName")]
        public string TradeName { get; set; } = string.Empty;

        [BsonElement("economicActivity")]
        public string EconomicActivity { get; set; } = string.Empty;

        [BsonElement("email")]
        public string Email { get; set; } = string.Empty;

        [BsonElement("phone")]
        public string Phone { get; set; } = string.Empty;
    }

    public class Location
    {
        [BsonElement("address")]
        public string Address { get; set; } = string.Empty;

        [BsonElement("city")]
        public string City { get; set; } = string.Empty;

        [BsonElement("department")]
        public string Department { get; set; } = string.Empty;
    }

    public class StatusHistory
    {
        [BsonElement("status")]
        public string Status { get; set; } = string.Empty;

        [BsonElement("date")]
        public DateTime Date { get; set; }

        [BsonElement("observation")]
        public string Observation { get; set; } = string.Empty;
    }

    public class Status
    {
        [BsonElement("current")]
        public string Current { get; set; } = string.Empty;

        [BsonElement("history")]
        public List<StatusHistory> History { get; set; } = new List<StatusHistory>();
    }

    [BsonIgnoreExtraElements]
    public class PotentialClient
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; } = ObjectId.GenerateNewId().ToString();

        [BsonElement("identification")]
        public Identification Identification { get; set; } = new();

        [BsonElement("businessInfo")]
        public BusinessInfo BusinessInfo { get; set; } = new();

        [BsonElement("location")]
        public Location Location { get; set; } = new();

        [BsonElement("status")]
        public Status Status { get; set; } = new();

        [BsonElement("createdAt")]
        public DateTime CreatedAt { get; set; }

        [BsonElement("updatedAt")]
        public DateTime UpdatedAt { get; set; }
    }
}