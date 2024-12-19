using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace SagaAserhi.Domain.Entities
{
    public class PotentialClient
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        [BsonElement("_id")]
        public string Id { get; set; }
        
        [BsonElement("identificationTypeId")]
        public int IdentificationTypeId { get; set; }

        [BsonElement("economicActivityId")]
        public int EconomicActivityId { get; set; }

        [BsonElement("potentialClientStatusId")]
        public int PotentialClientStatusId { get; set; }

        [BsonElement("creationDate")]
        public DateTime CreationDate { get; set; }

        [BsonElement("personType")]
        public string PersonType { get; set; }

        [BsonElement("potentialClientSize")]
        public string PotentialClientSize { get; set; }

        [BsonElement("companyBusinessName")]
        public string CompanyBusinessName { get; set; }

        [BsonElement("companyNit")]
        public string CompanyNit { get; set; }

        [BsonElement("companyVerificationDigit")]
        public string CompanyVerificationDigit { get; set; }

        [BsonElement("representativeNames")]
        public string RepresentativeNames { get; set; }

        [BsonElement("representativeLastNames")]
        public string RepresentativeLastNames { get; set; }

        [BsonElement("representativeIdentification")]
        public string RepresentativeIdentification { get; set; }

        [BsonElement("contactPhone")]
        public string ContactPhone { get; set; }

        [BsonElement("contactEmail")]
        public string ContactEmail { get; set; }
    }
}