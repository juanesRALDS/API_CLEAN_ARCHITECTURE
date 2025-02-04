using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using SagaAserhi.Domain.Entities;

namespace SagaAserhi.Application.DTO.SiteDto;

public class CreateSiteDto
{
    [Required]
    public string Name { get; set; } = string.Empty;
    [Required]
    public string Address { get; set; } = string.Empty;
    [Required]
    public string City { get; set; } = string.Empty;
    public string Department { get; set; } = string.Empty;
    [Phone]
    public string Phone { get; set; } = string.Empty;
    public List<WasteDto> Wastes { get; set; } = new();
    public FrequencyDto Frequency { get; set; } = new();

    public void Validate()
    {
        if (string.IsNullOrWhiteSpace(Name))
            throw new ArgumentException("El nombre es requerido");
        if (string.IsNullOrWhiteSpace(Address))
            throw new ArgumentException("La dirección es requerida");
        if (string.IsNullOrWhiteSpace(City))
            throw new ArgumentException("La ciudad es requerida");
    }


}

public class WasteDto
{
    [Required]
    public string Type { get; set; } = string.Empty;
    [Required]
    public string Classification { get; set; } = string.Empty;
    [Required]
    public string Treatment { get; set; } = string.Empty;
    [Required]
    [Range(0, double.MaxValue)]
    public decimal Price { get; set; }
    public string DescriptionWaste { get; set; } = string.Empty;
}

public class FrequencyDto
{
    [Required]
    public string FrequencyOfTravel { get; set; } = string.Empty;
    [Required]
    [Range(0, double.MaxValue)]
    public decimal Amount { get; set; }
}



public class SiteRequestDto
{
    public string ProposalId { get; set; } = string.Empty;
    public CreateSiteDto SiteInfo { get; set; } = null!;
}