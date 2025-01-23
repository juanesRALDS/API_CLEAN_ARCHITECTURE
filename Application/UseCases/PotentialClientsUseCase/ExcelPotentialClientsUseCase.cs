using SagaAserhi.Application.DTO.PotentialClientDto;
using SagaAserhi.Application.Interfaces.IRepository;
using SagaAserhi.Application.Interfaces.Services;
using SagaAserhi.Application.Interfaces.UseCasePotentialClient;

public class ExcelPotentialClientUseCase : IExcelPotentialClientUseCase
{
    private readonly IPotentialClientExcelServices _excelService;
    private readonly IPotentialClientRepository _repository;

    public ExcelPotentialClientUseCase(
        IPotentialClientExcelServices excelService,
        IPotentialClientRepository repository)
    {
        _excelService = excelService;
        _repository = repository;
    }

    public async Task<ExcelfileClientDto> Execute(int pageNumber, int pageSize, CancellationToken cancellationToken)
    {
        var (clients, totalCount) = await _repository.GetAllForExcel(pageNumber, pageSize, cancellationToken);

        byte[] excelContent = await _excelService.GenerateExcel(
            clients,
            pageNumber,
            pageSize,
            totalCount
        );

        return new ExcelfileClientDto
        {
            Content = excelContent,
            FileName = $"PotentialClients_Page{pageNumber}_{DateTime.Now:yyyyMMdd}.xlsx"
        };
    }
}