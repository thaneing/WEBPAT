using System;
using System.Collections;
using System.Collections.Generic;
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
using static CESAPSCOREWEBAPP.Models.Enums;

namespace CESAPSCOREWEBAPP.Controllers
{
    [Authorize]
    public class AccountReportsController : BaseController
    {

        private readonly NAVContext _navcontext;
        private readonly DatabaseContext _context;


        public AccountReportsController(NAVContext navcontext, DatabaseContext context)
        {
            _context = context;
            _navcontext = navcontext;
        }


        public IActionResult Index()
        {
            /*Check Session */
            var page = "252";
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


            //Account No
            var query = "SELECT DISTINCT [G_L Account No_] as LocationCode,[G_L Account No_] as JobNo FROM dbo."+ Environment.GetEnvironmentVariable("Company") +"G_L Entry] ORDER BY [G_L Account No_] asc";
            var jobNo = _navcontext.v_Job.FromSqlRaw(query).ToList();
            
            var AccountNo = _context.ConditionReports.Where(p => p.ReportDimension == "AccountNo" && p.ReportName== "AccountProfit").ToList();
            ViewData["AccountNo"] = AccountNo;
            ViewData["JobNo"] = jobNo;


            //GL

            var queryGL = "SELECT  Code as JobNo ,Name as LocationCode  FROM dbo."+ Environment.GetEnvironmentVariable("Company") +"Dimension Value] WHERE [Dimension Code]='JOB_GL'";
            var jobNoGL = _navcontext.v_Job.FromSqlRaw(queryGL).ToList();
            ViewData["JobGL"] = jobNoGL;
            var AccountNoGL = _context.ConditionReports.Where(p => p.ReportDimension == "GLOpen" && p.ReportName == "AccountProfit").ToList();
            ViewData["AccountGL"] = AccountNoGL;




            return View();


        }


        public object Get(DataSourceLoadOptions loadOptions)
        {
      

            /*Check Session */
            var page = "252";
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


            var queryData = "SELECT dbo."+ Environment.GetEnvironmentVariable("Company") +"G_L Entry].[Entry No_] as ID, " +
                "CASE WHEN SUBSTRING(dbo."+ Environment.GetEnvironmentVariable("Company") +"G_L Entry].[G_L Account No_],8,1)='M' THEN  SUBSTRING(dbo."+ Environment.GetEnvironmentVariable("Company") +"G_L Entry].[G_L Account No_],1,1)+'999999M' " +
                " ELSE SUBSTRING(dbo."+ Environment.GetEnvironmentVariable("Company") +"G_L Entry].[G_L Account No_],1,1)+'9999999' END AS ACCLV4, " +
                "CASE WHEN SUBSTRING(dbo."+ Environment.GetEnvironmentVariable("Company") +"G_L Entry].[G_L Account No_],8,1)='M' THEN  SUBSTRING(dbo."+ Environment.GetEnvironmentVariable("Company") +"G_L Entry].[G_L Account No_],1,2)+'99999M' " +
                " ELSE SUBSTRING(dbo."+ Environment.GetEnvironmentVariable("Company") +"G_L Entry].[G_L Account No_],1,2)+'99999' END AS ACCLV3, " +
                "CASE WHEN SUBSTRING(dbo."+ Environment.GetEnvironmentVariable("Company") +"G_L Entry].[G_L Account No_],8,1)='M' THEN  SUBSTRING(dbo."+ Environment.GetEnvironmentVariable("Company") +"G_L Entry].[G_L Account No_],1,4)+'999M' " +
                " ELSE SUBSTRING(dbo."+ Environment.GetEnvironmentVariable("Company") +"G_L Entry].[G_L Account No_],1,5)+'999' END AS ACCLV2, " +
                "dbo."+ Environment.GetEnvironmentVariable("Company") +"G_L Entry].[G_L Account No_] AS ACCLV1, " +
                "dbo."+ Environment.GetEnvironmentVariable("Company") +"G_L Entry].[Global Dimension 1 Code]  AS JobGL, " +
                "dbo."+ Environment.GetEnvironmentVariable("Company") +"G_L Entry].[Posting Date] AS PostingDate, " +
                "dbo."+ Environment.GetEnvironmentVariable("Company") +"G_L Entry].[Job No_] AS JobNo, " +
                "CASE WHEN CONVERT (varchar,dbo."+ Environment.GetEnvironmentVariable("Company") +"G_L Entry].[Posting Date],8)='00:00:00' THEN '' " +
                "		ELSE 'Close' END AS CloseAcc, " +
                "dbo."+ Environment.GetEnvironmentVariable("Company") +"G_L Entry].Amount, " +
                "dbo."+ Environment.GetEnvironmentVariable("Company") +"G_L Entry].[Global Dimension 2 Code] AS CostCode, " +
                "dbo."+ Environment.GetEnvironmentVariable("Company") +"G_L Entry].[Job Task No_] AS JobTaskNo," +
                "dbo."+ Environment.GetEnvironmentVariable("Company") +"Dimension Value].Name +' ( '+dbo."+ Environment.GetEnvironmentVariable("Company") +"G_L Entry].[Global Dimension 1 Code]+' )' as JobGLName  " +
                "FROM [dbo]."+ Environment.GetEnvironmentVariable("Company") +"G_L Entry] " +
                "Left JOIN dbo."+ Environment.GetEnvironmentVariable("Company") +"Dimension Value] ON dbo."+ Environment.GetEnvironmentVariable("Company") +"G_L Entry].[Global Dimension 1 Code] = dbo."+ Environment.GetEnvironmentVariable("Company") +"Dimension Value].Code " +
                "WHERE [Dimension Code]='JOB_GL' and CONVERT(varchar,dbo."+ Environment.GetEnvironmentVariable("Company") +"G_L Entry].[Posting Date],8)='00:00:00' ";


