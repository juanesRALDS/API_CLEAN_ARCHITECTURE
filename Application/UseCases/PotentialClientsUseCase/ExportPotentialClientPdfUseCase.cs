using SagaAserhi.Application.Interfaces;
using SagaAserhi.Application.Interfaces.Services;

namespace SagaAserhi.Application.UseCases.PotentialClientsUseCase;

public class ExportPotentialClientPdfUseCase : IExportPotentialClientPdfUseCase
{
    private readonly IPotentialClientPdfService _pdfService;

    public ExportPotentialClientPdfUseCase(IPotentialClientPdfService pdfService)
    {
        _pdfService = pdfService;
    }

    public async Task<byte[]> ExecuteAsync(CancellationToken cancellationToken)
    {
        try
        {
            return await _pdfService.ExportToPdf(cancellationToken);
        }
        catch (Exception ex)
        {
            throw new ApplicationException($"Error al generar el PDF: {ex.Message}", ex);
        }
    }
}