using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;
using CESAPSCOREWEBAPP.Helpers;
using CESAPSCOREWEBAPP.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using static CESAPSCOREWEBAPP.Models.Enums;

namespace CESAPSCOREWEBAPP.Controllers
{
    public class CheckJOErrorController : BaseController
    {
        private readonly NAVContext _navcontext;
        private readonly DatabaseContext _context;


        public CheckJOErrorController(NAVContext navcontext, DatabaseContext context)
        {
            _context = context;
            _navcontext = navcontext;
        }
        public IActionResult Index()
        {
            /*Check Session */
            var page = "183";
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

            return View();
        }

        public IActionResult GetData()
        {
            /*Check Session */
            var page = "183";
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



            IActionResult response = Unauthorized();
            var queryData = "SELECT ROW_NUMBER() OVER (ORDER BY dbo." + Environment.GetEnvironmentVariable("Company") + "Purch_ Rcpt_ Line].[Posting Date]) as ID," +
                " CONVERT(VARCHAR,dbo."+ Environment.GetEnvironmentVariable("Company") +"Purch_ Rcpt_ Line].[Posting Date],23) as PostingDate," +
                " dbo."+ Environment.GetEnvironmentVariable("Company") +"Purch_ Rcpt_ Line].[Document No_] AS GR, " +
                " dbo."+ Environment.GetEnvironmentVariable("Company") +"Purch_ Rcpt_ Line].[Order No_] as OrderDoc," +
                " dbo."+ Environment.GetEnvironmentVariable("Company") +"Purch_ Rcpt_ Line].No_ as ItemNo," +
                " dbo."+ Environment.GetEnvironmentVariable("Company") +"Purch_ Rcpt_ Line].Description as Des1," +
                " dbo."+ Environment.GetEnvironmentVariable("Company") +"Purch_ Rcpt_ Line].[Description 2] as Des2, " +
                " dbo."+ Environment.GetEnvironmentVariable("Company") +"Purch_ Rcpt_ Line].Quantity," +
                " dbo."+ Environment.GetEnvironmentVariable("Company") +"Purch_ Rcpt_ Line].[Unit Cost (LCY)] as UnitCost," +
                " dbo."+ Environment.GetEnvironmentVariable("Company") +"Purch_ Rcpt_ Line].[Job No_] as JobNo," +
                " dbo."+ Environment.GetEnvironmentVariable("Company") +"Purch_ Rcpt_ Line].[Receive Amount Line] as TotalReceive," +
                " dbo."+ Environment.GetEnvironmentVariable("Company") +"Purch_ Rcpt_ Line].[Retention _] as Retention," +
                " CONVERT(VARCHAR,(CONVERT(DECIMAL(15,2),dbo."+ Environment.GetEnvironmentVariable("Company") +"Purch_ Rcpt_ Line].[Retention Amount]))) as TotalRetention," +
                " CONVERT(VARCHAR,(CONVERT(DECIMAL(15,2),(dbo."+ Environment.GetEnvironmentVariable("Company") +"Purch_ Rcpt_ Line].Quantity*dbo."+ Environment.GetEnvironmentVariable("Company") +"Purch_ Rcpt_ Line].[Direct Unit Cost])*(dbo."+ Environment.GetEnvironmentVariable("Company") +"Purch_ Rcpt_ Line].[Retention _]/100)))) as CalRetention," +
                " dbo."+ Environment.GetEnvironmentVariable("Company") +"Purch_ Rcpt_ Header].[User ID] as UserId" +
                " FROM  " +
                " dbo."+ Environment.GetEnvironmentVariable("Company") +"Purch_ Rcpt_ Header] " +
                " INNER JOIN dbo."+ Environment.GetEnvironmentVariable("Company") +"Purch_ Rcpt_ Line] ON dbo."+ Environment.GetEnvironmentVariable("Company") +"Purch_ Rcpt_ Header].No_ = dbo."+ Environment.GetEnvironmentVariable("Company") +"Purch_ Rcpt_ Line].[Document No_] " +
                " WHERE dbo."+ Environment.GetEnvironmentVariable("Company") +"Purch_ Rcpt_ Line].[Document No_] IN( " +
                "       SELECT " +
                "		dbo."+ Environment.GetEnvironmentVariable("Company") +"Purch_ Rcpt_ Line].[Document No_] as GR" +
                " 		FROM" +
                "		dbo."+ Environment.GetEnvironmentVariable("Company") +"Purch_ Rcpt_ Header]" +
                " 		INNER JOIN dbo."+ Environment.GetEnvironmentVariable("Company") +"Purch_ Rcpt_ Line] ON dbo."+ Environment.GetEnvironmentVariable("Company") +"Purch_ Rcpt_ Header].No_ = dbo."+ Environment.GetEnvironmentVariable("Company") +"Purch_ Rcpt_ Line].[Document No_]" +
                " 		WHERE Type=2" +
                " 		GROUP BY " +
                "		dbo."+ Environment.GetEnvironmentVariable("Company") +"Purch_ Rcpt_ Line].[Document No_]" +
                "		HAVING sum(dbo."+ Environment.GetEnvironmentVariable("Company") +"Purch_ Rcpt_ Line].Quantity)>0" +
                ") and Quantity>0 and ( " +
                "CONVERT(VARCHAR,(CONVERT(DECIMAL(15,2),(dbo."+ Environment.GetEnvironmentVariable("Company") +"Purch_ Rcpt_ Line].Quantity*dbo."+ Environment.GetEnvironmentVariable("Company") +"Purch_ Rcpt_ Line].[Unit Cost (LCY)])*(dbo."+ Environment.GetEnvironmentVariable("Company") +"Purch_ Rcpt_ Line].[Retention _]/100))))<> " +
                "CONVERT(VARCHAR,(CONVERT(DECIMAL(15,2),dbo."+ Environment.GetEnvironmentVariable("Company") +"Purch_ Rcpt_ Line].[Retention Amount])))) and dbo."+ Environment.GetEnvironmentVariable("Company") +"Purch_ Rcpt_ Line].[Posting Date]>='2019-01-01 00:00:00' ";
        

            //ViewBag.sql = queryData;

            var checkJOErrors = _navcontext.CheckJOErrors.FromSqlRaw(queryData).ToList();
            var countdata = checkJOErrors.Count;


            response = Ok(new { data = checkJOErrors ,countdata= countdata });


            return response;
        }


