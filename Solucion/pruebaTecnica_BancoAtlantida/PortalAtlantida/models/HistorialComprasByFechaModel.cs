using Newtonsoft.Json;
using PortalAtlantida.Entities;
using PortalAtlantida.ModelViews;
using PortalAtlantida.ModelViews.GeneralResponse;
using PortalAtlantida.ModelViews.historialCompra;
using PortalAtlantida.tools;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace PortalAtlantida.models
{
    public class HistorialComprasByFechaModel
    {
        private string reponseDataJSON = null;

        public async Task<bool> fn_GetComprasFilter(HistorialByFechaEntidad datos)
        {
            bool resp = false;

            if (datos.token.Trim() == null)
            {
                //se ha recivido elementos vacios 
                return false;
            }
            else
            {
                //trabajando con los datos recividos
                var httpClient = new HttpClient();



                //string apiAddress = RoutesApi.GetRoute("cuentaInfo");
                RoutesApi obj = new RoutesApi();
                string apiAddress = RoutesApi.GetRoute("HistorialComprasMesFilter");

                using (httpClient)
                {

                    // Agregar el token al encabezado "Authorization" de la solicitud HTTP
                    httpClient.DefaultRequestHeaders.Add("Authorization", datos.token.Trim());

                    var ApiURL = apiAddress;
                    ApiURL += $"?fechaInicio={datos.fechaInicio.ToString("yyyy-MM-dd")}&fechaFinal={datos.fechaFinal.ToString("yyyy-MM-dd")}";


                    var response = await httpClient.GetAsync(ApiURL);

                    //reviso el status code que trae la api
                    if (response.IsSuccessStatusCode)
                    {
                        System.Diagnostics.Debug.WriteLine(response.Content.ReadAsStringAsync());
                        var result = await response.Content.ReadAsStringAsync();


                        reponseDataJSON = result;


                        // manejar la respuesta exitosa aquí
                        return true;
                    }
                    else
                    {
                        //no se obtuvo el resultado esperado 
                        return resp;
                    }
                }
            }
        }//fin fn_IsloggedIn

        public RtObjectHistorial<MVHistorialCompras> GetDataAPI()
        {
            RtObjectHistorial<MVHistorialCompras> rootObject = JsonConvert.DeserializeObject<RtObjectHistorial<MVHistorialCompras>>(reponseDataJSON);
            return rootObject;



        }
        //end methods
    }

    //end class
}

//end namespaces
