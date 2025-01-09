using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SagaAserhi.Application.DTO.SiteDto
{
    public class SiteDtos
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public string Address { get; set; }
        public string City { get; set; }
        public string Phone { get; set; }
        public bool Status { get; set; }
        public DateTime CreatedAt { get; set; }
        public string ProposalId { get; set; }
    }
}