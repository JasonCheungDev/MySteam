using System;
using Xunit;
using MySteam.Data;

namespace TDDMySteam
{
    public class UnitTest1
    {
        const string API_KEY = "758AEEE709F200A44D5A076B68F7636F";
        const string TEST_ACC_URL = "todo get url";

        [Fact]
        public void GetPublicResultFromApi()
        {
            ApiHelper api = new ApiHelper();
            
            String result = ApiHelper.Instance.GetMethods();
            Assert.NotNull(result);
            // ApiHelper 
            // DataAnalyzer 
        }

        [Fact]
        public void GetKeyResultFromApi()
        {
            ApiHelper api = new ApiHelper();
            api.SetKey(API_KEY);
            String result = api.GetKeyMethods();
            Assert.NotNull(result);
        }

        [Fact]
        public void GetPublicUser()
        {
            ApiHelper api = new ApiHelper();
            api.SetKey(API_KEY);
            SteamModel player = api.GetUserProfile(TEST_ACC_URL);
            Assert.Equals(player.Username, "Jason");
        }

        [Fact]
        public void AttemptKeyCallWithoutKey()
        {
            ApiHelper api = new ApiHelper();
            
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

            ApiHelper api = new ApiHelper();
            api.SetKey(API_KEY);
            SteamModel player = api.GetUserProfile(private_url);

            Assert.Equals(player.Visibility, Visibility.Private);
            // Assert returned view has warning message about limited data
        }

        [Fact]
        public void GetTotalGames_ShouldEqual50()
        {
            ApiHelper api = new ApiHelper();
            api.SetKey(API_KEY);
            var data = api.GetGames(TEST_ACC_URL);

            Assert.Equals(data.Length, 50);
        }

        [Fact]
        public void CalculateTotalPlayTime_ShouldEqual100()
        {
            ApiHelper api = new ApiHelper();
            api.SetKey(API_KEY);
            var data = api.GetGames(TEST_ACC_URL);

            DataAnalyzer analyzer = new DataAnalyzer();
            float totalTime = analyzer.CalculateTotalTimePlayed(data);

            Assert.Equals(totalTime, 100);
        }

    }
}
