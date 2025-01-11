// Application/Interfaces/Services/IPotentialClientPdfService.cs
using SagaAserhi.Domain.Entities;

namespace SagaAserhi.Application.Interfaces;

public interface IPotentialClientPdfService
{
    Task<byte[]> ExportToPdf(CancellationToken cancellationToken);
}