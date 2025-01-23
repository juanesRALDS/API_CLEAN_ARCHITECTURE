using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace SagaAserhi.Application.DTO.ContractsDtos
{
    public class AddAnnexDto
    {
        [Required]
        public string ContractId { get; set; } = string.Empty;
        public IFormFileCollection Files { get; set; } = null!;
    }
}