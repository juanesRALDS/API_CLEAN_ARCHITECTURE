using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SagaAserhi.Domain.Entities
{
    public class Proposal
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public decimal Amount { get; set; }
        public string Status { get; set; }
        public DateTime CreationDate { get; set; }
        public int PotentialClientId { get; set; }
        public PotentialClient PotentialClient { get; set; }
    }
}