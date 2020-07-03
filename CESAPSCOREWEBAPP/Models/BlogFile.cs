using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace CESAPSCOREWEBAPP.Models
{
    public class BlogFile
    {
        [Key]
        public int BlogFileId { get; set; }
        public string BlogFileName { get; set; }
        public string BlogFileType { get; set; }
        public int BlogId { get; set; }
    }
}
