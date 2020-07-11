using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;
using CESAPSCOREWEBAPP.Helpers;
using CESAPSCOREWEBAPP.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using static CESAPSCOREWEBAPP.Models.Enums;
using Microsoft.AspNetCore.Authorization;

namespace CESAPSCOREWEBAPP.Controllers
{
    [Authorize]

    public class WHT03Controller : BaseController
    {

        private readonly NAVContext _navcontext;
        private readonly DatabaseContext _context;




        public WHT03Controller(NAVContext navcontext, DatabaseContext context)
        {
            _context = context;
            _navcontext = navcontext;
        }

        public IActionResult Index()
        {

            /*Check Session */
            var page = "181";
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
         

            ViewBag.StartDate = DateTime.Now.ToString("01-01-yyyy", new CultureInfo("en-US"));
            ViewBag.EndDate = DateTime.Now.ToString("dd-MM-yyyy", new CultureInfo("en-US"));

            return View();
        }

        [Obsolete]
        public IActionResult GetData(string Startdate, string EndDate)
        {

            /*Check Session */
            var page = "181";
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


            var date1 = Startdate.Substring(6, 4) + "-" + Startdate.Substring(3, 2) + "-" + Startdate.Substring(0, 2) + " 00:00:00";
            var date2 = EndDate.Substring(6, 4) + "-" + EndDate.Substring(3, 2) + "-" + EndDate.Substring(0, 2) + " 23:59:59";

            IActionResult response = Unauthorized();
            var queryData = " SELECT b.WHTno,b.WHTName,b.SumAmount as SumAmount,b.SumBase as SumBase, "
            + " (CASE WHEN (b.SumBase<=1620000) THEN '<span class=''label label-primary''>Success</span>' "
            + " WHEN (b.SumBase<=1800000) THEN '<span class=''label label-warning''>Warning</span>' "
            + " WHEN (b.SumBase>1800000) THEN '<span class=''label label-danger''>Danger</span>' "
            + " END )as SumData"
            + " FROM(SELECT a.WHTno," 
            + " (SELECT TOP 1 [WHT Name] FROM dbo."+ Environment.GetEnvironmentVariable("Company") +"WHT Entry] WHERE [WHT Registration No_]=a.WHTno) as WHTName, "
            + " (SELECT isnull(SUM(dbo."+ Environment.GetEnvironmentVariable("Company") +"WHT Entry].Amount),0) FROM [dbo]."+ Environment.GetEnvironmentVariable("Company") +"WHT Entry] WHERE dbo."+ Environment.GetEnvironmentVariable("Company") +"WHT Entry].[WHT Registration No_]=a.WHTno AND (dbo."+ Environment.GetEnvironmentVariable("Company") +"WHT Entry].[WHT Bus_ Posting Group]='CES-WHT03' or dbo."+ Environment.GetEnvironmentVariable("Company") +"WHT Entry].[WHT Bus_ Posting Group]='BANK-WHT03') AND  dbo."+ Environment.GetEnvironmentVariable("Company") + "WHT Entry].[Posting Date]>={0} and dbo." + Environment.GetEnvironmentVariable("Company") + "WHT Entry].[Posting Date]<={1}) as SumAmount, "
            + " (SELECT isnull(SUM(dbo."+ Environment.GetEnvironmentVariable("Company") +"WHT Entry].Base),0) FROM [dbo]."+ Environment.GetEnvironmentVariable("Company") +"WHT Entry] WHERE dbo."+ Environment.GetEnvironmentVariable("Company") +"WHT Entry].[WHT Registration No_]=a.WHTno AND (dbo."+ Environment.GetEnvironmentVariable("Company") +"WHT Entry].[WHT Bus_ Posting Group]='CES-WHT03' or dbo."+ Environment.GetEnvironmentVariable("Company") +"WHT Entry].[WHT Bus_ Posting Group]='BANK-WHT03') AND  dbo."+ Environment.GetEnvironmentVariable("Company") + "WHT Entry].[Posting Date]>={0} and dbo." + Environment.GetEnvironmentVariable("Company") + "WHT Entry].[Posting Date]<={1}) as SumBase"
            + " FROM (SELECT DISTINCT "
            + " dbo."+ Environment.GetEnvironmentVariable("Company") +"WHT Entry].[WHT Registration No_] as WHTno "
            //+ " dbo."+ Environment.GetEnvironmentVariable("Company") +"WHT Entry].[WHT Name] as WHTName "
            + " FROM [dbo]."+ Environment.GetEnvironmentVariable("Company") +"WHT Entry] WHERE (dbo."+ Environment.GetEnvironmentVariable("Company") +"WHT Entry].[WHT Bus_ Posting Group]='CES-WHT03' or dbo."+ Environment.GetEnvironmentVariable("Company") +"WHT Entry].[WHT Bus_ Posting Group]='BANK-WHT03') ) as a  "
            + " )as b";


            //SqlParameter parameterStartDate = new SqlParameter("@StartDate", date1);
            //SqlParameter parameterEndDate = new SqlParameter("@EndDate", date2);

            ////ViewBag.sql = queryData;
            var v_WHT03s = _navcontext.v_WHT03s.FromSqlRaw(queryData,date1,date2).ToList();


            response = Ok(new { data = v_WHT03s });


            return response;

          
        }



