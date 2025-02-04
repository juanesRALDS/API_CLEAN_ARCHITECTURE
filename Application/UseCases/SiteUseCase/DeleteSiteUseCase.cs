using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using SagaAserhi.Application.Interfaces.IRepository;
using SagaAserhi.Application.Interfaces.ISiteUseCase;
using SagaAserhi.Domain.Entities;

namespace SagaAserhi.Application.UseCases.SiteUseCase;

public class DeleteSiteUseCase : IDeleteSiteUseCase
    {
        private readonly ISiteRepository _siteRepository;

        public DeleteSiteUseCase(ISiteRepository siteRepository)
        {
            _siteRepository = siteRepository;
        }

        public async Task Execute(string id)
        {
            if (string.IsNullOrEmpty(id))
                throw new ArgumentException("El ID del sitio es requerido");

            Site site = await _siteRepository.GetByIdSite(id);
            if (site == null)
                throw new KeyNotFoundException($"No se encontró el sitio con ID: {id}");

            await _siteRepository.DeleteSite(id);
        }
    }