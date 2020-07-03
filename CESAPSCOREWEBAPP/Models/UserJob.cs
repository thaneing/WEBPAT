using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace CESAPSCOREWEBAPP.Models
{
    public class UserJob
    {
        [Key]
        public int UserJobId { get; set; }
        public int UserId { get; set; }
        public string UserJobDetail {get;set;}
    }
}
