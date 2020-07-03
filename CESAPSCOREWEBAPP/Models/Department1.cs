using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace CESAPSCOREWEBAPP.Models
{
    public class Department1
    {
        [Key]
        public int Department1Id {get;set;}
        public string Department1Name { get; set; }

        public ICollection<Organiz> Organizs { get; set; }
    }
}
