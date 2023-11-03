using System;

namespace BancoAtlantida_API_REST.DTOS
{
    public class AgregarPagoDTO
    {
        //public String token { get; set; }
       
        public DateTime fechaPago { get; set; }

        public decimal montoPago { get; set; }
    }
}
