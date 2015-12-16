using System;
using System.Globalization;
using EvidentTestDashboard.Library.DTO;
using EvidentTestDashboard.Library.Entities;

namespace EvidentTestDashboard.Library.Factories
{
    public class BuildFactory
    {
        private static BuildFactory _instance;
        public static BuildFactory Instance {
            get
            {
                if (_instance == null)  _instance = new BuildFactory();
                return _instance;
            }
        }

        public Build Create(BuildDTO build)
        {
            return new Build()
            {
                TeamCityBuildId = build.Id,
                Number = build.Number,
                Href = build.Href,
                WebUrl = build.WebUrl,
                BuildSucceeded = ParseBuildStatus(build.Status),
                RunAt = DateTime.ParseExact(build.StartDate, "yyyyMMdd'T'HHmmsszzz", CultureInfo.InvariantCulture),
                Passed = build.TestOccurrences.Passed,
                Failed = build.TestOccurrences.Failed,
                TotalTestsRun = build.TestOccurrences.Count,
                State = build.State,
                Status = build.Status
            };
        }

        private bool ParseBuildStatus(string status)
        {
            if (status == Build.BUILD_SUCCESS) return true;
            return false;
        }
    }
}