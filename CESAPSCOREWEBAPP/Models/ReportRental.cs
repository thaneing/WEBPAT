using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace CESAPSCOREWEBAPP.Models
{
    public class ReportRental
    {
        [Key]
        public int ID { get; set; }
        public string Des { get; set; }
        public decimal NowTotal { get; set; }
        public decimal OldTotal { get; set; }
        public decimal SumTotal { get; set; }

     }
}
