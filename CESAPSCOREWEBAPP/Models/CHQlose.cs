﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace CESAPSCOREWEBAPP.Models
{
    public class CHQlose
    {
        [Key]

        public int ID { get; set; }
        public string BankCode { get; set; }
        public string BankName { get; set; }
        public DateTime PostingDate { get; set; }
        public string CHQNo { get; set; }
        public string Etc { get; set; }
        public string CreateBy { get; set; }
        public string EditBy { get; set; }
        public DateTime CreateDate { get; set; }
        public DateTime? EditDate { get; set; }
    }
}
