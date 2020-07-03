using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace CESAPSCOREWEBAPP.Models
{
    public class TitleOfUser
    {
        [Key]
        public int TitleOfUserId { get; set; }
        public string TitleOfUserName { get; set; }
        public ICollection<User> Users { get; set; }
    }
}
