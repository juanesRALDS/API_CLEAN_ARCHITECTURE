using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using SagaAserhi.Application.Application.Interfaces.Auth.IAuthUsecases;
using SagaAserhi.Application.DTO;
using SagaAserhi.Application.DTO.Auth;

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
    [Authorize]
    public async Task<IActionResult> GetUserFromToken()
    {
        try
        {
            string? authHeader = HttpContext.Request.Headers["Authorization"].FirstOrDefault();
            if (string.IsNullOrWhiteSpace(authHeader) || !authHeader.StartsWith("Bearer "))
            {
                return BadRequest(new { Message = "Token no proporcionado o formato incorrecto." });
            }

            string token = authHeader.Substring("Bearer ".Length).Trim();

            UserDto? user = await _getUserByTokenUseCase.Execute(token);
            if (user == null)
            {
                return Unauthorized(new { Message = "Token inválido o caducado o usuario no encontrado." });
            }

            return Ok(user);
        }
        catch (Exception ex)
        {
            return StatusCode(500, new { Message = "Ocurrió un error al procesar la solicitud.", Error = ex.Message });
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
            return Unauthorized("Credenciales inválidas.");
        }
    }

    [HttpPost("forgot-password")]
    public async Task<IActionResult> ForgotPassword([FromBody] TokenRequestDto request)
    {
        if (string.IsNullOrEmpty(request.Email))
            return BadRequest(new { Message = "El correo electrónico es obligatorio." });

        try
        {
            string? resetToken = await _GeneratePasswordReset.Execute(request.Email);
            if (string.IsNullOrEmpty(resetToken))
            {
                return BadRequest(new { Message = "no se pudo generar el token de restablecimiento" });
            }

            string resetUrl = $"{HttpContext.Request.Scheme}://{HttpContext.Request.Host}/reset-password?token={resetToken}";

            Console.WriteLine(resetUrl);

            return Ok(new
            {
                Message = "Token generado con éxito.",
                ResetUrl = resetUrl
            }
            );

        }
        catch (Exception ex)
        {
            return BadRequest(new { ex.Message });
        }
    }

    [HttpPost("update")]
    public async Task<IActionResult> UpdatePassword([FromBody] UpdatePasswordRequest request)
    {

        if (string.IsNullOrWhiteSpace(request.NewPassword) || string.IsNullOrWhiteSpace(request.Tokens))
            return BadRequest("la nueva contraseña y el token son requeridas");

        bool result = await _updatePasswordUseCase.Execute(request.Tokens, request.NewPassword);

        if (!result)
            return BadRequest("El token es inválido o ha expirado.");

        return Ok("Contraseña actualizada con éxito.");
    }
}