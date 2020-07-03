using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace CESAPSCOREWEBAPP.Models
{
    public class Nationality
    {
        [Key]
        public int NationalityId { get; set; }
        public string NationalityName { get; set; }
        public ICollection<User> Users { get; set; }
    }
}
