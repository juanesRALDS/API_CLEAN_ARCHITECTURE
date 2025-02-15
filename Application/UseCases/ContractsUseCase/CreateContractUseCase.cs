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

public class CreateContractUseCase : ICreateContractUseCase
{
    private readonly IContractRepository _contractRepository;
    private readonly IProposalRepository _proposalRepository;
    private readonly IFileService _fileService;

    public CreateContractUseCase(
        IContractRepository contractRepository,
        IProposalRepository proposalRepository,
        IFileService fileService)
    {
        _contractRepository = contractRepository;
        _proposalRepository = proposalRepository;
        _fileService = fileService;
    }
    public async Task<Contract> Execute(string proposalId, CreateContractDto dto)
    {
        try
        {
            dto.Validate();
            Proposal? proposal = await _proposalRepository.GetProposalById(proposalId)
                ?? throw new InvalidOperationException($"No se encontró la propuesta con ID: {proposalId}");

            Contract? contract = new Contract
            {
                Id = ObjectId.GenerateNewId().ToString(),
                ProposalId = proposalId,
                ClientId = proposal.ClientId,
                Number = $"CONT-{DateTime.Now:yyyyMMdd}-{Guid.NewGuid().ToString("N").Substring(0, 8)}",
                Status = dto.Status,
                Dates = new ContractDates
                {
                    Start = dto.Start,
                    End = dto.End
                },
                Documents = new Documents
                {
                    Annexes = new List<Annex>(),
                    Clauses = dto.Clauses.Select(c => new Clause
                    {
                        Title = c.Title,
                        Content = c.Content
                    }).ToList()
                },

                History = new List<ContractHistory>
            {
                new ContractHistory
                {
                    Status = "Creado",
                    Date = DateTime.UtcNow,
                    Observation = "Contrato creado",
                    UserId = "system" // Valor por defecto
                }
            },
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow
            };

            if (dto.Files != null && dto.Files.Any())
            {
                foreach (IFormFile file in dto.Files)
                {
                    if (file.Length > 0)
                    {
                        String? filePath = await _fileService.UploadFile(file, "contracts");
                        contract.Documents.Annexes.Add(new Annex
                        {
                            AnnexId = ObjectId.GenerateNewId().ToString(),
                            Title = file.FileName,
                            Path = filePath,
                            UploadDate = DateTime.UtcNow
                        });
                    }
                }
            }

            bool success = await _contractRepository.CreateContract(contract);
            if (!success)
                throw new InvalidOperationException("No se pudo crear el contrato");

            return contract;
        }
        catch (Exception ex)
        {
            throw new Exception($"Error al crear el contrato: {ex.Message}", ex);
        }
    }
}