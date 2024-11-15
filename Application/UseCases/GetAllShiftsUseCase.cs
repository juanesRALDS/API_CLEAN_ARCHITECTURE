using api_completa_mongodb_net_6_0.Domain.Entities;
using api_completa_mongodb_net_6_0.Domain.Interfaces;


namespace api_completa_mongodb_net_6_0.Application.UseCases
{
    public class GetAllShiftsUseCase
    {
        private readonly IShiftRepository _repository;

        public GetAllShiftsUseCase(IShiftRepository repository)
        {
            _repository = repository;
        }

        public async Task<List<Shift>> Execute() => await _repository.GetAllAsync();
    }
}
