using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace CESAPSCOREWEBAPP.Models
{
    public class DocumentBilling
    {
        [Key]
        public int ID { get; set; }
        public DateTime PostingDate { get; set; }
        public string PONo { get; set; }
        public string Site { get; set; }
        public string VendorName { get; set; }
        public string InvoiceNo { get; set; }
        public string DeliveryOrder { get; set; }
        public string Etc { get; set; }
        public string CreateBy { get; set; }
        public DateTime CreateDate {get;set;}
        public string UpdateBy { get; set; }
        public DateTime? UpdateDate { get; set; }
    }
}
