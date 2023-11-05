using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using ServiceToken.DTO;
using ServiceToken.Entidades.Data;
using ServiceToken.models;
using ServiceToken.Models;
using ServiceToken.tools;

namespace ServiceToken.Controllers
{
    [Route("services/[controller]")]
    [ApiController]
    public class TokenController : Controller
    {
        //inyectando automapper 
        private readonly IMapper mapper;
        public TokenController(IMapper mapper)
        {
            this.mapper = mapper;
        }




        [HttpPost("login")]
        public IActionResult Index([FromBody] LoginRequestDTO datos)
        {
            LoginResponseDTO rp = new LoginResponseDTO();

            //verificamos si el usuario es correcto en caso contrario informamos al usuario
            LoginModel loginModel = new LoginModel();

            EntidadResponseModels objEntidadResult = new EntidadResponseModels();

            objEntidadResult = loginModel.fn_AutorizarLogin(datos);

            rp.status = objEntidadResult.status;


            if (rp.status == true)
            {



                rp.AccessToken = objEntidadResult.result;

                Console.Out.WriteLine(rp);

                return StatusCode(200, rp);

            }
            else
            {
                rp.status = false;
                rp.AccessToken = null;  
                return StatusCode(404, rp);
            }

        }

        [HttpGet("verify")]
        public IActionResult verifyToken()
        {
            //[FromBody] EntidadDefecto datos
            var tokenHeader = HttpContext.Request.Headers["Authorization"];

            LoginResponseDTO rp = new LoginResponseDTO();
            JwtBuilder objwt = new JwtBuilder();

            //verificamos si el usuario es correcto en caso contrario informamos al usuario
            CRD_Token Model = new CRD_Token();

            EntidadResponseModels objEntidadResult = new EntidadResponseModels();


            if (tokenHeader.ToString().Length > 0)
            {

                EntidadDefecto datos = new EntidadDefecto();


                var datosToken = objwt.fn_VerificarToken(tokenHeader.ToString().Trim());

                var claimsPrincipal = datosToken;
                try
                {
                    datos.Token = tokenHeader.ToString().Trim();



                    if (objwt.fn_esLegibleToken(tokenHeader.ToString().Trim()) == false)
                    {
                        rp.AccessToken = null;
                        rp.status = false;
                    }
                    else
                    {
                        var authClaim = claimsPrincipal.Claims.FirstOrDefault(c => c.Type == "http://schemas.xmlsoap.org/ws/2005/05/identity/claims/authentication");
                        string? authValue = authClaim?.Value;

                        if (authValue != null)
                        {
                            //el token es valido y se ha encontrado el id necesario

                            objEntidadResult = Model.fn_ReadTokenDatabase(datos);
                            //buscamos los resultados 
                            EntidadTokenResponse data = new EntidadTokenResponse();

                            data = objEntidadResult.result;

                            if (data.Revoked == false) {
                                rp.AccessToken = data.Token.ToString();
                                rp.status = true;
                            } else {
                                rp.AccessToken = null;
                                rp.status = false;
                            }
                        }
                        else
                        {
                            rp.AccessToken = null;
                            rp.status = false;
                        }

                    }

                }
                catch (Exception)
                {
                    rp.AccessToken = null;
                    rp.status = false;
                }


                return StatusCode(200, rp);
            }
            else
            {
                return StatusCode(404, rp);
            }

        }

        [HttpGet("refresh")]
        public IActionResult refreshToken()
        {
            //[FromBody] EntidadDefecto datos
            var tokenHeader = HttpContext.Request.Headers["Authorization"];

            LoginResponseDTO rp = new LoginResponseDTO();
            JwtBuilder objwt = new JwtBuilder();

            //verificamos si el usuario es correcto en caso contrario informamos al usuario
            CRD_Token Model = new CRD_Token();

            EntidadResponseModels objEntidadResult = new EntidadResponseModels();


            if (tokenHeader.ToString().Length > 0)
            {

                EntidadDefecto datos = new EntidadDefecto();

                var datosToken = objwt.fn_VerificarToken(tokenHeader.ToString().Trim());

                var claimsPrincipal = datosToken;
                try
                {
                    datos.Token = tokenHeader.ToString().Trim();

                    if (objwt.fn_esLegibleToken(tokenHeader.ToString().Trim()) == false)
                    {
                        rp.AccessToken = null;
                        rp.status = false;
                    }
                    else
                    {
                        var authClaim = claimsPrincipal.Claims.FirstOrDefault(c => c.Type == "http://schemas.xmlsoap.org/ws/2005/05/identity/claims/authentication");
                        string? authValue = authClaim?.Value;

                        if (authValue != null)
                        {
                            //el token es valido y se ha encontrado el id necesario

                            objEntidadResult = Model.fn_ReadTokenDatabase(datos);
                            //buscamos los resultados 
                            EntidadTokenResponse data = new EntidadTokenResponse();

                            data = objEntidadResult.result;
                            DateTime now = DateTime.UtcNow.AddMinutes(JwtBuilder.ExpirationToken);
                            Console.WriteLine("ahora >>" + now);
                            Console.WriteLine("expira >>" + data.ExpirationTime);


                            if (data.Revoked == false && now > data.ExpirationTime )
                            {
                                //se renueva el token nuevo 
                                EntidadRegistroToken datosTokenUpdate = new EntidadRegistroToken();
                                datosTokenUpdate.TokenStr = data.Token;
                                datosTokenUpdate.ExpirationTime = data.ExpirationTime;
                                datosTokenUpdate.IssuedTime = data.IssuedTime;
                                datosTokenUpdate.TokenType = data.TokenType;
                                datosTokenUpdate.Revoked = true;


                                if (Model.fn_UpdateTokenToDatabase(datosTokenUpdate).status == true) 
                                {
                                    EntidadRegistroToken tokenNuevo = new EntidadRegistroToken();
                                    String tokn = objwt.fn_GenerateCustomToken(data.UserId.Trim());
                                    Console.WriteLine("nuevo token >> "+tokn);


                                    tokenNuevo.TokenStr = tokn;
                                    tokenNuevo.ExpirationTime = now;
                                    tokenNuevo.UserId = data.UserId;


                                    EntidadResponseModels rps = new EntidadResponseModels();
                                    rps = !Model.fn_SaveTokenToDatabase(tokenNuevo).result;

                                    if (rps != null)
                                    {
                                        rp.AccessToken = rps.result;
                                        rp.status = rps.status;
                                    }
                                    else {
                                        rp.AccessToken = tokenHeader.ToString().Trim();
                                        rp.status = false;
                                    }
                                    

                                } 
                                else 
                                {
                                    rp.AccessToken = tokenHeader.ToString().Trim();
                                    rp.status = false ;
                                }
                                
                            }
                            else
                            {
                                rp.AccessToken = tokenHeader.ToString().Trim();
                                rp.status = false;
                            }
                        }
                        else
                        {
                            rp.AccessToken = tokenHeader.ToString().Trim();
                            rp.status = false;
                        }

                    }

                }
                catch (Exception)
                {
                    rp.AccessToken = tokenHeader.ToString().Trim();
                    rp.status = false;
                }


                return StatusCode(200, rp);
            }
            else
            {
                return StatusCode(404, rp);
            }

        }

        //end functions
    }

    //end class
}
//end namespaces