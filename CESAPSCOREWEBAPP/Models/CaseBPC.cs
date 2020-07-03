using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace CESAPSCOREWEBAPP.Models
{
    public class CaseBPC
    {
        [Key]
        public int CaseBPCId { get; set; }
        public DateTime CaseBPCDate { get; set; }
        public string CaseMA { get; set; }
        public string CaseBPCSubject { get; set; }
        public string CaseBPCDetail { get; set; }
        public CaseBPCStatus caseBPCStatus { get; set; }
        public CaseBPCPLevel caseBPCPLevel { get; set; }


        public DateTime? CaseBPCPDateFix { get; set; }
        public string EditBy { get; set; }
        public  string openCaseBy { get; set; }




        public enum CaseBPCStatus
        {
            SendCase,
            WaitCES,
            WaitBCP,
            Complete
        }

        public enum CaseBPCPLevel
        {
            Level1,
            Level2,
            Level3,
            Level4,
            Level5
        }
    }
}
