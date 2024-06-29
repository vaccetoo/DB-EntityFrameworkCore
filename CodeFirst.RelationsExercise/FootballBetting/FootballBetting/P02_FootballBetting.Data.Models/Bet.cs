using P02_FootballBetting.Common;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace P02_FootballBetting.Data.Models
{
    public class Bet
    {
        [Key]
        public int BetId { get; set; }

        public decimal Amount { get; set; }

        public Result Prediction { get; set; }

        public DateTime DateTime { get; set; }

        //TODO: RELATIONS!!!
        public int UserId { get; set; }

        public int GameId { get; set; } 
    }
}
