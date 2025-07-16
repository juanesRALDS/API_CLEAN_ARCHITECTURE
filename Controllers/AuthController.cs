using SagaAserhi.Application.Interfaces.Auth.IAuthUsecases;
using SagaAserhi.Application.DTO.Auth;
using SagaAserhi.Application.DTO;
using Microsoft.AspNetCore.Mvc;

namespace SagaAserhi.Controllers;

[Route("api/[controller]")]
public class AuthController : Controller
{
    private readonly IGetUserByTokenUseCase _getUserByTokenUseCase;
    private readonly ILoginUseCase _loginUserUseCase;
    private readonly IGeneratePasswordResetTokenUseCase _GeneratePasswordReset;
    private readonly IUpdatePasswordUseCase _updatePasswordUseCase;

    public AuthController(
        IGetUserByTokenUseCase getUserByTokenUseCase,
        ILoginUseCase loginUserUseCase,
        IGeneratePasswordResetTokenUseCase generatePasswordResetTokenUseCase,
        IUpdatePasswordUseCase updatePasswordUseCase

    )
    {
        _getUserByTokenUseCase = getUserByTokenUseCase;
        _loginUserUseCase = loginUserUseCase;
        _GeneratePasswordReset = generatePasswordResetTokenUseCase;
        _updatePasswordUseCase = updatePasswordUseCase;
    }


    [HttpGet("GetUserFromToken")]
    public async Task<IActionResult> GetUserFromToken()
    {
        try
        {
            string? authHeader = HttpContext.Request.Headers["Authorization"].FirstOrDefault();
            if (string.IsNullOrWhiteSpace(authHeader) || !authHeader.StartsWith("Bearer "))
            {
                return BadRequest(new { Message = "Token not provided or incorrect format." });
            }

            string token = authHeader.Substring("Bearer ".Length).Trim();

            UserDto? user = await _getUserByTokenUseCase.Execute(token);
            if (user == null)
            {
                return Unauthorized(new { Message = "Invalid or expired token or user not found." });
            }

            return Ok(user);
        }
        catch (Exception ex)
        {
            return StatusCode(500, new { Message = "An error occurred while processing the request.", Error = ex.Message });
        }
    }

    [HttpPost("login")]
    public async Task<IActionResult> Login([FromBody] LoginUserDto loginDto)
    {
        try
        {
            string? tokens = await _loginUserUseCase.Execute(loginDto);

            return Ok(new { Tokens = tokens });
        }
        catch (UnauthorizedAccessException)
        {
            return Unauthorized("Invalid credentials.");
        }
    }

    [HttpPost("forgot-password")]
    public async Task<IActionResult> ForgotPassword([FromBody] TokenRequestDto request)
    {
        if (string.IsNullOrEmpty(request.Email))
            return BadRequest(new { Message = "Email is required." });

        try
        {
            string? resetToken = await _GeneratePasswordReset.Execute(request.Email);
            if (string.IsNullOrEmpty(resetToken))
            {
                return BadRequest(new { Message = "Failed to generate reset token" });
            }

            string resetUrl = $"{HttpContext.Request.Scheme}://{HttpContext.Request.Host}/reset-password?token={resetToken}";

            Console.WriteLine(resetUrl);

            return Ok(new
            {
                Message = "Token generated successfully.",
                ResetUrl = resetUrl
            }
            );

        }
        catch (Exception ex)
        {
            return BadRequest(new { ex.Message });
        }
    }

    [HttpPost("update-new-password")]
    public async Task<IActionResult> UpdatePassword([FromBody] UpdatePasswordRequest request)
    {

        if (string.IsNullOrWhiteSpace(request.NewPassword) || string.IsNullOrWhiteSpace(request.Tokens))
            return BadRequest("New password and token are required");

        bool result = await _updatePasswordUseCase.Execute(request.Tokens, request.NewPassword);

        if (!result)
            return BadRequest("The token is invalid or has expired.");

        return Ok("Password updated successfully.");
    }
}