using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace CESAPSCOREWEBAPP.Models
{
    public class AppResult
    {
        [Key]
        public int AppResultId { get; set; }
        public string AppResultName { get; set; }
        public ICollection<Appointment> Appointments { get; set; }
    }
}
