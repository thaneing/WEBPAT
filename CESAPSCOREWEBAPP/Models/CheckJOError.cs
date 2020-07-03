using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace CESAPSCOREWEBAPP.Models
{
    public class CheckJOError
    {
        [Key]
        public Int64 ID { get; set; }
        public string PostingDate { get; set; }
        public string GR { get; set; }
        public string OrderDoc { get; set; }
        public string ItemNo { get; set; }
        public string Des1 { get; set; }
        public string Des2 { get; set; }
        public decimal Quantity { get; set; }
        public decimal UnitCost { get; set; }
        public string JobNo { get; set; }
        public decimal TotalReceive { get; set; }
        public decimal Retention { get; set; }
        public string TotalRetention { get; set; }
        public string CalRetention { get; set; }
        public string UserId { get; set; }
    }
}
