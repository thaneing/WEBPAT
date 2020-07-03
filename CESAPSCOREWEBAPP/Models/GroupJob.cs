using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace CESAPSCOREWEBAPP.Models
{
    public class GroupJob
    {
        [Key]
        public int ID { get; set; }
        public string DocumentNo { get; set; }
        public string JobNo { get; set; }
        public string Description { get; set; }
        public string PostingDate { get; set; }
        public decimal Amount { get; set; }
        public string GroupNo { get; set; }
        public string GroupName { get; set; }
        public DateTime CreateDate { get; set; }
        public string CreateBy { get; set; }
        public DateTime? EditDate { get; set; }
        public string EditBy { get; set; }
    }
}
