using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace CESAPSCOREWEBAPP.Models
{
    public class ManualCat
    {
        [Key]
        public int ManualCatId { get; set; }
        public string ManualCatName { get; set; }

        [DataType(DataType.Date)]
        public DateTime ManualCatDate { get; set; }//วันที่เพิ่ม หมวดหมู่คู่มือ Now
        public DateTime? ManualCatEditDate { get; set; }//วันที่เพิ่ม หมวดหมู่คู่มือ Now
        public string ManualCatUser { get; set; }
        public string ManualCatUserEdit { get; set; }
        public ICollection<Manual> Manuals { get; set; }
    }
}
