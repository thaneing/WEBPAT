using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace CESAPSCOREWEBAPP.Models
{
    public class Religion
    {
        [Key]
        public int ReligionId { get; set; }
        public string ReligionName { get; set; }
        public ICollection<User> Users { get; set; }
    }
}
