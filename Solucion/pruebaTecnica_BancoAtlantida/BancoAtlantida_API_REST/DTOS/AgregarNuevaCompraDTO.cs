using System;

namespace BancoAtlantida_API_REST.DTOS
{
    public class AgregarNuevaCompraDTO
    {
        //public string? token { get; set; } = null;
        public DateTime FechaCompra { get; set; }
        public string DescripcionCompta { get; set; }
        public decimal MontoCompra { get; set; }
    }
}
