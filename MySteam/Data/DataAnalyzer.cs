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
            foreach (SimpleGameModel sgm in games)
            {
                if (sgm.playtime_forever == 0)
                    continue;
                if (mostPlayed == null)
                    mostPlayed = sgm;
                else if (sgm.playtime_forever > mostPlayed.playtime_forever)
                    mostPlayed = sgm;
            }
            return mostPlayed;
        }

        public static SimpleGameModel FindLeastPlayedGame(List<SimpleGameModel> games)
        {
            SimpleGameModel leastPlayed = null;
            foreach (SimpleGameModel sgm in games)
            {
                if (sgm.playtime_forever == 0)
                    continue;
                if (leastPlayed == null)
                    leastPlayed = sgm;
                else if (sgm.playtime_forever < leastPlayed.playtime_forever)
                    leastPlayed = sgm;
            }
            return leastPlayed;
        }

        public static DetailedGameData FindMostExpensiveGame(List<DetailedGameData> games)
        {
            DetailedGameData mostExpensive = null;
            int highestCost = 0;
            foreach (DetailedGameData dgd in games)
            {
                if (dgd.price_overview != null && dgd.price_overview.initial > highestCost)
                {
                    mostExpensive = dgd;
                }
            }
            return mostExpensive;
        }

        public static DetailedGameData FindLeastExpensiveGame(List<DetailedGameData> games)
        {
            DetailedGameData leastExpensive = null;
            int lowestCost = int.MaxValue;
            foreach (DetailedGameData dgd in games)
            {
                if (dgd.price_overview != null && dgd.price_overview.initial < lowestCost)
                {
                    leastExpensive = dgd;
                }
            }
            return leastExpensive;
        }

        public static GameValue FindMostWorthGame(List<SimpleGameModel> data1, List<DetailedGameData> data2)
        {
            // validation 
            if (data1.Count != data2.Count)
            {
                throw new ArgumentException("List of game data must equal each other");
            }

            GameValue bestValue = new GameValue() { value = -1 };    // create temp 0 value that will be overridden

            // find best value 
            for (int i = 0; i < data1.Count; i++)
            {
                if (data2[i].price_overview == null)
                {
                    continue;   // skip free games 
                }

                float currentValue = data2[i].price_overview.initial / data1[i].playtime_forever;

                if (currentValue > bestValue.value)
                {
                    bestValue = new GameValue()
                    {
                        simpleGame = data1[i],
                        detailedGame = data2[i],
                        value = currentValue
                    };
                }
            }
            
            // TODO: maybe throw exception if nothing is set 

            return bestValue;
        }

        public static GameValue FindLeastWorthGame(List<SimpleGameModel> data1, List<DetailedGameData> data2)
        {
            // validation 
            if (data1.Count != data2.Count)
            {
                throw new ArgumentException("List of game data must equal each other");
            }

            GameValue bestValue = new GameValue() { value = float.MaxValue };    // create temp value that will be overridden

            // find best value 
            for (int i = 0; i < data1.Count; i++)
            {
                if (data2[i].price_overview == null)
                {
                    continue;   // skip free games 
                }

                float currentValue = data2[i].price_overview.initial / data1[i].playtime_forever;

                if (currentValue < bestValue.value)
                {
                    bestValue = new GameValue()
                    {
                        simpleGame = data1[i],
                        detailedGame = data2[i],
                        value = currentValue
                    };
                }
            }

            // TODO: maybe throw exception if nothing is set 

            return bestValue;
        }

        public static double CalculateTotalGameWorth(List<DetailedGameData> data)
        {
            int totalWorth = 0; 
            foreach (DetailedGameData dgd in data)
            {
                if (dgd.price_overview != null) // non-free game 
                {
                    totalWorth += dgd.price_overview.initial;
                }
            }
            return Math.Round(totalWorth * 0.01, 2);
        }

        public static double CalculateAchievementPercentage(List<AchievementUserStats> achievementList)
        {
            int completedCount = 0;
            int incompletedCount = 0;

            foreach (AchievementUserStats aus in achievementList)
            {
                foreach (AchievementUser au in aus.achievements)
                {
                    if (au.achieved == 1)
                    {
                        completedCount++;
                    }
                    else
                    {
                        incompletedCount++;
                    }
                }
            }

            double percentage = (double)completedCount / (double)(completedCount + incompletedCount);
            return Math.Round(percentage, 4);
        }
    }
}

/// <summary>
/// Simple struct that holds a SimpleGame object, DetailedGame object, and value (cost / time). 
/// </summary>
public class GameValue
{
    public SimpleGameModel simpleGame;
    public DetailedGameData detailedGame;
    public float value;
}