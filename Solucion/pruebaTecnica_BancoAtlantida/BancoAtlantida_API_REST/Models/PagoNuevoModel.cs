using BancoAtlantida_API_REST.Entidades;
using BancoAtlantida_API_REST.Models.requestEntidad;
using BancoAtlantida_API_REST.tools;
using Dapper;
using System;
using System.Data;
using System.Data.SqlClient;

namespace BancoAtlantida_API_REST.Models
{
    public class PagoNuevoModel
    {
        private SqlConnection conection = null;
        private RequestModelEntidad requestModelEntidad = null;

        //esta funcion se encarga de ingresar los datos de una nueva compra a la db 
        public RequestModelEntidad fn_AgregarNuevoPago(AgregarPagoEntidad datos)
        {

            bool resp = false;
            requestModelEntidad = new RequestModelEntidad();

            try
            {

                ConectionDB cn = new ConectionDB();
                conection = cn.fn_GetConnection();
                EncodeID encode = new EncodeID();


                String idGen = encode.fn_sha256_Gen();

                Console.WriteLine("@id_pago "+ idGen);
                Console.WriteLine("@fechaPago "+ datos.fechaPago.ToString("yyyy-MM-dd"));
                Console.WriteLine("@montoPago "+ datos.montoPago);
                Console.WriteLine("@id_cuenta "+ datos.id_cuenta);

                var parameters = new DynamicParameters();
                parameters.Add("@id_pago", idGen);
                parameters.Add("@fechaPago", datos.fechaPago.ToString("yyyy-MM-dd"));
                parameters.Add("@montoPago", datos.montoPago);
                parameters.Add("@id_cuenta", datos.id_cuenta);
                parameters.Add("@return_value", dbType: DbType.Int32, direction: ParameterDirection.Output);

                if (cn.fn_StatusConection(conection) == true)
                {
                    using (var connection = conection)
                    {
                        //agregando los valores al procedure
                        //ejecutamos el procedure


                        connection.Execute("InsertarPagoCompleto", parameters, commandType: CommandType.StoredProcedure);

                        int returnValue = parameters.Get<int>("@return_value");
                        Console.WriteLine("valor de retorno " + returnValue);

                        //resp = result.Any();
                        if (returnValue == 1)
                        {
                            requestModelEntidad.result = returnValue;
                            requestModelEntidad.status = true;
                        }
                        else
                        {
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

                        //ejecutamos el procedure
                        connection.Execute("InsertarPagoCompleto", parameters, commandType: CommandType.StoredProcedure);

                        int returnValue = parameters.Get<int>("@return_value");
                        Console.WriteLine("valor de retorno " + returnValue);

                        //resp = result.Any();
                        if (returnValue == 1)
                        {
                            requestModelEntidad.result = returnValue;
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

        //end funcs
    }
    //end class
}
