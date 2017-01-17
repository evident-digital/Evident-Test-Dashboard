using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Configuration.ConfigurationManager;

namespace EvidentTestDashboard.Library
{
    public class Settings
    {
        public static string DefaultDashboard { get { return AppSettings["testDashboard:defaultDashboard"]; } }
        public static int TeamCitySyncDays
        {
            get
            {
                int result = -1;
                int.TryParse(AppSettings["teamCity:syncDays"], out result);
                if(result > 0)
                {
                    result = 0 - result;
                }
                return result;
            }
        }
        public static string TeamCityUserName { get { return AppSettings["teamCity:userName"]; } }
        public static string TeamCityPassword { get { return AppSettings["teamCity:password"]; } }
        public static string TeamCityBaseUri { get { return AppSettings["teamCity:uri:base"]; } }
        public static string TeamCityBuildsUri { get { return $"{TeamCityBaseUri}{AppSettings["teamCity:uri:builds"]}"; } }
        public static string TeamCityTestOccurrencesUri { get { return $"{TeamCityBaseUri}{AppSettings["teamCity:uri:tests"]}"; } }
    }
}
