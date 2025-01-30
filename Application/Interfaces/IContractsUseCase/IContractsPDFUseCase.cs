using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SagaAserhi.Application.Interfaces.IContractsUseCase;

public interface IContractsPDFUseCase
{
    public Task<byte[]> Execute(string clientId, string siteId);
}