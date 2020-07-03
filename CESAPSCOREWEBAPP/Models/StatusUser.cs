using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace CESAPSCOREWEBAPP.Models
{
    public class StatusUser
    {
        [Key]
        public int StatusUserId { get; set; }
        public string StatusUserName { get; set; }
        public ICollection<User> Users { get; set; }
    }
}
