﻿using P02_FootballBetting.Common;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace P02_FootballBetting.Data.Models
{
    public class Team
    {
        public Team()
        {
            HomeGames = new HashSet<Game>();
            AwayGames = new HashSet<Game>();
            Players = new HashSet<Player>();
        }


        [Key]
        public int TeamId { get; set; }

        [MaxLength(GlobalConstants.TeamNameMaxLength)]
        [Required]
        public string Name { get; set; } = null!;

        [MaxLength(GlobalConstants.UrlMaxLength)]
        public string? LogoUrl { get; set; }

        [Required]
        [MaxLength(GlobalConstants.InitialsMaxLength)]
        public string Initials { get; set; } = null!;

        public decimal Budget { get; set; }

        public int PrimaryKitColorId { get; set; }
        [ForeignKey(nameof(PrimaryKitColorId))]
        [InverseProperty("PrimaryKitTeams")]
        public Color PrimaryKitColor { get; set; } = null!;

        public int SecondaryKitColorId { get; set; }
        [ForeignKey(nameof(SecondaryKitColorId))]
        [InverseProperty("SecondaryKitTeams")]
        public Color SecondaryKitColor { get; set; }

        public int TownId { get; set; }
        [ForeignKey(nameof(TownId))]
        public Town Town { get; set; } = null!;

        public ICollection<Game> HomeGames { get; set; }

        public ICollection<Game> AwayGames { get; set; }

        public ICollection<Player> Players { get; set; }
    }
}
