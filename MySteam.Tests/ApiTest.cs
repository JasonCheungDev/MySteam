using Microsoft.VisualStudio.TestTools.UnitTesting;
using MySteam.Data;
using MySteam.Models;
using System;
using System.Diagnostics;
using System.Threading.Tasks;

namespace MySteam.Tests
{
    [TestClass]
    public class ApiTest
    {
        const string API_KEY = "758AEEE709F200A44D5A076B68F7636F";
        const string TEST_ACC_URL = "76561198020784166";
        // http://steamcommunity.com/profiles/76561198020784166/

        public ApiTest()
        {
            Trace.Listeners.Add(new TextWriterTraceListener(Console.Out));
        }

        [TestMethod]
        public async Task PublicApiResultsShouldNotBeNull()
        {
            var api = ApiHelper.Instance;
            var result = await api.GetPublicMethods();
            Trace.WriteLine(result);
            Assert.IsNotNull(result);
            Assert.IsFalse(String.IsNullOrEmpty(result));
        }

        [TestMethod]
        public async Task ApiKeyResultsShouldNotBeNull()
        {
            var api = ApiHelper.Instance;
            var result = await ApiHelper.Instance.GetPrivateMethods();
            Trace.WriteLine(result);
            Assert.IsNotNull(result);
            Assert.IsFalse(String.IsNullOrEmpty(result));
        }

        [TestMethod]
        public async Task ApiKeyResults_GreaterThanPublicApiResults()
        {
            var api = ApiHelper.Instance;

            var publicResults = await api.GetPublicMethods();

            api.SetKey(API_KEY);

            var keyResults = await api.GetPrivateMethods();

            Assert.IsFalse(String.IsNullOrEmpty(publicResults));
            Assert.IsFalse(String.IsNullOrEmpty(keyResults));
            Assert.IsTrue(keyResults.Length > publicResults.Length, String.Format("Public results {0} has equal or less than API key results {1}", publicResults.Length, keyResults.Length));
        }

        [TestMethod]
        [ExpectedException(typeof(MissingApiKeyException))]
        public async Task CallApiWithoutKey_ShouldThrowException()
        {
            ApiHelper api = ApiHelper.Instance;
            // do not set key

            var player = await api.GetUser(TEST_ACC_URL);

            // Assert Exception - MissingApiKeyException
        }

        [TestMethod]
        public async Task GetTestUser_NameIsXaieon()
        {
            ApiHelper api = ApiHelper.Instance;
            api.SetKey(API_KEY);
            var player = await api.GetUser(TEST_ACC_URL);
            Assert.AreEqual("Xaieon", player.personaname);
        }


        /*
        [Fact]
        public void AttemptKeyCallWithoutKey()
        {
            ApiHelper api = ApiHelper.Instance;

            try
            {
                SteamModel player = api.GetUserProfile(TEST_ACC_URL);
                Assert.Fail();
            }


            Assert.Equals(player.Username, "Jason");
        }

        [Fact]
        public void GetPrivateUser()
        {
            const string private_url = "todo get private acc";

            ApiHelper api = ApiHelper.Instance;
            api.SetKey(API_KEY);
            SteamModel player = api.GetUserProfile(private_url);

            Assert.Equals(player.Visibility, Visibility.Private);
            // Assert returned view has warning message about limited data
        }

        [Fact]
        public void GetTotalGames_ShouldEqual50()
        {
            ApiHelper api = ApiHelper.Instance;
            api.SetKey(API_KEY);
            var data = api.GetGames(TEST_ACC_URL);

            Assert.Equals(data.Length, 50);
        }

        [Fact]
        public void CalculateTotalPlayTime_ShouldEqual100()
        {
            ApiHelper api = ApiHelper.Instance;
            api.SetKey(API_KEY);
            var data = api.GetGames(TEST_ACC_URL);

            DataAnalyzer analyzer = new DataAnalyzer();
            float totalTime = analyzer.CalculateTotalTimePlayed(data);

            Assert.Equals(totalTime, 100);
        }
        */
    }
}
