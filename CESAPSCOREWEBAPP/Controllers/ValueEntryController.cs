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
using Microsoft.AspNetCore.Authorization;

namespace CESAPSCOREWEBAPP.Controllers
{
    [Authorize]
    public class ValueEntryController : BaseController
    {
        private readonly NAVContext _navcontext;
        private readonly DatabaseContext _context;

        public ValueEntryController(NAVContext navcontext, DatabaseContext context)
        {
            _context = context;
            _navcontext = navcontext;
        }
        public IActionResult Index()
        {
            /*Check Session */
            var page = "232";
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
            var queryData1 = "SELECT dbo." + Environment.GetEnvironmentVariable("Company") + "User Setup].[User ID] AS name,dbo." + Environment.GetEnvironmentVariable("Company") + "User Setup].[User ID] as code FROM [dbo]." + Environment.GetEnvironmentVariable("Company") + "User Setup] order by dbo." + Environment.GetEnvironmentVariable("Company") + "User Setup].[User ID] asc ";
            var sourceAutoCompletes = _navcontext.sourceAutoCompletes.FromSqlRaw(queryData1).ToList();
            ViewData["SourceAutoCompletes"] = sourceAutoCompletes;
            ViewBag.StartDate = DateTime.Now.ToString("01/MM/yyyy");
            ViewBag.EndDate = DateTime.Now.ToString("dd/MM/yyyy");

            return View();
        }
        public IActionResult GetData(string user,string date1,string date2)
        {
            /*Check Session */
            var page = "232";
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

            var queryData1 = "SELECT dbo." + Environment.GetEnvironmentVariable("Company") + "User Setup].[User ID] AS name,dbo." + Environment.GetEnvironmentVariable("Company") + "User Setup].[User ID] as code FROM [dbo]." + Environment.GetEnvironmentVariable("Company") + "User Setup] order by dbo." + Environment.GetEnvironmentVariable("Company") + "User Setup].[User ID] asc ";
            var sourceAutoCompletes = _navcontext.sourceAutoCompletes.FromSqlRaw(queryData1).ToList();
            ViewData["SourceAutoCompletes"] = sourceAutoCompletes;

            IActionResult response = Unauthorized();
            var StartDate = date1.Substring(6, 4) + "-" + date1.Substring(3, 2) + "-" + date1.Substring(0, 2) + " 00:00:00";
            var EndDate = date2.Substring(6, 4) + "-" + date2.Substring(3, 2) + "-" + date2.Substring(0, 2) + " 23:59:59";
            List<ValueEntry> ValueEntryDATA;
            var querydata = "";

            if (user == null)
            {
                querydata = "SELECT" +
                    " convert(varchar,dbo." + Environment.GetEnvironmentVariable("Company") + "Value Entry].[Posting Date],23) AS PostingDate," +
                    " dbo." + Environment.GetEnvironmentVariable("Company") + "Value Entry].[Entry No_] AS EntryNo," +
                    " CASE WHEN convert(varchar,dbo." + Environment.GetEnvironmentVariable("Company") + "Value Entry].[Item Ledger Entry Type]) = 0 then 'Purchase' " +
                    "   WHEN convert(varchar,dbo." + Environment.GetEnvironmentVariable("Company") + "Value Entry].[Item Ledger Entry Type]) = 1 then 'Sale' " +
                    "   WHEN convert(varchar,dbo." + Environment.GetEnvironmentVariable("Company") + "Value Entry].[Item Ledger Entry Type]) = 2 then 'Positive Adjmt' " +
                    "   WHEN convert(varchar,dbo." + Environment.GetEnvironmentVariable("Company") + "Value Entry].[Item Ledger Entry Type]) = 3 then 'Negative Adjmt' " +
                    "   WHEN convert(varchar,dbo." + Environment.GetEnvironmentVariable("Company") + "Value Entry].[Item Ledger Entry Type]) = 4 then 'Transfer' " +
                    "   WHEN convert(varchar,dbo." + Environment.GetEnvironmentVariable("Company") + "Value Entry].[Item Ledger Entry Type]) = 5 then 'Consumption' " +
                    "   WHEN convert(varchar,dbo." + Environment.GetEnvironmentVariable("Company") + "Value Entry].[Item Ledger Entry Type]) = 6 then 'Output' " +
                    "   WHEN convert(varchar,dbo." + Environment.GetEnvironmentVariable("Company") + "Value Entry].[Item Ledger Entry Type]) = 7 then '' " +
                    "   WHEN convert(varchar,dbo." + Environment.GetEnvironmentVariable("Company") + "Value Entry].[Item Ledger Entry Type]) = 8 then 'Asembly Consumption' " +
                    "   WHEN convert(varchar,dbo." + Environment.GetEnvironmentVariable("Company") + "Value Entry].[Item Ledger Entry Type]) = 9 then 'Asembly Output' " +
                    " end AS ItemLedgerEntryType," +
                    " dbo." + Environment.GetEnvironmentVariable("Company") + "Value Entry].[Cost per Unit] AS CostperUnit," +
                    " dbo." + Environment.GetEnvironmentVariable("Company") + "Value Entry].[User ID] AS UserID," +
                    " dbo." + Environment.GetEnvironmentVariable("Company") + "Value Entry].[Document No_] As DocumentNo," +
                    " dbo." + Environment.GetEnvironmentVariable("Company") + "Value Entry].Description AS Description," +
                    " CASE WHEN dbo." + Environment.GetEnvironmentVariable("Company") + "Value Entry].Adjustment = 0 then 'No'" +
                    "    WHEN  dbo." + Environment.GetEnvironmentVariable("Company") + "Value Entry].Adjustment =  1 then 'Yes' end AS Adjustment," +
                    " CASE WHEN convert(varchar,dbo." + Environment.GetEnvironmentVariable("Company") + "Value Entry].[Document Type]) = 0 then '' " +
                    "   WHEN convert(varchar,dbo." + Environment.GetEnvironmentVariable("Company") + "Value Entry].[Document Type]) = 1 then 'Sale Shipment' " +
                    "   WHEN convert(varchar,dbo." + Environment.GetEnvironmentVariable("Company") + "Value Entry].[Document Type]) = 2 then 'Sale Invoice' " +
                    "   WHEN convert(varchar,dbo." + Environment.GetEnvironmentVariable("Company") + "Value Entry].[Document Type]) = 3 then 'Sale Return Recitp' " +
                    "   WHEN convert(varchar,dbo." + Environment.GetEnvironmentVariable("Company") + "Value Entry].[Document Type]) = 4 then 'Sale Credit Memo' " +
                    "   WHEN convert(varchar,dbo." + Environment.GetEnvironmentVariable("Company") + "Value Entry].[Document Type]) = 5 then 'Purchase Recitp' " +
                    "   WHEN convert(varchar,dbo." + Environment.GetEnvironmentVariable("Company") + "Value Entry].[Document Type]) = 6 then 'Purchase Credit Memmo'" +
                    "   WHEN convert(varchar,dbo." + Environment.GetEnvironmentVariable("Company") + "Value Entry].[Document Type]) = 7 then 'Purchase Return Shipment' " +
                    "   WHEN convert(varchar,dbo." + Environment.GetEnvironmentVariable("Company") + "Value Entry].[Document Type]) = 8 then 'Purchase Credit Memmo' " +
                    "   WHEN convert(varchar,dbo." + Environment.GetEnvironmentVariable("Company") + "Value Entry].[Document Type]) = 9 then 'Tranfer Shipment' " +
                    "   WHEN convert(varchar,dbo." + Environment.GetEnvironmentVariable("Company") + "Value Entry].[Document Type]) = 10 then 'Tranfer Recitp' " +
                    "   WHEN convert(varchar,dbo." + Environment.GetEnvironmentVariable("Company") + "Value Entry].[Document Type]) = 11 then 'Service Shipment' " +
                    "   WHEN convert(varchar,dbo." + Environment.GetEnvironmentVariable("Company") + "Value Entry].[Document Type]) = 12 then 'Service Invoice' " +
                    "   WHEN convert(varchar,dbo." + Environment.GetEnvironmentVariable("Company") + "Value Entry].[Document Type]) = 13 then 'Service Credit Memo' " +
                    "   WHEN convert(varchar,dbo." + Environment.GetEnvironmentVariable("Company") + "Value Entry].[Document Type]) = 14 then 'Posted Assembly' " +
                    " end AS DocumentType," +
                    " dbo." + Environment.GetEnvironmentVariable("Company") + "Value Entry].[Cost Amount (Actual)] AS CostAmountActual" +
                    " FROM" +
                    " dbo." + Environment.GetEnvironmentVariable("Company") + "Value Entry]"+
                     " WHERE dbo." + Environment.GetEnvironmentVariable("Company") + "Value Entry].[Posting Date]>={0} and dbo." + Environment.GetEnvironmentVariable("Company") + "Value Entry].[Posting Date]<={1}" +
                   " ORDER BY dbo." + Environment.GetEnvironmentVariable("Company") + "Value Entry].[Entry No_] ASC";

                //SqlParameter parameterStartdate = new SqlParameter("@startdate", StartDate);
                //SqlParameter parameterEndDate = new SqlParameter("@enddate", EndDate);

                ValueEntryDATA = _navcontext.ValueEntries.FromSqlRaw(querydata, StartDate, EndDate).ToList();
            }
            else
            {
                querydata = "SELECT" +
                    " convert(varchar,dbo." + Environment.GetEnvironmentVariable("Company") + "Value Entry].[Posting Date],23) AS PostingDate," +
                    " dbo." + Environment.GetEnvironmentVariable("Company") + "Value Entry].[Entry No_] AS EntryNo," +
                    " CASE WHEN convert(varchar,dbo." + Environment.GetEnvironmentVariable("Company") + "Value Entry].[Item Ledger Entry Type]) = 0 then 'Purchase' " +
                    "   WHEN convert(varchar,dbo." + Environment.GetEnvironmentVariable("Company") + "Value Entry].[Item Ledger Entry Type]) = 1 then 'Sale' " +
                    "   WHEN convert(varchar,dbo." + Environment.GetEnvironmentVariable("Company") + "Value Entry].[Item Ledger Entry Type]) = 2 then 'Positive Adjmt' " +
                    "   WHEN convert(varchar,dbo." + Environment.GetEnvironmentVariable("Company") + "Value Entry].[Item Ledger Entry Type]) = 3 then 'Negative Adjmt' " +
                    "   WHEN convert(varchar,dbo." + Environment.GetEnvironmentVariable("Company") + "Value Entry].[Item Ledger Entry Type]) = 4 then 'Transfer' " +
                    "   WHEN convert(varchar,dbo." + Environment.GetEnvironmentVariable("Company") + "Value Entry].[Item Ledger Entry Type]) = 5 then 'Consumption' " +
                    "   WHEN convert(varchar,dbo." + Environment.GetEnvironmentVariable("Company") + "Value Entry].[Item Ledger Entry Type]) = 6 then 'Output' " +
                    "   WHEN convert(varchar,dbo." + Environment.GetEnvironmentVariable("Company") + "Value Entry].[Item Ledger Entry Type]) = 7 then '' " +
                    "   WHEN convert(varchar,dbo." + Environment.GetEnvironmentVariable("Company") + "Value Entry].[Item Ledger Entry Type]) = 8 then 'Asembly Consumption' " +
                    "   WHEN convert(varchar,dbo." + Environment.GetEnvironmentVariable("Company") + "Value Entry].[Item Ledger Entry Type]) = 9 then 'Asembly Output' " +
                    " end AS ItemLedgerEntryType," +
                    " dbo." + Environment.GetEnvironmentVariable("Company") + "Value Entry].[Cost per Unit] AS CostperUnit," +
                    " dbo." + Environment.GetEnvironmentVariable("Company") + "Value Entry].[User ID] AS UserID," +
                    " dbo." + Environment.GetEnvironmentVariable("Company") + "Value Entry].[Document No_] As DocumentNo," +
                    " dbo." + Environment.GetEnvironmentVariable("Company") + "Value Entry].Description AS Description," +
                    " CASE WHEN dbo." + Environment.GetEnvironmentVariable("Company") + "Value Entry].Adjustment = 0 then 'No'" +
                    "    WHEN  dbo." + Environment.GetEnvironmentVariable("Company") + "Value Entry].Adjustment =  1 then 'Yes' end AS Adjustment," +
                    " CASE WHEN convert(varchar,dbo." + Environment.GetEnvironmentVariable("Company") + "Value Entry].[Document Type]) = 0 then '' " +
                    "   WHEN convert(varchar,dbo." + Environment.GetEnvironmentVariable("Company") + "Value Entry].[Document Type]) = 1 then 'Sale Shipment' " +
                    "   WHEN convert(varchar,dbo." + Environment.GetEnvironmentVariable("Company") + "Value Entry].[Document Type]) = 2 then 'Sale Invoice' " +
                    "   WHEN convert(varchar,dbo." + Environment.GetEnvironmentVariable("Company") + "Value Entry].[Document Type]) = 3 then 'Sale Return Recitp' " +
                    "   WHEN convert(varchar,dbo." + Environment.GetEnvironmentVariable("Company") + "Value Entry].[Document Type]) = 4 then 'Sale Credit Memo' " +
                    "   WHEN convert(varchar,dbo." + Environment.GetEnvironmentVariable("Company") + "Value Entry].[Document Type]) = 5 then 'Purchase Recitp' " +
                    "   WHEN convert(varchar,dbo." + Environment.GetEnvironmentVariable("Company") + "Value Entry].[Document Type]) = 6 then 'Purchase Credit Memmo'" +
                    "   WHEN convert(varchar,dbo." + Environment.GetEnvironmentVariable("Company") + "Value Entry].[Document Type]) = 7 then 'Purchase Return Shipment' " +
                    "   WHEN convert(varchar,dbo." + Environment.GetEnvironmentVariable("Company") + "Value Entry].[Document Type]) = 8 then 'Purchase Credit Memmo' " +
                    "   WHEN convert(varchar,dbo." + Environment.GetEnvironmentVariable("Company") + "Value Entry].[Document Type]) = 9 then 'Tranfer Shipment' " +
                    "   WHEN convert(varchar,dbo." + Environment.GetEnvironmentVariable("Company") + "Value Entry].[Document Type]) = 10 then 'Tranfer Recitp' " +
                    "   WHEN convert(varchar,dbo." + Environment.GetEnvironmentVariable("Company") + "Value Entry].[Document Type]) = 11 then 'Service Shipment' " +
                    "   WHEN convert(varchar,dbo." + Environment.GetEnvironmentVariable("Company") + "Value Entry].[Document Type]) = 12 then 'Service Invoice' " +
                    "   WHEN convert(varchar,dbo." + Environment.GetEnvironmentVariable("Company") + "Value Entry].[Document Type]) = 13 then 'Service Credit Memo' " +
                    "   WHEN convert(varchar,dbo." + Environment.GetEnvironmentVariable("Company") + "Value Entry].[Document Type]) = 14 then 'Posted Assembly' " +
                    " end AS DocumentType," +
                    " dbo." + Environment.GetEnvironmentVariable("Company") + "Value Entry].[Cost Amount (Actual)] AS CostAmountActual" +
                    " FROM " +
                    " dbo." + Environment.GetEnvironmentVariable("Company") + "Value Entry]" +
                    " WHERE dbo." + Environment.GetEnvironmentVariable("Company") + "Value Entry].[User ID]  = {0} AND dbo." + Environment.GetEnvironmentVariable("Company") + "Value Entry].[Posting Date]>={1} and dbo." + Environment.GetEnvironmentVariable("Company") + "Value Entry].[Posting Date]<={2}" +
                    " ORDER BY dbo." + Environment.GetEnvironmentVariable("Company") + "Value Entry].[Entry No_] ASC";

               //SqlParameter parameteruser = new SqlParameter("@user", user);
               //SqlParameter parameterStartdate = new SqlParameter("@startdate", StartDate);
               //SqlParameter parameterEndDate = new SqlParameter("@enddate", EndDate);

                ValueEntryDATA = _navcontext.ValueEntries.FromSqlRaw(querydata, user, StartDate, EndDate).ToList();
            }


            response = Ok(new { data = ValueEntryDATA });


            return response;


        
        }
    }
}