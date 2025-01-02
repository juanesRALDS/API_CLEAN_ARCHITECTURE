namespace SagaAserhi.Application.DTO
{
    public class CreatePotentialClientDto
    {
        public string CompanyBusinessName { get; set; } = string.Empty;
        public string ContactPhone { get; set; } = string.Empty;
        public string ContactEmail { get; set; } = string.Empty;
        public string Status { get; set; } = string.Empty;
    }
}