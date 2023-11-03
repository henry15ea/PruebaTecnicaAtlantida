using Microsoft.AspNetCore.Http;
using System;

namespace PortalAtlantida.tools
{
    public class CookiesBuider
    {
        private readonly IHttpContextAccessor _httpContextAccessor;

        public CookiesBuider(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
        }

        public void SetCookie(string key, string value, int? expirationTime)
        {
            CookieOptions option = new CookieOptions();

            if (expirationTime.HasValue)
                option.Expires = DateTime.Now.AddMinutes(expirationTime.Value);
            else
                option.Expires = DateTime.Now.AddMinutes(30);

            option.HttpOnly = true;

            _httpContextAccessor.HttpContext.Response.Cookies.Append(key, value, option);
        }

        public void RemoveCookie(string key)
        {
            _httpContextAccessor.HttpContext.Response.Cookies.Delete(key);
        }

        public string GetCookie(string key)
        {
            return _httpContextAccessor.HttpContext.Request.Cookies[key];
        }

        public bool IsCookieExists(string key)
        {
            return _httpContextAccessor.HttpContext.Request.Cookies.ContainsKey(key);
        }

        //end methods
    }
    //end class
}
//end namespaces
