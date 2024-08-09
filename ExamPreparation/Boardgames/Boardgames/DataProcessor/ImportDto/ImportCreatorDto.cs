﻿using Boardgames.Data.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace Boardgames.DataProcessor.ImportDto
{
    [XmlType("Creator")]
    public class ImportCreatorDto
    {
        public ImportCreatorDto()
        {
            BoardgamesDto = new List<ImportBoardgameDto>();
        }

        [XmlElement("FirstName")]
        [Required]
        [MinLength(2), MaxLength(7)]
        public string FirstName { get; set; } = null!;

        [XmlElement("LastName")]
        [Required]
        [MinLength(2), MaxLength(7)]
        public string LastName { get; set; } = null!;

        [XmlArray("Boardgames")]
        public List<ImportBoardgameDto> BoardgamesDto { get; set; }
    }
}
