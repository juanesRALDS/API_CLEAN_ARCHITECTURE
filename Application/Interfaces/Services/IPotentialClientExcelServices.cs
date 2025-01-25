using SagaAserhi.Domain.Entities;

namespace SagaAserhi.Application.Interfaces.Services;

public interface IPotentialClientExcelServices
{
    Task<byte[]> ExportToExcel(CancellationToken cancellationToken);
}