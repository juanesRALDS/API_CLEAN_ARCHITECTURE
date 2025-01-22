using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using SagaAserhi.Application.DTO.ContractsDtos;
using SagaAserhi.Application.Interfaces.IContractsUseCase;
using SagaAserhi.Application.Interfaces.IRepository;
using SagaAserhi.Application.Interfaces.Services;
using SagaAserhi.Domain.Entities;

namespace SagaAserhi.Application.UseCases.ContractsUseCase;

public class UpdateContractUseCase : IUpdateContractUseCase
{
    private readonly IContractRepository _contractRepository;
    private readonly IFileService _fileService;

    public UpdateContractUseCase(IContractRepository contractRepository, IFileService fileService)
    {
        _contractRepository = contractRepository;
        _fileService = fileService;
    }

    public async Task<ContractDto> Execute(string id, UpdateContractDto dto)
    {
        dto.Validate();

        Contract existingContract = await _contractRepository.GetContractById(id)
            ?? throw new Exception("Contrato no encontrado");

        // Actualizar campos básicos
        existingContract.Status = dto.Status;
        existingContract.Dates.Start = dto.Start;
        existingContract.Dates.End = dto.End;
        existingContract.UpdatedAt = DateTime.UtcNow;

        // Actualizar cláusulas
        existingContract.Documents.Clauses = dto.Clauses.Select(c => new Clause
        {
            Title = c.Title,
            Content = c.Content
        }).ToList();

        // Manejar reemplazo de anexo
        if (!string.IsNullOrEmpty(dto.AnnexToReplaceId) && dto.NewFile != null)
        {
            var annexToReplace = existingContract.Documents.Annexes
                .FirstOrDefault(a => a.AnnexId == dto.AnnexToReplaceId);

            if (annexToReplace == null)
                throw new Exception($"Anexo con ID {dto.AnnexToReplaceId} no encontrado");

            // Eliminar archivo antiguo
            await _fileService.DeleteFile(annexToReplace.Path);

            // Subir nuevo archivo
            string newFilePath = await _fileService.UploadFile(dto.NewFile, "contracts");

            // Actualizar información del anexo manteniendo el ID original
            int index = existingContract.Documents.Annexes.IndexOf(annexToReplace);
            existingContract.Documents.Annexes[index] = new Annex
            {
                AnnexId = annexToReplace.AnnexId, // Mantener el ID original
                Title = dto.NewFile.FileName,
                Path = newFilePath,
                UploadDate = DateTime.UtcNow
            };
        }

        // Actualizar historial
        existingContract.History.Add(new ContractHistory
        {
            Status = dto.Status,
            Date = DateTime.UtcNow,
            Observation = $"Contrato actualizado. Anexo {dto.AnnexToReplaceId} reemplazado",
            UserId = "system"
        });


        // Guardar cambios
        Contract updatedContract = await _contractRepository.UpdateContract(id, existingContract);

        // Mapear a DTO
        return new ContractDto
        {
            Id = updatedContract.Id,
            ProposalId = updatedContract.ProposalId,
            ClientId = updatedContract.ClientId,
            Number = updatedContract.Number,
            Status = updatedContract.Status,
            Dates = new ContractDatesDto
            {
                Start = updatedContract.Dates.Start,
                End = updatedContract.Dates.End
            },
            Documents = new DocumentsDto
            {
                Annexes = updatedContract.Documents.Annexes.Select(a => new AnnexDto
                {
                    AnnexId = a.AnnexId,
                    Title = a.Title,
                    Path = a.Path,
                    UploadDate = a.UploadDate
                }).ToList(),
                Clauses = updatedContract.Documents.Clauses.Select(c => new ClauseDto
                {
                    Title = c.Title,
                    Content = c.Content
                }).ToList()
            },
            History = updatedContract.History.Select(h => new ContractHistoryDto
            {
                Status = h.Status,
                Date = h.Date,
                Observation = h.Observation,
                UserId = h.UserId
            }).ToList(),
            CreatedAt = updatedContract.CreatedAt,
            UpdatedAt = updatedContract.UpdatedAt
        };
    }
}