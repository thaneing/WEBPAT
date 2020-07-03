using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CESAPSCOREWEBAPP.Models
{
    public class AppRoom
    {
        public int AppRoomId { get; set; }
        public string AppRoomName { get; set; }
        public string AppRoomColor { get; set; }
        public ICollection<Appointment> Appointments { get; set; }
    }
}
