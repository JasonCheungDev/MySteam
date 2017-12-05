using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using MySteam.Data;
using MySteam.Models;
using Newtonsoft.Json;

namespace MySteam.Models
{
    public class MySteamContext : DbContext
    {
        private const string API_KEY = "758AEEE709F200A44D5A076B68F7636F";

        public MySteamContext (DbContextOptions<MySteamContext> options)
            : base(options)
        {
        }

        public async Task<UserModel> GetPlayerAsync(string steamId)
        {
            /* check for cached version
            var model = PlayerModel.SingleOrDefault(m => m.steamid == steamId);

            if (model) {
                return model;
            }*/

            ApiHelper.Instance.SetKey(API_KEY);
            return await ApiHelper.Instance.GetUser(steamId);
        }

        public async Task<DetailedGameData> GetDetailedGameData(int appid)
        {
            // 1. Check DB first  
            var dbResult = DetailedGame.FirstOrDefault(dgd => dgd.appid == appid);
            if (dbResult != null)
            {
                Console.WriteLine("GetDetailedGameData - Found " + appid + " in DB.");
                return dbResult.data;
            }

            // 2. Check API for missing
            Console.WriteLine("GetDetailedGameData - Did not find " + appid + " in DB. Pulling from API.");
            var appids = new List<int>() { appid };
            var apiResult = await ApiHelper.Instance.GetDetailedGameInfos(appids);

            // 3. Add API result to database 
            var result = apiResult.FirstOrDefault();
            if (!DetailedGame.Any(dgd => dgd.appid == result.steam_appid))  // double check to see if it has been added (async code, could be)
            {
                var serialized = new DetailedGameModelDatabase()
                {
                    appid = result.steam_appid,
                    DetailedGameModelDataString = JsonConvert.SerializeObject(result)
                };
                DetailedGame.Add(serialized);
                SaveChanges();
            }

            return result;
        }

        public async Task<List<DetailedGameData>> GetDetailedGameDatas(List<int> appids)
        {
            List<DetailedGameData> games = new List<DetailedGameData>();    // return result 
            List<int> missingGames = new List<int>();                       // appids not in database yet 

            // 1. Check DB first
            foreach (int appid in appids)
            {
                var dbResult = DetailedGame.FirstOrDefault(dgd => dgd.appid == appid);
                if (dbResult != null)
                {
                    Console.WriteLine("GetDetailedGameDatas - Found " + appid + " in DB.");
                    games.Add(dbResult.data);   // found game data in DB
                }
                else
                {
                    Console.WriteLine("GetDetailedGameDatas - Did not find " + appid + " in DB. Will pull from API.");
                    missingGames.Add(appid);    // add appid to retrieve from API
                }
            }

            // 2. Check API for missing 
            if (missingGames.Count > 0)
            {
                Console.WriteLine("GetDetailedGameDatas - Pulling " + missingGames.Count + " games from API.");
                List<DetailedGameData> retrievedGames = await ApiHelper.Instance.GetDetailedGameInfos(missingGames);
                games.AddRange(retrievedGames);

                // 3. Add to DB
                Console.WriteLine("GetDetailedGameDatas - Saving " + games.Count + " games retrieved from API.");
                var serialized = retrievedGames.Select(dgd => new DetailedGameModelDatabase()
                {
                    appid = dgd.steam_appid,
                    DetailedGameModelDataString = JsonConvert.SerializeObject(dgd)
                }).ToList();
                DetailedGame.AddRange(serialized);  // TODO: good chance this will crash  - no existing checking
                SaveChanges();
            }


            // 3. Return results 
            return games;
        }


        public DbSet<MySteam.Models.UserModel> PlayerModel { get; set; }

        public DbSet<MySteam.Models.SimpleGameModel> SimpleGameModel { get; set; }

        public DbSet<MySteam.Models.DetailedGameModelDatabase> DetailedGame { get; set; }
    }
}
