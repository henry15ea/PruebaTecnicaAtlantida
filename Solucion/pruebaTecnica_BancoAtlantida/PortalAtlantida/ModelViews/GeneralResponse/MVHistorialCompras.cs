using System;

namespace PortalAtlantida.ModelViews.GeneralResponse
{
    public class MVHistorialCompras
    {
        public String id_compra { get; set; }
        public DateTime fechaCompra { get; set; }
        public string descripcionCompta { get; set; } = "Sin datos";
        public decimal montoCompra { get; set; } = 0;
    }
}
