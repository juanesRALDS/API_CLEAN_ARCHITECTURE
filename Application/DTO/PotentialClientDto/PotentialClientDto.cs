// PotentialClientDto.cs
using MongoDB.Bson;
using SagaAserhi.Application.DTO.ProposalDtos;
using SagaAserhi.Domain.Entities;

namespace SagaAserhi.Application.DTO.PotentialClientDto;

public class PotentialClientDto
{
    public string Id { get; set; } = ObjectId.GenerateNewId().ToString();

    public Identification Identification { get; set; } = new();
    public BusinessInfo BusinessInfo { get; set; } = new();
    public Location Location { get; set; } = new();
    public Status Status { get; set; } = new();
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }
    public List<ProposalDto> Proposals { get; set; } = new();
}

public class UpdatePotentialClientDto
{
    public Identification? Identification { get; set; }
    public BusinessInfo? BusinessInfo { get; set; }
    public Location? Location { get; set; }
     public string? Status { get; set; }
}