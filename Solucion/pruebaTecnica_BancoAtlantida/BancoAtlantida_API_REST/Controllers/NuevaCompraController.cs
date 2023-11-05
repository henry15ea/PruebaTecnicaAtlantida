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
    public class NuevaCompraController : Controller
    {
        private readonly IMapper mapper;
        public NuevaCompraController(IMapper mapper)
        {
            this.mapper = mapper;
        }

        [HttpPost]
        public async Task<IActionResult> IndexAsync([FromBody] AgregarNuevaCompraDTO datos)
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
                    if (entidadResponse.status == false)
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
                                cfg.CreateMap<AgregarNuevaCompraDTO, AgregarNuevaCompraEntidad>()
                                    .ForMember(dest => dest.IdCuenta, opt => opt.MapFrom(src => authValue));
                            });

                            var mapper = config.CreateMapper();

                            var entidadCompleta = new HistorialComprasByFechaEntidad();
                            entidadCompleta.IdCuenta = authValue;

                            AgregarNuevaCompraEntidad entidadCompra = mapper.Map<AgregarNuevaCompraDTO, AgregarNuevaCompraEntidad>(datos);

                            //creamos el objeto para obtener las funciones del modelo
                            CompraNuevaModel modeloRespuesta = new CompraNuevaModel();


                            //buscamos los resultados 
                            var resultadoInfo = modeloRespuesta.fn_AgregarNuevaCompra(entidadCompra);

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

        //end methods
    }
    //end class
}
