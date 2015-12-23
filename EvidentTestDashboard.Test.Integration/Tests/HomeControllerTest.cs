using EvidentTestDashboard.Library.Repositories;
using EvidentTestDashboard.Library.Services;
using EvidentTestDashboard.Web.Controllers;
using Xunit;

namespace EvidentTestDashboard.Test.Integration.Tests
{
    public class HomeControllerTest
    {
        [Fact]
        public void ShouldReturnLatestBuildsPerBuildType()
        {
            var controller = new HomeController(new TestDashboardUOW(), new TeamCityService());
            var result = controller.GetLatestBuildsPerBuildType(1);
        }
    }
}