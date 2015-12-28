using EvidentTestDashboard.Library.Repositories;
using EvidentTestDashboard.Library.Services;
using EvidentTestDashboard.Web.Authorization;
using EvidentTestDashboard.Web.Jobs;
using Hangfire;
using Microsoft.Owin;
using Owin;

[assembly: OwinStartup(typeof(EvidentTestDashboard.Web.Startup))]

namespace EvidentTestDashboard.Web
{
    public class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            GlobalConfiguration.Configuration.UseSqlServerStorage("default");
            GlobalConfiguration.Configuration.UseNinjectActivator(
                                     new Ninject.Web.Common.Bootstrapper().Kernel);

            app.UseHangfireDashboard("/hangfire", new DashboardOptions
            {
                AuthorizationFilters = new[] { new NoAuthorizationFilter() }
            });
            app.UseHangfireServer();
            new HangfireJobs().Register();
        }
    }
}