using P02_FootballBetting.Common;
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
            Players = new HashSet<Player>();
        }


        [Key]
        public int TeamId { get; set; }

        [Required]
        [MaxLength(ValidationConstants.TeamNameMaxLength)]
        public string Name { get; set; } = null!;

        [MaxLength(ValidationConstants.UrlMaxLength)]
        public string? LogoUrl { get; set; }

        [Required]
        [MaxLength(ValidationConstants.InitialsMaxLength)]
        public string Initials { get; set; } = null!;

        [Required]
        public decimal Budget { get; set; }


        public int PrimaryKitColorId { get; set; }
        [ForeignKey(nameof(PrimaryKitColorId))]
        public Color PrimaryColor { get; set; } = null!;


        public int SecondaryKitColorId { get; set; }
        [ForeignKey(nameof(SecondaryKitColorId))]
        public Color SecondaryColor { get; set; } = null!;


        public int TownId { get; set; }
        [ForeignKey(nameof(TownId))]
        public Town Town { get; set; } = null!;

        public ICollection<Player> Players { get; set; } = null!;
        public ICollection<Game> HomeGames { get; set; } = null!;
        public ICollection<Game> AwayGames { get; set; } = null!;
    }
}
