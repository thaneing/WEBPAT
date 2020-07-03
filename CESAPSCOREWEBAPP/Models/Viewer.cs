using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace CESAPSCOREWEBAPP.Models
{
    public class Viewer
    {
        [Key]
        public int ViewerId { get; set; }

        [DataType(DataType.Date)]
        public DateTime ViewerDate { get; set; }//วันที่เพิ่ม หมวดหมู่คู่มือ Now

        public string ViewerUser { get; set; }
        public string ManualId { get; set; }

    }
}
