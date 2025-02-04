

using SagaAserhi.Application.Interfaces.Services;
using SagaAserhi.Application.Interfaces.UseCasePotentialClient;

namespace SagaAserhi.Application.UseCases.PotentialClientsUseCase;

using SagaAserhi.Application.Interfaces.Services;
using SagaAserhi.Application.Interfaces.UseCasePotentialClient;
using SagaAserhi.Application.Interfaces.IRepository;
using SagaAserhi.Domain.Entities;

public class ExportPotentialClientPdfUseCase : IExportPotentialClientPdfUseCase
{
    private readonly IPotentialClientPdfService _pdfService;
    private readonly IPotentialClientRepository _repository;

    public ExportPotentialClientPdfUseCase(
        IPotentialClientPdfService pdfService,
        IPotentialClientRepository repository)
    {
        _pdfService = pdfService;
        _repository = repository;
    }

    public async Task<byte[]> Execute(CancellationToken cancellationToken)
    {
        try
        {
            IEnumerable<PotentialClient>? clients = await _repository.GetAllAsync(cancellationToken);
            if (!clients.Any())
            {
                throw new InvalidOperationException("No hay clientes potenciales para generar el reporte");
            }

            return await _pdfService.GeneratePdf(clients);
        }
        catch (Exception ex) when (ex is not InvalidOperationException)
        {
            throw new ApplicationException($"Error al generar el PDF: {ex.Message}", ex);
        }
    }
}