using Microsoft.Extensions.DependencyInjection;
using Sample.Application.Contracts.Users.Commands;
using Sample.Application.Users.Commands;
using Sample.Application.Users.Queries;
using Sample.Commons.Contracts;
using Sample.Data.Users;

namespace Sample.Configuration
{
    internal static class ConfigureWithContractExtention
    {
        public static void AddWithContract(this IServiceCollection serviceCollection)
        {
            AddCommandHandlers(serviceCollection);

            AddQueryHandler(serviceCollection);

            AddRepositories(serviceCollection);
        }

        private static void AddCommandHandlers(IServiceCollection serviceCollection)
        {
            ConfigureReflectionExtension.AddInterfacesWithAssemblyReference(typeof(CreateUserCommandHandler).Assembly,
                serviceCollection, typeof(ICommandHandler<>));
        }

        private static void AddQueryHandler(IServiceCollection serviceCollection)
        {
            ConfigureReflectionExtension.AddInterfacesWithAssemblyReference(typeof(GetUsersQueryHandler).Assembly,
                serviceCollection, typeof(IQueryHandler<,>));
        }

        private static void AddRepositories(IServiceCollection serviceCollection)
        {
            ConfigureReflectionExtension.AddInterfacesWithAssemblyReference(typeof(UserRepository).Assembly,
                serviceCollection, typeof(IRepository));
        }
    }
}
