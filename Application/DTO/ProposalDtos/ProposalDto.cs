

namespace SagaAserhi.Application.DTO.ProposalDtos;
public class ProposalDto
{
    public string Id { get; set; } = string.Empty;
    public string ClientId { get; set; } = string.Empty;
    public string Number { get; set; } = string.Empty;
    public ProposalStatusDto Status { get; set; } = new();
    public List<SiteDto> Sites { get; set; } = new();
    public List<ProposalHistoryDto> History { get; set; } = new();
    public PaymentDto Payment { get; set; } = new();
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }
    public string CompanyBusinessName { get; set; } = string.Empty;
}

public class ProposalStatusDto
{
    public string Proposal { get; set; } = string.Empty;
    public string Sending { get; set; } = string.Empty;
    public string Review { get; set; } = string.Empty;
}

public class SiteDto
{
    public string Id { get; set; } = string.Empty;
    public string Name { get; set; } = string.Empty;
    public string Address { get; set; } = string.Empty;
    public string City { get; set; } = string.Empty;
    public string Phone { get; set; } = string.Empty;
    public string ClientID { get; set; } = string.Empty;
    public List<WasteDto> Wastes { get; set; } = new();
}

public class WasteDto
{
    public string Type { get; set; } = string.Empty;
    public string Classification { get; set; } = string.Empty;
    public string Treatment { get; set; } = string.Empty;
    public string Frequency { get; set; } = string.Empty;
    public decimal Price { get; set; }
    public string DescriptionWaste { get; set; } = string.Empty;
}


public class ProposalHistoryDto
{
    public DateTime Date { get; set; }
    public string PotentialClientId { get; set; } = string.Empty;
    public string Action { get; set; } = string.Empty;
}

public class PaymentDto
{
    public string Method { get; set; } = string.Empty;
    public string Frequency { get; set; } = string.Empty;
    public decimal Amount { get; set; }
}