using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace CESAPSCOREWEBAPP.Models
{
    public class V_ListJobTask
    {
        [Key]
        public Int64 ID { get; set; }
        public string JobTaskNo { get; set; }
        public string Description { get; set; }
        public string DescriptionFull { get; set; }
        public string MainJobID { get; set; }
        public string DescriptionMainJob { get; set; }
        public string SubJobID { get; set; }
        public string DescriptionSubJob { get; set; }
    }
}
