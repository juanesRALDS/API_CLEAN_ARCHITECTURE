using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace SagaAserhi.Application.DTO.ContractsDtos;
public class CreateContractDto
{
    [Required]
    public string Status { get; set; } = string.Empty;

    [Required]
    public DateTime Start { get; set; }

    [Required]
    public DateTime End { get; set; }

    public List<AnnexDto>? Annexes { get; set; }
    
    [Required]
    public List<ClauseDto> Clauses { get; set; } = new();

    public IFormFileCollection? Files { get; set; }

    public void Validate()
    {
        if (string.IsNullOrWhiteSpace(Status))
            throw new ArgumentException("El estado es requerido");
        if (Start == default)
            throw new ArgumentException("La fecha de inicio es requerida");
        if (End == default)
            throw new ArgumentException("La fecha de fin es requerida");
        if (!Clauses.Any())
            throw new ArgumentException("Debe incluir al menos una cláusula");
    }
}

