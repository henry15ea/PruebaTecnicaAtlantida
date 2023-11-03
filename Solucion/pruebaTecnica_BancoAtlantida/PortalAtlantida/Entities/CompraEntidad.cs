using System;

namespace PortalAtlantida.Entities
{
    public class CompraEntidad
    {
        public string? token { get; set; } = null;
        public DateTime fechaCompra { get; set; }
        public string descripcionCompta { get; set; } = "Sin datos";
        public decimal montoCompra { get; set; } = 0;
    }
}
