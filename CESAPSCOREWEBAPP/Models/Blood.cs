using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace CESAPSCOREWEBAPP.Models
{
    public class Blood
    {
        [Key]
        public int BloodId { get; set; }
        public string BloodName { get; set; }
        public ICollection<HRRecruite> HRRecruites { get; set; }
        public ICollection<User> Users { get; set; }
    }
}
