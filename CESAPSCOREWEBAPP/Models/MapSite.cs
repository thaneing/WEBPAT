using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace CESAPSCOREWEBAPP.Models
{
    public class MapSite
    {
        [Key]
        public int MapSiteId { get; set; }
        public string SiteName { get; set; }
        public string  SiteAddress { get; set; }
        public double Latitude { get; set; }
        public double Longitude { get; set; }
    }
}
