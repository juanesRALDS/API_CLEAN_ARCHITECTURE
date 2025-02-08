using System;
using System.Threading.Tasks;
using SagaAserhi.Application.DTO.ContractsDtos;
using SagaAserhi.Application.DTO.SiteDto;
using SagaAserhi.Application.Interfaces.IContractsUseCase;
using SagaAserhi.Application.Interfaces.IRepository;
using SagaAserhi.Domain.Entities;

namespace SagaAserhi.Application.UseCases.SiteUseCase
{
    public class UpdateSiteUseCase : IUpdateSiteUseCase
    {
        private readonly ISiteRepository _siteRepository;

        public UpdateSiteUseCase(ISiteRepository siteRepository)
        {
            _siteRepository = siteRepository;
        }

        public async Task<UpdateSiteDto> Execute(string id, UpdateSiteDto updateSiteDto)
        {
            if (string.IsNullOrEmpty(id))
                throw new ArgumentException("El ID no puede estar vacío");

            // Validar el DTO
            updateSiteDto.Validate();

            // Obtener el sitio existente
            Site existingSite = await _siteRepository.GetByIdSite(id);
            if (existingSite == null)
                throw new Exception("Sitio no encontrado");

            // Mapear DTO a entidad manteniendo datos importantes
            existingSite.Name = updateSiteDto.Name;
            existingSite.Address = updateSiteDto.Address;
            existingSite.City = updateSiteDto.City;
            existingSite.Phone = updateSiteDto.Phone;
            existingSite.Wastes = updateSiteDto.Wastes.Select(w => new Waste
            {
                Type = w.Type,
                Classification = w.Classification,
                Treatment = w.Treatment,
                Price = w.Price,
                DescriptionWaste = w.DescriptionWaste
            }).ToList();
            existingSite.Frequencies = new Frequency
            {
                FrequencyOfTravel = updateSiteDto.Frequency.FrequencyOfTravel,
                Amount = updateSiteDto.Frequency.Amount
            };

            // Calcular precio total
            existingSite.TotalPrice = existingSite.Wastes.Sum(w => w.Price) + existingSite.Frequencies.Amount;

            // Actualizar el sitio
            Site updatedSite = await _siteRepository.UpdateSite(id, existingSite);

            // Mapear entidad a DTO de respuesta
            UpdateSiteDto responseDto = new UpdateSiteDto
            {
                Name = updatedSite.Name,
                Address = updatedSite.Address,
                City = updatedSite.City,
                Phone = updatedSite.Phone,
                Wastes = updatedSite.Wastes.Select(w => new WasteDto
                {
                    Type = w.Type,
                    Classification = w.Classification,
                    Treatment = w.Treatment,
                    Price = w.Price,
                    DescriptionWaste = w.DescriptionWaste
                }).ToList(),
                Frequency = new FrequencyDto
                {
                    FrequencyOfTravel = updatedSite.Frequencies.FrequencyOfTravel,
                    Amount = updatedSite.Frequencies.Amount
                },
                CreatedAt = updatedSite.CreatedAt,
                UpdatedAt = DateTime.UtcNow
            };

            return responseDto;
        }
    }
}