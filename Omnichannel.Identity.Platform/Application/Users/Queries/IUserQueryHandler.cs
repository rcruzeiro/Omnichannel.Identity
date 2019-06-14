using Core.Framework.Cqrs.Queries;
using Omnichannel.Identity.Platform.Application.Users.Queries.DTOs;
using Omnichannel.Identity.Platform.Application.Users.Queries.Filters;

namespace Omnichannel.Identity.Platform.Application.Users.Queries
{
    public interface IUserQueryHandler :
        IQueryHandlerAsync<GetUserFilter, UserDTO>,
        IQueryHandlerAsync<LoginFilter, UserDTO>
    { }
}
