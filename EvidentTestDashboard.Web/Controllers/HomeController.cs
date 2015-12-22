using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web.Mvc;
using EvidentTestDashboard.Library.Contracts;
using EvidentTestDashboard.Library.Entities;
using EvidentTestDashboard.Library.Services;
using EvidentTestDashboard.Web.ViewModels;
using static System.Configuration.ConfigurationManager;

namespace EvidentTestDashboard.Web.Controllers
{
    public class HomeController : Controller
    {
        private static readonly string DEFAULT_LABEL = AppSettings["label:default"];

        private readonly ITestDashboardUOW _uow;
        private readonly ITeamCityService _teamCityService;

        public HomeController(ITestDashboardUOW uow, ITeamCityService teamCityService)
        {
            _uow = uow;
            _teamCityService = teamCityService;
        }

        public ActionResult Index()
        {
            var dashboard = _uow.Dashboards.GetAll().SingleOrDefault(d => d.DashboardName == DEFAULT_LABEL);

            var model = new DashboardViewModel()
            {
                Dashboard = dashboard
            };

            var environments = _uow.Environments.GetAll().Where(e => e.DashboardId == dashboard.DashboardId);
            environments.ToList().ForEach(e =>
            {
                if (_uow.Builds.GetAll().Any(b => b.BuildType.EnvironmentId == e.EnvironmentId))
                {
                    var build =
                    _uow.Builds.GetAll()
                        .Where(b => b.BuildType.EnvironmentId == e.EnvironmentId)
                        .OrderByDescending(b => b.RunAt)
                        .FirstOrDefault();

                    var labels = _uow.Labels.GetAll().Where(l => l.Dashboard.DashboardId == dashboard.DashboardId);
                    var testOccurrences = new Dictionary<string, List<TestOccurrence>>();
                    labels.ToList().ForEach(l => testOccurrences.Add(
                        l.LabelName,
                        _uow.TestOccurrences.GetAll()
                            .Where(to => to.Build.BuildId == build.BuildId && to.Label.LabelName == l.LabelName).OrderBy(t => t.Name).ToList()));

                    SetNotRunTests(testOccurrences, dashboard.DashboardId);
                    SortTestNamesPerLabel(testOccurrences);

                    var testOccurrencesNotRun = GetTestOccurrencesNotRun(testOccurrences);

                    var buildViewModel = new BuildViewModel()
                    {
                        TestOccurrencesNoRun = testOccurrencesNotRun,
                        Build = build,
                        Environment = e,
                        TestOccurrences = testOccurrences
                    };

                    model.builds.Add(buildViewModel);
                }
            });
            
            return View(model);
        }

        private static int GetTestOccurrencesNotRun(Dictionary<string, List<TestOccurrence>> testOccurrences)
        {
            var testOccurrencesNotRun =
                testOccurrences
                    .SelectMany(l => l.Value)
                    .Count(to => to.Status == TestOccurrence.TEST_OCCURRENCE_NOT_RUN);
            return testOccurrencesNotRun;
        }

        private void SortTestNamesPerLabel(Dictionary<string, List<TestOccurrence>> testOccurrences)
        {
            foreach (var label in testOccurrences.Keys)
            {
               testOccurrences[label].Sort((a, b) => string.Compare(a.Name, b.Name, StringComparison.Ordinal));
            }
        }

        private void SetNotRunTests(Dictionary<string, List<TestOccurrence>> testOccurrences, int dashboardId)
        {
            var allTestNames = GetAllTestNamesPerLabel(dashboardId);

            foreach (var label in allTestNames.Keys)
            {
                foreach (var testName in allTestNames[label])
                {
                    if (!testOccurrences[label].Select(to => to.Name).Contains(testName))
                    {
                        testOccurrences[label].Add(new TestOccurrence() { Name = testName, Status = TestOccurrence.TEST_OCCURRENCE_NOT_RUN });
                    }                    
                }
            }
        }

        public IDictionary<string, IEnumerable<string>> GetAllTestNamesPerLabel(int dashboardId)
        {
            var testOccurrences = new Dictionary<string, IEnumerable<string>>();
            var testNames = GetTestOccurrenceNamesForAllEnvironments();
            var labels = _uow.Labels.GetAll().Where(l => l.Dashboard.DashboardId == dashboardId);
            labels.ToList()
                .ForEach(
                    l =>
                        testOccurrences.Add(l.LabelName,
                            testNames.Where(tn => new Regex(l.Regex, RegexOptions.IgnoreCase).IsMatch(tn))));

            return testOccurrences;
        }

        public HashSet<string> GetTestOccurrenceNamesForAllEnvironments()
        {
            // The buildId of the latest build for each buildType
            var buildIds = _uow.Builds.GetAll()
                .GroupBy(b => b.BuildTypeId,
                    (key, values) =>
                        new
                        {
                            BuildTypeId = key,
                            RunAt = values.Max(b => b.RunAt),
                            TeamCityBuildId = values.Max(b => b.TeamCityBuildId)
                        })
                .Select(b => b.TeamCityBuildId);

            var result =  GetAllTestOccurrenceNames(buildIds);

            return result;
        }

        public HashSet<string> GetAllTestOccurrenceNames(IEnumerable<long> teamCityBuildIds)
        {
            var allTestOccurrenceNames = new HashSet<string>();
            teamCityBuildIds.ToList().ForEach(id =>
            {
                var testNames = _uow.TestOccurrences.GetAll().Where(to => to.Build.TeamCityBuildId == id).Select(to => to.Name);
                allTestOccurrenceNames.UnionWith(testNames);
            });

            return allTestOccurrenceNames;
        }
    }
}