using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace CESAPSCOREWEBAPP.Models
{
    public class FAChangeStatus
    {
        [Key]
        public Int64 ListNo { get; set; }
        public string ItemNo { get; set; }
        public string Description { get; set; }
        public string LocationCode { get; set; }
        public decimal StartDay { get; set; }
        public decimal Cutoff { get; set; }
        public decimal Diff { get; set; }
    }
}
