using System;

namespace PortalAtlantida.Entities
{
    public class HistorialByFechaEntidad
    {
        public string? token { get; set; } = null;
        public DateTime fechaInicio { get; set; }
        public DateTime fechaFinal { get; set; }
    }
}
