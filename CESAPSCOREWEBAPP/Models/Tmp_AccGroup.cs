using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CESAPSCOREWEBAPP.Models
{
    public class Tmp_AccGroup
    {
        public string PostingDate { get; set; }
        public string Doc { get; set; }
        public string JobNo { get; set; }
        public string Description { get; set; }
        public string CustomerGroup { get; set; }
        public decimal SumTotal { get; set; }
        public decimal SumTotalVat { get; set; }
        public decimal TotalNAV { get; set; }
        public decimal Retention { get; set; }
        public string GroupName { get; set; }
    }
}
