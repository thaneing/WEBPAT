using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CESAPSCOREWEBAPP.Models
{
    public class TempDataBinding
    {
        [BindProperty]
        public string SweetAlertTempdata { get; set; }
      
    }
}
