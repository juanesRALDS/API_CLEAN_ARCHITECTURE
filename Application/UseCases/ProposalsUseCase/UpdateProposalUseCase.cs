using SagaAserhi.Application.DTO.ProposalDtos;
using SagaAserhi.Application.Interfaces.IRepository;
using SagaAserhi.Application.Interfaces.IUseCaseProposal;
using SagaAserhi.Domain.Entities;

namespace SagaAserhi.Application.UseCases.ProposalsUseCase;

public class UpdateProposalUseCase : IUpdateProposalUseCase
{
    private readonly IProposalRepository _repository;

    public UpdateProposalUseCase(IProposalRepository repository)
    {
        _repository = repository;
    }

    public async Task<string> Execute(string id, UpdateProposalDto dto)
    {
        if (string.IsNullOrEmpty(id))
            throw new ArgumentException("El ID es requerido");

        if (dto == null)
            throw new ArgumentNullException(nameof(dto));

        try
        {
            Proposal? existingProposal = await _repository.GetProposalById(id)
                ?? throw new InvalidOperationException($"No se encontró la propuesta con ID: {id}");


            if (dto.Status != null)
            {
                if (!string.IsNullOrEmpty(dto.Status.Proposal))
                    existingProposal.Status.Proposal = dto.Status.Proposal;
                if (!string.IsNullOrEmpty(dto.Status.Sending))
                    existingProposal.Status.Sending = dto.Status.Sending;
                if (!string.IsNullOrEmpty(dto.Status.Review))
                    existingProposal.Status.Review = dto.Status.Review;
            }

            existingProposal.UpdatedAt = DateTime.UtcNow;

            bool result = await _repository.UpdateProposal(id, existingProposal);
            if (!result)
                throw new InvalidOperationException("No se pudo actualizar la propuesta");

            return "Propuesta actualizada exitosamente";
        }
        catch (Exception ex)
        {
            throw new InvalidOperationException($"Error al actualizar la propuesta: {ex.Message}", ex);
        }
    }
}