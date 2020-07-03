using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CESAPSCOREWEBAPP.Helpers;
using CESAPSCOREWEBAPP.Models;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using static CESAPSCOREWEBAPP.Models.Enums;

namespace CESAPSCOREWEBAPP.Controllers
{
    public class JobPlanningLineController : BaseController
    {
        private readonly DatabaseContext _context;
        private readonly NAVContext _navcontext;

        private const string XlsxContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
        private readonly IHostingEnvironment _hostingEnvironment;



        public JobPlanningLineController(DatabaseContext context, NAVContext navcontext, IHostingEnvironment hostingEnvironment)
        {
            _context = context;
            _navcontext = navcontext;
            _hostingEnvironment = hostingEnvironment;
        }



        public IActionResult Index()
        {

            /*Check Session */
            var page = "200";
            var typeofuser = "";
            var PermisionAction = "";
            // CheckSession
            if (string.IsNullOrEmpty(HttpContext.Session.GetString("Username")))
            {
                Alert("กรุณา Login เข้าสู่ระบบ", NotificationType.error);
                return RedirectToAction("Login", "Accounts");
            }
            else
            {
                typeofuser = HttpContext.Session.GetString("TypeOfUserId");
                PermisionAction = HttpContext.Session.GetString("PermisionAction");
                if (PermisionHelper.CheckPermision(typeofuser, PermisionAction, page) == false)
                {
                    Alert("คุณไม่มีสิทธิ์ใช้งานหน้าดังกล่าว", NotificationType.error);
                    return RedirectToAction("Index", "Home");
                }
            }
            /*Check Session */

            return View();
        }

        public IActionResult GenData()
        {


            /*Check Session */
            var page = "200";
            var typeofuser = "";
            var PermisionAction = "";
            // CheckSession
            if (string.IsNullOrEmpty(HttpContext.Session.GetString("Username")))
            {
                Alert("กรุณา Login เข้าสู่ระบบ", NotificationType.error);
                return RedirectToAction("Login", "Accounts");
            }
            else
            {
                typeofuser = HttpContext.Session.GetString("TypeOfUserId");
                PermisionAction = HttpContext.Session.GetString("PermisionAction");
                if (PermisionHelper.CheckPermision(typeofuser, PermisionAction, page) == false)
                {
                    Alert("คุณไม่มีสิทธิ์ใช้งานหน้าดังกล่าว", NotificationType.error);
                    return RedirectToAction("Index", "Home");
                }
            }
            /*Check Session */

            IActionResult response = Unauthorized();
            var queryJobPlanningLine = "SELECT ROW_NUMBER() OVER (ORDER BY [Job Task No_]) as ID," +
           "dbo."+ Environment.GetEnvironmentVariable("Company") +"Job Task].[Job Task No_] as JobTaskNo, " +
            "'' as JobNo, " +
            "dbo."+ Environment.GetEnvironmentVariable("Company") +"Job Task].Description as JobTaskCut, " +
            "SUBSTRING(dbo."+ Environment.GetEnvironmentVariable("Company") +"Job Task].[Job Task No_],1,1)+'0000' as MainJobID, " +
            "SUBSTRING(dbo."+ Environment.GetEnvironmentVariable("Company") +"Job Task].[Job Task No_],1,3)+'00' as SubJobID, " +

            "0.00 as TotalCost, " +
            "0.00 as Quantity " +
            "FROM [dbo]."+ Environment.GetEnvironmentVariable("Company") +"Job Task] " +
            "WHERE [Job No_]='ANDAZ' AND [Job Task Type]<>4 ";
            var jobPlanningLines = _navcontext.jobPlanningLines.FromSqlRaw(queryJobPlanningLine).ToList();

            response = Ok(new { data = jobPlanningLines });


            return response;
        }
    }
}