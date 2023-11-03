using System;


namespace BancoAtlantida_API_REST.Entidades
{
    public class HistorialComprasByFechaEntidad
    {
        public String token { get; set; }
        public string IdCuenta { get; set; }
        public DateTime FechaInicio { get; set; }
        public DateTime FechaFinal { get; set; }
    }
}
