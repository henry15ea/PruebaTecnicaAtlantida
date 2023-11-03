using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;
using PortalAtlantida.Entities;
using PortalAtlantida.models;
using PortalAtlantida.ModelViews;
using PortalAtlantida.tools;
using System.Threading.Tasks;

namespace PortalAtlantida.Pages
{
    public class IndexModel : PageModel
    {
        private readonly ILogger<IndexModel> _logger;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private int timeCookieExpire = 30;

        public CookiesBuider cbuilder { get; set; }
        RoutesApi ra = null;
        public IndexModel(ILogger<IndexModel> logger, IHttpContextAccessor httpContextAccessor)
        {
            _logger = logger;
            _httpContextAccessor = httpContextAccessor;
            cbuilder = new CookiesBuider(_httpContextAccessor);
            ra = new RoutesApi();

            
        }
        //funciones de clase 
        protected async Task<bool> fn_IsValidUser(UsuarioClavesEntidad datos) {
            bool response = false;
            LoginModel requestApi = new LoginModel();

            bool responseData = await  requestApi.fn_loginUser(datos);
            MVLoginResult rs = new MVLoginResult();

            if (responseData == true) {
               
                rs = requestApi.GetDataAPI();
                TempData["MensajeToast"] = rs.mensaje;

                //guardando datos del usuario en las cookies para usarlas luego
                cbuilder.SetCookie("username", datos.username, timeCookieExpire);
                cbuilder.SetCookie("token", rs.token , timeCookieExpire);

                response = true;
            } else {
                TempData["MensajeToast"] = "";
                cbuilder.SetCookie("username", "null", timeCookieExpire);
                cbuilder.SetCookie("token", "null", timeCookieExpire);
                response = false;
                TempData["MensajeToast"] = "Clave o usuario no valido";
            }


            return response;
        
        }

        //funciones de accion de pagina 
        public void OnGet()
        {
            TempData["MensajeToast"] = "";

            
        }
        public async Task<IActionResult> OnPost()
        {
            string uname = Request.Form["username"];
            string upass = Request.Form["password"];
            TempData["MensajeToast"] = "";

           
            if (string.IsNullOrEmpty(uname) || string.IsNullOrEmpty(upass))
            {
                // Las variables son nulas o están vacías
                // Realiza la lógica correspondiente aquí
            }
            else
            {
                // Las variables tienen valores
                // Realiza la lógica correspondiente aquí
                
                UsuarioClavesEntidad df = new UsuarioClavesEntidad();
                df.username = uname.Trim();
                df.password = upass.Trim();

                

                if (await this.fn_IsValidUser(df)) {
                    

                    return Redirect("/portalUsuario/HomeUser");
                } else {
                    return Redirect("/");
                }
            }

            

            // realizar la autenticación con los valores obtenidos
            return Page();
        }

        //end methods
    }
    //end class
}
//end namespaces