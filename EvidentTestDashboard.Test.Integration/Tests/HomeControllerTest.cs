using EvidentTestDashboard.Library.Repositories;
using EvidentTestDashboard.Library.Services;
using EvidentTestDashboard.Web.Controllers;
using Xunit;

namespace EvidentTestDashboard.Test.Integration.Tests
{
    public class HomeControllerTest
    {
        [Fact]
        public void ShouldReturnLatestBuildIds()
        {
            var controller = new HomeController(new TestDashboardUOW(), new TeamCityService());
            controller.GetTestOccurrenceNamesForAllEnvironments();
        }

        [Fact]
        public void ShoudReturnAllTestNamesPerLabel()
        {
            var controller = new HomeController(new TestDashboardUOW(), new TeamCityService());
            var result = controller.GetAllTestNamesPerLabel(1);
        }
    }
}