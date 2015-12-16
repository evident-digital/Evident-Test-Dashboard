using System.Threading.Tasks;
using EvidentTestDashboard.Library.DTO;

namespace EvidentTestDashboard.Library.Services
{
    public interface ITeamCityService
    {
        Task<TestOccurrenceCollectionDTO> GetLatestBuildTestDataAsync();
        Task<BuildDTO> GetLatestBuild();
    }
}