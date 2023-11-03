using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;
using PortalAtlantida.ModelViews;
using PortalAtlantida.ModelViews.historialCompra;
using PortalAtlantida.tools;
using System;

namespace PortalAtlantida.Pages.portalUsuario
{
    public class CerrarSessionModel : PageModel
    {
        private readonly ILogger<CerrarSessionModel> _logger;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private int timeCookieExpire = 30;
        public decimal Saldo { get; set; }
        public CookiesBuider cbuilder { get; set; }


        //var de usuario 
        public RootObject datosApiRest = new RootObject();
        public RtObjectHistorial<MVCompra> datosApiComprasRest = new RtObjectHistorial<MVCompra>();

        public String uname = null;

        public CerrarSessionModel(ILogger<CerrarSessionModel> logger, IHttpContextAccessor httpContextAccessor)
        {
            _logger = logger;
            _httpContextAccessor = httpContextAccessor;
            cbuilder = new CookiesBuider(_httpContextAccessor);


            RoutesApi ra = new RoutesApi();


        }

        public IActionResult fn_redirect()
        {
            cbuilder.RemoveCookie("token");
            cbuilder.RemoveCookie("username");
            return RedirectToPage("/Index");
        }

        public IActionResult OnGet()
        {
            Console.WriteLine("entraste");
            cbuilder.RemoveCookie("token");
            cbuilder.RemoveCookie("username");
            return RedirectToPage("/Index"); // redireccionar a la página de inicio
        }
    }
}
