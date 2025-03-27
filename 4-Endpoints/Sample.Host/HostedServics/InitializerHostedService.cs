using Sample.API;
using Sample.API.Login;
using Sample.Configuration.Authorizations;

namespace Sample.Host.HostedServics
{
    public class InitializerHostedService : IHostedService
    {
        private readonly IServiceProvider _provider;

        public InitializerHostedService(IServiceProvider provider)
        {
            _provider = provider;
        }

        public async Task StartAsync(CancellationToken stoppingToken)
        {
            using (var scope = _provider.CreateScope())
            {
                var accessTheControllers = scope.ServiceProvider.GetRequiredService<AccessTheControllers>();

                var assembly = typeof(LoginController).Assembly;

                await accessTheControllers.FillListControllers(assembly,typeof(BaseController));
            }
        }

        public Task StopAsync(CancellationToken stoppingToken)
        {
            // The code in here will run when the application stops
            // In your case, nothing to do
            return Task.CompletedTask;
        }
    }
}
