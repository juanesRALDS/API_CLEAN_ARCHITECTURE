// Application/Interfaces/UseCasePotentialClient/IExportPotentialClientPdfUseCase.cs
namespace SagaAserhi.Application.Interfaces;

public interface IExportPotentialClientPdfUseCase
{
    Task<byte[]> ExecuteAsync(CancellationToken cancellationToken);
}