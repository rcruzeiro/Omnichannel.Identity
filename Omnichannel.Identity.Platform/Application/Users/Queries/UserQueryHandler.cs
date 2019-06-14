using System;
using System.Threading;
using System.Threading.Tasks;
using Core.Framework.Cqrs.Queries;
using Omnichannel.Identity.Platform.Application.Users.Queries.DTOs;
using Omnichannel.Identity.Platform.Application.Users.Queries.Filters;
using Omnichannel.Identity.Platform.Domain;

namespace Omnichannel.Identity.Platform.Application.Users.Queries
{
    public class UserQueryHandler : IUserQueryHandler
    {
        readonly IUserRepository _userRepository;

        public UserQueryHandler(IUserRepository userRepository)
        {
            _userRepository = userRepository ?? throw new ArgumentNullException(nameof(userRepository));
        }

        public async Task<UserDTO> HandleAsync(GetUserFilter filter, CancellationToken cancellationToken = default)
        {
            try
            {
                ValidateFilter(filter);

                var user = await _userRepository.GetOneAsync(filter, cancellationToken);

                return user.ToDTO();
            }
            catch (Exception ex)
            { throw ex; }
        }

        public async Task<UserDTO> HandleAsync(LoginFilter filter, CancellationToken cancellationToken = default)
        {
            try
            {
                ValidateFilter(filter);

                var user = await _userRepository.GetOneAsync(filter, cancellationToken);

                return user.ToDTO();
            }
            catch (Exception ex)
            { throw ex; }
        }

        void ValidateFilter<T>(T filter)
            where T : class, IFilter
        {
            if (filter == null) throw new ArgumentNullException(nameof(filter));

            filter.Validate(null);
        }
    }
}
