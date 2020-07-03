using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace CESAPSCOREWEBAPP.Models
{
    public class ReportLevel
    {
        [Key]
        public string HeadLevel { get; set; }
        public int CountLevels { get; set; }
        public string progressbarLevel { get; set; }
    }
}
