using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace CESAPSCOREWEBAPP.Models
{
    public class TranSectionFixAsset
    {
        [Key]
        public int TranSectionFixAssetId { get; set; }
        public string FixAccNo { get; set; }
        public decimal FixAssetQty { get; set; }
        public string site { get; set; }
        public DateTime TransectionDate { get; set; }
        public string ActionData { get; set; }
        public string FixAssetItem { get; set; }
        public string FixAssetItem2 { get; set; }

        public string TransectionType { get; set; }

        public string TransectionBy { get; set; }
        public string TransectionEtc { get; set; }
    }
}
