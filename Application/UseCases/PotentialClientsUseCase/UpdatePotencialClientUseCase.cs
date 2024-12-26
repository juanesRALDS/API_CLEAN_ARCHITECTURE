using System.Net.Sockets;
using SagaAserhi.Application.DTO;
using SagaAserhi.Application.Interfaces;
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

            // mapear dto a entidad
            PotentialClient? updateClient = new()
            {
                Id = id,
                PersonType = dto.PersonType ?? existingClient.PersonType,
                CompanyBusinessName = dto.CompanyBusinessName ?? existingClient.CompanyBusinessName,
                RepresentativeNames = dto.RepresentativeNames ?? existingClient.RepresentativeNames,
                RepresentativeLastNames = dto.RepresentativeLastNames ?? existingClient.RepresentativeLastNames,
                ContactPhone = dto.ContactPhone ?? existingClient.ContactPhone,
                ContactEmail = dto.ContactEmail ?? existingClient.ContactEmail
            };

            //actualizar en la base de datos
            await _repository.UpdatePotentialClient(id, updateClient);
            
            // retornar cliente  actalizado
            return new UpdatePotentialClientDto
            {
                PersonType = updateClient.PersonType,
                CompanyBusinessName = updateClient.CompanyBusinessName,
                RepresentativeNames = updateClient.RepresentativeNames,
                RepresentativeLastNames = updateClient.RepresentativeLastNames,
                ContactPhone = updateClient.ContactPhone,
                ContactEmail = updateClient.ContactEmail
            };
        }   
    }

}