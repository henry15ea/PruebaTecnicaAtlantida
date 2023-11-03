using BancoAtlantida_API_REST.DTOS;
using BancoAtlantida_API_REST.tools;
using Dapper;
using System.Data;
using System.Data.SqlClient;
using System;
using BancoAtlantida_API_REST.Models.requestEntidad;
using System.Linq;

namespace BancoAtlantida_API_REST.Models
{
    public class LoginModel
    {
        private SqlConnection conection = null;
        private RequestModelEntidad requestModelEntidad = null;
        //funcion que verifica si el usario existe retornara true , caso contrario retorna false
        public RequestModelEntidad fn_Islogued(LoginDTO datos) { 
            
            bool resp = false;
            requestModelEntidad = new RequestModelEntidad();


            try {

                ConectionDB cn = new ConectionDB();
                conection = cn.fn_GetConnection();
                EncodeID encrypt = new EncodeID();

                string ps = encrypt.fn_sha256Builder(datos.password.Trim());
                Console.WriteLine("hash : "+ps);

                if (cn.fn_StatusConection(conection) == true){
                    using (var connection = conection)
                    {
                        var parameters = new
                        {
                            uname = datos.username.Trim(),
                            pass = ps.Trim()


                        };

                        var result = connection.Query("sp_loginUsuarioCredenciales", parameters, commandType: CommandType.StoredProcedure);

                        resp = result.Any();

                        if (resp)
                        {
                            requestModelEntidad.result = result.First().id_credencial;
                            Console.WriteLine(requestModelEntidad.result);
                        }


                        requestModelEntidad.status = resp;
                        
                    }

                    conection.Close();
                }
                else {
                    conection.Open();
                    using (var connection = conection)
                    {
                        var parameters = new
                        {
                            uname = datos.username.Trim(),
                            pass = ps.Trim()
                        };

                        var result = connection.Query("sp_loginUsuarioCredenciales", parameters, commandType: CommandType.StoredProcedure);

                        resp = result.Any();

                        if (resp)
                        {
                            requestModelEntidad.result = result.First().id_credencial;
                            Console.WriteLine(requestModelEntidad.result);
                            // Realiza las operaciones necesarias con la propiedad "id_credencial"
                        }


                        requestModelEntidad.status = resp;

                    }
                    conection.Close();

                }

                

                return requestModelEntidad;
            }
            catch(Exception e){

                return requestModelEntidad;
            }

           
        }
        //end is_logued

        //fin clase
    }
}
