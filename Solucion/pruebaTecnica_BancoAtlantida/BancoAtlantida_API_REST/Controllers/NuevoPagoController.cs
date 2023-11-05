using AutoMapper;
using BancoAtlantida_API_REST.DTOS;
using BancoAtlantida_API_REST.Entidades;
using BancoAtlantida_API_REST.microservices.Entidades;
using BancoAtlantida_API_REST.microservices.VerifyToken;
using BancoAtlantida_API_REST.Models;
using BancoAtlantida_API_REST.tools;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Linq;

namespace BancoAtlantida_API_REST.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class NuevoPagoController : Controller
    {
        private readonly IMapper mapper;
        public NuevoPagoController(IMapper mapper)
        {
            this.mapper = mapper;
        }

        [HttpPost]
        public async Task<IActionResult> IndexAsync([FromBody] AgregarPagoDTO datos)
        {
            //leyendo token desde el header 

            var tokenHeader = HttpContext.Request.Headers["Authorization"];

            JwtFabric objwt = new JwtFabric();
            ResponseEntidad rp = new ResponseEntidad();

            if (tokenHeader.ToString().Length > 0)
            {

                VerifyTokenCliente verifyToken = new VerifyTokenCliente();
                LoginResponseEntidad entidadResponse = new LoginResponseEntidad();
                MicroDefaultEntidad tdata = new MicroDefaultEntidad();

                tdata.token = tokenHeader;

                bool resp = await verifyToken.fn_VerifyToken(tdata);
                entidadResponse = verifyToken.GetDataAPI();

                
                try
                {
                    if (objwt.fn_esLegibleToken(tokenHeader.ToString().Trim()) == false)
                    {
                        rp.mensajeServidor = "El token adquirido no es valido o ya ha caducado";
                        rp.mensaje = "El token adquirido no es valido o ya ha caducado";
                        rp.resultado = false;
                    }
                    else
                    {
                        var datosToken = objwt.fn_VerificarToken(entidadResponse.AccessToken.Trim());

                        var claimsPrincipal = datosToken;

                        var authClaim = claimsPrincipal.Claims.FirstOrDefault(c => c.Type == "http://schemas.xmlsoap.org/ws/2005/05/identity/claims/authentication");
                        string? authValue = authClaim?.Value;

                        if (authValue != null)
                        {
                            //el token es valido y se ha encontrado el id necesario
                            // Convertir objeto DTO en entidad utilizando AutoMapper

                            var config = new MapperConfiguration(cfg => {
                                cfg.CreateMap<AgregarPagoDTO, AgregarPagoEntidad>()
                                    .ForMember(dest => dest.id_cuenta, opt => opt.MapFrom(src => authValue));
                            });

                            var mapper = config.CreateMapper();

                            var entidadCompleta = new AgregarPagoEntidad();
                            entidadCompleta.id_cuenta = authValue;

                            AgregarPagoEntidad entidadCompra = mapper.Map<AgregarPagoDTO, AgregarPagoEntidad>(datos);

                            //creamos el objeto para obtener las funciones del modelo
                            PagoNuevoModel modeloRespuesta = new PagoNuevoModel();


                            //buscamos los resultados 
                            var resultadoInfo = modeloRespuesta.fn_AgregarNuevoPago(entidadCompra);

                            rp.mensajeServidor = "Se han ejecutado los procesos completamente";
                            rp.codigo = 200;
                            rp.mensaje = "Datos obtenidos exitosamente!";
                            //rp.token = tokenHeader.ToString().Trim();
                            rp.resultado = true;
                            rp.datos = resultadoInfo;
                        }
                        else
                        {
                            rp.mensajeServidor = "El token adquirido no es valido o ya ha caducado";
                            rp.mensaje = "El token adquirido no es valido o ya ha caducado";
                            rp.resultado = false;
                        }

                    }

                }
                catch (Exception)
                {
                    rp.mensajeServidor = "El token adquirido no es valido o ya ha caducado";
                    rp.mensaje = "El token adquirido no es valido o ya ha caducado";
                    rp.resultado = false;
                }


                return StatusCode(200, rp);
            }
            else
            {
                return StatusCode(404, rp);
            }
        }

        //end controller routes
    }
    //end class
}
