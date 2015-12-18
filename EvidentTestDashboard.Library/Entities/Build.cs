using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace EvidentTestDashboard.Library.Entities
{
    public class Build
    {
        public static readonly string BUILD_SUCCESS = "SUCCESS";
        public static readonly string BUILD_FAILURE = "FAILURE";

        public Build()
        {
            TestOccurrences = new List<TestOccurrence>();
        }

        [Key]
        public int BuildId { get; set; }

        public int TotalTestsRun { get; set; }
        public int Passed { get; set; }
        public int Failed { get; set; }
        public long TeamCityBuildId { get; set; }
        public DateTime RunAt { get; set; }
        public string Number { get; set; }
        public string Status { get; set; }
        public string State { get; set; }
        public string Href { get; set; }
        public string WebUrl { get; set; }
        public bool BuildSucceeded { get; set; }


        public int BuildTypeId { get; set; }

        public virtual BuildType BuildType { get; set; }
        public virtual ICollection<TestOccurrence> TestOccurrences { get; set; }
    }
}