using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace CESAPSCOREWEBAPP.Models
{
    public class V_CheckLedger
    {
        [Key]
        public Int64 ID { get; set; }
        public DateTime Posdate { get; set; }
        public string Item { get; set; }
        public string Description {get;set;}
        public string Document { get; set; }

        public decimal ItemNeg { get; set; }
        public decimal ItemPos { get; set; }
        public decimal DiffItem { get; set; }
        public decimal Job { get; set; }
        public decimal DiffItemPosAndJob { get; set; }

        public decimal DiffItemNegAndJob { get; set; }
    }
}
