using System.Collections.Generic;

namespace EvidentTestDashboard.Library.Entities
{
    public class Environment
    {
        public Environment()
        {
            BuildTypes = new List<BuildType>();
        }

        public int EnvironmentId { get; set; }
        public string EnvironmentName { get; set; }
        public int DashboardId { get; set; }

        public virtual Dashboard Dashboard { get; set; }
        public virtual ICollection<BuildType> BuildTypes { get; set; }
    }
}