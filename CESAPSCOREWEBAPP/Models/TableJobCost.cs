using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CESAPSCOREWEBAPP.Models
{
    public class TableJobCost
    {

            // Auto-Initialized properties  
            public string JobTask { get; set; }
            public decimal ThisPeriodQTY { get; set; }
            public decimal ThisPeriodAMT { get; set; }
            public decimal OpenningQTY { get; set; }
            public decimal OpenningAMT { get; set; }
            public decimal CumBalQTY { get; set; }

            public decimal CumBalAMT { get; set; }

             public decimal BudgetQty { get; set; }
            public decimal PerQty { get; set; }
            public decimal BudgetAMT { get; set; }
            public decimal PerAMT { get; set; }
    }

}
