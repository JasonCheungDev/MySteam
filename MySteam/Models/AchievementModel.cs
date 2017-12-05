using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MySteam.Models
{
    #region Achievement Schema 

    public class AchievementSchema
    {
        public string name { get; set; }
        public int defaultvalue { get; set; }
        public string displayName { get; set; }
        public int hidden { get; set; }
        public string description { get; set; }
        public string icon { get; set; }
        public string icongray { get; set; }
    }

    public class AchievementSchemaList
    {
        public List<AchievementSchema> achievements { get; set; }
    }

    public class GameAchievement
    {
        public string gameName { get; set; }
        public string gameVersion { get; set; }
        public AchievementSchemaList availableGameStats { get; set; }
    }

    public class GameAchievementResult
    {
        public GameAchievement game { get; set; }
    }

    #endregion


    #region Achievement User 

    public class AchievementUser
    {
        public string apiname { get; set; }     // string not appid 
        public int achieved { get; set; }       // 1 for achieved, 0 for pending 
        public int unlocktime { get; set; }     // unknown format 
    }

    public class AchievementUserStats
    {
        public string steamID { get; set; }     // user id 
        public string gameName { get; set; }    // pretty game name 
        public List<AchievementUser> achievements { get; set; }
        public bool success { get; set; }       
    }

    public class AchievementUserResult
    {
        public AchievementUserStats playerstats { get; set; }
    }

    #endregion

}
