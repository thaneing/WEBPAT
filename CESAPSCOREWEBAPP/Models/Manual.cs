using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace CESAPSCOREWEBAPP.Models
{
    public class Manual
    {
        [Key]
        public int ManualId { get; set; }
        public string ManualName { get; set; }
        public string ManualLink { get; set; }
        public string ManuaDetail { get; set; }
        [DataType(DataType.Date)]
        public DateTime ManualDate { get; set; }//วันที่เพิ่มข้อมูลคู่มือ Date Now
        public DateTime? ManuaEditLastDate { get; set; }//วันที่แก้ไขข้อมูลคู่มือ Date Now
        public ManualHit ManualHits { get; set; }//เรียก class enum
        public ManualEnable ManualEnables { get; set; }//เรียก class enum
        public string ManualUser { get; set; }
        public string ManualUserEdit { get; set; }
        public int ManualCatId { get; set; }

        public ManualCat ManualCats { get; set; }

        //public ICollection<PictureManual> PictureManuals { get; set; }
        //public ICollection<FileManal> FileManals { get; set; }
        //public ICollection<Viewer> Viewers { get; set; }
        //public ICollection<Rate> Rates { get; set; }

        public enum ManualEnable
        {
            show,
            hide
        }
        public enum ManualHit
        {
            hit,
            none
        }
    }
}


