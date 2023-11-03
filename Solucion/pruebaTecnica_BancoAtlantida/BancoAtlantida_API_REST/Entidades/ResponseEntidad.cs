namespace BancoAtlantida_API_REST.Models
{
    public class ResponseEntidad
    {
        public string mensaje { get; set; } = "Mensaje por defecto";
        public int codigo { get; set; } = 404;

        public string mensajeServidor { get; set; } = "Este es un mensaje por defecto";
        public bool resultado { get; set; } = false;

        public string token { get; set; } = "null";

        public dynamic? datos { get; set; } = null;

    }
}
