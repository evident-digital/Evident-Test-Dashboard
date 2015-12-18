using System.Threading.Tasks;
using EvidentTestDashboard.Library.Contracts;
using EvidentTestDashboard.Library.Factories;
using EvidentTestDashboard.Library.Services;
using System.Linq;
using static System.Configuration.ConfigurationManager;

namespace EvidentTestDashboard.Web.Jobs
{
    public class BuildInformationJob
    {
        private static readonly string DEFAULT_LABEL = AppSettings["label:default"];

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
                buildType.Builds.Add(latestBuild);

                // Check if build isn't already saved in the database
                if (!_uow.Builds.GetAll().Any(b => b.TeamCityBuildId == latestBuildDTO.Id))
                {

                    var testOccurrencesForBuildDTO =
                        await _teamCityService.GetTestOccurrencesForBuildAsync(latestBuild.TeamCityBuildId);

                    var testOccurrencesForBuild =
                        testOccurrencesForBuildDTO.Select(t => TestOccurrenceFactory.Instance.Create(t)).ToList();
                    testOccurrencesForBuild.ForEach(t => latestBuild.TestOccurrences.Add(t));

                    var labels = _uow.Labels.GetAll();
                    foreach (var test in testOccurrencesForBuild)
                    {
                        var label = labels.SingleOrDefault(l => l.LabelName.Contains(test.Name)) ??
                                    labels.SingleOrDefault(l => l.LabelName == DEFAULT_LABEL);
                        label?.TestOccurrences.Add(test);
                    }

                    _uow.Commit();
                }
            }
        }

       
    }
}