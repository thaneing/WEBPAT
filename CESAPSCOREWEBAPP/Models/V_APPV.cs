using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace CESAPSCOREWEBAPP.Models
{
    public class V_APPV
    {
        public string PostingDate { get; set; }
        [Key]
        public int EntryNo { get; set; }
        public int VendorLedgerEntry { get;set;}
        public string DocumentNo { get; set; }
        public string VendorNo { get; set; }
        public string VendorName { get; set; }
        public string VendorPostingGroup { get; set; }
        public string DocumentType { get; set; }
        public string Amount { get; set; }
        public string AmountLCY { get; set; }
        public string UserId { get; set; }
        public string SourceCode { get; set; }
        public string InitialEntryDueDate { get; set; }
        public string InitialEntryGlobalDim1 { get; set; }
        public string InitialEntryGlobalDim2 { get; set; }
        public string EntryType { get; set; }
        public int CountDocument { get; set; }
        public string Documentname { get; set; }
        public string PayDate { get; set; }
        public decimal? VatAmount { get; set; }

    }
}
