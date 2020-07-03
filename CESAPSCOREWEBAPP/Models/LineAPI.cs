using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CESAPSCOREWEBAPP.Models
{
    public class LineAPI
    {
        public int Id { get; set; }
        public string LineToken { get; set; }
        public string Detail { get; set; }
        public OnOff onOff { get; set; }

        public enum OnOff
        {
            Off,
            On

        }
    }



}
