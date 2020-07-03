using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace CESAPSCOREWEBAPP.Models
{
    public class APDoc
    {
        [Key]
        public Int64 ID { get; set; }
        public string DocGR { get; set; }
        public string DocGR1 { get; set; }
        public string PostingIV { get; set; }
        public string DocAP { get; set; }
        //public string OrderDoc { get; set; }
        public string VendorNo { get; set; }
        public string VendorName { get; set; }
        public string JobGL { get; set; }
        public int PrograssTerm { get; set; }
        public decimal Amount { get; set; }
    }
}
