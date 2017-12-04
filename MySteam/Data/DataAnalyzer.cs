using MySteam.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MySteam.Data
{
    public class DataAnalyzer
    {
        public static int CalculateTotalTimePlayed(List<SimpleGameModel> games)
        {
            int timePlayed = 0;
            foreach (SimpleGameModel sgm in games)
            {
                timePlayed += sgm.playtime_forever;
                // timeRecentlyPlayed += sgm.playtime_2weeks;
            }
            return timePlayed;
        }

        public static SimpleGameModel FindMostPlayedGame(List<SimpleGameModel> games)
        {
            SimpleGameModel mostPlayed = null;
            int currentMostPlayedTime = 0;
            foreach (SimpleGameModel sgm in games)
            {
                if (sgm.playtime_forever > currentMostPlayedTime)
                {
                    mostPlayed = sgm;
                }
            }
            return mostPlayed;
        }

        public static float CalculateTotalGameWorth(List<DetailedGameData> data)
        {
            int totalWorth = 0;
            foreach (DetailedGameData dgd in data)
            {
                if (dgd.price_overview != null) // non-free game 
                {
                    totalWorth += dgd.price_overview.final;
                }
            }
            return totalWorth * 0.01f;
        }
    }
}
