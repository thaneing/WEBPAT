using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace CESAPSCOREWEBAPP.Models
{
    public class BlogPic
    {
        [Key]
        public int BlogPicId { get; set; }
        public string BlogPicName { get; set; }
        public int BlogId { get; set; }



    }
}
