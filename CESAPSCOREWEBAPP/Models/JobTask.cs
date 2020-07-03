using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace CESAPSCOREWEBAPP.Models
{
    public class JobTask
    {
        [Key]
        public string JobTaskNo { get; set; }
        public string MainJobID { get; set; }
        public string SubJobID { get; set; }
        public string DescriptionFull { get; set; }
        public string DescriptionSubJob { get; set; }
        public string JobTaskCut { get; set; }
        public string JobstringCust { get; set; }



    }
}
