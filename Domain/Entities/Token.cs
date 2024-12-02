using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace api_completa_mongodb_net_6_0.Domain.Entities;

public class Token
{
    [BsonId]
    [BsonRepresentation(BsonType.ObjectId)]
    public string Id { get; set; } = ObjectId.GenerateNewId().ToString(); 

    [BsonElement("Tokens")]
    public string Tokens { get; set; } = string.Empty;

    [BsonElement("expiration")]
    public DateTime Expiration { get; set; }

    [BsonElement("userId")]
    public string UserId { get; set; } = string.Empty;
    public DateTime ExpiryDate { get; internal set; }
}
