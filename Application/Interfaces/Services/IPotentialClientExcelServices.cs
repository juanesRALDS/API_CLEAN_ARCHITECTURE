using SagaAserhi.Domain.Entities;

namespace SagaAserhi.Application.Interfaces.Services
{
    public interface IPotentialClientExcelService
    {
        Task<byte[]> ExportToExcel(CancellationToken cancellationToken);
    }
}