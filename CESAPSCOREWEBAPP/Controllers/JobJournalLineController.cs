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
    public class JobJournalLineController : BaseController
    {
        private readonly NAVContext _navcontext;
        private readonly DatabaseContext _context;

        public JobJournalLineController(NAVContext navcontext, DatabaseContext context)
        {
            _context = context;
            _navcontext = navcontext;
        }
        public IActionResult Index()
        {
            /*Check Session */
            var page = "231";
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

            var queryData1 = "SELECT dbo." + Environment.GetEnvironmentVariable("Company") + "Location].Code AS name,dbo." + Environment.GetEnvironmentVariable("Company") + "Location].Name as code FROM [dbo]." + Environment.GetEnvironmentVariable("Company") + "Location] order by dbo." + Environment.GetEnvironmentVariable("Company") + "Location].Code asc ";
            var sourceAutoCompletes = _navcontext.sourceAutoCompletes.FromSqlRaw(queryData1).ToList();
            ViewData["SourceAutoCompletes"] = sourceAutoCompletes;

          

            return View();
        }

        public IActionResult GetData(string site)
        {
            /*Check Session */
            var page = "231";
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


            var queryData1 = "SELECT dbo." + Environment.GetEnvironmentVariable("Company") + "Location].Code AS name,dbo." + Environment.GetEnvironmentVariable("Company") + "Location].Name as code FROM [dbo]." + Environment.GetEnvironmentVariable("Company") + "Location] order by dbo." + Environment.GetEnvironmentVariable("Company") + "Location].Code asc ";
            var sourceAutoCompletes = _navcontext.sourceAutoCompletes.FromSqlRaw(queryData1).ToList();
            ViewData["SourceAutoCompletes"] = sourceAutoCompletes;

            IActionResult response = Unauthorized();
            List<JobJournalLine> JobJournalLineDATA;
            var querydata = "";

            if (site == null)
            {
                querydata = "SELECT  ROW_NUMBER() OVER(ORDER BY [Journal Batch Name]) as ID," +
                  " dbo." + Environment.GetEnvironmentVariable("Company") + "Job Journal Line].[Journal Batch Name] AS BatchName," +
                  " convert(varchar, dbo." + Environment.GetEnvironmentVariable("Company") + "Job Journal Line].[Shipment Date],23) AS ShipmentDate," +
                  " dbo." + Environment.GetEnvironmentVariable("Company") + "Job Journal Line].[Shipment No_] AS ShipmentNo," +
                  " dbo." + Environment.GetEnvironmentVariable("Company") + "Job Journal Line].[Job No_] AS JobNo," +
                  " dbo." + Environment.GetEnvironmentVariable("Company") + "Job Journal Line].[Document No_] AS DocumentNo ," +
                  " dbo." + Environment.GetEnvironmentVariable("Company") + "Job Journal Line].No_ AS No," +
                  " dbo." + Environment.GetEnvironmentVariable("Company") + "Job Journal Line].Description AS Description," +
                  //" dbo." + Environment.GetEnvironmentVariable("Company") + "Job Journal Line].Quantity AS Quantity," +
                  //" CASE WHEN dbo." + Environment.GetEnvironmentVariable("Company") + "Job Journal Line].[Type of task] = 0 then '0.00'" +
                  "  CASE   WHEN dbo." + Environment.GetEnvironmentVariable("Company") + "Job Journal Line].[Type of task] = 1 then dbo." + Environment.GetEnvironmentVariable("Company") + "Job Journal Line].Quantity" +
                  "       WHEN dbo." + Environment.GetEnvironmentVariable("Company") + "Job Journal Line].[Type of task] = 2 then dbo." + Environment.GetEnvironmentVariable("Company") + "Job Journal Line].[Rent_ Qty]" +
                  "           WHEN dbo." + Environment.GetEnvironmentVariable("Company") + "Job Journal Line].[Type of task] = 3 then dbo." + Environment.GetEnvironmentVariable("Company") + "Job Journal Line].Quantity end AS Quantity," +
                  " dbo." + Environment.GetEnvironmentVariable("Company") + "Job Journal Line].[Location Code] AS FormLocation," +
                  " dbo." + Environment.GetEnvironmentVariable("Company") + "Job Journal Line].[Shortcut Dimension 2 Code] AS CostCode," +
                  " dbo." + Environment.GetEnvironmentVariable("Company") + "Job Journal Line].[Job Task No_] AS JobTaskNo," +
                  " dbo." + Environment.GetEnvironmentVariable("Company") + "Job Journal Line].[Transfer Location Code] AS ToLocation," +
                  " CASE WHEN dbo." + Environment.GetEnvironmentVariable("Company") + "Job Journal Line].[Rental Type] = 0 then ''" +
                  "   WHEN dbo." + Environment.GetEnvironmentVariable("Company") + "Job Journal Line].[Rental Type] = 1 then 'Short Term'" +
                  "        WHEN dbo." + Environment.GetEnvironmentVariable("Company") + "Job Journal Line].[Rental Type] = 2 then 'Long Term'" +
                  " end AS RentalType," +
                   " CASE WHEN dbo." + Environment.GetEnvironmentVariable("Company") + "Job Journal Line].Type = 0 then 'Resourse'" +
                  "   WHEN dbo." + Environment.GetEnvironmentVariable("Company") + "Job Journal Line].Type = 1 then 'Item' " +
                  " end AS Type," +
                  " convert(varchar,dbo." + Environment.GetEnvironmentVariable("Company") + "Job Journal Line].[Remaining Confirm Quantity]) AS RemConfirmQty," +
                  " CASE WHEN dbo." + Environment.GetEnvironmentVariable("Company") + "Job Journal Line].[Type of task] = 0 then ''" +
                  "     WHEN dbo." + Environment.GetEnvironmentVariable("Company") + "Job Journal Line].[Type of task] = 1 then 'Return'" +
                  "       WHEN dbo." + Environment.GetEnvironmentVariable("Company") + "Job Journal Line].[Type of task] = 2 then 'Rental'" +
                  "           WHEN dbo." + Environment.GetEnvironmentVariable("Company") + "Job Journal Line].[Type of task] = 3 then 'Issue' end AS Typeoftask" +
                  " FROM" +
                  " dbo." + Environment.GetEnvironmentVariable("Company") + "Job Journal Line]" +
                  " WHERE " +
                  " dbo." + Environment.GetEnvironmentVariable("Company") + "Job Journal Line].No_ !='' ";


                JobJournalLineDATA = _navcontext.jobJournalLines.FromSqlRaw(querydata).ToList();
            }
            else
            {
                querydata = "SELECT  ROW_NUMBER() OVER(ORDER BY [Journal Batch Name]) as ID," +
                 " dbo." + Environment.GetEnvironmentVariable("Company") + "Job Journal Line].[Journal Batch Name] AS BatchName," +
                    " convert(varchar, dbo." + Environment.GetEnvironmentVariable("Company") + "Job Journal Line].[Shipment Date],23) AS ShipmentDate," +
                    " dbo." + Environment.GetEnvironmentVariable("Company") + "Job Journal Line].[Shipment No_] AS ShipmentNo," +
                    " dbo." + Environment.GetEnvironmentVariable("Company") + "Job Journal Line].[Job No_] AS JobNo," +
                    " dbo." + Environment.GetEnvironmentVariable("Company") + "Job Journal Line].[Document No_] AS DocumentNo ," +
                    " dbo." + Environment.GetEnvironmentVariable("Company") + "Job Journal Line].No_ AS No," +
                    " dbo." + Environment.GetEnvironmentVariable("Company") + "Job Journal Line].Description AS Description," +
                      //" dbo." + Environment.GetEnvironmentVariable("Company") + "Job Journal Line].Quantity AS Quantity," +
                      "  CASE   WHEN dbo." + Environment.GetEnvironmentVariable("Company") + "Job Journal Line].[Type of task] = 1 then dbo." + Environment.GetEnvironmentVariable("Company") + "Job Journal Line].Quantity" +
                    "       WHEN dbo." + Environment.GetEnvironmentVariable("Company") + "Job Journal Line].[Type of task] = 2 then dbo." + Environment.GetEnvironmentVariable("Company") + "Job Journal Line].[Rent_ Qty]" +
                    "           WHEN dbo." + Environment.GetEnvironmentVariable("Company") + "Job Journal Line].[Type of task] = 3 then dbo." + Environment.GetEnvironmentVariable("Company") + "Job Journal Line].Quantity end AS Quantity," +
                    " dbo." + Environment.GetEnvironmentVariable("Company") + "Job Journal Line].[Location Code] AS FormLocation," +
                    " dbo." + Environment.GetEnvironmentVariable("Company") + "Job Journal Line].[Shortcut Dimension 2 Code] AS CostCode," +
                    " dbo." + Environment.GetEnvironmentVariable("Company") + "Job Journal Line].[Job Task No_] AS JobTaskNo," +
                    " dbo." + Environment.GetEnvironmentVariable("Company") + "Job Journal Line].[Transfer Location Code] AS ToLocation," +
                    " CASE WHEN dbo." + Environment.GetEnvironmentVariable("Company") + "Job Journal Line].[Rental Type] = 0 then ''" +
                    "   WHEN dbo." + Environment.GetEnvironmentVariable("Company") + "Job Journal Line].[Rental Type] = 1 then 'Short Term'" +
                    "        WHEN dbo." + Environment.GetEnvironmentVariable("Company") + "Job Journal Line].[Rental Type] = 2 then 'Long Term'" +
                    " end AS RentalType," +
                     " CASE WHEN dbo." + Environment.GetEnvironmentVariable("Company") + "Job Journal Line].Type = 0 then 'Resourse'" +
                    "   WHEN dbo." + Environment.GetEnvironmentVariable("Company") + "Job Journal Line].Type = 1 then 'Item' " +
                    " end AS Type," +
                    " convert(varchar,dbo." + Environment.GetEnvironmentVariable("Company") + "Job Journal Line].[Remaining Confirm Quantity]) AS RemConfirmQty," +
                    " CASE WHEN dbo." + Environment.GetEnvironmentVariable("Company") + "Job Journal Line].[Type of task] = 0 then ''" +
                    "     WHEN dbo." + Environment.GetEnvironmentVariable("Company") + "Job Journal Line].[Type of task] = 1 then 'Return'" +
                    "       WHEN dbo." + Environment.GetEnvironmentVariable("Company") + "Job Journal Line].[Type of task] = 2 then 'Rental'" +
                    "           WHEN dbo." + Environment.GetEnvironmentVariable("Company") + "Job Journal Line].[Type of task] = 3 then 'Issue' end AS Typeoftask" +
                    " FROM" +
                    " dbo." + Environment.GetEnvironmentVariable("Company") + "Job Journal Line]" +
                    " WHERE " +
                    " dbo." + Environment.GetEnvironmentVariable("Company") + "Job Journal Line].No_ !='' and dbo." + Environment.GetEnvironmentVariable("Company") + "Job Journal Line].[Job No_]  = {0}";

                //SqlParameter parameterSite = new SqlParameter("@site", site);

                JobJournalLineDATA = _navcontext.jobJournalLines.FromSqlRaw(querydata, site).ToList();
            }


            response = Ok(new { data = JobJournalLineDATA });


            return response;
        }
       
    }
}