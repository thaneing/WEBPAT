using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
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
using Newtonsoft.Json;
using static CESAPSCOREWEBAPP.Models.Enums;

namespace CESAPSCOREWEBAPP.Controllers
{
    [Authorize]
    public class DocumentBillingsController : BaseController
    {
        private readonly NAVContext _navcontext;
        private readonly DatabaseContext _context;


        public DocumentBillingsController(NAVContext navcontext, DatabaseContext context)
        {
            _context = context;
            _navcontext = navcontext;
        }

        public IActionResult Index()
        {

            /*Check Session */
            var page = "269";
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
            var sql = "SELECT dbo."+ Environment.GetEnvironmentVariable("Company") +"Purchase Header].No_ as code, dbo."+ Environment.GetEnvironmentVariable("Company") +"Purchase Header].No_ as name " +
            "FROM dbo."+ Environment.GetEnvironmentVariable("Company") +"Purchase Header] WHERE [Job Document Type]=0  order by dbo."+ Environment.GetEnvironmentVariable("Company") +"Purchase Header].No_ ";

            //SqlParameter parameterEmpId = new SqlParameter("@empid", empid);
            var PO = _navcontext.sourceAutoCompletes.FromSqlRaw(sql).ToList();

            var sqlquery = "SELECT Code as name,Code as code FROM [dbo]."+ Environment.GetEnvironmentVariable("Company") +"Dimension Value] WHERE [Dimension Code]='JOB_GL' order by Code";

            //SqlParameter parameterEmpId = new SqlParameter("@empid", empid);
            var site = _navcontext.sourceAutoCompletes.FromSqlRaw(sqlquery).ToList();



            ViewData["PO"] =PO;

            ViewData["Site"] = site;

            ViewBag.StartDate = DateTime.Now.ToString("dd-MM-yyyy", new CultureInfo("en-US"));
            ViewBag.EndDate = DateTime.Now.ToString("dd-MM-yyyy", new CultureInfo("en-US"));




            return View();
        }




        [HttpGet]
        public IActionResult GetDataPO(string PO)
        {
            /*Check Session */
            var page = "269";
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
            var sql = "SELECT dbo."+ Environment.GetEnvironmentVariable("Company") +"Purchase Header].[Pay-to Name] AS name,dbo."+ Environment.GetEnvironmentVariable("Company") +"Purchase Header].[Shortcut Dimension 1 Code] as code " +
                "FROM dbo."+ Environment.GetEnvironmentVariable("Company") +"Purchase Header] WHERE [Job Document Type]=0 and dbo."+ Environment.GetEnvironmentVariable("Company") +"Purchase Header].No_ ={0} order by dbo."+ Environment.GetEnvironmentVariable("Company") +"Purchase Header].No_ ";

            //SqlParameter parameterEmpId = new SqlParameter("@empid", empid);
            var site = _navcontext.sourceAutoCompletes.FromSqlRaw(sql ,PO).ToList();


            response = Ok(new { data = site });


            return response;
        }


        [HttpGet]
        public IActionResult AddData(int ID,string date1,string PO, string vendor,string site, string invoice, string Delivery, string Etc)
        {

            /*Check Session */
            var page = "270";
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
            var dStartdate = date1.Substring(6, 4) + "-" + date1.Substring(3, 2) + "-" + date1.Substring(0, 2) + " 00:00:00";

            var StartDate = DateTime.Parse(dStartdate);
            var documentBilling = new DocumentBilling();
            if (ID == null)
            {
                documentBilling.PostingDate = StartDate;
                documentBilling.PONo = PO;
                documentBilling.Site = site;
                documentBilling.VendorName = vendor;
                documentBilling.DeliveryOrder = Delivery;
                documentBilling.InvoiceNo = invoice;
                documentBilling.Etc = Etc;
                documentBilling.CreateBy = HttpContext.Session.GetString("Username");
                documentBilling.CreateDate = DateTime.Now;
                _context.DocumentBillings.Add(documentBilling);
                _context.SaveChanges();
                response = Ok(new { data = documentBilling, datatype = "บันทึก" });
            }
            else {

                var documentBillings = _context.DocumentBillings.Where(a => a.ID ==ID).ToList();
                if (documentBillings.Count==0)
                {
                    documentBilling.PostingDate = StartDate;
                    documentBilling.PONo = PO;
                    documentBilling.Site = site;
                    documentBilling.VendorName = vendor;
                    documentBilling.DeliveryOrder = Delivery;
                    documentBilling.InvoiceNo = invoice;
                    documentBilling.Etc = Etc;
                    documentBilling.CreateBy = HttpContext.Session.GetString("Username");
                    documentBilling.CreateDate = DateTime.Now;
                    _context.DocumentBillings.Add(documentBilling);
                    _context.SaveChanges();
                    response = Ok(new { data = documentBilling, datatype = "บันทึก" });
                }
                else
                {
    
                    documentBillings[0].PostingDate = StartDate;
                    documentBillings[0].PONo = PO;
                    documentBillings[0].Site = site;
                    documentBillings[0].VendorName = vendor;
                    documentBillings[0].DeliveryOrder = Delivery;
                    documentBillings[0].InvoiceNo = invoice;
                    documentBillings[0].Etc = Etc;
                    documentBillings[0].UpdateBy = HttpContext.Session.GetString("Username");
                    documentBillings[0].UpdateDate = DateTime.Now;
                    _context.DocumentBillings.Update(documentBillings[0]);
                    _context.SaveChanges();
                    response = Ok(new { data = documentBillings[0],datatype="update" });
                }
            }

            return response;
        }


