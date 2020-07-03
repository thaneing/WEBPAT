using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace CESAPSCOREWEBAPP.Models
{
    public class OrderPurchaseLines
    {
        [Key]
        public Int64 ID { get; set; }
        public DateTime OrderDate { get; set; }
        public string DocumentNo { get; set; }
        public string VendorNo { get; set; }
        public string VendorName { get; set; }
        public string Location { get; set; }
        public string ItemNo { get; set; }
        public string Description { get; set; }
        public string Description2 { get; set; }
        public string UnitOfMeasure { get; set; }
        public decimal Quantity { get; set; }
        public decimal UnitCost { get; set; }
        public decimal Amount { get; set; }
        public decimal AmountInludVat { get; set; }
        public string JobNo { get; set; }
        public string JobTaskNo { get; set; }
        public string RefPR { get; set; }
        public string InventoryPostingGroupCode { get; set; }
        public string InventoryPostingGroupName { get; set; }
        public decimal Receive { get; set; }
        public decimal TotalReceive { get; set; }
    }
}
