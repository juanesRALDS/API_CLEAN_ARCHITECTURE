using SagaAserhi.Application.DTO.PotentialClientDto;
using SagaAserhi.Application.Interfaces.IRepository;
using SagaAserhi.Application.Interfaces.Services;
using SagaAserhi.Application.Interfaces.UseCasePotentialClient;

public class ExcelPotentialClientUseCase : IExcelPotentialClientUseCase
{
    private readonly IPotentialClientExcelServices _excelService;


    public ExcelPotentialClientUseCase(IPotentialClientExcelServices excelService)
    {
        _excelService = excelService;
    }

    public async Task<byte[]> Execute(CancellationToken cancellationToken)
    {
        return await _excelService.ExportToExcelPotencialClient(cancellationToken);
    }
}