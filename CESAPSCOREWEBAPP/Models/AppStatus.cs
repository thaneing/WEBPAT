using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace CESAPSCOREWEBAPP.Models
{
    public class AppStatus
    {
        [Key]
        public int AppStatusId { get; set; }
        public string AppStatusName { get; set; }
        public ICollection<Appointment> Appointments { get; set; }
    }
}
