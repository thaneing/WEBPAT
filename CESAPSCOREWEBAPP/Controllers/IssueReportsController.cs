using System;
using System.Collections.Generic;
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
    public class IssueReportsController : BaseController
    {
        private readonly NAVContext _navcontext;
        private readonly DatabaseContext _context;

        public IssueReportsController(NAVContext navcontext, DatabaseContext context)
        {
            _context = context;
            _navcontext = navcontext;
        }
        public IActionResult Index()
        {
            /*Check Session */
            var page = "253";
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

            var queryData1 = "SELECT dbo." + Environment.GetEnvironmentVariable("Company") + "Location].Code AS name,dbo." + Environment.GetEnvironmentVariable("Company") + "Location].Name as code FROM [dbo]." + Environment.GetEnvironmentVariable("Company") + "Location] order by dbo." + Environment.GetEnvironmentVariable("Company") + "Location].Code asc ";
            var sourceAutoCompletes = _navcontext.sourceAutoCompletes.FromSqlRaw(queryData1).ToList();
            ViewData["SourceAutoCompletes"] = sourceAutoCompletes;
            return View();
        }
        public IActionResult Getdata(string sites)
        {
            /*Check Session */
            var page = "253";
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
            List<IssueReport> IssueDATA;

            if(sites == null)
            {
                var querydataIssue = "SELECT ROW_NUMBER() OVER (ORDER BY b.ItemNo) AS ID," +
                     " b.ItemNo As  ItemNo,b.Des,b.IssueQty AS IssueQty,b.ReturnQty AS ReturnQty,(b.IssueQty - b.ReturnQty) AS Diff,b.Site AS JobNo,b.UnitCosAvg As UnitCostAvg,(b.OIS_Total - b.OR_Total) as OTotal,b.unit AS UnitOfMesure " +
                     " FROM (SELECT *," +
                     "  (SELECT Top 1 dbo." + Environment.GetEnvironmentVariable("Company") + "Job Ledger Entry].Description FROM dbo." + Environment.GetEnvironmentVariable("Company") + "Job Ledger Entry] WHERE dbo." + Environment.GetEnvironmentVariable("Company") + "Job Ledger Entry].No_ = a.ItemNo) as Des," +
                     "(SELECT Top 1  dbo." + Environment.GetEnvironmentVariable("Company") + "Job Ledger Entry].[Unit of Measure Code] FROM dbo.[C_E_S_ CO_, LTD_$Job Ledger Entry] WHERE dbo.[C_E_S_ CO_, LTD_$Job Ledger Entry].No_ =a.ItemNo) as unit," +
                     " (SELECT ISNULL(SUM(dbo.[C_E_S_ CO_, LTD_$Job Ledger Entry].Quantity),0) FROM  dbo." + Environment.GetEnvironmentVariable("Company") + "Job Ledger Entry]  WHERE  dbo." + Environment.GetEnvironmentVariable("Company") + "Job Ledger Entry].No_ = a.ItemNo and dbo." + Environment.GetEnvironmentVariable("Company") + "Job Ledger Entry].[Type of task] = 3 AND dbo." + Environment.GetEnvironmentVariable("Company") + "Job Ledger Entry].[Job No_] = a.Site) as IssueQty," +
                     " (SELECT ISNULL(SUM(dbo." + Environment.GetEnvironmentVariable("Company") + "Job Ledger Entry].Quantity), 0) FROM dbo." + Environment.GetEnvironmentVariable("Company") + "Job Ledger Entry] WHERE dbo." + Environment.GetEnvironmentVariable("Company") + "Job Ledger Entry].No_ = a.ItemNo and  dbo." + Environment.GetEnvironmentVariable("Company") + "Job Ledger Entry].[Type of task] = 1 AND dbo." + Environment.GetEnvironmentVariable("Company") + "Job Ledger Entry].[Job No_] = a.Site) as ReturnQty," +
                     " (SELECT Avg(dbo." + Environment.GetEnvironmentVariable("Company") + "Job Ledger Entry].[Unit Cost]) FROM dbo." + Environment.GetEnvironmentVariable("Company") + "Job Ledger Entry] WHERE dbo." + Environment.GetEnvironmentVariable("Company") + "Job Ledger Entry].No_ = a.ItemNo) as UnitCosAvg," +

                     " (SELECT ISNULL(SUM(dbo." + Environment.GetEnvironmentVariable("Company") + "Job Ledger Entry].[Original Total Cost]),0) FROM dbo." + Environment.GetEnvironmentVariable("Company") + "Job Ledger Entry] WHERE dbo." + Environment.GetEnvironmentVariable("Company") + "Job Ledger Entry].No_ = a.ItemNo and dbo." + Environment.GetEnvironmentVariable("Company") + "Job Ledger Entry].[Job No_] = a.Site AND  dbo." + Environment.GetEnvironmentVariable("Company") + "Job Ledger Entry].[Type of task] = 3) as OIS_Total," +

                     " (SELECT ISNULL(SUM(dbo." + Environment.GetEnvironmentVariable("Company") + "Job Ledger Entry].[Original Total Cost]),0) FROM  dbo." + Environment.GetEnvironmentVariable("Company") + "Job Ledger Entry]  WHERE dbo." + Environment.GetEnvironmentVariable("Company") + "Job Ledger Entry].No_ = a.ItemNo and dbo." + Environment.GetEnvironmentVariable("Company") + "Job Ledger Entry].[Job No_] = a.Site AND dbo." + Environment.GetEnvironmentVariable("Company") + "Job Ledger Entry].[Type of task] = 1) as OR_Total" +

                     " FROM" +
                     " (SELECT dbo." + Environment.GetEnvironmentVariable("Company") + "Job Ledger Entry].No_ AS ItemNo, " +
                     " dbo." + Environment.GetEnvironmentVariable("Company") + "Job Ledger Entry].[Job No_] AS Site" +
                     " FROM dbo." + Environment.GetEnvironmentVariable("Company") + "Job Ledger Entry] WHERE (dbo." + Environment.GetEnvironmentVariable("Company") + "Job Ledger Entry].No_ BETWEEN '010000000000' AND '129999999999') AND dbo." + Environment.GetEnvironmentVariable("Company") + "Job Ledger Entry].[Posting Date] >='2018-10-23 00:00:00'" +
                     " GROUP BY  dbo." + Environment.GetEnvironmentVariable("Company") + "Job Ledger Entry].No_, dbo." + Environment.GetEnvironmentVariable("Company") + "Job Ledger Entry].[Job No_]) as a) as b WHERE b.IssueQty !=0 OR b.ReturnQty !=0"; 
                
                IssueDATA = _navcontext.IssueReports.FromSqlRaw(querydataIssue).ToList();
            }
            else
            {
                var querydataIssue = "SELECT ROW_NUMBER() OVER (ORDER BY b.ItemNo) AS ID," +
                     " b.ItemNo As  ItemNo,b.Des AS Des,b.IssueQty AS IssueQty,b.ReturnQty AS ReturnQty,(b.IssueQty - b.ReturnQty) AS Diff,b.Site AS JobNo,b.UnitCosAvg As UnitCostAvg,(b.OIS_Total - b.OR_Total) as OTotal,b.unit AS UnitOfMesure " +
                    " FROM (SELECT *," +
                    "  (SELECT Top 1 dbo." + Environment.GetEnvironmentVariable("Company") + "Job Ledger Entry].Description FROM dbo." + Environment.GetEnvironmentVariable("Company") + "Job Ledger Entry] WHERE dbo." + Environment.GetEnvironmentVariable("Company") + "Job Ledger Entry].No_ = a.ItemNo) as Des," +
                    "(SELECT Top 1  dbo." + Environment.GetEnvironmentVariable("Company") + "Job Ledger Entry].[Unit of Measure Code] FROM dbo.[C_E_S_ CO_, LTD_$Job Ledger Entry] WHERE dbo.[C_E_S_ CO_, LTD_$Job Ledger Entry].No_ =a.ItemNo) as unit," +
                    " (SELECT ISNULL(SUM(dbo.[C_E_S_ CO_, LTD_$Job Ledger Entry].Quantity),0) FROM  dbo." + Environment.GetEnvironmentVariable("Company") + "Job Ledger Entry]  WHERE  dbo." + Environment.GetEnvironmentVariable("Company") + "Job Ledger Entry].No_ = a.ItemNo and dbo." + Environment.GetEnvironmentVariable("Company") + "Job Ledger Entry].[Type of task] = 3 AND dbo." + Environment.GetEnvironmentVariable("Company") + "Job Ledger Entry].[Job No_] = a.Site) as IssueQty," +
                    " (SELECT ISNULL(SUM(dbo." + Environment.GetEnvironmentVariable("Company") + "Job Ledger Entry].Quantity), 0) FROM dbo." + Environment.GetEnvironmentVariable("Company") + "Job Ledger Entry] WHERE dbo." + Environment.GetEnvironmentVariable("Company") + "Job Ledger Entry].No_ = a.ItemNo and  dbo." + Environment.GetEnvironmentVariable("Company") + "Job Ledger Entry].[Type of task] = 1 AND dbo." + Environment.GetEnvironmentVariable("Company") + "Job Ledger Entry].[Job No_] = a.Site) as ReturnQty," +
                    " (SELECT Avg(dbo." + Environment.GetEnvironmentVariable("Company") + "Job Ledger Entry].[Unit Cost]) FROM dbo." + Environment.GetEnvironmentVariable("Company") + "Job Ledger Entry] WHERE dbo." + Environment.GetEnvironmentVariable("Company") + "Job Ledger Entry].No_ = a.ItemNo) as UnitCosAvg," +

                    " (SELECT ISNULL(SUM(dbo." + Environment.GetEnvironmentVariable("Company") + "Job Ledger Entry].[Original Total Cost]),0) FROM dbo." + Environment.GetEnvironmentVariable("Company") + "Job Ledger Entry] WHERE dbo." + Environment.GetEnvironmentVariable("Company") + "Job Ledger Entry].No_ = a.ItemNo and dbo." + Environment.GetEnvironmentVariable("Company") + "Job Ledger Entry].[Job No_] = a.Site AND  dbo." + Environment.GetEnvironmentVariable("Company") + "Job Ledger Entry].[Type of task] = 3) as OIS_Total," +

                    " (SELECT ISNULL(SUM(dbo." + Environment.GetEnvironmentVariable("Company") + "Job Ledger Entry].[Original Total Cost]),0) FROM  dbo." + Environment.GetEnvironmentVariable("Company") + "Job Ledger Entry]  WHERE dbo." + Environment.GetEnvironmentVariable("Company") + "Job Ledger Entry].No_ = a.ItemNo and dbo." + Environment.GetEnvironmentVariable("Company") + "Job Ledger Entry].[Job No_] = a.Site AND dbo." + Environment.GetEnvironmentVariable("Company") + "Job Ledger Entry].[Type of task] = 1) as OR_Total" +

                    " FROM" +
                    " (SELECT dbo." + Environment.GetEnvironmentVariable("Company") + "Job Ledger Entry].No_ AS ItemNo, " +
                    " dbo." + Environment.GetEnvironmentVariable("Company") + "Job Ledger Entry].[Job No_] AS Site" +
                    " FROM dbo." + Environment.GetEnvironmentVariable("Company") + "Job Ledger Entry] WHERE (dbo." + Environment.GetEnvironmentVariable("Company") + "Job Ledger Entry].No_ BETWEEN '010000000000' AND '129999999999') AND dbo." + Environment.GetEnvironmentVariable("Company") + "Job Ledger Entry].[Posting Date] >='2018-10-23 00:00:00' AND dbo.[C_E_S_ CO_, LTD_$Job Ledger Entry].[Job No_] = {0}" +
                    " GROUP BY  dbo." + Environment.GetEnvironmentVariable("Company") + "Job Ledger Entry].No_, dbo." + Environment.GetEnvironmentVariable("Company") + "Job Ledger Entry].[Job No_]) as a) as b WHERE b.IssueQty !=0 OR b.ReturnQty !=0";

                IssueDATA = _navcontext.IssueReports.FromSqlRaw(querydataIssue, sites).ToList();

            }
            

            response = Ok(new { data = IssueDATA});


            return response;
        }
    }
}