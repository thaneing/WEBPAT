using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace CESAPSCOREWEBAPP.Models
{
    public class PostedPurchaseInvoices
    {
        [Key]
        public string DocumentNo { get; set; }
        public string PostingDate { get; set; }
        public string RefReceiptNo { get; set; }
        public string VendorName { get; set; }
        public string JobOrderNo { get; set; }
        public string VendorInvoiceNo { get; set; }
       /// public string VendorShipmentNo { get; set; }
        public decimal Amount { get; set; }
        public int ProgressTerm { get; set; }
        public string OrderNo { get; set; }
    }
}
