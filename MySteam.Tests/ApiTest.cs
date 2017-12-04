using Microsoft.EntityFrameworkCore;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MySteam.Data;
using MySteam.Models;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;

namespace MySteam.Tests
{
    [TestClass]
    public class ApiTest
    {
        const string API_KEY = "758AEEE709F200A44D5A076B68F7636F";
        const string TEST_ACC_URL = "76561198020784166";
        const string TEST_OLD_ACC_URL = "76561198051910246";
        // http://steamcommunity.com/profiles/76561198020784166/

        public ApiTest()
        {
            Trace.Listeners.Add(new TextWriterTraceListener(Console.Out));
            InitContext();
        }

        public void InitContext()
        {
            var builder = new DbContextOptionsBuilder<MySteamContext>()
                .UseInMemoryDatabase("TestContext");
            var context = new MySteamContext(builder.Options);
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
            api.SetKey(API_KEY);
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
        [ExpectedException(typeof(MissingSteamIDException))]
        public async Task TryGetUserWithNoId_ShouldThrowMissingIdException()
        {
            ApiHelper api = ApiHelper.Instance;
            api.SetKey(API_KEY);
            var player = await api.GetUser("");
            Assert.IsNull(player);
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidSteamIDException))]
        public async Task TryGetUserWithInvalidLength_ShouldThrowInvalidSteamIdException()
        {
            ApiHelper api = ApiHelper.Instance;
            api.SetKey(API_KEY);
            var player = await api.GetUser("This is totally a legit Steam ID!");
            Assert.IsNull(player);
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidSteamIDException))]
        public async Task TryGetUserWithInvalidFormat_ShouldThrowInvalidSteamIdException()
        {
            ApiHelper api = ApiHelper.Instance;
            api.SetKey(API_KEY);
            var player = await api.GetUser("123456789012345!@");
            Assert.IsNull(player);
        }

        [TestMethod]
        public async Task GetTestUser_NameIsXaieon()
        {
            ApiHelper api = ApiHelper.Instance;
            api.SetKey(API_KEY);
            var player = await api.GetUser(TEST_ACC_URL);
            Assert.AreEqual("Xaieon", player.personaname);
        }

        [TestMethod]
        public async Task GetAllGames_ShouldCountTo50()
        {
            ApiHelper api = ApiHelper.Instance;
            api.SetKey(API_KEY);
            
            var data = await api.GetGamesForUser(TEST_ACC_URL, true);

            Assert.AreEqual(93, data.Count);
        }

        [TestMethod]
        public async Task CalculateTotalPlayTime_ShouldEqual100()
        {
            ApiHelper api = ApiHelper.Instance;
            api.SetKey(API_KEY);

            var data = await api.GetGamesForUser(TEST_ACC_URL, true);
            int totalTime = DataAnalyzer.CalculateTotalTimePlayed(data);

            Assert.AreEqual(93057, totalTime);
        }

        [TestMethod]
        public async Task FindMostPlayedGame_ShouldBeNier()
        {
            ApiHelper api = ApiHelper.Instance;
            api.SetKey(API_KEY);

            var data = await api.GetGamesForUser(TEST_ACC_URL, true);
            SimpleGameModel game = DataAnalyzer.FindMostPlayedGame(data);

            Assert.AreEqual("Nier", game.name);
        }

        [TestMethod]
        public async Task TestIfDuplicateRequest_UsesDatabase()
        {
            ApiHelper api = ApiHelper.Instance;

            var id = new List<int>();
            id.Add(400);

            var data1 = await api.GetDetailedGameInfos(id);
            var data2 = await api.GetDetailedGameInfos(id);

            // Console should print first from API, second from DB
        }

        [TestMethod]
        public async Task FindTotalGameWorth()
        {
            ApiHelper api = ApiHelper.Instance;
            api.SetKey(API_KEY);

            var data = await api.GetGamesForUser(TEST_OLD_ACC_URL, true);
            var appIds = data.Select(sgm => sgm.appid).ToList();
            var detailedData = await api.GetDetailedGameInfos(appIds);
            
            float worth = DataAnalyzer.CalculateTotalGameWorth(detailedData);

            Assert.AreEqual(1000, worth);
        }

        [TestMethod]
        public async Task FindMostExpensiveGame_ShouldBeNier()
        {
            ApiHelper api = ApiHelper.Instance;
            api.SetKey(API_KEY);

            var data = await api.GetGamesForUser(TEST_ACC_URL, true);
            SimpleGameModel game = DataAnalyzer.FindMostPlayedGame(data);

            Assert.AreEqual("Nier", game.name);
        }

        [TestMethod]
        public async Task FindLeastExpensiveGame_ShouldBeFuri()
        {
            ApiHelper api = ApiHelper.Instance;
            api.SetKey(API_KEY);

            var data = await api.GetGamesForUser(TEST_ACC_URL, true);
            SimpleGameModel game = DataAnalyzer.FindMostPlayedGame(data);

            Assert.AreEqual("Nier", game.name);
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
