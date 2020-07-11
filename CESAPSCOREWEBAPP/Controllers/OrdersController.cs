using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using CESAPSCOREWEBAPP.Models;
using DevExtreme.AspNet.Data;
using DevExtreme.AspNet.Mvc;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CESAPSCOREWEBAPP.Controllers {

    [Authorize]
    [Route("api/[controller]")]
    public class OrdersController : Controller {

        [HttpGet]
        public object Get(DataSourceLoadOptions loadOptions) {
            return DataSourceLoader.Load(SampleData.Orders, loadOptions);
        }

    }
}