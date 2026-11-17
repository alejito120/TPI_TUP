namespace HardwareTech.Application.Interfaces
{
    public interface IDolarApiService
    {
        Task<decimal> GetDollarRateAsync();
    }
}