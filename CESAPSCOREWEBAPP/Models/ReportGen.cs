using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace CESAPSCOREWEBAPP.Models
{
    public class ReportGen
    {
        [Key]
        public string Head { get; set; }
        public string Etc { get; set; }
        public int CountGen { get; set; }
        public string Progressbar { get; set; }


    }
}
