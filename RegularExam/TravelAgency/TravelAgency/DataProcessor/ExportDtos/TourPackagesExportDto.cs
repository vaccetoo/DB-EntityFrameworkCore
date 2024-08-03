using System.ComponentModel.DataAnnotations;
using System.Xml.Serialization;

namespace TravelAgency.DataProcessor.ExportDtos
{
    [XmlType("TourPackage")]
    public class TourPackagesExportDto
    {
        [XmlElement("Name")]
        public string PackageName { get; set; } = null!;

        [XmlElement("Description")]
        public string? Description { get; set; }

        [XmlElement("Price")]
        public string Price { get; set; } = null!;
    }
}