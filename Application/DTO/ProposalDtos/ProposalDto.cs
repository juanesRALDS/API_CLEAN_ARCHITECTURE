namespace SagaAserhi.Application.DTO.ProposalDtos;

public class ProposalDto
{
        public string Id { get; set; } = string.Empty;
        public string Title { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public decimal Amount { get; set; }
        public string Status { get; set; } = string.Empty;
        public DateTime CreationDate { get; set; }
        public string PotentialClientId { get; set; } = string.Empty;
        public string CompanyBusinessName { get; set; }  = string.Empty;
        
}