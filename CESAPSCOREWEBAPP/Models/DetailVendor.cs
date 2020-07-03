using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace CESAPSCOREWEBAPP.Models
{
    public class DetailVendor
    {
        public string PostingDate { get; set; }
        [Key]
        public string DocumentInv { get; set; }
        public string DocumentReceipt { get; set; }
        public string GRDate { get; set; }
        public string RefPOJO { get; set; }
        public string PayToVendorNo { get; set; }
        public string PayToVendorName { get; set; }
        public string VatRegis { get; set; }
        public string JobGL { get; set; }
        
        public int PrograssTerm { get; set; }
        public string RefPR { get; set; }
        public decimal AmountVat { get; set; }

        public decimal AmountReceipt { get; set; }
        public string Type { get; set; }
    }
}
