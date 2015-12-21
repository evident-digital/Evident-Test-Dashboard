using System.Collections.Generic;
using EvidentTestDashboard.Library.Entities;

namespace EvidentTestDashboard.Web.ViewModels
{
    public class BuildViewModel
    {
        public Build Build { get; set; }
        public Environment Environment { get; set; }
        public IDictionary<string, IEnumerable<TestOccurrence>> TestOccurrences { get; set; }
    }
}