using Cadastre.Data.Enumerations;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace Cadastre.DataProcessor.ImportDtos
{
    [XmlType("District")]
    public class ImportDistrictDto
    {
        [Required]
        [MinLength(2)]
        [MaxLength(80)]
        [XmlElement("Name")]
        public string Name { get; set; } = null!;

        [Required]
        [StringLength(8)]
        [RegularExpression("^[A-Z]{2}-\\d{5}$")]
        [XmlElement("PostalCode")]
        public string PostalCode { get; set; } = null!;

        [Required]
        [XmlAttribute("Region")]
        public string Region { get; set; } = null!;

        [XmlArray("Properties")]
        public List<PropertyImportDto> Properties { get; set; } = null!;
    }
}
