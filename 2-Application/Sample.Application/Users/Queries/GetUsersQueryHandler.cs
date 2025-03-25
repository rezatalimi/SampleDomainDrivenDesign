using Sample.Application.Contracts.Users;
using Sample.Application.Contracts.Users.Queries;
using Sample.Commons.Contracts;

namespace Sample.Application.Users.Queries
{
    public class GetUsersQueryHandler : IQueryHandler<GetUsersFilterQuery, GetUsersResultQuery>
    {
        private readonly IUserRepository _userRepositorty;

        public GetUsersQueryHandler(IUserRepository userRepositorty)
        {
            _userRepositorty = userRepositorty;
        }

        public async Task<GetUsersResultQuery> ExecuteAsync(GetUsersFilterQuery filter)
        {
            var result = new GetUsersResultQuery();

            var filters = new UsersFilters { Role = filter.Role , Username = filter .Username};

            var users = await _userRepositorty.GetUsersByFilters(filters);

            result.Users = UserMap.Do(users);

            return result;
        }
    }
}
