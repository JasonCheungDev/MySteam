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

        public static float CalculateTotalGameWorth(List<SimpleGameModel> data)
        {
            throw new NotImplementedException();
        }
    }
}
