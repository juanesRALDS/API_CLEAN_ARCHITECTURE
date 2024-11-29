using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace api_completa_mongodb_net_6_0.Domain.Entities
{
    public class PasswordResetToken
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; } = ObjectId.GenerateNewId().ToString();

        public string UserId { get; set; } = string.Empty;// ID del usuario relacionado
        public string Token { get; set; } = string.Empty; // Token JWT generado
        public DateTime Expiration { get; set; } // Tiempo de expiración del token
    }
}
