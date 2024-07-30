using Medicines.Data.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace Medicines.DataProcessor.ImportDtos
{
    [XmlType("Pharmacy")]
    public class PharmacyImportDto
    {
        [Required]
        [MinLength(2), MaxLength(50)]
        [XmlElement("Name")]
        public string Name { get; set; } = null!;

        [Required]
        [StringLength(14)]
        [RegularExpression(@"^\(\d{3}\) \d{3}-\d{4}$")]
        [XmlElement("PhoneNumber")]
        public string PhoneNumber { get; set; } = null!;

        [Required]
        [XmlAttribute("non-stop")]
        [RegularExpression(@"^(true|false)$")]
        public string IsNonStop { get; set; } = null!;

        [XmlArray("Medicines")]
        public List<MedicineImportDto> Medicines { get; set; } = null!;
    }
}
