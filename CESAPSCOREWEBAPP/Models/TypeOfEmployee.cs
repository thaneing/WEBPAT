using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace CESAPSCOREWEBAPP.Models
{
    public class TypeOfEmployee
    {
        [Key]
        public int TypeOfEmployeeId {get;set;}
        public string TypeOfEmployeeName { get; set; }
        public ICollection<User> Users { get; set; }

    }
}
