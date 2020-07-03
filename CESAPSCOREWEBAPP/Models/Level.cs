using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace CESAPSCOREWEBAPP.Models
{
    public class Level
    {
        [Key]
        public int LevelId { get; set; }
        public string LevelName { get; set; }
        public ICollection<User> Users { get; set; }

        public ICollection<HRRecruite> HRRecruites { get; set; }
    }
}