            //ViewBag.sql = queryData;
            var V_OrderPurchaseLines = _navcontext.AccountReports.FromSqlRaw(queryData).ToList();

            return DataSourceLoader.Load(V_OrderPurchaseLines, loadOptions);

        }



        // POST: UserJob/Add/5
        [HttpPost, ActionName("Add")]
        public IActionResult Add(string ReportName, string ReportDimension,string ReportValue)
        {


            /*Check Session */
            var page = "252";
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

            ConditionReport ConditionReport = new ConditionReport();
    
            ConditionReport.ReportName = ReportName;
            ConditionReport.ReportDimension = ReportDimension;
            ConditionReport.ReportValue = ReportValue;

            _context.ConditionReports.Add(ConditionReport);
            _context.SaveChanges();


            return Ok(ConditionReport);
        }


        // POST: UserJob/remov/5
        [HttpPost, ActionName("remove")]
        public IActionResult remove(string ReportName, string ReportDimension, string ReportValue)
        {

            /*Check Session */
            var page = "252";
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

            List<ConditionReport> userJobs = _context.ConditionReports
                .Where(s => s.ReportName == ReportName && s.ReportDimension==ReportDimension && s.ReportValue==ReportValue)
                .ToList();

            _context.ConditionReports.RemoveRange(userJobs);
            _context.SaveChanges();


            return Ok(userJobs);
        }




        public IActionResult GetData()
        {


            /*Check Session */
            var page = "252";
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

            var queryData = "SELECT " +
                "ROW_NUMBER() OVER (ORDER BY No_) AS ID," +
                "dbo."+ Environment.GetEnvironmentVariable("Company") +"G_L Account].No_ as AccNo, " +
                "dbo."+ Environment.GetEnvironmentVariable("Company") +"G_L Account].Name as AccName," +
                "dbo."+ Environment.GetEnvironmentVariable("Company") +"G_L Account].Totaling, " +
                "CASE WHEN dbo."+ Environment.GetEnvironmentVariable("Company") +"G_L Account].[Account Type]=4 THEN  ' and (dbo."+ Environment.GetEnvironmentVariable("Company") +"G_L Account].No_ >='+LEFT(dbo."+ Environment.GetEnvironmentVariable("Company") +"G_L Account].Totaling,CHARINDEX('..',dbo."+ Environment.GetEnvironmentVariable("Company") +"G_L Account].Totaling)-1) " +
                "+' and dbo."+ Environment.GetEnvironmentVariable("Company") +"G_L Account].No_ <='+ SUBSTRING(dbo."+ Environment.GetEnvironmentVariable("Company") +"G_L Account].Totaling, CHARINDEX('..',dbo."+ Environment.GetEnvironmentVariable("Company") +"G_L Account].Totaling)+2,11)+')' " +
                "ELSE '' END as QueryData " +
                "FROM [dbo]."+ Environment.GetEnvironmentVariable("Company") +"G_L Account]  " +
                "Order by dbo."+ Environment.GetEnvironmentVariable("Company") +"G_L Account].No_ ";


            //ViewBag.sql = queryData;
            var chartOfAccounts = _navcontext.ChartOfAccounts.FromSqlRaw(queryData).ToList();







            response = Ok(new { data = chartOfAccounts });


            return response;

        }






