namespace SagaAserhi.Application.Interfaces.Services;

public interface IProposalExcelService
{
    Task<byte[]> ExportToExcel(CancellationToken cancellationToken);
}