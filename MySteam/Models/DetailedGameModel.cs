using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace MySteam.Models
{
    public class PcRequirements
    {
        public string minimum { get; set; }             // if PC supported return string
        // public List<string> minimum { get; set; }    // if PC not supported returns empty array
    }

    public class MacRequirements
    {
        public string minimum { get; set; }             // warning: see PcRequirements
    }

    public class Demo
    {
        public int appid { get; set; }
        public string description { get; set; }
    }

    public class PriceOverview
    {
        public string currency { get; set; }
        public int initial { get; set; }
        public int final { get; set; }
        public int discount_percent { get; set; }
    }

    public class Sub
    {
        public int packageid { get; set; }
        public string percent_savings_text { get; set; }
        public int percent_savings { get; set; }
        public string option_text { get; set; }
        public string option_description { get; set; }
        public string can_get_free_license { get; set; }
        public bool is_free_license { get; set; }
        public int price_in_cents_with_discount { get; set; }
    }

    public class PackageGroup
    {
        public string name { get; set; }
        public string title { get; set; }
        public string description { get; set; }
        public string selection_text { get; set; }
        public string save_text { get; set; }
        public int display_type { get; set; }
        public string is_recurring_subscription { get; set; }
        public List<Sub> subs { get; set; }
    }

    public class Platforms
    {
        public bool windows { get; set; }
        public bool mac { get; set; }
        public bool linux { get; set; }
    }

    public class Metacritic
    {
        public int score { get; set; }
        public string url { get; set; }
    }

    public class Category
    {
        public int id { get; set; }
        public string description { get; set; }
    }

    public class Genre
    {
        public string id { get; set; }
        public string description { get; set; }
    }

    public class Screenshot
    {
        public int id { get; set; }
        public string path_thumbnail { get; set; }
        public string path_full { get; set; }
    }

    public class Recommendations
    {
        public int total { get; set; }
    }

    public class Highlighted
    {
        public string name { get; set; }
        public string path { get; set; }
    }

    public class Achievements
    {
        public int total { get; set; }
        public List<Highlighted> highlighted { get; set; }
    }

    public class ReleaseDate
    {
        public bool coming_soon { get; set; }
        public string date { get; set; }
    }

    public class SupportInfo
    {
        public string url { get; set; }
        public string email { get; set; }
    }

    public class DetailedGameData
    {
        public string type { get; set; }
        public string name { get; set; }
        public int steam_appid { get; set; }
        public int required_age { get; set; }
        public bool is_free { get; set; }
        public List<int> dlc { get; set; }
        public string detailed_description { get; set; }
        public string about_the_game { get; set; }
        public string short_description { get; set; }
        public string supported_languages { get; set; }
        public string header_image { get; set; }
        public string website { get; set; }
        //[JsonProperty(Required = Required.AllowNull, NullValueHandling =NullValueHandling.Ignore)]
        //public PcRequirements pc_requirements { get; set; }       // either returns a string or an empty array (Object can't be of type string or List<string> at the same time
        //[JsonProperty(Required = Required.AllowNull, NullValueHandling = NullValueHandling.Ignore)]
        //public MacRequirements mac_requirements { get; set; }
        //public List<object> linux_requirements { get; set; }
        public List<string> developers { get; set; }
        public List<string> publishers { get; set; }
        public List<Demo> demos { get; set; }
        public PriceOverview price_overview { get; set; }
        public List<int> packages { get; set; }
        public List<PackageGroup> package_groups { get; set; }
        public Platforms platforms { get; set; }
        public Metacritic metacritic { get; set; }
        public List<Category> categories { get; set; }
        public List<Genre> genres { get; set; }
        public List<Screenshot> screenshots { get; set; }
        public Recommendations recommendations { get; set; }
        public Achievements achievements { get; set; }
        public ReleaseDate release_date { get; set; }
        public SupportInfo support_info { get; set; }
        public string background { get; set; }
    }

    public class DetailedGameModelResponse
    {
        public bool success { get; set; }
        public DetailedGameData data { get; set; }
    }

    // THIS MODEL DOESN'T WORK
    [System.Obsolete("This model does not work, please use Dictionary<int, DetailedGameModelResponse> instead.")]
    public class DetailedGameModelResult
    {
        public Dictionary<int, DetailedGameModelResponse> response { get; set; }
        // if the root object contained an array with key "response" and value "unknown key":"DGMR then this would work.
        // but the root object contains the unknown key right away. 
    }

    // EF

    public class DetailedGameModelDatabase
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int appid { get; set; }                        // appid of the game (ie. 578080 for PUBG)
        public string DetailedGameModelDataString { get; set; }  // json string of the DetailedGameModelData. Actual object not stored due to many different objects.

        [NotMapped]
        private DetailedGameData mData;
        [NotMapped]
        public DetailedGameData data
        {
            get
            {
                if (mData == null)
                {
                    mData = JsonConvert.DeserializeObject<DetailedGameData>(DetailedGameModelDataString);
                }
                return mData;
            }
        }
    }

}

/* JSON Format
{
    578080 (appid): {
        success: true (or false if appid does not exist),
        data: {...}
    }
}
*/
