namespace BancoAtlantida_API_REST.tools
{
    public class RoutesApi
    {
        static public Dictionary<string, string> Routes;
        static public String HostApi = "https://localhost:7270/services/Token";

        public RoutesApi()
        {
            Routes = new Dictionary<string, string>();
            Routes.Add("login", HostApi + "/login");
            Routes.Add("VerificarToken", HostApi + "/verify");
            Routes.Add("RefreshToken", HostApi + "/refresh");

        }

        static public string GetRoute(string key)
        {
            if (Routes.ContainsKey(key))
            {
                return Routes[key];
            }
            else
            {
                return null;
            }
        }
    }
}
