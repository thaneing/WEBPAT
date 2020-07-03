using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CESAPSCOREWEBAPP.Models
{
    public class RateOfRental
    {
        public Int64 ID { get; set; }
        public string ItemNo { get; set; }
        public string Description { get; set; }
        public string RentalUnit { get; set; }
        public string RentalPrice { get; set; }

    }
}

