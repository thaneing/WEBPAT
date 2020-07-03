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
    public class RentalsController : BaseController
    {
        private readonly NAVContext _navcontext;
        private readonly DatabaseContext _context;


        public RentalsController(NAVContext navcontext, DatabaseContext context)
        {
            _context = context;
            _navcontext = navcontext;
        }

        public IActionResult Index()
        {

            /*Check Session */
            var page = "187";
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

            var queryData1 = "SELECT dbo."+ Environment.GetEnvironmentVariable("Company") +"Location].Code AS name,dbo."+ Environment.GetEnvironmentVariable("Company") +"Location].Name as code FROM [dbo]."+ Environment.GetEnvironmentVariable("Company") +"Location] order by dbo."+ Environment.GetEnvironmentVariable("Company") +"Location].Code asc ";
            var sourceAutoCompletes = _navcontext.sourceAutoCompletes.FromSqlRaw(queryData1).ToList();
            ViewData["SourceAutoCompletes"] = sourceAutoCompletes;

            return View();
        }



        public IActionResult Gendata(string site)
        {


            /*Check Session */
            var page = "187";
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

            var queryData1 = "SELECT dbo."+ Environment.GetEnvironmentVariable("Company") +"Location].Code AS name,dbo."+ Environment.GetEnvironmentVariable("Company") +"Location].Name as code FROM [dbo]."+ Environment.GetEnvironmentVariable("Company") +"Location] order by dbo."+ Environment.GetEnvironmentVariable("Company") +"Location].Code asc ";
            var sourceAutoCompletes = _navcontext.sourceAutoCompletes.FromSqlRaw(queryData1).ToList();
            ViewData["SourceAutoCompletes"] = sourceAutoCompletes;


            IActionResult response = Unauthorized();
            List<Rental> APPVDATA;
            var queryData = "";
            if (site == null) {
                queryData = "SELECT *, " +
               "(SELECT dbo."+ Environment.GetEnvironmentVariable("Company") +"Job Ledger Entry].Description FROM dbo."+ Environment.GetEnvironmentVariable("Company") +"Job Ledger Entry] WHERE dbo."+ Environment.GetEnvironmentVariable("Company") +"Job Ledger Entry].[Entry No_]=a.JobLedgerEntry) as Description," +
               //"(SELECT ISNULL(Sum(dbo."+ Environment.GetEnvironmentVariable("Company") +"Job Ledger Entry].Quantity),0) AS Qty FROM dbo."+ Environment.GetEnvironmentVariable("Company") +"Job Ledger Entry] WHERE [Rental Applies-from Entry No_]=a.JobLedgerEntry) as ReturnQty," +

                //"(a.Quantity-(SELECT ISNULL(Sum(dbo." + Environment.GetEnvironmentVariable("Company") + "Job Ledger Entry].Quantity),0) AS Qty FROM dbo." + Environment.GetEnvironmentVariable("Company") + "Job Ledger Entry] WHERE [Rental Applies-from Entry No_]=a.JobLedgerEntry)) as ReturnQty," +
                   "(a.Quantity-(SELECT ISNULL(Sum(dbo." + Environment.GetEnvironmentVariable("Company") + "Rental Recurring].[Remaining Quantity]),0) AS Qty FROM dbo." + Environment.GetEnvironmentVariable("Company") + "Rental Recurring] WHERE [job Ledger Entry No_] = a.JobLedgerEntry)) as ReturnQty , " +
                  "(SELECT ISNULL(Sum(dbo." + Environment.GetEnvironmentVariable("Company") + "Rental Recurring].[Remaining Quantity]),0) AS Qty FROM dbo." + Environment.GetEnvironmentVariable("Company") + "Rental Recurring] WHERE [job Ledger Entry No_] = a.JobLedgerEntry) as Diff " +

                "FROM (  " +
               "SELECT " +
               "dbo."+ Environment.GetEnvironmentVariable("Company") +"Rental Recurring].[job Ledger Entry No_] as JobLedgerEntry, " +
               "convert(varchar, dbo."+ Environment.GetEnvironmentVariable("Company") +"Rental Recurring].[Posting Date], 23) as PostingDate, " +
               "dbo."+ Environment.GetEnvironmentVariable("Company") +"Rental Recurring].[Resource No_] as ResourceNo, "+
               "dbo." + Environment.GetEnvironmentVariable("Company") + "Rental Recurring].[FA No_] as FANo, " +//เพิ่ม FA No.
               "dbo." + Environment.GetEnvironmentVariable("Company") +"Rental Recurring].Quantity, " +
               "dbo."+ Environment.GetEnvironmentVariable("Company") +"Rental Recurring].[Job No_] as JobNo, " +
               "dbo."+ Environment.GetEnvironmentVariable("Company") +"Rental Recurring].[Job Task No_] as JobTaskNo, " +
               "dbo."+ Environment.GetEnvironmentVariable("Company") +"Rental Recurring].[Document No_] as DocumentNo " +
               "FROM " +
               "dbo."+ Environment.GetEnvironmentVariable("Company") +"Rental Recurring] " +
               "WHERE [End Recurring]=0 and [Rental Type]=2 and Original=1 )as a  ";
                //ViewBag.sql = queryData;
               APPVDATA = _navcontext.rentals.FromSqlRaw(queryData).ToList();
            }
            else
            { 
            queryData = "SELECT *, " +
                "(SELECT dbo."+ Environment.GetEnvironmentVariable("Company") +"Job Ledger Entry].Description FROM dbo."+ Environment.GetEnvironmentVariable("Company") +"Job Ledger Entry] WHERE dbo."+ Environment.GetEnvironmentVariable("Company") +"Job Ledger Entry].[Entry No_]=a.JobLedgerEntry) as Description," +
                //"(SELECT ISNULL(Sum(dbo."+ Environment.GetEnvironmentVariable("Company") + "Rental Recurring].[Remaining Quantity]),0) AS Qty FROM dbo." + Environment.GetEnvironmentVariable("Company") + "Rental Recurring] WHERE [job Ledger Entry No_] = a.JobLedgerEntry) as ReturnQty," +
                //"(a.Quantity-(SELECT ISNULL(Sum(dbo." + Environment.GetEnvironmentVariable("Company") +"Job Ledger Entry].Quantity),0) AS Qty FROM dbo."+ Environment.GetEnvironmentVariable("Company") +"Job Ledger Entry] WHERE [Rental Applies-from Entry No_]=a.JobLedgerEntry)) as ReturnQty," +
                //"(SELECT ISNULL(Sum(dbo." + Environment.GetEnvironmentVariable("Company") + "Rental Recurring].[Remaining Quantity]),0) AS Qty FROM dbo." + Environment.GetEnvironmentVariable("Company") + "Rental Recurring] WHERE [job Ledger Entry No_] = a.JobLedgerEntry)) as ReturnQty "+

                 "(a.Quantity-(SELECT ISNULL(Sum(dbo." + Environment.GetEnvironmentVariable("Company") + "Rental Recurring].[Remaining Quantity]),0) AS Qty FROM dbo." + Environment.GetEnvironmentVariable("Company") + "Rental Recurring] WHERE [job Ledger Entry No_] = a.JobLedgerEntry)) as ReturnQty , " +
                "(SELECT ISNULL(Sum(dbo." + Environment.GetEnvironmentVariable("Company") + "Rental Recurring].[Remaining Quantity]),0) AS Qty FROM dbo." + Environment.GetEnvironmentVariable("Company") + "Rental Recurring] WHERE [job Ledger Entry No_] = a.JobLedgerEntry) as Diff " +
                "FROM (  " +
                "SELECT " +
                "dbo."+ Environment.GetEnvironmentVariable("Company") +"Rental Recurring].[job Ledger Entry No_] as JobLedgerEntry, " +
                "convert(varchar, dbo."+ Environment.GetEnvironmentVariable("Company") +"Rental Recurring].[Posting Date], 23) as PostingDate, " +
                "dbo."+ Environment.GetEnvironmentVariable("Company") +"Rental Recurring].[Resource No_] as ResourceNo, " +
               "dbo." + Environment.GetEnvironmentVariable("Company") + "Rental Recurring].[FA No_] as FANo, " +//เพิ่ม FA No.
                "dbo." + Environment.GetEnvironmentVariable("Company") +"Rental Recurring].Quantity, " +
                "dbo."+ Environment.GetEnvironmentVariable("Company") +"Rental Recurring].[Job No_] as JobNo, " +
                "dbo."+ Environment.GetEnvironmentVariable("Company") +"Rental Recurring].[Job Task No_] as JobTaskNo, " +
                "dbo."+ Environment.GetEnvironmentVariable("Company") +"Rental Recurring].[Document No_] as DocumentNo " +
                "FROM " +
                "dbo."+ Environment.GetEnvironmentVariable("Company") +"Rental Recurring] " +
                "WHERE [End Recurring]=0 and [Rental Type]=2 and Original=1 and dbo."+ Environment.GetEnvironmentVariable("Company") +"Rental Recurring].[Job No_]={0})as a  ";
                //SqlParameter parameterSite = new SqlParameter("@site", site);
                APPVDATA = _navcontext.rentals.FromSqlRaw(queryData, site).ToList();
            }

        


            response = Ok(new { data = APPVDATA });


            return response;

            //return View();
        }

        public IActionResult GenReturndata(string job, string item)
        {


            /*Check Session */
            var page = "188";
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

            if (job == null)
            {
                job = "";
            }
            if (item == null)
            {
                item = "";
            }


            IActionResult response = Unauthorized();
            var queryData = "SELECT " +
                "dbo."+ Environment.GetEnvironmentVariable("Company") +"Job Ledger Entry].[Entry No_] AS EntryNo," +
                "dbo."+ Environment.GetEnvironmentVariable("Company") +"Job Ledger Entry].[Rental Applies-from Entry No_] AS RentalApply," +
                "dbo."+ Environment.GetEnvironmentVariable("Company") +"Job Ledger Entry].[Job No_] AS JobNo, " +
                "convert(varchar, dbo."+ Environment.GetEnvironmentVariable("Company") +"Job Ledger Entry].[Posting Date],23) AS PostingDate," +
                "dbo."+ Environment.GetEnvironmentVariable("Company") +"Job Ledger Entry].[Document No_] AS DocumentNo," +
                "dbo."+ Environment.GetEnvironmentVariable("Company") +"Job Ledger Entry].No_ AS ItemNo," +
                "dbo."+ Environment.GetEnvironmentVariable("Company") +"Resource].No_ as ResourceNo," +
                "dbo."+ Environment.GetEnvironmentVariable("Company") +"Job Ledger Entry].Description AS Description," +
                "dbo."+ Environment.GetEnvironmentVariable("Company") +"Job Ledger Entry].[Description 2] AS Description2," +
                "dbo."+ Environment.GetEnvironmentVariable("Company") +"Job Ledger Entry].Quantity," +
                "dbo."+ Environment.GetEnvironmentVariable("Company") +"Job Ledger Entry].[Unit of Measure Code] AS UnitOfMesure," +
                "dbo."+ Environment.GetEnvironmentVariable("Company") +"Job Ledger Entry].[Location Code] AS LocationCode," +
                "dbo."+ Environment.GetEnvironmentVariable("Company") +"Job Ledger Entry].[Journal Batch Name] AS JournalBatch," +
                "convert(varchar,dbo."+ Environment.GetEnvironmentVariable("Company") +"Job Ledger Entry].[Document Date],23) AS DocumentDate," +
                "dbo."+ Environment.GetEnvironmentVariable("Company") +"Job Ledger Entry].[Job Task No_] AS JobTaskNo," +
                "dbo."+ Environment.GetEnvironmentVariable("Company") +"Job Ledger Entry].[Ledger Entry Type] AS LedgerEntryType," +
                "dbo."+ Environment.GetEnvironmentVariable("Company") +"Job Ledger Entry].[Shipment No_] AS ShipmentNo," +
                "dbo."+ Environment.GetEnvironmentVariable("Company") +"Job Ledger Entry].Remark," +
                "CASE WHEN dbo."+ Environment.GetEnvironmentVariable("Company") +"Job Ledger Entry].[Rental Type]=0 then '' " +
                "	WHEN dbo."+ Environment.GetEnvironmentVariable("Company") +"Job Ledger Entry].[Rental Type]=1 then 'Short Term' " +
                "		WHEN dbo."+ Environment.GetEnvironmentVariable("Company") +"Job Ledger Entry].[Rental Type]=2 then 'Long Term' end AS RentalType, " +
                "dbo."+ Environment.GetEnvironmentVariable("Company") +"Job Ledger Entry].[To Location] AS ToLocation," +
                "dbo."+ Environment.GetEnvironmentVariable("Company") +"Job Ledger Entry].[From Location] AS FromLocation, " +
                "dbo." + Environment.GetEnvironmentVariable("Company") + "Job Ledger Entry].[FA No_] AS FANo " +
                "FROM " +
                "dbo."+ Environment.GetEnvironmentVariable("Company") +"Job Ledger Entry] " +
                "INNER JOIN dbo."+ Environment.GetEnvironmentVariable("Company") +"Resource] ON dbo."+ Environment.GetEnvironmentVariable("Company") +"Job Ledger Entry].No_ = dbo."+ Environment.GetEnvironmentVariable("Company") +"Resource].[Link to Item No_] " +
                "WHERE " +
                "dbo."+ Environment.GetEnvironmentVariable("Company") +"Job Ledger Entry].[Type of task] = 1 AND " +
                "dbo."+ Environment.GetEnvironmentVariable("Company") +"Job Ledger Entry].[Document No_] NOT LIKE 'CAL%' AND " +
                "(dbo."+ Environment.GetEnvironmentVariable("Company") +"Job Ledger Entry].No_ LIKE 'PC%' OR " +
                "dbo."+ Environment.GetEnvironmentVariable("Company") +"Job Ledger Entry].No_ LIKE 'FA%') " +
                "AND dbo."+ Environment.GetEnvironmentVariable("Company") +"Resource].No_  Like '%'+{1}+'%'  " +
                "and dbo."+ Environment.GetEnvironmentVariable("Company") +"Job Ledger Entry].[Job No_] Like '%'+{0}+'%' ";


            //SqlParameter parameterJob= new SqlParameter("@job", job.ToUpper());
            //SqlParameter parameterItem = new SqlParameter("@item", item.ToUpper());

            //ViewBag.sql = queryData;
            var listReturns = _navcontext.listReturns.FromSqlRaw(queryData, job.ToUpper(), item.ToUpper()).ToList();


            response = Ok(new { data = listReturns });


            return response;

            //return View();
        }



        public IActionResult Returndata()
        {


            /*Check Session */
            var page = "188";
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

            var queryData1 = "  SELECT No_ as name,No_ as code FROM[dbo]."+ Environment.GetEnvironmentVariable("Company") +"Job] order by No_ asc ";
            var sourceAutoCompletes = _navcontext.sourceAutoCompletes.FromSqlRaw(queryData1).ToList();
            ViewData["SourceAutoCompletes"] = sourceAutoCompletes;
        

            return View();
        }







        }
    }