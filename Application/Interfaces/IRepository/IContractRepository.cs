// Application/Interfaces/IRepository/IContractRepository.cs
using SagaAserhi.Domain.Entities;

namespace SagaAserhi.Application.Interfaces.IRepository;

public interface IContractRepository
{
    Task<(List<Contract>, int)> GetAllContracts(int pageNumber, int pageSize);
    Task<bool> CreateContract(Contract contract);
    Task<Contract?> GetContractById(string id);
    Task<List<Contract>> GetContractsByProposalId(string proposalId);
}