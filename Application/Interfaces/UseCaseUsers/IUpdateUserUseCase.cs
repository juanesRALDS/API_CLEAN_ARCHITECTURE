using SagaAserhi.Application.DTO;

namespace SagaAserhi.Application.Interfaces.UseCaseUsers;
public interface IUpdateUserUseCase
{
    Task<UpdateUserResponseDto> Execute(string id, UpdateUserDto updateUserDto);
}
