using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace CESAPSCOREWEBAPP.Models
{
    public class V_ReportJobCost
    {
        [Key]
        public Int64 ID { get; set; }
        public string MainJobID { get; set; }
        public string DescriptionMainJob { get; set; }
        public string SubJobID { get; set; }
        public string DescriptionSubJob { get; set; }
        public string JobTaskNo { get; set; }
        public string DescriptionFull { get; set; }
        public decimal ThisPeriodQTY { get; set; }
        public decimal ThisperiodAMT { get; set; }
        public decimal OpeningQTY { get; set; }
        public decimal OpeningAMT { get; set; }
        public decimal CumBalQTY { get; set; }
        public decimal CumBalAMT { get; set; }
        public decimal BudgetQTY { get; set; }
        public decimal BudgetAMT { get; set; }


    }
}
