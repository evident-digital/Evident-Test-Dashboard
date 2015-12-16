using System.Collections.Generic;
using Newtonsoft.Json;

namespace EvidentTestDashboard.Library.DTO
{
    public class BuildCollectionDTO
    {
        public int Count { get; set; }
        public string Href { get; set; }
        public string NextHref { get; set; }

        [JsonProperty(PropertyName = "build")]
        public List<BuildBriefDTO> Builds { get; set; }
    }
}