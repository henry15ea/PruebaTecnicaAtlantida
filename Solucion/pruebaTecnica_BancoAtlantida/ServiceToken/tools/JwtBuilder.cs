using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.IdentityModel.Tokens;

namespace ServiceToken.tools
{
    public class JwtBuilder
    {
        static private readonly string _claveSecretaToken = "D91C9A3FA58F99AFFD6B845BEADDDF9965A7358860D80155E3F8E2D6A58ECBAA";
        static private readonly string _emisorToken = "henry15ea";
        static private readonly int _expiracionMiinutosToken = 260;

        public static int ExpirationToken { get; } = _expiracionMiinutosToken;
        public static DateTime emitedToken { get; } = DateTime.UtcNow;

        //funcion que recive una identificacion como id de cuenta , para posteriormente generar el JWT token 
        //con datos relacionados a la sesion 
        public string fn_GenerateToken(string userId)
        {
            var TokenIDGen = Guid.NewGuid().ToString();
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(_claveSecretaToken);
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new[]
                {
                 new Claim(ClaimTypes.Authentication, userId),
                new Claim("tokenId", TokenIDGen)
            }),
                Expires = DateTime.UtcNow.AddMinutes(_expiracionMiinutosToken),
                Issuer = _emisorToken,
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };

            var token = tokenHandler.CreateToken(tokenDescriptor);
            return tokenHandler.WriteToken(token);
        }

        //esta funcion ayuda a crear un token basandose en el uuid para crear un token nuevo

        public string fn_GenerateCustomToken(string userId)
        {
            var TokenIDGen = Guid.NewGuid().ToString();
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(_claveSecretaToken);
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new[]
                {
                new Claim(ClaimTypes.Authentication, userId),
                new Claim("tokenId", TokenIDGen)
            }),
               
                Expires = DateTime.UtcNow.AddMinutes(_expiracionMiinutosToken),
                Issuer = _emisorToken,
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };

            var token = tokenHandler.CreateToken(tokenDescriptor);
            return tokenHandler.WriteToken(token);
        }


        //funcion que verifica si el token es valido y puede ser leido o es legible , retorna true si lo es y se puede 
        //leer y caso contrario que se haya adulterado el token entonces retorna false
        public bool fn_esLegibleToken(string token)
        {
            var jwtSecurityTokenHandler = new JwtSecurityTokenHandler();
            var validationParameters = new TokenValidationParameters
            {
                ValidateAudience = false,
                ValidateIssuer = false,
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_claveSecretaToken)),
                ValidateLifetime = true
            };

            var isTokenValid = false;
            SecurityToken validatedToken = null;

            try
            {
                var claimsPrincipal = jwtSecurityTokenHandler.ValidateToken(token, validationParameters, out validatedToken);
                isTokenValid = claimsPrincipal != null;
            }
            catch (Exception)
            {
                isTokenValid = false;
            }

            return isTokenValid;
        }


        //funcion que se encarga de leer el token para extraer los datos dentro de el , retorna un Claims para 
        //posteriormente ser usado en las peticiones dentro de la api

        public ClaimsPrincipal fn_VerificarToken(string token)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(_claveSecretaToken);

            var tokenValidationParameters = new TokenValidationParameters
            {
                ValidateIssuer = true,
                ValidateAudience = false,
                ValidateLifetime = true,
                ValidateIssuerSigningKey = true,
                ValidIssuer = _emisorToken,
                IssuerSigningKey = new SymmetricSecurityKey(key)
            };

            try
            {
                var principal = tokenHandler.ValidateToken(token, tokenValidationParameters, out var validatedToken);
                return principal;
            }
            catch (Exception)
            {
                // Handle token validation error
                return null;
            }



            //end class
        }
    }
}
