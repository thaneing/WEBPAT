using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace CESAPSCOREWEBAPP.Models
{
    public class ReportManagement
    {
        [Key]
        public string LevelHead { get; set; }
        public int CountMnagement { get; set; }
        public string ProgressbarManagement { get; set; }
    }
}
