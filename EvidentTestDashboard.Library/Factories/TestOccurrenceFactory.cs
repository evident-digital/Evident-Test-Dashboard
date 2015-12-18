using System.Linq;
using EvidentTestDashboard.Library.DTO;
using EvidentTestDashboard.Library.Entities;
using static System.Configuration.ConfigurationManager;

namespace EvidentTestDashboard.Library.Factories
{
    public class TestOccurrenceFactory
    {
        private static TestOccurrenceFactory _instance;
        public static TestOccurrenceFactory Instance {
            get
            {
                if (_instance == null) _instance = new TestOccurrenceFactory();
                return _instance;
            }
        }

        public TestOccurrence Create(TestOccurrenceDTO testOccurrence)
        {
            return new TestOccurrence()
            {
                TeamCityTestOccurrenceId = testOccurrence.Id,
                Duration = testOccurrence.Duration.HasValue ? testOccurrence.Duration : 0,
                Status = testOccurrence.Status,
                Href = testOccurrence.Href,
                Name = testOccurrence.LabelName,
                Details = testOccurrence.Details,
                TestOccurrenceSucceeded = ParseTestOccurrenceStatus(testOccurrence.Status)
            };
        }

        private bool ParseTestOccurrenceStatus(string status)
        {
            if (status == TestOccurrence.TEST_OCCURRENCE_SUCCESS) return true;
            return false;
        }

        
    }
}