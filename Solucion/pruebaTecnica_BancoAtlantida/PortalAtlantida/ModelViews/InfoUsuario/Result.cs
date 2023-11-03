using System;

namespace PortalAtlantida.ModelViews
{
    public class Result
    {
        public string id { get; set; }
        public string id_uInfo { get; set; }
        public string nombre { get; set; }
        public string apellidos { get; set; }
        public int edad { get; set; }
        public string correo { get; set; }
        public string ndoc { get; set; }
        public string direccion { get; set; }
        public string numeroTargeta { get; set; }
        public double deudaActual { get; set; }
        public double limiteCredito { get; set; }
        public double saldoDisponible { get; set; }
        public DateTime fechaUltimoPago { get; set; }
        public double interesBonificable { get; set; }
        public double porcentajeInteresConfigurable { get; set; }
        public double montoTotalComprasMesActual { get; set; }
        public double montoTotalComprasMesAnterior { get; set; }
        public double porcentajeConfigurableSaldoMinimo { get; set; }
        public double cuotaMinimaPagar { get; set; }
        public double montoTotalContado { get; set; }
        public double montoTotalContadoInteres { get; set; }
    }
}
