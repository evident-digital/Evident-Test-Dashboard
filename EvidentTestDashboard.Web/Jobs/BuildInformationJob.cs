using System.Threading.Tasks;
using EvidentTestDashboard.Library.Contracts;
using EvidentTestDashboard.Library.Factories;
using EvidentTestDashboard.Library.Services;
using System.Linq;

namespace EvidentTestDashboard.Web.Jobs
{
    public class BuildInformationJob
    {
        private readonly ITestDashboardUOW _uow;
        private readonly ITeamCityService _teamCityService;

        public BuildInformationJob(ITestDashboardUOW uow, ITeamCityService teamCityService)
        {
            _uow = uow;
            _teamCityService = teamCityService;
        }

        public void Schedule()
        {
            
        }

        public async Task CollectBuildDataAsync()
        {
            var latestBuildDTO = await _teamCityService.GetLatestBuild();
            var latestBuild = BuildFactory.Instance.Create(latestBuildDTO);

            var buildType =
                _uow.BuildTypes.GetAll().FirstOrDefault(bt => bt.BuildTypeName == latestBuildDTO.BuildType.Id);

            if (buildType != null)
            {
                // Check if build doesn't already exists in the database
                if (!_uow.Builds.GetAll().Any(b => b.TeamCityBuildId == latestBuildDTO.Id))
                {
                    var testOccurrencesForBuildDTO =
                        await _teamCityService.GetTestOccurrencesForBuildAsync(latestBuild.TeamCityBuildId);

                    var testOccurrencesForBuild =
                        testOccurrencesForBuildDTO.Select(t => TestOccurrenceFactory.Instance.Create(t)).ToList();
                    testOccurrencesForBuild.ForEach(t => latestBuild.TestOccurrences.Add(t));
                  
                    buildType.Builds.Add(latestBuild);
                    _uow.Commit();
                }
            }
        }
    }
}