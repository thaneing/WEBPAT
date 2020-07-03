using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace CESAPSCOREWEBAPP.Models
{
    public class V_ItemCode
    {
        [Key]
        public Int64 ID { get; set; }
        public string ItemNo { get; set; }
        public string ItemNo2 { get; set; }
        public string Description { get; set; }
        public string Description2 { get; set; }
        public string Unit { get; set; }
        public string Groupcode { get; set; }
        public string Costcode { get; set; }
    }
}
