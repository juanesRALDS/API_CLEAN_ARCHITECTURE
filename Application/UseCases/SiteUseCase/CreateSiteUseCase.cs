using SagaAserhi.Application.DTO.SiteDto;
using SagaAserhi.Application.Interfaces.IRepository;
using SagaAserhi.Application.Interfaces.ISiteUseCase;
using SagaAserhi.Domain.Entities;

namespace SagaAserhi.Application.UseCases.SiteUseCase
{
    public class CreateSiteUseCase : ICreateSiteUseCase
    {
        private readonly ISiteRepository _siteRepository;
        private readonly IProposalRepository _proposalRepository;

        public CreateSiteUseCase(ISiteRepository siteRepository, IProposalRepository proposalRepository)
        {
            _siteRepository = siteRepository;
            _proposalRepository = proposalRepository;
        }

        public async Task<SiteDtos> Execute(CreateSiteDto dto)
        {
            try
            {
                // Validaciones básicas
                if (dto == null) 
                    throw new ArgumentNullException(nameof(dto));
                
                dto.Validate();

                // Validar propuesta
                var proposal = await _proposalRepository.GetProposalById(dto.ProposalId);
                if (proposal == null)
                    throw new Exception($"Propuesta no encontrada con ID: {dto.ProposalId}");

                // Verificar si ya existe un sitio para esta propuesta
                var existingSites = await _siteRepository.GetByProposalIdAsync(dto.ProposalId);
                if (existingSites != null && existingSites.Any())
                    throw new Exception("Esta propuesta ya tiene un sitio asignado");

                // Crear nueva entidad Site
                var site = new Site
                {
                    Name = dto.Name?.Trim(),
                    Address = dto.Address?.Trim(),
                    City = dto.City?.Trim(),
                    Phone = dto.Phone?.Trim(),
                    ProposalId = dto.ProposalId,
                    Status = true,
                    CreatedAt = DateTime.UtcNow
                };

                // Guardar sitio
                await _siteRepository.CreateAsync(site);

                // Actualizar propuesta
                proposal.HasSite = true;
                await _proposalRepository.UpdateProposal(proposal);

                // Retornar DTO
                return new SiteDtos
                {
                    Id = site.Id,
                    Name = site.Name,
                    Address = site.Address,
                    City = site.City,
                    Phone = site.Phone,
                    ProposalId = site.ProposalId,
                    Status = site.Status,
                    CreatedAt = site.CreatedAt
                };
            }
            catch (Exception ex)
            {
                throw new Exception($"Error al crear la sede: {ex.Message}");
            }
        }
    }
}