using System;
using System.Threading;
using System.Threading.Tasks;
using Core.Framework.Cache;
using Core.Framework.Cqrs.Commands;
using Omnichannel.Identity.Platform.Application.Users.Commands.Actions;
using Omnichannel.Identity.Platform.Domain;

namespace Omnichannel.Identity.Platform.Application.Users.Commands
{
    public class UserCommandHandler : IUserCommandHandler
    {
        readonly IUserRepository _userRepository;

        public UserCommandHandler(IUserRepository userRepository, ICacheService cacheService)
        {
            _userRepository = userRepository ?? throw new ArgumentNullException(nameof(userRepository));
        }

        public async Task ExecuteAsync(CreateUserCommand command, CancellationToken cancellationToken = default)
        {
            try
            {
                ValidateCommand(command);

                var user = command.ToDomain();

                await _userRepository.AddAsync(user, cancellationToken);
                await _userRepository.SaveChangesAsync();
            }
            catch (Exception ex)
            { throw ex; }
        }

        public async Task ExecuteAsync(LoginUserCommand command, CancellationToken cancellationToken = default)
        {
            try
            {
                ValidateCommand(command);

                var user = await _userRepository.GetOneAsync(u => u.ID == command.UserId);

                if (user == null) throw new ArgumentNullException(nameof(user));

                user.Login(command.Token);

                _userRepository.Update(user);
                await _userRepository.SaveChangesAsync(cancellationToken);
            }
            catch (Exception ex)
            { throw ex; }
        }

        public async Task ExecuteAsync(LogoutUserCommand command, CancellationToken cancellationToken = default)
        {
            try
            {
                ValidateCommand(command);

                var user = await _userRepository.GetOneAsync(u => u.Company == command.Company && u.CPF == command.CPF);

                if (user == null) throw new ArgumentNullException(nameof(user));

                user.Logout();

                _userRepository.Update(user);
                await _userRepository.SaveChangesAsync(cancellationToken);
            }
            catch (Exception ex)
            { throw ex; }
        }

        void ValidateCommand<T>(T command)
            where T : class, ICommand
        {
            if (command == null) throw new ArgumentNullException(nameof(command));

            command.Validate(null);
        }
    }
}
