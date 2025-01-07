namespace SagaAserhi.Application.Interfaces.UseCasePotentialClient;

public interface IDeletePotentialClientUseCase
{
    Task<string> Execute(string id);
}