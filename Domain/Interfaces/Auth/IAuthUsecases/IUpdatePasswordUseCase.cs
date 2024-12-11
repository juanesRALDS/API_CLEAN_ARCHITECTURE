namespace api_completa_mongodb_net_6_0.Domain.Interfaces.Auth.IAuthUsecases;
public interface IUpdatePasswordUseCase
{
    Task<bool> Execute(string token, string newPassword);
}
