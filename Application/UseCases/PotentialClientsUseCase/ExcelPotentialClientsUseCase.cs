
using SagaAserhi.Application.Interfaces.IRepository;
using SagaAserhi.Application.Interfaces.Services;
using SagaAserhi.Application.Interfaces.UseCasePotentialClient;
using SagaAserhi.Infrastructure.Services;

namespace SagaAserhi.Application.UseCases.PotentialClientsUseCase;

public class ExcelPotentialClientUseCase : IExcelPotentialClientUseCase
{
    private readonly IPotentialClientExcelServices _excelService;
    private readonly IPotentialClientRepository _repository;

    public ExcelPotentialClientUseCase(IPotentialClientExcelServices excelService,
    IPotentialClientRepository repository)
    {
        _excelService = excelService;
        _repository = repository;
    }

    public async Task<byte[]> ExecuteAsync(CancellationToken cancellationToken)
    {
        try
        {
            var clients = await _repository.GetAllAsync(cancellationToken);
            if (!clients.Any())
            {
                throw new InvalidOperationException("No hay clientes potenciales para generar el reporte");
            }

            return await _excelService.GenerateExcel(clients);
        }
        catch (Exception ex) when (ex is not InvalidOperationException)
        {
            throw new ApplicationException($"Error al generar el PDF: {ex.Message}", ex);
        }
    }
}
