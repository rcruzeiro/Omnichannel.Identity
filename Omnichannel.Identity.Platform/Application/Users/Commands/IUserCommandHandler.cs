using Core.Framework.Cqrs.Commands;
using Omnichannel.Identity.Platform.Application.Users.Commands.Actions;

namespace Omnichannel.Identity.Platform.Application.Users.Commands
{
    public interface IUserCommandHandler :
        ICommandHandlerAsync<CreateUserCommand>
    { }
}
