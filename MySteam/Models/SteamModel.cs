using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MySteam.Models
{
    // API KEY: 758AEEE709F200A44D5A076B68F7636F
    // GET ALL AVAILABLE API CALLS: http://api.steampowered.com/ISteamWebAPIUtil/GetSupportedAPIList/v0001/
    // MAKE SAME REQUEST WITH KEY FOR MORE: http://api.steampowered.com/ISteamWebAPIUtil/GetSupportedAPIList/v0001/?key=758AEEE709F200A44D5A076B68F7636F
    /* STEAM INFO : http://api.steampowered.com/ISteamUser/method/v0001/?key=758AEEE709F200A44D5A076B68F7636F
        - GetFriendList
        - GetPlayerBans
        - GetPlayerSummaries
        http://api.steampowered.com/ISteamUser/GetPlayerSummaries/v0002/?key=758AEEE709F200A44D5A076B68F7636F&steamids=76561197960435530
        http://api.steampowered.com/IPlayerService/GetOwnedGames/v0001/?key=758AEEE709F200A44D5A076B68F7636F&steamid=76561197960434622&format=json

    */
    public class SteamModel
    {
        public int steamid { get; set; }
        public CommunityVisibilityState visibility { get; set; }
        public string displayname { get; set; }
        public string avatarurl { get; set; }
        // string personastate (offline, online, busy, etc.)
        // unixtime lastlogoff
    }

    public enum CommunityVisibilityState
    {
        NotVisible = 1, // (private, friends only, etc.)
        Visible = 3,    // (public)
    }
}
