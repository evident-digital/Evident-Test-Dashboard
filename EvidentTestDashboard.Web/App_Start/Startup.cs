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

            app.UseHangfireDashboard();
            app.UseHangfireServer();
        }
    }
}