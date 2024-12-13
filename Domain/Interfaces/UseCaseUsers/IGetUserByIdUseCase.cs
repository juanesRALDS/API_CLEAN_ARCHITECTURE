using api_completa_mongodb_net_6_0.Application.DTO;
using System.Threading.Tasks;

namespace api_completa_mongodb_net_6_0.Domain.Interfaces.UseCaseUsers;

public interface IGetUserByIdUseCase
{
    Task<UserDto> Execute(string userId);
}
