using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace CESAPSCOREWEBAPP.Models
{
    public class AppSuccess
    {
        [Key]
        public int AppSuccessId { get; set; }
        public string AppSuccessName { get; set; }
        public ICollection<Appointment> Appointments { get; set; }

    }
}
