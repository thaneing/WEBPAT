using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using CESAPSCOREWEBAPP.Models;
using System.Data.SqlClient;
using System.Globalization;
using Microsoft.AspNetCore.Http;
using System.IO;
using static CESAPSCOREWEBAPP.Models.Enums;
using CESAPSCOREWEBAPP.Helpers;
using MimeKit;
using Microsoft.Extensions.Configuration;
using System.Text;
using Rotativa.AspNetCore;
using DevExpress.XtraReports.UI;
using Microsoft.AspNetCore.Authorization;

namespace CESAPSCOREWEBAPP.Controllers
{
    [Authorize]
    public class OutstandingsController : BaseController
    {

        private readonly NAVContext _navcontext;
        private readonly DatabaseContext _context;


        public OutstandingsController(NAVContext navcontext, DatabaseContext context)
        {
            _context = context;
            _navcontext = navcontext;
        }
        public IActionResult Index()
        {

            /*Check Session */
            var page = "274";
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



            var query = "SELECT DISTINCT dbo." + Environment.GetEnvironmentVariable("Company") + "Item Ledger Entry].[Location Code] AS JobNo,dbo." + Environment.GetEnvironmentVariable("Company") + "Item Ledger Entry].[Location Code]AS LocationCode FROM dbo." + Environment.GetEnvironmentVariable("Company") + "Item Ledger Entry] ORDER BY dbo." + Environment.GetEnvironmentVariable("Company") + "Item Ledger Entry].[Location Code]";
            List<V_Job> jobNo = _navcontext.v_Job.FromSqlRaw(query).ToList();

            //var JobNo = _context.UserJobs
            //.Where(s => s.UserId == HttpContext.Session.GetInt32("Userid"))
            //.ToList();

            ViewData["JobNo"] = jobNo;

            ViewBag.StartDate = DateTime.Now.ToString("01-01-yyyy", new CultureInfo("en-US"));
            ViewBag.EndDate = DateTime.Now.ToString("dd-MM-yyyy", new CultureInfo("en-US"));
           
            return View();
        }

