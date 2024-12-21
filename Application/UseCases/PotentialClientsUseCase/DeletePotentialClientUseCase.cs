using SagaAserhi.Application.Interfaces;
using SagaAserhi.Application.Interfaces.UseCasePotentialClient;

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
            var client = await _repository.GetByIdPotencialClient(Id) 
                ?? throw new KeyNotFoundException("Cliente potencial no encontrado");

            await _repository.DeletePoTencialClient(Id);
            return "Cliente potencial eliminado exitosamente";
        }
    }
}