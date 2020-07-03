using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace CESAPSCOREWEBAPP.Models
{
    public class CHQCurrent
    {
        [Key]

        public Int64 ID { get; set; }
        public string YearData { get; set; }
        public string MonthDate { get; set; }
        public DateTime PostingDate { get; set; }
        public string BankCode { get; set; }
        public string CHQNo { get; set; }

    }
}
