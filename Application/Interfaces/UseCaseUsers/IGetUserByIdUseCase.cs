using SagaAserhi.Application.DTO;

namespace SagaAserhi.Application.Interfaces.UseCaseUsers;

public interface IGetUserByIdUseCase
{
    Task<UserDto> Execute(string userId);
}
