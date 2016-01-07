using System.Collections.Generic;

namespace EvidentTestDashboard.Library.Entities
{
    public class BuildType
    {
        public BuildType() 
        {
            Builds = new List<Build>();
        }

        public int BuildTypeId { get; set; }
        public string BuildTypeName { get; set; }
        public int EnvironmentId { get; set; }
        public string RequiredParamName { get; set; }
        public string RequiredParamValue { get; set; }

        public virtual Environment Environment { get; set; }
        public virtual ICollection<Build> Builds { get; set; }
    }
}