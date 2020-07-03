using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace CESAPSCOREWEBAPP.Models
{
    public class FileManal
    {
        [Key]
        public int FileManalId { get; set; }
        public string FileManalName { get; set; }

        //[DataType(DataType.Date)]
        //public DateTime FileManalDate { get; set; }//วันที่เพิ่ม หมวดหมู่คู่มือ Now
        //public string FileManalUser { get; set; }
        public string FileManalType { get; set; }
        public int ManualId { get; set; }

        //public Manual Manuals { get; set; }
    }
}
