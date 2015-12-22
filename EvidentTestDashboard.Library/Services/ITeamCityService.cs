using System.Collections.Generic;
using System.Threading.Tasks;
using EvidentTestDashboard.Library.DTO;

namespace EvidentTestDashboard.Library.Services
{
    public interface ITeamCityService
    {
        Task<IEnumerable<TestOccurrenceDTO>> GetTestOccurrencesForBuildAsync(long buildId);
        Task<IDictionary<string, BuildDTO>> GetLatestBuildsAsync(IEnumerable<string> buildTypes);
        //Task<HashSet<string>> GetAllTestOccurrenceNamesAsync(IEnumerable<long> teamCityBuildId);
    }
}