        public IActionResult WHT53()
        {

            /*Check Session */
            var page = "185";
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


            ViewBag.StartDate = DateTime.Now.ToString("01-01-yyyy", new CultureInfo("en-US"));
            ViewBag.EndDate = DateTime.Now.ToString("dd-MM-yyyy", new CultureInfo("en-US"));

            return View();
        }


        public IActionResult GetDataWHT53(string Startdate, string EndDate)
        {

            /*Check Session */
            var page = "185";
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


            var date1 = Startdate.Substring(6, 4) + "-" + Startdate.Substring(3, 2) + "-" + Startdate.Substring(0, 2) + " 00:00:00";
            var date2 = EndDate.Substring(6, 4) + "-" + EndDate.Substring(3, 2) + "-" + EndDate.Substring(0, 2) + " 23:59:59";

            IActionResult response = Unauthorized();
            var queryData = " SELECT b.WHTno,b.WHTName,b.SumAmount as SumAmount ,b.SumBase as SumBase, "
            + " (CASE WHEN (b.SumBase<=1620000) THEN '<span class=''label label-primary''>Success</span>' "
            + " WHEN (b.SumBase<=1800000) THEN '<span class=''label label-warning''>Warning</span>' "
            + " WHEN (b.SumBase>1800000) THEN '<span class=''label label-danger''>Danger</span>' "
            + " END )as SumData"
            + " FROM(SELECT DISTINCT a.WHTno," +
            "(select top 1 dbo."+ Environment.GetEnvironmentVariable("Company") +"WHT Entry].[WHT Name] FROM dbo."+ Environment.GetEnvironmentVariable("Company") +"WHT Entry] WHERE dbo."+ Environment.GetEnvironmentVariable("Company") +"WHT Entry].[WHT Registration No_]=a.WHTno) as WHTName,"
            + " (SELECT isnull(SUM(dbo."+ Environment.GetEnvironmentVariable("Company") +"WHT Entry].Amount),0) FROM [dbo]."+ Environment.GetEnvironmentVariable("Company") +"WHT Entry] WHERE dbo."+ Environment.GetEnvironmentVariable("Company") +"WHT Entry].[WHT Registration No_]=a.WHTno AND (dbo."+ Environment.GetEnvironmentVariable("Company") +"WHT Entry].[WHT Bus_ Posting Group]='CES-WHT53' or dbo."+ Environment.GetEnvironmentVariable("Company") +"WHT Entry].[WHT Bus_ Posting Group]='BANK-WHT53') AND  dbo."+ Environment.GetEnvironmentVariable("Company") +"WHT Entry].[Posting Date]>={0} and dbo."+ Environment.GetEnvironmentVariable("Company") +"WHT Entry].[Posting Date]<={1}) as SumAmount, "
            + " (SELECT isnull(SUM(dbo."+ Environment.GetEnvironmentVariable("Company") +"WHT Entry].Base),0) FROM [dbo]."+ Environment.GetEnvironmentVariable("Company") +"WHT Entry] WHERE dbo."+ Environment.GetEnvironmentVariable("Company") +"WHT Entry].[WHT Registration No_]=a.WHTno AND (dbo."+ Environment.GetEnvironmentVariable("Company") +"WHT Entry].[WHT Bus_ Posting Group]='CES-WHT53' or dbo."+ Environment.GetEnvironmentVariable("Company") +"WHT Entry].[WHT Bus_ Posting Group]='BANK-WHT53') AND  dbo."+ Environment.GetEnvironmentVariable("Company") +"WHT Entry].[Posting Date]>={0} and dbo."+ Environment.GetEnvironmentVariable("Company") +"WHT Entry].[Posting Date]<={1}) as SumBase"
            + " FROM (SELECT DISTINCT "
            + " dbo."+ Environment.GetEnvironmentVariable("Company") +"WHT Entry].[WHT Registration No_] as WHTno "

            + " FROM [dbo]."+ Environment.GetEnvironmentVariable("Company") +"WHT Entry] WHERE (dbo."+ Environment.GetEnvironmentVariable("Company") +"WHT Entry].[WHT Bus_ Posting Group]='CES-WHT53' or dbo."+ Environment.GetEnvironmentVariable("Company") +"WHT Entry].[WHT Bus_ Posting Group]='BANK-WHT53')) as a  "
            + " )as b";


            //SqlParameter parameterStartDate = new SqlParameter("@StartDate", date1);
            //SqlParameter parameterEndDate = new SqlParameter("@EndDate", date2);

            ////ViewBag.sql = queryData;
            var v_WHT03s = _navcontext.v_WHT03s.FromSqlRaw(queryData, date1, date2).ToList();


            response = Ok(new { data = v_WHT03s });


            return response;


        }

    }
}