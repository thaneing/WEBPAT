using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace CESAPSCOREWEBAPP.Models
{
    public class ReportDepartment1
    {
        [Key]
        public string HeadDepartment1 { get; set; }
        public int CountDepartment1 { get; set; }
        public int TotalEmployee1 { get; set; }
        public string ProgressbarDepartment1 { get; set; }
    }
}
