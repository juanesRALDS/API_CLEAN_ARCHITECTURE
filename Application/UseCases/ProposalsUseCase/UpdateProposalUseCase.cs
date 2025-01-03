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
        // Validaciones iniciales fuera del try-catch
        if (string.IsNullOrEmpty(id))
            throw new ArgumentException("El ID es requerido");

        if (dto == null)
            throw new ArgumentNullException(nameof(dto));

        try
        {
            Domain.Entities.Proposal? existingProposal = await _repository.GetProposalById(id) 
                ?? throw new InvalidOperationException($"No se encontró la propuesta con ID: {id}");
            existingProposal.Title = dto.Title;
            existingProposal.Description = dto.Description;
            existingProposal.Amount = dto.Amount;
            existingProposal.Status = dto.Status;

            bool result = await _repository.UpdateProposal(id, existingProposal);
            if (!result)
                throw new InvalidOperationException("No se pudo actualizar la propuesta");

            return "Propuesta actualizada exitosamente";
        }
        catch (InvalidOperationException)
        {
            throw; // Re-lanzar excepciones de operación sin envolverlas
        }
        catch (Exception ex)
        {
            throw new InvalidOperationException($"Error al actualizar la propuesta", ex);
        }
    }
}