        public IActionResult GetCoutJOError()
        {
          



            IActionResult response = Unauthorized();
            var queryData = "SELECT ROW_NUMBER() OVER (ORDER BY dbo." + Environment.GetEnvironmentVariable("Company") + "Purch_ Rcpt_ Line].[Posting Date]) as ID," +
                " CONVERT(VARCHAR,dbo."+ Environment.GetEnvironmentVariable("Company") +"Purch_ Rcpt_ Line].[Posting Date],23) as PostingDate," +
                " dbo."+ Environment.GetEnvironmentVariable("Company") +"Purch_ Rcpt_ Line].[Document No_] AS GR, " +
                " dbo."+ Environment.GetEnvironmentVariable("Company") +"Purch_ Rcpt_ Line].[Order No_] as OrderDoc," +
                " dbo."+ Environment.GetEnvironmentVariable("Company") +"Purch_ Rcpt_ Line].No_ as ItemNo," +
                " dbo."+ Environment.GetEnvironmentVariable("Company") +"Purch_ Rcpt_ Line].Description as Des1," +
                " dbo."+ Environment.GetEnvironmentVariable("Company") +"Purch_ Rcpt_ Line].[Description 2] as Des2, " +
                " dbo."+ Environment.GetEnvironmentVariable("Company") +"Purch_ Rcpt_ Line].Quantity," +
                " dbo."+ Environment.GetEnvironmentVariable("Company") +"Purch_ Rcpt_ Line].[Unit Cost (LCY)] as UnitCost," +
                " dbo."+ Environment.GetEnvironmentVariable("Company") +"Purch_ Rcpt_ Line].[Job No_] as JobNo," +
                " dbo."+ Environment.GetEnvironmentVariable("Company") +"Purch_ Rcpt_ Line].[Receive Amount Line] as TotalReceive," +
                " dbo."+ Environment.GetEnvironmentVariable("Company") +"Purch_ Rcpt_ Line].[Retention _] as Retention," +
                " CONVERT(VARCHAR,(CONVERT(DECIMAL(15,2),dbo."+ Environment.GetEnvironmentVariable("Company") +"Purch_ Rcpt_ Line].[Retention Amount]))) as TotalRetention," +
                " CONVERT(VARCHAR,(CONVERT(DECIMAL(15,2),(dbo."+ Environment.GetEnvironmentVariable("Company") +"Purch_ Rcpt_ Line].Quantity*dbo."+ Environment.GetEnvironmentVariable("Company") +"Purch_ Rcpt_ Line].[Direct Unit Cost])*(dbo."+ Environment.GetEnvironmentVariable("Company") +"Purch_ Rcpt_ Line].[Retention _]/100)))) as CalRetention," +
                " dbo."+ Environment.GetEnvironmentVariable("Company") +"Purch_ Rcpt_ Header].[User ID] as UserId" +
                " FROM  " +
                " dbo."+ Environment.GetEnvironmentVariable("Company") +"Purch_ Rcpt_ Header] " +
                " INNER JOIN dbo."+ Environment.GetEnvironmentVariable("Company") +"Purch_ Rcpt_ Line] ON dbo."+ Environment.GetEnvironmentVariable("Company") +"Purch_ Rcpt_ Header].No_ = dbo."+ Environment.GetEnvironmentVariable("Company") +"Purch_ Rcpt_ Line].[Document No_] " +
                " WHERE dbo."+ Environment.GetEnvironmentVariable("Company") +"Purch_ Rcpt_ Line].[Document No_] IN( " +
                "       SELECT " +
                "		dbo."+ Environment.GetEnvironmentVariable("Company") +"Purch_ Rcpt_ Line].[Document No_] as GR" +
                " 		FROM" +
                "		dbo."+ Environment.GetEnvironmentVariable("Company") +"Purch_ Rcpt_ Header]" +
                " 		INNER JOIN dbo."+ Environment.GetEnvironmentVariable("Company") +"Purch_ Rcpt_ Line] ON dbo."+ Environment.GetEnvironmentVariable("Company") +"Purch_ Rcpt_ Header].No_ = dbo."+ Environment.GetEnvironmentVariable("Company") +"Purch_ Rcpt_ Line].[Document No_]" +
                " 		WHERE Type=2" +
                " 		GROUP BY " +
                "		dbo."+ Environment.GetEnvironmentVariable("Company") +"Purch_ Rcpt_ Line].[Document No_]" +
                "		HAVING sum(dbo."+ Environment.GetEnvironmentVariable("Company") +"Purch_ Rcpt_ Line].Quantity)>0" +
                ") and Quantity>0 and ( " +
                "CONVERT(VARCHAR,(CONVERT(DECIMAL(15,2),(dbo."+ Environment.GetEnvironmentVariable("Company") +"Purch_ Rcpt_ Line].Quantity*dbo."+ Environment.GetEnvironmentVariable("Company") +"Purch_ Rcpt_ Line].[Unit Cost (LCY)])*(dbo."+ Environment.GetEnvironmentVariable("Company") +"Purch_ Rcpt_ Line].[Retention _]/100))))<> " +
                "CONVERT(VARCHAR,(CONVERT(DECIMAL(15,2),dbo."+ Environment.GetEnvironmentVariable("Company") +"Purch_ Rcpt_ Line].[Retention Amount])))) and dbo."+ Environment.GetEnvironmentVariable("Company") +"Purch_ Rcpt_ Line].[Posting Date]>='2019-01-01 00:00:00' ";


            //ViewBag.sql = queryData;

            var checkJOErrors = _navcontext.CheckJOErrors.FromSqlRaw(queryData).ToList();
            var countdata = checkJOErrors.Count;


            response = Ok(new { countdata = countdata });


            return response;
        }



