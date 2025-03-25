using Sample.Application.Contracts.Users;
using Sample.Application.Contracts.Users.Queries;
using Sample.Commons.Contracts;

namespace Sample.Application.Users.Queries
{
    public class GetCurrentUserQueryHandler : IQueryHandler<GetCurrentUserFilterQuery, GetCurrentUserResultQuery>
    {
        private readonly IUserRepository _userRepositorty;

        public GetCurrentUserQueryHandler(IUserRepository userRepositorty)
        {
            _userRepositorty = userRepositorty;
        }

        public async Task<GetCurrentUserResultQuery> ExecuteAsync(GetCurrentUserFilterQuery filter)
        {
            var result = new GetCurrentUserResultQuery();

            var found = await _userRepositorty.GetById(filter.MetaData.CurentUserId);

            result.User = UserMap.Do(found);

            return result;
        }
    }
}
