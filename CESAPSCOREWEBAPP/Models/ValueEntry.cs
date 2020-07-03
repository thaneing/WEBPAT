using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace CESAPSCOREWEBAPP.Models
{
    public class ValueEntry
    {
        [Key]
        public int EntryNo { get; set; }
        public string PostingDate { get; set; }
        public string UserID { get; set; }
        public string DocumentNo { get; set; }
        public string Description { get; set; }
        public decimal CostAmountActual { get; set; }
        public string DocumentType { get; set; }
        public string Adjustment { get; set; }
        public string ItemLedgerEntryType { get; set; }
        public decimal CostperUnit { get; set; }


    }
}
