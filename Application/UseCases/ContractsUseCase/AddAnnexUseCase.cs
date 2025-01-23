using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MongoDB.Bson;
using SagaAserhi.Application.DTO.ContractsDtos;
using SagaAserhi.Application.Interfaces.IContractsUseCase;
using SagaAserhi.Application.Interfaces.IRepository;
using SagaAserhi.Application.Interfaces.Services;
using SagaAserhi.Domain.Entities;

namespace SagaAserhi.Application.UseCases.ContractsUseCase;

public class AddAnnexUseCase : IAddAnnexUseCase
{
    private readonly IContractRepository _contractRepository;
    private readonly IFileService _fileService;

    public AddAnnexUseCase(IContractRepository contractRepository, IFileService fileService)
    {
        _contractRepository = contractRepository;
        _fileService = fileService;
    }

    public async Task<List<AnnexDto>> Execute(AddAnnexDto dto)
    {
        var contract = await _contractRepository.GetContractById(dto.ContractId)
            ?? throw new Exception("Contrato no encontrado");

        var newAnnexes = new List<Annex>();

        foreach (var file in dto.Files)
        {
            if (file.Length > 0)
            {
                string filePath = await _fileService.UploadFile(file, "contracts");
                var annex = new Annex
                {
                    AnnexId = ObjectId.GenerateNewId().ToString(),
                    Title = file.FileName,
                    Path = filePath,
                    UploadDate = DateTime.UtcNow
                };
                newAnnexes.Add(annex);
            }
        }

        contract.Documents.Annexes.AddRange(newAnnexes);
        contract.UpdatedAt = DateTime.UtcNow;

        await _contractRepository.UpdateContract(contract.Id, contract);

        return newAnnexes.Select(a => new AnnexDto
        {
            AnnexId = a.AnnexId,
            Title = a.Title,
            Path = a.Path,
            UploadDate = a.UploadDate
        }).ToList();
    }
}