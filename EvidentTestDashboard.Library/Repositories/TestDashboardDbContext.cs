using System.Data.Entity;
using System.Data.Entity.ModelConfiguration.Conventions;
using EvidentTestDashboard.Library.Entities;

namespace EvidentTestDashboard.Library.Repositories
{
    public class TestDashboardDbContext : DbContext
    {
        public TestDashboardDbContext()
            : base(nameOrConnectionString: "default") {
        }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Conventions.Remove<PluralizingTableNameConvention>();
            modelBuilder.Conventions.Remove<OneToManyCascadeDeleteConvention>();
        }

        public DbSet<Dashboard> Dashboards { get; set; }
        public DbSet<Environment> Environments { get; set; }
        public DbSet<BuildType> BuildTypes { get; set; }
        public DbSet<Build> Builds { get; set; }
        public DbSet<TestOccurrence> TestOccurences { get; set; }
        public DbSet<Label> Labels { get; set; }
    }
}