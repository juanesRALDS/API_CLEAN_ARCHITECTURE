using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SagaAserhi.Application.Interfaces.ISiteUseCase;

public interface IDeleteSiteUseCase
{
    Task Execute(string id);
}