using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Sample.Commons.Abstracts;
using Sample.Commons.Contracts;
using Sample.Configuration.Authentication;
using System.Security.Claims;

namespace Sample.Configuration
{
    public interface IDistributor
    {
        public Task Push<C>(C command) where C : Command;
        public Task<R> Pull<F,R>(F filter) where F : FilterQuery where R : ResultQuery;
    }

    public class Distributor : IDistributor
    {
        private readonly IServiceProvider _serviceProvider;
        private AccessManagementInMemory _accessManagementInMemory;
        private IHttpContextAccessor _httpContextAccessor;

        public Distributor(IServiceProvider serviceProvider,
            AccessManagementInMemory accessManagementInMemory,
            IHttpContextAccessor httpContextAccessor)
        {
            _serviceProvider = serviceProvider;
            _accessManagementInMemory = accessManagementInMemory;
            _httpContextAccessor = httpContextAccessor;
        }

        public async Task<R> Pull<F, R>(F filter)
            where F : FilterQuery
            where R : ResultQuery
        {
            var handler = _serviceProvider.GetRequiredService<IQueryHandler<F, R>>();

            filter.MetaData = GetMetadata();

            var queryResponse = await handler.ExecuteAsync(filter);

            return queryResponse;
        }

        public async Task Push<C>(C command) where C : Command
        {
            var handler = _serviceProvider.GetRequiredService<ICommandHandler<C>>();

            command.MetaData = GetMetadata();

            await handler.ExecuteAsync(command);
        }

        public MetaData GetMetadata()
        {
            var result = new MetaData();

            if (_httpContextAccessor.HttpContext != null && _httpContextAccessor.HttpContext.User.Claims.Any())
            {
                var claims = _httpContextAccessor.HttpContext.User.Claims;

                var token = claims.FirstOrDefault(x => x.Type == ClaimTypes.NameIdentifier);

                var accessInformation = _accessManagementInMemory.GetAccessInformationByToken(token.Value);

                result.CurentUserId = accessInformation.User.Id;

                result.CurentUserFullname = accessInformation.User.Fullname;

                result.Role = accessInformation.User.Role;
            }

            return result;
        }
    }
}
