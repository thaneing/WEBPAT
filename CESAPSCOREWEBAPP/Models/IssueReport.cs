using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace CESAPSCOREWEBAPP.Models
{
    public class IssueReport
    {
        [Key]
        public Int64 ID { get; set; }
        public string ItemNo { get; set; }
        public string Des { get; set; }
        public string JobNo { get; set; }

        public string UnitOfMesure { get; set; }
        public decimal Diff { get; set; }
        public decimal IssueQty { get; set; }
        public decimal ReturnQty { get; set; }
        //public decimal TotalC { get; set; }
        public decimal UnitCostAvg { get; set; }
        public decimal OTotal { get; set; }



    }
}
