using api_completa_mongodb_net_6_0.Application.DTO;
using api_completa_mongodb_net_6_0.Application.UseCases;
using Microsoft.AspNetCore.Mvc;

namespace api_completa_mongodb_net_6_0.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UsersController : ControllerBase
    {
        private readonly CreateUserUseCase _createUserUseCase;
        private readonly GetAllUsersUseCase _getAllUsersUseCase;
        private readonly UpdateUserUseCase _updateUserUseCase;
        private readonly DeleteUserUseCase _deleteUserUseCase;

        public UsersController(
            CreateUserUseCase createUserUseCase,
            GetAllUsersUseCase getAllUsersUseCase,
            UpdateUserUseCase updateUserUseCase,
            DeleteUserUseCase deleteUserUseCase)
        {
            _createUserUseCase = createUserUseCase;
            _getAllUsersUseCase = getAllUsersUseCase;
            _updateUserUseCase = updateUserUseCase;
            _deleteUserUseCase = deleteUserUseCase;
        }

        [HttpGet]
        public async Task<ActionResult<List<UserDto>>> GetAll()
        {
            var users = await _getAllUsersUseCase.ExecuteAsync();
            return Ok(users);
        }

        [HttpPost]
        public async Task<IActionResult> Create(CreateUserDto dto)
        {
            await _createUserUseCase.ExecuteAsync(dto);
            return CreatedAtAction(nameof(GetAll), new { });
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(string id, UpdateUserDto dto)
        {
            await _updateUserUseCase.ExecuteAsync(id, dto);
            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(string id)
        {
            await _deleteUserUseCase.ExecuteAsync(id);
            return NoContent();
        }
    }
}
