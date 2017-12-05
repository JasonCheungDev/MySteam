using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace MySteam.Models
{
    public class SimpleGameModel
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]       // required to generate view
        public int appid { get; set; }
        public string name { get; set; }                        // null if include_appinfo=0 (false)
        public int playtime_forever { get; set; }   
        public string img_icon_url { get; set; }                // null if include_appinfo=0 (false). Not a URL but a hash value.
        public string img_logo_url { get; set; }                // null if include_appinfo=0 (false). Not a URL but a hash value.
        public bool has_community_visible_stats { get; set; }   // null if include_appinfo=0 (false)
        public int? playtime_2weeks { get; set; }               // null if not played within 2 weeks 

        // returns a usuable URL 
        public string UsableImgIconUrl
        {
            get { return "http://media.steampowered.com/steamcommunity/public/images/apps/" + appid + "/" + img_icon_url + ".jpg"; }
        }

        // returns a usuable URL
        public string UsableImgLogoUrl
        {
            get { return "http://media.steampowered.com/steamcommunity/public/images/apps/" + appid + "/" + img_logo_url + ".jpg"; }
        }
    }

    public class SimpleGameResponse
    {
        public int game_count { get; set; }
        public List<SimpleGameModel> games { get; set; }
    }

    public class SimpleGameResult
    {
        public SimpleGameResponse response { get; set; }
    }
}

/*
public class Game
{
    public int appid { get; set; }
    public int playtime_forever { get; set; }
    public int? playtime_2weeks { get; set; }
}

public class Response
{
    public int game_count { get; set; }
    public List<Game> games { get; set; }
}

public class RootObject
{
    public Response response { get; set; }
}
*/
