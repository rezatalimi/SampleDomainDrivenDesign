using Sample.Commons.Abstracts;
using Sample.Commons.Enums;

namespace Sample.Application.Contracts.Users.Queries.Dtos
{
    public class UserDto : EntityDto
    {
        public Guid Id { get; set; }
        public string Fullname { get; set; } = string.Empty;
        public string Username { get; set; } = string.Empty;
        public UserRole Role { get; set; }
        public string RoleDisplay { get; set; } = string.Empty;
    }
}
