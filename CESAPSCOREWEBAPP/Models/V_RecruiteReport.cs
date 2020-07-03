using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace CESAPSCOREWEBAPP.Models
{
    public class V_RecruiteReport
    {
        [Key]
        public long RowNumber { get; set; }
        public string DepartmentName { get; set; }
        public string Department1Name { get; set; }
        public string PositionName { get; set; }
        public int RecuireNow { get; set; }
        public int Apptotal { get; set; }
        public int AppTel { get; set; }
        public string PerAppTel { get; set; }
        public int AppCome { get; set; }
        public string PerAppCome { get; set; }
        public int AppWait { get; set; }
        public string PerAppWait { get; set; }
        public int AppSucc { get; set; }
        public string PerAppSucc { get; set; }
        public int AppStart { get; set; }
        public string PerAppStart { get; set; }
        public int AppEtc2 { get; set; }
        public int AppEtc3 { get; set; }
        public int AppEtc4 { get; set; }
        public int AppEtc5 { get; set; }



    }
}
