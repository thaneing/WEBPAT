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
    public class AppvsController : BaseController
    {
        private readonly NAVContext _navcontext;
        private readonly DatabaseContext _context;


        public AppvsController(NAVContext navcontext, DatabaseContext context)
        {
            _context = context;
            _navcontext = navcontext;
        }


        public IActionResult Index()
        {

            /*Check Session */
            var page = "156";
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
            var page = "156";
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
            var sdate1 = Startdate;
            var sdate2 = EndDate;
            var rdate1 = Startdate;
            var rdate2 = EndDate;



            IActionResult response = Unauthorized();
            var queryData = " SELECT a.PostingDate,a.EntryNo,a.VendorLedgerEntry,a.DocumentNo,a.VendorNo,a.VendorName,a.VendorPostingGroup,a.DocumentType,a.UserId,a.Amount,a.AmountLCY,a.SourceCode,a.InitialEntryDueDate,a.InitialEntryGlobalDim1,a.InitialEntryGlobalDim2,a.EntryType, "
            + " (select count([Document No_])  from dbo."+ Environment.GetEnvironmentVariable("Company") +"Detailed Vendor Ledg_ Entry] WHERE [Source Code]='PAYMENTJNL' and [Vendor Ledger Entry No_]=a.VendorLedgerEntry) as CountDocument,"
            + " (select top 1 convert(varchar, [Posting Date], 23)  from dbo."+ Environment.GetEnvironmentVariable("Company") +"Detailed Vendor Ledg_ Entry] WHERE [Source Code]='PAYMENTJNL' and [Vendor Ledger Entry No_]=a.VendorLedgerEntry ORDER BY [Entry No_] desc) as Paydate,"
            + " (select top 1 [Document No_]  from dbo."+ Environment.GetEnvironmentVariable("Company") +"Detailed Vendor Ledg_ Entry] WHERE [Source Code]='PAYMENTJNL' and [Vendor Ledger Entry No_]=a.VendorLedgerEntry ORDER BY [Entry No_] desc) as DocumentName," +
            " (select top 1 dbo."+ Environment.GetEnvironmentVariable("Company") +"VAT Entry].Amount  from dbo."+ Environment.GetEnvironmentVariable("Company") +"VAT Entry] WHERE  dbo."+ Environment.GetEnvironmentVariable("Company") +"VAT Entry].[Document No_]=a.DocumentNo ORDER BY [Entry No_] desc) as VatAmount "
            + " FROM(SELECT convert(varchar, [Posting Date], 23)  as PostingDate,dbo."+ Environment.GetEnvironmentVariable("Company") +"Detailed Vendor Ledg_ Entry].[Entry No_] AS EntryNo,dbo."+ Environment.GetEnvironmentVariable("Company") +"Detailed Vendor Ledg_ Entry].[Vendor Ledger Entry No_] AS VendorLedgerEntry,dbo."+ Environment.GetEnvironmentVariable("Company") +"Detailed Vendor Ledg_ Entry].[Document No_] AS DocumentNo,"
            + " [Vendor No_] AS VendorNo,Name AS VendorName,[Vendor Posting Group] AS VendorPostingGroup,CASE  WHEN [Document Type]=2 THEN 'IV'WHEN [Document Type]=3 THEN 'Cr.Memmo' END AS DocumentType,"
            + " convert(varchar,FORMAT(dbo."+ Environment.GetEnvironmentVariable("Company") +"Detailed Vendor Ledg_ Entry].Amount,'###,###,###.00','en-US')) as Amount,"
            + " convert(varchar,FORMAT(dbo."+ Environment.GetEnvironmentVariable("Company") +"Detailed Vendor Ledg_ Entry].[Amount (LCY)],'###,###,###.00','en-US')) AS AmountLCY,"
            + " dbo."+ Environment.GetEnvironmentVariable("Company") +"Detailed Vendor Ledg_ Entry].[User ID] as UserId,"
            + " dbo."+ Environment.GetEnvironmentVariable("Company") +"Detailed Vendor Ledg_ Entry].[Source Code] AS SourceCode,"
            + " convert(varchar,dbo."+ Environment.GetEnvironmentVariable("Company") +"Detailed Vendor Ledg_ Entry].[Initial Entry Due Date], 23) AS InitialEntryDueDate,"
            + " dbo."+ Environment.GetEnvironmentVariable("Company") +"Detailed Vendor Ledg_ Entry].[Initial Entry Global Dim_ 1] AS InitialEntryGlobalDim1,"
            + " dbo."+ Environment.GetEnvironmentVariable("Company") +"Detailed Vendor Ledg_ Entry].[Initial Entry Global Dim_ 2] AS InitialEntryGlobalDim2,"
            + " CASE  WHEN [Entry Type]=2 THEN 'Appcation' WHEN [Entry Type]=1 THEN 'Initial Entry' END AS EntryType"
            + " FROM dbo."+ Environment.GetEnvironmentVariable("Company") +"Detailed Vendor Ledg_ Entry] INNER JOIN dbo."+ Environment.GetEnvironmentVariable("Company") +"Vendor] ON dbo."+ Environment.GetEnvironmentVariable("Company") +"Detailed Vendor Ledg_ Entry].[Vendor No_] = dbo."+ Environment.GetEnvironmentVariable("Company") +"Vendor].No_"
            + " WHERE dbo."+ Environment.GetEnvironmentVariable("Company") +"Detailed Vendor Ledg_ Entry].[Source Code] IN ('PURCHAPPL', 'PURCHASES', 'PURCHJNL', 'JOBGLJNL') and [Posting Date]>={0} and [Posting Date]<={1})as a  ORDER BY a.PostingDate DESC";

       
            //ViewBag.sql = queryData;
            List<V_APPV> APPVDATA = _navcontext.v_APPVs.FromSqlRaw(queryData, date1, date2).ToList();


            response = Ok(new { data = APPVDATA });


            return response;

        }



        public IActionResult CheckPo()
        {

            /*Check Session */
            var page = "158";
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




        public IActionResult GetPoData()
        {
            /*Check Session */
            var page = "158";
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
            var queryData = " SELECT ROW_NUMBER() OVER (ORDER BY dbo." + Environment.GetEnvironmentVariable("Company") + "Purchase Header].[Document Date]) as ID,convert(varchar,dbo." + Environment.GetEnvironmentVariable("Company") +"Purchase Header].[Document Date] , 23) AS DocumentDate, "
            + " dbo."+ Environment.GetEnvironmentVariable("Company") +"Purchase Line].[Location Code] AS LocationCode,"
            + " dbo."+ Environment.GetEnvironmentVariable("Company") +"Purchase Line].[Document No_] AS DocumentNo,"
            + " dbo."+ Environment.GetEnvironmentVariable("Company") +"Purchase Line].No_ AS ItemNo,"
            + " dbo."+ Environment.GetEnvironmentVariable("Company") +"Purchase Line].Description AS Description,"
            + " dbo."+ Environment.GetEnvironmentVariable("Company") +"Purchase Line].[Description 2] AS Description2,"
            + " convert(varchar,FORMAT(dbo."+ Environment.GetEnvironmentVariable("Company") +"Purchase Line].Quantity,'###,###,###.00','en-US')) as Quantity,"
            + " convert(varchar,FORMAT(dbo."+ Environment.GetEnvironmentVariable("Company") +"Purchase Line].Amount,'###,###,###.00','en-US')) as Amount,"
            + " dbo."+ Environment.GetEnvironmentVariable("Company") +"Purchase Header].[Last Receiving No_] as LastReceive,"
            + " dbo."+ Environment.GetEnvironmentVariable("Company") +"Purchase Header].[Assigned User ID] as UserCreate"
            + " FROM dbo."+ Environment.GetEnvironmentVariable("Company") +"Purchase Header]"
            + " INNER JOIN dbo."+ Environment.GetEnvironmentVariable("Company") +"Purchase Line] ON dbo."+ Environment.GetEnvironmentVariable("Company") +"Purchase Line].[Document No_] = dbo."+ Environment.GetEnvironmentVariable("Company") +"Purchase Header].No_"
        + " WHERE (([Outstanding Quantity]!=[Outstanding Qty_ (Base)]) or (dbo." + Environment.GetEnvironmentVariable("Company") + "Purchase Line].No_ LIKE 'PC%' AND dbo." + Environment.GetEnvironmentVariable("Company") + "Purchase Line].[Job Task No_] <> '' " +
        "   and dbo." + Environment.GetEnvironmentVariable("Company") + "Purchase Header].[Document Date]>'2019-03-01 00:00:00') " +
        " or  ((dbo." + Environment.GetEnvironmentVariable("Company") + "Purchase Line].[Shortcut Dimension 1 Code]='2070' or dbo." + Environment.GetEnvironmentVariable("Company") + "Purchase Line].[Shortcut Dimension 1 Code]='2071'  " +
        " or dbo." + Environment.GetEnvironmentVariable("Company") + "Purchase Header].[Shortcut Dimension 1 Code]='2071' or dbo." + Environment.GetEnvironmentVariable("Company") + "Purchase Header].[Shortcut Dimension 1 Code]='2070' " +
        " or dbo." + Environment.GetEnvironmentVariable("Company") + "Purchase Line].[Shortcut Dimension 1 Code]='1041' or dbo." + Environment.GetEnvironmentVariable("Company") + "Purchase Line].[Shortcut Dimension 1 Code]='1040' " +
        " or dbo." + Environment.GetEnvironmentVariable("Company") + "Purchase Header].[Shortcut Dimension 1 Code]='1041' or dbo." + Environment.GetEnvironmentVariable("Company") + "Purchase Header].[Shortcut Dimension 1 Code]='1040') and dbo." + Environment.GetEnvironmentVariable("Company") + "Purchase Line].[Document No_]<>'POPMKR-62060030'  " +
        "))  "
        + " ORDER BY dbo." + Environment.GetEnvironmentVariable("Company") + "Purchase Header].[Document Date] DESC";



            //ViewBag.sql = queryData;
            var CheckPOPCJobtasks = _navcontext.v_CheckPOPCJobtasks.FromSqlRaw(queryData).ToList();


            response = Ok(new { data = CheckPOPCJobtasks });


            return response;



        }





        public IActionResult GetPoError()
        {

            IActionResult response = Unauthorized();
            var queryData = " SELECT ROW_NUMBER() OVER (ORDER BY dbo." + Environment.GetEnvironmentVariable("Company") + "Purchase Header].[Document Date]) as ID,convert(varchar,dbo." + Environment.GetEnvironmentVariable("Company") +"Purchase Header].[Document Date] , 23) AS DocumentDate, "
        + " dbo."+ Environment.GetEnvironmentVariable("Company") +"Purchase Line].[Location Code] AS LocationCode,"
        + " dbo."+ Environment.GetEnvironmentVariable("Company") +"Purchase Line].[Document No_] AS DocumentNo,"
        + " dbo."+ Environment.GetEnvironmentVariable("Company") +"Purchase Line].No_ AS ItemNo,"
        + " dbo."+ Environment.GetEnvironmentVariable("Company") +"Purchase Line].Description AS Description,"
        + " dbo."+ Environment.GetEnvironmentVariable("Company") +"Purchase Line].[Description 2] AS Description2,"
        + " convert(varchar,FORMAT(dbo."+ Environment.GetEnvironmentVariable("Company") +"Purchase Line].Quantity,'###,###,###.00','en-US')) as Quantity,"
        + " convert(varchar,FORMAT(dbo."+ Environment.GetEnvironmentVariable("Company") +"Purchase Line].Amount,'###,###,###.00','en-US')) as Amount,"
        + " dbo."+ Environment.GetEnvironmentVariable("Company") +"Purchase Header].[Last Receiving No_] as LastReceive,"
        + " dbo."+ Environment.GetEnvironmentVariable("Company") +"Purchase Header].[Assigned User ID] as UserCreate"
        + " FROM dbo."+ Environment.GetEnvironmentVariable("Company") +"Purchase Header]"
        + " INNER JOIN dbo."+ Environment.GetEnvironmentVariable("Company") +"Purchase Line] ON dbo."+ Environment.GetEnvironmentVariable("Company") +"Purchase Line].[Document No_] = dbo."+ Environment.GetEnvironmentVariable("Company") +"Purchase Header].No_"
        + " WHERE (([Outstanding Quantity]!=[Outstanding Qty_ (Base)]) or (dbo." + Environment.GetEnvironmentVariable("Company") +"Purchase Line].No_ LIKE 'PC%' AND dbo."+ Environment.GetEnvironmentVariable("Company") +"Purchase Line].[Job Task No_] <> '' " +
        "   and dbo."+ Environment.GetEnvironmentVariable("Company") +"Purchase Header].[Document Date]>'2019-03-01 00:00:00') " +
        " or  ((dbo."+ Environment.GetEnvironmentVariable("Company") +"Purchase Line].[Shortcut Dimension 1 Code]='2070' or dbo."+ Environment.GetEnvironmentVariable("Company") +"Purchase Line].[Shortcut Dimension 1 Code]='2071'  " +
        " or dbo."+ Environment.GetEnvironmentVariable("Company") +"Purchase Header].[Shortcut Dimension 1 Code]='2071' or dbo."+ Environment.GetEnvironmentVariable("Company") +"Purchase Header].[Shortcut Dimension 1 Code]='2070' " +
        " or dbo."+ Environment.GetEnvironmentVariable("Company") +"Purchase Line].[Shortcut Dimension 1 Code]='1041' or dbo."+ Environment.GetEnvironmentVariable("Company") +"Purchase Line].[Shortcut Dimension 1 Code]='1040' " +
        " or dbo."+ Environment.GetEnvironmentVariable("Company") +"Purchase Header].[Shortcut Dimension 1 Code]='1041' or dbo."+ Environment.GetEnvironmentVariable("Company") +"Purchase Header].[Shortcut Dimension 1 Code]='1040') and dbo."+ Environment.GetEnvironmentVariable("Company") +"Purchase Line].[Document No_]<>'POPMKR-62060030'  " +
        "))  "
        + " ORDER BY dbo."+ Environment.GetEnvironmentVariable("Company") +"Purchase Header].[Document Date] DESC";


        //ViewBag.sql = queryData;
        var CheckPOPCJobtasks = _navcontext.v_CheckPOPCJobtasks.FromSqlRaw(queryData).ToList();

            var countdata = CheckPOPCJobtasks.Count;


            response = Ok(new { countdata = countdata });
            //response = Ok(new { data = CheckPOPCJobtasks });


            return response;
        }



        public IActionResult GetPoErrorAlert()
        {

            IActionResult response = Unauthorized();
            var queryData = " SELECT ROW_NUMBER() OVER (ORDER BY dbo." + Environment.GetEnvironmentVariable("Company") + "Purchase Header].[Document Date]) as ID,convert(varchar,dbo." + Environment.GetEnvironmentVariable("Company") +"Purchase Header].[Document Date] , 23) AS DocumentDate, "
        + " dbo."+ Environment.GetEnvironmentVariable("Company") +"Purchase Line].[Location Code] AS LocationCode,"
        + " dbo."+ Environment.GetEnvironmentVariable("Company") +"Purchase Line].[Document No_] AS DocumentNo,"
        + " dbo."+ Environment.GetEnvironmentVariable("Company") +"Purchase Line].No_ AS ItemNo,"
        + " dbo."+ Environment.GetEnvironmentVariable("Company") +"Purchase Line].Description AS Description,"
        + " dbo."+ Environment.GetEnvironmentVariable("Company") +"Purchase Line].[Description 2] AS Description2,"
        + " convert(varchar,FORMAT(dbo."+ Environment.GetEnvironmentVariable("Company") +"Purchase Line].Quantity,'###,###,###.00','en-US')) as Quantity,"
        + " convert(varchar,FORMAT(dbo."+ Environment.GetEnvironmentVariable("Company") +"Purchase Line].Amount,'###,###,###.00','en-US')) as Amount,"
        + " dbo."+ Environment.GetEnvironmentVariable("Company") +"Purchase Header].[Last Receiving No_] as LastReceive,"
        + " dbo."+ Environment.GetEnvironmentVariable("Company") +"Purchase Header].[Assigned User ID] as UserCreate"
        + " FROM dbo."+ Environment.GetEnvironmentVariable("Company") +"Purchase Header]"
        + " INNER JOIN dbo."+ Environment.GetEnvironmentVariable("Company") +"Purchase Line] ON dbo."+ Environment.GetEnvironmentVariable("Company") +"Purchase Line].[Document No_] = dbo."+ Environment.GetEnvironmentVariable("Company") +"Purchase Header].No_"
        + " WHERE (dbo."+ Environment.GetEnvironmentVariable("Company") +"Purchase Line].No_ LIKE 'PC%' AND dbo."+ Environment.GetEnvironmentVariable("Company") +"Purchase Line].[Job Task No_] <> '' " +
        "   and dbo."+ Environment.GetEnvironmentVariable("Company") +"Purchase Header].[Document Date]>'2019-03-01 00:00:00') " +
        " or  ((dbo."+ Environment.GetEnvironmentVariable("Company") +"Purchase Line].[Shortcut Dimension 1 Code]='2070' or dbo."+ Environment.GetEnvironmentVariable("Company") +"Purchase Line].[Shortcut Dimension 1 Code]='2071'  " +
        " or dbo."+ Environment.GetEnvironmentVariable("Company") +"Purchase Header].[Shortcut Dimension 1 Code]='2071' or dbo."+ Environment.GetEnvironmentVariable("Company") +"Purchase Header].[Shortcut Dimension 1 Code]='2070' " +
        " or dbo."+ Environment.GetEnvironmentVariable("Company") +"Purchase Line].[Shortcut Dimension 1 Code]='1041' or dbo."+ Environment.GetEnvironmentVariable("Company") +"Purchase Line].[Shortcut Dimension 1 Code]='1040' " +
        " or dbo."+ Environment.GetEnvironmentVariable("Company") +"Purchase Header].[Shortcut Dimension 1 Code]='1041' or dbo."+ Environment.GetEnvironmentVariable("Company") +"Purchase Header].[Shortcut Dimension 1 Code]='1040') and dbo."+ Environment.GetEnvironmentVariable("Company") +"Purchase Line].[Document No_]<>'POPMKR-62060030' " +
        ")  "
        + " ORDER BY dbo."+ Environment.GetEnvironmentVariable("Company") +"Purchase Header].[Document Date] DESC";


            //ViewBag.sql = queryData;
            var CheckPOPCJobtasks = _navcontext.v_CheckPOPCJobtasks.FromSqlRaw(queryData).ToList();

            var countdata = CheckPOPCJobtasks.Count;


            response = Ok(new { countdata = countdata });
            //response = Ok(new { data = CheckPOPCJobtasks });


            return response;



        }


        public IActionResult GetPoErrorLineAlert()
        {

            IActionResult response = Unauthorized();
            var queryData = " SELECT ROW_NUMBER() OVER (ORDER BY dbo." + Environment.GetEnvironmentVariable("Company") + "Purchase Header].[Document Date]) as ID,convert(varchar,dbo." + Environment.GetEnvironmentVariable("Company") +"Purchase Header].[Document Date] , 23) AS DocumentDate, "
        + " dbo."+ Environment.GetEnvironmentVariable("Company") +"Purchase Line].[Location Code] AS LocationCode,"
        + " dbo."+ Environment.GetEnvironmentVariable("Company") +"Purchase Line].[Document No_] AS DocumentNo,"
        + " dbo."+ Environment.GetEnvironmentVariable("Company") +"Purchase Line].No_ AS ItemNo,"
        + " dbo."+ Environment.GetEnvironmentVariable("Company") +"Purchase Line].Description AS Description,"
        + " dbo."+ Environment.GetEnvironmentVariable("Company") +"Purchase Line].[Description 2] AS Description2,"
        + " convert(varchar,FORMAT(dbo."+ Environment.GetEnvironmentVariable("Company") +"Purchase Line].Quantity,'###,###,###.00','en-US')) as Quantity,"
        + " convert(varchar,FORMAT(dbo."+ Environment.GetEnvironmentVariable("Company") +"Purchase Line].Amount,'###,###,###.00','en-US')) as Amount,"
        + " dbo."+ Environment.GetEnvironmentVariable("Company") +"Purchase Header].[Last Receiving No_] as LastReceive,"
        + " dbo."+ Environment.GetEnvironmentVariable("Company") +"Purchase Header].[Assigned User ID] as UserCreate"
        + " FROM dbo."+ Environment.GetEnvironmentVariable("Company") +"Purchase Header]"
        + " INNER JOIN dbo."+ Environment.GetEnvironmentVariable("Company") +"Purchase Line] ON dbo."+ Environment.GetEnvironmentVariable("Company") +"Purchase Line].[Document No_] = dbo."+ Environment.GetEnvironmentVariable("Company") +"Purchase Header].No_"
        + " WHERE (dbo."+ Environment.GetEnvironmentVariable("Company") +"Purchase Line].No_ LIKE 'PC%' AND dbo."+ Environment.GetEnvironmentVariable("Company") +"Purchase Line].[Job Task No_] <> '' " +
        "   and dbo."+ Environment.GetEnvironmentVariable("Company") +"Purchase Header].[Document Date]>'2019-03-01 00:00:00') " +
        " or  ((dbo."+ Environment.GetEnvironmentVariable("Company") +"Purchase Line].[Shortcut Dimension 1 Code]='2070' or dbo."+ Environment.GetEnvironmentVariable("Company") +"Purchase Line].[Shortcut Dimension 1 Code]='2071'  " +
        " or dbo."+ Environment.GetEnvironmentVariable("Company") +"Purchase Header].[Shortcut Dimension 1 Code]='2071' or dbo."+ Environment.GetEnvironmentVariable("Company") +"Purchase Header].[Shortcut Dimension 1 Code]='2070' " +
        " or dbo."+ Environment.GetEnvironmentVariable("Company") +"Purchase Line].[Shortcut Dimension 1 Code]='1041' or dbo."+ Environment.GetEnvironmentVariable("Company") +"Purchase Line].[Shortcut Dimension 1 Code]='1040' " +
        " or dbo."+ Environment.GetEnvironmentVariable("Company") +"Purchase Header].[Shortcut Dimension 1 Code]='1041' or dbo."+ Environment.GetEnvironmentVariable("Company") +"Purchase Header].[Shortcut Dimension 1 Code]='1040') and dbo."+ Environment.GetEnvironmentVariable("Company") +"Purchase Line].[Document No_]<>'POPMKR-62060030' " +
        ")  "
        + " ORDER BY dbo."+ Environment.GetEnvironmentVariable("Company") +"Purchase Header].[Document Date] DESC";


            //ViewBag.sql = queryData;
            var CheckPOPCJobtasks = _navcontext.v_CheckPOPCJobtasks.FromSqlRaw(queryData).ToList();

            //var countdata = CheckPOPCJobtasks.Count;


            response = Ok(CheckPOPCJobtasks);
            //response = Ok(new { data = CheckPOPCJobtasks });


            return response;

        }






    }
}