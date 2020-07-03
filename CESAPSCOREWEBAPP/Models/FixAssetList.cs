using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace CESAPSCOREWEBAPP.Models
{
    public class FixAssetList
    {
        public string ItemNo { get; set; }
        public string Description { get; set; }
        public string LocationCode { get; set; }
        public decimal Quantity { get; set; }
        [Key]
        public Int64 ListNo { get; set; }
        public string Detail { get; set; }
        public string Status { get; set; }
        public string Status1 { get; set; }
        public string Status2 { get; set; }
        public string Curren { get; set; }
        public string Etc { get; set; }
    }
}
