using Dapper;
using System.Data;
using System.Data.SqlClient;
using System;
using System.Linq;
using ServiceToken.tools;
using ServiceToken.Entidades.Data;
using ServiceToken.DTO;


namespace ServiceToken.models
{
    public class CRD_Token
    {
        private SqlConnection conection = null;
        private EntidadResponseModels EntidadResponseModels = null;
        //guarda el token en la db
        public EntidadResponseModels fn_SaveTokenToDatabase(EntidadRegistroToken datos)
        {

            bool resp = false;
            EntidadResponseModels = new EntidadResponseModels();
            EntidadResponseModels.result = 1;
            EntidadResponseModels.status = false;
            try
            {

                ConectionDB cn = new ConectionDB();
                conection = cn.fn_GetConnection();

                var parameters = new DynamicParameters();
                //parameters.Add("@id_pago", idGen);
                Console.Out.WriteLine(datos.TokenStr);
                parameters.Add("@Token", datos.TokenStr);
                parameters.Add("@UserId", datos.UserId);
                parameters.Add("@ExpirationTime", datos.ExpirationTime);
                parameters.Add("@TokenType", datos.TokenType);
                parameters.Add("@IssuedTime", datos.IssuedTime);
                parameters.Add("@Revoked", datos.Revoked);
                parameters.Add("@Action", "CREATE");

                if (cn.fn_StatusConection(conection) == true)
                {
                    using (var connection = conection)
                    {
                        //agregando los valores al procedure
                        //ejecutamos el procedure


                        int returnValue = connection.Execute("spTokens_CRUD", parameters, commandType: CommandType.StoredProcedure);
                        if (returnValue > 0)
                        {
                           
                            //el token se ha creado
                            EntidadResponseModels.result = datos.TokenStr;
                            EntidadResponseModels.status = true;
                        }
                        else
                        {
                            
                            //el token ya existe en la base de datos 
                            EntidadResponseModels.result = datos.TokenStr;
                            EntidadResponseModels.status = false;
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
                        int returnValue = connection.Execute("spTokens_CRUD", parameters, commandType: CommandType.StoredProcedure);


                        Console.WriteLine("resultado >> " + returnValue);
                        if (returnValue > 0)
                        {
                            //el token se ha creado
                            EntidadResponseModels.result = datos.TokenStr;
                            EntidadResponseModels.status = true;
                        }
                        else
                        {
                            EntidadResponseModels.result = datos.TokenStr;
                            EntidadResponseModels.status = false;
                        }
                    }

                    conection.Close();

                }



                return EntidadResponseModels;
            }
            catch (Exception e)
            {
                EntidadResponseModels.result = datos.TokenStr;
                EntidadResponseModels.status = false;
                return EntidadResponseModels;
            }


        }
        //end save function 

        //update token function

        public EntidadResponseModels fn_UpdateTokenToDatabase(EntidadRegistroToken datos)
        {
            EntidadResponseModels = new EntidadResponseModels();
           
            try
            {

                ConectionDB cn = new ConectionDB();
                conection = cn.fn_GetConnection();

                var parameters = new DynamicParameters();
                //parameters.Add("@id_pago", idGen);
                parameters.Add("@Token", datos.TokenStr);
                parameters.Add("@Revoked", datos.Revoked);
                parameters.Add("@ReturnValue", dbType: DbType.Int32, direction: ParameterDirection.Output);


                if (cn.fn_StatusConection(conection) == true)
                {
                    using (var connection = conection)
                    {
                        //agregando los valores al procedure
                        //ejecutamos el procedure


                        
                        connection.Execute("spTokens_Update", parameters, commandType: CommandType.StoredProcedure);
                        int returnValue = parameters.Get<int>("@ReturnValue");

                        Console.WriteLine(returnValue);
                        if (returnValue == 0)
                        {

                            //el token se ha creado
                            Console.WriteLine("> token refrescado");
                            EntidadResponseModels.result = datos.TokenStr;
                            EntidadResponseModels.status = true;
                        }
                        else
                        {
                            Console.WriteLine("> token fallo refresco");
                            //el token ya existe en la base de datos 
                            EntidadResponseModels.result = datos.TokenStr;
                            EntidadResponseModels.status = false;
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
                        connection.Execute("spTokens_Update", parameters, commandType: CommandType.StoredProcedure);
                        int returnValue = parameters.Get<int>("@ReturnValue");
                        Console.WriteLine(returnValue);

                        if (returnValue == 0)
                        {
                            Console.WriteLine("> token refrescado");
                            //el token se ha creado
                            EntidadResponseModels.result = datos.TokenStr;
                            EntidadResponseModels.status = true;
                        }
                        else
                        {
                            Console.WriteLine("> token fallo refresco");
                            //el token ya existe en la base de datos 
                            EntidadResponseModels.result = datos.TokenStr;
                            EntidadResponseModels.status = false;
                        }
                    }

                    conection.Close();

                }



                return EntidadResponseModels;
            }
            catch (Exception e)
            {
                Console.WriteLine("> error token refresco");
                
                return EntidadResponseModels;
            }


        }

        //end update token function


        public EntidadResponseModels fn_ReadTokenDatabase(EntidadDefecto datos)
        {

            bool resp = false;
            EntidadResponseModels = new EntidadResponseModels();
           
            try
            {

                ConectionDB cn = new ConectionDB();
                conection = cn.fn_GetConnection();

                var parameters = new DynamicParameters();
                //parameters.Add("@id_pago", idGen);
                parameters.Add("@Token", datos.Token.Trim());

                if (cn.fn_StatusConection(conection) == true)
                {
                    using (var connection = conection)
                    {
                        //agregando los valores al procedure
                        //ejecutamos el procedure


                        var result = connection.QueryFirstOrDefault<EntidadTokenResponse>("spTokens_Select", parameters, commandType: CommandType.StoredProcedure);
                        
                        

                        if (result != null)
                        {
                            //el token se ha creado
                            EntidadResponseModels.result = result;
                            EntidadResponseModels.status = true;
                        }
                        else
                        {
                            
                            EntidadResponseModels.result = null;
                            EntidadResponseModels.status = false;
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
                        var result = connection.QueryFirstOrDefault<EntidadTokenResponse>("spTokens_Select", parameters, commandType: CommandType.StoredProcedure);

                        if (result != null)
                        {
                            //el token se ha creado
                            EntidadResponseModels.result = result;
                            EntidadResponseModels.status = true;
                        }
                        else
                        {
                            EntidadResponseModels.result = null;
                            EntidadResponseModels.status = false;
                        }
                    }

                    conection.Close();

                }



                return EntidadResponseModels;
            }
            catch (Exception e)
            {
                return EntidadResponseModels;
            }


        }

        //end read function



        //end functions 
    }
    //end class
}
//end namespaces
