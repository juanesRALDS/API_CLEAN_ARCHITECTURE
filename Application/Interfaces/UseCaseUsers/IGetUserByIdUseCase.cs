using SagaAserhi.Application.DTO;
using System.Threading.Tasks;

namespace SagaAserhi.Application.Interfaces.UseCaseUsers;

public interface IGetUserByIdUseCase
{
    Task<UserDto> Execute(string userId);
}
