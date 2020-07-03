using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace CESAPSCOREWEBAPP.Models
{
    public class DataXY
    {
        [Key]
        public string X { get; set; }
        public int Y { get; set; }
    }


    public class DataXYDecimal
    {
        [Key]
        public string X { get; set; }
        public decimal Y { get; set; }
    }


    public class DataXYString
    {
        [Key]
        public string X { get; set; }
        public string Y { get; set; }
    }
    public class DataXXY
    {
        [Key]
        public string Name { get; set; }
        public int CountData { get; set; }
    }
}
