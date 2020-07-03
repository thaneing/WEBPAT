using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace CESAPSCOREWEBAPP.Models
{
    public class ItemBySite
    {
        [Key]
        public string  LocationCode { get; set; }
      
        public string Total { get; set; }
    }
}
