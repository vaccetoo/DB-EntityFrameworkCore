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
    public class Position
    {
        public Position()
        {
            Players = new HashSet<Player>();
        }

        [Key]
        public int PositionId { get; set; }

        [Required]
        [MaxLength(ValidationConstants.PositionNameMaxLength)]
        public string Name { get; set; } = null!;

        public int PlayerId { get; set; }
        [ForeignKey(nameof(PlayerId))]
        public ICollection<Player> Players { get; set; } = null!;
    }
}
