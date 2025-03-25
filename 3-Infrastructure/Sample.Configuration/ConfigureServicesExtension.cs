using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Sample.Commons;
using Sample.Configuration.Authentication;

namespace Sample.Configuration
{
    public static class ConfigureServicesExtension
    {
        public static void AddConfigureServices(this IServiceCollection services, ConfigurationManager configuration)
        {
            services.AddSettings(configuration);

            var sampleDB = configuration.GetConnectionString("Sample_DB");

            services.AddSampleDbContext(sampleDB);

            services.AddHttpClient();

            services.AddHttpContextAccessor();

            services.AddWithContract();

            services.AddAuthenticationSettings();

            services.AddScoped<IDistributor, Distributor>();

            services.AddSingleton<AccessManagementInMemory>();

            services.AddSwagger();
        }

        private static void AddAuthenticationSettings(this IServiceCollection serviceCollection)
        {
            serviceCollection.AddAuthentication(options =>
            {
                options.DefaultScheme = SampleTokenAuthenticationSchemeOptions.SchemeName;
                options.DefaultAuthenticateScheme = SampleTokenAuthenticationSchemeOptions.SchemeName;
                options.DefaultChallengeScheme = SampleTokenAuthenticationSchemeOptions.SchemeName;
            })
                .AddScheme<SampleTokenAuthenticationSchemeOptions,
                SampleTokenAuthenticationSchemeHandler>
                    (SampleTokenAuthenticationSchemeOptions.SchemeName, op => { });
        }

        public static void AddSettings(this IServiceCollection services, IConfiguration configuration)
        {
            var generalSettings = configuration.GetSection("GeneralSettings").Get<GeneralSettings>();

            services.AddSingleton(generalSettings);
        }

        public static async void LoadTokens(this WebApplication application)
        {
            using var scope = application.Services.CreateScope();

            var accessManagementInMemory = scope.ServiceProvider.GetRequiredService<AccessManagementInMemory>();

            await accessManagementInMemory.LoadTokens();
        }
    }
}
