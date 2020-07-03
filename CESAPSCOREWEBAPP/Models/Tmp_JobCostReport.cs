using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CESAPSCOREWEBAPP.Models
{
    public class Tmp_JobCostReport
    {
        public string JobMain { get; set; }
        public string JobSub { get; set; }
        public string JobTaskCut { get; set; }
        public string JobTaskFull { get; set; }
        public decimal ThisPeriodQty { get; set; }
        public decimal ThisPeriodAMT { get; set; }
        public decimal OpenningQTY { get; set; }
        public decimal OpenningAMT { get; set; }
        public decimal CumBalQTY { get; set; }
        public decimal CumBalAMT { get; set; }
        public decimal BudgetQTY { get; set; }
        public decimal PercenQTY { get; set; }
        public decimal BudgetAMT { get; set; }
        public decimal PercenAMT { get; set; }
    }
}
