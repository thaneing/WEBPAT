using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CESAPSCOREWEBAPP.Helpers;
using CESAPSCOREWEBAPP.Models;
using DevExtreme.AspNet.Data;
using DevExtreme.AspNet.Mvc;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using static CESAPSCOREWEBAPP.Models.Enums;

namespace CESAPSCOREWEBAPP.Controllers
{
    [Authorize]
    public class JobOrderReportsController : BaseController
    {
        private readonly NAVContext _navcontext;
        private readonly DatabaseContext _context;


        public JobOrderReportsController(NAVContext navcontext, DatabaseContext context)
        {
            _context = context;
            _navcontext = navcontext;
        }
        public IActionResult Index()
        {


            /*Check Session */
            var page = "215";
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




        public object GetDataFilter(DataSourceLoadOptions loadOptions)
        {


            var queryData = "SELECT ROW_NUMBER() OVER (ORDER BY dbo." + Environment.GetEnvironmentVariable("Company") + "Purchase Header].[Order Date]) as ID,"
                          + " dbo."+ Environment.GetEnvironmentVariable("Company") +"Purchase Header].No_ AS OrderNo," +
                          " dbo."+ Environment.GetEnvironmentVariable("Company") +"Purchase Header].[Order Date] AS OrderDate," +
                          "dbo."+ Environment.GetEnvironmentVariable("Company") +"Purchase Header].[Posting Description] AS JobDesc," +
                          "dbo."+ Environment.GetEnvironmentVariable("Company") +"Purchase Header].[Pay-to Vendor No_] AS VendorNo," +
                          "dbo."+ Environment.GetEnvironmentVariable("Company") +"Purchase Header].[Pay-to Name] AS VendorName," +
                          "dbo."+ Environment.GetEnvironmentVariable("Company") +"Purchase Header].[Location Code] AS Location," +
                          "dbo."+ Environment.GetEnvironmentVariable("Company") +"Purchase Line].No_ AS ItemNo," +
                          "dbo."+ Environment.GetEnvironmentVariable("Company") +"Purchase Line].Description+dbo."+ Environment.GetEnvironmentVariable("Company") +"Purchase Line].[Description 2] AS Des," +
                          "'' AS Des2," +
                          "dbo."+ Environment.GetEnvironmentVariable("Company") +"Purchase Line].[Unit of Measure] AS UnitOfMeasure," +
                          "dbo."+ Environment.GetEnvironmentVariable("Company") +"Purchase Line].Quantity AS Qty," +
                          "dbo."+ Environment.GetEnvironmentVariable("Company") +"Purchase Line].Amount AS Amount," +
                          "CASE WHEN dbo."+ Environment.GetEnvironmentVariable("Company") +"Purchase Line].Quantity=0 THEN 0 " +
                          "		 ELSE dbo."+ Environment.GetEnvironmentVariable("Company") +"Purchase Line].Amount/dbo."+ Environment.GetEnvironmentVariable("Company") +"Purchase Line].Quantity END AS UnitCost, " +
                          "dbo."+ Environment.GetEnvironmentVariable("Company") +"Purchase Line].[Quantity Received] as QtyReceived, " +
                          "(CASE WHEN dbo."+ Environment.GetEnvironmentVariable("Company") +"Purchase Line].Quantity=0 THEN 0 " +
                          "		 ELSE dbo."+ Environment.GetEnvironmentVariable("Company") +"Purchase Line].Amount/dbo."+ Environment.GetEnvironmentVariable("Company") +"Purchase Line].Quantity END)*dbo."+ Environment.GetEnvironmentVariable("Company") +"Purchase Line].[Quantity Received] as TotalReceived " +
                          "FROM " +
                          "dbo."+ Environment.GetEnvironmentVariable("Company") +"Purchase Header] " +
                          "INNER JOIN dbo."+ Environment.GetEnvironmentVariable("Company") +"Purchase Line] ON dbo."+ Environment.GetEnvironmentVariable("Company") +"Purchase Header].No_ = dbo."+ Environment.GetEnvironmentVariable("Company") +"Purchase Line].[Document No_] " +
                          "WHERE " +
                          "dbo."+ Environment.GetEnvironmentVariable("Company") +"Purchase Header].[Job Document Type] = 1 ";




            //ViewBag.sql = queryData;
            var JobOrderReports = _navcontext.JobOrderReports.FromSqlRaw(queryData).ToList();

            return DataSourceLoader.Load(JobOrderReports, loadOptions);

        }

        public IActionResult JOByVendor()
        {

            /*Check Session */
            var page = "216";
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



    }
}