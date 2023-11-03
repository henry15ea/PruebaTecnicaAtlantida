namespace BancoAtlantida_API_REST.Entidades
{
    public class DatosUsuarioEntidad
    {
        /*
         uc.id ,
        ui.id_uInfo,
        ui.nombre,
        ui.apellidos,
        ui.edad,
        ui.correo,
        ui.ndoc,
        ui.direccion,
        uc.numeroTargeta,
        uc.deudaActual,
        uc.limiteCredito,
        uc.saldoDisponible
         */
        public int id { get; set; }
        public int id_uInfo { get; set; }
        public string nombre { get; set; }
        public string apellidos { get; set; }
        public int edad { get; set; }
        public string correo { get; set; }
        public string ndoc { get; set; }
        public string direccion { get; set; }
        public string numeroTargeta { get; set; }
        public decimal deudaActual { get; set; }
        public decimal limiteCredito { get; set; }
        public decimal saldoDisponible { get; set; }
    }
}
