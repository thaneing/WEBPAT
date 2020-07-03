using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CESAPSCOREWEBAPP.Models
{
    public class JobOrderReport
    {
        public Int64 ID { get; set; }
        public string OrderNo { get; set; }
        public DateTime OrderDate { get; set; }
        public string JobDesc { get; set; }
        public string VendorNo { get; set; }
        public string VendorName { get; set; }
        public string Location { get; set; }
        public string ItemNo { get; set; }
        public string Des { get; set; }
        public string Des2 { get; set; }
        public string UnitOfMeasure { get; set; }
        public decimal Qty { get; set; }
        public decimal Amount { get; set; }
        public decimal UnitCost { get; set; }
        public decimal QtyReceived { get; set; }
        public decimal TotalReceived { get; set; }


    }
}
