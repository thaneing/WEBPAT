using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CESAPSCOREWEBAPP.Controllers
{
    [Authorize]
    public class MenusetingController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}