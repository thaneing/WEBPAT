using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace CESAPSCOREWEBAPP.Models
{
    public class V_Job
    {
      
        [Key]
        public string JobNo { get; set; }
        public string LocationCode { get; set; }

    }
}
