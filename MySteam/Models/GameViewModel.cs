using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace MySteam.Models
{
    public class GameViewModel
    {
        public string RequestId { get; set; }

        public int GameCount { get; set; }
        public string TotalTimePlayed { get; set; } // minutes
        public string TotalWorth { get; set; }   // $$.cc

        public GameValue MostExpensiveGame { get; set; }
        public GameValue LeastExpensiveGame { get; set; }

        public GameValue MostWorthGame { get; set; }
        public GameValue LeastWorthGame { get; set; }

        public List<GameValue> Games { get; set; }
    }
}
