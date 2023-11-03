using System;



namespace BancoAtlantida_API_REST.Models.requestEntidad
{
    public class compraParams
    {
        public string id_compra { get; set; }
        public DateTime fechaCompra { get; set; }
        public string descripcionCompta { get; set; }
        public decimal montoCompra { get; set; }
        public string id_cuenta { get; set; }
        public decimal limiteCreditoUsuario { get; set; }
        public int return_value { get; set; }
    }
}
