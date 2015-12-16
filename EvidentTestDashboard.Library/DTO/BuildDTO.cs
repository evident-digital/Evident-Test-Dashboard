using System;
using System.Collections.Generic;

namespace EvidentTestDashboard.Library.DTO
{
    public class BuildDTO
    {
        public int Id { get; set; }
        public string BuildTypeId { get; set; }
        public string Number { get; set; }
        public string Status { get; set; }
        public string State { get; set; }
        public string Href { get; set; }
        public string WebUrl { get; set; }
        public string StatusText { get; set; }
        public BuildTypeDTO BuildType { get; set; }
        public string QueuedDate { get; set; }
        public string StartDate { get; set; }
        public string FinishDate { get; set; }
        public TriggeredDTO Triggered { get; set; }
        public AgentDTO Agent { get; set; }
        public TestStatisticsDTO TestOccurrences { get; set; }
        public PropertiesDTO Properties { get; set; }

        //public Statistics statistics { get; set; }
        //public LastChanges lastChanges { get; set; }
        //public Changes changes { get; set; }
        //public Revisions revisions { get; set; }
        //public ProblemOccurrences problemOccurrences { get; set; }
        //public Artifacts artifacts { get; set; }
        //public RelatedIssues relatedIssues { get; set; }
        //public Tags tags { get; set; }
    }

    public class TriggeredDTO
    {
        public string Type { get; set; }
        public string Details { get; set; }
        public string Date { get; set; }
    }

    public class TestStatisticsDTO
    {
        public int Count { get; set; }
        public string Href { get; set; }
        public int Passed { get; set; }
        public int Failed { get; set; }
        public int NewFailed { get; set; }
        public bool @Default { get; set; }
    }

    public class PropertyDTO
    {
        public string Name { get; set; }
        public string Value { get; set; }
    }

    public class PropertiesDTO
    {
        public int Count { get; set; }
        public List<PropertyDTO> Property { get; set; }
    }

    public class AgentDTO
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int TypeId { get; set; }
        public string Href { get; set; }
    }

    public class BuildTypeDTO
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public string ProjectName { get; set; }
        public string ProjectId { get; set; }
        public string Href { get; set; }
        public string WebUrl { get; set; }
    }
}