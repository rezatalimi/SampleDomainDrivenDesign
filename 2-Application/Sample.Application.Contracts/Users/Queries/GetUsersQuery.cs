using Sample.Application.Contracts.Users.Queries.Dtos;
using Sample.Commons.Abstracts;
using Sample.Commons.Enums;

namespace Sample.Application.Contracts.Users.Queries
{
    public class GetUsersFilterQuery : FilterQuery
    {
        public string Username { get; set; } = string.Empty;
        public UserRole? Role { get; set; }
    }

    public class GetUsersResultQuery : ResultQuery
    {
        public List<UserDto> Users { get; set; } = new List<UserDto>();
    }
}
