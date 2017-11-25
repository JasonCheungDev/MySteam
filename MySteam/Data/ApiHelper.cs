using MySteam.Models;
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
        public static ApiHelper Instance
        {
            get
            {
                if (mInstance == null)
                {
                    mInstance = new ApiHelper();
                    return mInstance;
                }
                else
                {
                    return mInstance;
                }
            }
        }

        private static ApiHelper mInstance;

        private static HttpClient client = new HttpClient();

        private static string apiKey;


        public ApiHelper()
        {
            // 758AEEE709F200A44D5A076B68F7636F

            client.BaseAddress = new Uri("http://api.steampowered.com/");
            client.DefaultRequestHeaders.Accept.Clear();
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

            Console.ReadLine();
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

        public async Task<SteamModel> GetUser(string url)
        {
            if (String.IsNullOrWhiteSpace(apiKey))
                throw new MissingApiKeyException();

            SteamModel user = null;
            HttpResponseMessage response = await client.GetAsync(url);
            if (response.IsSuccessStatusCode)
            {
                user = await response.Content.ReadAsAsync<SteamModel>();
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