        public IActionResult Profit()
        {
            /*Check Session */
            var page = "252";
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


            //Account No
            //var query = "SELECT DISTINCT [G_L Account No_] as LocationCode,[G_L Account No_] as JobNo FROM dbo."+ Environment.GetEnvironmentVariable("Company") +"G_L Entry] ORDER BY [G_L Account No_] asc";

            var query = "SELECT " +

           "dbo."+ Environment.GetEnvironmentVariable("Company") +"G_L Account].No_ as LocationCode, " +
           "dbo."+ Environment.GetEnvironmentVariable("Company") +"G_L Account].No_ as JobNo  " +
           "FROM [dbo]."+ Environment.GetEnvironmentVariable("Company") +"G_L Account]  " +
           "Order by dbo."+ Environment.GetEnvironmentVariable("Company") +"G_L Account].No_ ";


           var jobNo = _navcontext.v_Job.FromSqlRaw(query).ToList();

            var AccountNo = _context.ConditionReports.Where(p => p.ReportDimension == "ChartOfAccount" && p.ReportName == "AccountProfit").ToList();
            ViewData["AccountNo"] = AccountNo;
            ViewData["JobNo"] = jobNo;


            //GL

            var queryGL = "SELECT  Code as JobNo ,Name as LocationCode  FROM dbo."+ Environment.GetEnvironmentVariable("Company") +"Dimension Value] WHERE [Dimension Code]='JOB_GL'";
            var jobNoGL = _navcontext.v_Job.FromSqlRaw(queryGL).ToList();
            ViewData["JobGL"] = jobNoGL;
            var AccountNoGL = _context.ConditionReports.Where(p => p.ReportDimension == "GLOpen" && p.ReportName == "AccountProfit").ToList();
            ViewData["AccountGL"] = AccountNoGL;




            return View();


        }



