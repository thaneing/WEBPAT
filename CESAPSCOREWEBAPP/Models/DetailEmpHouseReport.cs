using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace CESAPSCOREWEBAPP.Models
{
    public class DetailEmpHouseReport
    {
        [Key]
        public string Code { get; set; }
        public string Name { get; set; }
        public string PositionName { get; set; }
        public string Site { get; set; }
    }
}
