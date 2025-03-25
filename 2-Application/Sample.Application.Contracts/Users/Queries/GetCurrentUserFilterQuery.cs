using Sample.Application.Contracts.Users.Queries.Dtos;
using Sample.Commons.Abstracts;
using Sample.Commons.Enums;

namespace Sample.Application.Contracts.Users.Queries
{
    public class GetCurrentUserFilterQuery : FilterQuery
    {
        public string Username { get; set; } = string.Empty;
        public UserRole? Role { get; set; }
    }

    public class GetCurrentUserResultQuery : ResultQuery
    {
        public UserDto User { get; set; }
    }

}
