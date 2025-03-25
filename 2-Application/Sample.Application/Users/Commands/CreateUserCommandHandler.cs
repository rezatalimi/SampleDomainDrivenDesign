using Sample.Application.Contracts.Users;
using Sample.Application.Contracts.Users.Commands;
using Sample.Application.Users.Commands.Exceptions;
using Sample.Commons;
using Sample.Commons.Contracts;
using Sample.Commons.Extensions;
using Sample.Domain.Users;

namespace Sample.Application.Users.Commands
{
    public class CreateUserCommandHandler : ICommandHandler<CreateUserCommand>
    {
        private readonly IUserRepository _userRepositorty;
        private readonly GeneralSettings _generalSettings;

        public CreateUserCommandHandler(IUserRepository userRepositorty, GeneralSettings generalSettings)
        {
            _userRepositorty = userRepositorty;
            _generalSettings = generalSettings;
        }

        public async Task ExecuteAsync(CreateUserCommand command)
        {
            if (command.Password.IsComplexPassword() == false)
                throw new ThePasswordIsInvalidException();

            var userId = Guid.NewGuid();

            var password = command.Password.GetHashPassword(_generalSettings.Salt);

            var user = new User(userId, command.Role,command.Username, password, command.FullName);

            var found = await _userRepositorty.GetByUsername(command.Username);

            if (found != null)
            {
                throw new ThisUserIsDuplicateException();
            }

            await _userRepositorty.Add(user);
        }
    }
}
