﻿using Cadastre.Data.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace Cadastre.DataProcessor.ImportDtos
{
    [XmlType("Property")]
    public class PropertyImportDto
    {
        [Required]
        [MinLength(16)]
        [MaxLength(20)]
        [XmlElement("PropertyIdentifier")]
        public string PropertyIdentifier { get; set; } = null!;

        [Required]
        [XmlElement("Area")]
        public int Area { get; set; }

        [MinLength(5)]
        [MaxLength(500)]
        [XmlElement("Details")]
        public string? Details { get; set; }

        [Required]
        [MinLength(5)]
        [MaxLength(200)]
        [XmlElement("Address")]
        public string Address { get; set; } = null!;

        [Required]
        [XmlElement("DateOfAcquisition")]
        public string DateOfAcquisition { get; set; } = null!;
    }
}
