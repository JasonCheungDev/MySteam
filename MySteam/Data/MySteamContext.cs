using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using MySteam.Data;
using MySteam.Models;

namespace MySteam.Models
{
    public class MySteamContext : DbContext
    {
        private const string API_KEY = "758AEEE709F200A44D5A076B68F7636F";

        public MySteamContext (DbContextOptions<MySteamContext> options)
            : base(options)
        {
        }

        public async Task<PlayerModel> GetPlayerAsync(string steamId)
        {
            /* check for cached version
            var model = PlayerModel.SingleOrDefault(m => m.steamid == steamId);

            if (model) {
                return model;
            }*/

            ApiHelper.Instance.SetKey(API_KEY);
            return await ApiHelper.Instance.GetUser(steamId);
        }

        public DbSet<MySteam.Models.PlayerModel> PlayerModel { get; set; }

        public DbSet<MySteam.Models.SimpleGameModel> SimpleGameModel { get; set; }

        public DbSet<MySteam.Models.DetailedGameModelDatabase> DetailedGame { get; set; }
    }
}
