namespace ServiceToken.DTO
{
    public class RefreshTokenResponseDTO
    {
        public string AccessToken { get; set; }
        public bool State { get; set; } = false;
    }
}
