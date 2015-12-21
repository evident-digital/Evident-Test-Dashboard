using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace EvidentTestDashboard.Library.Entities
{
    public class Label
    {
        public Label()
        {
            TestOccurrences = new List<TestOccurrence>();
        }

        public int LabelId { get; set; }
        public string LabelName { get; set; }
        public int DashboardId { get; set; }
        
        public Dashboard Dashboard { get; set; }
        public ICollection<TestOccurrence> TestOccurrences { get; set; } 
    }
}