using BancoAtlantida_API_REST.microservices.Entidades;
using BancoAtlantida_API_REST.tools;
using Newtonsoft.Json;
using System.Text;

namespace BancoAtlantida_API_REST.microservices.Aunth
{
    public class AuthCliente
    {
        private string reponseDataJSON = null;

        public async Task<bool> fn_loginUser(UsuarioClavesEntidad datos)
        {
            bool resp = false;

            if (datos.username == null || datos.password == null)
            {
                //se ha recivido elementos vacios 
                return false;
            }
            else
            {
                //trabajando con los datos recividos
                var httpClient = new HttpClient();

                RoutesApi obj = new RoutesApi();
                string apiAddress = RoutesApi.GetRoute("login");

                using (httpClient)
                {
                    var data = new
                    {
                        username = datos.username.Trim(),
                        password = datos.password.Trim()
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

        public LoginResponseEntidad GetDataAPI()
        {
            Console.WriteLine(reponseDataJSON);
            return JsonConvert.DeserializeObject<LoginResponseEntidad>(reponseDataJSON);
        }

        //end methods
    }
    //end class
}