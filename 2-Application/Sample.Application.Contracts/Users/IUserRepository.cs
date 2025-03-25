using Sample.Commons.Contracts;
using Sample.Commons.Enums;
using Sample.Domain.Users;

namespace Sample.Application.Contracts.Users
{
    public interface IUserRepository : IRepository
    {
        Task Add(User user);
        Task Update(User user);
        Task Delete(User user);

        Task<User> GetById(Guid userID);
        Task<User> GetByUsername(string username);
        Task<List<User>> GetUsersByFilters(UsersFilters usersFilters);
    }

    public class UsersFilters
    {
        public string Username { get; set; } = string.Empty;
        public UserRole? Role { get; set; }
    }
}
