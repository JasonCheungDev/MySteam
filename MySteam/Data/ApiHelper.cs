using MySteam.Models;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace MySteam.Data
{
    public class ApiHelper
    {
        private static ApiHelper mInstance;

        public static ApiHelper Instance
        {
            get
            {
                if (mInstance == null)
                    mInstance = new ApiHelper();
                 return mInstance;
            }
        }

        // need another client 

        private static HttpClient client = new HttpClient();

        private static HttpClient storeClient = new HttpClient();

        private static string apiKey;

        private static Regex alphanumericRegex = new Regex("^[a-zA-Z0-9]*$");

        private MySteamContext db; 


        private ApiHelper()
        {
            // 758AEEE709F200A44D5A076B68F7636F
            Console.WriteLine("ApiHelper start");

            client.BaseAddress = new Uri("http://api.steampowered.com/");
            client.DefaultRequestHeaders.Accept.Clear();
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

            storeClient.BaseAddress = new Uri("http://store.steampowered.com/api/");
            storeClient.DefaultRequestHeaders.Accept.Clear();
            storeClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

            Console.WriteLine("ApiHelper done");
        }

        public void SetContext(MySteamContext dbContext)
        {
            db = dbContext;
        }

        public string Details()
        {
            return client.BaseAddress.ToString();
        }

        public async Task SimpleAsync()
        {
            await Task.Delay(10);
            throw new Exception("Should fail.");
        }

        public static async Task SuperSimpleAsync()
        {
            await Task.Delay(10);
            throw new Exception("Should fail.");
        }

        public async Task<string> GetPublicMethods()
        {
            string result = null;
            HttpResponseMessage response = await client.GetAsync("ISteamWebAPIUtil/GetSupportedAPIList/v0001/");
            if (response.IsSuccessStatusCode)
            {
                result = await response.Content.ReadAsStringAsync();
            }
            return result;
        }

        public async Task<string> GetPrivateMethods()
        {
            // http://api.steampowered.com/ISteamWebAPIUtil/GetSupportedAPIList/v0001/?key=758AEEE709F200A44D5A076B68F7636F"
            string result = null;
            HttpResponseMessage response = await client.GetAsync("ISteamWebAPIUtil/GetSupportedAPIList/v0001/" + "?key=" + apiKey);
            Console.WriteLine("GetPrivateMethods " + apiKey);
            if (response.IsSuccessStatusCode)
            {
                result = await response.Content.ReadAsStringAsync();
            }
            return result;
        }

        public async Task<UserModel> GetUser(string id)
        {
            if (String.IsNullOrWhiteSpace(apiKey))
                throw new MissingApiKeyException();

            if (String.IsNullOrWhiteSpace(id))
                throw new MissingSteamIDException();

            if (!IsDigitsOnly(id) || id.Length != 17)
                throw new InvalidSteamIDException(id);

            UserModel user = null;
            HttpResponseMessage response = await client.GetAsync("ISteamUser/GetPlayerSummaries/v0002/?key=" + apiKey + "&steamids=" + id);
            if (response.IsSuccessStatusCode)
            {
                var result = await response.Content.ReadAsAsync<UserRequestResult>();
                user = result.response.players.First();
                // var jsonString = await response.Content.ReadAsStringAsync();
                // Json.Decode(jsonString);
                // var jj = JObject.Parse(jsonString);
                // var models = JsonConvert.DeserializeObject<List<SteamModel>>(jsonString);
                // user = models[0];
                // user = await response.Content.ReadAsAsync<SteamModel>();
            }

            return user;
        }

        public async Task<int> GetUserLevel(string id)
        {
            if (String.IsNullOrWhiteSpace(apiKey))
                throw new MissingApiKeyException();

            if (String.IsNullOrWhiteSpace(id))
                throw new MissingSteamIDException();

            if (!IsDigitsOnly(id) || id.Length != 17)
                throw new InvalidSteamIDException(id);

            HttpResponseMessage response = await client.GetAsync("IPlayerService/GetSteamLevel/v0001/?key=" + apiKey + "&steamid=" + id);
            if (response.IsSuccessStatusCode)
            {
                var result = await response.Content.ReadAsAsync<UserLevelResult>();
                return result.response.player_level;
            }
            else
            {
                return -1;  // TODO; throw exception
            }
        }

        public async Task<List<SimpleGameModel>> GetGamesForUser(string id, bool details = false)
        {
            if (String.IsNullOrWhiteSpace(apiKey))
                throw new MissingApiKeyException();

            if (String.IsNullOrWhiteSpace(id))
                throw new MissingSteamIDException();

            if (!IsDigitsOnly(id) || id.Length != 17)
                throw new InvalidSteamIDException(id);

            List<SimpleGameModel> games = null;
            HttpResponseMessage response = await client.GetAsync("IPlayerService/GetOwnedGames/v0001/?key=" + apiKey + "&steamid=" + id + (details ? "&include_appinfo=1" : ""));
            // http://api.steampowered.com/IPlayerService/GetOwnedGames/v0001/?key=758AEEE709F200A44D5A076B68F7636F&steamid=76561198020784166&include_appinfo=1

            if (response.IsSuccessStatusCode)
            {
                var result = await response.Content.ReadAsAsync<SimpleGameResult>();
                games = result.response.games;
                // var jsonString = await response.Content.ReadAsStringAsync();
                // Json.Decode(jsonString);
                // var jj = JObject.Parse(jsonString);
                // var models = JsonConvert.DeserializeObject<List<SteamModel>>(jsonString);
                // user = models[0];
                // user = await response.Content.ReadAsAsync<SteamModel>();
            }

            return games;
        }

        public async Task<List<DetailedGameData>> GetDetailedGameInfos(List<int> appIds)
        {
            List<DetailedGameData> data = new List<DetailedGameData>();

            foreach (int appid in appIds)
            {
                Console.WriteLine("GetDetailedGameInfos for appid: " + appid);

                // retrieve using API request 
                HttpResponseMessage response = await storeClient.GetAsync("appdetails?appids=" + appid);
                if (response.IsSuccessStatusCode)
                {
                    var result = await response.Content.ReadAsAsync<Dictionary<int, DetailedGameModelResponse>>();

                    if (result.Values.First().success)
                    {
                        data.Add(result.Values.First().data);
                    }
                    else
                    {
                        Console.WriteLine("GetDetailedGameInfo - pulling data for " + appid + " failed.");
                    }
                }
                else
                {
                    Console.WriteLine("GetDetailedGameInfos response unsuccessful for appid: " + appid);
                }
            }

            return data;
        }

        public async Task<List<AchievementUserStats>> GetSimpleAchievementsForUser(string id, List<int> appids)
        {
            if (String.IsNullOrWhiteSpace(apiKey))
                throw new MissingApiKeyException();

            if (String.IsNullOrWhiteSpace(id))
                throw new MissingSteamIDException();

            if (!IsDigitsOnly(id) || id.Length != 17)
                throw new InvalidSteamIDException(id);

            if (appids.Count == 0)
                throw new ArgumentException("appids is empty!");

            List<AchievementUserStats> achievements = new List<AchievementUserStats>();

            foreach (int appid in appids)
            {
                HttpResponseMessage response = await client.GetAsync("ISteamUserStats/GetPlayerAchievements/v0001/?key=" + apiKey + "&steamid=" + id + "&appid=" + appid);
                if (response.IsSuccessStatusCode)
                {
                    var result = await response.Content.ReadAsAsync<AchievementUserResult>();
                   
                    if (result.playerstats.success && result.playerstats.achievements != null)  // Some gaves have an achievement model - but no achievements. 
                    {
                        achievements.Add(result.playerstats);
                    }
                    else
                    {
                        Console.WriteLine("GetSimpleAchievementsForUser - pulling data for " + appid + " failed.");
                    }
                }
                else
                {
                    Console.WriteLine("GetSimpleAchievementsForUser response unsuccessful for appid: " + appid);
                    // Some games don't have achievements - the response will be unsuccessful.
                }
            }

            return achievements;
        }



        //static async Task RunAsync()
        //{
        //    // New code:
        //    client.BaseAddress = new Uri("http://localhost:55268/");
        //    client.DefaultRequestHeaders.Accept.Clear();
        //    client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

        //    Console.ReadLine();
        //}

        public void SetKey(string key)
        {
            apiKey = key;
        }

        bool IsDigitsOnly(string str)
        {
            foreach (char c in str)
                if (c < '0' || c > '9')
                    return false;
            return true;
        }
    }

    public class MissingApiKeyException : Exception
    {
        public MissingApiKeyException() : base("Missing API key - Did you forget to call SetKey(key)?") { }
    }

    public class MissingSteamIDException : Exception
    {
        public MissingSteamIDException() : base("Missing Steam ID") { }
    }

    public class InvalidSteamIDException : Exception
    {
        public InvalidSteamIDException(string id) : base("Invalid Steam ID - " + id + ". Must be 32 alphanumeric characters.") { }
    }
}

/* Notes: 
 * https://msdn.microsoft.com/en-us/magazine/jj991977.aspx
 *  return Type => return Task<Type>
 *  return void => return Task
 *  eventhandler => return void 
 *  
 * 
 */
