using P02_FootballBetting.Common;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace P02_FootballBetting.Data.Models
{
    public class Player
    {
        [Key]
        public int PLayerId { get; set; }

        [Required]
        [MaxLength(ValidationConstants.NameMaxLength)]
        public string Name { get; set; } = null!;

        [Required]
        [MaxLength(ValidationConstants.PlayerNumberMaxLength)]
        public string SquadNumber { get; set; } = null!;

        public int TownId { get; set; }
        [ForeignKey(nameof(TownId))]
        public Town Town { get; set; } = null!;

        public int PositionId { get; set; }
        [ForeignKey(nameof(PositionId))]
        public Position Position { get; set; } = null!;

        [DefaultValue(false)]
        public bool IsInjured { get; set; }

        public int TeamId { get; set; }
        [ForeignKey(nameof(TeamId))]
        public Team Team { get; set; } = null!;
    }
}
