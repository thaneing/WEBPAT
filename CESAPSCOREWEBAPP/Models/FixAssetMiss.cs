﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace CESAPSCOREWEBAPP.Models
{
    public class FixAssetMiss
    {
        [Key]
        public Int64 ID { get; set; }
        public string FANO { get; set; }
        public string Description { get; set; }
        public DateTime StartDate { get; set; }
        [RegularExpression(@"^\d+\.\d{0,2}$")]
        public decimal Amount { get; set; }
        [RegularExpression(@"^\d+\.\d{0,2}$")]
        public decimal Quantity { get; set; }

    }
}
