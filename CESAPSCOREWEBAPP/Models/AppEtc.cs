using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CESAPSCOREWEBAPP.Models
{
    public class AppEtc
    {
        public int AppEtcId { get; set; }

        public string AppEtcName { get; set; }
        public ICollection<Appointment> appointments { get; set; }
    }
}