        public IActionResult GetData(string Startdate, string EndDate ,string type,string Job)
        {

            /*Check Session */
            var page = "274";
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

            var date11 = Startdate.Substring(6, 4) + "-" + Startdate.Substring(3, 2) + "-" + Startdate.Substring(0, 2);
            var date22 = EndDate.Substring(6, 4) + "-" + EndDate.Substring(3, 2) + "-" + EndDate.Substring(0, 2);

            var queryData = "";
            var jobFilter = "";
            List<Outstanding> Outstandings = new List<Outstanding>();
            IActionResult response = Unauthorized();
            var filter = "";
            var Site = "";
          
            if (Job ==null)
            {
                Site = "";
            }
            else
            {
                jobFilter=Job.Replace("\'", "");
                Site = " AND dbo." + Environment.GetEnvironmentVariable("Company") + "Purchase Line].[Location Code] IN (" + Job+")";
            }
            
            if (type =="3")
            {

                filter = "'PostingDate :" + date11 + ".." + date22 + ",Type: All,User : "+ HttpContext.Session.GetString("Username") +" Job : "+ jobFilter + "' as Filter ";
                queryData = "SELECT ROW_NUMBER() OVER (ORDER BY dbo." + Environment.GetEnvironmentVariable("Company") + "Purchase Header].No_) as ID, " +
                     "dbo." + Environment.GetEnvironmentVariable("Company") + "Purchase Header].No_ as OrderNo, " +
                     "dbo." + Environment.GetEnvironmentVariable("Company") + "Purchase Header].[Order Date] as OrderDate, " +
                      "dbo." + Environment.GetEnvironmentVariable("Company") + "Purchase Header].[Buy-from Vendor No_] as VendorNo, " +
                     "dbo." + Environment.GetEnvironmentVariable("Company") + "Purchase Header].[Pay-to Name] as VendorName, " +
                     "dbo." + Environment.GetEnvironmentVariable("Company") + "Purchase Line].No_ as ItemInLine, " +
                     "dbo." + Environment.GetEnvironmentVariable("Company") + "Purchase Line].Description as Description, " +
                     "dbo." + Environment.GetEnvironmentVariable("Company") + "Purchase Line].[Direct Unit Cost] as DirectUnitCost, " +
                     "dbo." + Environment.GetEnvironmentVariable("Company") + "Purchase Line].[Outstanding Quantity] as OutstandingQuantity, " +
                     "dbo." + Environment.GetEnvironmentVariable("Company") + "Purchase Line].[Unit of Measure] as UOM, " +
                     "dbo." + Environment.GetEnvironmentVariable("Company") + "Purchase Line].[Location Code] as JobNo, " +
                     "dbo." + Environment.GetEnvironmentVariable("Company") + "Purchase Line].Quantity, " +
                     "[Outstanding Quantity]*dbo." + Environment.GetEnvironmentVariable("Company") + "Purchase Line].[Direct Unit Cost] as Total," +
                     "case when dbo." + Environment.GetEnvironmentVariable("Company") + "Purchase Header].[Job Document Type]=0 THEN 'Purchase Order' ELSE 'Job Order' END as TypeOrder," + filter +

                     " FROM " +
                     "dbo." + Environment.GetEnvironmentVariable("Company") + "Purchase Header] " +
                     "INNER JOIN dbo." + Environment.GetEnvironmentVariable("Company") + "Purchase Line] ON dbo." + Environment.GetEnvironmentVariable("Company") + "Purchase Header].No_ = dbo." + Environment.GetEnvironmentVariable("Company") + "Purchase Line].[Document No_] " +
                     "WHERE " +
                        "dbo." + Environment.GetEnvironmentVariable("Company") + "Purchase Header].Status = 1 AND   dbo." + Environment.GetEnvironmentVariable("Company") + "Purchase Header].[Document Type] =1 and " +
                     "dbo." + Environment.GetEnvironmentVariable("Company") + "Purchase Line].[Outstanding Quantity] >0 " +
                     "and dbo." + Environment.GetEnvironmentVariable("Company") + "Purchase Header].[Order Date]>={0} and dbo." + Environment.GetEnvironmentVariable("Company") + "Purchase Header].[Order Date]<={1}  " + Site;
                Outstandings = _navcontext.Outstandings.FromSqlRaw(queryData, date1, date2).ToList();
            }
            else if(type == "1")
            {
                filter = "'PostingDate :" + date11 + ".." + date22 + ",Type: Job Order,User : " + HttpContext.Session.GetString("Username") + " Job : " + jobFilter + "' as Filter ";
                queryData = "SELECT ROW_NUMBER() OVER (ORDER BY dbo." + Environment.GetEnvironmentVariable("Company") + "Purchase Header].No_) as ID, " +
                    "dbo." + Environment.GetEnvironmentVariable("Company") + "Purchase Header].No_ as OrderNo, " +
                    "dbo." + Environment.GetEnvironmentVariable("Company") + "Purchase Header].[Order Date] as OrderDate, " +
                     "dbo." + Environment.GetEnvironmentVariable("Company") + "Purchase Header].[Buy-from Vendor No_] as VendorNo, " +
                    "dbo." + Environment.GetEnvironmentVariable("Company") + "Purchase Header].[Pay-to Name] as VendorName, " +
                    "dbo." + Environment.GetEnvironmentVariable("Company") + "Purchase Line].No_ as ItemInLine, " +
                    "dbo." + Environment.GetEnvironmentVariable("Company") + "Purchase Line].Description as Description, " +
                    "dbo." + Environment.GetEnvironmentVariable("Company") + "Purchase Line].[Direct Unit Cost] as DirectUnitCost, " +
                    "dbo." + Environment.GetEnvironmentVariable("Company") + "Purchase Line].[Outstanding Quantity] as OutstandingQuantity, " +
                    "dbo." + Environment.GetEnvironmentVariable("Company") + "Purchase Line].[Unit of Measure] as UOM, " +
                    "dbo." + Environment.GetEnvironmentVariable("Company") + "Purchase Line].[Location Code] as JobNo, " +
                    "dbo." + Environment.GetEnvironmentVariable("Company") + "Purchase Line].Quantity, " +
                    "[Outstanding Quantity]*dbo." + Environment.GetEnvironmentVariable("Company") + "Purchase Line].[Direct Unit Cost] as Total," +
                    "case when dbo." + Environment.GetEnvironmentVariable("Company") + "Purchase Header].[Job Document Type]=0 THEN 'Purchase Order' ELSE 'Job Order' END as TypeOrder," + filter +
                   " FROM " +
                    "dbo." + Environment.GetEnvironmentVariable("Company") + "Purchase Header] " +
                    "INNER JOIN dbo." + Environment.GetEnvironmentVariable("Company") + "Purchase Line] ON dbo." + Environment.GetEnvironmentVariable("Company") + "Purchase Header].No_ = dbo." + Environment.GetEnvironmentVariable("Company") + "Purchase Line].[Document No_] " +
                    "WHERE " +
                       "dbo." + Environment.GetEnvironmentVariable("Company") + "Purchase Header].Status = 1 AND   dbo." + Environment.GetEnvironmentVariable("Company") + "Purchase Header].[Document Type] =1 and " +
                    "dbo." + Environment.GetEnvironmentVariable("Company") + "Purchase Line].[Outstanding Quantity] >0 and dbo." + Environment.GetEnvironmentVariable("Company") + "Purchase Header].[Job Document Type]=1 " +
                    "and dbo." + Environment.GetEnvironmentVariable("Company") + "Purchase Header].[Order Date]>={0} and dbo." + Environment.GetEnvironmentVariable("Company") + "Purchase Header].[Order Date]<={1}  " + Site;
                Outstandings = _navcontext.Outstandings.FromSqlRaw(queryData, date1, date2).ToList();
            }
            else if (type == "0")
            {
                filter = "'PostingDate :" + date11 + ".." + date22 + ",Type: Purchase Order,User : " + HttpContext.Session.GetString("Username") + " Job : " + jobFilter + "' as Filter ";
                queryData = "SELECT ROW_NUMBER() OVER (ORDER BY dbo." + Environment.GetEnvironmentVariable("Company") + "Purchase Header].No_) as ID, " +
                 "dbo." + Environment.GetEnvironmentVariable("Company") + "Purchase Header].No_ as OrderNo, " +
                 "dbo." + Environment.GetEnvironmentVariable("Company") + "Purchase Header].[Order Date] as OrderDate, " +
                  "dbo." + Environment.GetEnvironmentVariable("Company") + "Purchase Header].[Buy-from Vendor No_] as VendorNo, " +
                 "dbo." + Environment.GetEnvironmentVariable("Company") + "Purchase Header].[Pay-to Name] as VendorName, " +
                 "dbo." + Environment.GetEnvironmentVariable("Company") + "Purchase Line].No_ as ItemInLine, " +
                 "dbo." + Environment.GetEnvironmentVariable("Company") + "Purchase Line].Description as Description, " +
                 "dbo." + Environment.GetEnvironmentVariable("Company") + "Purchase Line].[Direct Unit Cost] as DirectUnitCost, " +
                 "dbo." + Environment.GetEnvironmentVariable("Company") + "Purchase Line].[Outstanding Quantity] as OutstandingQuantity, " +
                 "dbo." + Environment.GetEnvironmentVariable("Company") + "Purchase Line].[Unit of Measure] as UOM, " +
                 "dbo." + Environment.GetEnvironmentVariable("Company") + "Purchase Line].[Location Code] as JobNo, " +
                 "dbo." + Environment.GetEnvironmentVariable("Company") + "Purchase Line].Quantity, " +
                 "[Outstanding Quantity]*dbo." + Environment.GetEnvironmentVariable("Company") + "Purchase Line].[Direct Unit Cost] as Total," +
                 "case when dbo." + Environment.GetEnvironmentVariable("Company") + "Purchase Header].[Job Document Type]=0 THEN 'Purchase Order' ELSE 'Job Order' END as TypeOrder," + filter +

                 " FROM " +
                 "dbo." + Environment.GetEnvironmentVariable("Company") + "Purchase Header] " +
                 "INNER JOIN dbo." + Environment.GetEnvironmentVariable("Company") + "Purchase Line] ON dbo." + Environment.GetEnvironmentVariable("Company") + "Purchase Header].No_ = dbo." + Environment.GetEnvironmentVariable("Company") + "Purchase Line].[Document No_] " +
                 "WHERE " +
                   "dbo." + Environment.GetEnvironmentVariable("Company") + "Purchase Header].Status = 1 AND   dbo." + Environment.GetEnvironmentVariable("Company") + "Purchase Header].[Document Type] =1 and " +
                 "dbo." + Environment.GetEnvironmentVariable("Company") + "Purchase Line].[Outstanding Quantity] >0 and dbo." + Environment.GetEnvironmentVariable("Company") + "Purchase Header].[Job Document Type]=0 " +
                 "and dbo." + Environment.GetEnvironmentVariable("Company") + "Purchase Header].[Order Date]>={0} and dbo." + Environment.GetEnvironmentVariable("Company") + "Purchase Header].[Order Date]<={1}  " + Site;
                Outstandings = _navcontext.Outstandings.FromSqlRaw(queryData, date1, date2).ToList();
            }


         




            response = Ok(new { data = Outstandings });


            return response;

        }



