using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace CESAPSCOREWEBAPP.Models
{
    public class TypeOfSalary
    {
        [Key]
        public int TypeOfSalaryId { get; set; }
        public string TypeOfSalaryName { get; set; }


        public ICollection<HRRecruite> HRRecruites { get; set; }
    }
}
