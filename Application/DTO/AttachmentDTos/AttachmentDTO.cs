using MongoDB.Bson;

namespace SagaAserhi.Application.DTOs
{
    public class AttachmentDTO
    {
        public string Id { get; set; } = ObjectId.GenerateNewId().ToString();
        public string FileName { get; set; } = string.Empty;
        public string ContentType { get; set; } = string.Empty;
        public DateTime UploadDate { get; set; } = DateTime.Now;
        public string PotentialClientId { get; set; } = string.Empty;
    }
}