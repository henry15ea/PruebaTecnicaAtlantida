using BancoAtlantida_API_REST.Entidades;
using BancoAtlantida_API_REST.Models.requestEntidad;
using BancoAtlantida_API_REST.tools;
using Dapper;
using System;
using System.Data;
using System.Data.SqlClient;

namespace BancoAtlantida_API_REST.Models
{
    public class CompraNuevaModel
    {
        private SqlConnection conection = null;
        private RequestModelEntidad requestModelEntidad = null;

        //funcion que se encarga de extraer los datos financieros de la cuenta del usuario segun su 
        //id de cuenta 
        private RequestModelEntidad fn_GetLimiteCreditoUser(string idCuenta)
        {
            var requestModelEntidad = new RequestModelEntidad();
            var cn = new ConectionDB();
            var conection = cn.fn_GetConnection();
            decimal limiteCredito = 0;

            try
            {
                if (cn.fn_StatusConection(conection))
                {
                    conection.Open();
                }

                using (var connection = conection)
                {
                    var parameters = new { id = idCuenta };
                    var result = connection.QuerySingle<UsuarioCreditoEntidad>("sp_selectUsuarioCredito", parameters, commandType: CommandType.StoredProcedure);
                    limiteCredito = (decimal)result.limiteCredito;
                }

                requestModelEntidad.result = limiteCredito;
            }
            catch (Exception ex)
            {
                requestModelEntidad.status = false;
            }
            finally
            {
                conection.Close();
            }

            return requestModelEntidad;
        }

        //esta funcion se encarga de ingresar los datos de una nueva compra a la db 
        public RequestModelEntidad fn_AgregarNuevaCompra(AgregarNuevaCompraEntidad datos)
        {

            bool resp = false;
            requestModelEntidad = new RequestModelEntidad();

            try
            {

                ConectionDB cn = new ConectionDB();
                conection = cn.fn_GetConnection();
                EncodeID encode = new EncodeID();


                String idGen = encode.fn_sha256_Gen();

                var requestModelEntidad = fn_GetLimiteCreditoUser(datos.IdCuenta.Trim());
                decimal limiteCredito = (decimal)requestModelEntidad.result;
                //decimal limiteCredito = this.fn_GetLimiteCreditoUser(datos.IdCuenta.Trim()).result;

                var parameters = new DynamicParameters();
                parameters.Add("@id_compra", idGen);
                parameters.Add("@fechaCompra", DateTime.Now);
                parameters.Add("@descripcionCompta", datos.DescripcionCompta);
                parameters.Add("@montoCompra", datos.MontoCompra);
                parameters.Add("@id_cuenta", datos.IdCuenta);
                parameters.Add("@limiteCreditoUsuario", limiteCredito);
                parameters.Add("@return_value", dbType: DbType.Int32, direction: ParameterDirection.Output);

                if (cn.fn_StatusConection(conection) == true)
                {
                    using (var connection = conection)
                    {
                        //agregando los valores al procedure
                        //ejecutamos el procedure


                        connection.Execute("sp_CreateComprasComplete", parameters, commandType: CommandType.StoredProcedure);

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
                        connection.Execute("sp_CreateComprasComplete", parameters, commandType: CommandType.StoredProcedure);

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

    }
    //end class
}
