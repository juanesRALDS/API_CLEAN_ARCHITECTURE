using SagaAserhi.Application.DTO;
using SagaAserhi.Application.Interfaces.IRepository;
using SagaAserhi.Application.Interfaces.UseCasePotentialClient;
using SagaAserhi.Domain.Entities;

namespace SagaAserhi.Application.UseCases.PotentialClientsUseCase
{
    public class CreatePotentialClientUseCase : ICreatePotentialClientUseCase
    {
        private readonly IPotentialClientRepository _repository;

        public CreatePotentialClientUseCase(IPotentialClientRepository repository)
        {
            _repository = repository;
        }

        public async Task<string> Execute(CreatePotentialClientDto dto)
        {
            if (dto == null)
                throw new ArgumentNullException(nameof(dto));

            if (string.IsNullOrEmpty(dto.CompanyBusinessName))
                throw new ArgumentException("Company name cannot be empty", 
                    nameof(dto.CompanyBusinessName));

            PotentialClient? potentialClient = new PotentialClient
            {
                CompanyBusinessName = dto.CompanyBusinessName,
                ContactPhone = dto.ContactPhone,
                ContactEmail = dto.ContactEmail,
                Status = dto.Status
            };

            await _repository.CreatePotentialClient(potentialClient);
            return "Potential client created successfully";
        }
    }
}