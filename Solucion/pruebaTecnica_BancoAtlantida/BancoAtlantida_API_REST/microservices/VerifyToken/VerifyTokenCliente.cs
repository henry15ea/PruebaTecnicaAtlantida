using BancoAtlantida_API_REST.microservices.Entidades;
using BancoAtlantida_API_REST.tools;
using Newtonsoft.Json;
using System.Text;

namespace BancoAtlantida_API_REST.microservices.VerifyToken
{
    public class VerifyTokenCliente
    {
        private string reponseDataJSON = null;

        public async Task<bool> fn_VerifyToken(MicroDefaultEntidad datos)
        {
            bool resp = false;

            if (string.IsNullOrEmpty(datos.token) || datos.token == "null")
            {
                //se ha recivido elementos vacios 
                return false;
            }
            else
            {
                //trabajando con los datos recividos
                var httpClient = new HttpClient();

                RoutesApi obj = new RoutesApi();
                string apiAddress = RoutesApi.GetRoute("VerificarToken");

                using (httpClient)
                {

                    // Agregar el token al encabezado "Authorization" de la solicitud HTTP
                    httpClient.DefaultRequestHeaders.Add("Authorization", datos.token.Trim());

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

        public LoginResponseEntidad GetDataAPI()
        {
            Console.WriteLine(reponseDataJSON);
            return JsonConvert.DeserializeObject<LoginResponseEntidad>(reponseDataJSON);
        }

        //end methods
    }
    //end class
}