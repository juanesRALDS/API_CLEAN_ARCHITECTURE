
using SagaAserhi.Application.DTO;
using SagaAserhi.Application.DTO.Auth;
using SagaAserhi.Application.Application.Interfaces.Auth.IAuthUsecases;
using SagaAserhi.Application.UseCases.Auth;
using SagaAserhi.Application.UseCases.Users;
using SagaAserhi.Application.Interfaces.UseCaseUsers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace SagaAserhi.Controllers;
[ApiController]
[Route("api/[controller]")]
public class UsersController : ControllerBase
{
    private readonly IGetAllUsersUseCase _getAllUsersUseCase;
    private readonly IUpdateUserUseCase _updateUserUseCase;
    private readonly IDeleteUserUseCase _deleteUserUseCase;
    private readonly IGetUserByTokenUseCase _getUserByTokenUseCase;

    private readonly IRegisterUseCase _registerUseCase;


    public UsersController(
        IGetAllUsersUseCase getAllUsersUseCase,
        IUpdateUserUseCase updateUserUseCase,
        IDeleteUserUseCase deleteUserUseCase,
        IGetUserByTokenUseCase getUserByTokenUseCase,
        IRegisterUseCase registerUseCase)
    {
        _getAllUsersUseCase = getAllUsersUseCase;
        _updateUserUseCase = updateUserUseCase;
        _deleteUserUseCase = deleteUserUseCase;
        _getUserByTokenUseCase = getUserByTokenUseCase;
        _registerUseCase = registerUseCase;
    }

    [HttpGet]
    public async Task<ActionResult<List<UserDto>>>  GetAll([FromQuery] int pageNumber = 1,
        [FromQuery] int pageSize = 10)
    {
        if (pageNumber <= 0 || pageSize <= 0)
            return BadRequest("Page number and size must be greater than 0");

        List<UserDto> users = await _getAllUsersUseCase.Execute(pageNumber, pageSize);
        return Ok(users);
    }

    [HttpPost("register")]
    public async Task<IActionResult> Register([FromBody] CreateUserDto userDto)
    {
        try
        {
            await _registerUseCase.Execute(userDto);
            return Ok("User registered successfully.");
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> Update(string id, UpdateUserDto dto)
    {
        UpdateUserResponseDto? response = await _updateUserUseCase.Execute(id, dto);
        return Ok(response);
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(string id)
    {
        await _deleteUserUseCase.Execute(id);
        return NoContent();
    }
}

