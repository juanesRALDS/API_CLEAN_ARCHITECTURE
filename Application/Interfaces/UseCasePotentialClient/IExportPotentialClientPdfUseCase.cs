// Application/Interfaces/UseCasePotentialClient/IExportPotentialClientPdfUseCase.cs
namespace SagaAserhi.Application.Interfaces.UseCasePotentialClient;

public interface IExportPotentialClientPdfUseCase
{
    Task<byte[]> Execute(CancellationToken cancellationToken);
}