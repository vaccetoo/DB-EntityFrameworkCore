using System.Xml.Serialization;

namespace Medicines.DataProcessor.ExportDtos
{
    [XmlType("Medicine")]
    public class MedicineExportDto
    {
        [XmlAttribute("Category")]
        public string Category { get; set; }

        [XmlElement("Name")]
        public string Name { get; set; }

        [XmlElement("Price")]
        public string Price { get; set; }

        [XmlElement("Producer")]
        public string Producer { get; set; }

        [XmlElement("BestBefore")]
        public string BestBefore { get; set; }
    }
}