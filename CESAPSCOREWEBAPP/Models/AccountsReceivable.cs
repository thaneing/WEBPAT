using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace CESAPSCOREWEBAPP.Models
{
    public class AccountsReceivable
    {
        [Key]
        public Int64 ID { get; set; }
        public string PostingDate { get; set; }
        public string Doc { get; set; }
        public string JobNo { get; set; }
        public string Description { get; set; }
        public string CustomerGroup { get; set; }
        public decimal SumTotal { get; set; }
        public decimal SumTotalVat { get; set; }
        public decimal TotalNAV { get; set; }
        public decimal Retention { get; set; }

    }
}
