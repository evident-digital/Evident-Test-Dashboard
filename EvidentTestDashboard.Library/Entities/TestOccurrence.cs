using System.ComponentModel.DataAnnotations;

namespace EvidentTestDashboard.Library.Entities
{
    public class TestOccurrence
    {
        [Key]
        public string TestOccurrenceId { get; set; }
        public string Name { get; set; }
        //public string Status { get; set; }
        //public string Href { get; set; }
        //public int? Duration { get; set; }

        public int BuildId { get; set; }

        public virtual Build Build { get; set; }
    }
}