// Application/DTO/ProposalDto.cs
namespace SagaAserhi.Application.DTO
{
    public class ProposalDto
    {
        public string Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public decimal Amount { get; set; }
        public string Status { get; set; }
        public DateTime CreationDate { get; set; }
        public string PotentialClientId { get; set; }
    }
}