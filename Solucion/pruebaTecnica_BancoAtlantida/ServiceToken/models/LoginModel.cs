
using Dapper;
using System.Data;
using System.Data.SqlClient;
using System;
using System.Linq;
using ServiceToken.tools;
using ServiceToken.Entidades.Data;
using ServiceToken.DTO;
using ServiceToken.models;

namespace ServiceToken.Models
{
    public class LoginModel
    {
        private SqlConnection conection = null;
        private EntidadResponseModels EntidadResponseModels = null;
        private CRD_Token crd_token = null;

        //funcion que verifica si el usario existe retornara true , caso contrario retorna false
        public EntidadResponseModels fn_Islogued(LoginRequestDTO datos) { 
            
            bool resp = false;
            String id_user = null;
            EntidadResponseModels = new EntidadResponseModels();


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
                            EntidadResponseModels.result = result.First().id_credencial;
                            
                            id_user = result.First().id_user;



                        }


                        EntidadResponseModels.status = resp;
                        
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

                            EntidadResponseModels.result = result.First().id_credencial;
                            Console.WriteLine(EntidadResponseModels.result);



                            // Realiza las operaciones necesarias con la propiedad "id_credencial"
                        }
                        
                            EntidadResponseModels.status = resp;
                        

                    }
                    conection.Close();

                }

                

                return EntidadResponseModels;
            }
            catch(Exception e){

                return EntidadResponseModels;
            }

           
        }
        //end is_logued

        public EntidadResponseModels fn_AutorizarLogin(LoginRequestDTO datos) 
        {
            crd_token = new CRD_Token();    
            try 
            {
                EntidadResponseModels = new EntidadResponseModels();
                EntidadResponseModels.status = false;
                EntidadResponseModels.result = null;

                JwtBuilder jwt = new JwtBuilder();
                EntidadResponseModels objEntidadResult = this.fn_Islogued(datos);

                if (objEntidadResult != null) {

                    EntidadRegistroToken datosToken = new EntidadRegistroToken();

                   
                    if (objEntidadResult.result != null) {

                        //generando token
                        String SessionToken = jwt.fn_GenerateToken(objEntidadResult.result);
                        
                        //guardadando datos 
                        datosToken.TokenStr = SessionToken;
                        datosToken.UserId = objEntidadResult.result;


                        //obj result para la funcion guardar
                        EntidadResponseModels objRes = new EntidadResponseModels();

                        objRes = crd_token.fn_SaveTokenToDatabase(datosToken);

                        if (objRes.status == true)
                        {
                            Console.WriteLine("guardando token");
                            EntidadResponseModels.status = true;
                            EntidadResponseModels.result = objRes.result;

                            return EntidadResponseModels;

                        }
                        else
                        {

                            Console.WriteLine("Retornando el token activo");
                            EntidadResponseModels.status = true;
                            EntidadResponseModels.result = objRes.result;
                            return EntidadResponseModels;

                        }

                        //fin guardar datos

                    } else {
                        return EntidadResponseModels;

                    }

                    //guardando el token a la db

                } else {
                    Console.WriteLine("no existe el usuario");
                    return EntidadResponseModels;
                }

             
            
            } catch (Exception e) 
            {
               
                return EntidadResponseModels;
            }
        
        }

        //funcion que hace la revocacion del token para que ya no sea valido 

        public EntidadResponseModels fn_RevocarToken(LoginRequestDTO datos)
        {
            crd_token = new CRD_Token();
            try
            {
                EntidadResponseModels = new EntidadResponseModels();
                EntidadResponseModels.status = false;
                EntidadResponseModels.result = null;

                JwtBuilder jwt = new JwtBuilder();
                EntidadResponseModels objEntidadResult = this.fn_Islogued(datos);

                if (objEntidadResult != null)
                {

                    EntidadRegistroToken datosToken = new EntidadRegistroToken();


                    if (objEntidadResult.result != null)
                    {

                        //generando token
                        String SessionToken = jwt.fn_GenerateToken(objEntidadResult.result);

                        //guardadando datos 
                        datosToken.TokenStr = SessionToken;
                        datosToken.UserId = objEntidadResult.result;


                        //obj result para la funcion guardar
                        EntidadResponseModels objRes = new EntidadResponseModels();

                        objRes = crd_token.fn_SaveTokenToDatabase(datosToken);

                        if (objRes.status == true)
                        {
                            Console.WriteLine("guardando token");
                            EntidadResponseModels.status = true;
                            EntidadResponseModels.result = objRes.result;

                            return EntidadResponseModels;

                        }
                        else
                        {

                            Console.WriteLine("Retornando el token activo");
                            EntidadResponseModels.status = true;
                            EntidadResponseModels.result = objRes.result;
                            return EntidadResponseModels;

                        }

                        //fin guardar datos

                    }
                    else
                    {
                        return EntidadResponseModels;

                    }

                    //guardando el token a la db

                }
                else
                {
                    Console.WriteLine("no existe el usuario");
                    return EntidadResponseModels;
                }



            }
            catch (Exception e)
            {

                return EntidadResponseModels;
            }

        }


        //fin clase
    }
}
