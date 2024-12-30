using MongoDB.Bson;
using SagaAserhi.Application.DTO.ProposalDtos;
using SagaAserhi.Domain.Entities;

namespace SagaAserhi.Application.DTO;

public class PotentialClientDto
{
    public string Id { get; set; } = ObjectId.GenerateNewId().ToString();
    public string PersonType { get; set; } = string.Empty;
    public string CompanyBusinessName { get; set; } = string.Empty;
    public string RepresentativeNames { get; set; } = string.Empty;
    public string RepresentativeLastNames { get; set; } = string.Empty;
    public string ContactPhone { get; set; } = string.Empty;
    public string ContactEmail { get; set; } = string.Empty;
     public List<ProposalDto> Proposals { get; set; } = new List<ProposalDto>();
}


public class UpdatePotentialClientDto : PotentialClientDto
{

}
