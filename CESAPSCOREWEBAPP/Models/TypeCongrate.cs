using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace CESAPSCOREWEBAPP.Models
{
    public class TypeCongrate
    {
        [Key]
        public int TypeCongrateId { get; set; }
        public string TypeCongrateName { get; set; }

        public ICollection<HRRecruite> HRRecruites { get; set; }
        public ICollection<User> Users { get; set; }
    }
}
