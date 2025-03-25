using Sample.Application.Contracts.Users.Queries.Dtos;
using Sample.Domain.Users;

namespace Sample.Application.Contracts.Users.Queries
{
    public static class UserMap
    {
        public static List<UserDto> Do(List<User> users)
        {
            var usersDto = new List<UserDto>();

            foreach (var user in users)
            {
                var dto = Do(user);

                usersDto.Add(dto);
            }

            return usersDto;
        }

        public static UserDto Do(User user)
        {
            var dto = new UserDto();

            dto.Id = user.Id;
            dto.Fullname = user.Fullname;
            dto.Username = user.Username;
            dto.Role = user.Role;
            dto.CreateAt = user.CreateAt;
            dto.CreateBy = user.CreateBy;
            dto.UpdateAt = user.UpdateAt;
            dto.UpdateBy = user.UpdateBy;

            return dto;
        }
    }
}
