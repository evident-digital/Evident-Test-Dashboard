namespace EvidentTestDashboard.Library.DTO
{
    public class TestOccurrenceDTO
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public string Status { get; set; }
        public string Href { get; set; }
        public int? Duration { get; set; }
        public string Details { get; set; }
        public string LabelName { get; set; }
        /// <summary>
        /// Gets or sets the first failed.
        /// </summary>
        /// <value>
        /// The first failed, if Status is FAILURE and FirstFailed is null, this is the first failure..
        /// </value>
        public FirstFailedDTO FirstFailed { get; set; }
    }
}