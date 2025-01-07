using SagaAserhi.Application.DTO;
using SagaAserhi.Application.Interfaces.IRepository;
using SagaAserhi.Application.Interfaces.UseCasePotentialClient;
using SagaAserhi.Domain.Entities;

namespace SagaAserhi.Application.UseCases.PotentialClientsUseCase
{
    public class UpdatePotentialClientUseCase : IUpdatePotentialClientUseCase
    {
        private readonly IPotentialClientRepository _repository;

        public UpdatePotentialClientUseCase(IPotentialClientRepository repository)
        {
            _repository = repository;
        }

        public async Task<UpdatePotentialClientDto> Execute(string id, UpdatePotentialClientDto dto)
        {
            if (string.IsNullOrWhiteSpace(id))
            {
                throw new ArgumentException("El ID del cliente potencial es obligatorio.");                 
            }

            PotentialClient? existingClient = await _repository.GetByIdPotencialClient(id) 
                ?? throw new KeyNotFoundException($"Cliente potencial con ID {id} no encontrado.");


            PotentialClient? updateClient = new()
            {
                Id = id,
                CompanyBusinessName = dto.CompanyBusinessName ?? existingClient.CompanyBusinessName,
                ContactPhone = dto.ContactPhone ?? existingClient.ContactPhone,
                ContactEmail = dto.ContactEmail ?? existingClient.ContactEmail,
                Status = dto.Status ?? existingClient.Status
            };


            await _repository.UpdatePotentialClient(id, updateClient);
            

            return new UpdatePotentialClientDto
            {
                CompanyBusinessName = updateClient.CompanyBusinessName,
                ContactPhone = updateClient.ContactPhone,
                ContactEmail = updateClient.ContactEmail,
                Status = updateClient.Status
            };
        }   
    }

}