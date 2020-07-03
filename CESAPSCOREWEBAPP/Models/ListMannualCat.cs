using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace CESAPSCOREWEBAPP.Models
{
    public class ListMannualCat
    {
        [Key]
        public string Namelist { get; set; }
        public string Detail { get; set; }
    }
}
