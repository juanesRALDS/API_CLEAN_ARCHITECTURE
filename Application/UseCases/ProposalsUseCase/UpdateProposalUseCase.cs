using SagaAserhi.Application.DTO.ProposalDtos;
using SagaAserhi.Application.Interfaces;
using SagaAserhi.Application.Interfaces.Iproposal.IUseCaseProposal;

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
        try
        {
            if (string.IsNullOrEmpty(id))
                throw new ArgumentException("El ID es requerido");

            if (dto == null)
                throw new ArgumentNullException(nameof(dto));

            var existingProposal = await _repository.GetProposalById(id);
            if (existingProposal == null)
                throw new InvalidOperationException($"No se encontró la propuesta con ID: {id}");

            existingProposal.Title = dto.Title;
            existingProposal.Description = dto.Description;

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