using Sample.Commons.Abstracts;
using Sample.Commons.Enums;

namespace Sample.Application.Contracts.Users.Commands
{
    public class CreateUserCommand : Command
    {
        public string FullName { get; set; } = string.Empty;
        public string Username { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;
        public UserRole Role { get; set; }
    }
}
