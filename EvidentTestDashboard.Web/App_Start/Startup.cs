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

            app.UseHangfireDashboard();
            app.UseHangfireServer();
        }
    }
}