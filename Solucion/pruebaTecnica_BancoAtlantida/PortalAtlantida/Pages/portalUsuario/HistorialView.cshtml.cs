using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;
using PortalAtlantida.Entities;
using PortalAtlantida.models;
using PortalAtlantida.ModelViews;
using PortalAtlantida.ModelViews.GeneralResponse;
using PortalAtlantida.ModelViews.historialCompra;
using PortalAtlantida.tools;
using System;
using System.Threading.Tasks;

namespace PortalAtlantida.Pages.portalUsuario
{
    public class HistorialViewModel : PageModel
    {
        private readonly ILogger<HistorialViewModel> _logger;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private int timeCookieExpire = 30;
        public decimal Saldo { get; set; }
        public CookiesBuider cbuilder { get; set; }

        String fechaInicioFormateada = null;
        String fechaFinFormateada = null;
        //var de usuario 
        public RootObject datosApiRest = new RootObject();
        public RtObjectHistorial<MVCompra> datosApiComprasRest = new RtObjectHistorial<MVCompra>();
        public RtObjectHistorial<MVHistorialCompras> datosApiComprasHist = new RtObjectHistorial<MVHistorialCompras>();
        public String uname = null;

        public HistorialViewModel(ILogger<HistorialViewModel> logger, IHttpContextAccessor httpContextAccessor)
        {
            _logger = logger;
            _httpContextAccessor = httpContextAccessor;
            cbuilder = new CookiesBuider(_httpContextAccessor);


            RoutesApi ra = new RoutesApi();
            // Variable con la fecha actual del mes (día 01)
            DateTime fechaInicioMes = new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1);

            // Variable con la fecha del final de mes
            DateTime fechaFinMes = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.DaysInMonth(DateTime.Now.Year, DateTime.Now.Month));

            this.fechaInicioFormateada = fechaInicioMes.ToString("yyyy-MM-dd");
            this.fechaFinFormateada = fechaFinMes.ToString("yyyy-MM-dd");

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


        protected async Task<bool> fn_GetHistorialCompras(HistorialByFechaEntidad datos)
        {
            bool response = false;
            HistorialComprasByFechaModel requestApi = new HistorialComprasByFechaModel();
            bool responseData = await requestApi.fn_GetComprasFilter(datos);
            RtObjectHistorial<MVHistorialCompras> datosApi = new RtObjectHistorial<MVHistorialCompras>();

            if (responseData == true)
            {
                datosApi = requestApi.GetDataAPI();
                datosApiComprasHist = datosApi;

                Console.WriteLine(datosApiComprasHist.datos.result.Count);
                response = true;
            }
            else
            {
                datosApiComprasHist = datosApi;
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

        public async Task fn_ComprasHistorial(String FechaIn, String fechaFin)
        {
            this.uname = cbuilder.GetCookie("username");
            String tk = cbuilder.GetCookie("token");
            if (this.uname == null && tk == null)
            {
                Response.Redirect("/");
            }
            else
            {  
                HistorialByFechaEntidad df = new HistorialByFechaEntidad();
                df.token = tk;
                df.fechaInicio = DateTime.Parse(FechaIn);
                df.fechaFinal = DateTime.Parse(fechaFin);


                if (await this.fn_GetHistorialCompras(df))
                {
                    Console.WriteLine("datos historial compras obtenidos");
                }
                else
                {
                    Console.WriteLine("No se pudo obtener datos historial compras");
                }
            }

        }

        private async Task fn_getHistorial() 
        {
            // Variable con la fecha actual del mes (día 01)
            DateTime fechaInicioMes = new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1);

            // Variable con la fecha del final de mes
            DateTime fechaFinMes = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.DaysInMonth(DateTime.Now.Year, DateTime.Now.Month));

            this.fechaInicioFormateada = fechaInicioMes.ToString("yyyy-MM-dd");
            this.fechaFinFormateada = fechaFinMes.ToString("yyyy-MM-dd");
            await fn_ComprasHistorial(fechaInicioFormateada, fechaFinFormateada);

        }

        public async Task<IActionResult> OnPostFiltroCompra(string fechaInicio, string fechaFinal)
        {
            // Procesar los datos del formulario y mostrar los resultados

            if (string.IsNullOrEmpty(fechaInicio) != null && string.IsNullOrEmpty(fechaFinal) != null)
            {
                this.fechaInicioFormateada = DateTime.Parse(fechaInicio).ToString("yyyy-MM-dd");
                this.fechaFinFormateada = DateTime.Parse(fechaFinal).ToString("yyyy-MM-dd");
                await fn_ComprasHistorial(fechaInicioFormateada, fechaFinFormateada);
            }
            else {
                fn_getHistorial();
            }

            TempData["MensajeToast"] = "";
          //  await fn_ReCompra();
            await fn_rec();
           
            return Page();
        }


        public async Task<IActionResult> OnGetAsync()
        {
           


            TempData["MensajeToast"] = "";
            ///await fn_ReCompra();
            await fn_ComprasHistorial(fechaInicioFormateada, fechaFinFormateada);
            await fn_rec();
            //await fn_getHistorial();

            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            await fn_rec();
            // Aquí se procesa la información del formulario
            return Page();
        }


    }
}
