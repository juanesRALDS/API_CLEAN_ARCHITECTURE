using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace SagaAserhi.Domain.Entities
{
    public class PotentialClient
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        [BsonElement("_id")]
        public string Id { get; set; } = ObjectId.GenerateNewId().ToString();
        
        [BsonElement("identificationTypeId")]
        public int IdentificationTypeId { get; set; }

        [BsonElement("economicActivityId")]
        public int EconomicActivityId { get; set; }

        [BsonElement("potentialClientStatusId")]
        public int PotentialClientStatusId { get; set; }

        [BsonElement("creationDate")]
        public DateTime CreationDate { get; set; }

        [BsonElement("personType")]
        public string PersonType { get; set; } = string.Empty;

        [BsonElement("potentialClientSize")]
        public string PotentialClientSize { get; set; } = string.Empty;

        [BsonElement("companyBusinessName")]
        public string CompanyBusinessName { get; set; } = string.Empty;

        [BsonElement("companyNit")]
        public string CompanyNit { get; set; } = string.Empty;

        [BsonElement("companyVerificationDigit")]
        public string CompanyVerificationDigit { get; set; } = string.Empty;

        [BsonElement("representativeNames")]
        public string RepresentativeNames { get; set; } = string.Empty;

        [BsonElement("representativeLastNames")]
        public string RepresentativeLastNames { get; set; } = string.Empty;

        [BsonElement("representativeIdentification")]
        public string RepresentativeIdentification { get; set; } = string.Empty;

        [BsonElement("contactPhone")]
        public string ContactPhone { get; set; } = string.Empty;

        [BsonElement("contactEmail")]
        public string ContactEmail { get; set; } = string.Empty;
    }
}