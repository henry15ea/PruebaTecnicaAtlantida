using Newtonsoft.Json;
using PortalAtlantida.Entities;
using PortalAtlantida.ModelViews;
using PortalAtlantida.ModelViews.historialCompra;
using PortalAtlantida.tools;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace PortalAtlantida.models
{
    public class HistorialComprasAllModel
    {

        private string reponseDataJSON = null;

        public async Task<bool> fn_GetComprasAll(DefaultEntidad datos)
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


                RoutesApi obj = new RoutesApi();
                string apiAddress = RoutesApi.GetRoute("HistorialCompasMeses");

                using (httpClient)
                {

                    // Agregar el token al encabezado de la solicitud HTTP
                    httpClient.DefaultRequestHeaders.Add("Authorization", datos.token.Trim());

                    //var content = new StringContent(JsonConvert.SerializeObject(data), Encoding.UTF8, "application/json");
                    var response = await httpClient.GetAsync(apiAddress);

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

        public RtObjectHistorial<MVCompra> GetDataAPI()
        {
            RtObjectHistorial<MVCompra> rootObject = JsonConvert.DeserializeObject<RtObjectHistorial<MVCompra>>(reponseDataJSON);
            return rootObject;



        }
        //end methods
    }

    //end class
}

//end namespaces
