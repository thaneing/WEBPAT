using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;
using CESAPSCOREWEBAPP.Helpers;
using CESAPSCOREWEBAPP.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using static CESAPSCOREWEBAPP.Models.Enums;

namespace CESAPSCOREWEBAPP.Controllers
{
    [Authorize]
    public class APRVController : BaseController
    {
            private readonly NAVContext _navcontext;
            private readonly DatabaseContext _context;


        public APRVController(NAVContext navcontext, DatabaseContext context)
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



            ViewBag.StartDate = DateTime.Now.ToString("01-MM-yyyy", new CultureInfo("en-US"));
            ViewBag.EndDate = DateTime.Now.ToString("dd-MM-yyyy", new CultureInfo("en-US"));

            return View();
       }


        public IActionResult GetData(string Startdate, string EndDate)
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



            var date1 = Startdate.Substring(6, 4) + "-" + Startdate.Substring(3, 2) + "-" + Startdate.Substring(0, 2) + " 00:00:00";
            var date2 = EndDate.Substring(6, 4) + "-" + EndDate.Substring(3, 2) + "-" + EndDate.Substring(0, 2) + " 23:59:59";
            var sdate1 = Startdate;
            var sdate2 = EndDate;
            var rdate1 = Startdate;
            var rdate2 = EndDate;



