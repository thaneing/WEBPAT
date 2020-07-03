using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace CESAPSCOREWEBAPP.Models
{
    public class V_CheckPOPCJobtask
    {
        [Key]
        public Int64 ID { get; set; }
        public string DocumentDate { get; set; }
        public string LocationCode { get; set; }
        public string DocumentNo { get; set; }
        public string ItemNo { get; set; }
        public string Description { get; set; }
        public string Description2 { get; set; }
        public string Quantity { get; set; }
        public string Amount { get; set; }
        public string LastReceive { get; set; }
        public string UserCreate { get; set; }
    }
}
