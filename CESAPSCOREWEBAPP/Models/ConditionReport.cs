using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace CESAPSCOREWEBAPP.Models
{
    public class ConditionReport
    {
        [Key]
        public int ID { get; set; }
        public string ReportName { get; set; }
        public string ReportDimension { get; set; }
        public string ReportValue { get; set; }
    }
}
