using System.ComponentModel.DataAnnotations;

namespace EvidentTestDashboard.Library.Entities
{
    public class TestOccurrence
    {
        public static readonly string TEST_OCCURRENCE_SUCCESS = "SUCCESS";
        public static readonly string TEST_OCCURRENCE_FAILURE = "FAILURE";

        [Key]
        public string TestOccurrenceId { get; set; }

        public string TeamCityTestOccurrenceId { get; set; }
        public string Name { get; set; }
        public string Status { get; set; }
        public string Href { get; set; }
        public int? Duration { get; set; }
        public string Details { get; set; }
        public bool TestOccurrenceSucceeded { get; set; }

        public int BuildId { get; set; }
        public int LabelId { get; set; }

        public virtual Build Build { get; set; }
        public virtual Label Label { get; set; }
    }
}