using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace CESAPSCOREWEBAPP.Models
{
    public class FixAccessData
    {
        [Key]
        public string FixAccNo { get; set; }
        public string Description { get; set; }
        public string Description2 { get; set; }
        public string FALocation { get; set; }
        public string RefPC { get; set; }
        public string RefPCDetail { get; set; }
        public Decimal FAQty { get; set; }
        public string ActionData { get; set; }



    }
}
