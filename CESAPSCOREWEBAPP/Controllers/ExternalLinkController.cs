using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CESAPSCOREWEBAPP.Controllers
{
    [Authorize]
    public class ExternalLinkController : Controller
    {

        public IActionResult Photo()
        {


            return View();
        }

        public IActionResult Meeting()
        {


            return View();
        }

        public IActionResult WebMail()
        {


            return View();
        }
    }
}