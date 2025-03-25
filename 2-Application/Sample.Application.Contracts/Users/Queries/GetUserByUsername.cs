using Sample.Application.Contracts.Users.Queries.Dtos;
using Sample.Commons.Abstracts;

namespace Sample.Application.Contracts.Users.Queries
{
    public class GetUserByUsernameFilterQuery : FilterQuery
    {
        public string Username { get; set; } = string.Empty;
    }

    public class GetUserByUsernameResultQuery : ResultQuery
    {
        public UserDto? User { get; set; }
    }
}
