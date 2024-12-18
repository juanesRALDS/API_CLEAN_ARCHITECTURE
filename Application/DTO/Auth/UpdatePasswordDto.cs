namespace SagaAserhi.Application.DTO.Auth;
public class UpdatePasswordRequest
{
    public string NewPassword { get; set; } = string.Empty;
    public string Tokens { get; set; } = string.Empty;
}

