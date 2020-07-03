using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CESAPSCOREWEBAPP.Models
{
    public class FixAccessDataTmp
    {
        [Key]
        public int FixAssId { get; set; }
        public string FixAccNo { get; set; }
        public string Description { get; set; }
        public string Description2 { get; set; }
        public string FALocation { get; set; }
        public string RefPC { get; set; }
        public string RefPCDetail { get; set; }
        public decimal FAQty { get; set; }
        public decimal FATransfer { get; set; }
        public DateTime? LastModifi { get; set; }
        public string ActionData { get; set; }
        public string FixAssetType { get; set; }
        public string FixAssetDetail { get; set; }







    }
}
