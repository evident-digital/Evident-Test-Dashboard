using System.Collections.Generic;
using EvidentTestDashboard.Library.Entities;

namespace EvidentTestDashboard.Library.DTO
{
    public class TestOccurrenceCollectionDTO
    {
        public int Count { get; set; }
        public string Href { get; set; }
        public List<TestOccurrence> TestOccurrence { get; set; }
        public bool Default { get; set; }
    }
}