        public IActionResult GetDataPDF(string Startdate, string EndDate, string type,string Job)
        {

            /*Check Session */
            var page = "274";
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

            var date11 = Startdate.Substring(6, 4) + "-" + Startdate.Substring(3, 2) + "-" + Startdate.Substring(0, 2);
            var date22 = EndDate.Substring(6, 4) + "-" + EndDate.Substring(3, 2) + "-" + EndDate.Substring(0, 2);

            var queryData = "";
            var jobFilter = "";
            List<Outstanding> Outstandings = new List<Outstanding>();
            IActionResult response = Unauthorized();
            var filter = "";
            var Site = "";

            if (Job == null)
            {
                Site = "";
            }
            else
            {
                jobFilter = Job.Replace("\'", "");
                Site = " AND dbo."+ Environment.GetEnvironmentVariable("Company") +"Purchase Line].[Location Code] IN (" + Job + ")";
            }

            if (type =="3")
            {

                filter = "'PostingDate :" + date11 + ".." + date22 + ",Type: All,User : "+ HttpContext.Session.GetString("Username") + " Job : " + jobFilter + "' as Filter ";
                queryData = "SELECT ROW_NUMBER() OVER (ORDER BY dbo." + Environment.GetEnvironmentVariable("Company") + "Purchase Header].No_) as ID, " +
                      "dbo." + Environment.GetEnvironmentVariable("Company") + "Purchase Header].No_ as OrderNo, " +
                      "dbo." + Environment.GetEnvironmentVariable("Company") + "Purchase Header].[Order Date] as OrderDate, " +
                      "dbo." + Environment.GetEnvironmentVariable("Company") + "Purchase Header].[Buy-from Vendor No_] as VendorNo, " +
                      "dbo." + Environment.GetEnvironmentVariable("Company") + "Purchase Header].[Pay-to Name] as VendorName, " +
                      "dbo." + Environment.GetEnvironmentVariable("Company") + "Purchase Line].No_ as ItemInLine, " +
                      "dbo." + Environment.GetEnvironmentVariable("Company") + "Purchase Line].Description as Description, " +
                      "dbo." + Environment.GetEnvironmentVariable("Company") + "Purchase Line].[Direct Unit Cost] as DirectUnitCost, " +
                      "dbo." + Environment.GetEnvironmentVariable("Company") + "Purchase Line].[Outstanding Quantity] as OutstandingQuantity, " +
                      "dbo." + Environment.GetEnvironmentVariable("Company") + "Purchase Line].[Unit of Measure] as UOM, " +
                      "dbo." + Environment.GetEnvironmentVariable("Company") + "Purchase Line].[Location Code] as JobNo, " +
                      "dbo." + Environment.GetEnvironmentVariable("Company") + "Purchase Line].Quantity, " +
                      "[Outstanding Quantity]*dbo." + Environment.GetEnvironmentVariable("Company") + "Purchase Line].[Direct Unit Cost] as Total," +
                      "case when dbo." + Environment.GetEnvironmentVariable("Company") + "Purchase Header].[Job Document Type]=0 THEN 'Purchase Order' ELSE 'Job Order' END as TypeOrder," + filter +

                      " FROM " +
                      "dbo." + Environment.GetEnvironmentVariable("Company") + "Purchase Header] " +
                      "INNER JOIN dbo." + Environment.GetEnvironmentVariable("Company") + "Purchase Line] ON dbo." + Environment.GetEnvironmentVariable("Company") + "Purchase Header].No_ = dbo." + Environment.GetEnvironmentVariable("Company") + "Purchase Line].[Document No_] " +
                      "WHERE " +
                        "dbo." + Environment.GetEnvironmentVariable("Company") + "Purchase Header].Status = 1 AND   dbo." + Environment.GetEnvironmentVariable("Company") + "Purchase Header].[Document Type] =1 and " +
                      "dbo." + Environment.GetEnvironmentVariable("Company") + "Purchase Line].[Outstanding Quantity] >0 " +
                      "and dbo." + Environment.GetEnvironmentVariable("Company") + "Purchase Header].[Order Date]>={0} and dbo." + Environment.GetEnvironmentVariable("Company") + "Purchase Header].[Order Date]<={1}  " + Site;
                Outstandings = _navcontext.Outstandings.FromSqlRaw(queryData, date1, date2).ToList();
            }
            else if(type == "1")
            {
                filter = "'PostingDate :" + date11 + ".." + date22 + ",Type: Job Order,User : " + HttpContext.Session.GetString("Username") + " Job : " + jobFilter + "' as Filter ";
                queryData = "SELECT ROW_NUMBER() OVER (ORDER BY dbo." + Environment.GetEnvironmentVariable("Company") + "Purchase Header].No_) as ID, " +
                    "dbo." + Environment.GetEnvironmentVariable("Company") + "Purchase Header].No_ as OrderNo, " +
                    "dbo." + Environment.GetEnvironmentVariable("Company") + "Purchase Header].[Order Date] as OrderDate, " +
                     "dbo." + Environment.GetEnvironmentVariable("Company") + "Purchase Header].[Buy-from Vendor No_] as VendorNo, " +
                    "dbo." + Environment.GetEnvironmentVariable("Company") + "Purchase Header].[Pay-to Name] as VendorName, " +
                    "dbo." + Environment.GetEnvironmentVariable("Company") + "Purchase Line].No_ as ItemInLine, " +
                    "dbo." + Environment.GetEnvironmentVariable("Company") + "Purchase Line].Description as Description, " +
                    "dbo." + Environment.GetEnvironmentVariable("Company") + "Purchase Line].[Direct Unit Cost] as DirectUnitCost, " +
                    "dbo." + Environment.GetEnvironmentVariable("Company") + "Purchase Line].[Outstanding Quantity] as OutstandingQuantity, " +
                    "dbo." + Environment.GetEnvironmentVariable("Company") + "Purchase Line].[Unit of Measure] as UOM, " +
                    "dbo." + Environment.GetEnvironmentVariable("Company") + "Purchase Line].[Location Code] as JobNo, " +
                    "dbo." + Environment.GetEnvironmentVariable("Company") + "Purchase Line].Quantity, " +
                    "[Outstanding Quantity]*dbo." + Environment.GetEnvironmentVariable("Company") + "Purchase Line].[Direct Unit Cost] as Total," +
                    "case when dbo." + Environment.GetEnvironmentVariable("Company") + "Purchase Header].[Job Document Type]=0 THEN 'Purchase Order' ELSE 'Job Order' END as TypeOrder," + filter +
                   " FROM " +
                    "dbo." + Environment.GetEnvironmentVariable("Company") + "Purchase Header] " +
                    "INNER JOIN dbo." + Environment.GetEnvironmentVariable("Company") + "Purchase Line] ON dbo." + Environment.GetEnvironmentVariable("Company") + "Purchase Header].No_ = dbo." + Environment.GetEnvironmentVariable("Company") + "Purchase Line].[Document No_] " +
                    "WHERE " +
                     "dbo." + Environment.GetEnvironmentVariable("Company") + "Purchase Header].Status = 1 AND   dbo." + Environment.GetEnvironmentVariable("Company") + "Purchase Header].[Document Type] =1 and " +
                    "dbo." + Environment.GetEnvironmentVariable("Company") + "Purchase Line].[Outstanding Quantity] >0 and dbo." + Environment.GetEnvironmentVariable("Company") + "Purchase Header].[Job Document Type]=1 " +
                    "and dbo." + Environment.GetEnvironmentVariable("Company") + "Purchase Header].[Order Date]>={0} and dbo." + Environment.GetEnvironmentVariable("Company") + "Purchase Header].[Order Date]<={1}  " + Site;
                Outstandings = _navcontext.Outstandings.FromSqlRaw(queryData, date1, date2).ToList();
            }
            else if (type == "0")
            {
                filter = "'PostingDate :" + date11 + ".." + date22 + ",Type: Purchase Order,User : " + HttpContext.Session.GetString("Username") + " Job : " + jobFilter + "' as Filter ";
                queryData = "SELECT ROW_NUMBER() OVER (ORDER BY dbo." + Environment.GetEnvironmentVariable("Company") + "Purchase Header].No_) as ID, " +
                 "dbo." + Environment.GetEnvironmentVariable("Company") + "Purchase Header].No_ as OrderNo, " +
                 "dbo." + Environment.GetEnvironmentVariable("Company") + "Purchase Header].[Order Date] as OrderDate, " +
                  "dbo." + Environment.GetEnvironmentVariable("Company") + "Purchase Header].[Buy-from Vendor No_] as VendorNo, " +
                 "dbo." + Environment.GetEnvironmentVariable("Company") + "Purchase Header].[Pay-to Name] as VendorName, " +
                 "dbo." + Environment.GetEnvironmentVariable("Company") + "Purchase Line].No_ as ItemInLine, " +
                 "dbo." + Environment.GetEnvironmentVariable("Company") + "Purchase Line].Description as Description, " +
                 "dbo." + Environment.GetEnvironmentVariable("Company") + "Purchase Line].[Direct Unit Cost] as DirectUnitCost, " +
                 "dbo." + Environment.GetEnvironmentVariable("Company") + "Purchase Line].[Outstanding Quantity] as OutstandingQuantity, " +
                 "dbo." + Environment.GetEnvironmentVariable("Company") + "Purchase Line].[Unit of Measure] as UOM, " +
                 "dbo." + Environment.GetEnvironmentVariable("Company") + "Purchase Line].[Location Code] as JobNo, " +
                 "dbo." + Environment.GetEnvironmentVariable("Company") + "Purchase Line].Quantity, " +
                 "[Outstanding Quantity]*dbo." + Environment.GetEnvironmentVariable("Company") + "Purchase Line].[Direct Unit Cost] as Total," +
                 "case when dbo." + Environment.GetEnvironmentVariable("Company") + "Purchase Header].[Job Document Type]=0 THEN 'Purchase Order' ELSE 'Job Order' END as TypeOrder," + filter +

                 " FROM " +
                 "dbo." + Environment.GetEnvironmentVariable("Company") + "Purchase Header] " +
                 "INNER JOIN dbo." + Environment.GetEnvironmentVariable("Company") + "Purchase Line] ON dbo." + Environment.GetEnvironmentVariable("Company") + "Purchase Header].No_ = dbo." + Environment.GetEnvironmentVariable("Company") + "Purchase Line].[Document No_] " +
                 "WHERE " +
                   "dbo." + Environment.GetEnvironmentVariable("Company") + "Purchase Header].Status = 1 AND   dbo." + Environment.GetEnvironmentVariable("Company") + "Purchase Header].[Document Type] =1 and " +
                 "dbo." + Environment.GetEnvironmentVariable("Company") + "Purchase Line].[Outstanding Quantity] >0 and dbo." + Environment.GetEnvironmentVariable("Company") + "Purchase Header].[Job Document Type]=0 " +
                 "and dbo." + Environment.GetEnvironmentVariable("Company") + "Purchase Header].[Order Date]>={0} and dbo." + Environment.GetEnvironmentVariable("Company") + "Purchase Header].[Order Date]<={1}  " + Site;
                Outstandings = _navcontext.Outstandings.FromSqlRaw(queryData, date1, date2).ToList();
            }







            XtraReport report = XtraReport.FromFile("reports\\Outstanding.repx");
            report.DataSource = Outstandings;


            report.CreateDocument(true);
            var @out = new MemoryStream();
            report.ExportToPdf(@out);
            @out.Position = 0;



            return new FileStreamResult(@out, "application/pdf");

        }

    }
}