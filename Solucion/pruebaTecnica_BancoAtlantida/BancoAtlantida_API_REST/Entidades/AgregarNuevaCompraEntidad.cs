using System;


namespace BancoAtlantida_API_REST.Entidades
{
    public class AgregarNuevaCompraEntidad
    {
        public string? token { get; set; } = null;
        public string IdCuenta { get; set; } = string.Empty;
        public DateTime FechaCompra { get; set; } = DateTime.Now;
        public string DescripcionCompta { get; set; } = "Sin descripcion";
        public decimal MontoCompra { get; set; } = 0;
        
    }
}
