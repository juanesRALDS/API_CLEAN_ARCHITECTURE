using SagaAserhi.Application.Interfaces.IRepository;
using SagaAserhi.Application.Interfaces.UseCasePotentialClient;
using SagaAserhi.Domain.Entities;

namespace SagaAserhi.Application.UseCases.PotentialClientsUseCase
{
    public class DeletePotentialClientUseCase : IDeletePotentialClientUseCase
    {
        private readonly IPotentialClientRepository _repository;

        public DeletePotentialClientUseCase(IPotentialClientRepository repository)
        {
            _repository = repository;
        }

        public async Task<string> Execute(string Id)
        {
            PotentialClient? client = await _repository.GetByIdPotencialClient(Id);

            if (client == null)
            {
                throw new KeyNotFoundException($"No se encontró el cliente potencial con id: {Id}");
            }

            await _repository.DeletePoTencialClient(Id);
            return "Cliente potencial eliminado exitosamente";
        }
    }
}