namespace PortalAtlantida.ModelViews.historialCompra
{
    public class RtObjectHistorial<T>
    {
        public string mensaje { get; set; }
        public int codigo { get; set; }
        public string mensajeServidor { get; set; }
        public bool resultado { get; set; }
        public string token { get; set; } = "demo";
        public DatosRC<T> datos { get; set; }
    }
}
