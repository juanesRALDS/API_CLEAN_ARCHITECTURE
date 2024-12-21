using SagaAserhi.Application.DTO;
using SagaAserhi.Application.Interfaces;
using SagaAserhi.Application.Interfaces.UseCasePotentialClient;

namespace SagaAserhi.Application.UseCases.PotentialClientsUseCase
{
    public class UpdatePotentialClientUseCase : IUpdatePotentialClientUseCase
    {
        private readonly IPotentialClientRepository _repository;

        public UpdatePotentialClientUseCase(IPotentialClientRepository repository)
        {
            _repository = repository;
        }

        public async Task<string> Execute(string id, UpdatePotentialClientDto dto)
        {
            var existingClient = await _repository.GetByIdPotencialClient(id) 
                ?? throw new KeyNotFoundException("Cliente potencial no encontrado");

            existingClient.IdentificationTypeId = dto.IdentificationTypeId;
            existingClient.EconomicActivityId = dto.EconomicActivityId;
            existingClient.PersonType = dto.PersonType;
            existingClient.PotentialClientSize = dto.PotentialClientSize;
            existingClient.CompanyBusinessName = dto.CompanyBusinessName;
            existingClient.RepresentativeNames = dto.RepresentativeNames;
            existingClient.RepresentativeLastNames = dto.RepresentativeLastNames;
            existingClient.RepresentativeIdentification = dto.RepresentativeIdentification;
            existingClient.ContactPhone = dto.ContactPhone;
            existingClient.ContactEmail = dto.ContactEmail;

            await _repository.UpdatePotentialClient(id, existingClient);
            return "Cliente potencial actualizado exitosamente";
        }
    }

}