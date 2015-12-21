using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace EvidentTestDashboard.Library.Entities
{
    public class Dashboard
    {
        public Dashboard()
        {
            Environments = new List<Environment>();
        }

        [Key]
        public int DashboardId { get; set; }
        public string DashboardName { get; set; }
    
        public virtual ICollection<Environment> Environments { get; set; }
        public virtual ICollection<Label> Labels { get; set; }
    }
}