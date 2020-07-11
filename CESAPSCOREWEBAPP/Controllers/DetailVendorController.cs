using System;
using System.Collections.Generic;
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
    public class DetailVendorController : BaseController
    {
        private readonly NAVContext _navcontext;
        private readonly DatabaseContext _context;

        public DetailVendorController(NAVContext navcontext, DatabaseContext context)
        {
            _context = context;
            _navcontext = navcontext;
        }
        public IActionResult Index()
        {

            /*Check Session */
            var page = "199";
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

        public IActionResult Gendata()
        {
            /*Check Session */
            var page = "199";
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

            //Query ALL GR JO PO

            var queryData = "SELECT ROW_NUMBER() OVER (ORDER BY c.GRNo) as ID, " +
                "c.GRNo,c.PostingGR,'' as JobType,c.JobGL,c.VendorName,c.VendorNo,'' as Type, " +
                "CASE WHEN (c.OrderNo='' and c.IV='') THEN c.APD " +
                "	   ELSE c.OrderNo END as OrderDoc, " +
                "(SELECT SUM(dbo."+ Environment.GetEnvironmentVariable("Company") +"Purch_ Rcpt_ Line].Quantity*dbo."+ Environment.GetEnvironmentVariable("Company") +"Purch_ Rcpt_ Line].[Unit Cost (LCY)]) FROM dbo."+ Environment.GetEnvironmentVariable("Company") +"Purch_ Rcpt_ Line] WHERE dbo."+ Environment.GetEnvironmentVariable("Company") +"Purch_ Rcpt_ Line].[Document No_]=c.GRNo) as ReceiveAmountLine " +
                " FROM ( " +
                "SELECT * " +
                ",(CASE WHEN b.OrderNo='' THEN  " +
                "(SELECT top 1 [dbo]."+ Environment.GetEnvironmentVariable("Company") +"Purch_ Inv_ Line].[Document No_] FROM [dbo]."+ Environment.GetEnvironmentVariable("Company") +"Purch_ Inv_ Line] WHERE [dbo]."+ Environment.GetEnvironmentVariable("Company") +"Purch_ Inv_ Line].[Posting Date]=b.PosDate  " +
                "and  [dbo]."+ Environment.GetEnvironmentVariable("Company") +"Purch_ Inv_ Line].[Direct Unit Cost]=b.TotalPrice and [dbo]."+ Environment.GetEnvironmentVariable("Company") +"Purch_ Inv_ Line].Description=b.Description and [dbo]."+ Environment.GetEnvironmentVariable("Company") +"Purch_ Inv_ Line].[Pay-to Vendor No_]=b.PayToVendor and  " +
                "[dbo]."+ Environment.GetEnvironmentVariable("Company") +"Purch_ Inv_ Line].[Dimension Set ID]=b.DimensionSetID) " +
                " ELSE '' END)  AS APD " +
                "FROM ( " +
                "SELECT *, " +
                "(SELECT Top 1 [Posting Date]  FROM dbo."+ Environment.GetEnvironmentVariable("Company") +"Purch_ Rcpt_ Line] WHERE dbo."+ Environment.GetEnvironmentVariable("Company") +"Purch_ Rcpt_ Line].[Document No_]=a.GRNo)  as PosDate, " +
                "(SELECT Top 1 dbo."+ Environment.GetEnvironmentVariable("Company") +"Purch_ Rcpt_ Line].[Pay-to Vendor No_] FROM dbo."+ Environment.GetEnvironmentVariable("Company") +"Purch_ Rcpt_ Line] WHERE dbo."+ Environment.GetEnvironmentVariable("Company") +"Purch_ Rcpt_ Line].[Document No_]=a.GRNo)  as PayToVendor, " +
                "(SELECT Top 1 dbo."+ Environment.GetEnvironmentVariable("Company") +"Purch_ Rcpt_ Line].[Direct Unit Cost] FROM dbo."+ Environment.GetEnvironmentVariable("Company") +"Purch_ Rcpt_ Line] WHERE dbo."+ Environment.GetEnvironmentVariable("Company") +"Purch_ Rcpt_ Line].[Document No_]=a.GRNo ORDER BY dbo."+ Environment.GetEnvironmentVariable("Company") +"Purch_ Rcpt_ Line].No_)  as TotalPrice, " +
                "(SELECT TOP 1 dbo."+ Environment.GetEnvironmentVariable("Company") +"Purch_ Rcpt_ Line].Description FROM dbo."+ Environment.GetEnvironmentVariable("Company") +"Purch_ Rcpt_ Line] WHERE dbo."+ Environment.GetEnvironmentVariable("Company") +"Purch_ Rcpt_ Line].[Document No_]=a.GRNo ORDER BY dbo."+ Environment.GetEnvironmentVariable("Company") +"Purch_ Rcpt_ Line].No_) as Description, " +
                "(SELECT TOP 1 dbo."+ Environment.GetEnvironmentVariable("Company") +"Purch_ Rcpt_ Line].[Dimension Set ID] FROM dbo."+ Environment.GetEnvironmentVariable("Company") +"Purch_ Rcpt_ Line] WHERE dbo."+ Environment.GetEnvironmentVariable("Company") +"Purch_ Rcpt_ Line].[Document No_]=a.GRNo ORDER BY dbo."+ Environment.GetEnvironmentVariable("Company") +"Purch_ Rcpt_ Line].No_) as DimensionSetID " +
                "FROM( " +
                "SELECT " +
                " dbo."+ Environment.GetEnvironmentVariable("Company") +"Purch_ Rcpt_ Header].No_ as GRNo, " +
                "CONVERT(VARCHAR,dbo."+ Environment.GetEnvironmentVariable("Company") +"Purch_ Rcpt_ Header].[Posting Date],23) as PostingGR, " +
                "CASE WHEN dbo."+ Environment.GetEnvironmentVariable("Company") +"Purch_ Rcpt_ Header].[Order No_] = '' THEN dbo."+ Environment.GetEnvironmentVariable("Company") +"Purch_ Rcpt_ Header].[Ref_ Job Order No_]  " +
                "		ELSE  dbo."+ Environment.GetEnvironmentVariable("Company") +"Purch_ Rcpt_ Header].[Order No_] END as OrderNo , " +
                "dbo."+ Environment.GetEnvironmentVariable("Company") +"Purch_ Rcpt_ Header].[Ref_ Job Order No_]  as IV, " +
                "dbo."+ Environment.GetEnvironmentVariable("Company") +"Purch_ Rcpt_ Header].[Shortcut Dimension 1 Code] as JobGL, " +
                "dbo."+ Environment.GetEnvironmentVariable("Company") +"Purch_ Rcpt_ Header].[Pay-to Vendor No_] as VendorNo, " +
                "dbo."+ Environment.GetEnvironmentVariable("Company") +"Purch_ Rcpt_ Header].[Pay-to Name] as VendorName " +
                "FROM dbo."+ Environment.GetEnvironmentVariable("Company") +"Purch_ Rcpt_ Header] " +
                ") as a " +
                ") as b " +
                ") as c  ";






            //var queryData = "SELECT ROW_NUMBER() OVER (ORDER BY dbo." + Environment.GetEnvironmentVariable("Company") + "Purch_ Rcpt_ Header].No_) as ID, " +
            //    "convert(varchar,dbo."+ Environment.GetEnvironmentVariable("Company") +"Purch_ Rcpt_ Header].[Posting Date],23) AS PostingGR, " +
            //    "dbo."+ Environment.GetEnvironmentVariable("Company") +"Purch_ Rcpt_ Header].No_ as GRNo, " +
            //    "CASE WHEN dbo."+ Environment.GetEnvironmentVariable("Company") +"Purch_ Rcpt_ Header].[Job Order Type] =0 then 'Purchase'  " +
            //    "			ELSE 'JobOrder' END AS JobType, " +
            //    "dbo."+ Environment.GetEnvironmentVariable("Company") +"Purch_ Rcpt_ Line].[Order No_] as OrderDoc, " +
            //    "dbo."+ Environment.GetEnvironmentVariable("Company") +"Purch_ Rcpt_ Header].[Shortcut Dimension 1 Code] as JobGL, " +
            //    "Sum(dbo."+ Environment.GetEnvironmentVariable("Company") +"Purch_ Rcpt_ Line].Quantity*dbo."+ Environment.GetEnvironmentVariable("Company") +"Purch_ Rcpt_ Line].[Unit Cost (LCY)]) AS ReceiveAmountLine," +
            //    "dbo."+ Environment.GetEnvironmentVariable("Company") +"Purch_ Rcpt_ Header].[Pay-to Vendor No_] as VendorNo, " +
            //    "dbo."+ Environment.GetEnvironmentVariable("Company") +"Purch_ Rcpt_ Header].[Pay-to Name] as VendorName," +
            //    "(CASE WHEN dbo."+ Environment.GetEnvironmentVariable("Company") +"Purch_ Rcpt_ Line].Type=0 THEN '' " +
            //    "    WHEN dbo."+ Environment.GetEnvironmentVariable("Company") +"Purch_ Rcpt_ Line].Type=1 THEN 'G/L' " +
            //    "    WHEN dbo."+ Environment.GetEnvironmentVariable("Company") +"Purch_ Rcpt_ Line].Type=2 THEN 'ITEM' " +
            //    "    WHEN dbo."+ Environment.GetEnvironmentVariable("Company") +"Purch_ Rcpt_ Line].Type=3 THEN 'FixAsset' " +
            //    "    WHEN dbo."+ Environment.GetEnvironmentVariable("Company") +"Purch_ Rcpt_ Line].Type=4 THEN 'Charge' END) as Type "+
            //"FROM " +
            //    "dbo."+ Environment.GetEnvironmentVariable("Company") +"Purch_ Rcpt_ Line] " +
            //    "LEFT JOIN dbo."+ Environment.GetEnvironmentVariable("Company") +"Purch_ Rcpt_ Header] ON dbo."+ Environment.GetEnvironmentVariable("Company") +"Purch_ Rcpt_ Header].No_ = dbo."+ Environment.GetEnvironmentVariable("Company") +"Purch_ Rcpt_ Line].[Document No_] " +
            //    "WHERE " +
            //    "dbo."+ Environment.GetEnvironmentVariable("Company") +"Purch_ Rcpt_ Line].Type != 0  " +
            //    //"and dbo."+ Environment.GetEnvironmentVariable("Company") +"Purch_ Rcpt_ Line].[Order No_]!=''" +
            //    "GROUP BY " +
            //    "dbo."+ Environment.GetEnvironmentVariable("Company") +"Purch_ Rcpt_ Header].[Posting Date], " +
            //    "dbo."+ Environment.GetEnvironmentVariable("Company") +"Purch_ Rcpt_ Header].No_, " +
            //    "dbo."+ Environment.GetEnvironmentVariable("Company") +"Purch_ Rcpt_ Line].[Document No_], " +
            //    "dbo."+ Environment.GetEnvironmentVariable("Company") +"Purch_ Rcpt_ Header].[Job Order Type], " +
            //    "dbo."+ Environment.GetEnvironmentVariable("Company") +"Purch_ Rcpt_ Header].[Job Document Type], " +
            //    "dbo."+ Environment.GetEnvironmentVariable("Company") +"Purch_ Rcpt_ Line].[Order No_], " +
            //    "dbo."+ Environment.GetEnvironmentVariable("Company") +"Purch_ Rcpt_ Header].[Shortcut Dimension 1 Code]," +
            //    "dbo."+ Environment.GetEnvironmentVariable("Company") +"Purch_ Rcpt_ Header].[Pay-to Vendor No_], " +
            //    "dbo."+ Environment.GetEnvironmentVariable("Company") +"Purch_ Rcpt_ Header].[Pay-to Name]," +
            //    "dbo."+ Environment.GetEnvironmentVariable("Company") +"Purch_ Rcpt_ Line].Type";


            var GRReceipts = _navcontext.GRReceipts.FromSqlRaw(queryData).ToList();
            var queryData2 = "SELECT ROW_NUMBER() OVER (ORDER BY dbo." + Environment.GetEnvironmentVariable("Company") + "Detailed Vendor Ledg_ Entry].[Document No_] ) as ID," +
                "CONVERT(VARCHAR,dbo."+ Environment.GetEnvironmentVariable("Company") +"Detailed Vendor Ledg_ Entry].[Posting Date],23) AS PostingIV, " +
                "dbo."+ Environment.GetEnvironmentVariable("Company") +"Detailed Vendor Ledg_ Entry].[Document No_] AS DocAP, " +
                "dbo."+ Environment.GetEnvironmentVariable("Company") +"Detailed Vendor Ledg_ Entry].Amount, " +
                "dbo."+ Environment.GetEnvironmentVariable("Company") +"Detailed Vendor Ledg_ Entry].[Vendor No_] AS VendorNo, " +
                "dbo."+ Environment.GetEnvironmentVariable("Company") +"Detailed Vendor Ledg_ Entry].[Initial Entry Global Dim_ 1] AS JobGL, " +
                "dbo."+ Environment.GetEnvironmentVariable("Company") +"Purch_ Inv_ Line].[Receipt No_] AS DocGR, " +
                "dbo."+ Environment.GetEnvironmentVariable("Company") +"Purch_ Inv_ Header].[Ref_ Receipt No_] AS DocGR1, " +
                "dbo."+ Environment.GetEnvironmentVariable("Company") +"Purch_ Inv_ Header].[Pay-to Name] AS VendorName, " +
                "isnull(dbo."+ Environment.GetEnvironmentVariable("Company") +"Purch_ Inv_ Header].[Progress Term],0) as PrograssTerm " +
                "FROM " +
                "dbo."+ Environment.GetEnvironmentVariable("Company") +"Detailed Vendor Ledg_ Entry] " +
                "INNER JOIN dbo."+ Environment.GetEnvironmentVariable("Company") +"Purch_ Inv_ Header] ON dbo."+ Environment.GetEnvironmentVariable("Company") +"Detailed Vendor Ledg_ Entry].[Vendor Ledger Entry No_] = dbo."+ Environment.GetEnvironmentVariable("Company") +"Purch_ Inv_ Header].[Vendor Ledger Entry No_] " +
                "INNER JOIN dbo."+ Environment.GetEnvironmentVariable("Company") +"Purch_ Inv_ Line] ON dbo."+ Environment.GetEnvironmentVariable("Company") +"Purch_ Inv_ Header].No_ = dbo."+ Environment.GetEnvironmentVariable("Company") +"Purch_ Inv_ Line].[Document No_] " +
                "WHERE " +
                //"dbo."+ Environment.GetEnvironmentVariable("Company") +"Purch_ Inv_ Line].[Receipt No_]!='' and " +
                " [Entry Type]=1 " +
                "group by  " +
                "dbo."+ Environment.GetEnvironmentVariable("Company") +"Detailed Vendor Ledg_ Entry].[Vendor Ledger Entry No_], " +
                "dbo."+ Environment.GetEnvironmentVariable("Company") +"Detailed Vendor Ledg_ Entry].[Entry Type], " +
                "dbo."+ Environment.GetEnvironmentVariable("Company") +"Detailed Vendor Ledg_ Entry].[Posting Date], " +
                "dbo."+ Environment.GetEnvironmentVariable("Company") +"Detailed Vendor Ledg_ Entry].[Document No_], " +
                "dbo."+ Environment.GetEnvironmentVariable("Company") +"Detailed Vendor Ledg_ Entry].Amount, " +
                "dbo."+ Environment.GetEnvironmentVariable("Company") +"Detailed Vendor Ledg_ Entry].[Vendor No_], " +
                "dbo."+ Environment.GetEnvironmentVariable("Company") +"Detailed Vendor Ledg_ Entry].[Initial Entry Global Dim_ 1], " +
                "dbo."+ Environment.GetEnvironmentVariable("Company") +"Purch_ Inv_ Header].[Vendor Ledger Entry No_], " +
                "dbo."+ Environment.GetEnvironmentVariable("Company") +"Purch_ Inv_ Line].[Receipt No_], " +
                "dbo."+ Environment.GetEnvironmentVariable("Company") +"Purch_ Inv_ Header].[Ref_ Receipt No_], " +
                "dbo."+ Environment.GetEnvironmentVariable("Company") +"Purch_ Inv_ Header].[Pay-to Name], " +
                "dbo."+ Environment.GetEnvironmentVariable("Company") +"Purch_ Inv_ Header].[Progress Term] " +
                "order by dbo."+ Environment.GetEnvironmentVariable("Company") +"Detailed Vendor Ledg_ Entry].[Document No_] ";


             //var queryData2 = "SELECT " +
             //    "CONVERT(VARCHAR,dbo."+ Environment.GetEnvironmentVariable("Company") +"Detailed Vendor Ledg_ Entry].[Posting Date],23) AS PostingIV, " +
             //    "dbo."+ Environment.GetEnvironmentVariable("Company") +"Detailed Vendor Ledg_ Entry].[Document No_] AS DocAP, " +
             //    "dbo."+ Environment.GetEnvironmentVariable("Company") +"Detailed Vendor Ledg_ Entry].[Initial Entry Global Dim_ 1] AS JobGL, " +
             //    "dbo."+ Environment.GetEnvironmentVariable("Company") +"Detailed Vendor Ledg_ Entry].Amount, " +
             //    "dbo."+ Environment.GetEnvironmentVariable("Company") +"Purch_ Inv_ Header].[Ref_ Job Order No_] AS OrderDoc, " +
             //    "dbo."+ Environment.GetEnvironmentVariable("Company") +"Purch_ Inv_ Header].[Ref_ Receipt No_] AS DocGR, " +
             //    "dbo."+ Environment.GetEnvironmentVariable("Company") +"Purch_ Inv_ Header].[Pay-to Vendor No_] AS VendorNo, " +
             //    "dbo."+ Environment.GetEnvironmentVariable("Company") +"Purch_ Inv_ Header].[Pay-to Name] AS VendorName, " +
             //    "ISNULL(dbo."+ Environment.GetEnvironmentVariable("Company") +"Purch_ Inv_ Header].[Progress Term],0) as PrograssTerm " +
             //    "FROM " +
             //    "dbo."+ Environment.GetEnvironmentVariable("Company") +"Detailed Vendor Ledg_ Entry] " +
             //    "LEFT JOIN dbo."+ Environment.GetEnvironmentVariable("Company") +"Vendor Ledger Entry] ON dbo."+ Environment.GetEnvironmentVariable("Company") +"Detailed Vendor Ledg_ Entry].[Vendor Ledger Entry No_] = dbo."+ Environment.GetEnvironmentVariable("Company") +"Vendor Ledger Entry].[Entry No_] " +
             //    "LEFT JOIN dbo."+ Environment.GetEnvironmentVariable("Company") +"Purch_ Inv_ Header] ON dbo."+ Environment.GetEnvironmentVariable("Company") +"Vendor Ledger Entry].[Document No_] = dbo."+ Environment.GetEnvironmentVariable("Company") +"Purch_ Inv_ Header].No_ " +
             //    "WHERE " +
             //    "dbo."+ Environment.GetEnvironmentVariable("Company") +"Detailed Vendor Ledg_ Entry].[Entry Type] = 1 " +
             //    "GROUP BY " +
             //    "dbo."+ Environment.GetEnvironmentVariable("Company") +"Detailed Vendor Ledg_ Entry].[Posting Date], " +
             //    "dbo."+ Environment.GetEnvironmentVariable("Company") +"Detailed Vendor Ledg_ Entry].[Document No_], " +
             //    "dbo."+ Environment.GetEnvironmentVariable("Company") +"Detailed Vendor Ledg_ Entry].[Vendor No_], " +
             //    "dbo."+ Environment.GetEnvironmentVariable("Company") +"Detailed Vendor Ledg_ Entry].Amount, " +
             //    "dbo."+ Environment.GetEnvironmentVariable("Company") +"Detailed Vendor Ledg_ Entry].[Initial Entry Global Dim_ 1], " +
             //    "dbo."+ Environment.GetEnvironmentVariable("Company") +"Vendor Ledger Entry].[Document No_], " +
             //    "dbo."+ Environment.GetEnvironmentVariable("Company") +"Purch_ Inv_ Header].[Ref_ Job Order No_], " +
             //    "dbo."+ Environment.GetEnvironmentVariable("Company") +"Purch_ Inv_ Header].[Ref_ Receipt No_], " +
             //    "dbo."+ Environment.GetEnvironmentVariable("Company") +"Purch_ Inv_ Header].[Pay-to Vendor No_], " +
             //    "dbo."+ Environment.GetEnvironmentVariable("Company") +"Purch_ Inv_ Header].[Pay-to Name], " +
             //    "dbo."+ Environment.GetEnvironmentVariable("Company") +"Purch_ Inv_ Header].[Progress Term] ";



            ////Query All AP
            //var queryData2 = "SELECT  " +
            //    "CONVERT(VARCHAR,dbo."+ Environment.GetEnvironmentVariable("Company") +"Purch_ Rcpt_ Header].[Posting Date],23) AS PostingGR, " +
            //    "dbo."+ Environment.GetEnvironmentVariable("Company") +"Purch_ Rcpt_ Header].No_ as DocGR, " +
            //    "CONVERT(VARCHAR,dbo."+ Environment.GetEnvironmentVariable("Company") +"Purch_ Inv_ Header].[Posting Date] ,23) AS PostingIV, " +
            //    "dbo."+ Environment.GetEnvironmentVariable("Company") +"Purch_ Inv_ Line].[Document No_] as DocAP, " +
            //    "dbo."+ Environment.GetEnvironmentVariable("Company") +"Purch_ Rcpt_ Header].[Order No_] as OrderDoc, " +
            //    "dbo."+ Environment.GetEnvironmentVariable("Company") +"Purch_ Rcpt_ Header].[Pay-to Vendor No_] as VendorNo, " +
            //    "dbo."+ Environment.GetEnvironmentVariable("Company") +"Purch_ Rcpt_ Header].[Pay-to Name] as VendorName, " +
            //    "dbo."+ Environment.GetEnvironmentVariable("Company") +"Purch_ Rcpt_ Header].[Shortcut Dimension 1 Code] as JobGL, " +
            //    "dbo."+ Environment.GetEnvironmentVariable("Company") +"Purch_ Rcpt_ Header].[Progress Term] as PrograssTerm, " +
            //    "dbo."+ Environment.GetEnvironmentVariable("Company") +"Detailed Vendor Ledg_ Entry].Amount " +
            //    "FROM " +
            //    "dbo."+ Environment.GetEnvironmentVariable("Company") +"Purch_ Rcpt_ Header] " +
            //    "LEFT JOIN dbo."+ Environment.GetEnvironmentVariable("Company") +"Purch_ Inv_ Line] ON dbo."+ Environment.GetEnvironmentVariable("Company") +"Purch_ Rcpt_ Header].No_ = dbo."+ Environment.GetEnvironmentVariable("Company") +"Purch_ Inv_ Line].[Receipt No_] " +
            //    "LEFT JOIN dbo."+ Environment.GetEnvironmentVariable("Company") +"Purch_ Inv_ Header] ON dbo."+ Environment.GetEnvironmentVariable("Company") +"Purch_ Inv_ Line].[Document No_] = dbo."+ Environment.GetEnvironmentVariable("Company") +"Purch_ Inv_ Header].No_ " +
            //    "LEFT JOIN dbo."+ Environment.GetEnvironmentVariable("Company") +"Detailed Vendor Ledg_ Entry] ON dbo."+ Environment.GetEnvironmentVariable("Company") +"Purch_ Inv_ Header].[Vendor Ledger Entry No_] = dbo."+ Environment.GetEnvironmentVariable("Company") +"Detailed Vendor Ledg_ Entry].[Vendor Ledger Entry No_] " +
            //    "LEFT JOIN dbo."+ Environment.GetEnvironmentVariable("Company") +"Purch_ Rcpt_ Line] ON dbo."+ Environment.GetEnvironmentVariable("Company") +"Purch_ Rcpt_ Header].No_ = dbo."+ Environment.GetEnvironmentVariable("Company") +"Purch_ Rcpt_ Line].[Document No_] " +
            //    "WHERE " +
            //    "dbo."+ Environment.GetEnvironmentVariable("Company") +"Detailed Vendor Ledg_ Entry].[Source Code] != 'PAYMENTJNL' " +
            //    "GROUP BY " +
            //    "dbo."+ Environment.GetEnvironmentVariable("Company") +"Purch_ Rcpt_ Header].[Posting Date], " +
            //    "dbo."+ Environment.GetEnvironmentVariable("Company") +"Purch_ Rcpt_ Header].No_, " +
            //    "dbo."+ Environment.GetEnvironmentVariable("Company") +"Purch_ Inv_ Header].[Posting Date], " +
            //    "dbo."+ Environment.GetEnvironmentVariable("Company") +"Purch_ Inv_ Line].[Document No_], " +
            //    "dbo."+ Environment.GetEnvironmentVariable("Company") +"Purch_ Rcpt_ Header].[Order No_], " +
            //    "dbo."+ Environment.GetEnvironmentVariable("Company") +"Purch_ Rcpt_ Header].[Buy-from Vendor No_], " +
            //    "dbo."+ Environment.GetEnvironmentVariable("Company") +"Purch_ Rcpt_ Header].[Pay-to Vendor No_], " +
            //    "dbo."+ Environment.GetEnvironmentVariable("Company") +"Purch_ Rcpt_ Header].[Pay-to Name], " +
            //    "dbo."+ Environment.GetEnvironmentVariable("Company") +"Purch_ Rcpt_ Header].[Shortcut Dimension 1 Code], " +
            //    "dbo."+ Environment.GetEnvironmentVariable("Company") +"Purch_ Rcpt_ Header].[Progress Term], " +
            //    "dbo."+ Environment.GetEnvironmentVariable("Company") +"Detailed Vendor Ledg_ Entry].Amount, " +
            //    "dbo."+ Environment.GetEnvironmentVariable("Company") +"Purch_ Rcpt_ Line].[Order No_] ";

            var APdocs = _navcontext.APDocs.FromSqlRaw(queryData2).ToList();



            //Query All PR
            var queryData1 = "SELECT DISTINCT " +
                "dbo."+ Environment.GetEnvironmentVariable("Company") +"Posted Requisition Line].[Purch_ Order No_] as name, " +
                "dbo."+ Environment.GetEnvironmentVariable("Company") +"Posted Requisition Line].[Document No_] as code " +
                "FROM [dbo]."+ Environment.GetEnvironmentVariable("Company") +"Posted Requisition Line] " +
                "WHERE Type=2  " +
                "order by [Purch_ Order No_] ";

            var sourceAutoCompletes = _navcontext.sourceAutoCompletes.FromSqlRaw(queryData1).ToList();
            var pr = "";

            List<DetailVendor> instances = new List<DetailVendor>();
            DetailVendor current = null;

            List<APDoc> APs;
           

            foreach (var std in GRReceipts as IList<GRReceipt>)
            {
                current = new DetailVendor();
                current.GRDate = std.PostingGR;
                current.DocumentReceipt = std.GRNo;
                current.RefPOJO = std.OrderDoc;
                current.AmountReceipt = std.ReceiveAmountLine;
                current.PayToVendorNo = std.VendorNo;
                current.PayToVendorName = std.VendorName;
                current.JobGL = std.JobGL;
                current.Type = std.Type;

                //Query AP With GRNo
                APs =APdocs.Where(c=>c.DocGR==std.GRNo || c.DocGR1 == std.GRNo).ToList();

                
                foreach (var ap  in APs as IList<APDoc>)
                {
                    current.DocumentInv = ap.DocAP;
                    current.PostingDate = ap.PostingIV;
                    current.AmountVat = ap.Amount;
                    current.PrograssTerm = ap.PrograssTerm;

                }

       



               pr = "";
                if (std.OrderDoc != "") { 
                   List<SourceAutoComplete>data = sourceAutoCompletes.Where(c => c.name == std.OrderDoc).ToList();
                    var doctmp = "";
                    foreach(var stdpr in data as IList<SourceAutoComplete>)
                    {
                       
                        if (doctmp == stdpr.code)
                        {

                        }
                        else
                        {
                           doctmp = stdpr.code;
                           pr += stdpr.code + "<br>";
                        }
                       
                    }
                }
                if (pr.Length > 0) { 
                 pr=pr.Substring(0,pr.Length-1);
                }

                current.RefPR = pr;

                instances.Add(current);
            }




                response = Ok(new { data = instances });


            return response;



          
        }



        public IActionResult GRNoAP()
        {

            /*Check Session */
            var page = "254";
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


            var queryData1 = "SELECT dbo." + Environment.GetEnvironmentVariable("Company") + "Location].Code AS name,dbo." + Environment.GetEnvironmentVariable("Company") + "Location].Name as code FROM [dbo]." + Environment.GetEnvironmentVariable("Company") + "Location] order by dbo." + Environment.GetEnvironmentVariable("Company") + "Location].Code asc ";
            var sourceAutoCompletes = _navcontext.sourceAutoCompletes.FromSqlRaw(queryData1).ToList();
            ViewData["SourceAutoCompletes"] = sourceAutoCompletes;

            return View();
        }






        public IActionResult GendataNoAP(string site,string TypeData)
        {
            /*Check Session */
            var page = "254";
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
            List<GRReceipt> GRReceipts = new List<GRReceipt>();
            var typedoc = "";
            //Query ALL GR JO PO
            if (TypeData == "0")
            {
                typedoc = "WHERE [Job Document Type]=0";
            }
            else if(TypeData=="1")
            {
                typedoc = "WHERE [Job Document Type]=1";

            }
            else
            {
                typedoc = "";
            }



            if (site != null) {
                var queryData = "" +
                    "SELECT * FROM (" +
                    "SELECT ROW_NUMBER() OVER (ORDER BY c.GRNo) as ID, " +
                    "c.GRNo,c.PostingGR,'' as JobType,c.JobGL,c.VendorName,c.VendorNo," +
                    "c.Type as Type, " +
                    "CASE WHEN (c.OrderNo='' and c.IV='') THEN c.APD " +
                    "	   ELSE c.OrderNo END as OrderDoc, " +
                    "(SELECT SUM(dbo."+ Environment.GetEnvironmentVariable("Company") +"Purch_ Rcpt_ Line].Quantity*dbo."+ Environment.GetEnvironmentVariable("Company") +"Purch_ Rcpt_ Line].[Unit Cost (LCY)]) FROM dbo."+ Environment.GetEnvironmentVariable("Company") +"Purch_ Rcpt_ Line] WHERE dbo."+ Environment.GetEnvironmentVariable("Company") +"Purch_ Rcpt_ Line].[Document No_]=c.GRNo) as ReceiveAmountLine " +
                    " FROM ( " +
                    "SELECT * " +
                    ",(CASE WHEN b.OrderNo='' THEN  " +
                    "(SELECT top 1 [dbo]."+ Environment.GetEnvironmentVariable("Company") +"Purch_ Inv_ Line].[Document No_] FROM [dbo]."+ Environment.GetEnvironmentVariable("Company") +"Purch_ Inv_ Line] WHERE [dbo]."+ Environment.GetEnvironmentVariable("Company") +"Purch_ Inv_ Line].[Posting Date]=b.PosDate  " +
                    "and  [dbo]."+ Environment.GetEnvironmentVariable("Company") +"Purch_ Inv_ Line].[Direct Unit Cost]=b.TotalPrice and [dbo]."+ Environment.GetEnvironmentVariable("Company") +"Purch_ Inv_ Line].Description=b.Description and [dbo]."+ Environment.GetEnvironmentVariable("Company") +"Purch_ Inv_ Line].[Pay-to Vendor No_]=b.PayToVendor and  " +
                    "[dbo]."+ Environment.GetEnvironmentVariable("Company") +"Purch_ Inv_ Line].[Dimension Set ID]=b.DimensionSetID) " +
                    " ELSE '' END)  AS APD " +
                    "FROM ( " +
                    "SELECT *, " +
                    "(SELECT Top 1 [Posting Date]  FROM dbo."+ Environment.GetEnvironmentVariable("Company") +"Purch_ Rcpt_ Line] WHERE dbo."+ Environment.GetEnvironmentVariable("Company") +"Purch_ Rcpt_ Line].[Document No_]=a.GRNo)  as PosDate, " +
                    "(SELECT Top 1 dbo."+ Environment.GetEnvironmentVariable("Company") +"Purch_ Rcpt_ Line].[Pay-to Vendor No_] FROM dbo."+ Environment.GetEnvironmentVariable("Company") +"Purch_ Rcpt_ Line] WHERE dbo."+ Environment.GetEnvironmentVariable("Company") +"Purch_ Rcpt_ Line].[Document No_]=a.GRNo)  as PayToVendor, " +
                    "(SELECT Top 1 dbo."+ Environment.GetEnvironmentVariable("Company") +"Purch_ Rcpt_ Line].[Direct Unit Cost] FROM dbo."+ Environment.GetEnvironmentVariable("Company") +"Purch_ Rcpt_ Line] WHERE dbo."+ Environment.GetEnvironmentVariable("Company") +"Purch_ Rcpt_ Line].[Document No_]=a.GRNo ORDER BY dbo."+ Environment.GetEnvironmentVariable("Company") +"Purch_ Rcpt_ Line].No_)  as TotalPrice, " +
                    "(SELECT TOP 1 dbo."+ Environment.GetEnvironmentVariable("Company") +"Purch_ Rcpt_ Line].Description FROM dbo."+ Environment.GetEnvironmentVariable("Company") +"Purch_ Rcpt_ Line] WHERE dbo."+ Environment.GetEnvironmentVariable("Company") +"Purch_ Rcpt_ Line].[Document No_]=a.GRNo ORDER BY dbo."+ Environment.GetEnvironmentVariable("Company") +"Purch_ Rcpt_ Line].No_) as Description, " +
                    "(SELECT TOP 1 dbo."+ Environment.GetEnvironmentVariable("Company") +"Purch_ Rcpt_ Line].[Dimension Set ID] FROM dbo."+ Environment.GetEnvironmentVariable("Company") +"Purch_ Rcpt_ Line] WHERE dbo."+ Environment.GetEnvironmentVariable("Company") +"Purch_ Rcpt_ Line].[Document No_]=a.GRNo ORDER BY dbo."+ Environment.GetEnvironmentVariable("Company") +"Purch_ Rcpt_ Line].No_) as DimensionSetID " +
                    "FROM( " +
                    "SELECT " +
                    " dbo."+ Environment.GetEnvironmentVariable("Company") +"Purch_ Rcpt_ Header].No_ as GRNo, " +
                    "CONVERT(VARCHAR,dbo."+ Environment.GetEnvironmentVariable("Company") +"Purch_ Rcpt_ Header].[Posting Date],23) as PostingGR, " +

                    "dbo."+ Environment.GetEnvironmentVariable("Company") +"Purch_ Rcpt_ Header].[Order No_]  as OrderNo, "+
                     "dbo."+ Environment.GetEnvironmentVariable("Company") +"Purch_ Rcpt_ Header].[Ref_ Job Order No_]  as IV, " +
                    "dbo."+ Environment.GetEnvironmentVariable("Company") +"Purch_ Rcpt_ Header].[Shortcut Dimension 1 Code] as JobGL, " +
                    "dbo."+ Environment.GetEnvironmentVariable("Company") +"Purch_ Rcpt_ Header].[Pay-to Vendor No_] as VendorNo, " +
                    "dbo."+ Environment.GetEnvironmentVariable("Company") +"Purch_ Rcpt_ Header].[Pay-to Name] as VendorName," +
                    "dbo."+ Environment.GetEnvironmentVariable("Company") +"Purch_ Rcpt_ Header].[Vendor Shipment No_] as Type  " +
                    "FROM dbo."+ Environment.GetEnvironmentVariable("Company") +"Purch_ Rcpt_ Header] " + typedoc +
                    ") as a " +
                    ") as b " +
                    ") as c " +
                    ")as d " +
                    "WHERE d.JobGL={0} and d.OrderDoc Not Like 'AP%' and d.ReceiveAmountLine<>0 and d.OrderDoc<>''";



                     GRReceipts = _navcontext.GRReceipts.FromSqlRaw(queryData,site).ToList();


            }
            else
            {
                var queryData = "" +
                  "SELECT * FROM (" +
                  "SELECT ROW_NUMBER() OVER (ORDER BY c.GRNo) as ID, " +
                  "c.GRNo,c.PostingGR,'' as JobType,c.JobGL,c.VendorName,c.VendorNo,c.Type as Type, " +
                  "CASE WHEN (c.OrderNo='' and c.IV='') THEN c.APD " +
                  "	   ELSE c.OrderNo END as OrderDoc, " +
                  "(SELECT SUM(dbo."+ Environment.GetEnvironmentVariable("Company") +"Purch_ Rcpt_ Line].Quantity*dbo."+ Environment.GetEnvironmentVariable("Company") +"Purch_ Rcpt_ Line].[Unit Cost (LCY)]) FROM dbo."+ Environment.GetEnvironmentVariable("Company") +"Purch_ Rcpt_ Line] WHERE dbo."+ Environment.GetEnvironmentVariable("Company") +"Purch_ Rcpt_ Line].[Document No_]=c.GRNo) as ReceiveAmountLine " +
                  " FROM ( " +
                  "SELECT * " +
                  ",(CASE WHEN b.OrderNo='' THEN  " +
                  "(SELECT top 1 [dbo]."+ Environment.GetEnvironmentVariable("Company") +"Purch_ Inv_ Line].[Document No_] FROM [dbo]."+ Environment.GetEnvironmentVariable("Company") +"Purch_ Inv_ Line] WHERE [dbo]."+ Environment.GetEnvironmentVariable("Company") +"Purch_ Inv_ Line].[Posting Date]=b.PosDate  " +
                  "and  [dbo]."+ Environment.GetEnvironmentVariable("Company") +"Purch_ Inv_ Line].[Direct Unit Cost]=b.TotalPrice and [dbo]."+ Environment.GetEnvironmentVariable("Company") +"Purch_ Inv_ Line].Description=b.Description and [dbo]."+ Environment.GetEnvironmentVariable("Company") +"Purch_ Inv_ Line].[Pay-to Vendor No_]=b.PayToVendor and  " +
                  "[dbo]."+ Environment.GetEnvironmentVariable("Company") +"Purch_ Inv_ Line].[Dimension Set ID]=b.DimensionSetID) " +
                  " ELSE '' END)  AS APD " +
                  "FROM ( " +
                  "SELECT *, " +
                  "(SELECT Top 1 [Posting Date]  FROM dbo."+ Environment.GetEnvironmentVariable("Company") +"Purch_ Rcpt_ Line] WHERE dbo."+ Environment.GetEnvironmentVariable("Company") +"Purch_ Rcpt_ Line].[Document No_]=a.GRNo)  as PosDate, " +
                  "(SELECT Top 1 dbo."+ Environment.GetEnvironmentVariable("Company") +"Purch_ Rcpt_ Line].[Pay-to Vendor No_] FROM dbo."+ Environment.GetEnvironmentVariable("Company") +"Purch_ Rcpt_ Line] WHERE dbo."+ Environment.GetEnvironmentVariable("Company") +"Purch_ Rcpt_ Line].[Document No_]=a.GRNo)  as PayToVendor, " +
                  "(SELECT Top 1 dbo."+ Environment.GetEnvironmentVariable("Company") +"Purch_ Rcpt_ Line].[Direct Unit Cost] FROM dbo."+ Environment.GetEnvironmentVariable("Company") +"Purch_ Rcpt_ Line] WHERE dbo."+ Environment.GetEnvironmentVariable("Company") +"Purch_ Rcpt_ Line].[Document No_]=a.GRNo ORDER BY dbo."+ Environment.GetEnvironmentVariable("Company") +"Purch_ Rcpt_ Line].No_)  as TotalPrice, " +
                  "(SELECT TOP 1 dbo."+ Environment.GetEnvironmentVariable("Company") +"Purch_ Rcpt_ Line].Description FROM dbo."+ Environment.GetEnvironmentVariable("Company") +"Purch_ Rcpt_ Line] WHERE dbo."+ Environment.GetEnvironmentVariable("Company") +"Purch_ Rcpt_ Line].[Document No_]=a.GRNo ORDER BY dbo."+ Environment.GetEnvironmentVariable("Company") +"Purch_ Rcpt_ Line].No_) as Description, " +
                  "(SELECT TOP 1 dbo."+ Environment.GetEnvironmentVariable("Company") +"Purch_ Rcpt_ Line].[Dimension Set ID] FROM dbo."+ Environment.GetEnvironmentVariable("Company") +"Purch_ Rcpt_ Line] WHERE dbo."+ Environment.GetEnvironmentVariable("Company") +"Purch_ Rcpt_ Line].[Document No_]=a.GRNo ORDER BY dbo."+ Environment.GetEnvironmentVariable("Company") +"Purch_ Rcpt_ Line].No_) as DimensionSetID " +
                  "FROM( " +
                  "SELECT " +
                  " dbo."+ Environment.GetEnvironmentVariable("Company") +"Purch_ Rcpt_ Header].No_ as GRNo, " +
                  "CONVERT(VARCHAR,dbo."+ Environment.GetEnvironmentVariable("Company") +"Purch_ Rcpt_ Header].[Posting Date],23) as PostingGR, " +
                     "dbo."+ Environment.GetEnvironmentVariable("Company") +"Purch_ Rcpt_ Header].[Order No_]  as OrderNo, " +
                  "dbo."+ Environment.GetEnvironmentVariable("Company") +"Purch_ Rcpt_ Header].[Ref_ Job Order No_]  as IV, " +
                  "dbo."+ Environment.GetEnvironmentVariable("Company") +"Purch_ Rcpt_ Header].[Shortcut Dimension 1 Code] as JobGL, " +
                  "dbo."+ Environment.GetEnvironmentVariable("Company") +"Purch_ Rcpt_ Header].[Pay-to Vendor No_] as VendorNo, " +
                  "dbo."+ Environment.GetEnvironmentVariable("Company") +"Purch_ Rcpt_ Header].[Pay-to Name] as VendorName, " +
                  "dbo."+ Environment.GetEnvironmentVariable("Company") +"Purch_ Rcpt_ Header].[Vendor Shipment No_] as Type  " +
                  "FROM dbo."+ Environment.GetEnvironmentVariable("Company") +"Purch_ Rcpt_ Header] " + typedoc +
                  ") as a " +
                  ") as b " +
                  ") as c " +
                  ")as d " +
                  "WHERE   d.OrderDoc Not Like 'AP%' and d.ReceiveAmountLine<>0 and d.OrderDoc<>''";



                GRReceipts = _navcontext.GRReceipts.FromSqlRaw(queryData).ToList();

            }


            var queryData2 = "SELECT ROW_NUMBER() OVER (ORDER BY dbo." + Environment.GetEnvironmentVariable("Company") + "Detailed Vendor Ledg_ Entry].[Document No_] ) as ID," +
                "CONVERT(VARCHAR,dbo." + Environment.GetEnvironmentVariable("Company") + "Detailed Vendor Ledg_ Entry].[Posting Date],23) AS PostingIV, " +
                "dbo." + Environment.GetEnvironmentVariable("Company") + "Detailed Vendor Ledg_ Entry].[Document No_] AS DocAP, " +
                "dbo." + Environment.GetEnvironmentVariable("Company") + "Detailed Vendor Ledg_ Entry].Amount, " +
                "dbo." + Environment.GetEnvironmentVariable("Company") + "Detailed Vendor Ledg_ Entry].[Vendor No_] AS VendorNo, " +
                "dbo." + Environment.GetEnvironmentVariable("Company") + "Detailed Vendor Ledg_ Entry].[Initial Entry Global Dim_ 1] AS JobGL, " +
                "dbo." + Environment.GetEnvironmentVariable("Company") + "Purch_ Inv_ Line].[Receipt No_] AS DocGR, " +
                "dbo." + Environment.GetEnvironmentVariable("Company") + "Purch_ Inv_ Header].[Ref_ Receipt No_] AS DocGR1, " +
                "dbo." + Environment.GetEnvironmentVariable("Company") + "Purch_ Inv_ Header].[Pay-to Name] AS VendorName, " +
                "isnull(dbo." + Environment.GetEnvironmentVariable("Company") + "Purch_ Inv_ Header].[Progress Term],0) as PrograssTerm " +
                "FROM " +
                "dbo." + Environment.GetEnvironmentVariable("Company") + "Detailed Vendor Ledg_ Entry] " +
                "INNER JOIN dbo." + Environment.GetEnvironmentVariable("Company") + "Purch_ Inv_ Header] ON dbo." + Environment.GetEnvironmentVariable("Company") + "Detailed Vendor Ledg_ Entry].[Vendor Ledger Entry No_] = dbo." + Environment.GetEnvironmentVariable("Company") + "Purch_ Inv_ Header].[Vendor Ledger Entry No_] " +
                "INNER JOIN dbo." + Environment.GetEnvironmentVariable("Company") + "Purch_ Inv_ Line] ON dbo." + Environment.GetEnvironmentVariable("Company") + "Purch_ Inv_ Header].No_ = dbo." + Environment.GetEnvironmentVariable("Company") + "Purch_ Inv_ Line].[Document No_] " +
                "WHERE " +
                //"dbo."+ Environment.GetEnvironmentVariable("Company") +"Purch_ Inv_ Line].[Receipt No_]!='' and " +
                " [Entry Type]=1 " +
                "group by  " +
                "dbo." + Environment.GetEnvironmentVariable("Company") + "Detailed Vendor Ledg_ Entry].[Vendor Ledger Entry No_], " +
                "dbo." + Environment.GetEnvironmentVariable("Company") + "Detailed Vendor Ledg_ Entry].[Entry Type], " +
                "dbo." + Environment.GetEnvironmentVariable("Company") + "Detailed Vendor Ledg_ Entry].[Posting Date], " +
                "dbo." + Environment.GetEnvironmentVariable("Company") + "Detailed Vendor Ledg_ Entry].[Document No_], " +
                "dbo." + Environment.GetEnvironmentVariable("Company") + "Detailed Vendor Ledg_ Entry].Amount, " +
                "dbo." + Environment.GetEnvironmentVariable("Company") + "Detailed Vendor Ledg_ Entry].[Vendor No_], " +
                "dbo." + Environment.GetEnvironmentVariable("Company") + "Detailed Vendor Ledg_ Entry].[Initial Entry Global Dim_ 1], " +
                "dbo." + Environment.GetEnvironmentVariable("Company") + "Purch_ Inv_ Header].[Vendor Ledger Entry No_], " +
                "dbo." + Environment.GetEnvironmentVariable("Company") + "Purch_ Inv_ Line].[Receipt No_], " +
                "dbo." + Environment.GetEnvironmentVariable("Company") + "Purch_ Inv_ Header].[Ref_ Receipt No_], " +
                "dbo." + Environment.GetEnvironmentVariable("Company") + "Purch_ Inv_ Header].[Pay-to Name], " +
                "dbo." + Environment.GetEnvironmentVariable("Company") + "Purch_ Inv_ Header].[Progress Term] " +
                "order by dbo." + Environment.GetEnvironmentVariable("Company") + "Detailed Vendor Ledg_ Entry].[Document No_] ";



            var APdocs = _navcontext.APDocs.FromSqlRaw(queryData2).ToList();



            //Query All PR
            var queryData1 = "SELECT DISTINCT " +
                "dbo." + Environment.GetEnvironmentVariable("Company") + "Posted Requisition Line].[Purch_ Order No_] as name, " +
                "dbo." + Environment.GetEnvironmentVariable("Company") + "Posted Requisition Line].[Document No_] as code " +
                "FROM [dbo]." + Environment.GetEnvironmentVariable("Company") + "Posted Requisition Line] " +
                "WHERE Type=2  " +
                "order by [Purch_ Order No_] ";

            var sourceAutoCompletes = _navcontext.sourceAutoCompletes.FromSqlRaw(queryData1).ToList();
            var pr = "";

            List<DetailVendor> instances = new List<DetailVendor>();
            DetailVendor current = null;

            List<APDoc> APs;


            foreach (var std in GRReceipts as IList<GRReceipt>)
            {
                current = new DetailVendor();
                current.GRDate = std.PostingGR;
                current.DocumentReceipt = std.GRNo;
                current.RefPOJO = std.OrderDoc;
                current.AmountReceipt = std.ReceiveAmountLine;
                current.PayToVendorNo = std.VendorNo;
                current.PayToVendorName = std.VendorName;
                current.JobGL = std.JobGL;
                current.Type = std.Type;

                //Query AP With GRNo
                APs = APdocs.Where(c => c.DocGR == std.GRNo || c.DocGR1 == std.GRNo).ToList();


                foreach (var ap in APs as IList<APDoc>)
                {
                    current.DocumentInv = ap.DocAP;
                    current.PostingDate = ap.PostingIV;
                    current.AmountVat = ap.Amount;
                    current.PrograssTerm = ap.PrograssTerm;

                }





                pr = "";
                if (std.OrderDoc != "")
                {
                    List<SourceAutoComplete> data = sourceAutoCompletes.Where(c => c.name == std.OrderDoc).ToList();
                    var doctmp = "";
                    foreach (var stdpr in data as IList<SourceAutoComplete>)
                    {

                        if (doctmp == stdpr.code)
                        {

                        }
                        else
                        {
                            doctmp = stdpr.code;
                            pr += stdpr.code + "<br>";
                        }

                    }
                }
                if (pr.Length > 0)
                {
                    pr = pr.Substring(0, pr.Length - 1);
                }

                current.RefPR = pr;

                instances.Add(current);
            }


            var docnotap=instances.Where(p => p.DocumentInv == null).ToList();
            var xy = from a in docnotap

                       .GroupBy(a => new { a.JobGL})
            select new
            {
                x = a.Key.JobGL,
                y = a.Count()
            };



            response = Ok(new { data = docnotap , dataSource = xy });


            return response;




        }

    }
}