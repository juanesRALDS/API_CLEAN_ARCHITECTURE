// PotentialClientDto.cs
using MongoDB.Bson;
using SagaAserhi.Application.DTO.ProposalDtos;
using SagaAserhi.Domain.Entities;

namespace SagaAserhi.Application.DTO.PotentialClientDto;

public class IdentificationDto
{
    public string Type { get; set; } = string.Empty;
    public string Number { get; set; } = string.Empty;
}

public class BusinessInfoDto
{
    public string TradeName { get; set; } = string.Empty;
    public string EconomicActivity { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string Phone { get; set; } = string.Empty;
}

public class LocationDto
{
    public string Address { get; set; } = string.Empty;
    public string City { get; set; } = string.Empty;
    public string Department { get; set; } = string.Empty;
}

public class StatusHistoryDto
{
    public string Status { get; set; } = string.Empty;
    public DateTime Date { get; set; }
    public string Observation { get; set; } = string.Empty;
}

public class StatusDto
{
    public string Current { get; set; } = string.Empty;
    public List<StatusHistoryDto> History { get; set; } = new();
}

public class PotentialClientDto
{
    public string Id { get; set; } = string.Empty;
    public IdentificationDto Identification { get; set; } = new();
    public string LegalRepresentative { get; set; } = string.Empty;
    public BusinessInfoDto BusinessInfo { get; set; } = new();
    public LocationDto Location { get; set; } = new();
    public StatusDto Status { get; set; } = new();
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }
}


public class UpdatePotentialClientDto
{
    public Identification? Identification { get; set; }
    public string? LegalRepresentative { get; set; }
    public BusinessInfo? BusinessInfo { get; set; }
    public string? Status { get; set; }
}