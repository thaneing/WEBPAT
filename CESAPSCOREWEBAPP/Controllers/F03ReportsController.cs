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

namespace CESAPSCOREWEBAPP.Controllers
{
    public class F03ReportsController : BaseController
    {
        private readonly NAVContext _navcontext;
        private readonly DatabaseContext _context;

        public F03ReportsController(NAVContext navcontext, DatabaseContext context)
        {
            _context = context;
            _navcontext = navcontext;
        }
        public IActionResult Index()
        {


            /*Check Session */
            var page = "268";
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
            /*Check Session */

            return View();
        }
        public IActionResult F03Reports()
        {


            /*Check Session */
            var page = "268";
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
            /*Check Session */

            var queryData1 = "SELECT dbo." + Environment.GetEnvironmentVariable("Company") + "User Setup].[User ID] AS name,dbo." + Environment.GetEnvironmentVariable("Company") + "User Setup].[User ID] as code FROM [dbo]." + Environment.GetEnvironmentVariable("Company") + "User Setup] order by dbo." + Environment.GetEnvironmentVariable("Company") + "User Setup].[User ID] asc ";
            var sourceAutoCompletes = _navcontext.sourceAutoCompletes.FromSqlRaw(queryData1).ToList();
            ViewData["SourceAutoCompletes"] = sourceAutoCompletes;

            ViewBag.StartDate = DateTime.Now.ToString("01/MM/yyyy", new CultureInfo("en-US")); 
            ViewBag.EndDate = DateTime.Now.ToString("dd/MM/yyyy", new CultureInfo("en-US")); 

            var queryData2 = "SELECT dbo." + Environment.GetEnvironmentVariable("Company") + "Location].Code AS name,dbo." + Environment.GetEnvironmentVariable("Company") + "Location].Name as code FROM [dbo]." + Environment.GetEnvironmentVariable("Company") + "Location] order by dbo." + Environment.GetEnvironmentVariable("Company") + "Location].Code asc ";
            var sourceAutoCompletesJob = _navcontext.sourceAutoCompletes.FromSqlRaw(queryData2).ToList();
            ViewData["SourceAutoCompletesJob"] = sourceAutoCompletesJob;



            return View();
        }
        public IActionResult Getdata(string user, string date1, string date2,string site,int type)
        {


            /*Check Session */
            var page = "268";
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
            /*Check Session */


            IActionResult response = Unauthorized();
            var StartDate = date1.Substring(6, 4) + "-" + date1.Substring(3, 2) + "-" + date1.Substring(0, 2) + " 00:00:00";
            var EndDate = date2.Substring(6, 4) + "-" + date2.Substring(3, 2) + "-" + date2.Substring(0, 2) + " 23:59:59";


            List<F03> F03DATA= new List<F03>();


            if (user == null)
            {

                user = "";
            }
            else
            {
                user = "dbo.[C_E_S_ CO_, LTD_$Purch_ Rcpt_ Header].[User ID] = '" + user + "' AND ";
            }



            if (type == 3)
            {
                var query = "SELECT ROW_NUMBER() OVER (ORDER BY c.NoGR) AS ID,c.ReceiptDate AS ReceiptDate,c.VendorName AS VendorName,c.Des AS Des,c.NoGR AS NoGR,c.DocNo AS DocNo,c.Ref AS Ref,c.Qty AS Qty,c.Uom As Uom,c.Disc AS Disc,c.UnitCost AS UnitCost,c.Amount As Amount,(c.Disc/100)*c.Amount AS DiscPrice,c.ShipmentNo AS ShipmentNo,c.AmountBase As AmountBase " +
                " FROM " +
                " (SELECT *, b.AmountBase AS Amount " +
                " From" +
                " (SELECT * FROM(SELECT convert(varchar,dbo.[C_E_S_ CO_, LTD_$Purch_ Rcpt_ Line].[Posting Date],23) AS ReceiptDate," +
                " dbo.[C_E_S_ CO_, LTD_$Purch_ Rcpt_ Header].[Buy-from Vendor Name] AS VendorName," +
                " CONCAT(dbo.[C_E_S_ CO_, LTD_$Purch_ Rcpt_ Line].Description," +
                " dbo.[C_E_S_ CO_, LTD_$Purch_ Rcpt_ Line].[Description 2]) AS Des," +
                " dbo.[C_E_S_ CO_, LTD_$Purch_ Rcpt_ Line].[Document No_] AS NoGR," +
                " dbo.[C_E_S_ CO_, LTD_$Purch_ Rcpt_ Line].[Order No_] AS DocNo," +
                " dbo.[C_E_S_ CO_, LTD_$Purch_ Rcpt_ Line].[Ref_ PR No_] AS Ref," +
                " dbo.[C_E_S_ CO_, LTD_$Purch_ Rcpt_ Line].[Unit of Measure] AS Uom," +
                " dbo.[C_E_S_ CO_, LTD_$Purch_ Rcpt_ Line].[Direct Unit Cost] As UnitCost," +
                " dbo.[C_E_S_ CO_, LTD_$Purch_ Rcpt_ Line].[Line Discount _] AS Disc," +
                " SUM(dbo.[C_E_S_ CO_, LTD_$Purch_ Rcpt_ Line].Quantity) AS Qty," +
                " dbo.[C_E_S_ CO_, LTD_$Purch_ Rcpt_ Line].[Vendor Shipment No_] AS ShipmentNo," +
                " dbo.[C_E_S_ CO_, LTD_$Purch_ Rcpt_ Line].[Item Charge Base Amount] AS AmountBase" +
                " FROM dbo.[C_E_S_ CO_, LTD_$Purch_ Rcpt_ Header] INNER JOIN dbo.[C_E_S_ CO_, LTD_$Purch_ Rcpt_ Line] ON dbo.[C_E_S_ CO_, LTD_$Purch_ Rcpt_ Header].No_ = dbo.[C_E_S_ CO_, LTD_$Purch_ Rcpt_ Line].[Document No_] " +
                " WHERE "+ user + " dbo.[C_E_S_ CO_, LTD_$Purch_ Rcpt_ Line].[Posting Date] BETWEEN {0} AND {1} AND dbo.[C_E_S_ CO_, LTD_$Purch_ Rcpt_ Header].[Shortcut Dimension 1 Code] = {2}" +
                " GROUP BY dbo.[C_E_S_ CO_, LTD_$Purch_ Rcpt_ Line].[Posting Date]," +
                " dbo.[C_E_S_ CO_, LTD_$Purch_ Rcpt_ Header].[Buy-from Vendor Name]," +
                " dbo.[C_E_S_ CO_, LTD_$Purch_ Rcpt_ Line].Description," +
                " dbo.[C_E_S_ CO_, LTD_$Purch_ Rcpt_ Line].[Document No_], " +
                " dbo.[C_E_S_ CO_, LTD_$Purch_ Rcpt_ Line].[Order No_]," +
                " dbo.[C_E_S_ CO_, LTD_$Purch_ Rcpt_ Line].[Ref_ PR No_]," +
                " dbo.[C_E_S_ CO_, LTD_$Purch_ Rcpt_ Line].[Unit of Measure]," +
                " dbo.[C_E_S_ CO_, LTD_$Purch_ Rcpt_ Line].[Direct Unit Cost]," +
                " dbo.[C_E_S_ CO_, LTD_$Purch_ Rcpt_ Line].[Line Discount _]," +
                " dbo.[C_E_S_ CO_, LTD_$Purch_ Rcpt_ Line].[Description 2]," +
                "dbo.[C_E_S_ CO_, LTD_$Purch_ Rcpt_ Line].[Vendor Shipment No_]," +
                " dbo.[C_E_S_ CO_, LTD_$Purch_ Rcpt_ Line].[Item Charge Base Amount]) as a" +
                ") as b) as c WHERE c.Qty != 0 ORDER BY c.NoGR";

                F03DATA = _navcontext.F03s.FromSqlRaw(query, StartDate, EndDate, site).ToList();
            }
            else
            {
                var query = "SELECT ROW_NUMBER() OVER (ORDER BY c.NoGR) AS ID,c.ReceiptDate AS ReceiptDate,c.VendorName AS VendorName,c.Des AS Des,c.NoGR AS NoGR,c.DocNo AS DocNo,c.Ref AS Ref,c.Qty AS Qty,c.Uom As Uom,c.Disc AS Disc,c.UnitCost AS UnitCost,c.Amount As Amount,(c.Disc/100)*c.Amount AS DiscPrice,c.ShipmentNo AS ShipmentNo,c.AmountBase As AmountBase " +
               " FROM " +
               " (SELECT *, b.AmountBase AS Amount " +
               " From" +
               " (SELECT * FROM(SELECT convert(varchar,dbo.[C_E_S_ CO_, LTD_$Purch_ Rcpt_ Line].[Posting Date],23) AS ReceiptDate," +
               " dbo.[C_E_S_ CO_, LTD_$Purch_ Rcpt_ Header].[Buy-from Vendor Name] AS VendorName," +
               " CONCAT(dbo.[C_E_S_ CO_, LTD_$Purch_ Rcpt_ Line].Description," +
               " dbo.[C_E_S_ CO_, LTD_$Purch_ Rcpt_ Line].[Description 2]) AS Des," +
               " dbo.[C_E_S_ CO_, LTD_$Purch_ Rcpt_ Line].[Document No_] AS NoGR," +
               " dbo.[C_E_S_ CO_, LTD_$Purch_ Rcpt_ Line].[Order No_] AS DocNo," +
               " dbo.[C_E_S_ CO_, LTD_$Purch_ Rcpt_ Line].[Ref_ PR No_] AS Ref," +
               " dbo.[C_E_S_ CO_, LTD_$Purch_ Rcpt_ Line].[Unit of Measure] AS Uom," +
               " dbo.[C_E_S_ CO_, LTD_$Purch_ Rcpt_ Line].[Direct Unit Cost] As UnitCost," +
               " dbo.[C_E_S_ CO_, LTD_$Purch_ Rcpt_ Line].[Line Discount _] AS Disc," +
               " SUM(dbo.[C_E_S_ CO_, LTD_$Purch_ Rcpt_ Line].Quantity) AS Qty," +
               " dbo.[C_E_S_ CO_, LTD_$Purch_ Rcpt_ Line].[Vendor Shipment No_] AS ShipmentNo," +
               " dbo.[C_E_S_ CO_, LTD_$Purch_ Rcpt_ Line].[Item Charge Base Amount] AS AmountBase" +
               " FROM dbo.[C_E_S_ CO_, LTD_$Purch_ Rcpt_ Header] INNER JOIN dbo.[C_E_S_ CO_, LTD_$Purch_ Rcpt_ Line] ON dbo.[C_E_S_ CO_, LTD_$Purch_ Rcpt_ Header].No_ = dbo.[C_E_S_ CO_, LTD_$Purch_ Rcpt_ Line].[Document No_] " +
               " WHERE "+ user + " dbo.[C_E_S_ CO_, LTD_$Purch_ Rcpt_ Line].[Posting Date] BETWEEN {0} AND {1} AND dbo.[C_E_S_ CO_, LTD_$Purch_ Rcpt_ Header].[Shortcut Dimension 1 Code] = {2} AND dbo.[C_E_S_ CO_, LTD_$Purch_ Rcpt_ Line].[Job Document Type] = {3}" +
               " GROUP BY dbo.[C_E_S_ CO_, LTD_$Purch_ Rcpt_ Line].[Posting Date]," +
               " dbo.[C_E_S_ CO_, LTD_$Purch_ Rcpt_ Header].[Buy-from Vendor Name]," +
               " dbo.[C_E_S_ CO_, LTD_$Purch_ Rcpt_ Line].Description," +
               " dbo.[C_E_S_ CO_, LTD_$Purch_ Rcpt_ Line].[Document No_], " +
               " dbo.[C_E_S_ CO_, LTD_$Purch_ Rcpt_ Line].[Order No_]," +
               " dbo.[C_E_S_ CO_, LTD_$Purch_ Rcpt_ Line].[Ref_ PR No_]," +
               " dbo.[C_E_S_ CO_, LTD_$Purch_ Rcpt_ Line].[Unit of Measure]," +
               " dbo.[C_E_S_ CO_, LTD_$Purch_ Rcpt_ Line].[Direct Unit Cost]," +
               " dbo.[C_E_S_ CO_, LTD_$Purch_ Rcpt_ Line].[Line Discount _]," +
               " dbo.[C_E_S_ CO_, LTD_$Purch_ Rcpt_ Line].[Description 2]," +
               "dbo.[C_E_S_ CO_, LTD_$Purch_ Rcpt_ Line].[Vendor Shipment No_]," +
               " dbo.[C_E_S_ CO_, LTD_$Purch_ Rcpt_ Line].[Item Charge Base Amount]) as a" +
               ") as b) as c WHERE c.Qty != 0 ORDER BY c.NoGR";

                F03DATA = _navcontext.F03s.FromSqlRaw(query, StartDate, EndDate, site, type).ToList();
            }
           

            response = Ok(new { data = F03DATA });


            return response;
        }

