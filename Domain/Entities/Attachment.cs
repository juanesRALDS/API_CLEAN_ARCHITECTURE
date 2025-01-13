using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SagaAserhi.Domain.Entities
{
    public class Attachment
    {
        public string Id { get; set; }
        public string FileName { get; set; }
        public string ContentType { get; set; }
        public byte[] FileData { get; set; }
        public DateTime UploadDate { get; set; }
        public string PotentialClientId { get; set; }
    }
}