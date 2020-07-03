using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace CESAPSCOREWEBAPP.Models
{
    public class Rental
    {
        [Key]
        public int JobLedgerEntry { get; set; }
        public string PostingDate { get; set; }
        public string ResourceNo { get; set; }
        public decimal Quantity { get; set; }
        public string JobNo { get; set; }
        public string JobTaskNo { get; set; }
        public string Description { get; set; }
        public string DocumentNo { get; set; }
        public decimal ReturnQty { get; set; }
        public decimal Diff { get; set; }
        public string FANo { get; set; }


    }
}
