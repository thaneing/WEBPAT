using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace CESAPSCOREWEBAPP.Models
{
    public class HouseRentalFile
    {
        [Key]
        public int HouseRentalFileID { get; set; }
        public string HouseRentalFileName { get; set; }
        public string HouseRentalFileType { get; set; }
        public int ID { get; set; }
        public string username { get; set; }
    }
}
