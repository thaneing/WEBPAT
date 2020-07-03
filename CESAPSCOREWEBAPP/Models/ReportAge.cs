using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace CESAPSCOREWEBAPP.Models
{
    public class ReportAge
    {
        [Key]
        public string Range { get; set; }
        public int CountAge { get; set; }
        public string progressbarAge { get; set; }
    }
}
