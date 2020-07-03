using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace CESAPSCOREWEBAPP.Models
{
    public class TypeOfResign
    {
        [Key]
        public int TypeOfResignId { get; set; }
        public string TypeOfResignName { get; set; }

        public ICollection<HRRecruite> HRRecruites { get; set; }

    }
}
