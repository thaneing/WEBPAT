using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace CESAPSCOREWEBAPP.Models
{
    public class ReportManPower
    {
        [Key]
        public int id { get; set; }
        public string Department { get; set; }
        public string Department1 { get; set; }
        public string PositionData { get; set; }

        public int CountData { get; set; }
        public int Power { get; set; }
        public string Diff { get; set; }



    }
}
