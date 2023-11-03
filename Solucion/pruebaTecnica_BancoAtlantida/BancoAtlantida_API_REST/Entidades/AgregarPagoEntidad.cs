using System;


namespace BancoAtlantida_API_REST.Entidades
{
    public class AgregarPagoEntidad
    {
        public String token { get; set; }
        public String id_pago { get; set; }
        public String id_cuenta { get; set; }

        public DateTime fechaPago { get; set; }

        public decimal montoPago { get; set; }
    }
}
