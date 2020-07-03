using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace CESAPSCOREWEBAPP.Models
{
    public class DocumentReceivable
    {
        [Key]
        public Int64 ID { get; set; }
        public DateTime PostingDate { get; set; }
        public string DocNo { get; set; }
        public string JobNo { get; set; }
        public string Detail { get; set; }
        public decimal Outstanding { get; set; }
        public decimal Amount { get; set; }
        public decimal TotalNAV { get; set; }

        public Status Statuss { get; set; }
        public enum Status
        {
            Open,
            Close
        }
        public string CreateBy { get; set; }
        public string EditBy { get; set; }
        public DateTime CreateDate { get; set; }
        public DateTime? EditDate { get; set; }
    }
}
