using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;
using PortalAtlantida.Entities;
using PortalAtlantida.models;
using PortalAtlantida.ModelViews;
using PortalAtlantida.ModelViews.historialCompra;
using PortalAtlantida.tools;
using System;
using System.Threading.Tasks;

namespace PortalAtlantida.Pages.portalUsuario
{
    public class EstadoCuentaViewModel : PageModel
    {
        private readonly ILogger<EstadoCuentaViewModel> _logger;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private int timeCookieExpire = 30;
        public decimal Saldo { get; set; }
        public CookiesBuider cbuilder { get; set; }


        //var de usuario 
        public RootObject datosApiRest = new RootObject();
        public RtObjectHistorial<MVCompra> datosApiComprasRest = new RtObjectHistorial<MVCompra>();
        public String uname = null;
        public EstadoCuentaViewModel(ILogger<EstadoCuentaViewModel> logger, IHttpContextAccessor httpContextAccessor)
        {
            _logger = logger;
            _httpContextAccessor = httpContextAccessor;
            cbuilder = new CookiesBuider(_httpContextAccessor);


            RoutesApi ra = new RoutesApi();


        }

        protected async Task<bool> fn_GetUserData(DefaultEntidad datos)
        {
            bool response = false;
            InfoUsuarioModel requestApi = new InfoUsuarioModel();
            bool responseData = await requestApi.fn_InfoUser(datos);
            RootObject datosApi = new RootObject();

            if (responseData == true)
            {
                datosApi = requestApi.GetDataAPI();
                datosApiRest = datosApi;
                Saldo = (decimal)datosApi.datos.result[0].saldoDisponible;
                cbuilder.SetCookie("saldo", Saldo.ToString(), timeCookieExpire);
                response = true;
            }
            else
            {
                datosApiRest = datosApi;
                response = false;
            }
            return response;
        }


        protected async Task<bool> fn_GetUCompras(DefaultEntidad datos)
        {
            bool response = false;
            HistorialComprasAllModel requestApi = new HistorialComprasAllModel();
            bool responseData = await requestApi.fn_GetComprasAll(datos);
            RtObjectHistorial<MVCompra> datosApi = new RtObjectHistorial<MVCompra>();

            Console.WriteLine("peticion a api compras" + responseData);

            if (responseData == true)
            {
                datosApi = requestApi.GetDataAPI();
                datosApiComprasRest = datosApi;
                Console.WriteLine(datosApi.datos.result[0].fechaCompra);
                response = true;
            }
            else
            {
                datosApiComprasRest = datosApi;
                response = false;
            }
            return response;
        }

        public async Task fn_rec()
        {
            this.uname = cbuilder.GetCookie("username");
            String tk = cbuilder.GetCookie("token");
            if (this.uname == null && tk == null)
            {
                Response.Redirect("/");
            }
            else
            {
                DefaultEntidad df = new DefaultEntidad();
                df.token = tk;

                if (await this.fn_GetUserData(df))
                {
                    Console.WriteLine("datos obtenidos usuario");
                }
                else
                {
                    Console.WriteLine("No se pudo obtener datos usuario");
                }
            }

        }


        public async Task fn_ReCompra()
        {
            this.uname = cbuilder.GetCookie("username");
            String tk = cbuilder.GetCookie("token");
            if (this.uname == null && tk == null)
            {
                Response.Redirect("/");
            }
            else
            {
                DefaultEntidad df = new DefaultEntidad();
                df.token = tk;

                if (await this.fn_GetUCompras(df))
                {
                    Console.WriteLine("datos compras obtenidos");
                }
                else
                {
                    Console.WriteLine("No se pudo obtener datos compras");
                }
            }

        }

        public async Task<IActionResult> OnGetAsync()
        {
            TempData["MensajeToast"] = "get";
            await fn_ReCompra();
            await fn_rec();

            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {

            string montoPago = Request.Form["montoPago"];

            // Hacer algo con el valor del input
            Console.WriteLine("pago recibido desde layout: " + montoPago);
            TempData["MensajeToast"] = "demo";

            await fn_rec();
            // Aquí se procesa la información del formulario
            return Page();
        }


    }
}
