using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace CESAPSCOREWEBAPP.Models
{
    public class HRRecruiteStatus
    {
        [Key]
        public int HRRecruiteStatusId { get; set; }
        public string HRRecruiteStatusName { get; set; }
        public ICollection<HRRecruite> HRRecruites { get; set; }
    }
}
