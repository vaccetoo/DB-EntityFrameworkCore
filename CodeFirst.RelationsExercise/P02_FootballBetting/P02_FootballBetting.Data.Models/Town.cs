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
    public class Town
    {
        [Key]
        public int TownId { get; set; }

        [Required]
        [MaxLength(ValidationConstants.TownNameMaxLength)]
        public string Name { get; set; } = null!;

        public int CountryId { get; set; }
        [ForeignKey(nameof(CountryId))]
        public Country Country { get; set; } = null!;
    }
}
