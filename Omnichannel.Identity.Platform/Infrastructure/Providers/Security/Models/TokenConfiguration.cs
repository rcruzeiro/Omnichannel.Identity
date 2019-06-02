namespace Omnichannel.Identity.Platform.Infrastructure.Providers.Security.Models
{
    public sealed class TokenConfiguration
    {
        public string Audience { get; set; }
        public string Issuer { get; set; }
        public int Seconds { get; set; }
    }
}
