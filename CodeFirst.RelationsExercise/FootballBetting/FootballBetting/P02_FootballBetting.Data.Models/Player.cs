using P02_FootballBetting.Common;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace P02_FootballBetting.Data.Models
{
    public class Player
    {
        [Key]
        public int PlayerId { get; set; }

        [Required]
        [MaxLength(GlobalConstants.PlayerNameMaxLength)]
        public string Name { get; set; } = null!;

        public string? SquadNumber { get; set; }

        [DefaultValue(false)]
        public bool IsInjured { get; set; }

        // TODO: RELATIONS !!!
        public int PositionId { get; set; }

        public int TeamId { get; set; }

        public int TownId { get; set; }
    }
}
