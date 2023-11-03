namespace BancoAtlantida_API_REST.Entidades
{
    public class UsuarioCreditoEntidad
    {
        public decimal deudaActual { get; set; } = 0;
        public decimal limiteCredito { get; set; } = 0;
        public decimal saldoDisponible { get; set; } = 0;
    }
}
