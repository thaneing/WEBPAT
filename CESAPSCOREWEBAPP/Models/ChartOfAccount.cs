using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace CESAPSCOREWEBAPP.Models
{
    public class ChartOfAccount
    {
        [Key]
        public Int64 ID { get; set; }
        public string AccNo { get; set; }
        public string AccName { get; set; }
        public string Totaling { get; set; }
        public string QueryData { get; set; }
    }
}
