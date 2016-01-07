using System.Collections.Generic;
using System.Threading.Tasks;
using EvidentTestDashboard.Library.DTO;
using System;

namespace EvidentTestDashboard.Library.Services
{
    public interface ITeamCityService
    {
        Task<IEnumerable<TestOccurrenceDTO>> GetTestOccurrencesForBuildAsync(long buildId);
        Task<IEnumerable<BuildDTO>> GetBuildsAsync(IEnumerable<string> buildTypes, DateTime sinceDate);
    }
}