using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace CESAPSCOREWEBAPP.Models
{
    public class MonitorPurchase
    {
        [Key]
        public Int64 ID { get; set; }
        public string POName { get; set; }
        public string No { get; set; }
        public string Job_GL_Code { get; set; }
        public string LocationHead { get; set; }
        public string LocationLine { get; set; }
        public string Job_GL_Code_Line { get; set; }
        public string JobNo { get; set; }
        public string JobTaskNo { get; set; }
        public string Description { get; set; }
        public string Description2 { get; set; }
        public decimal Quantity { get; set; }
        public decimal OutsatandingQty { get; set; }
        public string CostCode { get; set; }
        public string OrderDate { get; set; }

    }
}
