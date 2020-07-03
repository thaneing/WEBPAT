using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace CESAPSCOREWEBAPP.Models
{
    public class PurchaseOrderHeader
    {
        [Key]
        public int ID { get; set; }
        public string PONumber { get; set; }
        public string VendorName { get; set; }
        public string Site { get; set; }
    }
}
