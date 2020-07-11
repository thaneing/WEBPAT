using System;
using System.Collections.Generic;
using System.Data.SqlClient;
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
    public class PurchaseVatController : BaseController
    {
        private readonly NAVContext _navcontext;
        private readonly DatabaseContext _context;

        public PurchaseVatController(NAVContext navcontext, DatabaseContext context)
        {
            _context = context;
            _navcontext = navcontext;
        }


        public IActionResult Index()
        {
            /*Check Session */
            var page = "186";
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

            ViewBag.StartDate = DateTime.Now.ToString("01/01/yyyy");
            ViewBag.EndDate = DateTime.Now.ToString("dd/MM/yyyy");

            return View();
        }


        public IActionResult GetData(string date1 ,string date2)
        {

            /*Check Session */
            var page = "186";
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
            var StartDate = date1.Substring(6, 4) + "-" + date1.Substring(3, 2) + "-" + date1.Substring(0, 2) + " 00:00:00";
            var EndDate = date2.Substring(6, 4) + "-" + date2.Substring(3, 2) + "-" + date2.Substring(0, 2) + " 23:59:59";

            var queryData = "SELECT " +
                "z.UseTax, " +
                "z.Entry, " +
                "z.PostingDate, " +
                "z.DocumentDate, " +
                "z.DocumentNo, " +
                "z.ExternalDocument, " +
                "z.CusVenNo, " +
                "z.CusVenName, " +
                "z.CusVenName2, " +
                "z.VATRegis, " +
                "z.HeadOffice, " +
                "z.Branch, " +
                "z.Base, " +
                "z.Amount, " +
                "z.Vat, " +
                "z.VatBusPostingGroup, " +
                "(SELECT top 1 c.DocumentName " +
                "FROM( " +
                "SELECT " +
                "a.DocumentNo, " +
                "(select top 1 [Document No_]  from dbo."+ Environment.GetEnvironmentVariable("Company") +"Detailed Vendor Ledg_ Entry] WHERE [Source Code]='PAYMENTJNL' and [Vendor Ledger Entry No_]=a.VendorLedgerEntry ORDER BY [Entry No_] desc) as DocumentName " +
                "FROM( " +
                "SELECT " +
                "convert(varchar, [Posting Date], 23)  as PostingDate, " +
                "dbo."+ Environment.GetEnvironmentVariable("Company") +"Detailed Vendor Ledg_ Entry].[Entry No_] AS EntryNo, " +
                "dbo."+ Environment.GetEnvironmentVariable("Company") +"Detailed Vendor Ledg_ Entry].[Vendor Ledger Entry No_] AS VendorLedgerEntry, " +
                "dbo."+ Environment.GetEnvironmentVariable("Company") +"Detailed Vendor Ledg_ Entry].[Document No_] AS DocumentNo, " +
                "[Vendor No_] AS VendorNo, " +
                "Name AS VendorName, " +
                "[Vendor Posting Group] AS VendorPostingGroup, " +
                "CASE  WHEN [Document Type]=2 THEN 'INVOICE' " +
                "WHEN [Document Type]=3 THEN 'CRATIDMEMO' END AS DocumentType, " +
                "convert(varchar,FORMAT(dbo."+ Environment.GetEnvironmentVariable("Company") +"Detailed Vendor Ledg_ Entry].Amount,'###,###,###.00','en-US')) as Amount, " +
                "convert(varchar,FORMAT(dbo."+ Environment.GetEnvironmentVariable("Company") +"Detailed Vendor Ledg_ Entry].[Amount (LCY)],'###,###,###.00','en-US')) AS AmountLCY, " +
                "dbo."+ Environment.GetEnvironmentVariable("Company") +"Detailed Vendor Ledg_ Entry].[User ID] as UserId, " +
                "dbo."+ Environment.GetEnvironmentVariable("Company") +"Detailed Vendor Ledg_ Entry].[Source Code] AS SourceCode, " +
                "convert(varchar,dbo."+ Environment.GetEnvironmentVariable("Company") +"Detailed Vendor Ledg_ Entry].[Initial Entry Due Date], 23) AS InitialEntryDueDate, " +
                "dbo."+ Environment.GetEnvironmentVariable("Company") +"Detailed Vendor Ledg_ Entry].[Initial Entry Global Dim_ 1] AS InitialEntryGlobalDim1, " +
                "dbo."+ Environment.GetEnvironmentVariable("Company") +"Detailed Vendor Ledg_ Entry].[Initial Entry Global Dim_ 2] AS InitialEntryGlobalDim2, " +
                "CASE  WHEN [Entry Type]=2 THEN 'Appcation' " +
                "WHEN [Entry Type]=1 THEN 'Initial Entry' END AS EntryType " +
                "FROM " +
                "dbo."+ Environment.GetEnvironmentVariable("Company") +"Detailed Vendor Ledg_ Entry] " +
                "INNER JOIN dbo."+ Environment.GetEnvironmentVariable("Company") +"Vendor] ON dbo."+ Environment.GetEnvironmentVariable("Company") +"Detailed Vendor Ledg_ Entry].[Vendor No_] = dbo."+ Environment.GetEnvironmentVariable("Company") +"Vendor].No_ " +
                "WHERE dbo."+ Environment.GetEnvironmentVariable("Company") +"Detailed Vendor Ledg_ Entry].[Source Code] IN ('PURCHAPPL', 'PURCHASES', 'PURCHJNL', 'JOBGLJNL'))as a  " +
                ") as c WHERE c.DocumentNo=z.DocumentNo " +
                ") as DocumentAP " +
                "FROM( " +
                "SELECT " +
                "dbo."+ Environment.GetEnvironmentVariable("Company") +"VAT Statement Entry].[Use Tax] as UseTax, " +
                "dbo."+ Environment.GetEnvironmentVariable("Company") +"VAT Statement Entry].[Entry No_] as Entry, " +
                "convert(varchar, [Posting Date],23) as PostingDate, " +
                "convert(varchar, dbo."+ Environment.GetEnvironmentVariable("Company") +"VAT Statement Entry].[Document Date],23) as DocumentDate, " +
                "dbo."+ Environment.GetEnvironmentVariable("Company") +"VAT Statement Entry].[Document No_] as DocumentNo, " +
                "dbo."+ Environment.GetEnvironmentVariable("Company") +"VAT Statement Entry].[External Document No_] as ExternalDocument, " +
                "dbo."+ Environment.GetEnvironmentVariable("Company") +"VAT Statement Entry].[Cust__Vend_ No_] as CusVenNo, " +
                "dbo."+ Environment.GetEnvironmentVariable("Company") +"VAT Statement Entry].[Cust__Vend_ Name] as CusVenName, " +
                "dbo."+ Environment.GetEnvironmentVariable("Company") +"VAT Statement Entry].[Cust__Vend_ Name 2] as CusVenName2, " +
                "dbo."+ Environment.GetEnvironmentVariable("Company") +"VAT Statement Entry].[VAT Registration No_] as VATRegis, " +
                "CASE  WHEN dbo."+ Environment.GetEnvironmentVariable("Company") +"VAT Statement Entry].[Head Office]=1 THEN 'Yes' " +
                "WHEN dbo."+ Environment.GetEnvironmentVariable("Company") +"VAT Statement Entry].[Head Office]=0 THEN 'No' END as HeadOffice, " +
                "dbo."+ Environment.GetEnvironmentVariable("Company") +"VAT Statement Entry].[Branch No_] as Branch, " +
                "dbo."+ Environment.GetEnvironmentVariable("Company") +"VAT Statement Entry].Base as Base, " +
                "dbo."+ Environment.GetEnvironmentVariable("Company") +"VAT Statement Entry].Amount as Amount, " +
                "dbo."+ Environment.GetEnvironmentVariable("Company") +"VAT Statement Entry].VAT_ as Vat, " +
                "dbo."+ Environment.GetEnvironmentVariable("Company") +"VAT Statement Entry].[VAT Bus_ Posting Group] as VatBusPostingGroup " +
                "FROM [dbo]."+ Environment.GetEnvironmentVariable("Company") + "VAT Statement Entry] WHERE [Entry Type]=1 and Posted=0 AND [dbo]." + Environment.GetEnvironmentVariable("Company") + "VAT Statement Entry].[VAT Prod_ Posting Group]='S7D' ) as z " +
                "WHERE z.PostingDate>={0} and z.PostingDate<={1}";



            //SqlParameter parameterStartdate = new SqlParameter("@startdate", StartDate);
            //SqlParameter parameterEndDate = new SqlParameter("@enddate", EndDate);
            //ViewBag.sql = queryData;
            var purchaseVats = _navcontext.PurchaseVats.FromSqlRaw(queryData, StartDate, EndDate).ToList();


            response = Ok(new { data = purchaseVats });


            return response;

        }
    }
}