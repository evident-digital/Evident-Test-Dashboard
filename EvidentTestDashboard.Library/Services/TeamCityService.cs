using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using Newtonsoft.Json;
using System.Linq;
using System.Threading.Tasks;
using EvidentTestDashboard.Library.Contracts;
using EvidentTestDashboard.Library.DTO;
using EvidentTestDashboard.Library.Entities;
using Newtonsoft.Json.Linq;
using static System.Configuration.ConfigurationManager;

namespace EvidentTestDashboard.Library.Services
{
    public class TeamCityService : ITeamCityService
    {
        private static readonly string TC_BUILD_TYPE_ID = AppSettings["teamCity:buildTypeId:Selenium"];
        private static readonly string TC_BASE_URI = AppSettings["teamCity:uri:base"];
        private static readonly string TC_BUILDS_URI = $@"{AppSettings["teamCity:uri:base"]}{AppSettings["teamCity:uri:builds"]}";
        private static readonly string TC_TEST_OCCURRENCES_URI = $@"{AppSettings["teamCity:uri:base"]}{AppSettings["teamCity:uri:tests"]}";

        private readonly HttpClient _client;


        public TeamCityService()
        {
            _client = new HttpClient();

            // In format "userName:password" for HTTP Basic auth
            var authenticationInfoByteArray = Encoding.ASCII.GetBytes($@"{AppSettings["userName"]}:{AppSettings["password"]}");
            _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Basic", Convert.ToBase64String(authenticationInfoByteArray));
            _client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
        }

        public async Task<TestOccurrenceCollectionDTO> GetLatestBuildTestDataAsync()
        {
            TestOccurrenceCollectionDTO result = null;
            var latestBuildId = await GetLatestBuildIdAsync();
            if (latestBuildId != -1)
            {
                var testResultUri = $"{TC_TEST_OCCURRENCES_URI}?locator=build:{latestBuildId}";
                var testOccurrencesJson = await _client.GetStringAsync(testResultUri);
                result = JsonConvert.DeserializeObject<TestOccurrenceCollectionDTO>(testOccurrencesJson);
            }

            return result;
        }

        public async Task<BuildDTO> GetLatestBuild()
        {
            var buildsJson = await _client.GetStringAsync(TC_BUILDS_URI);
            var buildsCollection = JsonConvert.DeserializeObject<BuildCollectionDTO>(buildsJson);

            var latestBuildId = await GetLatestBuildIdAsync();
            var latestBuildJson = await _client.GetStringAsync($"{TC_BUILDS_URI}/id:{latestBuildId}");
            var latestBuild = JsonConvert.DeserializeObject<BuildDTO>(latestBuildJson);

            return latestBuild;
        }

        public async Task<IEnumerable<TestOccurrenceDTO>> GetTestOccurrencesForBuildAsync(long buildId)
        {
            string testOccurrencesJson =
                await _client.GetStringAsync($"{TC_TEST_OCCURRENCES_URI}?locator=build:{buildId}");
            var testOccurrencesCollection = JsonConvert.DeserializeObject<TestOccurrenceCollectionDTO>(testOccurrencesJson);

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
             //return testName.Replace("<add test name>", "").Split('.').FirstOrDefault();
            return string.Empty;
        }

        private async Task<int> GetLatestBuildIdAsync()
        {
            var latestBuildId = -1;

            String buildsJson = await _client.GetStringAsync(TC_BUILDS_URI);
            var buildsCollection = JsonConvert.DeserializeObject<BuildCollectionDTO>(buildsJson);

            var latestBuild = 
                buildsCollection.Builds
                    .Where(b => b.BuildTypeId == TC_BUILD_TYPE_ID)
                    .Aggregate((b1, b2) => b1.Id > b2.Id ? b1 : b2);

            if (latestBuild != null)
            {
                latestBuildId = latestBuild.Id;
            }

            return latestBuildId;
        }
    }
}