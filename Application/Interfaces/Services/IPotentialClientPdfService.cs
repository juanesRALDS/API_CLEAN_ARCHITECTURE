// Application/Interfaces/Services/IPotentialClientPdfService.cs
using SagaAserhi.Domain.Entities;
namespace SagaAserhi.Application.Interfaces.Services;

public interface IPotentialClientPdfService
{

    Task<byte[]> GeneratePdf(IEnumerable<PotentialClient> clients);

}