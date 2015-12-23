using System;
using System.Collections.Generic;
using EvidentTestDashboard.Library.Entities;
using Environment = EvidentTestDashboard.Library.Entities.Environment;

namespace EvidentTestDashboard.Web.ViewModels
{
    public class AggregateBuildsViewModel
    {
        public DateTime LastRun { get; set; }
        public int TestsPassed { get; set; }
        public int TestsFailed { get; set; }
        public int TestsNotRun { get; set; }
        public bool BuildSucceeded { get; set; }
        public string EnvironmentName { get; set; }
        public IDictionary<string, List<TestOccurrence>> TestOccurrences { get; set; }
    }
}