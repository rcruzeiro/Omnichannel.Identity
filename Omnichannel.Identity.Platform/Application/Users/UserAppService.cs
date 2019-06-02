using System;
using System.Threading;
using System.Threading.Tasks;
using Core.Framework.Cache;
using Newtonsoft.Json;
using Omnichannel.Identity.Platform.Application.Users.Commands;
using Omnichannel.Identity.Platform.Application.Users.Commands.Actions;
using Omnichannel.Identity.Platform.Application.Users.Queries;
using Omnichannel.Identity.Platform.Application.Users.Queries.DTOs;
using Omnichannel.Identity.Platform.Application.Users.Queries.Filters;
using Omnichannel.Identity.Platform.Infrastructure.Providers.Security;
using Omnichannel.Identity.Platform.Infrastructure.Providers.Security.Models;

namespace Omnichannel.Identity.Platform.Application.Users
{
    public class UserAppService : IUserAppService
    {
        readonly IUserCommandHandler _userCommandHandler;
        readonly IUserQueryHandler _userQueryHandler;
        readonly ICacheService _cacheService;
        readonly ISecurityTokenService _securityTokenService;

        public UserAppService(IUserCommandHandler userCommandHandler,
                              IUserQueryHandler userQueryHandler,
                              ICacheService cacheService,
                              ISecurityTokenService securityTokenService)
        {
            _userCommandHandler = userCommandHandler ?? throw new ArgumentNullException(nameof(userCommandHandler));
            _userQueryHandler = userQueryHandler ?? throw new ArgumentNullException(nameof(userQueryHandler));
            _cacheService = cacheService ?? throw new ArgumentNullException(nameof(cacheService));
            _securityTokenService = securityTokenService ?? throw new ArgumentNullException(nameof(securityTokenService));
        }

        public async Task Create(CreateUserCommand command, CancellationToken cancellationToken = default)
        {
            try
            {
                await _userCommandHandler.ExecuteAsync(command, cancellationToken);
            }
            catch (Exception ex)
            { throw ex; }
        }

        public async Task<UserDTO> Login(LoginFilter filter, CancellationToken cancellationToken = default)
        {
            try
            {
                var user = await _userQueryHandler.HandleAsync(filter, cancellationToken);

                if (user == null) throw new ArgumentNullException(nameof(user));

                if (!user.Active) throw new InvalidOperationException("user is currently not active.");

                // create token data
                var tokenData = new TokenData(user.Company, user.Name, user.Email);

                // create a token for the new logged user
                user.Token = _securityTokenService.CreateToken(tokenData);

                _cacheService.Set(user.Token, JsonConvert.SerializeObject(user));

                return user;
            }
            catch (Exception ex)
            { throw ex; }
        }

        public void Logout(string token)
        {
            try
            {
                _cacheService.Remove(token);
            }
            catch (Exception ex)
            { throw ex; }
        }

        public UserDTO GetUser(string token)
        {
            try
            {
                if (!_cacheService.Exists(token)) throw new InvalidOperationException("user is not logged in.");

                return JsonConvert.DeserializeObject<UserDTO>(
                    _cacheService.Get(token));
            }
            catch (Exception ex)
            { throw ex; }
        }
    }
}
