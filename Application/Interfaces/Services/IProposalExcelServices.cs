
namespace SagaAserhi.Infrastructure.Services;

public interface IProposalExcelService
{
    Task<byte[]> ExportToExcel(CancellationToken cancellationToken);
}