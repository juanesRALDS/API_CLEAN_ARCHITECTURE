namespace SagaAserhi.Application.DTO;

public class PotentialClientDto
{
    public int IdentificationTypeId { get; set; }
    public int EconomicActivityId { get; set; }
    public DateTime CreationDate { get; set; }
    public string PersonType { get; set; } = string.Empty;
    public string PotentialClientSize { get; set; } = string.Empty;
    public string CompanyBusinessName { get; set; } = string.Empty;
    public string RepresentativeNames { get; set; } = string.Empty;
    public string RepresentativeLastNames { get; set; } = string.Empty;
    public string RepresentativeIdentification { get; set; } = string.Empty;
    public string ContactPhone { get; set; } = string.Empty;
    public string ContactEmail { get; set; } = string.Empty;
}

public class UpdatePotentialClientDto : PotentialClientDto
{

}
