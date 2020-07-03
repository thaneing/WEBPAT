using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace CESAPSCOREWEBAPP.Models
{
    public class BankRatio
    {
        [Key]

        public int ID { get; set; }
        public string BankName { get; set; }
        public string BankCode { get; set; }
        public int IssueQty { get; set; }
        public string CreateBy { get; set; }
        public string EditBy { get; set; }
        public DateTime CreateDate { get; set; }
        public DateTime? EditDate { get; set; }
    }
}
