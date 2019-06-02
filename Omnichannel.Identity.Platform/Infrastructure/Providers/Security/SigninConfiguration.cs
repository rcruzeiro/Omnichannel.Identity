using System.Security.Cryptography;
using Microsoft.IdentityModel.Tokens;

namespace Omnichannel.Identity.Platform.Infrastructure.Providers.Security
{
    public class SigninConfiguration
    {
        public SecurityKey Key { get; }
        public SigningCredentials Credentials { get; }

        public SigninConfiguration()
        {
            using (var provider = new RSACryptoServiceProvider(2048))
                Key = new RsaSecurityKey(provider.ExportParameters(true));

            Credentials = new SigningCredentials(
                Key, SecurityAlgorithms.RsaSha256Signature);
        }
    }
}
