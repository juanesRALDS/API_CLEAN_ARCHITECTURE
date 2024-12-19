namespace SagaAserhi.Application.Interfaces.Auth.IAuthUsecases;

public interface IGeneratePasswordResetTokenUseCase
{
    Task<string> Execute(string email);
}