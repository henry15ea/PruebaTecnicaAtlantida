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
    public class HomeUserModel : PageModel
    {
        private readonly ILogger<HomeUserModel> _logger;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private int timeCookieExpire = 30;
        public decimal Saldo { get; set; }
        public CookiesBuider cbuilder { get; set; }


        //var de usuario 
        public RootObject datosApiRest = new RootObject();
        public RtObjectHistorial<MVCompra> datosApiComprasRest = new RtObjectHistorial<MVCompra>();

        public String uname = null;

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
                TempData["MensajeToast"] = "Clave o usuario no valido";
            }


            return response;

        }

         public IActionResult OnPostLogout()
        {
            // Lógica para cerrar sesión aquí
            // Por ejemplo, puedes invalidar la sesión actual y redirigir al usuario a la página de inicio de sesión
            Console.WriteLine("has cerrado sesion");
            return Redirect("/");
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

        protected async Task<bool> fn_NewCompra(CompraEntidad datos)
        {
            bool response = false;
            NuevaCompraModel requestApi = new NuevaCompraModel();
            bool responseData = await requestApi.fn_AgregaCompra(datos);
            StandarObjectResponse datosApi = new StandarObjectResponse();

            Console.WriteLine("peticion a api compras" + responseData);

            if (responseData == true)
            {
                datosApi = requestApi.GetDataAPI();
               
                Console.WriteLine(datosApi.datos);
                response = true;
            }
            else
            {
                response = false;
            }
            return response;
        }

        protected async Task<bool> fn_NewPago(PagoEntidad datos)
        {
            bool response = false;
            NuevoPagoModel requestApi = new NuevoPagoModel();
            bool responseData = await requestApi.fn_NuevoPago(datos);
            StandarObjectResponse datosApi = new StandarObjectResponse();

            Console.WriteLine("peticion a api pagos" + responseData);

            if (responseData == true)
            {
                response = true;
            }
            else
            {
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
                    Console.WriteLine("datos obtenidos");
                }
                else
                {
                    Console.WriteLine("No se pudo obtener datos");
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

        public async Task fn_NwCompra(CompraEntidad datos)
        {
            this.uname = cbuilder.GetCookie("username");
            String tk = cbuilder.GetCookie("token");
            if (this.uname == null && tk == null)
            {
                Response.Redirect("/");
            }
            else
            {
                if (await this.fn_NewCompra(datos))
                {
                    Console.WriteLine("datos compras obtenidos");
                }
                else
                {
                    Console.WriteLine("No se pudo obtener datos compras");
                }
            }

        }


        public async Task fn_NwPago(PagoEntidad datos)
        {
            this.uname = cbuilder.GetCookie("username");
            String tk = cbuilder.GetCookie("token");
            if (this.uname == null && tk == null)
            {
                Response.Redirect("/");
            }
            else
            {
                if (await this.fn_NewPago(datos))
                {
                    Console.WriteLine("datos compras obtenidos");
                }
                else
                {
                    Console.WriteLine("No se pudo obtener datos compras");
                }
            }

        }

        public HomeUserModel(ILogger<HomeUserModel> logger, IHttpContextAccessor httpContextAccessor)
        {
            _logger = logger;
            _httpContextAccessor = httpContextAccessor;
            cbuilder = new CookiesBuider(_httpContextAccessor);
           

            RoutesApi ra = new RoutesApi();


        }

        public async Task<IActionResult> OnGetAsync()
        {
            TempData["MensajeToast"] = "";
            await fn_ReCompra();
            await fn_rec();

            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {

            string montoPago = Request.Form["montoPago"];

            // Hacer algo con el valor del input
            Console.WriteLine("pago recibido onpost home : " + montoPago);
            TempData["MensajeToast"] = "demo";

            await fn_rec();
            // Aquí se procesa la información del formulario
            return Page();
        }

        public async Task<IActionResult> OnPostPagoFacturaAsync(string fechaPago, decimal montoPago)
        {

            if (fechaPago != null && montoPago > 0)
            {
                
                String tk = cbuilder.GetCookie("token");
                PagoEntidad pme = new PagoEntidad();
                DateTime fecha = DateTime.Parse(fechaPago);
                string fechaFormateada = fecha.ToString("yyyy-MM-dd");
               
                
                pme.token = tk;
                pme.fechaPago = DateTime.Parse(fechaFormateada);
                pme.montoPago = montoPago;
                await this.fn_NewPago(pme);
                TempData["MensajeToast"] = "Se ha realizado el pago";
                


            }
            else
            {
                TempData["MensajeToast"] = "Los datos introducidos no son validos";
            }
            // Aquí se procesa la información del formulario
            await fn_ReCompra();
            await fn_rec();
            return Page();
        }

        public async Task<IActionResult> OnPostCompraProductoAsync(string fechaCompra, String descripcionCompra, decimal montoCompra)
        {
            
            if (fechaCompra != null && montoCompra > 0 && descripcionCompra !=null || descripcionCompra !="")
            {
                String tk = cbuilder.GetCookie("token");
                CompraEntidad cme = new CompraEntidad();
                DateTime fecha = DateTime.Parse(fechaCompra);
                string fechaFormateada = fecha.ToString("yyyy-MM-dd");

                cme.token = tk;
                cme.fechaCompra = DateTime.Parse(fechaFormateada);
                cme.descripcionCompta = descripcionCompra.ToUpper();
                cme.montoCompra = montoCompra;
                await fn_NwCompra(cme);
                TempData["MensajeToast"] = "Se ha realizado la compra";

            }
            else
            {
                TempData["MensajeToast"] = "Los datos introducidos no son validos";
            }
            // Aquí se procesa la información del formulario
            await fn_ReCompra();
            await fn_rec();
            return Page();
        }




    }
}
