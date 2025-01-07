namespace SagaAserhi.Application.Interfaces.IUseCaseProposal;

public interface IExcelProposalUseCase
{
    Task<byte[]> ExecuteAsync(CancellationToken cancellationToken);
}