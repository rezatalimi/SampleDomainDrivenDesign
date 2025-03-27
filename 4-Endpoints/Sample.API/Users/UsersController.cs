using Microsoft.AspNetCore.Mvc;
using Sample.Application.Contracts.Users.Commands;
using Sample.Application.Contracts.Users.Queries;
using Sample.Configuration;
using Sample.Configuration.Authorizations;

namespace Sample.API.Users
{
    [ApiController]
    [Route("[controller]")]
    public class UsersController : BaseController
    {
        public UsersController(IDistributor distributor) : base(distributor)
        {
        }

        [AdminUserRoles]
        [HttpPost("create-user")]
        public Task CreateUserCommand(CreateUserCommand command)
        {
            return Distributor.Push(command);
        }

        [AdminUserRoles]
        [HttpGet("get-users")]
        public Task<GetUsersResultQuery> GetUsersQuery(GetUsersFilterQuery filterQuery)
        {
            return Distributor.Pull<GetUsersFilterQuery, GetUsersResultQuery>(filterQuery);
        }

        [AdminUserRoles]
        [HttpGet("get-user")]
        public Task<GetUsersResultQuery> GetUserQuery(GetUsersFilterQuery filterQuery)
        {
            return Distributor.Pull<GetUsersFilterQuery, GetUsersResultQuery>(filterQuery);
        }

        [AllUserRoles]
        [HttpGet("get-current-user")]
        public Task<GetCurrentUserResultQuery> GetCurrentUser()
        {
            var filterQuery = new GetCurrentUserFilterQuery();

            return Distributor.Pull<GetCurrentUserFilterQuery, GetCurrentUserResultQuery>(filterQuery);
        }
    }
}
