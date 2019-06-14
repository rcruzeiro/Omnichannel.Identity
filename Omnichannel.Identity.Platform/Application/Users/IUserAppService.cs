using System.Threading;
using System.Threading.Tasks;
using Omnichannel.Identity.Platform.Application.Users.Commands.Actions;
using Omnichannel.Identity.Platform.Application.Users.Queries.DTOs;
using Omnichannel.Identity.Platform.Application.Users.Queries.Filters;

namespace Omnichannel.Identity.Platform.Application.Users
{
    public interface IUserAppService
    {
        Task Create(CreateUserCommand command, CancellationToken cancellationToken = default);
        Task<UserDTO> Login(LoginFilter filter, CancellationToken cancellationToken = default);
        Task Logout(LogoutUserCommand command, CancellationToken cancellationToken = default);
        Task<UserDTO> GetUser(GetUserFilter filter, CancellationToken cancellationToken = default);
    }
}
