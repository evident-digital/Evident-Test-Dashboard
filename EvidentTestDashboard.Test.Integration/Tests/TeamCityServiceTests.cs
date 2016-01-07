using EvidentTestDashboard.Library.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace EvidentTestDashboard.Test.Integration.Tests
{
    public class TeamCityServiceTests
    {
        [Fact]
        public async void ShouldGetBuildCollectionAsync()
        {
            var sut = new TeamCityService();
            var collection = await sut.GetBuildCollectionAsync(DateTime.Now.AddDays(-1.0));
            Assert.NotNull(collection);
        }
        [Fact]
        public async void ShouldGetBuildsAsync()
        {
            var sut = new TeamCityService();
            var builds = await sut.GetBuildsAsync(
                new string[] { "Ncoi_Sprint_SeleniumTestrun", "Ncoi_Sprint_IntegrationTestrun", "Ncoi_Main_TestSeleniumTestrun" }
                , DateTime.Now.AddDays(-1.0));
            Assert.NotNull(builds);
        }
    }
}
