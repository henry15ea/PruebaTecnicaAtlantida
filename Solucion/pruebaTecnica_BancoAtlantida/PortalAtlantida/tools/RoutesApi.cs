using System;
using System.Collections.Generic;

namespace PortalAtlantida.tools
{
    public  class RoutesApi
    {
        static public Dictionary<string, string> Routes;
        static public String HostApi = "https://localhost:7192/api";

        public RoutesApi()
        {
            Routes = new Dictionary<string, string>();
            Routes.Add("login", HostApi + "/login");
            Routes.Add("CuentaInfo", HostApi + "/cuentaInfo");
            Routes.Add("NuevaCompra", HostApi + "/NuevaCompra");
            Routes.Add("NuevaPago", HostApi + "/NuevoPago");
            Routes.Add("HistorialCompasMeses", HostApi + "/HistorialCompasMeses");
            Routes.Add("HistorialComprasMesFilter", HostApi + "/HistorialComprasMesFilter");
          
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
