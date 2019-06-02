using Omnichannel.Identity.Platform.Infrastructure.Providers.Security.Models;

namespace Omnichannel.Identity.Platform.Infrastructure.Providers.Security
{
    public interface ISecurityTokenService
    {
        string CreateToken(TokenData data);
    }
}
