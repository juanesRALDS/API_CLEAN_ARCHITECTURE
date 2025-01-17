using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MongoDB.Bson;

namespace SagaAserhi.Domain.Entities
{
    public class Attachment
    {
        public string Id { get; set; } = ObjectId.GenerateNewId().ToString();
        public string FileName { get; set; } = string.Empty;
        public string ContentType { get; set; } = string.Empty;
        public byte[] FileData { get; set; } = Array.Empty<byte>();
        public DateTime UploadDate { get; set; } = DateTime.Now;
        public string PotentialClientId { get; set; } = string.Empty;
    }
}