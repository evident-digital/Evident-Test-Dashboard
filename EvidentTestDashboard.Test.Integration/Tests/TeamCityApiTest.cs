using Xunit;
using EvidentTestDashboard.Library.Services;

namespace EvidentTestDashboard.Test.Integration.Tests
{
    public class TeamCityApiTest
    {
        [Fact]
        public async void ShouldGetInfoFromTeamCity()
        {
            var sut = new TeamCityService();
            var result =  await sut.GetLatestBuildTestDataAsync();

            Assert.NotNull(result);
        }

        [Fact]
        public async void ShouldReturnLatestBuild()
        {
            var sut = new TeamCityService();
            var result = await sut.GetLatestBuild();

            Assert.NotNull(result);
        }
    }
}
