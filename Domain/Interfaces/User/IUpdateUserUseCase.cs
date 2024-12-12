using api_completa_mongodb_net_6_0.Application.DTO;
using System.Threading.Tasks;

namespace api_completa_mongodb_net_6_0.Domain.Interfaces.Users
{
    public interface IUpdateUserUseCase
    {
        Task<UpdateUserResponseDto> Execute(string id, UpdateUserDto updateUserDto);
    }
}