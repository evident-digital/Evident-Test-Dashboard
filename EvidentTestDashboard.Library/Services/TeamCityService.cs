using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using EvidentTestDashboard.Library.DTO;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using static System.Configuration.ConfigurationManager;

namespace EvidentTestDashboard.Library.Services
{
    public class TeamCityService : ITeamCityService
    {
        private static readonly string SELENIUM_TEST_PREFIX = $"{AppSettings["selenium:testPrefix"]}{"."}";
        private static readonly string TC_BASE_URI = AppSettings["teamCity:uri:base"];
        private static readonly string TC_BUILDS_URI = $@"{AppSettings["teamCity:uri:base"]}{AppSettings["teamCity:uri:builds"]}";
        private static readonly string TC_TEST_OCCURRENCES_URI = $@"{AppSettings["teamCity:uri:base"]}{AppSettings["teamCity:uri:tests"]}";

        private readonly HttpClient _client;


        public TeamCityService()
        {
            _client = new HttpClient();

            // In format "userName:password" for HTTP Basic auth
            var authenticationInfoByteArray =
                Encoding.ASCII.GetBytes($@"{AppSettings["userName"]}:{AppSettings["password"]}");
            _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Basic",
                Convert.ToBase64String(authenticationInfoByteArray));
            _client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
        }

        public async Task<IDictionary<string, BuildDTO>> GetLatestBuildsAsync(IEnumerable<string> buildTypes)
        {
            var result = new Dictionary<string, BuildDTO>();

            var latestBuildIds = await GetLatestBuildIdsAsync(buildTypes);
            foreach (var buildType in latestBuildIds.Keys)
            {
                var buildJson = await _client.GetStringAsync($"{TC_BUILDS_URI}/id:{latestBuildIds[buildType]}");
                var build = JsonConvert.DeserializeObject<BuildDTO>(buildJson);
                result.Add(buildType, build);
            }

            return result;
        }

        public async Task<IEnumerable<TestOccurrenceDTO>> GetTestOccurrencesForBuildAsync(long buildId)
        {
            var testOccurrencesJson =
                await _client.GetStringAsync($"{TC_TEST_OCCURRENCES_URI}?locator=build:{buildId}");
            var testOccurrencesCollection =
                JsonConvert.DeserializeObject<TestOccurrenceCollectionDTO>(testOccurrencesJson);

            foreach (var test in testOccurrencesCollection.TestOccurrences)
            {
                var testExtendedInfoJson = await _client.GetStringAsync($"{TC_BASE_URI}{test.Href}");
                var testExtendedInfo = JObject.Parse(testExtendedInfoJson);
                test.Details = testExtendedInfo.Value<string>("details");
                test.LabelName = GetLabelName(test.Name);
            }

            return testOccurrencesCollection.TestOccurrences;
        }

        private string GetLabelName(string testName)
        {
            return testName.Replace(SELENIUM_TEST_PREFIX, "").Replace("V2", "").Split('.').FirstOrDefault();
        }

        private async Task<IDictionary<string, int>> GetLatestBuildIdsAsync(IEnumerable<string> buildTypeIds)
        {
            var result = new Dictionary<string, int>();

            var buildsJson = await _client.GetStringAsync(TC_BUILDS_URI);
            var buildsCollection = JsonConvert.DeserializeObject<BuildCollectionDTO>(buildsJson);

            foreach (var buildType in buildTypeIds)
            {
                if (buildsCollection.Builds.Any(b => b.BuildTypeId == buildType))
                {
                    var latestBuild =
                        buildsCollection.Builds
                            .Where(b => b.BuildTypeId == buildType)
                            .Aggregate((b1, b2) => b1.Id > b2.Id ? b1 : b2);

                    if (latestBuild != null)
                    {
                        result.Add(buildType, latestBuild.Id);
                    }
                }
            }

            return result;
        }
    }
}