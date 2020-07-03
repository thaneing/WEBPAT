using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace CESAPSCOREWEBAPP.Models
{
    public class RegisGroupJob
    {
        [Key]
        public int ID { get; set; }
        public string GroupCode { get; set; }
        public string GroupName { get; set; }
        public string GroupDes { get; set; }
        public string RegisBy { get; set; }
        public DateTime RegisDate { get; set; }
        public string GroupSite { get; set; }

    }
}
