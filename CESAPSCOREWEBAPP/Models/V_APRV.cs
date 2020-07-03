using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace CESAPSCOREWEBAPP.Models
{
    public class V_APRV
    {
        public string PostingDate { get; set; }
        [Key]
        public int EntryNo { get; set; }
        public int CusLedgerEntryNo { get; set; }
        public string DocumentType { get; set; }
        public string DocumentNo { get; set; }
        public string CustomerNo { get; set; }
        public string CustomerName { get; set; }
        public string Description { get; set; }
        public string CustomerPostingGroup { get; set; }
        public string AmountAP { get; set; }
        public string AmountApLCY { get; set; }
        public string DueDate { get; set; }
        public string GlobalDim1 { get; set; }
        public string EntryType { get; set; }
        public string SourceCode { get; set; }
        public string UserId { get; set; }
        public int Unapplied { get; set; }
        public string RVDocNo { get; set; }
        public string RVDate { get; set; }





    }
}
