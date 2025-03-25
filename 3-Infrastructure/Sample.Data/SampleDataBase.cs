using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Sample.Data.Tokens;
using Sample.Domain.Users;

namespace Sample.Data
{
    public class SampleDataBase : DbContext
    {
        public DbSet<User> Users { get; set; }
        public DbSet<Token> Tokens { get; set; }

        public SampleDataBase(DbContextOptions<SampleDataBase> options) : base(options)
        {
            ChangeTracker.QueryTrackingBehavior = QueryTrackingBehavior.NoTracking;
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.UseCollation("Persian_100_CI_AS");
            modelBuilder.ApplyConfigurationsFromAssembly(GetType().Assembly);
        }
    }

    public class SampleDbContextFactory : IDesignTimeDbContextFactory<SampleDataBase>
    {
        public SampleDataBase CreateDbContext(string[] args)
        {
            var optionsBuilder = new DbContextOptionsBuilder<SampleDataBase>();
            optionsBuilder.UseSqlServer("Data Source=.;Initial Catalog=Sample_DB;Trusted_Connection=True;TrustServerCertificate=True;User Id=sa;Password=123;");
            optionsBuilder.EnableDetailedErrors();
            optionsBuilder.EnableSensitiveDataLogging();
            return new SampleDataBase(optionsBuilder.Options);
        }
    }
}
