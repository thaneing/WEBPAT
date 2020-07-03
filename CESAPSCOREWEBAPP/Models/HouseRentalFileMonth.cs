using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace CESAPSCOREWEBAPP.Models
{
    public class HouseRentalFileMonth
    {
        [Key]
        public int FileMonthID { get; set; }
        public string FileMonthName { get; set; }

        public DateTime FileMonthDate { get; set; }//เดือนประจำไฟล์
        public string FileMonthType { get; set; }
        public string Period { get; set; }
        public string Job { get; set; }
    }
}
