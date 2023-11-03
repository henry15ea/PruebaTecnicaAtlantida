using ServiceToken.tools;

namespace ServiceToken.Entidades.Data
{
    public class EntidadTokenResponse
    {
        //[Token], [UserId], [ExpirationTime], [TokenType], [IssuedTime], [Revoked]

        //token generado
        public string Token { get; set; }

        //id usuario o de cuenta
        public string UserId { get; set; }

        //tiempo de disponibilidad de token
        public DateTimeOffset ExpirationTime { get; set; } = DateTime.UtcNow.AddMinutes(JwtBuilder.ExpirationToken);

        //tipo de token 
        public int TokenType { get; set; } = 1; //por defecto es 1 = ACCESS

        //tiempo en que el token fue emitido
        public DateTimeOffset IssuedTime { get; set; } = DateTime.UtcNow;
        //estado del token , si esta disponible o ya ha vencido 
        public bool Revoked { get; set; } = false;
    }
}
