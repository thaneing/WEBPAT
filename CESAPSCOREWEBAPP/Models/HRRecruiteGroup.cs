﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace CESAPSCOREWEBAPP.Models
{
    public class HRRecruiteGroup
    {
        [Key]
        public int HRRecruiteGroupId { get; set; }
        public string HRRecruiteGroupDetail { get; set; }

        public ICollection<HRRecruite> HRRecruites { get; set; }
    }
}
