using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using EvidentTestDashboard.Library.Contracts;
using EvidentTestDashboard.Library.Entities;
using EvidentTestDashboard.Library.Services;
using EvidentTestDashboard.Web.ViewModels;
using Environment = EvidentTestDashboard.Library.Entities.Environment;
using EvidentTestDashboard.Library;

namespace EvidentTestDashboard.Web.Controllers
{
    public class HomeController : Controller
    {

        private readonly ITestDashboardUOW _uow;
        private readonly ITeamCityService _teamCityService;

        public HomeController(ITestDashboardUOW uow, ITeamCityService teamCityService)
        {
            _uow = uow;
            _teamCityService = teamCityService;
        }

        public ActionResult Index()
        {
            var dashboard = _uow.Dashboards.GetAll().SingleOrDefault(d => d.DashboardName == Settings.DefaultDashboard);

            var model = new DashboardViewModel()
            {
                Dashboard = dashboard
            };

            var environments = _uow.Environments.GetAll().Where(e => e.DashboardId == dashboard.DashboardId);
            var labels = _uow.Labels.GetAll().Where(l => l.Dashboard.DashboardId == dashboard.DashboardId).ToList();
            environments.ToList().ForEach(e =>
            {
                if (_uow.Builds.GetAll().Any(b => b.BuildType.EnvironmentId == e.EnvironmentId))
                {
                    // There can be multiple latest builds (one for each BuildType for the given environment)
                    var builds = GetLatestBuildsForEnvironment(e);

                    var testOccurrences = GetTestOccurrencesForBuildsPerLabel(builds, labels);

                    SetNotRunTests(testOccurrences, dashboard.DashboardId);
                    SortTestNamesPerLabel(testOccurrences);

                    // Contains data for 1 or more builds per Environment
                    var buildViewModel = new AggregateBuildsViewModel()
                    {
                        TestsNotRun = GetCountTestOccurrencesNotRun(testOccurrences),
                        TestsPassed = builds.Sum(b => b.Passed),
                        TestsFailed = builds.Sum(b => b.Failed),
                        BuildSucceeded = builds.All(b => b.BuildSucceeded),
                        EnvironmentName = e.EnvironmentName,
                        TestOccurrences = testOccurrences,
                        LastRun = builds.Max(b => b.RunAt)
                    };

                    model.builds.Add(buildViewModel);
                }
            });
            
            return View(model);
        }

        private Dictionary<string, List<TestOccurrence>> GetTestOccurrencesForBuildsPerLabel(IEnumerable<Build> builds, List<Label> labels)
        {
            var testOccurrences = new Dictionary<string, List<TestOccurrence>>();

            foreach (var build in builds)
            {
                labels.ForEach(l =>
                {
                    var tests = _uow.TestOccurrences.GetAll()
                        .Where(to => to.Build.BuildId == build.BuildId && to.Label.LabelName == l.LabelName)
                        .OrderBy(t => t.Name)
                        .ToList();
                    if (testOccurrences.ContainsKey(l.LabelName))
                        testOccurrences[l.LabelName].AddRange(tests);
                    else
                        testOccurrences.Add(l.LabelName, tests);
                });
            }

            return testOccurrences;
        }

        private IEnumerable<Build> GetLatestBuildsForEnvironment(Environment e)
        {
            var builds = _uow.Builds.GetAll()
                .Where(b => b.BuildType.EnvironmentId == e.EnvironmentId)
                .GroupBy(b => b.BuildType,
                    (key, values) => values.Where(bt => bt.RunAt == values.Max(x => x.RunAt)))
                .SelectMany(x => x)
                .ToList();
            return builds;
        }

        private int GetCountTestOccurrencesNotRun(Dictionary<string, List<TestOccurrence>> testOccurrences)
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
            foreach (KeyValuePair<Label, IEnumerable<TestOccurrence>> entry in allTestNames)
            {
                var missingTests = entry.Value.Where(
                    to => !testOccurrences[entry.Key.LabelName]
                        .Select(t => t.Name)
                        .Contains(to.Name))
                    .Select(to => new TestOccurrence() {Name = to.Name, Status = TestOccurrence.TEST_OCCURRENCE_NOT_RUN});

                testOccurrences[entry.Key.LabelName].AddRange(missingTests);
            }
        }

        public IDictionary<Label, IEnumerable<TestOccurrence>> GetAllTestNamesPerLabel(int dashboardId)
        {
            // Get the tests for each BuildTypes latest build
            List<TestOccurrence> testOccurrences = new List<TestOccurrence>();
            var latestBuilds = GetLatestBuildsPerBuildType(dashboardId);
            latestBuilds.ToList().ForEach(b =>
            {
                testOccurrences.AddRange(_uow.TestOccurrences.GetAll().Where(to => to.BuildId == b.BuildId));
            });

            // Group the tests per label
            var testOccurrencesPerLabel = testOccurrences
                .GroupBy(to => to.Label)
                .ToDictionary(g => g.Key, g => g.Key.TestOccurrences.AsEnumerable());

            return testOccurrencesPerLabel;
        }

        public IEnumerable<Build> GetLatestBuildsPerBuildType(int dashboardId)
        {
            var builds = _uow.Builds.GetAll()
                .Where(b => b.BuildType.Environment.DashboardId == dashboardId)
                .GroupBy(b => b.BuildTypeId,
                    (key, values) => values.Where(b => b.RunAt == values.Max(x => x.RunAt)))
                .SelectMany(x => x)
                .ToList();

            return builds;
        }
    }
}