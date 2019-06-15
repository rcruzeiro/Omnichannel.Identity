using System;
using System.Threading;
using System.Threading.Tasks;
using Core.Framework.Cache;
using Omnichannel.Identity.Platform.Application.Users.Commands;
using Omnichannel.Identity.Platform.Application.Users.Commands.Actions;
using Omnichannel.Identity.Platform.Application.Users.Queries;
using Omnichannel.Identity.Platform.Application.Users.Queries.DTOs;
using Omnichannel.Identity.Platform.Application.Users.Queries.Filters;
using Omnichannel.Identity.Platform.Infrastructure.Providers.Cache.Models;
using Omnichannel.Identity.Platform.Infrastructure.Providers.Security;
using Omnichannel.Identity.Platform.Infrastructure.Providers.Security.Models;

namespace Omnichannel.Identity.Platform.Application.Users
{
    public class UserAppService : IUserAppService
    {
        readonly IUserCommandHandler _userCommandHandler;
        readonly IUserQueryHandler _userQueryHandler;
        readonly ISecurityTokenService _securityTokenService;
        readonly ICacheService _cacheService;
        readonly CacheConfiguration _cacheConfiguration;

        public UserAppService(IUserCommandHandler userCommandHandler,
                              IUserQueryHandler userQueryHandler,
                              ISecurityTokenService securityTokenService,
                              ICacheService cacheService,
                              CacheConfiguration cacheConfiguration)
        {
            _userCommandHandler = userCommandHandler ?? throw new ArgumentNullException(nameof(userCommandHandler));
            _userQueryHandler = userQueryHandler ?? throw new ArgumentNullException(nameof(userQueryHandler));
            _securityTokenService = securityTokenService ?? throw new ArgumentNullException(nameof(securityTokenService));
            _cacheService = cacheService ?? throw new ArgumentNullException(nameof(cacheService));
            _cacheConfiguration = cacheConfiguration ?? throw new ArgumentNullException(nameof(cacheConfiguration));
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
                var tokenData = new TokenData(user.Company, user.Name, user.Email, user.CPF);

                // create and save a token for the logged user
                user.Token = _securityTokenService.CreateToken(tokenData);
                var loginCommand = new LoginUserCommand(user.Id, user.Token);
                await _userCommandHandler.ExecuteAsync(loginCommand, cancellationToken);

                return user;
            }
            catch (Exception ex)
            { throw ex; }
        }

        public async Task Logout(LogoutUserCommand command, CancellationToken cancellationToken = default)
        {
            try
            {
                await _userCommandHandler.ExecuteAsync(command, cancellationToken);
            }
            catch (Exception ex)
            { throw ex; }
        }

        public async Task<UserDTO> GetUser(GetUserFilter filter, CancellationToken cancellationToken = default)
        {
            try
            {
                return await _userQueryHandler.HandleAsync(filter, cancellationToken);
            }
            catch (Exception ex)
            { throw ex; }
        }
    }
}
