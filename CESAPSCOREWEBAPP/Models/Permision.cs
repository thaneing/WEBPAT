using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace CESAPSCOREWEBAPP.Models
{
    public class Permision
    {
        [Key]
        public int PermisionId { get; set; }
        public string PermisionName { get; set; }
        public string PermisionAction { get; set; }
        public ICollection<Login> Logins { get; set; }
    }
}
