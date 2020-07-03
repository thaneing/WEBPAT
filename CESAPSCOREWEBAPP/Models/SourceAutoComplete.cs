using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace CESAPSCOREWEBAPP.Models
{
    public class SourceAutoComplete
    {
        [Key]
        public string name { get; set; }
        public string code { get; set; }

    }

}
