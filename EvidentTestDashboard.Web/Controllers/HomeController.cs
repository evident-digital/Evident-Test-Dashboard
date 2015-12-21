using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Web.Mvc;
using EvidentTestDashboard.Library.Contracts;
using EvidentTestDashboard.Library.Entities;
using EvidentTestDashboard.Web.ViewModels;
using static System.Configuration.ConfigurationManager;

namespace EvidentTestDashboard.Web.Controllers
{
    public class HomeController : Controller
    {
        private static readonly string DEFAULT_LABEL = AppSettings["label:default"];

        private readonly ITestDashboardUOW _uow;

        public HomeController(ITestDashboardUOW uow)
        {
            _uow = uow;
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
                    var testOccurrences = new Dictionary<string, IEnumerable<TestOccurrence>>();
                    labels.ToList().ForEach(l => testOccurrences.Add(
                        l.LabelName,
                        _uow.TestOccurrences.GetAll()
                            .Where(to => to.Build.BuildId == build.BuildId && to.Label.LabelName == l.LabelName)));

                    var buildViewModel = new BuildViewModel()
                    {
                        Build = build,
                        Environment = e,
                        TestOccurrences = testOccurrences
                    };

                    model.builds.Add(buildViewModel);
                }
            });
            
            return View(model);
        }
    }
}