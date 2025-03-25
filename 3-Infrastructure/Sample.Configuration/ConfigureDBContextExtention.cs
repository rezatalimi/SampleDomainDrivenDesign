using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Sample.Commons;
using Sample.Commons.Enums;
using Sample.Commons.Extensions;
using Sample.Data;
using Sample.Domain.Users;

namespace Sample.Configuration
{
    public static class ConfigureDBContextExtention
    {
        internal static void AddSampleDbContext(this IServiceCollection services, string connectionString)
        {
            services.AddDbContext<SampleDataBase>(
                   (provider, options) =>
                   {
                       options.UseSqlServer(connectionString);
                       options.UseQueryTrackingBehavior(QueryTrackingBehavior.NoTracking);
                   }, ServiceLifetime.Transient
               );
        }

        public static async void SampleDbContextConfiguration(this WebApplication application)
        {
            using var scope = application.Services.CreateScope();

            var sampleDBContext = scope.ServiceProvider.GetRequiredService<SampleDataBase>();

            var generalSettings = scope.ServiceProvider.GetRequiredService<GeneralSettings>();

            sampleDBContext.Database.Migrate();

            await AppendSuperUser(sampleDBContext, generalSettings);
        }

        private static async Task AppendSuperUser(SampleDataBase sampleDBContext, GeneralSettings generalSettings)
        {
            var count = sampleDBContext.Users.Count();

            if (count == 0)
            {
                var password = ("Test@%123Admin").GetHashPassword(generalSettings.Salt);

                var user = new User(Guid.NewGuid(),
                             UserRole.Admin,
                             "RezaTaslimi",
                             password,
                             "Reza Taslimi");

                sampleDBContext.Users.Add(user);

                await sampleDBContext.SaveChangesAsync();
            }
        }

    }
}
