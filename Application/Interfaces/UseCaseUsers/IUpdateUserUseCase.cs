using SagaAserhi.Application.DTO;
using System.Threading.Tasks;

namespace SagaAserhi.Application.Interfaces.UseCaseUsers;
public interface IUpdateUserUseCase
{
    Task<UpdateUserResponseDto> Execute(string id, UpdateUserDto updateUserDto);
}
