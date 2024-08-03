﻿using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TravelAgency.Data.Models;

namespace TravelAgency.DataProcessor.ImportDtos
{
    public class ImportBookingsDto
    {
        [JsonProperty("BookingDate")]
        public string BookingDate { get; set; } = null!;

        [JsonProperty("CustomerName")]
        public string CustomerName { get; set; } = null!;

        [JsonProperty("TourPackageName")]
        public string TourPackageName { get; set; } = null!;
    }
}
