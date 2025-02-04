using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using SagaAserhi.Application.Interfaces.IContractsUseCase;
using SagaAserhi.Application.Interfaces.IRepository;
using SagaAserhi.Application.Interfaces.Services;
using SagaAserhi.Domain.Entities;

namespace SagaAserhi.Application.UseCases.ContractsUseCase;

public class ContractsPDFUseCase : IContractsPDFUseCase
{
    private readonly IContratPDFServices _pdfService;
    private readonly IPotentialClientRepository _clientRepository;
    private readonly ISiteRepository _siteRepository;

    public ContractsPDFUseCase(
    IContratPDFServices pdfService,
    IPotentialClientRepository clientRepository,
    ISiteRepository siteRepository)
    {
        _pdfService = pdfService;
        _clientRepository = clientRepository;
        _siteRepository = siteRepository;
    }

    public async Task<byte[]> Execute(string clientId, string siteId)
    {
        PotentialClient? client = await _clientRepository.GetByIdPotencialClient(clientId);
        if (client == null)
            throw new KeyNotFoundException($"Cliente no encontrado con ID: {clientId}");

        Site? site = await _siteRepository.GetByIdSite(siteId);
        if (site == null)
            throw new KeyNotFoundException($"Sitio no encontrado con ID: {siteId}");

        return await _pdfService.GenerateContractPDFServices(client, site);
    }
}
