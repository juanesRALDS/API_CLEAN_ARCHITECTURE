using SagaAserhi.Application.Interfaces.IUseCaseProposal;
using SagaAserhi.Infrastructure.Services;

namespace SagaAserhi.Application.UseCases.ProposalsUseCase;

public class ExcelProposalUseCase : IExcelProposalUseCase
{
    private readonly IProposalExcelService _excelService;

    public ExcelProposalUseCase(IProposalExcelService excelService)
    {
        _excelService = excelService;
    }

    public async Task<byte[]> ExecuteAsync(CancellationToken cancellationToken)
    {
        return await _excelService.ExportToExcel(cancellationToken);
    }
}