using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace CESAPSCOREWEBAPP.Models
{
    public class CHQDirect
    {
        [Key]
        public Int64 ID { get; set; }
        public string YearDate { get; set; }
        public string MonthDate { get; set; }
        public DateTime PostingDate { get; set; }
        public int CountData { get; set; }
        public Decimal SumTotal { get; set; }
        public string MonthTH { get; set; }
    }
}
