
using SagaAserhi.Application.Interfaces.Services;
using SagaAserhi.Application.Interfaces.UseCasePotentialClient;
using SagaAserhi.Domain.Entities;

namespace SagaAserhi.Application.UseCases.PotentialClientsUseCase;

public class ExcelPotentialClientUseCase : IExcelPotentialClientUseCase
{
    private readonly IPotentialClientExcelService _excelService;

    public ExcelPotentialClientUseCase(IPotentialClientExcelService excelService)
    {
        _excelService = excelService;
    }

    public async Task<byte[]> ExecuteAsync(CancellationToken cancellationToken)
    {
        return await _excelService.ExportToExcel(cancellationToken);
    }
}