using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace CESAPSCOREWEBAPP.Models
{
    public class ItemLedgerEntry
    {
 
        [Key]
        public Int64 ID { get; set; }
        public string ItemNo { get; set; }
        public string Description { get; set; }
        public decimal TotalItem { get; set; }
        public string LocationCode { get; set; }
        public decimal Quantity { get; set; }


    }
}
