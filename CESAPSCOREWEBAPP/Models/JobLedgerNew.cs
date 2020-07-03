using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace CESAPSCOREWEBAPP.Models
{
    public class JobLedgerNew
    {
        public Int64 ID { get; set; }
        public DateTime PostingDate { get; set; }
        public string DocumentNo { get; set; }
        public string Document3 { get; set;}
        public string JobNo { get; set; }
        public string JobMain { get; set; }
        public string JobSub { get; set; }
     
        public string JobLedgerEntry { get; set; }
        public string JobLedgerEntry1 { get; set; }
        public decimal LineAmount { get; set; }
        public decimal Quantity { get; set; }
        public decimal TotalCost { get; set; }
        public int TypeOfTask { get; set; }
        public string FromLocation { get; set; }
        public decimal Total { get; set; }
    }
}
