using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace CESAPSCOREWEBAPP.Models
{
    public class PurchaseVat
    {
        public Byte UseTax { get; set; }
        [Key]
        public int Entry { get; set; }
        public string PostingDate { get; set; }
        public string DocumentDate { get; set; }
        public string DocumentNo { get; set; }
        public string ExternalDocument { get; set; }
        public string CusVenNo { get; set; }
        public string  CusVenName { get; set; }
        public string CusVenName2 { get; set; }
        public string VATRegis { get; set;}
        public string HeadOffice { get; set; }
        public string Branch { get; set; }
        public decimal Base { get; set; }
        public decimal Amount { get; set; }
        public decimal Vat { get; set; }
        public string VatBusPostingGroup { get; set; }
        public string DocumentAP { get; set; }
    }
}
