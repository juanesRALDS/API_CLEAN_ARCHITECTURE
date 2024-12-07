using System.Security.Claims;
using api_completa_mongodb_net_6_0.Application.DTO;
using api_completa_mongodb_net_6_0.Application.DTO.Auth;
using api_completa_mongodb_net_6_0.Application.UseCases.Auth;
using api_completa_mongodb_net_6_0.Application.UseCases.Users;
using api_completa_mongodb_net_6_0.Domain.Interfaces.Auth;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MongoApiDemo.Domain.Interfaces.Auth.IAuthUsecases;

namespace api_completa_mongodb_net_6_0.Controllers;
[ApiController]
[Route("api/[controller]")]
public class UsersController : ControllerBase
{
    private readonly GetAllUsersUseCase _getAllUsersUseCase;
    private readonly UpdateUserUseCase _updateUserUseCase;
    private readonly DeleteUserUseCase _deleteUserUseCase;
    private readonly ITokenService _tokenService;
    private readonly GetUserByTokenUseCase _getUserByTokenUseCase;

    private readonly IRegisterUseCase _registerUseCase;


    public UsersController(
        GetAllUsersUseCase getAllUsersUseCase,
        UpdateUserUseCase updateUserUseCase,
        DeleteUserUseCase deleteUserUseCase,
        ITokenService tokenService,
        GetUserByTokenUseCase getUserByTokenUseCase,
        IRegisterUseCase registerUseCase)
    {
        _getAllUsersUseCase = getAllUsersUseCase;
        _updateUserUseCase = updateUserUseCase;
        _deleteUserUseCase = deleteUserUseCase;
        _tokenService = tokenService;
        _getUserByTokenUseCase = getUserByTokenUseCase;
        _registerUseCase = registerUseCase;
    }

    [HttpGet]
    public async Task<ActionResult<List<UserDto>>> GetAll([FromQuery] int pageNumber = 1, [FromQuery] int pageSize = 10)
    {
        if (pageNumber <= 0 || pageSize <= 0)
            return BadRequest("El número de página y el tamaño deben ser mayores a 0");

        List<UserDto> users = await _getAllUsersUseCase.Login(pageNumber, pageSize);
        return Ok(users);
    }

    [HttpPost("register")]
    public async Task<IActionResult> Register([FromBody] CreateUserDto userDto)
    {
        try
        {
            await _registerUseCase.Register(userDto);
            return Ok("Usuario registrado exitosamente.");
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }


    [HttpGet("get-user-by-token")]
    [Authorize] // Si deseas proteger este endpoint
    public async Task<IActionResult> GetUserByToken([FromHeader] string authorization)
    {
        if (string.IsNullOrEmpty(authorization))
            return BadRequest("Token is missing.");

        string? tokens = authorization.StartsWith("Bearer ") ? authorization.Substring(7) : authorization;

        UserDto? user = await _getUserByTokenUseCase.Login(tokens);

        if (user == null) return NotFound("User not found.");

        return Ok(user);
    }



    [HttpPut("{id}")]
    public async Task<IActionResult> Update(string id, UpdateUserDto dto)
    {
        UpdateUserResponseDto? response = await _updateUserUseCase.Login(id, dto);
        return Ok(response);
    }


    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(string id)
    {
        await _deleteUserUseCase.Login(id);
        return NoContent();
    }
}

