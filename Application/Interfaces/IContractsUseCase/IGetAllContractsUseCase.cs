// Application/Interfaces/IUseCaseContract/IGetAllContractsUseCase.cs

// Application/Interfaces/IUseCaseContract/IGetAllContractsUseCase.cs
using SagaAserhi.Application.DTO.ContractsDtos;

namespace SagaAserhi.Application.Interfaces.IContractsUseCase;

public interface IGetAllContractsUseCase
{
    Task<(List<ContractDto>, int)> Execute(int pageNumber, int pageSize);
}