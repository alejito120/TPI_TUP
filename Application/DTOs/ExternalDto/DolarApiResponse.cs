namespace HardwareTech.Application.DTOs.ExternalDto
{
    public class DolarApiResponse
    {
        public string Moneda { get; set; } = string.Empty;

        public string Casa { get; set; } = string.Empty;

        public decimal Compra { get; set; }

        public decimal Venta { get; set; }

        public string FechaActualizacion { get; set; } = string.Empty;
    }
}
