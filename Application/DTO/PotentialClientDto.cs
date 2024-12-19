namespace SagaAserhi.Application.DTO;

public class PotentialClientDto
{
    public int IdentificationTypeId { get; set; }
    public int EconomicActivityId { get; set; }
    public DateTime CreationDate { get; set; }
    public string PersonType { get; set; } = null!;
    public string PotentialClientSize { get; set; } = null!;
    public string CompanyBusinessName { get; set; } = null!;
    public string RepresentativeNames { get; set; } = null!;
    public string RepresentativeLastNames { get; set; } = null!;
    public string RepresentativeIdentification { get; set; } = null!;
    public string ContactPhone { get; set; } = null!;
    public string ContactEmail { get; set; } = null!;
}