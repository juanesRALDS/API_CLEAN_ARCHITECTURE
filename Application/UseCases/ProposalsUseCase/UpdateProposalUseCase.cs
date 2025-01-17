using SagaAserhi.Application.DTO.ProposalDtos;
using SagaAserhi.Application.Interfaces.IRepository;
using SagaAserhi.Application.Interfaces.IUseCaseProposal;

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
            var existingProposal = await _repository.GetProposalById(id)
                ?? throw new InvalidOperationException($"No se encontró la propuesta con ID: {id}");

            // Actualizar estado
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

            var result = await _repository.UpdateProposal(id, existingProposal);
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