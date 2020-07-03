using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace CESAPSCOREWEBAPP.Models
{
    public class PictureManual
    {
        [Key]
        public int PictureManualId { get; set; }
        public string PictureManualName { get; set; }

        //[DataType(DataType.Date)]
        //public DateTime PictureDate { get; set; }//วันที่เพิ่ม หมวดหมู่คู่มือ Now
        //public string PictureUser { get; set; }
        public int ManualId { get; set; }

        //public Manual Manuals { get; set; }

     
    }
}
