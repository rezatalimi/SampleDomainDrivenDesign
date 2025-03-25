using Microsoft.Extensions.DependencyInjection;
using Sample.Commons.Contracts;
using System.Reflection;

namespace Sample.Configuration
{
    internal static class ConfigureReflectionExtension
    {
        static bool IsSameInterface(this Type type, Type handlerInterface)
        {
            var list = type.GetInterfaces();

            var result = list.Any(i => i.Name == handlerInterface.Name);

            return result;
        }

        static Type GetSameInterface(Type[] types, Type handlerInterface)
        {
            var result = types.First();

            return result;
        }

        public static void AddGenericInterfacesWithAssemblyReference(Assembly assembly, IServiceCollection serviceCollection, Type handlerInterface)
        {
            var handlerTypes = assembly.GetTypes().Where(t => t.GetInterfaces()
            .Any(i => i.IsGenericType && i.GetGenericTypeDefinition() == handlerInterface)).ToList();

            foreach (var handlerType in handlerTypes)
            {
                var interfaceType = handlerType.GetInterfaces().
                    First(i => i.GetGenericTypeDefinition() == handlerInterface);
                serviceCollection.AddScoped(interfaceType, handlerType);
            }
        }

        public static void AddInterfacesWithAssemblyReference(Assembly assembly, IServiceCollection serviceCollection, Type handlerInterface)
        {
            var implementations = assembly.GetTypes()
                .Where(t => t.GetTypeInfo().ImplementedInterfaces.Any(x => x.Name == handlerInterface.Name) &&
                           t.IsClass == true);

            foreach (var implementation in implementations)
            {
                var serviceType = GetSameInterface(implementation.GetInterfaces(), handlerInterface);

                serviceCollection.AddScoped(serviceType, implementation);
            }
        }
    }
}
