using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace SagaAserhi.Domain.Entities;

public class PotentialClient
{
    [BsonId]
    [BsonRepresentation(BsonType.ObjectId)]
    public string Id { get; set; } = ObjectId.GenerateNewId().ToString();

    [BsonElement("companyBusinessName")]
    public string CompanyBusinessName { get; set; } = string.Empty;

    [BsonElement("contactPhone")]
    public string ContactPhone { get; set; } = string.Empty;

    [BsonElement("contactEmail")]
    public string ContactEmail { get; set; } = string.Empty;

    [BsonElement("creationDate")]
    public DateTime CreationDate { get; set; }

    [BsonElement("status")]
    public string Status { get; set; } = string.Empty;

    [BsonElement("proposals")]
    public List<string> ProposalIds { get; set; } = new List<string>();
}