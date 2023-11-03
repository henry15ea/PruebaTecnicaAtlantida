using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using System.Data.SqlClient;
using Newtonsoft.Json;
using System;
using BancoAtlantida_API_REST.DTOS;
using BancoAtlantida_API_REST.Models;
using BancoAtlantida_API_REST.tools;
using AutoMapper;
using BancoAtlantida_API_REST.Models.requestEntidad;
using BancoAtlantida_API_REST.Entidades;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;

namespace BancoAtlantida_API_REST.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class HistorialCompasMesesController : Controller
    {
        private readonly IMapper mapper;
        public HistorialCompasMesesController(IMapper mapper)
        {
            this.mapper = mapper;
        }

        [HttpGet]
        public IActionResult Index()
        {
            //leyendo token desde el header 

            var tokenHeader = HttpContext.Request.Headers["Authorization"];

            JwtFabric objwt = new JwtFabric();
            ResponseEntidad rp = new ResponseEntidad();

            if (tokenHeader.ToString().Length > 0)
            {
                var datosToken = objwt.fn_VerificarToken(tokenHeader.ToString().Trim());

                var claimsPrincipal = datosToken;
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
                        var authClaim = claimsPrincipal.Claims.FirstOrDefault(c => c.Type == "http://schemas.xmlsoap.org/ws/2005/05/identity/claims/authentication");
                        string? authValue = authClaim?.Value;

                        if (authValue != null)
                        {
                            //el token es valido y se ha encontrado el id necesario
                            // Convertir objeto DTO en entidad utilizando AutoMapper
                           
                            var config = new MapperConfiguration(cfg => {
                                cfg.CreateMap<HistorialCompraByMesDTO, HistorialComprasByFechaEntidad>()
                                    .ForMember(dest => dest.IdCuenta, opt => opt.MapFrom(src => authValue));
                            });

                            var mapper = config.CreateMapper();

                            var entidadCompleta = new HistorialComprasByFechaEntidad();
                            entidadCompleta.IdCuenta = authValue;

                            Console.WriteLine("usuario ID >>" + entidadCompleta.IdCuenta);

                            //HistorialComprasByFechaEntidad entidadCompra = mapper.Map<HistorialCompraByMesDTO, HistorialComprasByFechaEntidad>(datos);

                            //creamos el objeto para obtener las funciones del modelo
                            HistorialComprasMesModel modeloRespuesta = new HistorialComprasMesModel();

                            
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


    }
    //end class
}
