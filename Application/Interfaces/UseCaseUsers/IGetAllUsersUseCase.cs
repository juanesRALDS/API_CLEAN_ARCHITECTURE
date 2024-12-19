
using SagaAserhi.Application.DTO;

namespace SagaAserhi.Application.Interfaces.UseCaseUsers;

public interface IGetAllUsersUseCase
{
    Task<List<UserDto>> Execute(int pageNumber, int pageSize);
}
