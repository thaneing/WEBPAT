using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace CESAPSCOREWEBAPP.Models
{
    public class AppTelType
    {
        [Key]
        public int AppTelTypeId { get; set; }
        public string AppTelTypeName { get; set; }

        public ICollection<Appointment> Appointments { get; set; }
    }
}
