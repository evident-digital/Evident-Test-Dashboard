using System.Threading.Tasks;
using EvidentTestDashboard.Library.Contracts;
using EvidentTestDashboard.Library.Factories;
using EvidentTestDashboard.Library.Services;
using System.Linq;
using System.Text.RegularExpressions;
using static System.Configuration.ConfigurationManager;
using EvidentTestDashboard.Library.Entities;
using System;

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
            var buildTypeNames = _uow.BuildTypes.GetAll().Where(bt => bt.Environment.Dashboard.DashboardName == DEFAULT_LABEL).Select(bt => bt.BuildTypeName).Distinct();
            var latestBuildDTOs = await _teamCityService.GetBuildsAsync(buildTypeNames, DateTime.Now.AddDays(-1.0));

            var latestBuilds = latestBuildDTOs
                .Select(bDto => BuildFactory.Instance.Create(bDto))
                .ToDictionary(b => b.TeamCityBuildId, b => b);

            foreach (var buildDto in latestBuildDTOs)
            {
                var buildTypes =
                    _uow.BuildTypes.GetAll().Where(bt => bt.BuildTypeName == buildDto.BuildTypeId).ToList();

                if (buildTypes != null && buildTypes.Any())
                {
                    var build = latestBuilds[buildDto.Id];

                    // Check if build isn't already saved in the database
                    if (!_uow.Builds.GetAll().Any(b => b.TeamCityBuildId == buildDto.Id))
                    {
                        BuildType buildType = null;
                        if (buildTypes.Count > 1)
                        {
                            // we have multiple build types of the same name..
                            // prob. because we have a parameter we need to check..
                            buildType = buildTypes.FirstOrDefault(bt => {
                                return buildDto.Properties.Property.Any(p => p.Name == bt.RequiredParamName && p.Value == bt.RequiredParamValue);
                            });
                        }
                        else
                        {
                            buildType = buildTypes.First();
                        }
                        if(buildType == null)
                        {
                            continue;
                        }
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