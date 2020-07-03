using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace CESAPSCOREWEBAPP.Models
{
    public class Rate
    {
        [Key]
        public int RateId { get; set; }
        public decimal RateScore { get; set; }

        [DataType(DataType.Date)]
        public DateTime RateDate { get; set; }//วันที่เพิ่ม หมวดหมู่คู่มือ Now
      
        public string RateUser { get; set; }
        public string ManualId { get; set; }

    }
}
