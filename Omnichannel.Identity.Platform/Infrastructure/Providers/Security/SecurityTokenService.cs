using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Principal;
using Microsoft.IdentityModel.Tokens;
using Omnichannel.Identity.Platform.Infrastructure.Providers.Security.Models;

namespace Omnichannel.Identity.Platform.Infrastructure.Providers.Security
{
    public class SecurityTokenService : ISecurityTokenService
    {
        readonly SigninConfiguration _signinConfiguration;
        readonly TokenConfiguration _token;

        public SecurityTokenService(SigninConfiguration signinConfiguration, TokenConfiguration token)
        {
            _signinConfiguration = signinConfiguration ?? throw new ArgumentNullException(nameof(signinConfiguration));
            _token = token ?? throw new ArgumentNullException(nameof(token));
        }

        public string CreateToken(TokenData data)
        {
            try
            {
                ClaimsIdentity identity = new ClaimsIdentity(
                    new GenericIdentity(data.Email, "Login"),
                    new[]
                    {
                        new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString("N")),
                        new Claim("company", data.Company),
                        new Claim("username", data.CPF),
                        new Claim("name", data.Name)
                    });
                var handler = new JwtSecurityTokenHandler();
                var securityToken = handler.CreateToken(new SecurityTokenDescriptor
                {
                    Issuer = _token.Issuer,
                    Audience = _token.Audience,
                    SigningCredentials = _signinConfiguration.Credentials,
                    Subject = identity,
                    NotBefore = DateTime.Now,
                    Expires = DateTime.Now + TimeSpan.FromSeconds(_token.Seconds)
                });
                var token = handler.WriteToken(securityToken);

                return token;
            }
            catch (Exception ex)
            { throw ex; }
        }
    }
}