        [HttpGet]
        public IActionResult GetData(string Startdate, string EndDate)
        {


            /*Check Session */
            var page = "269";
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



            var date1 = Startdate.Substring(6, 4) + "-" + Startdate.Substring(3, 2) + "-" + Startdate.Substring(0, 2) + " 00:00:00";
            var date2 = EndDate.Substring(6, 4) + "-" + EndDate.Substring(3, 2) + "-" + EndDate.Substring(0, 2) + " 23:59:59";

            var ddate1 = DateTime.Parse(date1);
            var ddate2 = DateTime.Parse(date2);




            IActionResult response = Unauthorized();
            //var queryData = "SELECT a.PostingDate,a.EntryNo,a.CusLedgerEntryNo,a.DocumentType,a.DocumentNo,a.CustomerNo,a.CustomerName,a.CustomerPostingGroup,a.AmountAP," +
            //    " a.AmountApLCY,a.DueDate,a.GlobalDim1,a.EntryType,a.SourceCode,a.UserId,a.Unapplied," +
            //    " (select TOP 1 dbo." + Environment.GetEnvironmentVariable("Company") + "Detailed Cust_ Ledg_ Entry].[Document No_] FROM dbo." + Environment.GetEnvironmentVariable("Company") + "Detailed Cust_ Ledg_ Entry] WHERE dbo." + Environment.GetEnvironmentVariable("Company") + "Detailed Cust_ Ledg_ Entry].[Cust_ Ledger Entry No_]=a.CusLedgerEntryNo and (dbo." + Environment.GetEnvironmentVariable("Company") + "Detailed Cust_ Ledg_ Entry].[Document Type]!=2 and dbo." + Environment.GetEnvironmentVariable("Company") + "Detailed Cust_ Ledg_ Entry].[Document Type]!=3)) as RVDocNo," +
            //    " (select TOP 1 convert(varchar, dbo." + Environment.GetEnvironmentVariable("Company") + "Detailed Cust_ Ledg_ Entry].[Posting Date], 23) FROM dbo." + Environment.GetEnvironmentVariable("Company") + "Detailed Cust_ Ledg_ Entry] WHERE dbo." + Environment.GetEnvironmentVariable("Company") + "Detailed Cust_ Ledg_ Entry].[Cust_ Ledger Entry No_]=a.CusLedgerEntryNo and (dbo." + Environment.GetEnvironmentVariable("Company") + "Detailed Cust_ Ledg_ Entry].[Document Type]!=2 and dbo." + Environment.GetEnvironmentVariable("Company") + "Detailed Cust_ Ledg_ Entry].[Document Type]!=3)) as RVDate " +
            //    " FROM (SELECT " +
            //    " convert(varchar, dbo." + Environment.GetEnvironmentVariable("Company") + "Detailed Cust_ Ledg_ Entry].[Posting Date], 23) AS PostingDate," +
            //    " dbo." + Environment.GetEnvironmentVariable("Company") + "Detailed Cust_ Ledg_ Entry].[Cust_ Ledger Entry No_] as CusLedgerEntryNo," +
            //    " CASE WHEN dbo." + Environment.GetEnvironmentVariable("Company") + "Detailed Cust_ Ledg_ Entry].[Document Type] =2 then 'Invoice'" +
            //    " 		WHEN dbo." + Environment.GetEnvironmentVariable("Company") + "Detailed Cust_ Ledg_ Entry].[Document Type] =3 then 'CreaditMemmo' END AS DocumentType," +
            //    " dbo." + Environment.GetEnvironmentVariable("Company") + "Detailed Cust_ Ledg_ Entry].[Document No_] AS DocumentNo," +
            //    " dbo." + Environment.GetEnvironmentVariable("Company") + "Detailed Cust_ Ledg_ Entry].[Customer No_] AS CustomerNo," +
            //    " convert(varchar,FORMAT(dbo." + Environment.GetEnvironmentVariable("Company") + "Detailed Cust_ Ledg_ Entry].Amount,'###,###,###.00','en-US')) AS AmountAP," +
            //    " convert(varchar,FORMAT(dbo." + Environment.GetEnvironmentVariable("Company") + "Detailed Cust_ Ledg_ Entry].[Amount (LCY)],'###,###,###.00','en-US')) AS AmountApLCY," +
            //    " convert(varchar, dbo." + Environment.GetEnvironmentVariable("Company") + "Detailed Cust_ Ledg_ Entry].[Initial Entry Due Date],23) AS DueDate," +
            //    " dbo." + Environment.GetEnvironmentVariable("Company") + "Detailed Cust_ Ledg_ Entry].[Initial Entry Global Dim_ 1] AS GlobalDim1," +
            //    " CASE WHEN dbo." + Environment.GetEnvironmentVariable("Company") + "Detailed Cust_ Ledg_ Entry].[Entry Type]=2 THEN 'Application' " +
            //    " 			WHEN dbo." + Environment.GetEnvironmentVariable("Company") + "Detailed Cust_ Ledg_ Entry].[Entry Type]=1 THEN 'Initial Entry' END AS EntryType," +
            //    " dbo." + Environment.GetEnvironmentVariable("Company") + "Detailed Cust_ Ledg_ Entry].[Source Code] AS SourceCode," +
            //    " dbo." + Environment.GetEnvironmentVariable("Company") + "Detailed Cust_ Ledg_ Entry].[Unapplied by Entry No_] AS Unapplied," +
            //    " dbo." + Environment.GetEnvironmentVariable("Company") + "Detailed Cust_ Ledg_ Entry].[User ID] AS UserId," +
            //    " dbo." + Environment.GetEnvironmentVariable("Company") + "Customer].Name AS CustomerName," +
            //    " dbo." + Environment.GetEnvironmentVariable("Company") + "Customer].[Customer Posting Group] AS CustomerPostingGroup," +
            //    " dbo." + Environment.GetEnvironmentVariable("Company") + "Detailed Cust_ Ledg_ Entry].[Entry No_] as EntryNo" +
            //    " FROM " +
            //    " dbo." + Environment.GetEnvironmentVariable("Company") + "Detailed Cust_ Ledg_ Entry] " +
            //    " INNER JOIN dbo." + Environment.GetEnvironmentVariable("Company") + "Customer] ON dbo." + Environment.GetEnvironmentVariable("Company") + "Detailed Cust_ Ledg_ Entry].[Customer No_] = dbo." + Environment.GetEnvironmentVariable("Company") + "Customer].No_ " +
            //    " WHERE(dbo." + Environment.GetEnvironmentVariable("Company") + "Detailed Cust_ Ledg_ Entry].[Document Type] = '2' OR dbo." + Environment.GetEnvironmentVariable("Company") + "Detailed Cust_ Ledg_ Entry].[Document Type] = '3')  " +
            //    " and dbo." + Environment.GetEnvironmentVariable("Company") + "Detailed Cust_ Ledg_ Entry].[Posting Date] >={0} and dbo." + Environment.GetEnvironmentVariable("Company") + "Detailed Cust_ Ledg_ Entry].[Posting Date]<={1}" +
            //    " ) as a ";




            //SqlParameter parameterStart = new SqlParameter("@startdate", date1);
            //SqlParameter parameterDate1 = new SqlParameter("@enddate", date2);
            //ViewBag.sql = queryData;
            var documentBillings = _context.DocumentBillings.Where(p => p.PostingDate >= ddate1 && p.PostingDate <= ddate2).OrderBy(p=>p.PostingDate).ToList();


            response = Ok(new { data = documentBillings });


            return response;
        }



        [HttpGet]
        public IActionResult GetByID(int ID)
        {
            /*Check Session */
            var page = "269";
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

            //var documentBilling = new DocumentBilling();

            var documentBillings = _context.DocumentBillings.Where(a => a.ID == ID).ToList();
            if (documentBillings.Count > 0)
            {

                response = Ok(new { data = documentBillings[0], datatype = "0" });
            }
            else
            {
                response = Ok(new { data = documentBillings[0], datatype = "1" });
            }


            return response;
        }



      

        [HttpGet]
        public IActionResult DeleteByID(int ID)
        {
            /*Check Session */
            var page = "272";
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

            //var documentBilling = new DocumentBilling();


            var documentBilling = new DocumentBilling();
            var documentBillings = _context.DocumentBillings.Where(a => a.ID == ID).ToList();
            if (documentBillings.Count > 0)
            {

               _context.DocumentBillings.Remove(documentBillings[0]);
                _context.SaveChanges();
                response = Ok(new { data = documentBillings[0], datatype = "1" });
            }
            else
            {
                response = Ok(new { data = documentBilling, datatype = "0" });
            }


            return response;
        }

    }
}