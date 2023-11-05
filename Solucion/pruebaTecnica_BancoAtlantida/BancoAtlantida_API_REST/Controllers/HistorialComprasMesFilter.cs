using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using System.Data.SqlClient;
using Newtonsoft.Json;
using BancoAtlantida_API_REST.DTOS;
using BancoAtlantida_API_REST.Models;
using BancoAtlantida_API_REST.tools;
using AutoMapper;
using BancoAtlantida_API_REST.Models.requestEntidad;
using BancoAtlantida_API_REST.Entidades;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System;
using BancoAtlantida_API_REST.microservices.VerifyToken;
using BancoAtlantida_API_REST.microservices.Entidades;

namespace BancoAtlantida_API_REST.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class HistorialComprasMesFilter : Controller
    {
        private readonly IMapper mapper;

        public HistorialComprasMesFilter(IMapper mapper)
        {
            this.mapper = mapper;
        }
        [HttpGet]
        public async Task<IActionResult> IndexAsync([FromQuery(Name = "fechaInicio")] DateTime fechaInicio,
                            [FromQuery(Name = "fechaFinal")] DateTime fechaFinal)
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
                                cfg.CreateMap<HistorialCompraByMesFilterDTO, HistorialComprasByFechaEntidad>()
                                    .ForMember(dest => dest.IdCuenta, opt => opt.MapFrom(src => authValue));
                            });

                            var mapper = config.CreateMapper();

                            var entidadCompleta = new HistorialComprasByFechaEntidad();
                            entidadCompleta.IdCuenta = authValue;
                            entidadCompleta.FechaInicio = fechaInicio;
                            entidadCompleta.FechaFinal = fechaFinal;

                           // HistorialComprasByFechaEntidad entidadCompra = mapper.Map<HistorialCompraByMesFilterDTO, HistorialComprasByFechaEntidad>(datos);

                            //creamos el objeto para obtener las funciones del modelo
                            HistorialCompraMesFilterModel modeloRespuesta = new HistorialCompraMesFilterModel();

                            
                            //buscamos los resultados 
                            var resultadoInfo = modeloRespuesta.fn_HistrialCompraDosMeses(entidadCompleta);

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
        //end index
    }
    //end class
}
