using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace CESAPSCOREWEBAPP.Models
{
    public class Povince
    {
        [Key]
        public int PovinceId { get; set; }
        public string PovinceName { get; set; }
        public ICollection<User> Users { get; set; }
    }
}
