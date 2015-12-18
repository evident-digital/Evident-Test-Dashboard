using System;
using System.ComponentModel.DataAnnotations;
using System.Data.Entity.Validation;
using EvidentTestDashboard.Library.Repositories;
using EvidentTestDashboard.Library.Services;
using EvidentTestDashboard.Web.Jobs;
using Xunit;

namespace EvidentTestDashboard.Test.Integration.Tests
{
    public class BuildInformationJobTest
    {
        [Fact]
        public async void ShouldAddLatestBuildToDb()
        {
            var sut = new BuildInformationJob(new TestDashboardUOW(), new TeamCityService());

            try
            {
                await sut.CollectBuildDataAsync();

            }
            catch (DbEntityValidationException ex)
            {
                
                throw;
            }

            string bla = "";
        }
    }
}