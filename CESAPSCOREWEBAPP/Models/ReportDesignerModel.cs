using DevExpress.XtraReports.UI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CESAPSCOREWEBAPP.Models
{
    public class ReportDesignerModel
    {
        public XtraReport Report { get; set; }
        public Dictionary<string, object> DataSources { get; set; }
    }
}
