using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace CESAPSCOREWEBAPP.Models
{
    public class SessionNav
    {
        [Key]
        public Guid SID { get; set; }
        public string ID { get;set; }
        public string DateLogin { get; set; }
        public string DateDoIt { get; set; }
    }
}
