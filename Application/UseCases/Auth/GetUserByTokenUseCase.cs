// Application/UseCases/GetUserByTokenUseCase.cs
using System.Security.Claims;
using System.Threading.Tasks;
using SagaAserhi.Application.DTO;
using SagaAserhi.Application.Application.Interfaces.Auth.IAuthUsecases;
using SagaAserhi.Domain.Interfaces.Auth;
using SagaAserhi.Application.Interfaces.UseCaseUsers;
using SagaAserhi.Infrastructure.Services;
using SagaAserhi.Application.Interfaces;


namespace SagaAserhi.Application.UseCases.Auth
{
    public class GetUserByTokenUseCase : IGetUserByTokenUseCase
    {
        private readonly ITokenService _tokenServices;
        private readonly IUserRepository _userRepository;


        public GetUserByTokenUseCase(
            ITokenService tokenServices,
            IUserRepository userRepository
        )
        {
            _tokenServices = tokenServices;
            _userRepository = userRepository;
        }


        public async Task<UserDto?> Execute(string token)
        {
            // Validar el token primero
            ClaimsPrincipal? principal = _tokenServices.ValidateTokenAndGetPrincipal(token);
            if (principal == null) return null;

            // Obtener el ID del usuario desde los claims
            string? userId = principal.Claims.FirstOrDefault(c => c.Type == "id")?.Value;
            if (string.IsNullOrEmpty(userId)) return null;

            // Obtener el usuario de la base de datos
            Domain.Entities.User? user = await _userRepository.GetUserById(userId);
            if (user == null) return null;

            return new UserDto
            {
                Id = user.Id,
                Name = user.Name,
                Email = user.Email,
            };
        }
    }
}
