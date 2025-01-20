// Application/DTO/ContractDtos/ContractDto.cs
namespace SagaAserhi.Application.DTO.ContractsDtos;

public class ContractDto
{
    public string Id { get; set; } = string.Empty;
    public string ProposalId { get; set; } = string.Empty;
    public string ClientId { get; set; } = string.Empty;
    public string Number { get; set; } = string.Empty;
    public string Status { get; set; } = string.Empty;
    public ContractDatesDto Dates { get; set; } = new();
    public PaymentDto Payment { get; set; } = new();
    public DocumentsDto Documents { get; set; } = new();
    public List<ContractHistoryDto> History { get; set; } = new();
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }
}

public class ContractDatesDto
{
    public DateTime Start { get; set; }
    public DateTime End { get; set; }
}

public class PaymentDto
{
    public string Method { get; set; } = string.Empty;
    public string Frequency { get; set; } = string.Empty;
    public decimal Amount { get; set; }
}

public class DocumentsDto
{
    public List<AnnexDto> Annexes { get; set; } = new();
    public List<ClauseDto> Clauses { get; set; } = new();
}

public class AnnexDto
{
    public string Name { get; set; } = string.Empty;
    public string Path { get; set; } = string.Empty;
    public DateTime UploadDate { get; set; }
}

public class ClauseDto
{
    public string Content { get; set; } = string.Empty;
}

public class ContractHistoryDto
{
    public string Status { get; set; } = string.Empty;
    public DateTime Date { get; set; }
    public string Observation { get; set; } = string.Empty;
    public string UserId { get; set; } = string.Empty;
}