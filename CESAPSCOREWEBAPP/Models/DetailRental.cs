using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace CESAPSCOREWEBAPP.Models
{
    public class DetailRental
    {
        [Key]
        public Int64 ID { get; set; }
        public DateTime PostingDate { get; set; }
        public string JobNo { get; set; }
        public string FromLocation { get; set; }
        public string ToLocation { get; set; }
        public string ItemNo { get; set; }
        public string ItemCat { get; set; }
        public string Des { get; set; }
        public decimal TotalCost { get; set; }

    }
}
