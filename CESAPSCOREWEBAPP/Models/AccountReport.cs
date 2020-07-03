using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace CESAPSCOREWEBAPP.Models
{
    public class AccountReport
    {
        [Key]
        public Int32 ID { get; set; }
        public string ACCLV4 { get; set; }
        public string ACCLV3 { get; set; }
        public string ACCLV2 { get; set; }
        public string ACCLV1 { get; set; }
        public string JobGL { get; set; }
        public DateTime PostingDate { get; set; }
        public string CloseAcc { get; set; }
        public Decimal Amount { get; set; }
        public string CostCode { get; set; }
        public string JobTaskNo { get; set; }
        public string JobGLName { get; set; }
    }

}