        public IActionResult GetdataReport(string user, string date1, string date2, string site, int type)
        {


            /*Check Session */
            var page = "268";
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
            /*Check Session */


            IActionResult response = Unauthorized();
            var StartDate = date1.Substring(6, 4) + "-" + date1.Substring(3, 2) + "-" + date1.Substring(0, 2) + " 00:00:00";
            var EndDate = date2.Substring(6, 4) + "-" + date2.Substring(3, 2) + "-" + date2.Substring(0, 2) + " 23:59:59";
            var SStartDate = date1.Substring(0, 2) + "/" + date1.Substring(3, 2) + "/" + date1.Substring(6, 4);

            var EEndDate = date2.Substring(0, 2) + "/" + date2.Substring(3, 2) + "/" + date2.Substring(6, 4);


            List <F03> F03DATA = new List<F03>();
            var user1 = "";

            if (user == null)
            {

                user1 = "";
            }
            else
            {
                user1 = "dbo.[C_E_S_ CO_, LTD_$Purch_ Rcpt_ Header].[User ID] = '" + user + "' AND ";
            }



            if (type == 3)
            {
                var query = "SELECT ROW_NUMBER() OVER (ORDER BY c.NoGR) AS ID,c.ReceiptDate AS ReceiptDate,c.VendorName AS VendorName,c.Des AS Des,c.NoGR AS NoGR,c.DocNo AS DocNo,c.Ref AS Ref,c.Qty AS Qty,c.Uom As Uom,c.Disc AS Disc,c.UnitCost AS UnitCost,c.Amount As Amount,(c.Disc/100)*c.Amount AS DiscPrice,c.ShipmentNo AS ShipmentNo,c.AmountBase As AmountBase " +
                " FROM " +
                " (SELECT *, b.AmountBase  AS Amount " +
                " From" +
                " (SELECT * FROM(SELECT convert(varchar,dbo.[C_E_S_ CO_, LTD_$Purch_ Rcpt_ Line].[Posting Date],23) AS ReceiptDate," +
                " dbo.[C_E_S_ CO_, LTD_$Purch_ Rcpt_ Header].[Buy-from Vendor Name] AS VendorName," +
                " CONCAT(dbo.[C_E_S_ CO_, LTD_$Purch_ Rcpt_ Line].Description," +
                " dbo.[C_E_S_ CO_, LTD_$Purch_ Rcpt_ Line].[Description 2]) AS Des," +
                " dbo.[C_E_S_ CO_, LTD_$Purch_ Rcpt_ Line].[Document No_] AS NoGR," +
                " dbo.[C_E_S_ CO_, LTD_$Purch_ Rcpt_ Line].[Order No_] AS DocNo," +
                " dbo.[C_E_S_ CO_, LTD_$Purch_ Rcpt_ Line].[Ref_ PR No_] AS Ref," +
                " dbo.[C_E_S_ CO_, LTD_$Purch_ Rcpt_ Line].[Unit of Measure] AS Uom," +
                " dbo.[C_E_S_ CO_, LTD_$Purch_ Rcpt_ Line].[Direct Unit Cost] As UnitCost," +
                " dbo.[C_E_S_ CO_, LTD_$Purch_ Rcpt_ Line].[Line Discount _] AS Disc," +
                " SUM(dbo.[C_E_S_ CO_, LTD_$Purch_ Rcpt_ Line].Quantity) AS Qty," +
                " dbo.[C_E_S_ CO_, LTD_$Purch_ Rcpt_ Line].[Vendor Shipment No_] AS ShipmentNo," +
                " dbo.[C_E_S_ CO_, LTD_$Purch_ Rcpt_ Line].[Item Charge Base Amount] AS AmountBase" +
                " FROM dbo.[C_E_S_ CO_, LTD_$Purch_ Rcpt_ Header] INNER JOIN dbo.[C_E_S_ CO_, LTD_$Purch_ Rcpt_ Line] ON dbo.[C_E_S_ CO_, LTD_$Purch_ Rcpt_ Header].No_ = dbo.[C_E_S_ CO_, LTD_$Purch_ Rcpt_ Line].[Document No_] " +
                " WHERE " + user1 + " dbo.[C_E_S_ CO_, LTD_$Purch_ Rcpt_ Line].[Posting Date] BETWEEN {0} AND {1} AND dbo.[C_E_S_ CO_, LTD_$Purch_ Rcpt_ Header].[Shortcut Dimension 1 Code] = {2}" +
                " GROUP BY dbo.[C_E_S_ CO_, LTD_$Purch_ Rcpt_ Line].[Posting Date]," +
                " dbo.[C_E_S_ CO_, LTD_$Purch_ Rcpt_ Header].[Buy-from Vendor Name]," +
                " dbo.[C_E_S_ CO_, LTD_$Purch_ Rcpt_ Line].Description," +
                " dbo.[C_E_S_ CO_, LTD_$Purch_ Rcpt_ Line].[Document No_], " +
                " dbo.[C_E_S_ CO_, LTD_$Purch_ Rcpt_ Line].[Order No_]," +
                " dbo.[C_E_S_ CO_, LTD_$Purch_ Rcpt_ Line].[Ref_ PR No_]," +
                " dbo.[C_E_S_ CO_, LTD_$Purch_ Rcpt_ Line].[Unit of Measure]," +
                " dbo.[C_E_S_ CO_, LTD_$Purch_ Rcpt_ Line].[Direct Unit Cost]," +
                " dbo.[C_E_S_ CO_, LTD_$Purch_ Rcpt_ Line].[Line Discount _]," +
                " dbo.[C_E_S_ CO_, LTD_$Purch_ Rcpt_ Line].[Description 2]," +
                "dbo.[C_E_S_ CO_, LTD_$Purch_ Rcpt_ Line].[Vendor Shipment No_]," +
                " dbo.[C_E_S_ CO_, LTD_$Purch_ Rcpt_ Line].[Item Charge Base Amount]) as a" +
                ") as b) as c WHERE c.Qty != 0 ORDER BY c.NoGR";

                F03DATA = _navcontext.F03s.FromSqlRaw(query, StartDate, EndDate, site).ToList();
            }
            else
            {
                var query = "SELECT ROW_NUMBER() OVER (ORDER BY c.NoGR) AS ID,c.ReceiptDate AS ReceiptDate,c.VendorName AS VendorName,c.Des AS Des,c.NoGR AS NoGR,c.DocNo AS DocNo,c.Ref AS Ref,c.Qty AS Qty,c.Uom As Uom,c.Disc AS Disc,c.UnitCost AS UnitCost,c.Amount As Amount,(c.Disc/100)*c.Amount AS DiscPrice,c.ShipmentNo AS ShipmentNo,c.AmountBase As AmountBase " +
               " FROM " +
               " (SELECT *, b.AmountBase AS Amount " +
               " From" +
               " (SELECT * FROM(SELECT convert(varchar,dbo.[C_E_S_ CO_, LTD_$Purch_ Rcpt_ Line].[Posting Date],23) AS ReceiptDate," +
               " dbo.[C_E_S_ CO_, LTD_$Purch_ Rcpt_ Header].[Buy-from Vendor Name] AS VendorName," +
               " CONCAT(dbo.[C_E_S_ CO_, LTD_$Purch_ Rcpt_ Line].Description," +
               " dbo.[C_E_S_ CO_, LTD_$Purch_ Rcpt_ Line].[Description 2]) AS Des," +
               " dbo.[C_E_S_ CO_, LTD_$Purch_ Rcpt_ Line].[Document No_] AS NoGR," +
               " dbo.[C_E_S_ CO_, LTD_$Purch_ Rcpt_ Line].[Order No_] AS DocNo," +
               " dbo.[C_E_S_ CO_, LTD_$Purch_ Rcpt_ Line].[Ref_ PR No_] AS Ref," +
               " dbo.[C_E_S_ CO_, LTD_$Purch_ Rcpt_ Line].[Unit of Measure] AS Uom," +
               " dbo.[C_E_S_ CO_, LTD_$Purch_ Rcpt_ Line].[Direct Unit Cost] As UnitCost," +
               " dbo.[C_E_S_ CO_, LTD_$Purch_ Rcpt_ Line].[Line Discount _] AS Disc," +
               " SUM(dbo.[C_E_S_ CO_, LTD_$Purch_ Rcpt_ Line].Quantity) AS Qty," +
               " dbo.[C_E_S_ CO_, LTD_$Purch_ Rcpt_ Line].[Vendor Shipment No_] AS ShipmentNo," +
               " dbo.[C_E_S_ CO_, LTD_$Purch_ Rcpt_ Line].[Item Charge Base Amount] AS AmountBase" +
               " FROM dbo.[C_E_S_ CO_, LTD_$Purch_ Rcpt_ Header] INNER JOIN dbo.[C_E_S_ CO_, LTD_$Purch_ Rcpt_ Line] ON dbo.[C_E_S_ CO_, LTD_$Purch_ Rcpt_ Header].No_ = dbo.[C_E_S_ CO_, LTD_$Purch_ Rcpt_ Line].[Document No_] " +
               " WHERE " + user1 + " dbo.[C_E_S_ CO_, LTD_$Purch_ Rcpt_ Line].[Posting Date] BETWEEN {0} AND {1} AND dbo.[C_E_S_ CO_, LTD_$Purch_ Rcpt_ Header].[Shortcut Dimension 1 Code] = {2} AND dbo.[C_E_S_ CO_, LTD_$Purch_ Rcpt_ Line].[Job Document Type] = {3}" +
               " GROUP BY dbo.[C_E_S_ CO_, LTD_$Purch_ Rcpt_ Line].[Posting Date]," +
               " dbo.[C_E_S_ CO_, LTD_$Purch_ Rcpt_ Header].[Buy-from Vendor Name]," +
               " dbo.[C_E_S_ CO_, LTD_$Purch_ Rcpt_ Line].Description," +
               " dbo.[C_E_S_ CO_, LTD_$Purch_ Rcpt_ Line].[Document No_], " +
               " dbo.[C_E_S_ CO_, LTD_$Purch_ Rcpt_ Line].[Order No_]," +
               " dbo.[C_E_S_ CO_, LTD_$Purch_ Rcpt_ Line].[Ref_ PR No_]," +
               " dbo.[C_E_S_ CO_, LTD_$Purch_ Rcpt_ Line].[Unit of Measure]," +
               " dbo.[C_E_S_ CO_, LTD_$Purch_ Rcpt_ Line].[Direct Unit Cost]," +
               " dbo.[C_E_S_ CO_, LTD_$Purch_ Rcpt_ Line].[Line Discount _]," +
               " dbo.[C_E_S_ CO_, LTD_$Purch_ Rcpt_ Line].[Description 2]," +
               "dbo.[C_E_S_ CO_, LTD_$Purch_ Rcpt_ Line].[Vendor Shipment No_]," +
               " dbo.[C_E_S_ CO_, LTD_$Purch_ Rcpt_ Line].[Item Charge Base Amount]) as a" +
               ") as b) as c WHERE c.Qty != 0 ORDER BY c.NoGR";

                F03DATA = _navcontext.F03s.FromSqlRaw(query, StartDate, EndDate, site, type).ToList();
            }

            //response = Ok(new { data = F03DATA });
            List<F03DATAReport> f03DATAReports = new List<F03DATAReport>();
            F03DATAReport fdataReport;

            foreach(var std in F03DATA)
            {
                fdataReport = new F03DATAReport();
                fdataReport.ID = std.ID;
                fdataReport.DocNo = std.DocNo;
                fdataReport.NoGR = std.NoGR;
                fdataReport.Des = std.Des;
                fdataReport.ReceiptDate = std.ReceiptDate.Substring(8, 2) + "/" + std.ReceiptDate.Substring(5, 2) + "/" + std.ReceiptDate.Substring(0, 4);
                fdataReport.VendorName = std.VendorName;
                fdataReport.Ref = std.Ref;
                fdataReport.Uom = std.Uom;
                fdataReport.UnitCost = std.UnitCost;
                fdataReport.Disc = std.Disc;
                fdataReport.Qty = std.Qty;
                fdataReport.Amount = std.Amount;
                fdataReport.DiscPrice = std.DiscPrice;
                fdataReport.ShipmentNo = std.ShipmentNo;
                fdataReport.Filter="Filter : PostingDate :" + SStartDate + ".." + EEndDate + ", Assigned User ID : " + user +" ,Stie : "+site;
                f03DATAReports.Add(fdataReport);
            }

            XtraReport report = XtraReport.FromFile("reports\\ReportF03.repx");
            report.DataSource = f03DATAReports;



            report.CreateDocument(true);
            var @out = new MemoryStream();
            report.ExportToPdf(@out);
            @out.Position = 0;



            response = Ok(new { data = f03DATAReports });


            //return response;
            return new FileStreamResult(@out, "application/pdf");

        }
    }
}