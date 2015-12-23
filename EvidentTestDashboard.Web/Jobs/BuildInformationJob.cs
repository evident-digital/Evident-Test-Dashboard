using System.Threading.Tasks;
using EvidentTestDashboard.Library.Contracts;
using EvidentTestDashboard.Library.Factories;
using EvidentTestDashboard.Library.Services;
using System.Linq;
using System.Text.RegularExpressions;
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

        public async Task CollectBuildDataAsync()
        {
            var buildTypeNames = _uow.BuildTypes.GetAll().Where(bt => bt.Environment.Dashboard.DashboardName == DEFAULT_LABEL).Select(bt => bt.BuildTypeName);
            var latestBuildDTOs = await _teamCityService.GetLatestBuildsAsync(buildTypeNames);

            var latestBuilds =
                latestBuildDTOs.Select(i => new {BuildType = i.Key, Build = BuildFactory.Instance.Create(i.Value)})
                    .ToDictionary(i => i.BuildType, i => i.Build);

            foreach (var buildTypeName in latestBuilds.Keys)
            {
                var buildType =
                    _uow.BuildTypes.GetAll().FirstOrDefault(bt => bt.BuildTypeName == buildTypeName);

                if (buildType != null)
                {
                    var build = latestBuilds[buildTypeName];

                    // Check if build isn't already saved in the database
                    if (!_uow.Builds.GetAll().Any(b => b.TeamCityBuildId == build.TeamCityBuildId && b.BuildTypeId == buildType.BuildTypeId))
                    {
                        buildType.Builds.Add(build);

                        var testOccurrencesForBuildDTO =
                            await _teamCityService.GetTestOccurrencesForBuildAsync(build.TeamCityBuildId);

                        var testOccurrencesForBuild =
                            testOccurrencesForBuildDTO.Select(t => TestOccurrenceFactory.Instance.Create(t)).ToList();
                        testOccurrencesForBuild.ForEach(t => build.TestOccurrences.Add(t));

                        var labels = _uow.Labels.GetAll().ToList();
                        foreach (var test in testOccurrencesForBuild)
                        {
                            var label = labels.SingleOrDefault(l => l.LabelName != "Onbekend" && new Regex(l.Regex, RegexOptions.IgnoreCase).IsMatch(test.Name))
                                ?? labels.SingleOrDefault(l => l.LabelName == "Onbekend");
                            label?.TestOccurrences.Add(test);
                        }

                        _uow.Commit();
                    }
                }
            }
        }

    }
}