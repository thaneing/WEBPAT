﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace CESAPSCOREWEBAPP.Models
{
    public class WebModul
    {
        [Key]
        public int WebModulId { get; set; }
        public string WebModulName { get; set; }
    }
}
