using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CESAPSCOREWEBAPP.Models
{
    public class Monitor
    {
        public int Id {get;set;}
        public DateTime Date { get; set; }
        public string IP { get; set; }
        public string Mac { get; set; }
        public string OS { get; set; }
        public string OSVersion { get; set; }
        public string Browser { get; set; }
        public string BrowserVersion { get; set; }
        public string Latitude { get; set; }
        public string Longitude { get; set; }
        public string Username { get; set; }




    }
}
