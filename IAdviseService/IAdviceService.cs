using WeatherDTO;

namespace IAdviseService
{
    public interface IAdviceService
    {
        public Task<AdviceResultDTO> GetAdvice(string location);
    }
}
