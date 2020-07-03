using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace CESAPSCOREWEBAPP.Models
{
    public class JobJournalLine
    {
        [Key]

        public Int64 ID { get; set; }
        public string JobNo { get; set; }
        public string ShipmentDate { get; set; }
        public string ShipmentNo { get; set; }
        public string Type { get; set; }
        public string Typeoftask { get; set; }
        public string DocumentNo { get; set; }
        public string No { get; set; }
        public string JobTaskNo { get; set; }
        public string Description { get; set; }
        public decimal Quantity { get; set; }
        public string FormLocation { get; set; }
        public string ToLocation { get; set; }
        public string RentalType { get; set; }
        public string RemConfirmQty { get; set; }
        public string BatchName { get; set; }
    }
}
