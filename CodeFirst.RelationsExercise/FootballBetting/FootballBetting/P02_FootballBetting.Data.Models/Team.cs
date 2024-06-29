using P02_FootballBetting.Common;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace P02_FootballBetting.Data.Models
{
    public class Team
    {
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

        //TODO: RELATIONS !!!
        public int PrimaryKitColorId { get; set; }

        public int SecondaryKitColorId { get; set; }

        public int TownId { get; set; } 
    }
}
