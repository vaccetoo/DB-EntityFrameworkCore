﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace P02_FootballBetting.Data.Models
{
    public class PlayerStatistic
    {
        public int GameId { get; set; }
        [ForeignKey(nameof(GameId))]
        [InverseProperty("PlayersStatistics")]
        public Game Game { get; set; } = null!;

        public int PlayerId { get; set; }
        [ForeignKey(nameof(PlayerId))]
        [InverseProperty("PlayersStatistics")]
        public Player Player { get; set; } = null!;

        public int ScoredGoals { get; set; }

        public int Assists { get; set; }

        public int MinutesPlayed { get; set; }
    }
}
