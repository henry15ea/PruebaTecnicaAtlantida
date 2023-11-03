using Newtonsoft.Json;
using PortalAtlantida.Entities;
using PortalAtlantida.ModelViews.GeneralResponse;
using PortalAtlantida.tools;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace PortalAtlantida.models
{
    public class NuevaCompraModel
    {
        private string reponseDataJSON = null;

        public async Task<bool> fn_AgregaCompra(CompraEntidad datos)
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
                string apiAddress = RoutesApi.GetRoute("NuevaCompra");

                using (httpClient)
                {

                    // Agregar el token al encabezado de la solicitud HTTP
                    httpClient.DefaultRequestHeaders.Add("Authorization", datos.token.Trim());

                    var data = new
                    {
                        fechaCompra = datos.fechaCompra,
                        descripcionCompta = datos.descripcionCompta.Trim(),
                        montoCompra = datos.montoCompra
                    };

                    var content = new StringContent(JsonConvert.SerializeObject(data), Encoding.UTF8, "application/json");
                    var response = await httpClient.PostAsync(apiAddress, content);

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

        public StandarObjectResponse GetDataAPI()
        {
            StandarObjectResponse rootObject = JsonConvert.DeserializeObject<StandarObjectResponse>(reponseDataJSON);
            return rootObject;



        }
        //end methods
    }

    //end class
}

//end namespaces
