using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace CESAPSCOREWEBAPP.Models
{
    public class JobPlanningLine
    {
        [Key]
        public Int64 ID { get; set; }
        public string JobNo { get; set; }
        public string JobTaskCut { get; set; }
        public string MainJobID { get; set; }
        public string SubJobID { get; set; }
        
        public string JobTaskNo { get; set; }
        public Decimal TotalCost { get; set; }
        public Decimal Quantity { get; set; }



    }
}
