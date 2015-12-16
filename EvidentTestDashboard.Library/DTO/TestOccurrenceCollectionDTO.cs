using System.Collections.Generic;
using Newtonsoft.Json;

namespace EvidentTestDashboard.Library.DTO
{
    public class TestOccurrenceCollectionDTO
    {
        public int Count { get; set; }
        public string Href { get; set; }
        [JsonProperty(PropertyName = "TestOccurrence")]
        public List<TestOccurrenceDTO> TestOccurrences { get; set; }
        public bool Default { get; set; }
    }
}