using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace CESAPSCOREWEBAPP.Models
{
    public class F03DATAReport
    {
        [Key]
        public Int64 ID { get; set; }
        public string DocNo { get; set; }
        public string NoGR { get; set; }
        public string Des { get; set; }
        public string ReceiptDate { get; set; }
        public string VendorName { get; set; }
        public string Ref { get; set; }
        public string Uom { get; set; }
        public decimal UnitCost { get; set; }
        public decimal Disc { get; set; }
        public decimal Qty { get; set; }
        public decimal Amount { get; set; }
        public decimal DiscPrice { get; set; }
        public string ShipmentNo { get; set; }
        public string Filter { get; set; }
    }
}
