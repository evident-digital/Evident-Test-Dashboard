using System;
using System.Data.Entity;
using EvidentTestDashboard.Library.Contracts;
using EvidentTestDashboard.Library.Entities;
using Environment = EvidentTestDashboard.Library.Entities.Environment;

namespace EvidentTestDashboard.Library.Repositories
{
    public class TestDashboardUOW : ITestDashboardUOW, IDisposable
     {
        private DbContext DbContext { get; set; }

        public TestDashboardUOW()
        {
            CreateDbContext();
        }

        public void Commit()
        {
            DbContext.SaveChanges();
        }

        public IRepository<Dashboard> Dashboards => new EFRepository<Dashboard>(DbContext);
        public IRepository<Environment> Environments => new EFRepository<Environment>(DbContext);
        public IRepository<BuildType> BuildTypes => new EFRepository<BuildType>(DbContext);
        public IRepository<Build> Builds => new EFRepository<Build>(DbContext);
        public IRepository<TestOccurrence> TestOccurrences => new EFRepository<TestOccurrence>(DbContext);
        public IRepository<Label> Labels => new EFRepository<Label>(DbContext);

        private void CreateDbContext()
        {
            DbContext = new TestDashboardDbContext();
        }

        public void Dispose()
        {
            Dispose(true);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (disposing)
            {
                DbContext?.Dispose();
            }
        }
    }
}