        public IActionResult GetJOErrorLineAlert()
        {




            IActionResult response = Unauthorized();
            var queryData = "SELECT ROW_NUMBER() OVER (ORDER BY dbo." + Environment.GetEnvironmentVariable("Company") + "Purch_ Rcpt_ Line].[Posting Date]) as ID," +
                " CONVERT(VARCHAR,dbo."+ Environment.GetEnvironmentVariable("Company") +"Purch_ Rcpt_ Line].[Posting Date],23) as PostingDate," +
                " dbo."+ Environment.GetEnvironmentVariable("Company") +"Purch_ Rcpt_ Line].[Document No_] AS GR, " +
                " dbo."+ Environment.GetEnvironmentVariable("Company") +"Purch_ Rcpt_ Line].[Order No_] as OrderDoc," +
                " dbo."+ Environment.GetEnvironmentVariable("Company") +"Purch_ Rcpt_ Line].No_ as ItemNo," +
                " dbo."+ Environment.GetEnvironmentVariable("Company") +"Purch_ Rcpt_ Line].Description as Des1," +
                " dbo."+ Environment.GetEnvironmentVariable("Company") +"Purch_ Rcpt_ Line].[Description 2] as Des2, " +
                " dbo."+ Environment.GetEnvironmentVariable("Company") +"Purch_ Rcpt_ Line].Quantity," +
                " dbo."+ Environment.GetEnvironmentVariable("Company") +"Purch_ Rcpt_ Line].[Unit Cost (LCY)] as UnitCost," +
                " dbo."+ Environment.GetEnvironmentVariable("Company") +"Purch_ Rcpt_ Line].[Job No_] as JobNo," +
                " dbo."+ Environment.GetEnvironmentVariable("Company") +"Purch_ Rcpt_ Line].[Receive Amount Line] as TotalReceive," +
                " dbo."+ Environment.GetEnvironmentVariable("Company") +"Purch_ Rcpt_ Line].[Retention _] as Retention," +
                " CONVERT(VARCHAR,(CONVERT(DECIMAL(15,2),dbo."+ Environment.GetEnvironmentVariable("Company") +"Purch_ Rcpt_ Line].[Retention Amount]))) as TotalRetention," +
                " CONVERT(VARCHAR,(CONVERT(DECIMAL(15,2),(dbo."+ Environment.GetEnvironmentVariable("Company") +"Purch_ Rcpt_ Line].Quantity*dbo."+ Environment.GetEnvironmentVariable("Company") +"Purch_ Rcpt_ Line].[Direct Unit Cost])*(dbo."+ Environment.GetEnvironmentVariable("Company") +"Purch_ Rcpt_ Line].[Retention _]/100)))) as CalRetention," +
                " dbo."+ Environment.GetEnvironmentVariable("Company") +"Purch_ Rcpt_ Header].[User ID] as UserId" +
                " FROM  " +
                " dbo."+ Environment.GetEnvironmentVariable("Company") +"Purch_ Rcpt_ Header] " +
                " INNER JOIN dbo."+ Environment.GetEnvironmentVariable("Company") +"Purch_ Rcpt_ Line] ON dbo."+ Environment.GetEnvironmentVariable("Company") +"Purch_ Rcpt_ Header].No_ = dbo."+ Environment.GetEnvironmentVariable("Company") +"Purch_ Rcpt_ Line].[Document No_] " +
                " WHERE dbo."+ Environment.GetEnvironmentVariable("Company") +"Purch_ Rcpt_ Line].[Document No_] IN( " +
                "       SELECT " +
                "		dbo."+ Environment.GetEnvironmentVariable("Company") +"Purch_ Rcpt_ Line].[Document No_] as GR" +
                " 		FROM" +
                "		dbo."+ Environment.GetEnvironmentVariable("Company") +"Purch_ Rcpt_ Header]" +
                " 		INNER JOIN dbo."+ Environment.GetEnvironmentVariable("Company") +"Purch_ Rcpt_ Line] ON dbo."+ Environment.GetEnvironmentVariable("Company") +"Purch_ Rcpt_ Header].No_ = dbo."+ Environment.GetEnvironmentVariable("Company") +"Purch_ Rcpt_ Line].[Document No_]" +
                " 		WHERE Type=2" +
                " 		GROUP BY " +
                "		dbo."+ Environment.GetEnvironmentVariable("Company") +"Purch_ Rcpt_ Line].[Document No_]" +
                "		HAVING sum(dbo."+ Environment.GetEnvironmentVariable("Company") +"Purch_ Rcpt_ Line].Quantity)>0" +
                ") and Quantity>0 and ( " +
                "CONVERT(VARCHAR,(CONVERT(DECIMAL(15,2),(dbo."+ Environment.GetEnvironmentVariable("Company") +"Purch_ Rcpt_ Line].Quantity*dbo."+ Environment.GetEnvironmentVariable("Company") +"Purch_ Rcpt_ Line].[Unit Cost (LCY)])*(dbo."+ Environment.GetEnvironmentVariable("Company") +"Purch_ Rcpt_ Line].[Retention _]/100))))<> " +
                "CONVERT(VARCHAR,(CONVERT(DECIMAL(15,2),dbo."+ Environment.GetEnvironmentVariable("Company") +"Purch_ Rcpt_ Line].[Retention Amount])))) and dbo."+ Environment.GetEnvironmentVariable("Company") +"Purch_ Rcpt_ Line].[Posting Date]>='2019-01-01 00:00:00' ";


            //ViewBag.sql = queryData;

            var checkJOErrors = _navcontext.CheckJOErrors.FromSqlRaw(queryData).ToList();
            //var countdata = checkJOErrors.Count;


            response = Ok(checkJOErrors);


            return response;
        }

        public async Task<IActionResult> GenDataToken()
        {
            /*Check Session */
            IActionResult response = Unauthorized();
            var LineApi = await _context.LineAPIs.Where(p => p.Id == 2).ToListAsync();
            response = Ok(LineApi);
            return response;
        }



    }
}