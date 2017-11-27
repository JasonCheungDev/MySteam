using MySteam.Models;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
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


        private static HttpClient client = new HttpClient();

        private static string apiKey;


        private ApiHelper()
        {
            // 758AEEE709F200A44D5A076B68F7636F
            Console.WriteLine("ApiHelper start");

            client.BaseAddress = new Uri("http://api.steampowered.com/");
            client.DefaultRequestHeaders.Accept.Clear();
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

            Console.WriteLine("ApiHelper done");
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
            string result = null;
            HttpResponseMessage response = await client.GetAsync("ISteamWebAPIUtil/GetSupportedAPIList/v0001/" + "?key=" + apiKey);
            if (response.IsSuccessStatusCode)
            {
                result = await response.Content.ReadAsStringAsync();
            }
            return result;
        }

        public async Task<PlayerModel> GetUser(string id)
        {
            if (String.IsNullOrWhiteSpace(apiKey))
                throw new MissingApiKeyException();

            PlayerModel user = null;
            HttpResponseMessage response = await client.GetAsync("ISteamUser/GetPlayerSummaries/v0002/?key=" + apiKey + "&steamids=" + id);
            if (response.IsSuccessStatusCode)
            {
                var result = await response.Content.ReadAsAsync<PlayerModelResponse>();
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


    }

    public class MissingApiKeyException : Exception
    {
        public MissingApiKeyException() : base("Missing API key - Did you forget to call SetKey(key)?") { }
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
