using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace CESAPSCOREWEBAPP.Models
{
    public class ListReturn
    {
        [Key]
        public int EntryNo { get; set; }
        public int RentalApply { get; set; }
        public string JobNo { get; set; }
        public string PostingDate { get; set; }
        public string DocumentNo { get; set; }
        public string ItemNo { get; set; }
        public string ResourceNo { get; set; }
        public string Description { get; set; }
        public string Description2 { get; set; }
        public decimal Quantity { get; set; }
        public string UnitOfMesure { get; set; }
        public string LocationCode { get; set; }
        public string JournalBatch { get; set; }
        public string DocumentDate { get; set; }
        public string JobTaskNo { get; set; }
        public int LedgerEntryType { get; set; }
        public string ShipmentNo { get; set; }
        public string Remark { get; set; }
        public string RentalType { get; set; }
        public string ToLocation { get; set; }
        public string FromLocation { get; set; }
        public string FANo { get; set; }
       



    }
}
