using Sample.Application.Contracts.Users;
using Sample.Application.Contracts.Users.Queries;
using Sample.Commons.Contracts;

namespace Sample.Application.Users.Queries
{
    public class GetUserByUsernameHandler : IQueryHandler<GetUserByUsernameFilterQuery, GetUserByUsernameResultQuery>
    {
        private readonly IUserRepository _userRepositorty;

        public GetUserByUsernameHandler(IUserRepository userRepositorty)
        {
            _userRepositorty = userRepositorty;
        }

        public async Task<GetUserByUsernameResultQuery> ExecuteAsync(GetUserByUsernameFilterQuery filter)
        {
            var result = new GetUserByUsernameResultQuery();

            var found = await _userRepositorty.GetByUsername(filter.Username);

            result.User = UserMap.Do(found);

            return result;
        }
    }
}
