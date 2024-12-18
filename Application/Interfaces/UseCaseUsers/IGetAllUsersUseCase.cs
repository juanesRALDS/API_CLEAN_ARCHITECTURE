using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using SagaAserhi.Application.DTO;

namespace SagaAserhi.Application.Interfaces.UseCaseUsers;

public interface IGetAllUsersUseCase
{
    Task<List<UserDto>> Execute(int pageNumber, int pageSize);
}
