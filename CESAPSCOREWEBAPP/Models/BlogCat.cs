using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace CESAPSCOREWEBAPP.Models
{
    public class BlogCat
    {
        [Key]
        public int BlogCatId { get; set; }
        public string BlogCatName { get; set; }
        public ICollection<Blog>Blogs { get; set; }
    }
}
