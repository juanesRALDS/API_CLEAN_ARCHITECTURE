using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace SagaAserhi.Application.DTO.SiteDto;

public class CreateSiteDto
{
    [Required]
    public string Name { get; set; }
    [Required]
    public string Address { get; set; }
    [Required]
    public string City { get; set; }
    [Phone]
    public string Phone { get; set; }
    public string ProposalId { get; set; }

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