            IActionResult response = Unauthorized();
            var queryData = "SELECT a.PostingDate,a.EntryNo,a.CusLedgerEntryNo,a.DocumentType,a.DocumentNo,a.CustomerNo,a.CustomerName,a.CustomerPostingGroup,a.AmountAP," +
                " a.AmountApLCY,a.DueDate,a.GlobalDim1,a.EntryType,a.SourceCode,a.UserId,a.Unapplied,a.Description, " +
                " (select TOP 1 dbo."+ Environment.GetEnvironmentVariable("Company") +"Detailed Cust_ Ledg_ Entry].[Document No_] FROM dbo."+ Environment.GetEnvironmentVariable("Company") +"Detailed Cust_ Ledg_ Entry] WHERE dbo."+ Environment.GetEnvironmentVariable("Company") +"Detailed Cust_ Ledg_ Entry].[Cust_ Ledger Entry No_]=a.CusLedgerEntryNo and (dbo."+ Environment.GetEnvironmentVariable("Company") +"Detailed Cust_ Ledg_ Entry].[Document Type]!=2 and dbo."+ Environment.GetEnvironmentVariable("Company") +"Detailed Cust_ Ledg_ Entry].[Document Type]!=3)) as RVDocNo," +
                " (select TOP 1 convert(varchar, dbo."+ Environment.GetEnvironmentVariable("Company") +"Detailed Cust_ Ledg_ Entry].[Posting Date], 23) FROM dbo."+ Environment.GetEnvironmentVariable("Company") +"Detailed Cust_ Ledg_ Entry] WHERE dbo."+ Environment.GetEnvironmentVariable("Company") +"Detailed Cust_ Ledg_ Entry].[Cust_ Ledger Entry No_]=a.CusLedgerEntryNo and (dbo."+ Environment.GetEnvironmentVariable("Company") +"Detailed Cust_ Ledg_ Entry].[Document Type]!=2 and dbo."+ Environment.GetEnvironmentVariable("Company") +"Detailed Cust_ Ledg_ Entry].[Document Type]!=3)) as RVDate " +
                " FROM (SELECT " +
                " convert(varchar, dbo."+ Environment.GetEnvironmentVariable("Company") +"Detailed Cust_ Ledg_ Entry].[Posting Date], 23) AS PostingDate," +
                " dbo."+ Environment.GetEnvironmentVariable("Company") +"Detailed Cust_ Ledg_ Entry].[Cust_ Ledger Entry No_] as CusLedgerEntryNo," +
                " CASE WHEN dbo."+ Environment.GetEnvironmentVariable("Company") +"Detailed Cust_ Ledg_ Entry].[Document Type] =2 then 'Invoice'" +
                " 		WHEN dbo."+ Environment.GetEnvironmentVariable("Company") +"Detailed Cust_ Ledg_ Entry].[Document Type] =3 then 'CreaditMemmo' END AS DocumentType," +
                " dbo."+ Environment.GetEnvironmentVariable("Company") +"Detailed Cust_ Ledg_ Entry].[Document No_] AS DocumentNo," +
                "dbo." + Environment.GetEnvironmentVariable("Company") + "Cust_ Ledger Entry].Description," +
                " dbo."+ Environment.GetEnvironmentVariable("Company") +"Detailed Cust_ Ledg_ Entry].[Customer No_] AS CustomerNo," +
                " convert(varchar,FORMAT(dbo."+ Environment.GetEnvironmentVariable("Company") +"Detailed Cust_ Ledg_ Entry].Amount,'###,###,###.00','en-US')) AS AmountAP," +
                " convert(varchar,FORMAT(dbo."+ Environment.GetEnvironmentVariable("Company") +"Detailed Cust_ Ledg_ Entry].[Amount (LCY)],'###,###,###.00','en-US')) AS AmountApLCY," +
                " convert(varchar, dbo."+ Environment.GetEnvironmentVariable("Company") +"Detailed Cust_ Ledg_ Entry].[Initial Entry Due Date],23) AS DueDate," +
                " dbo."+ Environment.GetEnvironmentVariable("Company") +"Detailed Cust_ Ledg_ Entry].[Initial Entry Global Dim_ 1] AS GlobalDim1," +
                " CASE WHEN dbo."+ Environment.GetEnvironmentVariable("Company") +"Detailed Cust_ Ledg_ Entry].[Entry Type]=2 THEN 'Application' " +
                " 			WHEN dbo."+ Environment.GetEnvironmentVariable("Company") +"Detailed Cust_ Ledg_ Entry].[Entry Type]=1 THEN 'Initial Entry' END AS EntryType," +
                " dbo."+ Environment.GetEnvironmentVariable("Company") +"Detailed Cust_ Ledg_ Entry].[Source Code] AS SourceCode," +
                " dbo."+ Environment.GetEnvironmentVariable("Company") +"Detailed Cust_ Ledg_ Entry].[Unapplied by Entry No_] AS Unapplied," +
                " dbo."+ Environment.GetEnvironmentVariable("Company") +"Detailed Cust_ Ledg_ Entry].[User ID] AS UserId," +
                " dbo."+ Environment.GetEnvironmentVariable("Company") +"Customer].Name AS CustomerName," +
                " dbo."+ Environment.GetEnvironmentVariable("Company") +"Customer].[Customer Posting Group] AS CustomerPostingGroup," +
                " dbo."+ Environment.GetEnvironmentVariable("Company") +"Detailed Cust_ Ledg_ Entry].[Entry No_] as EntryNo" +
                " FROM " +
                " dbo."+ Environment.GetEnvironmentVariable("Company") +"Detailed Cust_ Ledg_ Entry] " +
                " INNER JOIN dbo."+ Environment.GetEnvironmentVariable("Company") +"Customer] ON dbo."+ Environment.GetEnvironmentVariable("Company") +"Detailed Cust_ Ledg_ Entry].[Customer No_] = dbo."+ Environment.GetEnvironmentVariable("Company") +"Customer].No_ " +
                " INNER  JOIN dbo."+ Environment.GetEnvironmentVariable("Company") +"Cust_ Ledger Entry] ON dbo."+ Environment.GetEnvironmentVariable("Company") +"Detailed Cust_ Ledg_ Entry].[Cust_ Ledger Entry No_] = dbo."+ Environment.GetEnvironmentVariable("Company") +"Cust_ Ledger Entry].[Entry No_] " +
                " WHERE(dbo."+ Environment.GetEnvironmentVariable("Company") +"Detailed Cust_ Ledg_ Entry].[Document Type] = '2' OR dbo."+ Environment.GetEnvironmentVariable("Company") +"Detailed Cust_ Ledg_ Entry].[Document Type] = '3')  " +
                " and dbo."+ Environment.GetEnvironmentVariable("Company") +"Detailed Cust_ Ledg_ Entry].[Posting Date] >={0} and dbo."+ Environment.GetEnvironmentVariable("Company") +"Detailed Cust_ Ledg_ Entry].[Posting Date]<={1}" +
                " ) as a ";


            //SqlParameter parameterStart = new SqlParameter("@startdate", date1);
            //SqlParameter parameterDate1 = new SqlParameter("@enddate", date2);
            //ViewBag.sql = queryData;
            var APRVDATA = _navcontext.v_APRVs.FromSqlRaw(queryData, date1, date2).ToList();


            response = Ok(new { data = APRVDATA });


            return response;
        }
    }
}