        public IActionResult GetDataProfit()
        {


            /*Check Session */
            var page = "252";
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
            ArrayList myList = new ArrayList(1000);

            var AccountNo = _context.ConditionReports.Where(p => p.ReportDimension == "ChartOfAccount" && p.ReportName == "AccountProfit").OrderBy(x => x.ReportValue).ToList();

            var head = "<table id='itemmetrix'>"
                   + "<thead>"
                   + "<tr>"
                   + "<th>JobGL</th>";
            var footer = "<tfoot>"
                      + "<tr>"
                      + "<td>ItemNo</td><td>Decription</td><td style=\'text-align :right;\'>Total</td>";

            var columdata = 1;
            foreach (var location in AccountNo as IList<ConditionReport>)
            {
                head += "<th style=\'text-align :right;\'>" + location.ReportValue + "</th>";
                footer += "<td style=\'text-align :right;\'>" + location.ReportValue + "</td>";
            }

            head += "</tr></thead>";

            var table = "<tbody>";
            int countDes = 0;

            var GLData = _context.ConditionReports.Where(p => p.ReportDimension == "GLOpen" && p.ReportName == "AccountProfit").OrderBy(x => x.ReportValue).ToList();

            foreach (var itm in GLData as IList<ConditionReport>)
            {

                table += "<tr>";
                table += "<td>" + itm.ReportValue + "</td>";
                foreach (var location in AccountNo as IList<ConditionReport>)
                {
                    table += "<td>";

                    table += itm.ReportValue;



                   table += "</td>";
                }
                table += "</tr>";

            }


            //var queryData = "SELECT " +
            //    "ROW_NUMBER() OVER (ORDER BY No_) AS ID," +
            //    "dbo."+ Environment.GetEnvironmentVariable("Company") +"G_L Account].No_ as AccNo, " +
            //    "dbo."+ Environment.GetEnvironmentVariable("Company") +"G_L Account].Name as AccName," +
            //    "dbo."+ Environment.GetEnvironmentVariable("Company") +"G_L Account].Totaling, " +
            //    "CASE WHEN dbo."+ Environment.GetEnvironmentVariable("Company") +"G_L Account].[Account Type]=4 THEN  ' and (dbo."+ Environment.GetEnvironmentVariable("Company") +"G_L Account].No_ >='+LEFT(dbo."+ Environment.GetEnvironmentVariable("Company") +"G_L Account].Totaling,CHARINDEX('..',dbo."+ Environment.GetEnvironmentVariable("Company") +"G_L Account].Totaling)-1) " +
            //    "+' and dbo."+ Environment.GetEnvironmentVariable("Company") +"G_L Account].No_ <='+ SUBSTRING(dbo."+ Environment.GetEnvironmentVariable("Company") +"G_L Account].Totaling, CHARINDEX('..',dbo."+ Environment.GetEnvironmentVariable("Company") +"G_L Account].Totaling)+2,11)+')' " +
            //    "ELSE '' END as QueryData " +
            //    "FROM [dbo]."+ Environment.GetEnvironmentVariable("Company") +"G_L Account]  " +
            //    "Order by dbo."+ Environment.GetEnvironmentVariable("Company") +"G_L Account].No_ ";
            //var chartOfAccounts = _navcontext.ChartOfAccounts.FromSqlRaw(queryData).ToList();


            //var queryDataAll = "SELECT dbo."+ Environment.GetEnvironmentVariable("Company") +"G_L Entry].[Entry No_] as ID, " +
            //        "CASE WHEN SUBSTRING(dbo."+ Environment.GetEnvironmentVariable("Company") +"G_L Entry].[G_L Account No_],8,1)='M' THEN  SUBSTRING(dbo."+ Environment.GetEnvironmentVariable("Company") +"G_L Entry].[G_L Account No_],1,1)+'999999M' " +
            //        " ELSE SUBSTRING(dbo."+ Environment.GetEnvironmentVariable("Company") +"G_L Entry].[G_L Account No_],1,1)+'9999999' END AS ACCLV4, " +
            //        "CASE WHEN SUBSTRING(dbo."+ Environment.GetEnvironmentVariable("Company") +"G_L Entry].[G_L Account No_],8,1)='M' THEN  SUBSTRING(dbo."+ Environment.GetEnvironmentVariable("Company") +"G_L Entry].[G_L Account No_],1,2)+'99999M' " +
            //        " ELSE SUBSTRING(dbo."+ Environment.GetEnvironmentVariable("Company") +"G_L Entry].[G_L Account No_],1,2)+'99999' END AS ACCLV3, " +
            //        "CASE WHEN SUBSTRING(dbo."+ Environment.GetEnvironmentVariable("Company") +"G_L Entry].[G_L Account No_],8,1)='M' THEN  SUBSTRING(dbo."+ Environment.GetEnvironmentVariable("Company") +"G_L Entry].[G_L Account No_],1,4)+'999M' " +
            //        " ELSE SUBSTRING(dbo."+ Environment.GetEnvironmentVariable("Company") +"G_L Entry].[G_L Account No_],1,5)+'999' END AS ACCLV2, " +
            //        "dbo."+ Environment.GetEnvironmentVariable("Company") +"G_L Entry].[G_L Account No_] AS ACCLV1, " +
            //        "dbo."+ Environment.GetEnvironmentVariable("Company") +"G_L Entry].[Global Dimension 1 Code]  AS JobGL, " +
            //        "dbo."+ Environment.GetEnvironmentVariable("Company") +"G_L Entry].[Posting Date] AS PostingDate, " +
            //        "dbo."+ Environment.GetEnvironmentVariable("Company") +"G_L Entry].[Job No_] AS JobNo, " +
            //        "CASE WHEN CONVERT (varchar,dbo."+ Environment.GetEnvironmentVariable("Company") +"G_L Entry].[Posting Date],8)='00:00:00' THEN '' " +
            //        "		ELSE 'Close' END AS CloseAcc, " +
            //        "dbo."+ Environment.GetEnvironmentVariable("Company") +"G_L Entry].Amount, " +
            //        "dbo."+ Environment.GetEnvironmentVariable("Company") +"G_L Entry].[Global Dimension 2 Code] AS CostCode, " +
            //        "dbo."+ Environment.GetEnvironmentVariable("Company") +"G_L Entry].[Job Task No_] AS JobTaskNo," +
            //        "dbo."+ Environment.GetEnvironmentVariable("Company") +"Dimension Value].Name +' ( '+dbo."+ Environment.GetEnvironmentVariable("Company") +"G_L Entry].[Global Dimension 1 Code]+' )' as JobGLName  " +
            //        "FROM [dbo]."+ Environment.GetEnvironmentVariable("Company") +"G_L Entry] " +
            //        "Left JOIN dbo."+ Environment.GetEnvironmentVariable("Company") +"Dimension Value] ON dbo."+ Environment.GetEnvironmentVariable("Company") +"G_L Entry].[Global Dimension 1 Code] = dbo."+ Environment.GetEnvironmentVariable("Company") +"Dimension Value].Code " +
            //        "WHERE [Dimension Code]='JOB_GL' and CONVERT(varchar,dbo."+ Environment.GetEnvironmentVariable("Company") +"G_L Entry].[Posting Date],8)='00:00:00' ";


            ////ViewBag.sql = queryData;
            //var V_OrderPurchaseLines = _navcontext.AccountReports.FromSqlRaw(queryDataAll).ToList();

            var table1 = "</tbody></table>";
            var total_table = head + table + footer + table1;
            //total_table = query;
            response = Ok(new { table = total_table});
            //response = Ok(new { table = query });
            //response = Ok(new { table = queryLocation });
            return response;










            //response = Ok(new { data = chartOfAccounts });




        }






    }
}