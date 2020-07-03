using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;
using CESAPSCOREWEBAPP.Helpers;
using CESAPSCOREWEBAPP.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using static CESAPSCOREWEBAPP.Models.Enums;

namespace CESAPSCOREWEBAPP.Controllers
{
    public class CheckGLController :BaseController
    {



        private readonly NAVContext _navcontext;
        private readonly DatabaseContext _context;


        public CheckGLController(NAVContext navcontext, DatabaseContext context)
        {
            _context = context;
            _navcontext = navcontext;
        }



        // GET: CheckGL
        public ActionResult Index()
        {

            /*Check Session */
            var page = "149";
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

            ViewData["JobNo"] = new SelectList(_context.UserJobs.Where(s => s.UserId == HttpContext.Session.GetInt32("Userid")).OrderBy(s => s.UserJobDetail).ToList(), "UserJobDetail", "UserJobDetail");
            ViewBag.StartDate = DateTime.Now.ToString("01-01-2018", new CultureInfo("en-US"));
            ViewBag.EndDate = DateTime.Now.ToString("dd-MM-yyyy", new CultureInfo("en-US"));

            return View();
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Index(string JobNo, string Startdate, string EndDate)
        {

            IActionResult response = Unauthorized();

            /*Check Session */
            var page = "149";
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


            ViewData["JobNo"] = new SelectList(_context.UserJobs.Where(s => s.UserId == HttpContext.Session.GetInt32("Userid")).OrderBy(s => s.UserJobDetail).ToList(), "UserJobDetail", "UserJobDetail", JobNo);
            var date1 = Startdate.Substring(6, 4) + "-" + Startdate.Substring(3, 2) + "-" + Startdate.Substring(0, 2) + " 00:00:00";
            var date2 = EndDate.Substring(6, 4) + "-" + EndDate.Substring(3, 2) + "-" + EndDate.Substring(0, 2) + " 23:59:59";
            var sdate1 = Startdate;
            var sdate2 = EndDate;
            var rdate1 = Startdate;
            var rdate2 = EndDate;
            ViewBag.StartDate = rdate1;
            ViewBag.EndDate = rdate2;
            ViewBag.SStartDate = Startdate;
            ViewBag.SEndDate = EndDate;
            ViewBag.Job = JobNo;



            var queryData = "SELECT ROW_NUMBER() OVER (ORDER BY [Job Task No_]) as ID," +
                           "dbo."+ Environment.GetEnvironmentVariable("Company") +"Job Ledger Entry].[Posting Date] AS PostingDate, " +
                           "dbo."+ Environment.GetEnvironmentVariable("Company") +"Job Ledger Entry].[Document No_] AS DocumentNo, " +
                           "SUBSTRING(dbo."+ Environment.GetEnvironmentVariable("Company") +"Job Ledger Entry].[Document No_],1,3) as Document3, " +
                           "dbo."+ Environment.GetEnvironmentVariable("Company") +"Job Ledger Entry].[Job No_] AS JobNo, " +
                           "CONCAT(SUBSTRING(dbo."+ Environment.GetEnvironmentVariable("Company") +"Job Ledger Entry].[Job Task No_],1,1),'0000') AS JobMain, " +
                           "CONCAT(SUBSTRING(dbo."+ Environment.GetEnvironmentVariable("Company") +"Job Ledger Entry].[Job Task No_],1,3),'00') AS JobSub, " +
                           "SUBSTRING(dbo."+ Environment.GetEnvironmentVariable("Company") +"Job Ledger Entry].[Job Task No_],1,5) AS JobLedgerEntry, " +
                           "SUBSTRING(dbo."+ Environment.GetEnvironmentVariable("Company") +"Job Ledger Entry].[Job Task No_],1,1) AS JobLedgerEntry1, " +
                           "dbo."+ Environment.GetEnvironmentVariable("Company") +"Job Ledger Entry].[Line Amount] AS LineAmount, " +
                           "dbo."+ Environment.GetEnvironmentVariable("Company") +"Job Ledger Entry].Quantity AS Quantity, " +
                           "CASE WHEN dbo."+ Environment.GetEnvironmentVariable("Company") +"Job Ledger Entry].[Type of task]=0 and dbo."+ Environment.GetEnvironmentVariable("Company") +"Job Ledger Entry].[Original Total Cost]=0 THEN dbo."+ Environment.GetEnvironmentVariable("Company") +"Job Ledger Entry].[Total Cost] ELSE dbo."+ Environment.GetEnvironmentVariable("Company") +"Job Ledger Entry].[Original Total Cost] END AS TotalCost,  " +
                            "dbo."+ Environment.GetEnvironmentVariable("Company") +"Job Ledger Entry].[Total Cost] AS Total, " +
                           "dbo."+ Environment.GetEnvironmentVariable("Company") +"Job Ledger Entry].[Type of task] AS TypeOfTask, " +
                           "dbo."+ Environment.GetEnvironmentVariable("Company") +"Job Ledger Entry].[From Location] AS FromLocation " +
                           " FROM " +
                           "dbo."+ Environment.GetEnvironmentVariable("Company") +"Job Ledger Entry] " +
                           " WHERE dbo."+ Environment.GetEnvironmentVariable("Company") +"Job Ledger Entry].[Job No_] ={0} or dbo."+ Environment.GetEnvironmentVariable("Company") +"Job Ledger Entry].[From Location] ={0}  " +
                           "Order by [Job No_],dbo."+ Environment.GetEnvironmentVariable("Company") +"Job Ledger Entry].[Job Task No_],[Posting Date] ASC ";
            //SqlParameter parameterJob = new SqlParameter("@job", JobNo);
            var JobLedgerNews = _navcontext.JobLedgerNews.FromSqlRaw(queryData, JobNo).ToList();



            var queryData1 = "SELECT ROW_NUMBER() OVER (ORDER BY b.JobTaskNo) as ID,b.MainJobID,b.DescriptionMainJob,b.SubJobID,b.DescriptionSubJob,b.JobTaskNo,b.DescriptionFull, " +
                           "(SELECT isnull(SUM([Amount]),0) as Total FROM dbo."+ Environment.GetEnvironmentVariable("Company") +"Analysis View Entry] WHERE [Analysis View Code]='JOB COST A' and Convert(Time,[Posting Date])<>'23:59:59'  and [Posting Date]>={1} and [Posting Date] <={2} and [Dimension 1 Value Code]={0} and [Dimension 2 Value Code]<>'' and [Dimension 2 Value Code]=b.JobTaskNo) as GLAcc " +
                           "FROM( SELECT a.JobTaskNo,a.MainJobID,a.SubJobID,  " +
                           "(SELECT TOP 1 dbo."+ Environment.GetEnvironmentVariable("Company") +"Job Task].Description FROM dbo."+ Environment.GetEnvironmentVariable("Company") +"Job Task] WHERE [Job Task No_]=a.JobTaskNo) as Description,  " +
                           "(SELECT TOP 1 ([Job Task No_]+' '+Description) FROM dbo."+ Environment.GetEnvironmentVariable("Company") +"Job Task] WHERE [Job Task No_]=a.JobTaskNo) as DescriptionFull, " +
                           "(SELECT TOP 1 dbo."+ Environment.GetEnvironmentVariable("Company") +"Job Task].Description FROM dbo."+ Environment.GetEnvironmentVariable("Company") +"Job Task] WHERE SUBSTRING(dbo."+ Environment.GetEnvironmentVariable("Company") +"Job Task].[Job Task No_],1,1)+'0000'=a.MainJobID) as DescriptionMainJob,  " +
                           "(SELECT TOP 1 dbo."+ Environment.GetEnvironmentVariable("Company") +"Job Task].Description FROM dbo."+ Environment.GetEnvironmentVariable("Company") +"Job Task] WHERE SUBSTRING(dbo."+ Environment.GetEnvironmentVariable("Company") +"Job Task].[Job Task No_],1,3)+'00'=a.SubJobID) as DescriptionSubJob " +
                           "FROM (SELECT DISTINCT dbo."+ Environment.GetEnvironmentVariable("Company") +"Job Task].[Job Task No_] as JobTaskNo, SUBSTRING(dbo."+ Environment.GetEnvironmentVariable("Company") +"Job Task].[Job Task No_],1,1)+'0000' as MainJobID,SUBSTRING(dbo."+ Environment.GetEnvironmentVariable("Company") +"Job Task].[Job Task No_],1,3)+'00' as SubJobID FROM dbo."+ Environment.GetEnvironmentVariable("Company") +"Job Task]) as a) as b ORDER BY b.JobTaskNo";

            //SqlParameter parameterStart = new SqlParameter("@start", date1);
            //SqlParameter parameterDate1 = new SqlParameter("@date1", date2);
            var checkGLs = _navcontext.CheckGLs.FromSqlRaw(queryData1, JobNo, date1, date2).ToList();


            List<V_CheckCostDiffGL> instances = new List<V_CheckCostDiffGL>();
            V_CheckCostDiffGL current = null;



            List<V_CheckCostDiffGL> SubTotal = new List<V_CheckCostDiffGL>();

            decimal sumReclass;
            decimal sumIssue;
            decimal sumRental;
            decimal sumReturnCal;
            decimal sumReturn;
            decimal sumIssueExternal;
            var startdate = Convert.ToDateTime(date1);
            var enddate = Convert.ToDateTime(date2);
            decimal CumBalAMT = 0;
            decimal sumReclassAPO = 0;




            foreach (var std in checkGLs as IList<CheckGL>)
            {
                current = new V_CheckCostDiffGL();
                current.MainJobID = std.MainJobID;
                current.DescriptionMainJob = std.DescriptionMainJob;
                current.SubJobID = std.SubJobID;
                current.DescriptionSubJob = std.DescriptionSubJob;
                current.JobTaskNo = std.JobTaskNo;
                current.DescriptionFull = std.DescriptionFull;
                current.GLAcc = std.GLAcc;

                if (std.MainJobID == "N0000" || std.MainJobID == "O0000")
                {

                    sumReclass = 0;
                    sumIssue = 0;
                    sumRental = 0;
                    sumReturnCal = 0;
                    sumReturn = 0;
                    sumIssueExternal = 0;
                    sumReclass = JobLedgerNews.Where(c => c.JobNo == JobNo && c.JobLedgerEntry == std.JobTaskNo && c.TypeOfTask == 0 && c.PostingDate <= enddate).Sum(c => c.TotalCost+c.LineAmount);
                    sumIssue = JobLedgerNews.Where(c => c.JobNo == JobNo && c.JobLedgerEntry == std.JobTaskNo && c.TypeOfTask == 3 && c.PostingDate <= enddate).Sum(c => c.TotalCost + c.LineAmount);
                    sumRental = JobLedgerNews.Where(c => c.JobNo == JobNo && c.JobLedgerEntry == std.JobTaskNo && c.TypeOfTask == 2 && c.PostingDate <= enddate).Sum(c => c.TotalCost + c.LineAmount);
                    sumReturnCal = JobLedgerNews.Where(c => c.JobNo == JobNo && c.JobLedgerEntry == std.JobTaskNo && c.TypeOfTask == 1 && c.Document3 == "CAL" && c.PostingDate <= enddate).Sum(c => c.TotalCost + c.LineAmount) * 2;
                    sumReturn = JobLedgerNews.Where(c => c.JobNo == JobNo && c.JobLedgerEntry == std.JobTaskNo && c.TypeOfTask == 1 && c.PostingDate <= enddate).Sum(c => c.TotalCost + c.LineAmount);
                    sumIssueExternal = JobLedgerNews.Where(c => c.JobNo != JobNo && c.JobLedgerEntry == std.JobTaskNo && c.TypeOfTask == 3 && c.FromLocation == JobNo && c.PostingDate <= enddate).Sum(c => c.TotalCost + c.LineAmount);
                   // sumIssueExternal = JobLedgerNews.Where(c => c.JobNo != JobNo && c.JobLedgerEntry == std.JobTaskNo && c.TypeOfTask == 3 && c.FromLocation == JobNo && c.PostingDate <= enddate).Sum(c => c.TotalCost + c.LineAmount) * 2;
                    sumReclassAPO = JobLedgerNews.Where(c => c.JobNo != JobNo && c.JobLedgerEntry == std.JobTaskNo && c.TypeOfTask == 0 && c.PostingDate <= enddate && c.TotalCost == 0).Sum(c => c.Total + c.LineAmount);
                    CumBalAMT = (sumReclass + sumIssue + sumRental + sumReturnCal + sumReclassAPO) - (sumReturn + sumIssueExternal);
                    current.CumBalAMT = CumBalAMT;
                    current.Diff = std.GLAcc - current.CumBalAMT;

                }
                else
                {

               
                    sumReclass = 0;
                    sumIssue = 0;
                    sumRental = 0;
                    sumReturnCal = 0;
                    sumReturn = 0;
                    sumIssueExternal = 0;
                    sumReclass = JobLedgerNews.Where(c => c.JobNo == JobNo && c.JobLedgerEntry == std.JobTaskNo && c.TypeOfTask == 0 && c.PostingDate <= enddate).Sum(c => c.TotalCost);
                    sumIssue = JobLedgerNews.Where(c => c.JobNo == JobNo && c.JobLedgerEntry == std.JobTaskNo && c.TypeOfTask == 3 && c.PostingDate <= enddate).Sum(c => c.TotalCost);
                    sumRental = JobLedgerNews.Where(c => c.JobNo == JobNo && c.JobLedgerEntry == std.JobTaskNo && c.TypeOfTask == 2 && c.PostingDate <= enddate).Sum(c => c.TotalCost);
                    sumReturnCal = JobLedgerNews.Where(c => c.JobNo == JobNo && c.JobLedgerEntry == std.JobTaskNo && c.TypeOfTask == 1 && c.Document3 == "CAL" && c.PostingDate <= enddate).Sum(c => c.TotalCost) * 2;
                    sumReturn = JobLedgerNews.Where(c => c.JobNo == JobNo && c.JobLedgerEntry == std.JobTaskNo && c.TypeOfTask == 1 && c.PostingDate <= enddate).Sum(c => c.TotalCost);
                    sumIssueExternal = JobLedgerNews.Where(c => c.JobNo != JobNo && c.JobLedgerEntry == std.JobTaskNo && c.TypeOfTask == 3 && c.FromLocation == JobNo && c.PostingDate <= enddate).Sum(c => c.TotalCost);
                    //sumIssueExternal = JobLedgerNews.Where(c => c.JobNo != JobNo && c.JobLedgerEntry == std.JobTaskNo && c.TypeOfTask == 3 && c.FromLocation == JobNo && c.PostingDate <= enddate).Sum(c => c.TotalCost) * 2;
                    sumReclassAPO = JobLedgerNews.Where(c => c.JobNo != JobNo && c.JobLedgerEntry == std.JobTaskNo && c.TypeOfTask == 0 && c.PostingDate <= enddate && c.TotalCost == 0).Sum(c => c.Total);
                    CumBalAMT = (sumReclass + sumIssue + sumRental + sumReturnCal + sumReclassAPO) - (sumReturn + sumIssueExternal);
                    current.CumBalAMT = CumBalAMT;
                    current.Diff = std.GLAcc - current.CumBalAMT;
                }
                instances.Add(current);
            }


            //response = Ok(new { data = instances });
            ViewData["JsonRegionList"] = instances;
            return View();
        }




        public static string SetFontRed(decimal value1, int level) //Check Color
        {
            string result = "";

            if (level == 1)
            {

                if (value1 < 0)
                {
                    result = "<font color='red'><b>" + value1.ToString("#,##0.00") + "</b></font>";
                }
                else
                {
                    result = "<font color='black'><b>" + value1.ToString("#,##0.00") + "</b></font>";
                }
            }
            else if (level == 3)
            {
                if (value1 < 0)
                {
                    result = "<font color='red'>" + value1.ToString("#,##0.00") + "</font>";
                }
                else
                {
                    result = "<font color='black'>" + value1.ToString("#,##0.00") + "</font>";
                }

            }
            return result;
        }


    }
}