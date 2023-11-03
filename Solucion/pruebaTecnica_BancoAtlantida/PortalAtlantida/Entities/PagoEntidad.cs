using System;

namespace PortalAtlantida.Entities
{
    public class PagoEntidad
    {
        public string? token { get; set; } = null;
        public DateTime fechaPago { get; set; }
        public decimal montoPago { get; set; } = 0;
    }
}
