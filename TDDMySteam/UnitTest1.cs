using System;
using Xunit;
using MySteam.Data;

namespace TDDMySteam
{
    public class UnitTest1
    {
        const string API_KEY = "758AEEE709F200A44D5A076B68F7636F";
        const string TEST_ACC_URL = "todo get url";

        // Warning: bad code!
        [Fact]
        public async System.Threading.Tasks.Task IncorrectlyPassingTestAsync()
        {
            await ApiHelper.Instance.SimpleAsync();
        }

        /*
        [Fact]
        public async void GetPublicResultFromApi()
        {
            ApiHelper api = ApiHelper.Instance;
            Assert.NotNull(result);
            // ApiHelper 
            // DataAnalyzer 
        }

        [Fact]
        public void GetKeyResultFromApi()
        {
            ApiHelper api = ApiHelper.Instance;
            api.SetKey(API_KEY);
            String result = api.GetKeyMethods();
            Assert.NotNull(result);
        }

        [Fact]
        public void GetPublicUser()
        {
            ApiHelper api = ApiHelper.Instance;
            api.SetKey(API_KEY);
            SteamModel player = api.GetUserProfile(TEST_ACC_URL);
            Assert.Equals(player.Username, "Jason");
        }

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
