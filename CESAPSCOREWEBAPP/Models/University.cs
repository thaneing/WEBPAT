using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CESAPSCOREWEBAPP.Models
{
    public class University
    {
       public int UniversityId { get; set; }
       public string UniversiryName { get; set; }

       public ICollection<HRRecruite> HRRecruites { get; set; }
    }
}
