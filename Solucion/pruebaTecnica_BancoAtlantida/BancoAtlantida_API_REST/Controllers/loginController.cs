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

namespace BancoAtlantida_API_REST.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class loginController : Controller
    {
        private readonly IMapper mapper;
        public loginController(IMapper mapper)
        {
            this.mapper = mapper;
        }


        [HttpPost]
        public IActionResult Index([FromBody] LoginDTO datos)
        {
            ResponseEntidad rp = new ResponseEntidad();

            //verificamos si el usuario es correcto en caso contrario informamos al usuario
            LoginModel loginModel = new LoginModel();

            RequestModelEntidad objEntidadResult = new RequestModelEntidad();

            objEntidadResult = loginModel.fn_Islogued(datos);

            rp.resultado = objEntidadResult.status;

            if (objEntidadResult.status == true) {

                rp.mensajeServidor = "Las peticiones se han completado con exito";
                rp.codigo = 200;
                rp.mensaje = "El usuario existe en la db";

                //generamos el token para el usuario que ha iniciado sesion
                JwtFabric jwt = new JwtFabric();

                String data = (string)objEntidadResult.result;


                rp.token = jwt.fn_GenerateToken(data.Trim());



                return StatusCode(200, rp);

            } else {
                rp.mensajeServidor = "Datos de acceso invalidos para el usuario proporcionado";
                rp.codigo = 404;
                rp.mensaje = "Usuario o clave incorrecta , ingresa nuevamente los datos";
                return StatusCode(404, rp);
            }

        }
    }
}
