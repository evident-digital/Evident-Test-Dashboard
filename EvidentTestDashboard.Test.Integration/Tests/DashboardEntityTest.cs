using System.Linq;
using EvidentTestDashboard.Library.Entities;
using EvidentTestDashboard.Library.Repositories;
using Xunit;

namespace EvidentTestDashboard.Test.Integration.Tests
{
    public class DashboardEntityTest
    {
        [Fact]
        public void ShouldRetrieveDashboardAndAddEnvironment()
        {
            var dbContext = new TestDashboardDbContext();
            var repo = new EFRepository<Dashboard>(dbContext);

            var dashboard = repo.GetAll().FirstOrDefault();
            dashboard.Environments.Add(new Environment() { EnvironmentName = "sprint.test.evident.nl"});

            dbContext.SaveChanges();

        }

        [Fact]
        public void ShouldInsertBuildType()
        {
            var dbContext = new TestDashboardDbContext();
            var repo = new EFRepository<Dashboard>(dbContext);
            var environment =
                repo.GetById(2).Environments.FirstOrDefault(e => e.EnvironmentName == "sprint.test.evident.nl");
            environment.BuildTypes.Add(new BuildType() { BuildTypeName = "Ncoi_Sprint_SeleniumTestrun" });
            dbContext.SaveChanges();
        }

        [Fact]
        public void ShouldInsertRecordInDatabase()
        {
            var dbContext = new TestDashboardDbContext();
            var repo = new EFRepository<Dashboard>(dbContext);

            var entity = new Dashboard()
            {
                DashboardName = "TEST"
            };

            repo.Add(entity);
            dbContext.SaveChanges();
        } 
    }
}