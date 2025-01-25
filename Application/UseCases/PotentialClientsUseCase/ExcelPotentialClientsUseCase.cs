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

    public async Task<ExcelfileClientDto> Execute(CancellationToken cancellationToken)
    {
        byte[] excelContent = await _excelService.ExportToExcel(cancellationToken);
        
        return new ExcelfileClientDto
        {
            Content = excelContent,
            FileName = $"PotentialClients_{DateTime.Now:yyyyMMdd}.xlsx"
        };
    }
}