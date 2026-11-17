using HardwareTech.Application.DTOs.ExternalDto;
using HardwareTech.Application.Exceptions;
using HardwareTech.Application.Interfaces;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;

namespace HardwareTech.Infraestructure.Services
{
    public class DolarApiService : IDolarApiService
    {
        private readonly HttpClient _httpClient;

        public DolarApiService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<decimal> GetDollarRateAsync()
        {
            var response = await _httpClient.GetFromJsonAsync<DolarApiResponse>(
                "https://dolarapi.com/v1/dolares/oficial");

            if (response is null)
            {
                throw new DolarApiException(
                    "No se pudo obtener la cotización del dólar.");
            }

            return response.Venta;
        }
    }
}
