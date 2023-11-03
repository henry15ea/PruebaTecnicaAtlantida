namespace PortalAtlantida.ModelViews
{
    public class RootObject
    {
        public string mensaje { get; set; }
        public int codigo { get; set; }
        public string mensajeServidor { get; set; }
        public bool resultado { get; set; }
        public string token { get; set; } = "demo";
        public MVOperations datos { get; set; }
    }
}
