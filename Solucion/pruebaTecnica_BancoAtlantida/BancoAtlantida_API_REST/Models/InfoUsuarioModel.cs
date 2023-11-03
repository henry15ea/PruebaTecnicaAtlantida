using BancoAtlantida_API_REST.DTOS;
using BancoAtlantida_API_REST.tools;
using Dapper;
using System.Data;
using System.Data.SqlClient;
using System;
using BancoAtlantida_API_REST.Models.requestEntidad;
using BancoAtlantida_API_REST.Entidades;

namespace BancoAtlantida_API_REST.Models
{
    public class InfoUsuarioModel
    {
        private SqlConnection conection = null;
        private RequestModelEntidad requestModelEntidad = null;


        public RequestModelEntidad fn_DatosUsuario(defaultEntidad datos) 
        {

            bool resp = false;
            requestModelEntidad = new RequestModelEntidad();

            try
            {

                ConectionDB cn = new ConectionDB();
                conection = cn.fn_GetConnection();
               

                if (cn.fn_StatusConection(conection) == true)
                {
                    using (var connection = conection)
                    {
                        //agregando los valores al procedure
                        var parameters = new
                        {
                            id = datos.token.Trim()
                        };

                        //ejecutamos el procedure
                        /*
                        var resultData = connection.Query(
                            "sp_selectUsuarioCuentas", 
                            parameters, 
                            commandType: CommandType.StoredProcedure).AsList();
                        */


                        var resultData = connection.Query(
                           "sp_selectUsuarioCuentasComplete",
                           parameters,
                           commandType: CommandType.StoredProcedure).AsList();


                        //resp = result.Any();
                        Console.WriteLine("token : " + datos.token.Trim());
                        Console.WriteLine("datos encontrados : " + resultData.Count);
                        if (resultData.Count >=0)
                        {
                            requestModelEntidad.result = resultData; ;
                            requestModelEntidad.status = true;
                        }
                        else {
                            requestModelEntidad.status = false;
                        }
                    }

                    conection.Close();
                }
                else
                {
                    conection.Open();

                    using (var connection = conection)
                    {
                        //agregando los valores al procedure
                        var parameters = new
                        {
                            id = datos.token.Trim()
                        };

                        //ejecutamos el procedure


                        var resultData = connection.Query(
                            "sp_selectUsuarioCuentasComplete",
                            parameters,
                            commandType: CommandType.StoredProcedure).AsList();

                        /*
                          var resultData = connection.Query(
                            "sp_selectUsuarioCuentas",
                            parameters,
                            commandType: CommandType.StoredProcedure).AsList();
                         */


                        //resp = result.Any();
                        Console.WriteLine("token : " + datos.token.Trim());
                        Console.WriteLine("datos encontrados : " + resultData.Count);
                        if (resultData.Count >= 0)
                        {
                            requestModelEntidad.result = resultData; ;
                            requestModelEntidad.status = true;
                        }
                        else
                        {
                            requestModelEntidad.status = false;
                        }
                    }

                    conection.Close();

                }



                return requestModelEntidad;
            }
            catch (Exception e)
            {

                return requestModelEntidad;
            }


        }

        //end fn_DatosUsuario

        //end class
    }
}
