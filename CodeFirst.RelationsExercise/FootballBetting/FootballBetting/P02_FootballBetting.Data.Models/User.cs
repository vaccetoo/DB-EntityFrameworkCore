using P02_FootballBetting.Common;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace P02_FootballBetting.Data.Models
{
    public class User
    {
        [Key]
        public int UserId { get; set; }

        [Required]
        [MaxLength(GlobalConstants.UserNameMaxLength)]
        public string Username { get; set; } = null!;

        [Required]
        [MaxLength(GlobalConstants.UsersNamesMaxLength)]
        public string Name { get; set; } = null!;

        [Required]
        [MaxLength(GlobalConstants.PassworMaxLength)]
        public string Password { get; set; } = null!;

        [Required]
        [MaxLength(GlobalConstants.EmailMaxLength)]
        public string Email { get; set; } = null!;

        public decimal Balance { get; set; }
    }
}
