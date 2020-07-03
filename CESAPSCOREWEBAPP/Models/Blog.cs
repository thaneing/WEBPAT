using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace CESAPSCOREWEBAPP.Models
{
    public class Blog
    {
        [Key]
        public int BlogId { get; set; }
        public string BlogTitle { get; set; }
        [DataType(DataType.Date)]
        public DateTime BlogDate { get; set; }

        [DataType(DataType.Date)]
        public DateTime BlogEndDate { get; set; }
        public string BlogDetail { get; set; }
        public string BlogPicTitle { get; set; }
        public int BlogStatus { get; set; }
        
        public int BlogCatId { get; set; }
   
        public DateTime BlogCreateDate { get; set; }
        public string  BlogCreateBy { get; set; }
        
        public string BlogUpdateBy { get; set; }
        public DateTime BlogUpdateDate { get; set; }

        public BlogCat BlogCats { get; set; }





    }
}
