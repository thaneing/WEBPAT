using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace CESAPSCOREWEBAPP.Models
{
    public class CheckGRError
    {
        [Key]
        public Int64 ID { get; set; }
        public string PostingDate { get; set; }
        public string DocumentNo { get; set; }
        public string ItemNo { get; set; }
        public decimal ValueDiff { get; set; }
        public string Location { get; set; }
    }
}
