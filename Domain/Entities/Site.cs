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
        public string Id { get; set; } // Quitar private
        public string Name { get; set; } // Quitar private
        public string Address { get; set; } // Quitar private
        public string City { get; set; } // Quitar private
        public string Phone { get; set; } // Quitar private
        public bool Status { get; set; } = true; // Valor por defecto
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public string ProposalId { get; set; } // Quitar private y GenerateNewId
    }
}