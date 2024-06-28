using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace P02_FootballBetting.Data.Models
{
    public class Game
    {
        [Key]
        public int GameId { get; set; }

        public int HomeTeamId { get; set; }
        [ForeignKey(nameof(HomeTeamId))]
        public Team HomeTeam { get; set; } = null!;

        public int AwayTeamId { get; set; }
        [ForeignKey(nameof(AwayTeamId))]
        public Team AwayTeam { get; set; } = null!;

        public DateTime DateTime { get; set; }

        public int HomeTeamBetRate { get; set; }

        public int AwayTeamBetRate { get; set; }

        public int DrawBetRate { get; set; }

        public string Result { get; set; } = null!;
    }
}
