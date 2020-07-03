using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CESAPSCOREWEBAPP.Models
{
    public class CHQDirectReportGen
    {
        public string Month { get; set; }
        public int Q1 { get; set; }
        public decimal TotalQ1 { get; set; }
        public int Q2 { get; set; }
        public decimal TotalQ2 { get; set; }
        public int QTotal { get; set; }
        public decimal QAmount { get; set; }
    }
}
