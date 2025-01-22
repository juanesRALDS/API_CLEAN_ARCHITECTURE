using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SagaAserhi.Application.DTO.ContractsDtos;

public class UpdateContractDto
{
    public string Status { get; set; } = string.Empty;
    public DateTime Start { get; set; }
    public DateTime End { get; set; }
    public List<ClauseDto> Clauses { get; set; } = new();
    public string? AnnexToReplaceId { get; set; } // ID o título del anexo a reemplazar
    public IFormFile? NewFile { get; set; }

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
        if (!string.IsNullOrEmpty(AnnexToReplaceId) && NewFile == null)
            throw new ArgumentException("Debe proporcionar un nuevo archivo para reemplazar el anexo");
    }
}