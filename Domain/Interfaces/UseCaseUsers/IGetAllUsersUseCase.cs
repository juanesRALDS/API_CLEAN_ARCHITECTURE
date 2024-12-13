using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using api_completa_mongodb_net_6_0.Application.DTO;

namespace api_completa_mongodb_net_6_0.Domain.Interfaces.UseCaseUsers;

public interface IGetAllUsersUseCase
{
    Task<List<UserDto>> Execute(int pageNumber, int pageSize);
}
