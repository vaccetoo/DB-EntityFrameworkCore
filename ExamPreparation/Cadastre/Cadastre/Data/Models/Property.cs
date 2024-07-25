﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cadastre.Data.Models
{
    public class Property
    {
        public Property()
        {
            PropertiesCitizens = new List<PropertyCitizen>();
        }

        [Key]
        public int Id { get; set; }

        [Required]
        [MinLength(16)]
        [MaxLength(20)]
        public string PropertyIdentifier { get; set; } = null!;

        [Required]
        public int Area { get; set; }

        [MinLength(5)]
        [MaxLength(500)]
        public string? Details { get; set; }

        [Required]
        [MinLength(5)]
        [MaxLength(200)]
        public string Address { get; set; } = null!;

        [Required]
        public DateTime DateOfAcquisition { get; set; }

        [Required]
        public int DistrictId { get; set; }

        [ForeignKey(nameof(DistrictId))]
        public District District { get; set; } = null!;

        public ICollection<PropertyCitizen> PropertiesCitizens { get; set; }
    }
}
