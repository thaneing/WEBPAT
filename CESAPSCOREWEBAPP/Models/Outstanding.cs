using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace CESAPSCOREWEBAPP.Models
{
    public class Outstanding
    {
        [Key]
        public Int64 ID { get; set; }
        public string OrderNo { get; set; }
        public DateTime OrderDate { get; set; }
        public string VendorNo { get; set; }
        public string VendorName { get; set; }
        public string ItemInLine { get; set; }
        public string Description { get; set; }
        public decimal DirectUnitCost { get; set; }
        public decimal OutstandingQuantity { get; set; }
        public string UOM { get; set; }
        public string JobNo { get; set; }
        public decimal Quantity { get; set; }
        public decimal Total { get; set; }
        public string TypeOrder { get; set; }
        public string Filter { get; set; }


    }
}
