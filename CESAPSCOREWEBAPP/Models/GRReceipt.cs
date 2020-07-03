using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace CESAPSCOREWEBAPP.Models
{
    public class GRReceipt
    {
        [Key]
        public Int64 ID { get; set; }
        public string PostingGR { get; set; }
        public string GRNo { get; set; }
        public string JobType { get; set; }
        public string OrderDoc { get; set; }
        public string JobGL { get; set; }
        public decimal ReceiveAmountLine { get; set; }
        public string VendorNo { get; set; }
        public string VendorName { get; set; }
        public string Type { get; set; }

    }
}
