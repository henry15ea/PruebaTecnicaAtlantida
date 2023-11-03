using System;

namespace BancoAtlantida_API_REST.DTOS
{
    public class HistorialCompraByMesFilterDTO
    {
        //public String token { get; set; }
        public DateTime FechaInicio { get; set; }
        public DateTime FechaFinal { get; set; }
    }
}
