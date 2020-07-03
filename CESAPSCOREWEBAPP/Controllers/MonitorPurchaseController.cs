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
    public class MonitorPurchaseController : BaseController
    {
        private readonly NAVContext _navcontext;
        private readonly DatabaseContext _context;

        public MonitorPurchaseController(NAVContext navcontext, DatabaseContext context)
        {
            _context = context;
            _navcontext = navcontext;
        }

        public IActionResult Index()
        {
            /*Check Session */
            var page = "225";
            var typeofuser = "";
            var PermisionAction = "";
            // CheckSession
            if (string.IsNullOrEmpty(HttpContext.Session.GetString("Username")))
            {
                Alert("คุณไม่มีสิทธิ์ใช้งานหน้าดังกล่าว", NotificationType.error);
                return RedirectToAction("Index", "Home");
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

            var queryData1 = "SELECT dbo." + Environment.GetEnvironmentVariable("Company") + "Location].Code AS name,dbo." + Environment.GetEnvironmentVariable("Company") + "Location].Name as code FROM [dbo]." + Environment.GetEnvironmentVariable("Company") + "Location] order by dbo." + Environment.GetEnvironmentVariable("Company") + "Location].Code asc ";
            var sourceAutoCompletes = _navcontext.sourceAutoCompletes.FromSqlRaw(queryData1).ToList();
            ViewData["SourceAutoCompletes"] = sourceAutoCompletes;

            ViewBag.StartDate = DateTime.Now.ToString("01/MM/yyyy");
            ViewBag.EndDate = DateTime.Now.ToString("dd/MM/yyyy");

            return View();
        }


        public IActionResult Getdata(string site,string date1,string date2)
        {
            /*Check Session */
            var page = "225";
            var typeofuser = "";
            var PermisionAction = "";
            // CheckSession
            if (string.IsNullOrEmpty(HttpContext.Session.GetString("Username")))
            {
                Alert("คุณไม่มีสิทธิ์ใช้งานหน้าดังกล่าว", NotificationType.error);
                return RedirectToAction("Index", "Home");
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

            var queryData1 = "SELECT dbo." + Environment.GetEnvironmentVariable("Company") + "Location].Code AS name,dbo." + Environment.GetEnvironmentVariable("Company") + "Location].Name as code FROM [dbo]." + Environment.GetEnvironmentVariable("Company") + "Location] order by dbo." + Environment.GetEnvironmentVariable("Company") + "Location].Code asc ";
            var sourceAutoCompletes = _navcontext.sourceAutoCompletes.FromSqlRaw(queryData1).ToList();
            ViewData["SourceAutoCompletes"] = sourceAutoCompletes;

            IActionResult response = Unauthorized();
            var StartDate = date1.Substring(6, 4) + "-" + date1.Substring(3, 2) + "-" + date1.Substring(0, 2) + " 00:00:00";
            var EndDate = date2.Substring(6, 4) + "-" + date2.Substring(3, 2) + "-" + date2.Substring(0, 2) + " 23:59:59";

            List<MonitorPurchase> PurchaseDATA;
            var querydata = "";

            if (site == null)
            {
                querydata = "SELECT ROW_NUMBER() OVER (ORDER BY dbo." + Environment.GetEnvironmentVariable("Company") + "Purchase Line].[Order Date]) as ID," +
                          " convert(varchar, dbo." + Environment.GetEnvironmentVariable("Company") + "Purchase Line].[Order Date],23) as OrderDate," +
                          " dbo." + Environment.GetEnvironmentVariable("Company") + "Purchase Header].[No_] AS POName," +
                          " dbo." + Environment.GetEnvironmentVariable("Company") + "Purchase Header].[Location Code] AS LocationHead," +
                          " dbo." + Environment.GetEnvironmentVariable("Company") + "Purchase Header].[Shortcut Dimension 1 Code] AS Job_GL_Code," +
                          " dbo." + Environment.GetEnvironmentVariable("Company") + "Purchase Line].[Job No_] as JobNo," +
                          " dbo." + Environment.GetEnvironmentVariable("Company") + "Purchase Line].[Job Task No_] as JobTaskNo," +
                          " dbo." + Environment.GetEnvironmentVariable("Company") + "Purchase Line].[Shortcut Dimension 1 Code] AS Job_GL_Code_Line," +
                          " dbo." + Environment.GetEnvironmentVariable("Company") + "Purchase Line].[Location Code] AS LocationLine," +
                          " dbo." + Environment.GetEnvironmentVariable("Company") + "Purchase Line].[No_] as No," +
                          " dbo." + Environment.GetEnvironmentVariable("Company") + "Purchase Line].[Description] as Description," +
                          " dbo." + Environment.GetEnvironmentVariable("Company") + "Purchase Line].[Description 2] as Description2," +
                          " dbo." + Environment.GetEnvironmentVariable("Company") + "Purchase Line].[Quantity] as Quantity," +
                          " dbo." + Environment.GetEnvironmentVariable("Company") + "Purchase Line].[Outstanding Quantity] as OutsatandingQty," +
                          " dbo." + Environment.GetEnvironmentVariable("Company") + "Purchase Line].[Shortcut Dimension 2 Code] AS CostCode" +
                          " FROM " +
                          " dbo." + Environment.GetEnvironmentVariable("Company") + "Purchase Header]" +
                          " INNER JOIN dbo." + Environment.GetEnvironmentVariable("Company") + "Purchase Line] ON dbo." + Environment.GetEnvironmentVariable("Company") + "Purchase Line].[Document No_] = dbo." + Environment.GetEnvironmentVariable("Company") + "Purchase Header].[No_]" +
                          " WHERE" +
                          " dbo." + Environment.GetEnvironmentVariable("Company") + "Purchase Line].Type = 2  AND dbo." + Environment.GetEnvironmentVariable("Company") + "Purchase Header].[Job Document Type] = 0 AND dbo." + Environment.GetEnvironmentVariable("Company") + "Purchase Line].[Order Date]>={0} and dbo." + Environment.GetEnvironmentVariable("Company") + "Purchase Line].[Order Date]<={1}";

                //SqlParameter parameterStartdate = new SqlParameter("@startdate", StartDate);
                //SqlParameter parameterEndDate = new SqlParameter("@enddate", EndDate);

                PurchaseDATA = _navcontext.monitorPurchases.FromSqlRaw(querydata, StartDate, EndDate).ToList();
            }
            else
            {
                querydata = "SELECT ROW_NUMBER() OVER (ORDER BY dbo." + Environment.GetEnvironmentVariable("Company") + "Purchase Line].[Order Date]) as ID," +
                        " convert(varchar, dbo." + Environment.GetEnvironmentVariable("Company") + "Purchase Line].[Order Date],23) as OrderDate," +
                        " dbo." + Environment.GetEnvironmentVariable("Company") + "Purchase Header].[No_] AS POName," +
                        " dbo." + Environment.GetEnvironmentVariable("Company") + "Purchase Header].[Location Code] AS LocationHead," +
                        " dbo." + Environment.GetEnvironmentVariable("Company") + "Purchase Header].[Shortcut Dimension 1 Code] AS Job_GL_Code," +
                        " dbo." + Environment.GetEnvironmentVariable("Company") + "Purchase Line].[Job No_] as JobNo," +
                        " dbo." + Environment.GetEnvironmentVariable("Company") + "Purchase Line].[Job Task No_] as JobTaskNo," +
                        " dbo." + Environment.GetEnvironmentVariable("Company") + "Purchase Line].[Shortcut Dimension 1 Code] AS Job_GL_Code_Line," +
                        " dbo." + Environment.GetEnvironmentVariable("Company") + "Purchase Line].[Location Code] AS LocationLine," +
                        " dbo." + Environment.GetEnvironmentVariable("Company") + "Purchase Line].[No_] as No," +
                        " dbo." + Environment.GetEnvironmentVariable("Company") + "Purchase Line].[Description] as Description," +
                        " dbo." + Environment.GetEnvironmentVariable("Company") + "Purchase Line].[Description 2] as Description2," +
                        " dbo." + Environment.GetEnvironmentVariable("Company") + "Purchase Line].[Quantity] as Quantity," +
                        " dbo." + Environment.GetEnvironmentVariable("Company") + "Purchase Line].[Outstanding Quantity] as OutsatandingQty," +
                        " dbo." + Environment.GetEnvironmentVariable("Company") + "Purchase Line].[Shortcut Dimension 2 Code] AS CostCode" +
                        " FROM " +
                        " dbo." + Environment.GetEnvironmentVariable("Company") + "Purchase Header]" +
                        " INNER JOIN dbo." + Environment.GetEnvironmentVariable("Company") + "Purchase Line] ON dbo." + Environment.GetEnvironmentVariable("Company") + "Purchase Line].[Document No_] = dbo." + Environment.GetEnvironmentVariable("Company") + "Purchase Header].[No_]" +
                        " WHERE" +
                        " dbo." + Environment.GetEnvironmentVariable("Company") + "Purchase Line].Type = 2  AND dbo." + Environment.GetEnvironmentVariable("Company") + "Purchase Header].[Job Document Type] = 0 and dbo." + Environment.GetEnvironmentVariable("Company") + "Purchase Header].[Location Code] = {0} AND dbo." + Environment.GetEnvironmentVariable("Company") + "Purchase Line].[Order Date]>={1} and dbo." + Environment.GetEnvironmentVariable("Company") + "Purchase Line].[Order Date]<={2}";

                        //SqlParameter parameterSite = new SqlParameter("@site", site);
                        //SqlParameter parameterStartdate = new SqlParameter("@startdate", StartDate);
                        //SqlParameter parameterEndDate = new SqlParameter("@enddate", EndDate);

                PurchaseDATA = _navcontext.monitorPurchases.FromSqlRaw(querydata,site, StartDate, EndDate).ToList();
            }
        

            response = Ok(new { data = PurchaseDATA });


            return response;
        }
    }
}