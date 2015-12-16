using EvidentTestDashboard.Library.Entities;

namespace EvidentTestDashboard.Library.Contracts
{
    public interface ITestDashboardUOW
    {
        void Commit();

        IRepository<Dashboard> Dashboards { get; }
        IRepository<Environment> Environments { get; }
        IRepository<BuildType> BuildTypes { get; }
        IRepository<Build> Builds { get; }
        IRepository<TestOccurrence> TestOccurrences { get; }
        IRepository<Label> Labels { get; } 
    }
}