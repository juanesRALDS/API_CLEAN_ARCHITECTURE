namespace SagaAserhi.Application.Application.Interfaces.Auth.IAuthUsecases;
public interface IUpdatePasswordUseCase
{
    Task<bool> Execute(string token, string newPassword);
}
