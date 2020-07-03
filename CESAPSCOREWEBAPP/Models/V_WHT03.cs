using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace CESAPSCOREWEBAPP.Models
{
    public class V_WHT03
    {
        [Key]
        public string WHTno { get; set; }
        public string WHTName { get; set; }
        public decimal SumAmount { get; set; }
        public decimal SumBase { get; set; }
        public string SumData { get; set; }
    }
}
