using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SagaAserhi.Domain.Entities
{
    public class Proposal
    {
        public string Id { get; set; } = string.Empty;
        public string Title { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public decimal Amount { get; set; }
        public string Status { get; set; } = string.Empty;
        public DateTime CreationDate { get; set; }
        public string PotentialClientId { get; set; } = string.Empty;
        // Removemos la propiedad virtual PotentialClient
    }
}       