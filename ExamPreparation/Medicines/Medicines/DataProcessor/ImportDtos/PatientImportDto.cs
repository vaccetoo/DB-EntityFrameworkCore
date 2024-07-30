using Medicines.Data.Models.Enums;
using Medicines.Data.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Medicines.DataProcessor.ImportDtos
{
    public class PatientImportDto
    {
        [Required]
        [MinLength(5), MaxLength(100)]
        [JsonProperty("FullName")]
        public string FullName { get; set; } = null!;

        [Required]
        [Range(0, 2)]
        [JsonProperty("AgeGroup")]
        public int AgeGroup { get; set; }

        [Required]
        [Range(0, 1)]
        public int Gender { get; set; }

        [JsonProperty("Medicines")]
        public List<int> Medicines { get; set; }
    }
}
