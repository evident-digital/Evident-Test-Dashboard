using EvidentTestDashboard.Library;
using EvidentTestDashboard.Library.Jobs;
using EvidentTestDashboard.Library.Repositories;
using EvidentTestDashboard.Library.Services;
using Hangfire;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace EvidentTestDashboard.Web.Jobs
{
    public class HangfireJobs
    {
        internal void Register()
        {
            RecurringJob.AddOrUpdate(() => SyncWithTeamCity(), Cron.Hourly);
        }

        public async void SyncWithTeamCity()
        {
            var job = new BuildInformationJob(new TestDashboardUOW(), new TeamCityService());
            await job.CollectBuildDataAsync(DateTime.Now.AddDays(Settings.TeamCitySyncDays));
        }
    }
}