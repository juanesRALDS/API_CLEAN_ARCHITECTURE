using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SagaAserhi.Application.Interfaces.UseCaseUsers;

public interface IDeleteUserUseCase
{
    Task Execute(string id);
}
