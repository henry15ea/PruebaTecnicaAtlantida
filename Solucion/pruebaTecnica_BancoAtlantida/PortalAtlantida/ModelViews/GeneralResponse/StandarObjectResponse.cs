using PortalAtlantida.ModelViews.historialCompra;

namespace PortalAtlantida.ModelViews.GeneralResponse
{
    public class StandarObjectResponse
    {
        public string mensaje { get; set; }
        public int codigo { get; set; }
        public string mensajeServidor { get; set; }
        public bool resultado { get; set; }
        public string token { get; set; }
        public MVOperations datos { get; set; }
    }
}
