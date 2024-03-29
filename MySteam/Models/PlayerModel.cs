﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace MySteam.Models
{
    #region User Summary 

    public class UserModel
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public string steamid { get; set; }
        [Display(Name = "Visibility")]
        public UserVisibility communityvisibilitystate { get; set; }
        public int profilestate { get; set; }
        [Display(Name = "Username")]
        public string personaname { get; set; }
        public int lastlogoff { get; set; }
        public string profileurl { get; set; }
        public string avatar { get; set; }
        public string avatarmedium { get; set; }
        public string avatarfull { get; set; }
        [Display(Name = "State")]
        public UserState personastate { get; set; }
        [Display(Name = "Real Name")]
        public string realname { get; set; }
        public string primaryclanid { get; set; }
        public int timecreated { get; set; }
        public int personastateflags { get; set; }
        public string loccountrycode { get; set; }
        public string locstatecode { get; set; }
        public int loccityid { get; set; }

        [NotMapped]
        public string TimeSinceLastLogoff {
            get {
                var timespan = TimeSpan.FromSeconds(DateTimeOffset.Now.ToUnixTimeSeconds() - lastlogoff);
                return timespan.ToString(@"h\:mm\:ss");
            }
        }
    }

    public class UserResponse
    {
        public List<UserModel> players { get; set; }
    }

    public class UserRequestResult
    {
        public UserResponse response { get; set; }
    }

    public enum UserVisibility
    {
        Invisible = 1,  // private, friends, only, etc. 
        Visible = 3     // public
    }

    public enum UserState
    {
        Offline = 0,
        Online = 1,
        Busy = 2,
        Away = 3,
        Snooze = 4,
        LookingToTrade = 5,
        LookingToPlay = 6
    }

    #endregion

    #region User Level

    public class UserLevelResponse
    {
        public int player_level { get; set; }
    }

    public class UserLevelResult
    {
        public UserLevelResponse response { get; set; }
    }

    #endregion

}
