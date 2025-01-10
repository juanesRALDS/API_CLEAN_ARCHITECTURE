using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace SagaAserhi.Application.DTO.SiteDto;

public class CreateSiteDto
{
    [Required]
    public string Name { get; set; } = string.Empty;
    [Required]
    public string Address { get; set; } = string.Empty;
    [Required]
    public string City { get; set; } = string.Empty;
    [Phone]
    public string Phone { get; set; } = string.Empty;

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

public class SiteRequestDto
{
    public string ProposalId { get; set; } = string.Empty;
    public CreateSiteDto SiteInfo { get; set; } = null!;
}