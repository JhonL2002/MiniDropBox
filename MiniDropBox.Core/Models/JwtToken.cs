namespace MiniDropBox.Core.Models
{
    public class JwtToken
    {
        public string Token { get; set; }
        public DateTime Expiration { get; set; }
    }
}
