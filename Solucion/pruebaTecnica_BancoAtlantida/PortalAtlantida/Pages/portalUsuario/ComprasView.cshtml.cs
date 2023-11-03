using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;
using PortalAtlantida.ModelViews;
using PortalAtlantida.tools;
using System;
using System.Threading.Tasks;

namespace PortalAtlantida.Pages.portalUsuario
{
    public class ComprasViewModel : PageModel
    {
        private readonly ILogger<ComprasViewModel> _logger;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private int timeCookieExpire = 30;
        public decimal Saldo { get; set; }
        public CookiesBuider cbuilder { get; set; }


        //var de usuario 
        public RootObject datosApiRest = new RootObject();
        public String uname = null;

        private void fn_SetInitParameters() {
            this.uname = cbuilder.GetCookie("username");
            this.Saldo = decimal.Parse(cbuilder.GetCookie("saldo"));

        }   

        public ComprasViewModel(ILogger<ComprasViewModel> logger, IHttpContextAccessor httpContextAccessor)
        {
            _logger = logger;
            _httpContextAccessor = httpContextAccessor;
            cbuilder = new CookiesBuider(_httpContextAccessor);


            RoutesApi ra = new RoutesApi();

            this.fn_SetInitParameters();

        }

        public async Task OnGetAsync()
        {
            // Aquí puedes realizar cualquier otra tarea asincrónica que necesites
            
        }
    }
}
