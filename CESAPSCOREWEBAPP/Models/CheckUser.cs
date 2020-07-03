using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace CESAPSCOREWEBAPP.Models
{
    public class CheckUser
    {
        [Key]
        public int CheckUserId { get; set; }
        public string CheckUserName { get; set; }
        public ICollection<Login> Logins { get; set; }
    }
}
