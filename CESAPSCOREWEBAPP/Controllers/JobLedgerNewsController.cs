using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Drawing;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using CESAPSCOREWEBAPP.Helpers;
using CESAPSCOREWEBAPP.Models;
using DevExtreme.AspNet.Data;
using DevExtreme.AspNet.Mvc;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Nancy.Json;
using OfficeOpenXml;
using OfficeOpenXml.Style;
using OfficeOpenXml.Table;
using static CESAPSCOREWEBAPP.Models.Enums;

namespace CESAPSCOREWEBAPP.Controllers
{
    [Authorize]
    public class JobLedgerNewsController : BaseController
    {

        private readonly DatabaseContext _context;
        private readonly NAVContext _navcontext;


        private const string XlsxContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
        private readonly IHostingEnvironment _hostingEnvironment;


        public JobLedgerNewsController(DatabaseContext context, NAVContext navcontext, IHostingEnvironment hostingEnvironment)
        {
            _context = context;
            _navcontext = navcontext;
            _hostingEnvironment = hostingEnvironment;
        }

        // GET: JobLedgerEntry
        public IActionResult Index()
        {

            /*Check Session */
            var page = "6";
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

            ViewData["JobNo"] = new SelectList(_context.UserJobs.Where(s => s.UserId == HttpContext.Session.GetInt32("Userid")).OrderBy(s => s.UserJobDetail).ToList(), "UserJobDetail", "UserJobDetail");



            ViewBag.StartDate = DateTime.Now.ToString("01-MM-yyyy", new CultureInfo("en-US"));
            ViewBag.EndDate = DateTime.Now.ToString("dd-MM-yyyy", new CultureInfo("en-US"));

            return View();
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Index(string JobNo, string Startdate, string EndDate)
        {

            IActionResult response = Unauthorized();
            string totalview = "";
            string headtable = "";
            string table = "";
            string main = "";
            string sub = "";
            string subID = "";
            string mainID = "";
            decimal sumThisPeriodQTY = 0;
            decimal sumThisPeriodAMT = 0;
            decimal sumOpeningQTY = 0;
            decimal sumOpeningAMT = 0;
            decimal sumCumBalQTY = 0;
            decimal sumCumBalAMT = 0;
            decimal sumBudgetQTY = 0;
            decimal sumBudgetAMT = 0;
            decimal sumSubThisPeriodQTY = 0;
            decimal sumSubThisPeriodAMT = 0;
            decimal sumSubOpeningQTY = 0;
            decimal sumSubOpeningAMT = 0;
            decimal sumSubCumBalQTY = 0;
            decimal sumSubCumBalAMT = 0;
            decimal sumSubBudgetQTY = 0;
            decimal sumSubBudgetAMT = 0;
            decimal A00 = 0;
            decimal B00 = 0;
            decimal C00 = 0;
            decimal D00 = 0;
            decimal E00 = 0;
            decimal F00 = 0;
            decimal G00 = 0;
            decimal H00 = 0;
            decimal N00 = 0;
            decimal O00 = 0;
            decimal G0210 = 0;
            decimal C0110 = 0;
            decimal C0120 = 0;
            decimal C0130 = 0;
            decimal C0410 = 0;
            decimal C0510 = 0;
            decimal C0910 = 0;
            decimal C1010 = 0;
            decimal C1110 = 0;
            decimal D0450 = 0;
            decimal H0110 = 0;
            decimal H0130 = 0;
            decimal H0200 = 0;
            decimal H0800 = 0;
            decimal D2100 = 0;
            decimal E0800 = 0;
            decimal E0910 = 0;
            decimal E0920 = 0;
            decimal E0930 = 0;
            decimal E0940 = 0;
            decimal H0600 = 0;
            decimal F5999 = 0;
            decimal H0400 = 0;
            decimal TIC = 0; //total Internal Cost
            decimal TCXI = 0; //Total Cose (External+Internal)
            int check = 0;
            int check1 = 0;
            string summain = "";
            string Sumhead = "";
            string summainIncome = "";

            decimal thisQ = 0;
            decimal thisA = 0;
            decimal openQ = 0;
            decimal openA = 0;
            decimal cumQ = 0;
            decimal cumA = 0;
            decimal budQ = 0;
            decimal budA = 0;

            decimal OthisQ = 0;
            decimal OthisA = 0;
            decimal OopenQ = 0;
            decimal OopenA = 0;
            decimal OcumQ = 0;
            decimal OcumA = 0;
            decimal ObudQ = 0;
            decimal ObudA = 0;


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

            var queryJobTask = "SELECT *," +
                "(select top 1 [Job Task No_]+' '+dbo."+ Environment.GetEnvironmentVariable("Company") +"Job Task].Description from dbo."+ Environment.GetEnvironmentVariable("Company") +"Job Task] where dbo."+ Environment.GetEnvironmentVariable("Company") +"Job Task].[Job Task No_]=a.JobTaskNo) as DescriptionFull, " +
                "(select top 1 dbo."+ Environment.GetEnvironmentVariable("Company") +"Job Task].Description from dbo."+ Environment.GetEnvironmentVariable("Company") +"Job Task] where dbo."+ Environment.GetEnvironmentVariable("Company") +"Job Task].[Job Task No_]=a.JobTaskNo) as DescriptionSubJob " +
                " FROM( " +
                "SELECT " +
                "DISTINCT dbo."+ Environment.GetEnvironmentVariable("Company") +"Job Task].[Job Task No_] as JobTaskNo, " +
                "SUBSTRING(dbo."+ Environment.GetEnvironmentVariable("Company") +"Job Task].[Job Task No_],1,1)+'0000' as MainJobID, " +
                "SUBSTRING(dbo."+ Environment.GetEnvironmentVariable("Company") +"Job Task].[Job Task No_],1,3)+'00' as SubJobID , " +
                "SUBSTRING(dbo."+ Environment.GetEnvironmentVariable("Company") +"Job Task].[Job Task No_],1,5) as JobTaskCut," +
                "SUBSTRING(dbo."+ Environment.GetEnvironmentVariable("Company") +"Job Task].[Job Task No_],1,1) as JobstringCust " +
                "FROM dbo."+ Environment.GetEnvironmentVariable("Company") +"Job Task]  ) as a Order by a.JobTaskNo asc ";


            var jobTasks = _navcontext.jobTasks.FromSqlRaw(queryJobTask).ToList();





            var queryJobPlanningLine = "SELECT ROW_NUMBER() OVER (ORDER BY [Job Task No_]) as ID," +
            "dbo." + Environment.GetEnvironmentVariable("Company") + "Job Planning Line].[Job No_] as JobNo, " +
            "SUBSTRING(dbo." + Environment.GetEnvironmentVariable("Company") + "Job Planning Line].[Job Task No_],1,1) as JobTaskCut, " +
            "SUBSTRING(dbo." + Environment.GetEnvironmentVariable("Company") + "Job Planning Line].[Job Task No_],1,1)+'0000' as MainJobID, " +
            "SUBSTRING(dbo." + Environment.GetEnvironmentVariable("Company") + "Job Planning Line].[Job Task No_],1,3)+'00' as SubJobID, " +
            "dbo." + Environment.GetEnvironmentVariable("Company") + "Job Planning Line].[Job Task No_] as JobTaskNo, " +
            "ROUND(isnull(dbo." + Environment.GetEnvironmentVariable("Company") + "Job Planning Line].[Total Cost],0),2) as TotalCost, " +
            "ROUND(isnull(dbo." + Environment.GetEnvironmentVariable("Company") + "Job Planning Line].Quantity,0),2) as Quantity " +
            "FROM dbo." + Environment.GetEnvironmentVariable("Company") + "Job Planning Line] ";


            //var queryJobPlanningLine = "SELECT " +
            //    "dbo."+ Environment.GetEnvironmentVariable("Company") +"Job Planning Line].[Job No_] as JobNo, " +
            //    "SUBSTRING(dbo."+ Environment.GetEnvironmentVariable("Company") +"Job Planning Line].[Job Task No_],1,1) as JobTaskCut, " +
            //    "SUBSTRING(dbo."+ Environment.GetEnvironmentVariable("Company") +"Job Planning Line].[Job Task No_],1,1)+'0000' as MainJobID, " +
            //    "SUBSTRING(dbo."+ Environment.GetEnvironmentVariable("Company") +"Job Planning Line].[Job Task No_],1,3)+'00' as SubJobID, " +
            //    "dbo."+ Environment.GetEnvironmentVariable("Company") +"Job Planning Line].[Job Task No_] as JobTaskNo, " +
            //    "ROUND(isnull(dbo." + Environment.GetEnvironmentVariable("Company") +"Job Planning Line].[Total Cost (LCY)],0),2) as TotalCost, " +
            //    "ROUND(isnull(dbo." + Environment.GetEnvironmentVariable("Company") + "Job Planning Line].Quantity,0),2) as Quantity " +
            //    "FROM dbo."+ Environment.GetEnvironmentVariable("Company") +"Job Planning Line] ";
            var jobPlanningLines = _navcontext.jobPlanningLines.FromSqlRaw(queryJobPlanningLine).ToList();


            var queryData = "SELECT ROW_NUMBER() OVER (ORDER BY [Job Task No_]) as ID, " +
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
                " WHERE dbo."+ Environment.GetEnvironmentVariable("Company") +"Job Ledger Entry].[Job No_] ={0} or dbo."+ Environment.GetEnvironmentVariable("Company") +"Job Ledger Entry].[From Location] ={0} "  +
                "Order by [Job No_],dbo."+ Environment.GetEnvironmentVariable("Company") +"Job Ledger Entry].[Job Task No_],[Posting Date] ASC ";

            //SqlParameter parameterJob = new SqlParameter("@job", JobNo);
            var JobLedgerNews = _navcontext.JobLedgerNews.FromSqlRaw(queryData, JobNo).ToList();

            decimal sumReclass;
            decimal sumIssue;
            decimal sumRental;
            decimal sumReturnCal;
            decimal sumReturn;
            decimal sumIssueExternal;
            var startdate = Convert.ToDateTime(date1);
            var enddate = Convert.ToDateTime(date2);
            decimal ThisPeriodQty = 0;
            decimal ThisPeriodAMT = 0;
            decimal OpenningQTY = 0;
            decimal OpenningAMT = 0;
            decimal CumBalQTY = 0;
            decimal CumBalAMT = 0;
            decimal BudgetQTY = 0;
            decimal BudgetAMT = 0;
            decimal PercenAMT = 0;
            decimal PercenQTY = 0;
            decimal sumReclassAPO = 0;
            

            List<Tmp_JobCostReport> instances = new List<Tmp_JobCostReport>();
            Tmp_JobCostReport current = null;



            List<Tmp_JobCostReport> SubTotal = new List<Tmp_JobCostReport>();
            Tmp_JobCostReport CurrentSubTotal = null;

            var checkzero = 0;

            foreach (var std in jobTasks as IList<JobTask>)
            {



                current = new Tmp_JobCostReport();
                current.JobMain = std.MainJobID;
                current.JobSub = std.SubJobID;
                current.JobTaskCut = std.JobTaskCut;
                current.JobTaskFull = std.DescriptionFull;

                if (std.JobTaskCut == std.MainJobID) //ถ้าเป็น Head ให้ทำ
                {
                    if (std.JobstringCust == "O" || std.JobstringCust == "N")
                    {
                        sumReclass =0;
                        sumIssue =0;
                        sumRental =0;
                        sumReturnCal =0;
                        sumReturn = 0;
                        sumIssueExternal =0;
                        sumReclass = JobLedgerNews.Where(c => c.JobNo == JobNo && c.JobMain == std.JobTaskCut && c.TypeOfTask == 0 && c.PostingDate >= startdate && c.PostingDate <= enddate).Sum(c => c.Quantity);
                        sumIssue = JobLedgerNews.Where(c => c.JobNo == JobNo && c.JobMain == std.JobTaskCut && c.TypeOfTask == 3 && c.PostingDate >= startdate && c.PostingDate <= enddate).Sum(c => c.Quantity);
                        sumRental = JobLedgerNews.Where(c => c.JobNo == JobNo && c.JobMain == std.JobTaskCut && c.TypeOfTask == 2 && c.PostingDate >= startdate && c.PostingDate <= enddate).Sum(c => c.Quantity);
                        sumReturnCal = JobLedgerNews.Where(c => c.JobNo == JobNo && c.JobMain == std.JobTaskCut && c.TypeOfTask == 1 && c.Document3 == "CAL" && c.PostingDate >= startdate && c.PostingDate <= enddate).Sum(c => c.Quantity) * 2;
                        sumReturn = JobLedgerNews.Where(c => c.JobNo == JobNo && c.JobMain == std.JobTaskCut && c.TypeOfTask == 1 && c.PostingDate >= startdate && c.PostingDate <= enddate).Sum(c => c.Quantity);
                        sumIssueExternal = JobLedgerNews.Where(c => c.JobNo != JobNo && c.JobMain == std.JobTaskCut && c.TypeOfTask == 3 && c.PostingDate >= startdate && c.FromLocation == JobNo && c.PostingDate <= enddate).Sum(c => c.Quantity);
                     

                        ThisPeriodQty = (sumReclass + sumIssue + sumRental + sumReturnCal) - (sumReturn + sumIssueExternal);
                        current.ThisPeriodQty = ThisPeriodQty;



                        sumReclass = 0;
                        sumIssue = 0;
                        sumRental = 0;
                        sumReturnCal = 0;
                        sumReturn = 0;
                        sumIssueExternal = 0;
                        sumReclass = JobLedgerNews.Where(c => c.JobNo == JobNo && c.JobMain == std.JobTaskCut && c.TypeOfTask == 0 && c.PostingDate >= startdate && c.PostingDate <= enddate).Sum(c => c.TotalCost + c.LineAmount);
                        sumIssue = JobLedgerNews.Where(c => c.JobNo == JobNo && c.JobMain == std.JobTaskCut && c.TypeOfTask == 3 && c.PostingDate >= startdate && c.PostingDate <= enddate).Sum(c => c.TotalCost + c.LineAmount);
                        sumRental = JobLedgerNews.Where(c => c.JobNo == JobNo && c.JobMain == std.JobTaskCut && c.TypeOfTask == 2 && c.PostingDate >= startdate && c.PostingDate <= enddate).Sum(c => c.TotalCost + c.LineAmount);
                        sumReturnCal = JobLedgerNews.Where(c => c.JobNo == JobNo && c.JobMain == std.JobTaskCut && c.TypeOfTask == 1 && c.Document3 == "CAL" && c.PostingDate >= startdate && c.PostingDate <= enddate).Sum(c => c.TotalCost + c.LineAmount) * 2;
                        sumReturn = JobLedgerNews.Where(c => c.JobNo == JobNo && c.JobMain == std.JobTaskCut && c.TypeOfTask == 1 && c.PostingDate >= startdate && c.PostingDate <= enddate).Sum(c => c.TotalCost + c.LineAmount);
                        sumIssueExternal = JobLedgerNews.Where(c => c.JobNo != JobNo && c.JobMain == std.JobTaskCut && c.TypeOfTask == 3 && c.FromLocation == JobNo && c.PostingDate >= startdate && c.PostingDate <= enddate).Sum(c => c.TotalCost + c.LineAmount);
                        sumReclassAPO = JobLedgerNews.Where(c => c.JobNo == JobNo && c.JobMain == std.JobTaskCut && c.TypeOfTask == 0 && c.PostingDate>=startdate && c.PostingDate <= enddate && c.TotalCost == 0).Sum(c => c.Total);
                        ThisPeriodAMT = (sumReclass + sumIssue + sumRental + sumReturnCal+sumReclassAPO) - (sumReturn + sumIssueExternal);
                        current.ThisPeriodAMT = ThisPeriodAMT;



                        sumReclass = 0;
                        sumIssue = 0;
                        sumRental = 0;
                        sumReturnCal = 0;
                        sumReturn = 0;
                        sumIssueExternal = 0;
                        sumReclass = JobLedgerNews.Where(c => c.JobNo == JobNo && c.JobMain == std.JobTaskCut && c.TypeOfTask == 0 && c.PostingDate < startdate).Sum(c => c.Quantity);
                        sumIssue = JobLedgerNews.Where(c => c.JobNo == JobNo && c.JobMain == std.JobTaskCut && c.TypeOfTask == 3 && c.PostingDate < startdate).Sum(c => c.Quantity);
                        sumRental = JobLedgerNews.Where(c => c.JobNo == JobNo && c.JobMain == std.JobTaskCut && c.TypeOfTask == 2 && c.PostingDate < startdate).Sum(c => c.Quantity);
                        sumReturnCal = JobLedgerNews.Where(c => c.JobNo == JobNo && c.JobMain == std.JobTaskCut && c.TypeOfTask == 1 && c.Document3 == "CAL" && c.PostingDate < startdate).Sum(c => c.Quantity) * 2;
                        sumReturn = JobLedgerNews.Where(c => c.JobNo == JobNo && c.JobMain == std.JobTaskCut && c.TypeOfTask == 1 && c.PostingDate < startdate).Sum(c => c.Quantity);
                        sumIssueExternal = JobLedgerNews.Where(c => c.JobNo != JobNo && c.JobMain == std.JobTaskCut && c.TypeOfTask == 3 && c.PostingDate < startdate && c.FromLocation == JobNo).Sum(c => c.Quantity);
                        OpenningQTY = (sumReclass + sumIssue + sumRental + sumReturnCal) - (sumReturn + sumIssueExternal);
                        current.OpenningQTY = OpenningQTY;



                        sumReclass = 0;
                        sumIssue = 0;
                        sumRental = 0;
                        sumReturnCal = 0;
                        sumReturn = 0;
                        sumIssueExternal = 0;
                        sumReclass = JobLedgerNews.Where(c => c.JobNo == JobNo && c.JobMain == std.JobTaskCut && c.TypeOfTask == 0 && c.PostingDate < startdate).Sum(c => c.TotalCost + c.LineAmount);
                        sumIssue = JobLedgerNews.Where(c => c.JobNo == JobNo && c.JobMain == std.JobTaskCut && c.TypeOfTask == 3 && c.PostingDate < startdate).Sum(c => c.TotalCost + c.LineAmount);
                        sumRental = JobLedgerNews.Where(c => c.JobNo == JobNo && c.JobMain == std.JobTaskCut && c.TypeOfTask == 2 && c.PostingDate < startdate).Sum(c => c.TotalCost + c.LineAmount);
                        sumReturnCal = JobLedgerNews.Where(c => c.JobNo == JobNo && c.JobMain == std.JobTaskCut && c.TypeOfTask == 1 && c.Document3 == "CAL" && c.PostingDate < startdate).Sum(c => c.TotalCost + c.LineAmount) * 2;
                        sumReturn = JobLedgerNews.Where(c => c.JobNo == JobNo && c.JobMain == std.JobTaskCut && c.TypeOfTask == 1 && c.PostingDate < startdate).Sum(c => c.TotalCost + c.LineAmount);
                        sumIssueExternal = JobLedgerNews.Where(c => c.JobNo != JobNo && c.JobMain == std.JobTaskCut && c.TypeOfTask == 3 && c.FromLocation == JobNo && c.PostingDate < startdate).Sum(c => c.TotalCost + c.LineAmount);
                        sumReclassAPO = JobLedgerNews.Where(c => c.JobNo == JobNo && c.JobMain == std.JobTaskCut && c.TypeOfTask == 0 && c.PostingDate < startdate && c.TotalCost == 0).Sum(c => c.Total);
                        OpenningAMT = (sumReclass + sumIssue + sumRental + sumReturnCal+sumReclassAPO) - (sumReturn + sumIssueExternal);
                        current.OpenningAMT = OpenningAMT;


                        sumReclass = 0;
                        sumIssue = 0;
                        sumRental = 0;
                        sumReturnCal = 0;
                        sumReturn = 0;
                        sumIssueExternal = 0;
                        sumReclass = JobLedgerNews.Where(c => c.JobNo == JobNo && c.JobMain == std.JobTaskCut && c.TypeOfTask == 0 && c.PostingDate <= enddate).Sum(c => c.Quantity);
                        sumIssue = JobLedgerNews.Where(c => c.JobNo == JobNo && c.JobMain == std.JobTaskCut && c.TypeOfTask == 3 && c.PostingDate <= enddate).Sum(c => c.Quantity);
                        sumRental = JobLedgerNews.Where(c => c.JobNo == JobNo && c.JobMain == std.JobTaskCut && c.TypeOfTask == 2 && c.PostingDate <= enddate).Sum(c => c.Quantity);
                        sumReturnCal = JobLedgerNews.Where(c => c.JobNo == JobNo && c.JobMain == std.JobTaskCut && c.TypeOfTask == 1 && c.Document3 == "CAL" && c.PostingDate <= enddate).Sum(c => c.Quantity) * 2;
                        sumReturn = JobLedgerNews.Where(c => c.JobNo == JobNo && c.JobMain == std.JobTaskCut && c.TypeOfTask == 1 && c.PostingDate <= enddate).Sum(c => c.Quantity);
                        sumIssueExternal = JobLedgerNews.Where(c => c.JobNo != JobNo && c.JobMain == std.JobTaskCut && c.TypeOfTask == 3 && c.PostingDate <= enddate && c.FromLocation == JobNo).Sum(c => c.Quantity);
                        CumBalQTY = (sumReclass + sumIssue + sumRental + sumReturnCal) - (sumReturn + sumIssueExternal);
                        current.CumBalQTY = CumBalQTY;


                        sumReclass = 0;
                        sumIssue = 0;
                        sumRental = 0;
                        sumReturnCal = 0;
                        sumReturn = 0;
                        sumIssueExternal = 0;
                        sumReclass = JobLedgerNews.Where(c => c.JobNo == JobNo && c.JobMain == std.JobTaskCut && c.TypeOfTask == 0 && c.PostingDate <= enddate).Sum(c => c.TotalCost + c.LineAmount);
                        sumIssue = JobLedgerNews.Where(c => c.JobNo == JobNo && c.JobMain == std.JobTaskCut && c.TypeOfTask == 3 && c.PostingDate <= enddate).Sum(c => c.TotalCost + c.LineAmount);
                        sumRental = JobLedgerNews.Where(c => c.JobNo == JobNo && c.JobMain == std.JobTaskCut && c.TypeOfTask == 2 && c.PostingDate <= enddate).Sum(c => c.TotalCost + c.LineAmount);
                        sumReturnCal = JobLedgerNews.Where(c => c.JobNo == JobNo && c.JobMain == std.JobTaskCut && c.TypeOfTask == 1 && c.Document3 == "CAL" && c.PostingDate <= enddate).Sum(c => c.TotalCost + c.LineAmount) * 2;
                        sumReturn = JobLedgerNews.Where(c => c.JobNo == JobNo && c.JobMain == std.JobTaskCut && c.TypeOfTask == 1 && c.PostingDate <= enddate).Sum(c => c.TotalCost + c.LineAmount);
                        sumIssueExternal = JobLedgerNews.Where(c => c.JobNo != JobNo && c.JobMain == std.JobTaskCut && c.TypeOfTask == 3 && c.FromLocation == JobNo && c.PostingDate <= enddate).Sum(c => c.TotalCost + c.LineAmount);
                        sumReclassAPO = JobLedgerNews.Where(c => c.JobNo == JobNo && c.JobMain == std.JobTaskCut && c.TypeOfTask == 0 && c.PostingDate <= enddate && c.TotalCost == 0).Sum(c => c.Total);
                        CumBalAMT = (sumReclass + sumIssue + sumRental + sumReturnCal+sumReclassAPO) - (sumReturn + sumIssueExternal);
                        current.CumBalAMT = CumBalAMT;


                        BudgetQTY = jobPlanningLines.Where(c => c.JobNo == JobNo && c.MainJobID == std.JobTaskCut).Sum(c => c.Quantity);
                        BudgetAMT= jobPlanningLines.Where(c => c.JobNo == JobNo && c.MainJobID == std.JobTaskCut).Sum(c => c.TotalCost);
                        if (BudgetQTY == 0)
                        {
                            PercenQTY = 0;
                        }
                        else
                        {
                            PercenQTY = (CumBalQTY / BudgetQTY) * 100;
                        }

                        if (BudgetAMT == 0)
                        {
                            PercenAMT = 0;
                        }
                        else
                        {
                            PercenAMT = (CumBalAMT / BudgetAMT) * 100;
                        }

                        current.BudgetQTY = BudgetQTY;
                        current.BudgetAMT = BudgetAMT;
                        current.PercenAMT = PercenAMT;
                        current.PercenQTY = PercenQTY;


                    }
                    else
                    {

                        sumReclass = 0;
                        sumIssue = 0;
                        sumRental = 0;
                        sumReturnCal = 0;
                        sumReturn = 0;
                        sumIssueExternal = 0;
                        sumReclass = JobLedgerNews.Where(c => c.JobNo == JobNo && c.JobMain == std.JobTaskCut && c.TypeOfTask == 0 && c.PostingDate >= startdate && c.PostingDate <= enddate).Sum(c => c.Quantity);
                        sumIssue = JobLedgerNews.Where(c => c.JobNo == JobNo && c.JobMain == std.JobTaskCut && c.TypeOfTask == 3 && c.PostingDate >= startdate && c.PostingDate <= enddate).Sum(c => c.Quantity);
                        sumRental = JobLedgerNews.Where(c => c.JobNo == JobNo && c.JobMain == std.JobTaskCut && c.TypeOfTask == 2 && c.PostingDate >= startdate && c.PostingDate <= enddate).Sum(c => c.Quantity);
                        sumReturnCal = JobLedgerNews.Where(c => c.JobNo == JobNo && c.JobMain == std.JobTaskCut && c.TypeOfTask == 1 && c.Document3 == "CAL" && c.PostingDate >= startdate && c.PostingDate <= enddate).Sum(c => c.Quantity) * 2;
                        sumReturn = JobLedgerNews.Where(c => c.JobNo == JobNo && c.JobMain == std.JobTaskCut && c.TypeOfTask == 1 && c.PostingDate >= startdate && c.PostingDate <= enddate).Sum(c => c.Quantity);
                        sumIssueExternal = JobLedgerNews.Where(c => c.JobNo != JobNo && c.JobMain == std.JobTaskCut && c.TypeOfTask == 3 && c.PostingDate >= startdate && c.FromLocation == JobNo && c.PostingDate <= enddate).Sum(c => c.Quantity);
                        ThisPeriodQty = (sumReclass + sumIssue + sumRental + sumReturnCal) - (sumReturn + sumIssueExternal);
                        current.ThisPeriodQty = ThisPeriodQty;


                        sumReclass = 0;
                        sumIssue = 0;
                        sumRental = 0;
                        sumReturnCal = 0;
                        sumReturn = 0;
                        sumIssueExternal = 0;
                        sumReclass = JobLedgerNews.Where(c => c.JobNo == JobNo && c.JobMain == std.JobTaskCut && c.TypeOfTask == 0 && c.PostingDate >= startdate && c.PostingDate <= enddate).Sum(c => c.TotalCost);
                        sumIssue = JobLedgerNews.Where(c => c.JobNo == JobNo && c.JobMain == std.JobTaskCut && c.TypeOfTask == 3 && c.PostingDate >= startdate && c.PostingDate <= enddate).Sum(c => c.TotalCost);
                        sumRental = JobLedgerNews.Where(c => c.JobNo == JobNo && c.JobMain == std.JobTaskCut && c.TypeOfTask == 2 && c.PostingDate >= startdate && c.PostingDate <= enddate).Sum(c => c.TotalCost);
                        sumReturnCal = JobLedgerNews.Where(c => c.JobNo == JobNo && c.JobMain == std.JobTaskCut && c.TypeOfTask == 1 && c.Document3 == "CAL" && c.PostingDate >= startdate && c.PostingDate <= enddate).Sum(c => c.TotalCost) * 2;
                        sumReturn = JobLedgerNews.Where(c => c.JobNo == JobNo && c.JobMain == std.JobTaskCut && c.TypeOfTask == 1 && c.PostingDate >= startdate && c.PostingDate <= enddate).Sum(c => c.TotalCost);
                        sumIssueExternal = JobLedgerNews.Where(c => c.JobNo != JobNo && c.JobMain == std.JobTaskCut && c.TypeOfTask == 3 && c.FromLocation == JobNo && c.PostingDate >= startdate && c.PostingDate <= enddate).Sum(c => c.TotalCost);
                        sumReclassAPO = JobLedgerNews.Where(c => c.JobNo == JobNo && c.JobMain == std.JobTaskCut && c.TypeOfTask == 0 && c.PostingDate>=startdate && c.PostingDate <= enddate && c.TotalCost == 0).Sum(c => c.Total);
                        ThisPeriodAMT = (sumReclass + sumIssue + sumRental + sumReturnCal+sumReclassAPO) - (sumReturn + sumIssueExternal);
                        current.ThisPeriodAMT = ThisPeriodAMT;


                        sumReclass = 0;
                        sumIssue = 0;
                        sumRental = 0;
                        sumReturnCal = 0;
                        sumReturn = 0;
                        sumIssueExternal = 0;
                        sumReclass = JobLedgerNews.Where(c => c.JobNo == JobNo && c.JobMain == std.JobTaskCut && c.TypeOfTask == 0 && c.PostingDate < startdate).Sum(c => c.Quantity);
                        sumIssue = JobLedgerNews.Where(c => c.JobNo == JobNo && c.JobMain == std.JobTaskCut && c.TypeOfTask == 3 && c.PostingDate < startdate).Sum(c => c.Quantity);
                        sumRental = JobLedgerNews.Where(c => c.JobNo == JobNo && c.JobMain == std.JobTaskCut && c.TypeOfTask == 2 && c.PostingDate < startdate).Sum(c => c.Quantity);
                        sumReturnCal = JobLedgerNews.Where(c => c.JobNo == JobNo && c.JobMain == std.JobTaskCut && c.TypeOfTask == 1 && c.Document3 == "CAL" && c.PostingDate < startdate).Sum(c => c.Quantity) * 2;
                        sumReturn = JobLedgerNews.Where(c => c.JobNo == JobNo && c.JobMain == std.JobTaskCut && c.TypeOfTask == 1 && c.PostingDate < startdate).Sum(c => c.Quantity);
                        sumIssueExternal = JobLedgerNews.Where(c => c.JobNo != JobNo && c.JobMain == std.JobTaskCut && c.TypeOfTask == 3 && c.PostingDate < startdate && c.FromLocation == JobNo).Sum(c => c.Quantity);
                        OpenningQTY = (sumReclass + sumIssue + sumRental + sumReturnCal) - (sumReturn + sumIssueExternal);
                        current.OpenningQTY = OpenningQTY;



                        sumReclass = 0;
                        sumIssue = 0;
                        sumRental = 0;
                        sumReturnCal = 0;
                        sumReturn = 0;
                        sumIssueExternal = 0;
                        sumReclass = JobLedgerNews.Where(c => c.JobNo == JobNo && c.JobMain == std.JobTaskCut && c.TypeOfTask == 0 && c.PostingDate < startdate).Sum(c => c.TotalCost);
                        sumIssue = JobLedgerNews.Where(c => c.JobNo == JobNo && c.JobMain == std.JobTaskCut && c.TypeOfTask == 3 && c.PostingDate < startdate).Sum(c => c.TotalCost);
                        sumRental = JobLedgerNews.Where(c => c.JobNo == JobNo && c.JobMain == std.JobTaskCut && c.TypeOfTask == 2 && c.PostingDate < startdate).Sum(c => c.TotalCost);
                        sumReturnCal = JobLedgerNews.Where(c => c.JobNo == JobNo && c.JobMain == std.JobTaskCut && c.TypeOfTask == 1 && c.Document3 == "CAL" && c.PostingDate < startdate).Sum(c => c.TotalCost) * 2;
                        sumReturn = JobLedgerNews.Where(c => c.JobNo == JobNo && c.JobMain == std.JobTaskCut && c.TypeOfTask == 1 && c.PostingDate < startdate).Sum(c => c.TotalCost);
                        sumIssueExternal = JobLedgerNews.Where(c => c.JobNo != JobNo && c.JobMain == std.JobTaskCut && c.TypeOfTask == 3 && c.FromLocation == JobNo && c.PostingDate < startdate).Sum(c => c.TotalCost);
                        sumReclassAPO = JobLedgerNews.Where(c => c.JobNo == JobNo && c.JobMain == std.JobTaskCut && c.TypeOfTask == 0 && c.PostingDate <startdate && c.TotalCost == 0).Sum(c => c.Total);
                        OpenningAMT = (sumReclass + sumIssue + sumRental + sumReturnCal+sumReclassAPO) - (sumReturn + sumIssueExternal);
                        current.OpenningAMT = OpenningAMT;



                        sumReclass = 0;
                        sumIssue = 0;
                        sumRental = 0;
                        sumReturnCal = 0;
                        sumReturn = 0;
                        sumIssueExternal = 0;
                        sumReclass = JobLedgerNews.Where(c => c.JobNo == JobNo && c.JobMain == std.JobTaskCut && c.TypeOfTask == 0 && c.PostingDate <= enddate).Sum(c => c.Quantity);
                        sumIssue = JobLedgerNews.Where(c => c.JobNo == JobNo && c.JobMain == std.JobTaskCut && c.TypeOfTask == 3 && c.PostingDate <= enddate).Sum(c => c.Quantity);
                        sumRental = JobLedgerNews.Where(c => c.JobNo == JobNo && c.JobMain == std.JobTaskCut && c.TypeOfTask == 2 && c.PostingDate <= enddate).Sum(c => c.Quantity);
                        sumReturnCal = JobLedgerNews.Where(c => c.JobNo == JobNo && c.JobMain == std.JobTaskCut && c.TypeOfTask == 1 && c.Document3 == "CAL" && c.PostingDate <= enddate).Sum(c => c.Quantity) * 2;
                        sumReturn = JobLedgerNews.Where(c => c.JobNo == JobNo && c.JobMain == std.JobTaskCut && c.TypeOfTask == 1 && c.PostingDate <= enddate).Sum(c => c.Quantity);
                        sumIssueExternal = JobLedgerNews.Where(c => c.JobNo != JobNo && c.JobMain == std.JobTaskCut && c.TypeOfTask == 3 && c.PostingDate <= enddate && c.FromLocation == JobNo).Sum(c => c.Quantity);
                        CumBalQTY = (sumReclass + sumIssue + sumRental + sumReturnCal) - (sumReturn + sumIssueExternal);
                        current.CumBalQTY = CumBalQTY;


                        sumReclass = 0;
                        sumIssue = 0;
                        sumRental = 0;
                        sumReturnCal = 0;
                        sumReturn = 0;
                        sumIssueExternal = 0;
                        sumReclass = JobLedgerNews.Where(c => c.JobNo == JobNo && c.JobMain == std.JobTaskCut && c.TypeOfTask == 0 && c.PostingDate <= enddate).Sum(c => c.TotalCost);
                        sumIssue = JobLedgerNews.Where(c => c.JobNo == JobNo && c.JobMain == std.JobTaskCut && c.TypeOfTask == 3 && c.PostingDate <= enddate).Sum(c => c.TotalCost);
                        sumRental = JobLedgerNews.Where(c => c.JobNo == JobNo && c.JobMain == std.JobTaskCut && c.TypeOfTask == 2 && c.PostingDate <= enddate).Sum(c => c.TotalCost);
                        sumReturnCal = JobLedgerNews.Where(c => c.JobNo == JobNo && c.JobMain == std.JobTaskCut && c.TypeOfTask == 1 && c.Document3 == "CAL" && c.PostingDate <= enddate).Sum(c => c.TotalCost) * 2;
                        sumReturn = JobLedgerNews.Where(c => c.JobNo == JobNo && c.JobMain == std.JobTaskCut && c.TypeOfTask == 1 && c.PostingDate <= enddate).Sum(c => c.TotalCost);
                        sumIssueExternal = JobLedgerNews.Where(c => c.JobNo != JobNo && c.JobMain == std.JobTaskCut && c.TypeOfTask == 3 && c.FromLocation == JobNo && c.PostingDate <= enddate).Sum(c => c.TotalCost);
                        sumReclassAPO = JobLedgerNews.Where(c => c.JobNo == JobNo && c.JobMain == std.JobTaskCut && c.TypeOfTask == 0 && c.PostingDate <= enddate && c.TotalCost==0).Sum(c => c.Total);
                        CumBalAMT = (sumReclass + sumIssue + sumRental + sumReturnCal+sumReclassAPO) - (sumReturn + sumIssueExternal);
                        current.CumBalAMT = CumBalAMT;


                        BudgetQTY = jobPlanningLines.Where(c => c.JobNo == JobNo && c.MainJobID == std.JobTaskCut).Sum(c => c.Quantity);
                        BudgetAMT = jobPlanningLines.Where(c => c.JobNo == JobNo && c.MainJobID == std.JobTaskCut).Sum(c => c.TotalCost);
                        if (BudgetQTY == 0)
                        {
                            PercenQTY = 0;
                        }
                        else
                        {
                            PercenQTY = (CumBalQTY / BudgetQTY) * 100;
                        }

                        if (BudgetAMT == 0)
                        {
                            PercenAMT = 0;
                        }
                        else
                        {
                            PercenAMT = (CumBalAMT / BudgetAMT) * 100;
                        }

                        current.BudgetQTY = BudgetQTY;
                        current.BudgetAMT = BudgetAMT;
                        current.PercenAMT = PercenAMT;
                        current.PercenQTY = PercenQTY;


                    }
                    CurrentSubTotal = new Tmp_JobCostReport();
                    CurrentSubTotal.JobTaskFull = std.MainJobID;
                    CurrentSubTotal.ThisPeriodQty = ThisPeriodQty;
                    CurrentSubTotal.ThisPeriodAMT = ThisPeriodAMT;
                    CurrentSubTotal.OpenningQTY = OpenningQTY;
                    CurrentSubTotal.OpenningAMT = OpenningAMT;
                    CurrentSubTotal.CumBalQTY = CumBalQTY;
                    CurrentSubTotal.CumBalAMT = CumBalAMT;
                    CurrentSubTotal.BudgetQTY = BudgetQTY;
                    CurrentSubTotal.BudgetAMT = BudgetAMT;
                    CurrentSubTotal.PercenQTY = PercenQTY;
                    CurrentSubTotal.PercenAMT = PercenAMT;

                    if (std.MainJobID == "A0000")
                    {
                        CurrentSubTotal.JobSub = "04";
                        A00 = CumBalAMT;
                        ViewBag.A00 = SetFontRed(CumBalAMT, 1);
                        TCXI += CumBalAMT;

                    }
                    if (std.MainJobID == "B0000")
                    {
                        CurrentSubTotal.JobSub = "05";
                        B00 = CumBalAMT;
                        ViewBag.B00 = SetFontRed(CumBalAMT, 1);
                        TCXI += CumBalAMT;
                    }
                    if (std.MainJobID == "C0000")
                    {
                        CurrentSubTotal.JobSub = "06";
                        C00 = CumBalAMT;
                        ViewBag.C00 = SetFontRed(CumBalAMT, 1);
                        TCXI += CumBalAMT;
                    }
                    if (std.MainJobID == "D0000")
                    {
                        CurrentSubTotal.JobSub = "07";
                        D00 = CumBalAMT;
                        ViewBag.D00 = SetFontRed(CumBalAMT, 1);
                        TCXI += CumBalAMT;
                    }
                    if (std.MainJobID == "E0000")
                    {
                        CurrentSubTotal.JobSub = "08";
                        E00 = CumBalAMT;
                        ViewBag.E00 = SetFontRed(CumBalAMT, 1);
                        TCXI += CumBalAMT;
                    }
                    if (std.MainJobID == "F0000")
                    {
                        CurrentSubTotal.JobSub = "09";
                        F00 = CumBalAMT;
                        ViewBag.F00 = SetFontRed(CumBalAMT, 1);
                        TCXI += CumBalAMT;
                    }

                    if (std.MainJobID == "G0000")
                    {
                        CurrentSubTotal.JobSub = "10";
                        G00 = CumBalAMT;
                        ViewBag.G00 = SetFontRed(CumBalAMT, 1);
                        TCXI += CumBalAMT;
                    }
                    if (std.MainJobID == "H0000")
                    {
                        CurrentSubTotal.JobSub = "11";
                        H00 = CumBalAMT;
                        ViewBag.H00 = SetFontRed(CumBalAMT, 1);
                        TCXI += CumBalAMT;
                    }
                    if (std.MainJobID == "N0000")
                    {
                        CurrentSubTotal.JobSub = "01";
                        N00 = CumBalAMT;
                        CurrentSubTotal.JobSub = "1";
                    }

                    if (std.MainJobID == "O0000")
                    {
                        CurrentSubTotal.JobSub = "12";
                        O00 = CumBalAMT;
                        CurrentSubTotal.JobSub = "2";
                    }


                   SubTotal.Add(CurrentSubTotal);

                }
                else if(std.JobTaskCut==std.SubJobID) //ถ้าเป็น Sub ให้ทำ
                {

                    if (std.JobstringCust == "O" || std.JobstringCust == "N")
                    {
                        sumReclass = 0;
                        sumIssue = 0;
                        sumRental = 0;
                        sumReturnCal = 0;
                        sumReturn = 0;
                        sumIssueExternal = 0;
                        sumReclass = JobLedgerNews.Where(c => c.JobNo == JobNo && c.JobSub == std.JobTaskCut && c.TypeOfTask == 0 && c.PostingDate >= startdate && c.PostingDate <= enddate).Sum(c => c.Quantity);
                        sumIssue = JobLedgerNews.Where(c => c.JobNo == JobNo && c.JobSub == std.JobTaskCut && c.TypeOfTask == 3 && c.PostingDate >= startdate && c.PostingDate <= enddate).Sum(c => c.Quantity);
                        sumRental = JobLedgerNews.Where(c => c.JobNo == JobNo && c.JobSub == std.JobTaskCut && c.TypeOfTask == 2 && c.PostingDate >= startdate && c.PostingDate <= enddate).Sum(c => c.Quantity);
                        sumReturnCal = JobLedgerNews.Where(c => c.JobNo == JobNo && c.JobSub == std.JobTaskCut && c.TypeOfTask == 1 && c.Document3 == "CAL" && c.PostingDate >= startdate && c.PostingDate <= enddate).Sum(c => c.Quantity) * 2;
                        sumReturn = JobLedgerNews.Where(c => c.JobNo == JobNo && c.JobSub == std.JobTaskCut && c.TypeOfTask == 1 && c.PostingDate >= startdate && c.PostingDate <= enddate).Sum(c => c.Quantity);
                        sumIssueExternal = JobLedgerNews.Where(c => c.JobNo != JobNo && c.JobSub == std.JobTaskCut && c.TypeOfTask == 3 && c.PostingDate >= startdate && c.FromLocation == JobNo && c.PostingDate <= enddate).Sum(c => c.Quantity);
                        ThisPeriodQty = (sumReclass + sumIssue + sumRental + sumReturnCal) - (sumReturn + sumIssueExternal);
                        current.ThisPeriodQty = ThisPeriodQty;

                        sumReclass = 0;
                        sumIssue = 0;
                        sumRental = 0;
                        sumReturnCal = 0;
                        sumReturn = 0;
                        sumIssueExternal = 0;
                        sumReclass = JobLedgerNews.Where(c => c.JobNo == JobNo && c.JobSub == std.JobTaskCut && c.TypeOfTask == 0 && c.PostingDate >= startdate && c.PostingDate <= enddate).Sum(c => c.TotalCost + c.LineAmount);
                        sumIssue = JobLedgerNews.Where(c => c.JobNo == JobNo && c.JobSub == std.JobTaskCut && c.TypeOfTask == 3 && c.PostingDate >= startdate && c.PostingDate <= enddate).Sum(c => c.TotalCost + c.LineAmount);
                        sumRental = JobLedgerNews.Where(c => c.JobNo == JobNo && c.JobSub == std.JobTaskCut && c.TypeOfTask == 2 && c.PostingDate >= startdate && c.PostingDate <= enddate).Sum(c => c.TotalCost + c.LineAmount);
                        sumReturnCal = JobLedgerNews.Where(c => c.JobNo == JobNo && c.JobSub == std.JobTaskCut && c.TypeOfTask == 1 && c.Document3 == "CAL" && c.PostingDate >= startdate && c.PostingDate <= enddate).Sum(c => c.TotalCost + c.LineAmount) * 2;
                        sumReturn = JobLedgerNews.Where(c => c.JobNo == JobNo && c.JobSub == std.JobTaskCut && c.TypeOfTask == 1 && c.PostingDate >= startdate && c.PostingDate <= enddate).Sum(c => c.TotalCost + c.LineAmount);
                        sumIssueExternal = JobLedgerNews.Where(c => c.JobNo != JobNo && c.JobSub == std.JobTaskCut && c.TypeOfTask == 3 && c.FromLocation == JobNo && c.PostingDate >= startdate && c.PostingDate <= enddate).Sum(c => c.TotalCost + c.LineAmount);
                        sumReclassAPO = JobLedgerNews.Where(c => c.JobNo != JobNo && c.JobSub == std.JobTaskCut && c.TypeOfTask == 0 && c.PostingDate >= startdate && c.PostingDate <= enddate && c.TotalCost == 0).Sum(c => c.Total);
                        ThisPeriodAMT = (sumReclass + sumIssue + sumRental + sumReturnCal+sumReclassAPO) - (sumReturn + sumIssueExternal);
                        current.ThisPeriodAMT = ThisPeriodAMT;

                        sumReclass = 0;
                        sumIssue = 0;
                        sumRental = 0;
                        sumReturnCal = 0;
                        sumReturn = 0;
                        sumIssueExternal = 0;
                        sumReclass = JobLedgerNews.Where(c => c.JobNo == JobNo && c.JobSub == std.JobTaskCut && c.TypeOfTask == 0 && c.PostingDate < startdate).Sum(c => c.Quantity);
                        sumIssue = JobLedgerNews.Where(c => c.JobNo == JobNo && c.JobSub == std.JobTaskCut && c.TypeOfTask == 3 && c.PostingDate < startdate).Sum(c => c.Quantity);
                        sumRental = JobLedgerNews.Where(c => c.JobNo == JobNo && c.JobSub == std.JobTaskCut && c.TypeOfTask == 2 && c.PostingDate < startdate).Sum(c => c.Quantity);
                        sumReturnCal = JobLedgerNews.Where(c => c.JobNo == JobNo && c.JobSub == std.JobTaskCut && c.TypeOfTask == 1 && c.Document3 == "CAL" && c.PostingDate < startdate).Sum(c => c.Quantity) * 2;
                        sumReturn = JobLedgerNews.Where(c => c.JobNo == JobNo && c.JobSub == std.JobTaskCut && c.TypeOfTask == 1 && c.PostingDate < startdate).Sum(c => c.Quantity);
                        sumIssueExternal = JobLedgerNews.Where(c => c.JobNo != JobNo && c.JobSub == std.JobTaskCut && c.TypeOfTask == 3 && c.PostingDate < startdate && c.FromLocation == JobNo).Sum(c => c.Quantity);
                        OpenningQTY = (sumReclass + sumIssue + sumRental + sumReturnCal) - (sumReturn + sumIssueExternal);
                        current.OpenningQTY = OpenningQTY;


                        sumReclass = 0;
                        sumIssue = 0;
                        sumRental = 0;
                        sumReturnCal = 0;
                        sumReturn = 0;
                        sumIssueExternal = 0;
                        sumReclass = JobLedgerNews.Where(c => c.JobNo == JobNo && c.JobSub == std.JobTaskCut && c.TypeOfTask == 0 && c.PostingDate < startdate).Sum(c => c.TotalCost + c.LineAmount);
                        sumIssue = JobLedgerNews.Where(c => c.JobNo == JobNo && c.JobSub == std.JobTaskCut && c.TypeOfTask == 3 && c.PostingDate < startdate).Sum(c => c.TotalCost + c.LineAmount);
                        sumRental = JobLedgerNews.Where(c => c.JobNo == JobNo && c.JobSub == std.JobTaskCut && c.TypeOfTask == 2 && c.PostingDate < startdate).Sum(c => c.TotalCost + c.LineAmount);
                        sumReturnCal = JobLedgerNews.Where(c => c.JobNo == JobNo && c.JobSub == std.JobTaskCut && c.TypeOfTask == 1 && c.Document3 == "CAL" && c.PostingDate < startdate).Sum(c => c.TotalCost + c.LineAmount) * 2;
                        sumReturn = JobLedgerNews.Where(c => c.JobNo == JobNo && c.JobSub == std.JobTaskCut && c.TypeOfTask == 1 && c.PostingDate < startdate).Sum(c => c.TotalCost + c.LineAmount);
                        sumIssueExternal = JobLedgerNews.Where(c => c.JobNo != JobNo && c.JobSub == std.JobTaskCut && c.TypeOfTask == 3 && c.FromLocation == JobNo && c.PostingDate < startdate).Sum(c => c.TotalCost + c.LineAmount);
                        sumReclassAPO = JobLedgerNews.Where(c => c.JobNo != JobNo && c.JobSub == std.JobTaskCut && c.TypeOfTask == 0 && c.PostingDate < startdate && c.TotalCost == 0).Sum(c => c.Total);
                        OpenningAMT = (sumReclass + sumIssue + sumRental + sumReturnCal+sumReclassAPO) - (sumReturn + sumIssueExternal);
                        current.OpenningAMT = OpenningAMT;


                        sumReclass = 0;
                        sumIssue = 0;
                        sumRental = 0;
                        sumReturnCal = 0;
                        sumReturn = 0;
                        sumIssueExternal = 0;
                        sumReclass = JobLedgerNews.Where(c => c.JobNo == JobNo && c.JobSub == std.JobTaskCut && c.TypeOfTask == 0 && c.PostingDate <= enddate).Sum(c => c.Quantity);
                        sumIssue = JobLedgerNews.Where(c => c.JobNo == JobNo && c.JobSub == std.JobTaskCut && c.TypeOfTask == 3 && c.PostingDate <= enddate).Sum(c => c.Quantity);
                        sumRental = JobLedgerNews.Where(c => c.JobNo == JobNo && c.JobSub == std.JobTaskCut && c.TypeOfTask == 2 && c.PostingDate <= enddate).Sum(c => c.Quantity);
                        sumReturnCal = JobLedgerNews.Where(c => c.JobNo == JobNo && c.JobSub == std.JobTaskCut && c.TypeOfTask == 1 && c.Document3 == "CAL" && c.PostingDate <= enddate).Sum(c => c.Quantity) * 2;
                        sumReturn = JobLedgerNews.Where(c => c.JobNo == JobNo && c.JobSub == std.JobTaskCut && c.TypeOfTask == 1 && c.PostingDate <= enddate).Sum(c => c.Quantity);
                        sumIssueExternal = JobLedgerNews.Where(c => c.JobNo != JobNo && c.JobSub == std.JobTaskCut && c.TypeOfTask == 3 && c.PostingDate <= enddate && c.FromLocation == JobNo).Sum(c => c.Quantity);
                        CumBalQTY = (sumReclass + sumIssue + sumRental + sumReturnCal) - (sumReturn + sumIssueExternal);
                        current.CumBalQTY = CumBalQTY;


                        sumReclass = 0;
                        sumIssue = 0;
                        sumRental = 0;
                        sumReturnCal = 0;
                        sumReturn = 0;
                        sumIssueExternal = 0;
                        sumReclass = JobLedgerNews.Where(c => c.JobNo == JobNo && c.JobSub == std.JobTaskCut && c.TypeOfTask == 0 && c.PostingDate <= enddate).Sum(c => c.TotalCost + c.LineAmount);
                        sumIssue = JobLedgerNews.Where(c => c.JobNo == JobNo && c.JobSub == std.JobTaskCut && c.TypeOfTask == 3 && c.PostingDate <= enddate).Sum(c => c.TotalCost + c.LineAmount);
                        sumRental = JobLedgerNews.Where(c => c.JobNo == JobNo && c.JobSub == std.JobTaskCut && c.TypeOfTask == 2 && c.PostingDate <= enddate).Sum(c => c.TotalCost + c.LineAmount);
                        sumReturnCal = JobLedgerNews.Where(c => c.JobNo == JobNo && c.JobSub == std.JobTaskCut && c.TypeOfTask == 1 && c.Document3 == "CAL" && c.PostingDate <= enddate).Sum(c => c.TotalCost + c.LineAmount) * 2;
                        sumReturn = JobLedgerNews.Where(c => c.JobNo == JobNo && c.JobSub == std.JobTaskCut && c.TypeOfTask == 1 && c.PostingDate <= enddate).Sum(c => c.TotalCost + c.LineAmount);
                        sumIssueExternal = JobLedgerNews.Where(c => c.JobNo != JobNo && c.JobSub == std.JobTaskCut && c.TypeOfTask == 3 && c.FromLocation == JobNo && c.PostingDate <= enddate).Sum(c => c.TotalCost + c.LineAmount);
                        sumReclassAPO = JobLedgerNews.Where(c => c.JobNo != JobNo && c.JobSub == std.JobTaskCut && c.TypeOfTask == 0 && c.PostingDate <= enddate && c.TotalCost == 0).Sum(c => c.Total);
                        CumBalAMT = (sumReclass + sumIssue + sumRental + sumReturnCal+sumReclassAPO) - (sumReturn + sumIssueExternal);
                        current.CumBalAMT = CumBalAMT;



                        BudgetQTY = jobPlanningLines.Where(c => c.JobNo == JobNo && c.SubJobID == std.JobTaskCut).Sum(c => c.Quantity);
                        BudgetAMT = jobPlanningLines.Where(c => c.JobNo == JobNo && c.SubJobID == std.JobTaskCut).Sum(c => c.TotalCost);
                        if (BudgetQTY == 0)
                        {
                            PercenQTY = 0;
                        }
                        else
                        {
                            PercenQTY = (CumBalQTY / BudgetQTY) * 100;
                        }

                        if (BudgetAMT == 0)
                        {
                            PercenAMT = 0;
                        }
                        else
                        {
                            PercenAMT = (CumBalAMT / BudgetAMT) * 100;
                        }

                        current.BudgetQTY = BudgetQTY;
                        current.BudgetAMT = BudgetAMT;
                        current.PercenAMT = PercenAMT;
                        current.PercenQTY = PercenQTY;


                    }
                    else
                    {
                        sumReclass = 0;
                        sumIssue = 0;
                        sumRental = 0;
                        sumReturnCal = 0;
                        sumReturn = 0;
                        sumIssueExternal = 0;
                        sumReclass = JobLedgerNews.Where(c => c.JobNo == JobNo && c.JobSub == std.JobTaskCut && c.TypeOfTask == 0 && c.PostingDate >= startdate && c.PostingDate <= enddate).Sum(c => c.Quantity);
                        sumIssue = JobLedgerNews.Where(c => c.JobNo == JobNo && c.JobSub == std.JobTaskCut && c.TypeOfTask == 3 && c.PostingDate >= startdate && c.PostingDate <= enddate).Sum(c => c.Quantity);
                        sumRental = JobLedgerNews.Where(c => c.JobNo == JobNo && c.JobSub == std.JobTaskCut && c.TypeOfTask == 2 && c.PostingDate >= startdate && c.PostingDate <= enddate).Sum(c => c.Quantity);
                        sumReturnCal = JobLedgerNews.Where(c => c.JobNo == JobNo && c.JobSub == std.JobTaskCut && c.TypeOfTask == 1 && c.Document3 == "CAL" && c.PostingDate >= startdate && c.PostingDate <= enddate).Sum(c => c.Quantity) * 2;
                        sumReturn = JobLedgerNews.Where(c => c.JobNo == JobNo && c.JobSub == std.JobTaskCut && c.TypeOfTask == 1 && c.PostingDate >= startdate && c.PostingDate <= enddate).Sum(c => c.Quantity);
                        sumIssueExternal = JobLedgerNews.Where(c => c.JobNo != JobNo && c.JobSub == std.JobTaskCut && c.TypeOfTask == 3 && c.PostingDate >= startdate && c.FromLocation == JobNo && c.PostingDate <= enddate).Sum(c => c.Quantity);
                        ThisPeriodQty = (sumReclass + sumIssue + sumRental + sumReturnCal) - (sumReturn + sumIssueExternal);
                        current.ThisPeriodQty = ThisPeriodQty;

                        sumReclass = 0;
                        sumIssue = 0;
                        sumRental = 0;
                        sumReturnCal = 0;
                        sumReturn = 0;
                        sumIssueExternal = 0;
                        sumReclass = JobLedgerNews.Where(c => c.JobNo == JobNo && c.JobSub == std.JobTaskCut && c.TypeOfTask == 0 && c.PostingDate >= startdate && c.PostingDate <= enddate).Sum(c => c.TotalCost);
                        sumIssue = JobLedgerNews.Where(c => c.JobNo == JobNo && c.JobSub == std.JobTaskCut && c.TypeOfTask == 3 && c.PostingDate >= startdate && c.PostingDate <= enddate).Sum(c => c.TotalCost);
                        sumRental = JobLedgerNews.Where(c => c.JobNo == JobNo && c.JobSub == std.JobTaskCut && c.TypeOfTask == 2 && c.PostingDate >= startdate && c.PostingDate <= enddate).Sum(c => c.TotalCost);
                        sumReturnCal = JobLedgerNews.Where(c => c.JobNo == JobNo && c.JobSub == std.JobTaskCut && c.TypeOfTask == 1 && c.Document3 == "CAL" && c.PostingDate >= startdate && c.PostingDate <= enddate).Sum(c => c.TotalCost) * 2;
                        sumReturn = JobLedgerNews.Where(c => c.JobNo == JobNo && c.JobSub == std.JobTaskCut && c.TypeOfTask == 1 && c.PostingDate >= startdate && c.PostingDate <= enddate).Sum(c => c.TotalCost);
                        sumIssueExternal = JobLedgerNews.Where(c => c.JobNo != JobNo && c.JobSub == std.JobTaskCut && c.TypeOfTask == 3 && c.FromLocation == JobNo && c.PostingDate >= startdate && c.PostingDate <= enddate).Sum(c => c.TotalCost);
                        sumReclassAPO = JobLedgerNews.Where(c => c.JobNo != JobNo && c.JobSub == std.JobTaskCut && c.TypeOfTask == 0 && c.PostingDate >= startdate && c.PostingDate <= enddate && c.TotalCost == 0).Sum(c => c.Total);
                        ThisPeriodAMT = (sumReclass + sumIssue + sumRental + sumReturnCal+sumReclassAPO) - (sumReturn + sumIssueExternal);
                        current.ThisPeriodAMT = ThisPeriodAMT;

                        sumReclass = 0;
                        sumIssue = 0;
                        sumRental = 0;
                        sumReturnCal = 0;
                        sumReturn = 0;
                        sumIssueExternal = 0;
                        sumReclass = JobLedgerNews.Where(c => c.JobNo == JobNo && c.JobSub == std.JobTaskCut && c.TypeOfTask == 0 && c.PostingDate < startdate).Sum(c => c.Quantity);
                        sumIssue = JobLedgerNews.Where(c => c.JobNo == JobNo && c.JobSub == std.JobTaskCut && c.TypeOfTask == 3 && c.PostingDate < startdate).Sum(c => c.Quantity);
                        sumRental = JobLedgerNews.Where(c => c.JobNo == JobNo && c.JobSub == std.JobTaskCut && c.TypeOfTask == 2 && c.PostingDate < startdate).Sum(c => c.Quantity);
                        sumReturnCal = JobLedgerNews.Where(c => c.JobNo == JobNo && c.JobSub == std.JobTaskCut && c.TypeOfTask == 1 && c.Document3 == "CAL" && c.PostingDate < startdate).Sum(c => c.Quantity) * 2;
                        sumReturn = JobLedgerNews.Where(c => c.JobNo == JobNo && c.JobSub == std.JobTaskCut && c.TypeOfTask == 1 && c.PostingDate < startdate).Sum(c => c.Quantity);
                        sumIssueExternal = JobLedgerNews.Where(c => c.JobNo != JobNo && c.JobSub == std.JobTaskCut && c.TypeOfTask == 3 && c.PostingDate < startdate && c.FromLocation == JobNo).Sum(c => c.Quantity);
                        OpenningQTY = (sumReclass + sumIssue + sumRental + sumReturnCal) - (sumReturn + sumIssueExternal);
                        current.OpenningQTY = OpenningQTY;


                        sumReclass = 0;
                        sumIssue = 0;
                        sumRental = 0;
                        sumReturnCal = 0;
                        sumReturn = 0;
                        sumIssueExternal = 0;
                        sumReclass = JobLedgerNews.Where(c => c.JobNo == JobNo && c.JobSub == std.JobTaskCut && c.TypeOfTask == 0 && c.PostingDate < startdate).Sum(c => c.TotalCost);
                        sumIssue = JobLedgerNews.Where(c => c.JobNo == JobNo && c.JobSub == std.JobTaskCut && c.TypeOfTask == 3 && c.PostingDate < startdate).Sum(c => c.TotalCost);
                        sumRental = JobLedgerNews.Where(c => c.JobNo == JobNo && c.JobSub == std.JobTaskCut && c.TypeOfTask == 2 && c.PostingDate < startdate).Sum(c => c.TotalCost);
                        sumReturnCal = JobLedgerNews.Where(c => c.JobNo == JobNo && c.JobSub == std.JobTaskCut && c.TypeOfTask == 1 && c.Document3 == "CAL" && c.PostingDate < startdate).Sum(c => c.TotalCost) * 2;
                        sumReturn = JobLedgerNews.Where(c => c.JobNo == JobNo && c.JobSub == std.JobTaskCut && c.TypeOfTask == 1 && c.PostingDate < startdate).Sum(c => c.TotalCost);
                        sumIssueExternal = JobLedgerNews.Where(c => c.JobNo != JobNo && c.JobSub == std.JobTaskCut && c.TypeOfTask == 3 && c.FromLocation == JobNo && c.PostingDate < startdate).Sum(c => c.TotalCost);
                        sumReclassAPO = JobLedgerNews.Where(c => c.JobNo != JobNo && c.JobSub == std.JobTaskCut && c.TypeOfTask == 0 && c.PostingDate < startdate  && c.TotalCost == 0).Sum(c => c.Total);
                        OpenningAMT = (sumReclass + sumIssue + sumRental + sumReturnCal+sumReclassAPO) - (sumReturn + sumIssueExternal);
                        current.OpenningAMT = OpenningAMT;

                        sumReclass = 0;
                        sumIssue = 0;
                        sumRental = 0;
                        sumReturnCal = 0;
                        sumReturn = 0;
                        sumIssueExternal = 0;
                        sumReclass = JobLedgerNews.Where(c => c.JobNo == JobNo && c.JobSub == std.JobTaskCut && c.TypeOfTask == 0 && c.PostingDate <= enddate).Sum(c => c.Quantity);
                        sumIssue = JobLedgerNews.Where(c => c.JobNo == JobNo && c.JobSub == std.JobTaskCut && c.TypeOfTask == 3 && c.PostingDate <= enddate).Sum(c => c.Quantity);
                        sumRental = JobLedgerNews.Where(c => c.JobNo == JobNo && c.JobSub == std.JobTaskCut && c.TypeOfTask == 2 && c.PostingDate <= enddate).Sum(c => c.Quantity);
                        sumReturnCal = JobLedgerNews.Where(c => c.JobNo == JobNo && c.JobSub == std.JobTaskCut && c.TypeOfTask == 1 && c.Document3 == "CAL" && c.PostingDate <= enddate).Sum(c => c.Quantity) * 2;
                        sumReturn = JobLedgerNews.Where(c => c.JobNo == JobNo && c.JobSub == std.JobTaskCut && c.TypeOfTask == 1 && c.PostingDate <= enddate).Sum(c => c.Quantity);
                        sumIssueExternal = JobLedgerNews.Where(c => c.JobNo != JobNo && c.JobSub == std.JobTaskCut && c.TypeOfTask == 3 && c.PostingDate <= enddate && c.FromLocation == JobNo).Sum(c => c.Quantity);
                        CumBalQTY = (sumReclass + sumIssue + sumRental + sumReturnCal) - (sumReturn + sumIssueExternal);
                        current.CumBalQTY = CumBalQTY;

                        sumReclass = 0;
                        sumIssue = 0;
                        sumRental = 0;
                        sumReturnCal = 0;
                        sumReturn = 0;
                        sumIssueExternal = 0;
                        sumReclass = JobLedgerNews.Where(c => c.JobNo == JobNo && c.JobSub == std.JobTaskCut && c.TypeOfTask == 0 && c.PostingDate <= enddate).Sum(c => c.TotalCost);
                        sumIssue = JobLedgerNews.Where(c => c.JobNo == JobNo && c.JobSub == std.JobTaskCut && c.TypeOfTask == 3 && c.PostingDate <= enddate).Sum(c => c.TotalCost);
                        sumRental = JobLedgerNews.Where(c => c.JobNo == JobNo && c.JobSub == std.JobTaskCut && c.TypeOfTask == 2 && c.PostingDate <= enddate).Sum(c => c.TotalCost);
                        sumReturnCal = JobLedgerNews.Where(c => c.JobNo == JobNo && c.JobSub == std.JobTaskCut && c.TypeOfTask == 1 && c.Document3 == "CAL" && c.PostingDate <= enddate).Sum(c => c.TotalCost) * 2;
                        sumReturn = JobLedgerNews.Where(c => c.JobNo == JobNo && c.JobSub == std.JobTaskCut && c.TypeOfTask == 1 && c.PostingDate <= enddate).Sum(c => c.TotalCost);
                        sumIssueExternal = JobLedgerNews.Where(c => c.JobNo != JobNo && c.JobSub == std.JobTaskCut && c.TypeOfTask == 3 && c.FromLocation == JobNo && c.PostingDate <= enddate).Sum(c => c.TotalCost);
                        sumReclassAPO = JobLedgerNews.Where(c => c.JobNo != JobNo && c.JobSub == std.JobTaskCut && c.TypeOfTask == 0  && c.PostingDate <= enddate && c.TotalCost == 0).Sum(c => c.Total);
                        CumBalAMT = (sumReclass + sumIssue + sumRental + sumReturnCal+sumReclassAPO) - (sumReturn + sumIssueExternal);
                        current.CumBalAMT = CumBalAMT;


                        BudgetQTY = jobPlanningLines.Where(c => c.JobNo == JobNo && c.SubJobID == std.JobTaskCut).Sum(c => c.Quantity);
                        BudgetAMT = jobPlanningLines.Where(c => c.JobNo == JobNo && c.SubJobID == std.JobTaskCut).Sum(c => c.TotalCost);
                        if (BudgetQTY == 0)
                        {
                            PercenQTY = 0;
                        }
                        else
                        {
                            PercenQTY = (CumBalQTY / BudgetQTY) * 100;
                        }

                        if (BudgetAMT == 0)
                        {
                            PercenAMT = 0;
                        }
                        else
                        {
                            PercenAMT = (CumBalAMT / BudgetAMT) * 100;
                        }

                        current.BudgetQTY = BudgetQTY;
                        current.BudgetAMT = BudgetAMT;
                        current.PercenAMT = PercenAMT;
                        current.PercenQTY = PercenQTY;

                    }


                    if (std.JobTaskNo == "D2100")
                    {
                        D2100 = CumBalAMT;
                        ViewBag.D2100 = SetFontRed(CumBalAMT, 1);
                    }

                    if (std.JobTaskNo == "E0800")
                    {
                        E0800 = CumBalAMT;
                        ViewBag.E0800 = SetFontRed(CumBalAMT, 1);
                    }
                    if (std.JobTaskNo == "H0400")
                    {
                        H0400 = CumBalAMT;
                        ViewBag.H0400 = SetFontRed(CumBalAMT, 1);
                    }
                    if (std.JobTaskNo == "H0200")
                    {
                        H0200 = CumBalAMT;
                        ViewBag.H0200 = SetFontRed(CumBalAMT, 1);
                    }
                    if (std.JobTaskNo == "H0800")
                    {
                        H0800 = CumBalAMT;
                        ViewBag.H0800 = SetFontRed(CumBalAMT, 1);
                    }
                    if (std.JobTaskNo == "H0600")
                    {
                        H0600 = CumBalAMT;
                        ViewBag.H0600 = SetFontRed(CumBalAMT, 1);
                    }


                }
                else  //ถ้าเป็นทั่วไปให้ทำ
                { 


                    if (std.JobstringCust == "O" || std.JobstringCust == "N")
                    {
                        sumReclass = 0;
                        sumIssue = 0;
                        sumRental = 0;
                        sumReturnCal = 0;
                        sumReturn = 0;
                        sumIssueExternal = 0;
                        sumReclass = JobLedgerNews.Where(c => c.JobNo == JobNo && c.JobLedgerEntry == std.JobTaskCut && c.TypeOfTask == 0 && c.PostingDate >= startdate && c.PostingDate <= enddate).Sum(c => c.Quantity);
                        sumIssue = JobLedgerNews.Where(c => c.JobNo == JobNo && c.JobLedgerEntry == std.JobTaskCut && c.TypeOfTask == 3 && c.PostingDate >= startdate && c.PostingDate <= enddate).Sum(c => c.Quantity);
                        sumRental = JobLedgerNews.Where(c => c.JobNo == JobNo && c.JobLedgerEntry == std.JobTaskCut && c.TypeOfTask == 2 && c.PostingDate >= startdate && c.PostingDate <= enddate).Sum(c => c.Quantity);
                        sumReturnCal = JobLedgerNews.Where(c => c.JobNo == JobNo && c.JobLedgerEntry == std.JobTaskCut && c.TypeOfTask == 1 && c.Document3 == "CAL" && c.PostingDate >= startdate && c.PostingDate <= enddate).Sum(c => c.Quantity) * 2;
                        sumReturn = JobLedgerNews.Where(c => c.JobNo == JobNo && c.JobLedgerEntry == std.JobTaskCut && c.TypeOfTask == 1 && c.PostingDate >= startdate && c.PostingDate <= enddate).Sum(c => c.Quantity);
                        sumIssueExternal = JobLedgerNews.Where(c => c.JobNo != JobNo && c.JobLedgerEntry == std.JobTaskCut && c.TypeOfTask == 3 && c.PostingDate >= startdate && c.FromLocation == JobNo && c.PostingDate <= enddate).Sum(c => c.Quantity);
                       
                        ThisPeriodQty = (sumReclass + sumIssue + sumRental + sumReturnCal) - (sumReturn + sumIssueExternal);
                        current.ThisPeriodQty = ThisPeriodQty;

                        sumReclass = 0;
                        sumIssue = 0;
                        sumRental = 0;
                        sumReturnCal = 0;
                        sumReturn = 0;
                        sumIssueExternal = 0;
                        sumReclass = JobLedgerNews.Where(c => c.JobNo == JobNo && c.JobLedgerEntry == std.JobTaskCut && c.TypeOfTask == 0 && c.PostingDate >= startdate && c.PostingDate <= enddate).Sum(c => c.TotalCost +c.LineAmount);
                        sumIssue = JobLedgerNews.Where(c => c.JobNo == JobNo && c.JobLedgerEntry == std.JobTaskCut && c.TypeOfTask == 3 && c.PostingDate >= startdate && c.PostingDate <= enddate).Sum(c => c.TotalCost + c.LineAmount);
                        sumRental = JobLedgerNews.Where(c => c.JobNo == JobNo && c.JobLedgerEntry == std.JobTaskCut && c.TypeOfTask == 2 && c.PostingDate >= startdate && c.PostingDate <= enddate).Sum(c => c.TotalCost + c.LineAmount);
                        sumReturnCal = JobLedgerNews.Where(c => c.JobNo == JobNo && c.JobLedgerEntry == std.JobTaskCut && c.TypeOfTask == 1 && c.Document3 == "CAL" && c.PostingDate >= startdate && c.PostingDate <= enddate).Sum(c => c.TotalCost + c.LineAmount) * 2;
                        sumReturn = JobLedgerNews.Where(c => c.JobNo == JobNo && c.JobLedgerEntry == std.JobTaskCut && c.TypeOfTask == 1 && c.PostingDate >= startdate && c.PostingDate <= enddate).Sum(c => c.TotalCost + c.LineAmount);
                        sumIssueExternal = JobLedgerNews.Where(c => c.JobNo != JobNo && c.JobLedgerEntry == std.JobTaskCut && c.TypeOfTask == 3 && c.FromLocation == JobNo && c.PostingDate >= startdate && c.PostingDate <= enddate).Sum(c => c.TotalCost + c.LineAmount);
                        sumReclassAPO = JobLedgerNews.Where(c => c.JobNo != JobNo && c.JobLedgerEntry == std.JobTaskCut && c.TypeOfTask == 0 && c.PostingDate >= startdate && c.PostingDate <= enddate && c.TotalCost == 0).Sum(c => c.Total);
                        ThisPeriodAMT = (sumReclass + sumIssue + sumRental + sumReturnCal+sumReclassAPO) - (sumReturn + sumIssueExternal);
                        current.ThisPeriodAMT = ThisPeriodAMT;

                        sumReclass = 0;
                        sumIssue = 0;
                        sumRental = 0;
                        sumReturnCal = 0;
                        sumReturn = 0;
                        sumIssueExternal = 0;
                        sumReclass = JobLedgerNews.Where(c => c.JobNo == JobNo && c.JobLedgerEntry == std.JobTaskCut && c.TypeOfTask == 0 && c.PostingDate < startdate).Sum(c => c.Quantity);
                        sumIssue = JobLedgerNews.Where(c => c.JobNo == JobNo && c.JobLedgerEntry == std.JobTaskCut && c.TypeOfTask == 3 && c.PostingDate < startdate).Sum(c => c.Quantity);
                        sumRental = JobLedgerNews.Where(c => c.JobNo == JobNo && c.JobLedgerEntry == std.JobTaskCut && c.TypeOfTask == 2 && c.PostingDate < startdate).Sum(c => c.Quantity);
                        sumReturnCal = JobLedgerNews.Where(c => c.JobNo == JobNo && c.JobLedgerEntry == std.JobTaskCut && c.TypeOfTask == 1 && c.Document3 == "CAL" && c.PostingDate < startdate).Sum(c => c.Quantity) * 2;
                        sumReturn = JobLedgerNews.Where(c => c.JobNo == JobNo && c.JobLedgerEntry == std.JobTaskCut && c.TypeOfTask == 1 && c.PostingDate < startdate).Sum(c => c.Quantity);
                        sumIssueExternal = JobLedgerNews.Where(c => c.JobNo != JobNo && c.JobLedgerEntry == std.JobTaskCut && c.TypeOfTask == 3 && c.PostingDate < startdate && c.FromLocation == JobNo).Sum(c => c.Quantity);
                        OpenningQTY = (sumReclass + sumIssue + sumRental + sumReturnCal) - (sumReturn + sumIssueExternal);
                        current.OpenningQTY = OpenningQTY;

                        sumReclass = 0;
                        sumIssue = 0;
                        sumRental = 0;
                        sumReturnCal = 0;
                        sumReturn = 0;
                        sumIssueExternal = 0;
                        sumReclass = JobLedgerNews.Where(c => c.JobNo == JobNo && c.JobLedgerEntry == std.JobTaskCut && c.TypeOfTask == 0 && c.PostingDate < startdate).Sum(c => c.TotalCost + c.LineAmount);
                        sumIssue = JobLedgerNews.Where(c => c.JobNo == JobNo && c.JobLedgerEntry == std.JobTaskCut && c.TypeOfTask == 3 && c.PostingDate < startdate).Sum(c => c.TotalCost + c.LineAmount);
                        sumRental = JobLedgerNews.Where(c => c.JobNo == JobNo && c.JobLedgerEntry == std.JobTaskCut && c.TypeOfTask == 2 && c.PostingDate < startdate).Sum(c => c.TotalCost + c.LineAmount);
                        sumReturnCal = JobLedgerNews.Where(c => c.JobNo == JobNo && c.JobLedgerEntry == std.JobTaskCut && c.TypeOfTask == 1 && c.Document3 == "CAL" && c.PostingDate < startdate).Sum(c => c.TotalCost + c.LineAmount) * 2;
                        sumReturn = JobLedgerNews.Where(c => c.JobNo == JobNo && c.JobLedgerEntry == std.JobTaskCut && c.TypeOfTask == 1 && c.PostingDate < startdate).Sum(c => c.TotalCost + c.LineAmount);
                        sumIssueExternal = JobLedgerNews.Where(c => c.JobNo != JobNo && c.JobLedgerEntry == std.JobTaskCut && c.TypeOfTask == 3 && c.FromLocation == JobNo && c.PostingDate < startdate).Sum(c => c.TotalCost + c.LineAmount);
                        sumReclassAPO = JobLedgerNews.Where(c => c.JobNo != JobNo && c.JobLedgerEntry == std.JobTaskCut && c.TypeOfTask == 0 && c.PostingDate < startdate && c.TotalCost == 0).Sum(c => c.Total);
                        OpenningAMT = (sumReclass + sumIssue + sumRental + sumReturnCal+sumReclassAPO) - (sumReturn + sumIssueExternal);
                        current.OpenningAMT = OpenningAMT;

                        sumReclass = 0;
                        sumIssue = 0;
                        sumRental = 0;
                        sumReturnCal = 0;
                        sumReturn = 0;
                        sumIssueExternal = 0;
                        sumReclass = JobLedgerNews.Where(c => c.JobNo == JobNo && c.JobLedgerEntry == std.JobTaskCut && c.TypeOfTask == 0 && c.PostingDate <= enddate).Sum(c => c.Quantity);
                        sumIssue = JobLedgerNews.Where(c => c.JobNo == JobNo && c.JobLedgerEntry == std.JobTaskCut && c.TypeOfTask == 3 && c.PostingDate <= enddate).Sum(c => c.Quantity);
                        sumRental = JobLedgerNews.Where(c => c.JobNo == JobNo && c.JobLedgerEntry == std.JobTaskCut && c.TypeOfTask == 2 && c.PostingDate <= enddate).Sum(c => c.Quantity);
                        sumReturnCal = JobLedgerNews.Where(c => c.JobNo == JobNo && c.JobLedgerEntry == std.JobTaskCut && c.TypeOfTask == 1 && c.Document3 == "CAL" && c.PostingDate <= enddate).Sum(c => c.Quantity) * 2;
                        sumReturn = JobLedgerNews.Where(c => c.JobNo == JobNo && c.JobLedgerEntry == std.JobTaskCut && c.TypeOfTask == 1 && c.PostingDate <= enddate).Sum(c => c.Quantity);
                        sumIssueExternal = JobLedgerNews.Where(c => c.JobNo != JobNo && c.JobLedgerEntry == std.JobTaskCut && c.TypeOfTask == 3 && c.PostingDate <= enddate && c.FromLocation == JobNo).Sum(c => c.Quantity);
                        CumBalQTY = (sumReclass + sumIssue + sumRental + sumReturnCal) - (sumReturn + sumIssueExternal);
                        current.CumBalQTY = CumBalQTY;

                        sumReclass = 0;
                        sumIssue = 0;
                        sumRental = 0;
                        sumReturnCal = 0;
                        sumReturn = 0;
                        sumIssueExternal = 0;
                        sumReclass = JobLedgerNews.Where(c => c.JobNo == JobNo && c.JobLedgerEntry == std.JobTaskCut && c.TypeOfTask == 0 && c.PostingDate <= enddate).Sum(c => c.TotalCost + c.LineAmount);
                        sumIssue = JobLedgerNews.Where(c => c.JobNo == JobNo && c.JobLedgerEntry == std.JobTaskCut && c.TypeOfTask == 3 && c.PostingDate <= enddate).Sum(c => c.TotalCost + c.LineAmount);
                        sumRental = JobLedgerNews.Where(c => c.JobNo == JobNo && c.JobLedgerEntry == std.JobTaskCut && c.TypeOfTask == 2 && c.PostingDate <= enddate).Sum(c => c.TotalCost + c.LineAmount);
                        sumReturnCal = JobLedgerNews.Where(c => c.JobNo == JobNo && c.JobLedgerEntry == std.JobTaskCut && c.TypeOfTask == 1 && c.Document3 == "CAL" && c.PostingDate <= enddate).Sum(c => c.TotalCost + c.LineAmount) * 2;
                        sumReturn = JobLedgerNews.Where(c => c.JobNo == JobNo && c.JobLedgerEntry == std.JobTaskCut && c.TypeOfTask == 1 && c.PostingDate <= enddate).Sum(c => c.TotalCost + c.LineAmount);
                        sumIssueExternal = JobLedgerNews.Where(c => c.JobNo != JobNo && c.JobLedgerEntry == std.JobTaskCut && c.TypeOfTask == 3 && c.FromLocation == JobNo && c.PostingDate <= enddate).Sum(c => c.TotalCost + c.LineAmount);
                        sumReclassAPO = JobLedgerNews.Where(c => c.JobNo != JobNo && c.JobLedgerEntry == std.JobTaskCut && c.TypeOfTask == 0 && c.PostingDate <= enddate && c.TotalCost == 0).Sum(c => c.Total);
                        CumBalAMT = (sumReclass + sumIssue + sumRental + sumReturnCal+sumReclassAPO) - (sumReturn + sumIssueExternal);
                        current.CumBalAMT = CumBalAMT;



                        BudgetQTY = jobPlanningLines.Where(c => c.JobNo == JobNo && c.JobTaskNo== std.JobTaskCut).Sum(c => c.Quantity);
                        BudgetAMT = jobPlanningLines.Where(c => c.JobNo == JobNo && c.JobTaskNo == std.JobTaskCut).Sum(c => c.TotalCost);
                        if (BudgetQTY == 0)
                        {
                            PercenQTY = 0;
                        }
                        else
                        {
                            PercenQTY = (CumBalQTY / BudgetQTY) * 100;
                        }

                        if (BudgetAMT == 0)
                        {
                            PercenAMT = 0;
                        }
                        else
                        {
                            PercenAMT = (CumBalAMT / BudgetAMT) * 100;
                        }

                        current.BudgetQTY = BudgetQTY;
                        current.BudgetAMT = BudgetAMT;
                        current.PercenAMT = PercenAMT;
                        current.PercenQTY = PercenQTY;



                    }
                    else
                    {
                        sumReclass = 0;
                        sumIssue = 0;
                        sumRental = 0;
                        sumReturnCal = 0;
                        sumReturn = 0;
                        sumIssueExternal = 0;
                        sumReclass = JobLedgerNews.Where(c => c.JobNo == JobNo && c.JobLedgerEntry == std.JobTaskCut && c.TypeOfTask == 0 && c.PostingDate >= startdate && c.PostingDate <= enddate).Sum(c => c.Quantity);
                        sumIssue = JobLedgerNews.Where(c => c.JobNo == JobNo && c.JobLedgerEntry == std.JobTaskCut && c.TypeOfTask == 3 && c.PostingDate >= startdate && c.PostingDate <= enddate).Sum(c => c.Quantity);
                        sumRental = JobLedgerNews.Where(c => c.JobNo == JobNo && c.JobLedgerEntry == std.JobTaskCut && c.TypeOfTask == 2 && c.PostingDate >= startdate && c.PostingDate <= enddate).Sum(c => c.Quantity);
                        sumReturnCal = JobLedgerNews.Where(c => c.JobNo == JobNo && c.JobLedgerEntry == std.JobTaskCut && c.TypeOfTask == 1 && c.Document3 == "CAL" && c.PostingDate >= startdate && c.PostingDate <= enddate).Sum(c => c.Quantity) * 2;
                        sumReturn = JobLedgerNews.Where(c => c.JobNo == JobNo && c.JobLedgerEntry == std.JobTaskCut && c.TypeOfTask == 1 && c.PostingDate >= startdate && c.PostingDate <= enddate).Sum(c => c.Quantity);
                        sumIssueExternal = JobLedgerNews.Where(c => c.JobNo != JobNo && c.JobLedgerEntry == std.JobTaskCut && c.TypeOfTask == 3 && c.PostingDate >= startdate && c.FromLocation == JobNo && c.PostingDate <= enddate).Sum(c => c.Quantity);
                        ThisPeriodQty = (sumReclass + sumIssue + sumRental + sumReturnCal) - (sumReturn + sumIssueExternal);
                        current.ThisPeriodQty = ThisPeriodQty;

                        sumReclass = 0;
                        sumIssue = 0;
                        sumRental = 0;
                        sumReturnCal = 0;
                        sumReturn = 0;
                        sumIssueExternal = 0;
                        sumReclass = JobLedgerNews.Where(c => c.JobNo == JobNo && c.JobLedgerEntry == std.JobTaskCut && c.TypeOfTask == 0 && c.PostingDate >= startdate && c.PostingDate <= enddate).Sum(c => c.TotalCost);
                        sumIssue = JobLedgerNews.Where(c => c.JobNo == JobNo && c.JobLedgerEntry == std.JobTaskCut && c.TypeOfTask == 3 && c.PostingDate >= startdate && c.PostingDate <= enddate).Sum(c => c.TotalCost);
                        sumRental = JobLedgerNews.Where(c => c.JobNo == JobNo && c.JobLedgerEntry == std.JobTaskCut && c.TypeOfTask == 2 && c.PostingDate >= startdate && c.PostingDate <= enddate).Sum(c => c.TotalCost);
                        sumReturnCal = JobLedgerNews.Where(c => c.JobNo == JobNo && c.JobLedgerEntry == std.JobTaskCut && c.TypeOfTask == 1 && c.Document3 == "CAL" && c.PostingDate >= startdate && c.PostingDate <= enddate).Sum(c => c.TotalCost) * 2;
                        sumReturn = JobLedgerNews.Where(c => c.JobNo == JobNo && c.JobLedgerEntry == std.JobTaskCut && c.TypeOfTask == 1 && c.PostingDate >= startdate && c.PostingDate <= enddate).Sum(c => c.TotalCost);
                        sumIssueExternal = JobLedgerNews.Where(c => c.JobNo != JobNo && c.JobLedgerEntry == std.JobTaskCut && c.TypeOfTask == 3 && c.FromLocation == JobNo && c.PostingDate >= startdate && c.PostingDate <= enddate).Sum(c => c.TotalCost);
                        sumReclassAPO = JobLedgerNews.Where(c => c.JobNo != JobNo && c.JobLedgerEntry == std.JobTaskCut && c.TypeOfTask == 0 && c.PostingDate >= startdate && c.PostingDate <= enddate && c.TotalCost == 0).Sum(c => c.Total);
                        ThisPeriodAMT = (sumReclass + sumIssue + sumRental + sumReturnCal+sumReclassAPO) - (sumReturn + sumIssueExternal);
                        current.ThisPeriodAMT = ThisPeriodAMT;

                        sumReclass = 0;
                        sumIssue = 0;
                        sumRental = 0;
                        sumReturnCal = 0;
                        sumReturn = 0;
                        sumIssueExternal = 0;
                        sumReclass = JobLedgerNews.Where(c => c.JobNo == JobNo && c.JobLedgerEntry == std.JobTaskCut && c.TypeOfTask == 0 && c.PostingDate < startdate).Sum(c => c.Quantity);
                        sumIssue = JobLedgerNews.Where(c => c.JobNo == JobNo && c.JobLedgerEntry == std.JobTaskCut && c.TypeOfTask == 3 && c.PostingDate < startdate).Sum(c => c.Quantity);
                        sumRental = JobLedgerNews.Where(c => c.JobNo == JobNo && c.JobLedgerEntry == std.JobTaskCut && c.TypeOfTask == 2 && c.PostingDate < startdate).Sum(c => c.Quantity);
                        sumReturnCal = JobLedgerNews.Where(c => c.JobNo == JobNo && c.JobLedgerEntry == std.JobTaskCut && c.TypeOfTask == 1 && c.Document3 == "CAL" && c.PostingDate < startdate).Sum(c => c.Quantity) * 2;
                        sumReturn = JobLedgerNews.Where(c => c.JobNo == JobNo && c.JobLedgerEntry == std.JobTaskCut && c.TypeOfTask == 1 && c.PostingDate < startdate).Sum(c => c.Quantity);
                        sumIssueExternal = JobLedgerNews.Where(c => c.JobNo != JobNo && c.JobLedgerEntry == std.JobTaskCut && c.TypeOfTask == 3 && c.PostingDate < startdate && c.FromLocation == JobNo).Sum(c => c.Quantity);
                        OpenningQTY = (sumReclass + sumIssue + sumRental + sumReturnCal) - (sumReturn + sumIssueExternal);
                        current.OpenningQTY = OpenningQTY;


                        sumReclass = 0;
                        sumIssue = 0;
                        sumRental = 0;
                        sumReturnCal = 0;
                        sumReturn = 0;
                        sumIssueExternal = 0;
                        sumReclass = JobLedgerNews.Where(c => c.JobNo == JobNo && c.JobLedgerEntry == std.JobTaskCut && c.TypeOfTask == 0 && c.PostingDate < startdate).Sum(c => c.TotalCost);
                        sumIssue = JobLedgerNews.Where(c => c.JobNo == JobNo && c.JobLedgerEntry == std.JobTaskCut && c.TypeOfTask == 3 && c.PostingDate < startdate).Sum(c => c.TotalCost);
                        sumRental = JobLedgerNews.Where(c => c.JobNo == JobNo && c.JobLedgerEntry == std.JobTaskCut && c.TypeOfTask == 2 && c.PostingDate < startdate).Sum(c => c.TotalCost);
                        sumReturnCal = JobLedgerNews.Where(c => c.JobNo == JobNo && c.JobLedgerEntry == std.JobTaskCut && c.TypeOfTask == 1 && c.Document3 == "CAL" && c.PostingDate < startdate).Sum(c => c.TotalCost) * 2;
                        sumReturn = JobLedgerNews.Where(c => c.JobNo == JobNo && c.JobLedgerEntry == std.JobTaskCut && c.TypeOfTask == 1 && c.PostingDate < startdate ).Sum(c => c.TotalCost);
                        sumIssueExternal = JobLedgerNews.Where(c => c.JobNo != JobNo && c.JobLedgerEntry == std.JobTaskCut && c.TypeOfTask == 3 && c.FromLocation == JobNo && c.PostingDate < startdate).Sum(c => c.TotalCost);
                        sumReclassAPO = JobLedgerNews.Where(c => c.JobNo != JobNo && c.JobLedgerEntry == std.JobTaskCut && c.TypeOfTask == 0 && c.PostingDate < startdate && c.TotalCost == 0).Sum(c => c.Total);
                        OpenningAMT = (sumReclass + sumIssue + sumRental + sumReturnCal+sumReclassAPO) - (sumReturn + sumIssueExternal);
                        current.OpenningAMT = OpenningAMT;

                        sumReclass = 0;
                        sumIssue = 0;
                        sumRental = 0;
                        sumReturnCal = 0;
                        sumReturn = 0;
                        sumIssueExternal = 0;
                        sumReclass = JobLedgerNews.Where(c => c.JobNo == JobNo && c.JobLedgerEntry == std.JobTaskCut && c.TypeOfTask == 0 && c.PostingDate <= enddate).Sum(c => c.Quantity);
                        sumIssue = JobLedgerNews.Where(c => c.JobNo == JobNo && c.JobLedgerEntry == std.JobTaskCut && c.TypeOfTask == 3 && c.PostingDate <= enddate).Sum(c => c.Quantity);
                        sumRental = JobLedgerNews.Where(c => c.JobNo == JobNo && c.JobLedgerEntry == std.JobTaskCut && c.TypeOfTask == 2 && c.PostingDate <= enddate).Sum(c => c.Quantity);
                        sumReturnCal = JobLedgerNews.Where(c => c.JobNo == JobNo && c.JobLedgerEntry == std.JobTaskCut && c.TypeOfTask == 1 && c.Document3 == "CAL" && c.PostingDate <= enddate).Sum(c => c.Quantity) * 2;
                        sumReturn = JobLedgerNews.Where(c => c.JobNo == JobNo && c.JobLedgerEntry == std.JobTaskCut && c.TypeOfTask == 1 && c.PostingDate <= enddate).Sum(c => c.Quantity);
                        sumIssueExternal = JobLedgerNews.Where(c => c.JobNo != JobNo && c.JobLedgerEntry == std.JobTaskCut && c.TypeOfTask == 3 && c.PostingDate <= enddate && c.FromLocation == JobNo).Sum(c => c.Quantity);
                        CumBalQTY = (sumReclass + sumIssue + sumRental + sumReturnCal) - (sumReturn + sumIssueExternal);
                        current.CumBalQTY = CumBalQTY;

                        sumReclass = 0;
                        sumIssue = 0;
                        sumRental = 0;
                        sumReturnCal = 0;
                        sumReturn = 0;
                        sumIssueExternal = 0;
                        sumReclass = JobLedgerNews.Where(c => c.JobNo == JobNo && c.JobLedgerEntry == std.JobTaskCut && c.TypeOfTask == 0 && c.PostingDate <= enddate).Sum(c => c.TotalCost);
                        sumIssue = JobLedgerNews.Where(c => c.JobNo == JobNo && c.JobLedgerEntry == std.JobTaskCut && c.TypeOfTask == 3 && c.PostingDate <= enddate).Sum(c => c.TotalCost);
                        sumRental = JobLedgerNews.Where(c => c.JobNo == JobNo && c.JobLedgerEntry == std.JobTaskCut && c.TypeOfTask == 2 && c.PostingDate <= enddate).Sum(c => c.TotalCost);
                        sumReturnCal = JobLedgerNews.Where(c => c.JobNo == JobNo && c.JobLedgerEntry == std.JobTaskCut && c.TypeOfTask == 1 && c.Document3 == "CAL" && c.PostingDate <= enddate).Sum(c => c.TotalCost) * 2;
                        sumReturn = JobLedgerNews.Where(c => c.JobNo == JobNo && c.JobLedgerEntry == std.JobTaskCut && c.TypeOfTask == 1 && c.PostingDate <= enddate).Sum(c => c.TotalCost);
                        sumIssueExternal = JobLedgerNews.Where(c => c.JobNo != JobNo && c.JobLedgerEntry == std.JobTaskCut && c.TypeOfTask == 3 && c.FromLocation == JobNo && c.PostingDate <= enddate).Sum(c => c.TotalCost);
                        sumReclassAPO = JobLedgerNews.Where(c => c.JobNo != JobNo && c.JobLedgerEntry == std.JobTaskCut && c.TypeOfTask == 0 && c.PostingDate <= enddate && c.TotalCost == 0).Sum(c => c.Total);
                        CumBalAMT = (sumReclass + sumIssue + sumRental + sumReturnCal+sumReclassAPO) - (sumReturn + sumIssueExternal);
                        current.CumBalAMT = CumBalAMT;

                        BudgetQTY = jobPlanningLines.Where(c => c.JobNo == JobNo && c.JobTaskNo == std.JobTaskCut).Sum(c => c.Quantity);
                        BudgetAMT = jobPlanningLines.Where(c => c.JobNo == JobNo && c.JobTaskNo == std.JobTaskCut).Sum(c => c.TotalCost);
                        if (BudgetQTY == 0)
                        {
                            PercenQTY = 0;
                        }
                        else
                        {
                            PercenQTY = (CumBalQTY / BudgetQTY) * 100;
                        }

                        if (BudgetAMT == 0)
                        {
                            PercenAMT = 0;
                        }
                        else
                        {
                            PercenAMT = (CumBalAMT / BudgetAMT) * 100;
                        }

                        current.BudgetQTY = BudgetQTY;
                        current.BudgetAMT = BudgetAMT;
                        current.PercenAMT = PercenAMT;
                        current.PercenQTY = PercenQTY;
                    }


                    if (std.JobTaskNo == "G0210")
                    {
                        G0210 = CumBalAMT;
                        ViewBag.G0210N = SetFontRed(CumBalAMT * -1, 1);
                        ViewBag.G0210P = SetFontRed(CumBalAMT, 1);
                    }
                    if (std.JobTaskNo == "C0110")
                    {
                        C0110 = CumBalAMT;
                        ViewBag.C0110 = SetFontRed(CumBalAMT, 1);
                    }
                    if (std.JobTaskNo == "C0120")
                    {
                        C0120 = CumBalAMT;
                        ViewBag.C0120 = SetFontRed(CumBalAMT, 1);
                    }

                    if (std.JobTaskNo == "C0130")
                    {
                        C0130 = CumBalAMT;
                        ViewBag.C0130 = SetFontRed(CumBalAMT, 1);
                    }

                    if (std.JobTaskNo == "C0410")
                    {
                        C0410 = CumBalAMT;
                        ViewBag.C0410 = SetFontRed(CumBalAMT, 1);
                    }

                    if (std.JobTaskNo == "C0510")
                    {
                        C0510 = CumBalAMT;
                        ViewBag.C0510 = SetFontRed(CumBalAMT, 1);
                    }
                    if (std.JobTaskNo == "C0910")
                    {
                        C0910 = CumBalAMT;
                        ViewBag.C0910 = SetFontRed(CumBalAMT, 1);
                    }
                    if (std.JobTaskNo == "C1010")
                    {
                        C1010 = CumBalAMT;
                        ViewBag.C1010 = SetFontRed(CumBalAMT, 1);
                    }
                    if (std.JobTaskNo == "C1110")
                    {
                        C1110 = CumBalAMT;
                        ViewBag.C1110 = SetFontRed(CumBalAMT, 1);
                    }
                    if (std.JobTaskNo == "D0450")
                    {
                        D0450 = CumBalAMT;
                        ViewBag.D0450 = SetFontRed(CumBalAMT, 1);
                    }
                    if (std.JobTaskNo == "H0110")
                    {
                        H0110 = CumBalAMT;
                        ViewBag.H0110 = SetFontRed(CumBalAMT, 1);
                    }
                    if (std.JobTaskNo == "H0130")
                    {
                        H0130 = CumBalAMT;
                        ViewBag.H0130 = SetFontRed(CumBalAMT, 1);
                    }


                    if (std.JobTaskNo == "E0910")
                    {
                        E0910 = CumBalAMT;
                        ViewBag.E0910 = SetFontRed(CumBalAMT, 1);
                    }
                    if (std.JobTaskNo == "E0920")
                    {
                        E0920 = CumBalAMT;
                        ViewBag.E0920 = SetFontRed(CumBalAMT, 1);
                    }
                    if (std.JobTaskNo == "E0930")
                    {
                        E0930 = CumBalAMT;
                        ViewBag.E0930 = SetFontRed(CumBalAMT, 1);
                    }
                    if (std.JobTaskNo == "E0940")
                    {
                        E0940 = CumBalAMT;
                        ViewBag.E0940 = SetFontRed(CumBalAMT, 1);
                    }

                    if (std.JobTaskNo == "F5999")
                    {
                        F5999 = CumBalAMT;
                        ViewBag.F5999 = SetFontRed(CumBalAMT, 1);
                    }
                }



                //ให้แสดงเฉพาะ Row ไม่เท่ากับ 0

                if (current.ThisPeriodAMT != 0)
                {
                    checkzero = 1;
                }
                if (current.ThisPeriodQty != 0)
                {
                    checkzero = 1;
                }
                if (current.OpenningAMT != 0)
                {
                    checkzero = 1;
                }
                if (current.OpenningQTY != 0)
                {
                    checkzero = 1;
                }
                if (current.CumBalAMT != 0)
                {
                    checkzero = 1;
                }
                if (current.CumBalQTY != 0)
                {
                    checkzero = 1;
                }
                if (current.BudgetQTY != 0)
                {
                    checkzero = 1;
                }
                if (current.BudgetAMT!= 0)
                {
                    checkzero = 1;
                }


                if (checkzero == 1) {
                 instances.Add(current);
                }
                checkzero = 0;

                //----
            }


     

            TIC += C0110 + C0120 + C0130 + C0410 + C0510 + C0910 + C1010 + C1110 + D0450 + H0110 + H0130 + H0200 + H0800;
            ViewBag.TIC = SetFontRed(TIC, 1);


            ViewBag.A0000 = A00;
            ViewBag.B0000 = B00;
            ViewBag.C0000 = C00;
            ViewBag.D0000 = D00;
            ViewBag.E0000 = E00;
            ViewBag.F0000 = F00;
            ViewBag.G0000 = G00;
            ViewBag.H0000 = H00;


            TCXI = (A00 + B00 + C00 + D00 + E00 + F00 +G00+ H00)+ (G0210 * -1);

            ViewBag.TCXI = SetFontRed(TCXI, 1);  ///TCXI-TIC

            ViewBag.N00 = SetFontRed(N00, 1);
            ViewBag.O00 = SetFontRed(O00, 1);
            ViewBag.INCOME = SetFontRed((N00 + O00), 1);
            ViewBag.INCOME1 = SetFontRed((N00 + O00) * -1, 1);
            ViewBag.ExternalCost = SetFontRed(((TCXI + (G0210 * -1)) - TIC), 1);
            ViewBag.GrossProfit = SetFontRed(((N00 + O00) * -1) - ((TCXI + (G0210 * -1)) - TIC), 1);

            ViewBag.NetProfit = SetFontRed(((((N00 + O00) * -1) - ((TCXI + (G0210 * -1)) - TIC)) - TIC), 1);
            ViewBag.PerGross = SetFontRed(Percen(((((N00 + O00) * -1) - ((TCXI + (G0210 * -1)) - TIC))), (N00 + O00) * -1), 1) + "<b>%</b>";
            ViewBag.PerNet = SetFontRed(Percen(((((N00 + O00) * -1) - ((TCXI + (G0210 * -1)) - TIC)) - TIC), (N00 + O00) * -1), 1) + "<b>%</b>";



            response = Ok(new { data= instances });
            var data = new { data = instances };
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            ViewData["JsonRegionList"] = instances;
            ViewData["JsonTotalList"] = SubTotal;



            return View();
        }


        // GET: Levels/Details/5
        public IActionResult Add([Bind("EstimateJob,EstimateStart,EstimateEnd,Contractwork,ExtraWork,OtherIncome,AECT,UECT,EC,AICT,NetProfit,defalse,a1,a2, a3, a4, a5,  a6,  a7,  a8,  a9,  a10, a11, a12,  a13, a14, a15, a16, a17, b1, b2, b3, b4, c1, c2, c3, c4, c5,  c6,  c7,  c8,  c9,  c10,  c11,  c12,  c13,  c14,  c15,  c16,  c17,  c18,  c19,  c20,  c21,  c22,  c23,  c24,  c25,  c26,  c27,  c28,  c29,  c30,  c31,  c32,  c33,  c34,  c35,  c36,  c37")] Estimate estimate)
        {

            estimate.EstimateBy = HttpContext.Session.GetString("Username");
            estimate.DateCreate = DateTime.Now;

            _context.Estimates.Add(estimate);
            _context.SaveChanges();

            //ViewBag.EstmatJob = estimate.EstimateJob;


            return Ok(estimate);
        }



        public IActionResult History(string job)
        {
            IActionResult response = Unauthorized();
            var etimates = _context.Estimates
            .Where(s => s.EstimateJob == job).OrderByDescending(s => s.EstimateId)
            .ToList();
            string link = "";

            var i = etimates.Count;
            foreach (var es in etimates as IList<Estimate>) //เช็คค่าตาม Location
            {
                link += "<button class='btn btn-info' data-animal-type='" + es.EstimateId + "' onclick='showdatahis(this);'>ไฟล์ " + i + "_" + es.DateCreate.ToString("yyyyMMdd") + "</button>";
                i--;
            }
            response = Ok(new { link = link });
            return response;
        }


        public IActionResult ShowHistory(int id)
        {
            IActionResult response = Unauthorized();
            var etimates = _context.Estimates
            .Where(s => s.EstimateId == id)
            .ToList();
            foreach (var es in etimates as IList<Estimate>) //เช็คค่าตาม Location
            {
                response = Ok(new
                {
                    estimateJob = es.EstimateJob,
                    estimateStart = es.EstimateStart,
                    estimateEnd = es.EstimateEnd,
                    estimateBy = es.EstimateBy,
                    defalse = es.defalse,
                    dateCreate = es.DateCreate.ToString("dd/MM/yyyy"),
                    a1 = es.a1,
                    a2 = es.a2,
                    a3 = es.a3,
                    a4 = es.a4,
                    a5 = es.a5,
                    a6 = es.a6,
                    a7 = es.a7,
                    a8 = es.a8,
                    a9 = es.a9,
                    a10 = es.a10,
                    a11 = es.a11,
                    a12 = es.a12,
                    a13 = es.a13,
                    a14 = es.a14,
                    a15 = es.a15,
                    a16 = es.a16,
                    a17 = es.a17,
                    b1 = es.b1,
                    b2 = es.b2,
                    b3 = es.b3,
                    b4 = es.b4,
                    c1 = es.c1,
                    c2 = es.c2,
                    c3 = es.c3,
                    c4 = es.c4,
                    c5 = es.c5,
                    c6 = es.c6,
                    c7 = es.c7,
                    c8 = es.c8,
                    c9 = es.c9,
                    c10 = es.c10,
                    c11 = es.c11,
                    c12 = es.c12,
                    c13 = es.c13,
                    c14 = es.c14,
                    c15 = es.c15,
                    c16 = es.c16,
                    c17 = es.c17,
                    c18 = es.c18,
                    c19 = es.c19,
                    c20 = es.c20,
                    c21 = es.c21,
                    c22 = es.c22,
                    c23 = es.c23,
                    c24 = es.c24,
                    c25 = es.c25,
                    c26 = es.c26,
                    c27 = es.c27,
                    c28 = es.c28,
                    c29 = es.c29,
                    c30 = es.c30,
                    c31 = es.c31,
                    c32 = es.c32,
                    c33 = es.c33,
                    c34 = es.c34,
                    c35 = es.c35,
                    c36 = es.c36,
                    c37 = es.c37
                });
            }
            return response;
        }



        /// <summary>
        /// /Home/FileReport
        /// </summary>
        public IActionResult FileReport([Bind("EstimateJob,EstimateStart,EstimateEnd,Contractwork,ExtraWork,OtherIncome,AECT,UECT,EC,AICT,NetProfit,defalse,a1,a2, a3, a4, a5,  a6,  a7,  a8,  a9,  a10, a11, a12,  a13, a14, a15, a16, a17, b1, b2, b3, b4, c1, c2, c3, c4, c5,  c6,  c7,  c8,  c9,  c10,  c11,  c12,  c13,  c14,  c15,  c16,  c17,  c18,  c19,  c20,  c21,  c22,  c23,  c24,  c25,  c26,  c27,  c28,  c29,  c30,  c31,  c32,  c33,  c34,  c35,  c36,  c37")] Estimate estimate)
        {
            IActionResult response = Unauthorized();
            var fileDownloadName = "report.xlsx";
            var reportsFolder = "reports";
            using (var package = createExcelPackage(estimate))
            {
                package.SaveAs(new FileInfo(Path.Combine(_hostingEnvironment.WebRootPath, reportsFolder, fileDownloadName)));

            }
            response = Ok(new { reportsFolder = $"~/{reportsFolder}/{fileDownloadName}",filename= fileDownloadName });
            return response;

            //return File($"~/{reportsFolder}/{fileDownloadName}", XlsxContentType, fileDownloadName);
        }


        [HttpGet]
        public virtual ActionResult Download(string file)
        {
            var fileDownloadName = "report.xlsx";
            var reportsFolder = "reports";
            string fullPath = Path.Combine(_hostingEnvironment.WebRootPath, reportsFolder, file);
            return File($"~/{reportsFolder}/{fileDownloadName}", XlsxContentType, fileDownloadName);
        }

        private ExcelPackage createExcelPackage([Bind("EstimateJob,EstimateStart,EstimateEnd,Contractwork,ExtraWork,OtherIncome,AECT,UECT,EC,AICT,NetProfit,defalse,a1,a2, a3, a4, a5,  a6,  a7,  a8,  a9,  a10, a11, a12,  a13, a14, a15, a16, a17, b1, b2, b3, b4, c1, c2, c3, c4, c5,  c6,  c7,  c8,  c9,  c10,  c11,  c12,  c13,  c14,  c15,  c16,  c17,  c18,  c19,  c20,  c21,  c22,  c23,  c24,  c25,  c26,  c27,  c28,  c29,  c30,  c31,  c32,  c33,  c34,  c35,  c36,  c37")] Estimate estimate)
        {
            var package = new ExcelPackage();
            package.Workbook.Properties.Title = "Salary Report";
            package.Workbook.Properties.Author = "Vahid N.";
            package.Workbook.Properties.Subject = "Salary Report";
            package.Workbook.Properties.Keywords = "Salary";


            var worksheet = package.Workbook.Worksheets.Add("JobCost");
            worksheet.Cells[1, 1].Value = "Job : " + estimate.EstimateJob + " ช่วงเวลา : " + estimate.EstimateStart + " - " + estimate.EstimateEnd;
            worksheet.Cells["A1:E1"].Merge = true;
            worksheet.Cells["A1"].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
            //First add the headers
            worksheet.Cells[2, 1].Value = "Accounting Job Cost";
            worksheet.Cells[2, 2].Value = "";
            worksheet.Cells[2, 3].Value = "";
            worksheet.Cells[2, 4].Value = "Income & Cost";
            worksheet.Cells[2, 5].Value = "";

            //Add values

            var numberformat = "#,##0";
            var dataCellStyleName = "TableNumber";
            var numStyle = package.Workbook.Styles.CreateNamedStyle(dataCellStyleName);
            numStyle.Style.Numberformat.Format = numberformat;



            worksheet.Cells[3, 1].Value = "Income";
            worksheet.Cells[3, 2].Value = decimal.Parse(estimate.a1);
            worksheet.Cells[3, 3].Value = "";
            worksheet.Cells[3, 4].Value = "-N0000 Work Done";
            worksheet.Cells[3, 5].Value = decimal.Parse(estimate.c1);


            worksheet.Cells[4, 1].Value = "External Cost";
            worksheet.Cells[4, 2].Value = decimal.Parse(estimate.a2);
            worksheet.Cells[4, 2].Style.Border.Bottom.Style = ExcelBorderStyle.Thick;
            worksheet.Cells[4, 3].Value = "";
            worksheet.Cells[4, 4].Value = "-O0000 Other Income";
            worksheet.Cells[4, 5].Value = decimal.Parse(estimate.c2);
            worksheet.Cells[4, 5].Style.Border.Bottom.Style = ExcelBorderStyle.Thick;


            worksheet.Cells[5, 1].Value = "Gross Profit / Gross Margin";
            worksheet.Cells[5, 2].Formula = "=B3-B4";
            worksheet.Cells[5, 3].Formula = "=IFERROR((B5/B3)*100,0)";
            worksheet.Cells[5, 4].Value = "           Income";
            worksheet.Cells[5, 5].Value = decimal.Parse(estimate.c3);
            worksheet.Cells[5, 5].Style.Border.Bottom.Style = ExcelBorderStyle.Double;



            worksheet.Cells[6, 1].Value = "Total Internal";
            worksheet.Cells[6, 2].Value = decimal.Parse(estimate.a4);
            worksheet.Cells[6, 2].Style.Border.Bottom.Style = ExcelBorderStyle.Thick;
            worksheet.Cells[6, 3].Value = "";
            worksheet.Cells[6, 3].Value = "";
            worksheet.Cells[6, 4].Value = "-A0000 หมวดเตรียมการ";
            worksheet.Cells[6, 5].Value = decimal.Parse(estimate.c4);



            worksheet.Cells[7, 1].Value = "Net Profit / Net Margin";
            worksheet.Cells[7, 2].Formula = "=B5-B6";
            worksheet.Cells[7, 2].Style.Border.Bottom.Style = ExcelBorderStyle.Double;
            worksheet.Cells[7, 3].Formula = "=IFERROR((B7/B3)*100,0)";
            worksheet.Cells[7, 4].Value = "-B0000 หมวมดค่าใช้จ่าย";
            worksheet.Cells[7, 5].Value = decimal.Parse(estimate.c5);

            worksheet.Cells[8, 1].Value = "";
            worksheet.Cells[8, 2].Value = "";
            worksheet.Cells[8, 3].Value = "";
            worksheet.Cells[8, 4].Value = "-C0000 หมวดค่าเช่า";
            worksheet.Cells[8, 5].Value = decimal.Parse(estimate.c6);

            worksheet.Cells[9, 1].Value = "";
            worksheet.Cells[9, 2].Value = "";
            worksheet.Cells[9, 3].Value = "";
            worksheet.Cells[9, 4].Value = "-D0000 หมวดค่าวัสดุ";
            worksheet.Cells[9, 5].Value = decimal.Parse(estimate.c7);


            worksheet.Cells[10, 1].Value = "ประมาณการ";
            worksheet.Cells[10, 2].Value = "";
            worksheet.Cells[10, 3].Value = "";
            worksheet.Cells[10, 4].Value = "-E0000 หมวดค่าแรงจ้างเหมา";
            worksheet.Cells[10, 5].Value = decimal.Parse(estimate.c8);

            //B
            worksheet.Cells[11, 1].Value = "Contact Work";
            worksheet.Cells[11, 2].Value = decimal.Parse(estimate.a6);
            worksheet.Cells[11, 3].Value = "";
            worksheet.Cells[11, 4].Value = "-F0000 หมวดค่าจ้างเหมาช่วง";
            worksheet.Cells[11, 5].Value = decimal.Parse(estimate.c9);

            worksheet.Cells[12, 1].Value = "";
            worksheet.Cells[12, 2].Value = "";
            worksheet.Cells[12, 3].Value = "";
            worksheet.Cells[12, 4].Value = "-G0000 หมวดค่าใช้จ่ายอื่นๆ";
            worksheet.Cells[12, 5].Value = decimal.Parse(estimate.c10);

            //B
            worksheet.Cells[13, 1].Value = "Extra Work";
            worksheet.Cells[13, 2].Value = decimal.Parse(estimate.a7);
            worksheet.Cells[13, 3].Value = "";
            worksheet.Cells[13, 4].Value = "-G0210 ภาษีหัก ณ ที่จ่าย";
            worksheet.Cells[13, 5].Value = decimal.Parse(estimate.c11);

            worksheet.Cells[14, 1].Value = "";
            worksheet.Cells[14, 2].Value = "";
            worksheet.Cells[14, 3].Value = "";
            worksheet.Cells[14, 4].Value = "-H0000 หมวดค่าใช่จ่ายส่วนกลาง";
            worksheet.Cells[14, 5].Value = decimal.Parse(estimate.c12);
            worksheet.Cells[14, 5].Style.Border.Bottom.Style = ExcelBorderStyle.Thick;

            //B
            worksheet.Cells[15, 1].Value = "Orther Income";
            worksheet.Cells[15, 2].Value = decimal.Parse(estimate.a8);
            worksheet.Cells[15, 2].Style.Border.Bottom.Style = ExcelBorderStyle.Thick;
            worksheet.Cells[15, 3].Value = "";
            worksheet.Cells[15, 4].Value = "Total cost (external + Internal)(1)";
            worksheet.Cells[15, 5].Value = decimal.Parse(estimate.c13);
            worksheet.Cells[15, 5].Style.Border.Bottom.Style = ExcelBorderStyle.Double;

            worksheet.Cells[16, 1].Value = "TOTAL INCOME";
            worksheet.Cells[16, 2].Formula = "=B11+B13+B15";
            worksheet.Cells[16, 2].Style.Border.Bottom.Style = ExcelBorderStyle.Double;
            worksheet.Cells[16, 3].Value = "";
            worksheet.Cells[16, 4].Value = "Internal Cost";
            worksheet.Cells[16, 5].Value = "";

            worksheet.Cells[17, 1].Value = "Actual External Cost Todate";
            worksheet.Cells[17, 2].Value = decimal.Parse(estimate.a10);
            worksheet.Cells[17, 3].Value = "";
            worksheet.Cells[17, 4].Value = "-C0110 ค่าเช่าขนส่งภายใน";
            worksheet.Cells[17, 5].Value = decimal.Parse(estimate.c14);

            worksheet.Cells[18, 1].Value = "";
            worksheet.Cells[18, 2].Value = "";
            worksheet.Cells[18, 3].Value = "";
            worksheet.Cells[18, 4].Value = "-C0120 ค่าเช่าเครื่องจักรภายใน";
            worksheet.Cells[18, 5].Value = decimal.Parse(estimate.c15);

            worksheet.Cells[19, 1].Value = "Update External cost อื่นๆ";
            worksheet.Cells[19, 2].Value = decimal.Parse(estimate.a11);
            worksheet.Cells[19, 3].Value = "";
            worksheet.Cells[19, 4].Value = "-C0130 ค่าเช่าเครื่องใช้ส.น.ง.ภายใน";
            worksheet.Cells[19, 5].Value = decimal.Parse(estimate.c16);

            worksheet.Cells[20, 1].Value = "ประมาณการเพิ่มจนจบงาน (เฉพาะ External cost)";
            worksheet.Cells[20, 2].Style.Border.Bottom.Style = ExcelBorderStyle.Thick;
            worksheet.Cells[20, 2].Value = decimal.Parse(estimate.a12);
            worksheet.Cells[20, 3].Value = "";
            worksheet.Cells[20, 4].Value = "-C0410 ค่าเช่านั่งร้านภายใน";
            worksheet.Cells[20, 5].Value = decimal.Parse(estimate.c17);

            worksheet.Cells[21, 1].Value = "Total External Cost";
            worksheet.Cells[21, 2].Formula = "=B17+B19+B20";
            worksheet.Cells[21, 2].Style.Border.Bottom.Style = ExcelBorderStyle.Double;
            worksheet.Cells[21, 3].Value = "";
            worksheet.Cells[21, 4].Value = "-C0510 ค่าเช่าไฟล์ภายใน";
            worksheet.Cells[21, 5].Value = decimal.Parse(estimate.c18);

            worksheet.Cells[22, 1].Value = "Gross Profit / Gross Margin";
            worksheet.Cells[22, 2].Formula = "=(B11+B13+B15)-(B17+B19+B20)";
            worksheet.Cells[22, 3].Formula = "=IFERROR((((B11+B13+B15)-(B17+B19+B20))/(B11+B13+B15))*100,0)";
            worksheet.Cells[22, 4].Value = "-C910 ค่าเช่า H-Beam ภายใน";
            worksheet.Cells[22, 5].Value = decimal.Parse(estimate.c19);

            worksheet.Cells[23, 1].Value = "Actual internal Cost Todate";
            worksheet.Cells[23, 2].Value = decimal.Parse(estimate.a15);
            worksheet.Cells[23, 3].Value = "";
            worksheet.Cells[23, 4].Value = "-C1010 ค่าเช่าแผ่นเหล็กภายใน";
            worksheet.Cells[23, 5].Value = decimal.Parse(estimate.c20);

            worksheet.Cells[24, 1].Value = "ประมาณการ Internal";
            worksheet.Cells[24, 2].Value = decimal.Parse(estimate.a16);
            worksheet.Cells[24, 2].Style.Border.Bottom.Style = ExcelBorderStyle.Thick;
            worksheet.Cells[24, 3].Value = "";
            worksheet.Cells[24, 4].Value = "-C1110 ค่าเช่าเหล็กรูปพรรณภายใน";
            worksheet.Cells[24, 5].Value = decimal.Parse(estimate.c21);

            worksheet.Cells[25, 1].Value = "Net Profit / Net Margin";
            worksheet.Cells[25, 2].Formula = "=((B11+B13+B15)-(B17+B19+B20))-(B23+B24)";
            worksheet.Cells[25, 2].Style.Border.Bottom.Style = ExcelBorderStyle.Double;
            worksheet.Cells[25, 3].Formula = "=IFERROR(((((B11+B13+B15)-(B17+B19+B20))-(B23+B24))/(B11+B13+B15))*100,0)";
            worksheet.Cells[25, 4].Value = "-D0450 ค่าเช่าแบบเหล็กภายใน";
            worksheet.Cells[25, 5].Value = decimal.Parse(estimate.c22);

            worksheet.Cells[26, 1].Value = "";
            worksheet.Cells[26, 2].Value = "";
            worksheet.Cells[26, 3].Value = "";
            worksheet.Cells[26, 4].Value = "-H0110 เงินเดือน";
            worksheet.Cells[26, 5].Value = decimal.Parse(estimate.c23);

            worksheet.Cells[27, 1].Value = "";
            worksheet.Cells[27, 2].Value = "";
            worksheet.Cells[27, 3].Value = "";
            worksheet.Cells[27, 4].Value = "-H0130 เบี้ยเลี้ยงพนักงาน";
            worksheet.Cells[27, 5].Value = decimal.Parse(estimate.c24);

            worksheet.Cells[28, 1].Value = "";
            worksheet.Cells[28, 2].Value = "";
            worksheet.Cells[28, 3].Value = "";
            worksheet.Cells[28, 4].Value = "-H0200 ค่าใช้จ่ายทางอ้อม";
            worksheet.Cells[28, 5].Value = decimal.Parse(estimate.c25);

            worksheet.Cells[29, 1].Value = "";
            worksheet.Cells[29, 2].Value = "";
            worksheet.Cells[29, 3].Value = "";
            worksheet.Cells[29, 4].Value = "-H0800 ค่าจ่ายภายใน";
            worksheet.Cells[29, 5].Value = decimal.Parse(estimate.c26);
            worksheet.Cells[29, 5].Style.Border.Bottom.Style = ExcelBorderStyle.Thick;

            worksheet.Cells[30, 1].Value = "";
            worksheet.Cells[30, 2].Value = "";
            worksheet.Cells[30, 3].Value = "";
            worksheet.Cells[30, 4].Value = "Total Internal Cost (2)";
            worksheet.Cells[30, 5].Value = decimal.Parse(estimate.c27);
            worksheet.Cells[30, 5].Style.Border.Bottom.Style = ExcelBorderStyle.Double;

            worksheet.Cells[31, 1].Value = "";
            worksheet.Cells[31, 2].Value = "";
            worksheet.Cells[31, 3].Value = "";
            worksheet.Cells[31, 4].Value = "รายละเอียดต้นทุนบางรายการ";
            worksheet.Cells[31, 5].Value = "";

            worksheet.Cells[32, 1].Value = "";
            worksheet.Cells[32, 2].Value = "";
            worksheet.Cells[32, 3].Value = "";
            worksheet.Cells[32, 4].Value = "-D2100 วัสดุเช่าส่วนกลาง (เฉพาะฝ่ายบัญชีเท่านั้น)";
            worksheet.Cells[32, 5].Value = decimal.Parse(estimate.c28);

            worksheet.Cells[33, 1].Value = "";
            worksheet.Cells[33, 2].Value = "";
            worksheet.Cells[33, 3].Value = "";
            worksheet.Cells[33, 4].Value = "-E0800 ค่าแรงรายวันงานก่อสร้าง)";
            worksheet.Cells[33, 5].Value = decimal.Parse(estimate.c29);

            worksheet.Cells[34, 1].Value = "";
            worksheet.Cells[34, 2].Value = "";
            worksheet.Cells[34, 3].Value = "";
            worksheet.Cells[34, 4].Value = "-E0910 ค่าแรงรายวันบริษัท[ปกติ])";
            worksheet.Cells[34, 5].Value = decimal.Parse(estimate.c30);

            worksheet.Cells[35, 1].Value = "";
            worksheet.Cells[35, 2].Value = "";
            worksheet.Cells[35, 3].Value = "";
            worksheet.Cells[35, 4].Value = "-E0920 ค่าแรงรายวันบริษัท[ล่วงหน้า]";
            worksheet.Cells[35, 5].Value = decimal.Parse(estimate.c31);

            worksheet.Cells[36, 1].Value = "";
            worksheet.Cells[36, 2].Value = "";
            worksheet.Cells[36, 3].Value = "";
            worksheet.Cells[36, 4].Value = "-E0930 พนักงานบริษัท[ล่วงหน้า]";
            worksheet.Cells[36, 5].Value = decimal.Parse(estimate.c32);

            worksheet.Cells[37, 1].Value = "";
            worksheet.Cells[37, 2].Value = "";
            worksheet.Cells[37, 3].Value = "";
            worksheet.Cells[37, 4].Value = "-E0940 ค่าแรงฝากเบิก";
            worksheet.Cells[37, 5].Value = decimal.Parse(estimate.c33);

            worksheet.Cells[38, 1].Value = "";
            worksheet.Cells[38, 2].Value = "";
            worksheet.Cells[38, 3].Value = "";
            worksheet.Cells[38, 4].Value = "-G0210 ภาษีหัก ณ ที่จ่าย";
            worksheet.Cells[38, 5].Value = decimal.Parse(estimate.c34);

            worksheet.Cells[39, 1].Value = "";
            worksheet.Cells[39, 2].Value = "";
            worksheet.Cells[39, 3].Value = "";
            worksheet.Cells[39, 4].Value = "-H0600 ต้นทุนโอน";
            worksheet.Cells[39, 5].Value = decimal.Parse(estimate.c35);

            worksheet.Cells[40, 1].Value = "";
            worksheet.Cells[40, 2].Value = "";
            worksheet.Cells[40, 3].Value = "";
            worksheet.Cells[40, 4].Value = "-F5999 ค่าใช้จ่ายต้นทุนล่วงหน้า";
            worksheet.Cells[40, 5].Value = decimal.Parse(estimate.c36);

            worksheet.Cells[41, 1].Value = "";
            worksheet.Cells[41, 2].Value = "";
            worksheet.Cells[41, 3].Value = "";
            worksheet.Cells[41, 4].Value = "-H0400 ล/นมัดจำ-เงินล่วงหน้า";
            worksheet.Cells[41, 5].Value = decimal.Parse(estimate.c37);

            worksheet.Cells["B1:B40,C1:C40,E1:E40"].Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;

            worksheet.Column(3).Style.Numberformat.Format = "#0\\.00%";
            worksheet.Column(2).Style.Numberformat.Format = "#,##0.00";
            worksheet.Column(5).Style.Numberformat.Format = "#,##0.00";


            ExcelAddress cellIsAddress = new ExcelAddress("A1:AZ10000");
            var cfRule37 = worksheet.ConditionalFormatting.AddLessThan(cellIsAddress);
            cfRule37.Formula = "0";
            cfRule37.Style.Font.Bold = true;
            cfRule37.Style.Font.Color.Color = Color.Red;

            // AutoFitColumns
            worksheet.Cells[1, 1, 41, 5].AutoFitColumns();

            // Add to table / Add summary row
            var tbl = worksheet.Tables.Add(new ExcelAddressBase(fromRow: 2, fromCol: 1, toRow: 41, toColumn: 5), "Data");
            tbl.ShowHeader = false;
            tbl.TableStyle = TableStyles.Dark9;
            tbl.ShowFilter = false;







            var worksheet3 = package.Workbook.Worksheets.Add("JobGroup");
            worksheet3.Cells[1, 1].Value = "Job : " + estimate.EstimateJob + " ช่วงเวลา : " + estimate.EstimateStart + " - " + estimate.EstimateEnd;
            worksheet3.Cells["A1:K1"].Merge = true;
            worksheet3.Cells["A1"].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;

            var cfRule1111 = worksheet3.ConditionalFormatting.AddLessThan(cellIsAddress);
            cfRule1111.Formula = "0";
            cfRule1111.Style.Font.Bold = true;
            cfRule1111.Style.Font.Color.Color = Color.Red;
            //First add the headers
            worksheet3.Cells[2, 1].Value = "JobTask";
            worksheet3.Cells[2, 2].Value = "ThisPeriodQTY";
            worksheet3.Cells[2, 3].Value = "ThisPeriodAMT";
            worksheet3.Cells[2, 4].Value = "OpenningQTY";
            worksheet3.Cells[2, 5].Value = "OpenningAMT";
            worksheet3.Cells[2, 6].Value = "CumBalQTY";
            worksheet3.Cells[2, 7].Value = "CumBalAMT";
            worksheet3.Cells[2, 8].Value = "BudgetQty";
            worksheet3.Cells[2, 9].Value = "Qry%";
            worksheet3.Cells[2, 10].Value = "BudgetAMT";
            worksheet3.Cells[2, 11].Value = "AMT%";








            var worksheet2 = package.Workbook.Worksheets.Add("JobCostDetail");
            worksheet2.Cells[1, 1].Value = "Job : " + estimate.EstimateJob + " ช่วงเวลา : " + estimate.EstimateStart + " - " + estimate.EstimateEnd;
            worksheet2.Cells["A1:K1"].Merge = true;
            worksheet2.Cells["A1"].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;

            var cfRule38 = worksheet2.ConditionalFormatting.AddLessThan(cellIsAddress);
            cfRule38.Formula = "0";
            cfRule38.Style.Font.Bold = true;
            cfRule38.Style.Font.Color.Color = Color.Red;




            //First add the headers
            worksheet2.Cells[2, 1].Value = "JobTask";
            worksheet2.Cells[2, 2].Value = "ThisPeriodQTY";
            worksheet2.Cells[2, 3].Value = "ThisPeriodAMT";
            worksheet2.Cells[2, 4].Value = "OpenningQTY";
            worksheet2.Cells[2, 5].Value = "OpenningAMT";
            worksheet2.Cells[2, 6].Value = "CumBalQTY";
            worksheet2.Cells[2, 7].Value = "CumBalAMT";
            worksheet2.Cells[2, 8].Value = "BudgetQty";
            worksheet2.Cells[2, 9].Value = "Qry%";
            worksheet2.Cells[2, 10].Value = "BudgetAMT";
            worksheet2.Cells[2, 11].Value = "AMT%";



            var JobNo = estimate.EstimateJob;
            var date1 = estimate.EstimateStart + " 00:00:00";
            var date2 = estimate.EstimateEnd + " 23:59:59";


            decimal A00 = 0;
            decimal B00 = 0;
            decimal C00 = 0;
            decimal D00 = 0;
            decimal E00 = 0;
            decimal F00 = 0;
            decimal G00 = 0;
            decimal H00 = 0;
            decimal N00 = 0;
            decimal O00 = 0;
            decimal G0210 = 0;
            decimal C0110 = 0;
            decimal C0120 = 0;
            decimal C0130 = 0;
            decimal C0410 = 0;
            decimal C0510 = 0;
            decimal C0910 = 0;
            decimal C1010 = 0;
            decimal C1110 = 0;
            decimal D0450 = 0;
            decimal H0110 = 0;
            decimal H0130 = 0;
            decimal H0200 = 0;
            decimal H0800 = 0;
            decimal D2100 = 0;
            decimal E0800 = 0;
            decimal E0910 = 0;
            decimal E0920 = 0;
            decimal E0930 = 0;
            decimal E0940 = 0;
            decimal H0600 = 0;
            decimal F5999 = 0;
            decimal H0400 = 0;
            decimal TCXI = 0; //Total Cose (External+Internal)
    
            var r = 3;

            decimal thisQ = 0;
            decimal thisA = 0;
            decimal openQ = 0;
            decimal openA = 0;
            decimal cumQ = 0;
            decimal cumA = 0;
            decimal budQ = 0;
            decimal budA = 0;

            decimal OthisQ = 0;
            decimal OthisA = 0;
            decimal OopenQ = 0;
            decimal OopenA = 0;
            decimal OcumQ = 0;
            decimal OcumA = 0;
            decimal ObudQ = 0;
            decimal ObudA = 0;




            var queryJobTask = "SELECT *," +
                "(select top 1 [Job Task No_]+' '+dbo."+ Environment.GetEnvironmentVariable("Company") +"Job Task].Description from dbo."+ Environment.GetEnvironmentVariable("Company") +"Job Task] where dbo."+ Environment.GetEnvironmentVariable("Company") +"Job Task].[Job Task No_]=a.JobTaskNo) as DescriptionFull, " +
                "(select top 1 dbo."+ Environment.GetEnvironmentVariable("Company") +"Job Task].Description from dbo."+ Environment.GetEnvironmentVariable("Company") +"Job Task] where dbo."+ Environment.GetEnvironmentVariable("Company") +"Job Task].[Job Task No_]=a.JobTaskNo) as DescriptionSubJob " +
                " FROM( " +
                "SELECT " +
                "DISTINCT dbo."+ Environment.GetEnvironmentVariable("Company") +"Job Task].[Job Task No_] as JobTaskNo, " +
                "SUBSTRING(dbo."+ Environment.GetEnvironmentVariable("Company") +"Job Task].[Job Task No_],1,1)+'0000' as MainJobID, " +
                "SUBSTRING(dbo."+ Environment.GetEnvironmentVariable("Company") +"Job Task].[Job Task No_],1,3)+'00' as SubJobID , " +
                "SUBSTRING(dbo."+ Environment.GetEnvironmentVariable("Company") +"Job Task].[Job Task No_],1,5) as JobTaskCut," +
                "SUBSTRING(dbo."+ Environment.GetEnvironmentVariable("Company") +"Job Task].[Job Task No_],1,1) as JobstringCust " +
                "FROM dbo."+ Environment.GetEnvironmentVariable("Company") +"Job Task]  ) as a Order by a.JobTaskNo asc ";


            var jobTasks = _navcontext.jobTasks.FromSqlRaw(queryJobTask).ToList();


            var queryJobPlanningLine = "SELECT ROW_NUMBER() OVER (ORDER BY [Job Task No_]) as ID," +
                "dbo."+ Environment.GetEnvironmentVariable("Company") +"Job Planning Line].[Job No_] as JobNo, " +
                "SUBSTRING(dbo."+ Environment.GetEnvironmentVariable("Company") +"Job Planning Line].[Job Task No_],1,1) as JobTaskCut, " +
                "SUBSTRING(dbo."+ Environment.GetEnvironmentVariable("Company") +"Job Planning Line].[Job Task No_],1,1)+'0000' as MainJobID, " +
                "SUBSTRING(dbo."+ Environment.GetEnvironmentVariable("Company") +"Job Planning Line].[Job Task No_],1,3)+'00' as SubJobID, " +
                "dbo."+ Environment.GetEnvironmentVariable("Company") +"Job Planning Line].[Job Task No_] as JobTaskNo, " +
                "dbo."+ Environment.GetEnvironmentVariable("Company") +"Job Planning Line].[Total Cost (LCY)] as TotalCost, " +
                "dbo."+ Environment.GetEnvironmentVariable("Company") +"Job Planning Line].Quantity as Quantity " +
                "FROM dbo."+ Environment.GetEnvironmentVariable("Company") +"Job Planning Line] ";
            var jobPlanningLines = _navcontext.jobPlanningLines.FromSqlRaw(queryJobPlanningLine).ToList();


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
            var JobLedgerNews = _navcontext.JobLedgerNews.FromSqlRaw(queryData,JobNo).ToList();




            decimal sumReclass;
            decimal sumIssue;
            decimal sumRental;
            decimal sumReturnCal;
            decimal sumReturn;
            decimal sumIssueExternal;
            var startdate = Convert.ToDateTime(date1);
            var enddate = Convert.ToDateTime(date2);
            decimal ThisPeriodQty = 0;
            decimal ThisPeriodAMT = 0;
            decimal OpenningQTY = 0;
            decimal OpenningAMT = 0;
            decimal CumBalQTY = 0;
            decimal CumBalAMT = 0;
            decimal BudgetQTY = 0;
            decimal BudgetAMT = 0;
            decimal PercenAMT = 0;
            decimal PercenQTY = 0;
            decimal sumReclassAPO = 0;
            int rl = 6;


            //List<Tmp_JobCostReport> instances = new List<Tmp_JobCostReport>();
            //Tmp_JobCostReport current = null;



            //List<Tmp_JobCostReport> SubTotal = new List<Tmp_JobCostReport>();
            //Tmp_JobCostReport CurrentSubTotal = null;

            foreach (var std in jobTasks as IList<JobTask>)
            {

                if (std.JobTaskCut == std.MainJobID) //ถ้าเป็น Head ให้ทำ
                {
                    if (std.JobstringCust == "O" || std.JobstringCust == "N")
                    {
                        sumReclass = 0;
                        sumIssue = 0;
                        sumRental = 0;
                        sumReturnCal = 0;
                        sumReturn = 0;
                        sumIssueExternal = 0;
                        sumReclass = JobLedgerNews.Where(c => c.JobNo == JobNo && c.JobMain == std.JobTaskCut && c.TypeOfTask == 0 && c.PostingDate >= startdate && c.PostingDate <= enddate).Sum(c => c.Quantity);
                        sumIssue = JobLedgerNews.Where(c => c.JobNo == JobNo && c.JobMain == std.JobTaskCut && c.TypeOfTask == 3 && c.PostingDate >= startdate && c.PostingDate <= enddate).Sum(c => c.Quantity);
                        sumRental = JobLedgerNews.Where(c => c.JobNo == JobNo && c.JobMain == std.JobTaskCut && c.TypeOfTask == 2 && c.PostingDate >= startdate && c.PostingDate <= enddate).Sum(c => c.Quantity);
                        sumReturnCal = JobLedgerNews.Where(c => c.JobNo == JobNo && c.JobMain == std.JobTaskCut && c.TypeOfTask == 1 && c.Document3 == "CAL" && c.PostingDate >= startdate && c.PostingDate <= enddate).Sum(c => c.Quantity) * 2;
                        sumReturn = JobLedgerNews.Where(c => c.JobNo == JobNo && c.JobMain == std.JobTaskCut && c.TypeOfTask == 1 && c.PostingDate >= startdate && c.PostingDate <= enddate).Sum(c => c.Quantity);
                        sumIssueExternal = JobLedgerNews.Where(c => c.JobNo != JobNo && c.JobMain == std.JobTaskCut && c.TypeOfTask == 3 && c.PostingDate >= startdate && c.FromLocation == JobNo && c.PostingDate <= enddate).Sum(c => c.Quantity) * 2;


                        ThisPeriodQty = (sumReclass + sumIssue + sumRental + sumReturnCal) - (sumReturn + sumIssueExternal);
                        //current.ThisPeriodQty = ThisPeriodQty;



                        sumReclass = 0;
                        sumIssue = 0;
                        sumRental = 0;
                        sumReturnCal = 0;
                        sumReturn = 0;
                        sumIssueExternal = 0;
                        sumReclass = JobLedgerNews.Where(c => c.JobNo == JobNo && c.JobMain == std.JobTaskCut && c.TypeOfTask == 0 && c.PostingDate >= startdate && c.PostingDate <= enddate).Sum(c => c.TotalCost + c.LineAmount);
                        sumIssue = JobLedgerNews.Where(c => c.JobNo == JobNo && c.JobMain == std.JobTaskCut && c.TypeOfTask == 3 && c.PostingDate >= startdate && c.PostingDate <= enddate).Sum(c => c.TotalCost + c.LineAmount);
                        sumRental = JobLedgerNews.Where(c => c.JobNo == JobNo && c.JobMain == std.JobTaskCut && c.TypeOfTask == 2 && c.PostingDate >= startdate && c.PostingDate <= enddate).Sum(c => c.TotalCost + c.LineAmount);
                        sumReturnCal = JobLedgerNews.Where(c => c.JobNo == JobNo && c.JobMain == std.JobTaskCut && c.TypeOfTask == 1 && c.Document3 == "CAL" && c.PostingDate >= startdate && c.PostingDate <= enddate).Sum(c => c.TotalCost + c.LineAmount) * 2;
                        sumReturn = JobLedgerNews.Where(c => c.JobNo == JobNo && c.JobMain == std.JobTaskCut && c.TypeOfTask == 1 && c.PostingDate >= startdate && c.PostingDate <= enddate).Sum(c => c.TotalCost + c.LineAmount);
                        sumIssueExternal = JobLedgerNews.Where(c => c.JobNo != JobNo && c.JobMain == std.JobTaskCut && c.TypeOfTask == 3 && c.FromLocation == JobNo && c.PostingDate >= startdate && c.PostingDate <= enddate).Sum(c => c.TotalCost + c.LineAmount) * 2;
                        sumReclassAPO = JobLedgerNews.Where(c => c.JobNo == JobNo && c.JobMain == std.JobTaskCut && c.TypeOfTask == 0 && c.PostingDate >= startdate && c.PostingDate <= enddate && c.TotalCost == 0).Sum(c => c.Total);
                        ThisPeriodAMT = (sumReclass + sumIssue + sumRental + sumReturnCal + sumReclassAPO) - (sumReturn + sumIssueExternal);
                        //current.ThisPeriodAMT = ThisPeriodAMT;



                        sumReclass = 0;
                        sumIssue = 0;
                        sumRental = 0;
                        sumReturnCal = 0;
                        sumReturn = 0;
                        sumIssueExternal = 0;
                        sumReclass = JobLedgerNews.Where(c => c.JobNo == JobNo && c.JobMain == std.JobTaskCut && c.TypeOfTask == 0 && c.PostingDate < startdate).Sum(c => c.Quantity);
                        sumIssue = JobLedgerNews.Where(c => c.JobNo == JobNo && c.JobMain == std.JobTaskCut && c.TypeOfTask == 3 && c.PostingDate < startdate).Sum(c => c.Quantity);
                        sumRental = JobLedgerNews.Where(c => c.JobNo == JobNo && c.JobMain == std.JobTaskCut && c.TypeOfTask == 2 && c.PostingDate < startdate).Sum(c => c.Quantity);
                        sumReturnCal = JobLedgerNews.Where(c => c.JobNo == JobNo && c.JobMain == std.JobTaskCut && c.TypeOfTask == 1 && c.Document3 == "CAL" && c.PostingDate < startdate).Sum(c => c.Quantity) * 2;
                        sumReturn = JobLedgerNews.Where(c => c.JobNo == JobNo && c.JobMain == std.JobTaskCut && c.TypeOfTask == 1 && c.PostingDate < startdate).Sum(c => c.Quantity);
                        sumIssueExternal = JobLedgerNews.Where(c => c.JobNo != JobNo && c.JobMain == std.JobTaskCut && c.TypeOfTask == 3 && c.PostingDate < startdate && c.FromLocation == JobNo).Sum(c => c.Quantity) * 2;
                        OpenningQTY = (sumReclass + sumIssue + sumRental + sumReturnCal) - (sumReturn + sumIssueExternal);
                        //current.OpenningQTY = OpenningQTY;



                        sumReclass = 0;
                        sumIssue = 0;
                        sumRental = 0;
                        sumReturnCal = 0;
                        sumReturn = 0;
                        sumIssueExternal = 0;
                        sumReclass = JobLedgerNews.Where(c => c.JobNo == JobNo && c.JobMain == std.JobTaskCut && c.TypeOfTask == 0 && c.PostingDate < startdate).Sum(c => c.TotalCost + c.LineAmount);
                        sumIssue = JobLedgerNews.Where(c => c.JobNo == JobNo && c.JobMain == std.JobTaskCut && c.TypeOfTask == 3 && c.PostingDate < startdate).Sum(c => c.TotalCost + c.LineAmount);
                        sumRental = JobLedgerNews.Where(c => c.JobNo == JobNo && c.JobMain == std.JobTaskCut && c.TypeOfTask == 2 && c.PostingDate < startdate).Sum(c => c.TotalCost + c.LineAmount);
                        sumReturnCal = JobLedgerNews.Where(c => c.JobNo == JobNo && c.JobMain == std.JobTaskCut && c.TypeOfTask == 1 && c.Document3 == "CAL" && c.PostingDate < startdate).Sum(c => c.TotalCost + c.LineAmount) * 2;
                        sumReturn = JobLedgerNews.Where(c => c.JobNo == JobNo && c.JobMain == std.JobTaskCut && c.TypeOfTask == 1 && c.PostingDate < startdate).Sum(c => c.TotalCost + c.LineAmount);
                        sumIssueExternal = JobLedgerNews.Where(c => c.JobNo != JobNo && c.JobMain == std.JobTaskCut && c.TypeOfTask == 3 && c.FromLocation == JobNo && c.PostingDate < startdate).Sum(c => c.TotalCost + c.LineAmount) * 2;
                        sumReclassAPO = JobLedgerNews.Where(c => c.JobNo == JobNo && c.JobMain == std.JobTaskCut && c.TypeOfTask == 0 && c.PostingDate < startdate && c.TotalCost == 0).Sum(c => c.Total);
                        OpenningAMT = (sumReclass + sumIssue + sumRental + sumReturnCal + sumReclassAPO) - (sumReturn + sumIssueExternal);
                        //current.OpenningAMT = OpenningAMT;


                        sumReclass = 0;
                        sumIssue = 0;
                        sumRental = 0;
                        sumReturnCal = 0;
                        sumReturn = 0;
                        sumIssueExternal = 0;
                        sumReclass = JobLedgerNews.Where(c => c.JobNo == JobNo && c.JobMain == std.JobTaskCut && c.TypeOfTask == 0 && c.PostingDate <= enddate).Sum(c => c.Quantity);
                        sumIssue = JobLedgerNews.Where(c => c.JobNo == JobNo && c.JobMain == std.JobTaskCut && c.TypeOfTask == 3 && c.PostingDate <= enddate).Sum(c => c.Quantity);
                        sumRental = JobLedgerNews.Where(c => c.JobNo == JobNo && c.JobMain == std.JobTaskCut && c.TypeOfTask == 2 && c.PostingDate <= enddate).Sum(c => c.Quantity);
                        sumReturnCal = JobLedgerNews.Where(c => c.JobNo == JobNo && c.JobMain == std.JobTaskCut && c.TypeOfTask == 1 && c.Document3 == "CAL" && c.PostingDate <= enddate).Sum(c => c.Quantity) * 2;
                        sumReturn = JobLedgerNews.Where(c => c.JobNo == JobNo && c.JobMain == std.JobTaskCut && c.TypeOfTask == 1 && c.PostingDate <= enddate).Sum(c => c.Quantity);
                        sumIssueExternal = JobLedgerNews.Where(c => c.JobNo != JobNo && c.JobMain == std.JobTaskCut && c.TypeOfTask == 3 && c.PostingDate <= enddate && c.FromLocation == JobNo).Sum(c => c.Quantity) * 2;
                        CumBalQTY = (sumReclass + sumIssue + sumRental + sumReturnCal) - (sumReturn + sumIssueExternal);
                        //current.CumBalQTY = CumBalQTY;


                        sumReclass = 0;
                        sumIssue = 0;
                        sumRental = 0;
                        sumReturnCal = 0;
                        sumReturn = 0;
                        sumIssueExternal = 0;
                        sumReclass = JobLedgerNews.Where(c => c.JobNo == JobNo && c.JobMain == std.JobTaskCut && c.TypeOfTask == 0 && c.PostingDate <= enddate).Sum(c => c.TotalCost + c.LineAmount);
                        sumIssue = JobLedgerNews.Where(c => c.JobNo == JobNo && c.JobMain == std.JobTaskCut && c.TypeOfTask == 3 && c.PostingDate <= enddate).Sum(c => c.TotalCost + c.LineAmount);
                        sumRental = JobLedgerNews.Where(c => c.JobNo == JobNo && c.JobMain == std.JobTaskCut && c.TypeOfTask == 2 && c.PostingDate <= enddate).Sum(c => c.TotalCost + c.LineAmount);
                        sumReturnCal = JobLedgerNews.Where(c => c.JobNo == JobNo && c.JobMain == std.JobTaskCut && c.TypeOfTask == 1 && c.Document3 == "CAL" && c.PostingDate <= enddate).Sum(c => c.TotalCost + c.LineAmount) * 2;
                        sumReturn = JobLedgerNews.Where(c => c.JobNo == JobNo && c.JobMain == std.JobTaskCut && c.TypeOfTask == 1 && c.PostingDate <= enddate).Sum(c => c.TotalCost + c.LineAmount);
                        sumIssueExternal = JobLedgerNews.Where(c => c.JobNo != JobNo && c.JobMain == std.JobTaskCut && c.TypeOfTask == 3 && c.FromLocation == JobNo && c.PostingDate <= enddate).Sum(c => c.TotalCost + c.LineAmount) * 2;
                        sumReclassAPO = JobLedgerNews.Where(c => c.JobNo == JobNo && c.JobMain == std.JobTaskCut && c.TypeOfTask == 0 && c.PostingDate <= enddate && c.TotalCost == 0).Sum(c => c.Total);
                        CumBalAMT = (sumReclass + sumIssue + sumRental + sumReturnCal + sumReclassAPO) - (sumReturn + sumIssueExternal);
                        //current.CumBalAMT = CumBalAMT;


                        BudgetQTY = jobPlanningLines.Where(c => c.JobNo == JobNo && c.MainJobID == std.JobTaskCut).Sum(c => c.Quantity);
                        BudgetAMT = jobPlanningLines.Where(c => c.JobNo == JobNo && c.MainJobID == std.JobTaskCut).Sum(c => c.TotalCost);
                        if (BudgetQTY == 0)
                        {
                            PercenQTY = 0;
                        }
                        else
                        {
                            PercenQTY = (CumBalQTY / BudgetQTY) * 100;
                        }

                        if (BudgetAMT == 0)
                        {
                            PercenAMT = 0;
                        }
                        else
                        {
                            PercenAMT = (CumBalAMT / BudgetAMT) * 100;
                        }

   


                    }
                    else
                    {

                        sumReclass = 0;
                        sumIssue = 0;
                        sumRental = 0;
                        sumReturnCal = 0;
                        sumReturn = 0;
                        sumIssueExternal = 0;
                        sumReclass = JobLedgerNews.Where(c => c.JobNo == JobNo && c.JobMain == std.JobTaskCut && c.TypeOfTask == 0 && c.PostingDate >= startdate && c.PostingDate <= enddate).Sum(c => c.Quantity);
                        sumIssue = JobLedgerNews.Where(c => c.JobNo == JobNo && c.JobMain == std.JobTaskCut && c.TypeOfTask == 3 && c.PostingDate >= startdate && c.PostingDate <= enddate).Sum(c => c.Quantity);
                        sumRental = JobLedgerNews.Where(c => c.JobNo == JobNo && c.JobMain == std.JobTaskCut && c.TypeOfTask == 2 && c.PostingDate >= startdate && c.PostingDate <= enddate).Sum(c => c.Quantity);
                        sumReturnCal = JobLedgerNews.Where(c => c.JobNo == JobNo && c.JobMain == std.JobTaskCut && c.TypeOfTask == 1 && c.Document3 == "CAL" && c.PostingDate >= startdate && c.PostingDate <= enddate).Sum(c => c.Quantity) * 2;
                        sumReturn = JobLedgerNews.Where(c => c.JobNo == JobNo && c.JobMain == std.JobTaskCut && c.TypeOfTask == 1 && c.PostingDate >= startdate && c.PostingDate <= enddate).Sum(c => c.Quantity);
                        sumIssueExternal = JobLedgerNews.Where(c => c.JobNo != JobNo && c.JobMain == std.JobTaskCut && c.TypeOfTask == 3 && c.PostingDate >= startdate && c.FromLocation == JobNo && c.PostingDate <= enddate).Sum(c => c.Quantity) * 2;
                        ThisPeriodQty = (sumReclass + sumIssue + sumRental + sumReturnCal) - (sumReturn + sumIssueExternal);
                        //current.ThisPeriodQty = ThisPeriodQty;


                        sumReclass = 0;
                        sumIssue = 0;
                        sumRental = 0;
                        sumReturnCal = 0;
                        sumReturn = 0;
                        sumIssueExternal = 0;
                        sumReclass = JobLedgerNews.Where(c => c.JobNo == JobNo && c.JobMain == std.JobTaskCut && c.TypeOfTask == 0 && c.PostingDate >= startdate && c.PostingDate <= enddate).Sum(c => c.TotalCost);
                        sumIssue = JobLedgerNews.Where(c => c.JobNo == JobNo && c.JobMain == std.JobTaskCut && c.TypeOfTask == 3 && c.PostingDate >= startdate && c.PostingDate <= enddate).Sum(c => c.TotalCost);
                        sumRental = JobLedgerNews.Where(c => c.JobNo == JobNo && c.JobMain == std.JobTaskCut && c.TypeOfTask == 2 && c.PostingDate >= startdate && c.PostingDate <= enddate).Sum(c => c.TotalCost);
                        sumReturnCal = JobLedgerNews.Where(c => c.JobNo == JobNo && c.JobMain == std.JobTaskCut && c.TypeOfTask == 1 && c.Document3 == "CAL" && c.PostingDate >= startdate && c.PostingDate <= enddate).Sum(c => c.TotalCost) * 2;
                        sumReturn = JobLedgerNews.Where(c => c.JobNo == JobNo && c.JobMain == std.JobTaskCut && c.TypeOfTask == 1 && c.PostingDate >= startdate && c.PostingDate <= enddate).Sum(c => c.TotalCost);
                        sumIssueExternal = JobLedgerNews.Where(c => c.JobNo != JobNo && c.JobMain == std.JobTaskCut && c.TypeOfTask == 3 && c.FromLocation == JobNo && c.PostingDate >= startdate && c.PostingDate <= enddate).Sum(c => c.TotalCost) * 2;
                        sumReclassAPO = JobLedgerNews.Where(c => c.JobNo == JobNo && c.JobMain == std.JobTaskCut && c.TypeOfTask == 0 && c.PostingDate >= startdate && c.PostingDate <= enddate && c.TotalCost == 0).Sum(c => c.Total);
                        ThisPeriodAMT = (sumReclass + sumIssue + sumRental + sumReturnCal + sumReclassAPO) - (sumReturn + sumIssueExternal);
                        //current.ThisPeriodAMT = ThisPeriodAMT;


                        sumReclass = 0;
                        sumIssue = 0;
                        sumRental = 0;
                        sumReturnCal = 0;
                        sumReturn = 0;
                        sumIssueExternal = 0;
                        sumReclass = JobLedgerNews.Where(c => c.JobNo == JobNo && c.JobMain == std.JobTaskCut && c.TypeOfTask == 0 && c.PostingDate < startdate).Sum(c => c.Quantity);
                        sumIssue = JobLedgerNews.Where(c => c.JobNo == JobNo && c.JobMain == std.JobTaskCut && c.TypeOfTask == 3 && c.PostingDate < startdate).Sum(c => c.Quantity);
                        sumRental = JobLedgerNews.Where(c => c.JobNo == JobNo && c.JobMain == std.JobTaskCut && c.TypeOfTask == 2 && c.PostingDate < startdate).Sum(c => c.Quantity);
                        sumReturnCal = JobLedgerNews.Where(c => c.JobNo == JobNo && c.JobMain == std.JobTaskCut && c.TypeOfTask == 1 && c.Document3 == "CAL" && c.PostingDate < startdate).Sum(c => c.Quantity) * 2;
                        sumReturn = JobLedgerNews.Where(c => c.JobNo == JobNo && c.JobMain == std.JobTaskCut && c.TypeOfTask == 1 && c.PostingDate < startdate).Sum(c => c.Quantity);
                        sumIssueExternal = JobLedgerNews.Where(c => c.JobNo != JobNo && c.JobMain == std.JobTaskCut && c.TypeOfTask == 3 && c.PostingDate < startdate && c.FromLocation == JobNo).Sum(c => c.Quantity) * 2;
                        OpenningQTY = (sumReclass + sumIssue + sumRental + sumReturnCal) - (sumReturn + sumIssueExternal);
                        //current.OpenningQTY = OpenningQTY;



                        sumReclass = 0;
                        sumIssue = 0;
                        sumRental = 0;
                        sumReturnCal = 0;
                        sumReturn = 0;
                        sumIssueExternal = 0;
                        sumReclass = JobLedgerNews.Where(c => c.JobNo == JobNo && c.JobMain == std.JobTaskCut && c.TypeOfTask == 0 && c.PostingDate < startdate).Sum(c => c.TotalCost);
                        sumIssue = JobLedgerNews.Where(c => c.JobNo == JobNo && c.JobMain == std.JobTaskCut && c.TypeOfTask == 3 && c.PostingDate < startdate).Sum(c => c.TotalCost);
                        sumRental = JobLedgerNews.Where(c => c.JobNo == JobNo && c.JobMain == std.JobTaskCut && c.TypeOfTask == 2 && c.PostingDate < startdate).Sum(c => c.TotalCost);
                        sumReturnCal = JobLedgerNews.Where(c => c.JobNo == JobNo && c.JobMain == std.JobTaskCut && c.TypeOfTask == 1 && c.Document3 == "CAL" && c.PostingDate < startdate).Sum(c => c.TotalCost) * 2;
                        sumReturn = JobLedgerNews.Where(c => c.JobNo == JobNo && c.JobMain == std.JobTaskCut && c.TypeOfTask == 1 && c.PostingDate < startdate).Sum(c => c.TotalCost);
                        sumIssueExternal = JobLedgerNews.Where(c => c.JobNo != JobNo && c.JobMain == std.JobTaskCut && c.TypeOfTask == 3 && c.FromLocation == JobNo && c.PostingDate < startdate).Sum(c => c.TotalCost) * 2;
                        sumReclassAPO = JobLedgerNews.Where(c => c.JobNo == JobNo && c.JobMain == std.JobTaskCut && c.TypeOfTask == 0 && c.PostingDate < startdate && c.TotalCost == 0).Sum(c => c.Total);
                        OpenningAMT = (sumReclass + sumIssue + sumRental + sumReturnCal + sumReclassAPO) - (sumReturn + sumIssueExternal);
                        //current.OpenningAMT = OpenningAMT;



                        sumReclass = 0;
                        sumIssue = 0;
                        sumRental = 0;
                        sumReturnCal = 0;
                        sumReturn = 0;
                        sumIssueExternal = 0;
                        sumReclass = JobLedgerNews.Where(c => c.JobNo == JobNo && c.JobMain == std.JobTaskCut && c.TypeOfTask == 0 && c.PostingDate <= enddate).Sum(c => c.Quantity);
                        sumIssue = JobLedgerNews.Where(c => c.JobNo == JobNo && c.JobMain == std.JobTaskCut && c.TypeOfTask == 3 && c.PostingDate <= enddate).Sum(c => c.Quantity);
                        sumRental = JobLedgerNews.Where(c => c.JobNo == JobNo && c.JobMain == std.JobTaskCut && c.TypeOfTask == 2 && c.PostingDate <= enddate).Sum(c => c.Quantity);
                        sumReturnCal = JobLedgerNews.Where(c => c.JobNo == JobNo && c.JobMain == std.JobTaskCut && c.TypeOfTask == 1 && c.Document3 == "CAL" && c.PostingDate <= enddate).Sum(c => c.Quantity) * 2;
                        sumReturn = JobLedgerNews.Where(c => c.JobNo == JobNo && c.JobMain == std.JobTaskCut && c.TypeOfTask == 1 && c.PostingDate <= enddate).Sum(c => c.Quantity);
                        sumIssueExternal = JobLedgerNews.Where(c => c.JobNo != JobNo && c.JobMain == std.JobTaskCut && c.TypeOfTask == 3 && c.PostingDate <= enddate && c.FromLocation == JobNo).Sum(c => c.Quantity) * 2;
                        CumBalQTY = (sumReclass + sumIssue + sumRental + sumReturnCal) - (sumReturn + sumIssueExternal);
                        //current.CumBalQTY = CumBalQTY;


                        sumReclass = 0;
                        sumIssue = 0;
                        sumRental = 0;
                        sumReturnCal = 0;
                        sumReturn = 0;
                        sumIssueExternal = 0;
                        sumReclass = JobLedgerNews.Where(c => c.JobNo == JobNo && c.JobMain == std.JobTaskCut && c.TypeOfTask == 0 && c.PostingDate <= enddate).Sum(c => c.TotalCost);
                        sumIssue = JobLedgerNews.Where(c => c.JobNo == JobNo && c.JobMain == std.JobTaskCut && c.TypeOfTask == 3 && c.PostingDate <= enddate).Sum(c => c.TotalCost);
                        sumRental = JobLedgerNews.Where(c => c.JobNo == JobNo && c.JobMain == std.JobTaskCut && c.TypeOfTask == 2 && c.PostingDate <= enddate).Sum(c => c.TotalCost);
                        sumReturnCal = JobLedgerNews.Where(c => c.JobNo == JobNo && c.JobMain == std.JobTaskCut && c.TypeOfTask == 1 && c.Document3 == "CAL" && c.PostingDate <= enddate).Sum(c => c.TotalCost) * 2;
                        sumReturn = JobLedgerNews.Where(c => c.JobNo == JobNo && c.JobMain == std.JobTaskCut && c.TypeOfTask == 1 && c.PostingDate <= enddate).Sum(c => c.TotalCost);
                        sumIssueExternal = JobLedgerNews.Where(c => c.JobNo != JobNo && c.JobMain == std.JobTaskCut && c.TypeOfTask == 3 && c.FromLocation == JobNo && c.PostingDate <= enddate).Sum(c => c.TotalCost) * 2;
                        sumReclassAPO = JobLedgerNews.Where(c => c.JobNo == JobNo && c.JobMain == std.JobTaskCut && c.TypeOfTask == 0 && c.PostingDate <= enddate && c.TotalCost == 0).Sum(c => c.Total);
                        CumBalAMT = (sumReclass + sumIssue + sumRental + sumReturnCal + sumReclassAPO) - (sumReturn + sumIssueExternal);
                        //current.CumBalAMT = CumBalAMT;


                        BudgetQTY = jobPlanningLines.Where(c => c.JobNo == JobNo && c.MainJobID == std.JobTaskCut).Sum(c => c.Quantity);
                        BudgetAMT = jobPlanningLines.Where(c => c.JobNo == JobNo && c.MainJobID == std.JobTaskCut).Sum(c => c.TotalCost);
                        if (BudgetQTY == 0)
                        {
                            PercenQTY = 0;
                        }
                        else
                        {
                            PercenQTY = (CumBalQTY / BudgetQTY) * 100;
                        }

                        if (BudgetAMT == 0)
                        {
                            PercenAMT = 0;
                        }
                        else
                        {
                            PercenAMT = (CumBalAMT / BudgetAMT) * 100;
                        }
                    }

                    worksheet2.Cells[r, 1].Value = std.DescriptionFull;
                    worksheet2.Cells[r, 2].Value = ThisPeriodQty;
                    worksheet2.Cells[r, 3].Value = ThisPeriodAMT;
                    worksheet2.Cells[r, 4].Value = OpenningQTY;
                    worksheet2.Cells[r, 5].Value = OpenningAMT;
                    worksheet2.Cells[r, 6].Value = CumBalQTY;
                    worksheet2.Cells[r, 7].Value = CumBalAMT;
                    worksheet2.Cells[r, 8].Value = BudgetQTY;
                    worksheet2.Cells[r, 9].Value = PercenQTY.ToString("#,###.00") + "%";
                    worksheet2.Cells[r, 10].Value = BudgetAMT;
                    worksheet2.Cells[r, 11].Value = PercenAMT.ToString("#,###.00") + "%";
                    r++;

                    if (std.MainJobID == "A0000")
                    {
                     
                        A00 = CumBalAMT;
                        ViewBag.A00 = SetFontRed(CumBalAMT, 1);
                        TCXI += CumBalAMT;

                        worksheet3.Cells[rl, 1].Value = std.DescriptionFull;
                        worksheet3.Cells[rl, 2].Value = ThisPeriodQty;
                        worksheet3.Cells[rl, 3].Value = ThisPeriodAMT;
                        worksheet3.Cells[rl, 4].Value = OpenningQTY;
                        worksheet3.Cells[rl, 5].Value = OpenningAMT;
                        worksheet3.Cells[rl, 6].Value = CumBalQTY;
                        worksheet3.Cells[rl, 7].Value = CumBalAMT;
                        worksheet3.Cells[rl, 8].Value = BudgetQTY;
                        worksheet3.Cells[rl, 9].Value =PercenQTY.ToString("#,###.00") + "%";
                        worksheet3.Cells[rl, 10].Value = BudgetAMT;
                        worksheet3.Cells[rl, 11].Value = PercenAMT.ToString("#,###.00") + "%";

                        thisQ += ThisPeriodQty;
                        thisA += ThisPeriodAMT;
                        openQ += OpenningQTY;
                        openA += OpenningAMT;
                        cumQ += CumBalQTY;
                        cumA += CumBalAMT;
                        budQ += BudgetQTY;
                        budA += BudgetAMT;

                        rl += 1;

                    }
                    if (std.MainJobID == "B0000")
                    {
                        //CurrentSubTotal.JobSub = "05";
                        B00 = CumBalAMT;
                        ViewBag.B00 = SetFontRed(CumBalAMT, 1);
                        TCXI += CumBalAMT;

                        worksheet3.Cells[rl, 1].Value = std.DescriptionFull;
                        worksheet3.Cells[rl, 2].Value = ThisPeriodQty;
                        worksheet3.Cells[rl, 3].Value = ThisPeriodAMT;
                        worksheet3.Cells[rl, 4].Value = OpenningQTY;
                        worksheet3.Cells[rl, 5].Value = OpenningAMT;
                        worksheet3.Cells[rl, 6].Value = CumBalQTY;
                        worksheet3.Cells[rl, 7].Value = CumBalAMT;
                        worksheet3.Cells[rl, 8].Value = BudgetQTY;
                        worksheet3.Cells[rl, 9].Value = PercenQTY.ToString("#,###.00") + "%";
                        worksheet3.Cells[rl, 10].Value = BudgetAMT;
                        worksheet3.Cells[rl, 11].Value = PercenAMT.ToString("#,###.00") + "%";

                        thisQ += ThisPeriodQty;
                        thisA += ThisPeriodAMT;
                        openQ += OpenningQTY;
                        openA += OpenningAMT;
                        cumQ += CumBalQTY;
                        cumA += CumBalAMT;
                        budQ += BudgetQTY;
                        budA += BudgetAMT;

                        rl += 1;


                    }
                    if (std.MainJobID == "C0000")
                    {
                        //CurrentSubTotal.JobSub = "06";
                        C00 = CumBalAMT;
                        ViewBag.C00 = SetFontRed(CumBalAMT, 1);
                        TCXI += CumBalAMT;

                        worksheet3.Cells[rl, 1].Value = std.DescriptionFull;
                        worksheet3.Cells[rl, 2].Value = ThisPeriodQty;
                        worksheet3.Cells[rl, 3].Value = ThisPeriodAMT;
                        worksheet3.Cells[rl, 4].Value = OpenningQTY;
                        worksheet3.Cells[rl, 5].Value = OpenningAMT;
                        worksheet3.Cells[rl, 6].Value = CumBalQTY;
                        worksheet3.Cells[rl, 7].Value = CumBalAMT;
                        worksheet3.Cells[rl, 8].Value = BudgetQTY;
                        worksheet3.Cells[rl, 9].Value = PercenQTY.ToString("#,###.00") + "%";
                        worksheet3.Cells[rl, 10].Value = BudgetAMT;
                        worksheet3.Cells[rl, 11].Value = PercenAMT.ToString("#,###.00") + "%";

                        thisQ += ThisPeriodQty;
                        thisA += ThisPeriodAMT;
                        openQ += OpenningQTY;
                        openA += OpenningAMT;
                        cumQ += CumBalQTY;
                        cumA += CumBalAMT;
                        budQ += BudgetQTY;
                        budA += BudgetAMT;

                        rl += 1;
                    }
                    if (std.MainJobID == "D0000")
                    {
                        //CurrentSubTotal.JobSub = "07";
                        D00 = CumBalAMT;
                        ViewBag.D00 = SetFontRed(CumBalAMT, 1);
                        TCXI += CumBalAMT;

                        worksheet3.Cells[rl, 1].Value = std.DescriptionFull;
                        worksheet3.Cells[rl, 2].Value = ThisPeriodQty;
                        worksheet3.Cells[rl, 3].Value = ThisPeriodAMT;
                        worksheet3.Cells[rl, 4].Value = OpenningQTY;
                        worksheet3.Cells[rl, 5].Value = OpenningAMT;
                        worksheet3.Cells[rl, 6].Value = CumBalQTY;
                        worksheet3.Cells[rl, 7].Value = CumBalAMT;
                        worksheet3.Cells[rl, 8].Value = BudgetQTY;
                        worksheet3.Cells[rl, 9].Value = PercenQTY.ToString("#,###.00") + "%";
                        worksheet3.Cells[rl, 10].Value = BudgetAMT;
                        worksheet3.Cells[rl, 11].Value = PercenAMT.ToString("#,###.00") + "%";

                        thisQ += ThisPeriodQty;
                        thisA += ThisPeriodAMT;
                        openQ += OpenningQTY;
                        openA += OpenningAMT;
                        cumQ += CumBalQTY;
                        cumA += CumBalAMT;
                        budQ += BudgetQTY;
                        budA += BudgetAMT;

                        rl += 1;
                    }
                    if (std.MainJobID == "E0000")
                    {
                        //CurrentSubTotal.JobSub = "08";
                        E00 = CumBalAMT;
                        ViewBag.E00 = SetFontRed(CumBalAMT, 1);
                        TCXI += CumBalAMT;

                        worksheet3.Cells[rl, 1].Value = std.DescriptionFull;
                        worksheet3.Cells[rl, 2].Value = ThisPeriodQty;
                        worksheet3.Cells[rl, 3].Value = ThisPeriodAMT;
                        worksheet3.Cells[rl, 4].Value = OpenningQTY;
                        worksheet3.Cells[rl, 5].Value = OpenningAMT;
                        worksheet3.Cells[rl, 6].Value = CumBalQTY;
                        worksheet3.Cells[rl, 7].Value = CumBalAMT;
                        worksheet3.Cells[rl, 8].Value = BudgetQTY;
                        worksheet3.Cells[rl, 9].Value = PercenQTY.ToString("#,###.00") + "%";
                        worksheet3.Cells[rl, 10].Value = BudgetAMT;
                        worksheet3.Cells[rl, 11].Value = PercenAMT.ToString("#,###.00") + "%";

                        thisQ += ThisPeriodQty;
                        thisA += ThisPeriodAMT;
                        openQ += OpenningQTY;
                        openA += OpenningAMT;
                        cumQ += CumBalQTY;
                        cumA += CumBalAMT;
                        budQ += BudgetQTY;
                        budA += BudgetAMT;

                        rl += 1;
                    }
                    if (std.MainJobID == "F0000")
                    {
                        //CurrentSubTotal.JobSub = "09";
                        F00 = CumBalAMT;
                        ViewBag.F00 = SetFontRed(CumBalAMT, 1);
                        TCXI += CumBalAMT;

                        worksheet3.Cells[rl, 1].Value = std.DescriptionFull;
                        worksheet3.Cells[rl, 2].Value = ThisPeriodQty;
                        worksheet3.Cells[rl, 3].Value = ThisPeriodAMT;
                        worksheet3.Cells[rl, 4].Value = OpenningQTY;
                        worksheet3.Cells[rl, 5].Value = OpenningAMT;
                        worksheet3.Cells[rl, 6].Value = CumBalQTY;
                        worksheet3.Cells[rl, 7].Value = CumBalAMT;
                        worksheet3.Cells[rl, 8].Value = BudgetQTY;
                        worksheet3.Cells[rl, 9].Value = PercenQTY.ToString("#,###.00") + "%"; 
                        worksheet3.Cells[rl, 10].Value = BudgetAMT;
                        worksheet3.Cells[rl, 11].Value = PercenAMT.ToString("#,###.00") + "%";

                        thisQ += ThisPeriodQty;
                        thisA += ThisPeriodAMT;
                        openQ += OpenningQTY;
                        openA += OpenningAMT;
                        cumQ += CumBalQTY;
                        cumA += CumBalAMT;
                        budQ += BudgetQTY;
                        budA += BudgetAMT;

                        rl += 1;
                    }

                    if (std.MainJobID == "G0000")
                    {
                        //CurrentSubTotal.JobSub = "10";
                        G00 = CumBalAMT;
                        ViewBag.G00 = SetFontRed(CumBalAMT, 1);
                        TCXI += CumBalAMT;

                        worksheet3.Cells[rl, 1].Value = std.DescriptionFull;
                        worksheet3.Cells[rl, 2].Value = ThisPeriodQty;
                        worksheet3.Cells[rl, 3].Value = ThisPeriodAMT;
                        worksheet3.Cells[rl, 4].Value = OpenningQTY;
                        worksheet3.Cells[rl, 5].Value = OpenningAMT;
                        worksheet3.Cells[rl, 6].Value = CumBalQTY;
                        worksheet3.Cells[rl, 7].Value = CumBalAMT;
                        worksheet3.Cells[rl, 8].Value = BudgetQTY;
                        worksheet3.Cells[rl, 9].Value = PercenQTY.ToString("#,###.00") + "%";
                        worksheet3.Cells[rl, 10].Value = BudgetAMT;
                        worksheet3.Cells[rl, 11].Value = PercenAMT.ToString("#,###.00") + "%";

                        thisQ += ThisPeriodQty;
                        thisA += ThisPeriodAMT;
                        openQ += OpenningQTY;
                        openA += OpenningAMT;
                        cumQ += CumBalQTY;
                        cumA += CumBalAMT;
                        budQ += BudgetQTY;
                        budA += BudgetAMT;

                        rl += 1;
                    }
                    if (std.MainJobID == "H0000")
                    {
                        //CurrentSubTotal.JobSub = "11";
                        H00 = CumBalAMT;
                        ViewBag.H00 = SetFontRed(CumBalAMT, 1);
                        TCXI += CumBalAMT;

                        worksheet3.Cells[rl, 1].Value = std.DescriptionFull;
                        worksheet3.Cells[rl, 2].Value = ThisPeriodQty;
                        worksheet3.Cells[rl, 3].Value = ThisPeriodAMT;
                        worksheet3.Cells[rl, 4].Value = OpenningQTY;
                        worksheet3.Cells[rl, 5].Value = OpenningAMT;
                        worksheet3.Cells[rl, 6].Value = CumBalQTY;
                        worksheet3.Cells[rl, 7].Value = CumBalAMT;
                        worksheet3.Cells[rl, 8].Value = BudgetQTY;
                        worksheet3.Cells[rl, 9].Value = PercenQTY.ToString("#,###.00") + "%";
                        worksheet3.Cells[rl, 10].Value = BudgetAMT;
                        worksheet3.Cells[rl, 11].Value = PercenAMT.ToString("#,###.00") + "%";

                        thisQ += ThisPeriodQty;
                        thisA += ThisPeriodAMT;
                        openQ += OpenningQTY;
                        openA += OpenningAMT;
                        cumQ += CumBalQTY;
                        cumA += CumBalAMT;
                        budQ += BudgetQTY;
                        budA += BudgetAMT;

                        rl += 1;
                    }
                    if (std.MainJobID == "N0000")
                    {
              
                        N00 = CumBalAMT;

                        worksheet3.Cells[3, 1].Value = std.DescriptionFull;
                        worksheet3.Cells[3, 2].Value = ThisPeriodQty;
                        worksheet3.Cells[3, 3].Value = ThisPeriodAMT;
                        worksheet3.Cells[3, 4].Value = OpenningQTY;
                        worksheet3.Cells[3, 5].Value = OpenningAMT;
                        worksheet3.Cells[3, 6].Value = CumBalQTY;
                        worksheet3.Cells[3, 7].Value = CumBalAMT;
                        worksheet3.Cells[3, 8].Value = BudgetQTY;
                        worksheet3.Cells[3, 9].Value = PercenQTY.ToString("#,###.00") + "%";
                        worksheet3.Cells[3, 10].Value = BudgetAMT;
                        worksheet3.Cells[3, 11].Value = PercenAMT.ToString("#,###.00") + "%";

                        OthisQ += ThisPeriodQty;
                        OthisA += ThisPeriodAMT;
                        OopenQ += OpenningQTY;
                        OopenA += OpenningAMT;
                        OcumQ += CumBalQTY;
                        OcumA += CumBalAMT;
                        ObudQ += BudgetQTY;
                        ObudA += BudgetAMT;



                    }

                    if (std.MainJobID == "O0000")
                    {
                        O00 = CumBalAMT;

                        worksheet3.Cells[4, 1].Value = std.DescriptionFull;
                        worksheet3.Cells[4, 2].Value = ThisPeriodQty;
                        worksheet3.Cells[4, 3].Value = ThisPeriodAMT;
                        worksheet3.Cells[4, 4].Value = OpenningQTY;
                        worksheet3.Cells[4, 5].Value = OpenningAMT;
                        worksheet3.Cells[4, 6].Value = CumBalQTY;
                        worksheet3.Cells[4, 7].Value = CumBalAMT;
                        worksheet3.Cells[4, 8].Value = BudgetQTY;
                        worksheet3.Cells[4, 9].Value = PercenQTY.ToString("#,###.00") + "%";
                        worksheet3.Cells[4, 10].Value = BudgetAMT;
                        worksheet3.Cells[4, 11].Value = PercenAMT.ToString("#,###.00") + "%";

                        OthisQ += ThisPeriodQty;
                        OthisA += ThisPeriodAMT;
                        OopenQ += OpenningQTY;
                        OopenA += OpenningAMT;
                        OcumQ += CumBalQTY;
                        OcumA += CumBalAMT;
                        ObudQ += BudgetQTY;
                        ObudA += BudgetAMT;



                    }


                }
                else if (std.JobTaskCut == std.SubJobID) //ถ้าเป็น Sub ให้ทำ
                {

                    if (std.JobstringCust == "O" || std.JobstringCust == "N")
                    {
                        sumReclass = 0;
                        sumIssue = 0;
                        sumRental = 0;
                        sumReturnCal = 0;
                        sumReturn = 0;
                        sumIssueExternal = 0;
                        sumReclass = JobLedgerNews.Where(c => c.JobNo == JobNo && c.JobSub == std.JobTaskCut && c.TypeOfTask == 0 && c.PostingDate >= startdate && c.PostingDate <= enddate).Sum(c => c.Quantity);
                        sumIssue = JobLedgerNews.Where(c => c.JobNo == JobNo && c.JobSub == std.JobTaskCut && c.TypeOfTask == 3 && c.PostingDate >= startdate && c.PostingDate <= enddate).Sum(c => c.Quantity);
                        sumRental = JobLedgerNews.Where(c => c.JobNo == JobNo && c.JobSub == std.JobTaskCut && c.TypeOfTask == 2 && c.PostingDate >= startdate && c.PostingDate <= enddate).Sum(c => c.Quantity);
                        sumReturnCal = JobLedgerNews.Where(c => c.JobNo == JobNo && c.JobSub == std.JobTaskCut && c.TypeOfTask == 1 && c.Document3 == "CAL" && c.PostingDate >= startdate && c.PostingDate <= enddate).Sum(c => c.Quantity) * 2;
                        sumReturn = JobLedgerNews.Where(c => c.JobNo == JobNo && c.JobSub == std.JobTaskCut && c.TypeOfTask == 1 && c.PostingDate >= startdate && c.PostingDate <= enddate).Sum(c => c.Quantity);
                        sumIssueExternal = JobLedgerNews.Where(c => c.JobNo != JobNo && c.JobSub == std.JobTaskCut && c.TypeOfTask == 3 && c.PostingDate >= startdate && c.FromLocation == JobNo && c.PostingDate <= enddate).Sum(c => c.Quantity) * 2;
                        ThisPeriodQty = (sumReclass + sumIssue + sumRental + sumReturnCal) - (sumReturn + sumIssueExternal);
                        //current.ThisPeriodQty = ThisPeriodQty;

                        sumReclass = 0;
                        sumIssue = 0;
                        sumRental = 0;
                        sumReturnCal = 0;
                        sumReturn = 0;
                        sumIssueExternal = 0;
                        sumReclass = JobLedgerNews.Where(c => c.JobNo == JobNo && c.JobSub == std.JobTaskCut && c.TypeOfTask == 0 && c.PostingDate >= startdate && c.PostingDate <= enddate).Sum(c => c.TotalCost + c.LineAmount);
                        sumIssue = JobLedgerNews.Where(c => c.JobNo == JobNo && c.JobSub == std.JobTaskCut && c.TypeOfTask == 3 && c.PostingDate >= startdate && c.PostingDate <= enddate).Sum(c => c.TotalCost + c.LineAmount);
                        sumRental = JobLedgerNews.Where(c => c.JobNo == JobNo && c.JobSub == std.JobTaskCut && c.TypeOfTask == 2 && c.PostingDate >= startdate && c.PostingDate <= enddate).Sum(c => c.TotalCost + c.LineAmount);
                        sumReturnCal = JobLedgerNews.Where(c => c.JobNo == JobNo && c.JobSub == std.JobTaskCut && c.TypeOfTask == 1 && c.Document3 == "CAL" && c.PostingDate >= startdate && c.PostingDate <= enddate).Sum(c => c.TotalCost + c.LineAmount) * 2;
                        sumReturn = JobLedgerNews.Where(c => c.JobNo == JobNo && c.JobSub == std.JobTaskCut && c.TypeOfTask == 1 && c.PostingDate >= startdate && c.PostingDate <= enddate).Sum(c => c.TotalCost + c.LineAmount);
                        sumIssueExternal = JobLedgerNews.Where(c => c.JobNo != JobNo && c.JobSub == std.JobTaskCut && c.TypeOfTask == 3 && c.FromLocation == JobNo && c.PostingDate >= startdate && c.PostingDate <= enddate).Sum(c => c.TotalCost + c.LineAmount) * 2;
                        sumReclassAPO = JobLedgerNews.Where(c => c.JobNo != JobNo && c.JobSub == std.JobTaskCut && c.TypeOfTask == 0 && c.PostingDate >= startdate && c.PostingDate <= enddate && c.TotalCost == 0).Sum(c => c.Total);
                        ThisPeriodAMT = (sumReclass + sumIssue + sumRental + sumReturnCal + sumReclassAPO) - (sumReturn + sumIssueExternal);
                        //current.ThisPeriodAMT = ThisPeriodAMT;

                        sumReclass = 0;
                        sumIssue = 0;
                        sumRental = 0;
                        sumReturnCal = 0;
                        sumReturn = 0;
                        sumIssueExternal = 0;
                        sumReclass = JobLedgerNews.Where(c => c.JobNo == JobNo && c.JobSub == std.JobTaskCut && c.TypeOfTask == 0 && c.PostingDate < startdate).Sum(c => c.Quantity);
                        sumIssue = JobLedgerNews.Where(c => c.JobNo == JobNo && c.JobSub == std.JobTaskCut && c.TypeOfTask == 3 && c.PostingDate < startdate).Sum(c => c.Quantity);
                        sumRental = JobLedgerNews.Where(c => c.JobNo == JobNo && c.JobSub == std.JobTaskCut && c.TypeOfTask == 2 && c.PostingDate < startdate).Sum(c => c.Quantity);
                        sumReturnCal = JobLedgerNews.Where(c => c.JobNo == JobNo && c.JobSub == std.JobTaskCut && c.TypeOfTask == 1 && c.Document3 == "CAL" && c.PostingDate < startdate).Sum(c => c.Quantity) * 2;
                        sumReturn = JobLedgerNews.Where(c => c.JobNo == JobNo && c.JobSub == std.JobTaskCut && c.TypeOfTask == 1 && c.PostingDate < startdate).Sum(c => c.Quantity);
                        sumIssueExternal = JobLedgerNews.Where(c => c.JobNo != JobNo && c.JobSub == std.JobTaskCut && c.TypeOfTask == 3 && c.PostingDate < startdate && c.FromLocation == JobNo).Sum(c => c.Quantity) * 2;
                        OpenningQTY = (sumReclass + sumIssue + sumRental + sumReturnCal) - (sumReturn + sumIssueExternal);
                        //current.OpenningQTY = OpenningQTY;


                        sumReclass = 0;
                        sumIssue = 0;
                        sumRental = 0;
                        sumReturnCal = 0;
                        sumReturn = 0;
                        sumIssueExternal = 0;
                        sumReclass = JobLedgerNews.Where(c => c.JobNo == JobNo && c.JobSub == std.JobTaskCut && c.TypeOfTask == 0 && c.PostingDate < startdate).Sum(c => c.TotalCost + c.LineAmount);
                        sumIssue = JobLedgerNews.Where(c => c.JobNo == JobNo && c.JobSub == std.JobTaskCut && c.TypeOfTask == 3 && c.PostingDate < startdate).Sum(c => c.TotalCost + c.LineAmount);
                        sumRental = JobLedgerNews.Where(c => c.JobNo == JobNo && c.JobSub == std.JobTaskCut && c.TypeOfTask == 2 && c.PostingDate < startdate).Sum(c => c.TotalCost + c.LineAmount);
                        sumReturnCal = JobLedgerNews.Where(c => c.JobNo == JobNo && c.JobSub == std.JobTaskCut && c.TypeOfTask == 1 && c.Document3 == "CAL" && c.PostingDate < startdate).Sum(c => c.TotalCost + c.LineAmount) * 2;
                        sumReturn = JobLedgerNews.Where(c => c.JobNo == JobNo && c.JobSub == std.JobTaskCut && c.TypeOfTask == 1 && c.PostingDate < startdate).Sum(c => c.TotalCost + c.LineAmount);
                        sumIssueExternal = JobLedgerNews.Where(c => c.JobNo != JobNo && c.JobSub == std.JobTaskCut && c.TypeOfTask == 3 && c.FromLocation == JobNo && c.PostingDate < startdate).Sum(c => c.TotalCost + c.LineAmount) * 2;
                        sumReclassAPO = JobLedgerNews.Where(c => c.JobNo != JobNo && c.JobSub == std.JobTaskCut && c.TypeOfTask == 0 && c.PostingDate < startdate && c.TotalCost == 0).Sum(c => c.Total);
                        OpenningAMT = (sumReclass + sumIssue + sumRental + sumReturnCal + sumReclassAPO) - (sumReturn + sumIssueExternal);
                        //current.OpenningAMT = OpenningAMT;


                        sumReclass = 0;
                        sumIssue = 0;
                        sumRental = 0;
                        sumReturnCal = 0;
                        sumReturn = 0;
                        sumIssueExternal = 0;
                        sumReclass = JobLedgerNews.Where(c => c.JobNo == JobNo && c.JobSub == std.JobTaskCut && c.TypeOfTask == 0 && c.PostingDate <= enddate).Sum(c => c.Quantity);
                        sumIssue = JobLedgerNews.Where(c => c.JobNo == JobNo && c.JobSub == std.JobTaskCut && c.TypeOfTask == 3 && c.PostingDate <= enddate).Sum(c => c.Quantity);
                        sumRental = JobLedgerNews.Where(c => c.JobNo == JobNo && c.JobSub == std.JobTaskCut && c.TypeOfTask == 2 && c.PostingDate <= enddate).Sum(c => c.Quantity);
                        sumReturnCal = JobLedgerNews.Where(c => c.JobNo == JobNo && c.JobSub == std.JobTaskCut && c.TypeOfTask == 1 && c.Document3 == "CAL" && c.PostingDate <= enddate).Sum(c => c.Quantity) * 2;
                        sumReturn = JobLedgerNews.Where(c => c.JobNo == JobNo && c.JobSub == std.JobTaskCut && c.TypeOfTask == 1 && c.PostingDate <= enddate).Sum(c => c.Quantity);
                        sumIssueExternal = JobLedgerNews.Where(c => c.JobNo != JobNo && c.JobSub == std.JobTaskCut && c.TypeOfTask == 3 && c.PostingDate <= enddate && c.FromLocation == JobNo).Sum(c => c.Quantity) * 2;
                        CumBalQTY = (sumReclass + sumIssue + sumRental + sumReturnCal) - (sumReturn + sumIssueExternal);
                        //current.CumBalQTY = CumBalQTY;


                        sumReclass = 0;
                        sumIssue = 0;
                        sumRental = 0;
                        sumReturnCal = 0;
                        sumReturn = 0;
                        sumIssueExternal = 0;
                        sumReclass = JobLedgerNews.Where(c => c.JobNo == JobNo && c.JobSub == std.JobTaskCut && c.TypeOfTask == 0 && c.PostingDate <= enddate).Sum(c => c.TotalCost + c.LineAmount);
                        sumIssue = JobLedgerNews.Where(c => c.JobNo == JobNo && c.JobSub == std.JobTaskCut && c.TypeOfTask == 3 && c.PostingDate <= enddate).Sum(c => c.TotalCost + c.LineAmount);
                        sumRental = JobLedgerNews.Where(c => c.JobNo == JobNo && c.JobSub == std.JobTaskCut && c.TypeOfTask == 2 && c.PostingDate <= enddate).Sum(c => c.TotalCost + c.LineAmount);
                        sumReturnCal = JobLedgerNews.Where(c => c.JobNo == JobNo && c.JobSub == std.JobTaskCut && c.TypeOfTask == 1 && c.Document3 == "CAL" && c.PostingDate <= enddate).Sum(c => c.TotalCost + c.LineAmount) * 2;
                        sumReturn = JobLedgerNews.Where(c => c.JobNo == JobNo && c.JobSub == std.JobTaskCut && c.TypeOfTask == 1 && c.PostingDate <= enddate).Sum(c => c.TotalCost + c.LineAmount);
                        sumIssueExternal = JobLedgerNews.Where(c => c.JobNo != JobNo && c.JobSub == std.JobTaskCut && c.TypeOfTask == 3 && c.FromLocation == JobNo && c.PostingDate <= enddate).Sum(c => c.TotalCost + c.LineAmount) * 2;
                        sumReclassAPO = JobLedgerNews.Where(c => c.JobNo != JobNo && c.JobSub == std.JobTaskCut && c.TypeOfTask == 0 && c.PostingDate <= enddate && c.TotalCost == 0).Sum(c => c.Total);
                        CumBalAMT = (sumReclass + sumIssue + sumRental + sumReturnCal + sumReclassAPO) - (sumReturn + sumIssueExternal);
                        //current.CumBalAMT = CumBalAMT;



                        BudgetQTY = jobPlanningLines.Where(c => c.JobNo == JobNo && c.SubJobID == std.JobTaskCut).Sum(c => c.Quantity);
                        BudgetAMT = jobPlanningLines.Where(c => c.JobNo == JobNo && c.SubJobID == std.JobTaskCut).Sum(c => c.TotalCost);
                        if (BudgetQTY == 0)
                        {
                            PercenQTY = 0;
                        }
                        else
                        {
                            PercenQTY = (CumBalQTY / BudgetQTY) * 100;
                        }

                        if (BudgetAMT == 0)
                        {
                            PercenAMT = 0;
                        }
                        else
                        {
                            PercenAMT = (CumBalAMT / BudgetAMT) * 100;
                        }

                        //current.BudgetQTY = BudgetQTY;
                        //current.BudgetAMT = BudgetAMT;
                        //current.PercenAMT = PercenAMT;
                        //current.PercenQTY = PercenQTY;

                    }
                    else
                    {
                        sumReclass = 0;
                        sumIssue = 0;
                        sumRental = 0;
                        sumReturnCal = 0;
                        sumReturn = 0;
                        sumIssueExternal = 0;
                        sumReclass = JobLedgerNews.Where(c => c.JobNo == JobNo && c.JobSub == std.JobTaskCut && c.TypeOfTask == 0 && c.PostingDate >= startdate && c.PostingDate <= enddate).Sum(c => c.Quantity);
                        sumIssue = JobLedgerNews.Where(c => c.JobNo == JobNo && c.JobSub == std.JobTaskCut && c.TypeOfTask == 3 && c.PostingDate >= startdate && c.PostingDate <= enddate).Sum(c => c.Quantity);
                        sumRental = JobLedgerNews.Where(c => c.JobNo == JobNo && c.JobSub == std.JobTaskCut && c.TypeOfTask == 2 && c.PostingDate >= startdate && c.PostingDate <= enddate).Sum(c => c.Quantity);
                        sumReturnCal = JobLedgerNews.Where(c => c.JobNo == JobNo && c.JobSub == std.JobTaskCut && c.TypeOfTask == 1 && c.Document3 == "CAL" && c.PostingDate >= startdate && c.PostingDate <= enddate).Sum(c => c.Quantity) * 2;
                        sumReturn = JobLedgerNews.Where(c => c.JobNo == JobNo && c.JobSub == std.JobTaskCut && c.TypeOfTask == 1 && c.PostingDate >= startdate && c.PostingDate <= enddate).Sum(c => c.Quantity);
                        sumIssueExternal = JobLedgerNews.Where(c => c.JobNo != JobNo && c.JobSub == std.JobTaskCut && c.TypeOfTask == 3 && c.PostingDate >= startdate && c.FromLocation == JobNo && c.PostingDate <= enddate).Sum(c => c.Quantity) * 2;
                        ThisPeriodQty = (sumReclass + sumIssue + sumRental + sumReturnCal) - (sumReturn + sumIssueExternal);
                        //current.ThisPeriodQty = ThisPeriodQty;

                        sumReclass = 0;
                        sumIssue = 0;
                        sumRental = 0;
                        sumReturnCal = 0;
                        sumReturn = 0;
                        sumIssueExternal = 0;
                        sumReclass = JobLedgerNews.Where(c => c.JobNo == JobNo && c.JobSub == std.JobTaskCut && c.TypeOfTask == 0 && c.PostingDate >= startdate && c.PostingDate <= enddate).Sum(c => c.TotalCost);
                        sumIssue = JobLedgerNews.Where(c => c.JobNo == JobNo && c.JobSub == std.JobTaskCut && c.TypeOfTask == 3 && c.PostingDate >= startdate && c.PostingDate <= enddate).Sum(c => c.TotalCost);
                        sumRental = JobLedgerNews.Where(c => c.JobNo == JobNo && c.JobSub == std.JobTaskCut && c.TypeOfTask == 2 && c.PostingDate >= startdate && c.PostingDate <= enddate).Sum(c => c.TotalCost);
                        sumReturnCal = JobLedgerNews.Where(c => c.JobNo == JobNo && c.JobSub == std.JobTaskCut && c.TypeOfTask == 1 && c.Document3 == "CAL" && c.PostingDate >= startdate && c.PostingDate <= enddate).Sum(c => c.TotalCost) * 2;
                        sumReturn = JobLedgerNews.Where(c => c.JobNo == JobNo && c.JobSub == std.JobTaskCut && c.TypeOfTask == 1 && c.PostingDate >= startdate && c.PostingDate <= enddate).Sum(c => c.TotalCost);
                        sumIssueExternal = JobLedgerNews.Where(c => c.JobNo != JobNo && c.JobSub == std.JobTaskCut && c.TypeOfTask == 3 && c.FromLocation == JobNo && c.PostingDate >= startdate && c.PostingDate <= enddate).Sum(c => c.TotalCost) * 2;
                        sumReclassAPO = JobLedgerNews.Where(c => c.JobNo != JobNo && c.JobSub == std.JobTaskCut && c.TypeOfTask == 0 && c.PostingDate >= startdate && c.PostingDate <= enddate && c.TotalCost == 0).Sum(c => c.Total);
                        ThisPeriodAMT = (sumReclass + sumIssue + sumRental + sumReturnCal + sumReclassAPO) - (sumReturn + sumIssueExternal);
                        //current.ThisPeriodAMT = ThisPeriodAMT;

                        sumReclass = 0;
                        sumIssue = 0;
                        sumRental = 0;
                        sumReturnCal = 0;
                        sumReturn = 0;
                        sumIssueExternal = 0;
                        sumReclass = JobLedgerNews.Where(c => c.JobNo == JobNo && c.JobSub == std.JobTaskCut && c.TypeOfTask == 0 && c.PostingDate < startdate).Sum(c => c.Quantity);
                        sumIssue = JobLedgerNews.Where(c => c.JobNo == JobNo && c.JobSub == std.JobTaskCut && c.TypeOfTask == 3 && c.PostingDate < startdate).Sum(c => c.Quantity);
                        sumRental = JobLedgerNews.Where(c => c.JobNo == JobNo && c.JobSub == std.JobTaskCut && c.TypeOfTask == 2 && c.PostingDate < startdate).Sum(c => c.Quantity);
                        sumReturnCal = JobLedgerNews.Where(c => c.JobNo == JobNo && c.JobSub == std.JobTaskCut && c.TypeOfTask == 1 && c.Document3 == "CAL" && c.PostingDate < startdate).Sum(c => c.Quantity) * 2;
                        sumReturn = JobLedgerNews.Where(c => c.JobNo == JobNo && c.JobSub == std.JobTaskCut && c.TypeOfTask == 1 && c.PostingDate < startdate).Sum(c => c.Quantity);
                        sumIssueExternal = JobLedgerNews.Where(c => c.JobNo != JobNo && c.JobSub == std.JobTaskCut && c.TypeOfTask == 3 && c.PostingDate < startdate && c.FromLocation == JobNo).Sum(c => c.Quantity) * 2;
                        OpenningQTY = (sumReclass + sumIssue + sumRental + sumReturnCal) - (sumReturn + sumIssueExternal);
                        //current.OpenningQTY = OpenningQTY;


                        sumReclass = 0;
                        sumIssue = 0;
                        sumRental = 0;
                        sumReturnCal = 0;
                        sumReturn = 0;
                        sumIssueExternal = 0;
                        sumReclass = JobLedgerNews.Where(c => c.JobNo == JobNo && c.JobSub == std.JobTaskCut && c.TypeOfTask == 0 && c.PostingDate < startdate).Sum(c => c.TotalCost);
                        sumIssue = JobLedgerNews.Where(c => c.JobNo == JobNo && c.JobSub == std.JobTaskCut && c.TypeOfTask == 3 && c.PostingDate < startdate).Sum(c => c.TotalCost);
                        sumRental = JobLedgerNews.Where(c => c.JobNo == JobNo && c.JobSub == std.JobTaskCut && c.TypeOfTask == 2 && c.PostingDate < startdate).Sum(c => c.TotalCost);
                        sumReturnCal = JobLedgerNews.Where(c => c.JobNo == JobNo && c.JobSub == std.JobTaskCut && c.TypeOfTask == 1 && c.Document3 == "CAL" && c.PostingDate < startdate).Sum(c => c.TotalCost) * 2;
                        sumReturn = JobLedgerNews.Where(c => c.JobNo == JobNo && c.JobSub == std.JobTaskCut && c.TypeOfTask == 1 && c.PostingDate < startdate).Sum(c => c.TotalCost);
                        sumIssueExternal = JobLedgerNews.Where(c => c.JobNo != JobNo && c.JobSub == std.JobTaskCut && c.TypeOfTask == 3 && c.FromLocation == JobNo && c.PostingDate < startdate).Sum(c => c.TotalCost) * 2;
                        sumReclassAPO = JobLedgerNews.Where(c => c.JobNo != JobNo && c.JobSub == std.JobTaskCut && c.TypeOfTask == 0 && c.PostingDate < startdate && c.TotalCost == 0).Sum(c => c.Total);
                        OpenningAMT = (sumReclass + sumIssue + sumRental + sumReturnCal + sumReclassAPO) - (sumReturn + sumIssueExternal);
                        //current.OpenningAMT = OpenningAMT;

                        sumReclass = 0;
                        sumIssue = 0;
                        sumRental = 0;
                        sumReturnCal = 0;
                        sumReturn = 0;
                        sumIssueExternal = 0;
                        sumReclass = JobLedgerNews.Where(c => c.JobNo == JobNo && c.JobSub == std.JobTaskCut && c.TypeOfTask == 0 && c.PostingDate <= enddate).Sum(c => c.Quantity);
                        sumIssue = JobLedgerNews.Where(c => c.JobNo == JobNo && c.JobSub == std.JobTaskCut && c.TypeOfTask == 3 && c.PostingDate <= enddate).Sum(c => c.Quantity);
                        sumRental = JobLedgerNews.Where(c => c.JobNo == JobNo && c.JobSub == std.JobTaskCut && c.TypeOfTask == 2 && c.PostingDate <= enddate).Sum(c => c.Quantity);
                        sumReturnCal = JobLedgerNews.Where(c => c.JobNo == JobNo && c.JobSub == std.JobTaskCut && c.TypeOfTask == 1 && c.Document3 == "CAL" && c.PostingDate <= enddate).Sum(c => c.Quantity) * 2;
                        sumReturn = JobLedgerNews.Where(c => c.JobNo == JobNo && c.JobSub == std.JobTaskCut && c.TypeOfTask == 1 && c.PostingDate <= enddate).Sum(c => c.Quantity);
                        sumIssueExternal = JobLedgerNews.Where(c => c.JobNo != JobNo && c.JobSub == std.JobTaskCut && c.TypeOfTask == 3 && c.PostingDate <= enddate && c.FromLocation == JobNo).Sum(c => c.Quantity) * 2;
                        CumBalQTY = (sumReclass + sumIssue + sumRental + sumReturnCal) - (sumReturn + sumIssueExternal);
                        //current.CumBalQTY = CumBalQTY;

                        sumReclass = 0;
                        sumIssue = 0;
                        sumRental = 0;
                        sumReturnCal = 0;
                        sumReturn = 0;
                        sumIssueExternal = 0;
                        sumReclass = JobLedgerNews.Where(c => c.JobNo == JobNo && c.JobSub == std.JobTaskCut && c.TypeOfTask == 0 && c.PostingDate <= enddate).Sum(c => c.TotalCost);
                        sumIssue = JobLedgerNews.Where(c => c.JobNo == JobNo && c.JobSub == std.JobTaskCut && c.TypeOfTask == 3 && c.PostingDate <= enddate).Sum(c => c.TotalCost);
                        sumRental = JobLedgerNews.Where(c => c.JobNo == JobNo && c.JobSub == std.JobTaskCut && c.TypeOfTask == 2 && c.PostingDate <= enddate).Sum(c => c.TotalCost);
                        sumReturnCal = JobLedgerNews.Where(c => c.JobNo == JobNo && c.JobSub == std.JobTaskCut && c.TypeOfTask == 1 && c.Document3 == "CAL" && c.PostingDate <= enddate).Sum(c => c.TotalCost) * 2;
                        sumReturn = JobLedgerNews.Where(c => c.JobNo == JobNo && c.JobSub == std.JobTaskCut && c.TypeOfTask == 1 && c.PostingDate <= enddate).Sum(c => c.TotalCost);
                        sumIssueExternal = JobLedgerNews.Where(c => c.JobNo != JobNo && c.JobSub == std.JobTaskCut && c.TypeOfTask == 3 && c.FromLocation == JobNo && c.PostingDate <= enddate).Sum(c => c.TotalCost) * 2;
                        sumReclassAPO = JobLedgerNews.Where(c => c.JobNo != JobNo && c.JobSub == std.JobTaskCut && c.TypeOfTask == 0 && c.PostingDate <= enddate && c.TotalCost == 0).Sum(c => c.Total);
                        CumBalAMT = (sumReclass + sumIssue + sumRental + sumReturnCal + sumReclassAPO) - (sumReturn + sumIssueExternal);
                        //current.CumBalAMT = CumBalAMT;


                        BudgetQTY = jobPlanningLines.Where(c => c.JobNo == JobNo && c.SubJobID == std.JobTaskCut).Sum(c => c.Quantity);
                        BudgetAMT = jobPlanningLines.Where(c => c.JobNo == JobNo && c.SubJobID == std.JobTaskCut).Sum(c => c.TotalCost);
                        if (BudgetQTY == 0)
                        {
                            PercenQTY = 0;
                        }
                        else
                        {
                            PercenQTY = (CumBalQTY / BudgetQTY) * 100;
                        }

                        if (BudgetAMT == 0)
                        {
                            PercenAMT = 0;
                        }
                        else
                        {
                            PercenAMT = (CumBalAMT / BudgetAMT) * 100;
                        }

                        //current.BudgetQTY = BudgetQTY;
                        //current.BudgetAMT = BudgetAMT;
                        //current.PercenAMT = PercenAMT;
                        //current.PercenQTY = PercenQTY;

                    }
                    worksheet2.Cells[r, 1].Value = std.DescriptionFull;
                    worksheet2.Cells[r, 2].Value = ThisPeriodQty;
                    worksheet2.Cells[r, 3].Value = ThisPeriodAMT;
                    worksheet2.Cells[r, 4].Value = OpenningQTY;
                    worksheet2.Cells[r, 5].Value = OpenningAMT;
                    worksheet2.Cells[r, 6].Value = CumBalQTY;
                    worksheet2.Cells[r, 7].Value = CumBalAMT;
                    worksheet2.Cells[r, 8].Value = BudgetQTY;
                    worksheet2.Cells[r, 9].Value = PercenQTY.ToString("#,###.00") + "%";
                    worksheet2.Cells[r, 10].Value = BudgetAMT;
                    worksheet2.Cells[r, 11].Value = PercenAMT.ToString("#,###.00") + "%";
                    r++;


                    if (std.JobTaskNo == "D2100")
                    {
                        D2100 = CumBalAMT;
                        ViewBag.D2100 = SetFontRed(CumBalAMT, 1);
                    }

                    if (std.JobTaskNo == "E0800")
                    {
                        E0800 = CumBalAMT;
                        ViewBag.E0800 = SetFontRed(CumBalAMT, 1);
                    }
                    if (std.JobTaskNo == "H0400")
                    {
                        H0400 = CumBalAMT;
                        ViewBag.H0400 = SetFontRed(CumBalAMT, 1);
                    }
                    if (std.JobTaskNo == "H0200")
                    {
                        H0200 = CumBalAMT;
                        ViewBag.H0200 = SetFontRed(CumBalAMT, 1);
                    }
                    if (std.JobTaskNo == "H0800")
                    {
                        H0800 = CumBalAMT;
                        ViewBag.H0800 = SetFontRed(CumBalAMT, 1);
                    }
                    if (std.JobTaskNo == "H0600")
                    {
                        H0600 = CumBalAMT;
                        ViewBag.H0600 = SetFontRed(CumBalAMT, 1);
                    }


                }
                else  //ถ้าเป็นทั่วไปให้ทำ
                {


                    if (std.JobstringCust == "O" || std.JobstringCust == "N")
                    {
                        sumReclass = 0;
                        sumIssue = 0;
                        sumRental = 0;
                        sumReturnCal = 0;
                        sumReturn = 0;
                        sumIssueExternal = 0;
                        sumReclass = JobLedgerNews.Where(c => c.JobNo == JobNo && c.JobLedgerEntry == std.JobTaskCut && c.TypeOfTask == 0 && c.PostingDate >= startdate && c.PostingDate <= enddate).Sum(c => c.Quantity);
                        sumIssue = JobLedgerNews.Where(c => c.JobNo == JobNo && c.JobLedgerEntry == std.JobTaskCut && c.TypeOfTask == 3 && c.PostingDate >= startdate && c.PostingDate <= enddate).Sum(c => c.Quantity);
                        sumRental = JobLedgerNews.Where(c => c.JobNo == JobNo && c.JobLedgerEntry == std.JobTaskCut && c.TypeOfTask == 2 && c.PostingDate >= startdate && c.PostingDate <= enddate).Sum(c => c.Quantity);
                        sumReturnCal = JobLedgerNews.Where(c => c.JobNo == JobNo && c.JobLedgerEntry == std.JobTaskCut && c.TypeOfTask == 1 && c.Document3 == "CAL" && c.PostingDate >= startdate && c.PostingDate <= enddate).Sum(c => c.Quantity) * 2;
                        sumReturn = JobLedgerNews.Where(c => c.JobNo == JobNo && c.JobLedgerEntry == std.JobTaskCut && c.TypeOfTask == 1 && c.PostingDate >= startdate && c.PostingDate <= enddate).Sum(c => c.Quantity);
                        sumIssueExternal = JobLedgerNews.Where(c => c.JobNo != JobNo && c.JobLedgerEntry == std.JobTaskCut && c.TypeOfTask == 3 && c.PostingDate >= startdate && c.FromLocation == JobNo && c.PostingDate <= enddate).Sum(c => c.Quantity) * 2;

                        ThisPeriodQty = (sumReclass + sumIssue + sumRental + sumReturnCal) - (sumReturn + sumIssueExternal);
                        //current.ThisPeriodQty = ThisPeriodQty;

                        sumReclass = 0;
                        sumIssue = 0;
                        sumRental = 0;
                        sumReturnCal = 0;
                        sumReturn = 0;
                        sumIssueExternal = 0;
                        sumReclass = JobLedgerNews.Where(c => c.JobNo == JobNo && c.JobLedgerEntry == std.JobTaskCut && c.TypeOfTask == 0 && c.PostingDate >= startdate && c.PostingDate <= enddate).Sum(c => c.TotalCost + c.LineAmount);
                        sumIssue = JobLedgerNews.Where(c => c.JobNo == JobNo && c.JobLedgerEntry == std.JobTaskCut && c.TypeOfTask == 3 && c.PostingDate >= startdate && c.PostingDate <= enddate).Sum(c => c.TotalCost + c.LineAmount);
                        sumRental = JobLedgerNews.Where(c => c.JobNo == JobNo && c.JobLedgerEntry == std.JobTaskCut && c.TypeOfTask == 2 && c.PostingDate >= startdate && c.PostingDate <= enddate).Sum(c => c.TotalCost + c.LineAmount);
                        sumReturnCal = JobLedgerNews.Where(c => c.JobNo == JobNo && c.JobLedgerEntry == std.JobTaskCut && c.TypeOfTask == 1 && c.Document3 == "CAL" && c.PostingDate >= startdate && c.PostingDate <= enddate).Sum(c => c.TotalCost + c.LineAmount) * 2;
                        sumReturn = JobLedgerNews.Where(c => c.JobNo == JobNo && c.JobLedgerEntry == std.JobTaskCut && c.TypeOfTask == 1 && c.PostingDate >= startdate && c.PostingDate <= enddate).Sum(c => c.TotalCost + c.LineAmount);
                        sumIssueExternal = JobLedgerNews.Where(c => c.JobNo != JobNo && c.JobLedgerEntry == std.JobTaskCut && c.TypeOfTask == 3 && c.FromLocation == JobNo && c.PostingDate >= startdate && c.PostingDate <= enddate).Sum(c => c.TotalCost + c.LineAmount) * 2;
                        sumReclassAPO = JobLedgerNews.Where(c => c.JobNo != JobNo && c.JobLedgerEntry == std.JobTaskCut && c.TypeOfTask == 0 && c.PostingDate >= startdate && c.PostingDate <= enddate && c.TotalCost == 0).Sum(c => c.Total);
                        ThisPeriodAMT = (sumReclass + sumIssue + sumRental + sumReturnCal + sumReclassAPO) - (sumReturn + sumIssueExternal);
                        //current.ThisPeriodAMT = ThisPeriodAMT;

                        sumReclass = 0;
                        sumIssue = 0;
                        sumRental = 0;
                        sumReturnCal = 0;
                        sumReturn = 0;
                        sumIssueExternal = 0;
                        sumReclass = JobLedgerNews.Where(c => c.JobNo == JobNo && c.JobLedgerEntry == std.JobTaskCut && c.TypeOfTask == 0 && c.PostingDate < startdate).Sum(c => c.Quantity);
                        sumIssue = JobLedgerNews.Where(c => c.JobNo == JobNo && c.JobLedgerEntry == std.JobTaskCut && c.TypeOfTask == 3 && c.PostingDate < startdate).Sum(c => c.Quantity);
                        sumRental = JobLedgerNews.Where(c => c.JobNo == JobNo && c.JobLedgerEntry == std.JobTaskCut && c.TypeOfTask == 2 && c.PostingDate < startdate).Sum(c => c.Quantity);
                        sumReturnCal = JobLedgerNews.Where(c => c.JobNo == JobNo && c.JobLedgerEntry == std.JobTaskCut && c.TypeOfTask == 1 && c.Document3 == "CAL" && c.PostingDate < startdate).Sum(c => c.Quantity) * 2;
                        sumReturn = JobLedgerNews.Where(c => c.JobNo == JobNo && c.JobLedgerEntry == std.JobTaskCut && c.TypeOfTask == 1 && c.PostingDate < startdate).Sum(c => c.Quantity);
                        sumIssueExternal = JobLedgerNews.Where(c => c.JobNo != JobNo && c.JobLedgerEntry == std.JobTaskCut && c.TypeOfTask == 3 && c.PostingDate < startdate && c.FromLocation == JobNo).Sum(c => c.Quantity) * 2;
                        OpenningQTY = (sumReclass + sumIssue + sumRental + sumReturnCal) - (sumReturn + sumIssueExternal);
                        //current.OpenningQTY = OpenningQTY;

                        sumReclass = 0;
                        sumIssue = 0;
                        sumRental = 0;
                        sumReturnCal = 0;
                        sumReturn = 0;
                        sumIssueExternal = 0;
                        sumReclass = JobLedgerNews.Where(c => c.JobNo == JobNo && c.JobLedgerEntry == std.JobTaskCut && c.TypeOfTask == 0 && c.PostingDate < startdate).Sum(c => c.TotalCost + c.LineAmount);
                        sumIssue = JobLedgerNews.Where(c => c.JobNo == JobNo && c.JobLedgerEntry == std.JobTaskCut && c.TypeOfTask == 3 && c.PostingDate < startdate).Sum(c => c.TotalCost + c.LineAmount);
                        sumRental = JobLedgerNews.Where(c => c.JobNo == JobNo && c.JobLedgerEntry == std.JobTaskCut && c.TypeOfTask == 2 && c.PostingDate < startdate).Sum(c => c.TotalCost + c.LineAmount);
                        sumReturnCal = JobLedgerNews.Where(c => c.JobNo == JobNo && c.JobLedgerEntry == std.JobTaskCut && c.TypeOfTask == 1 && c.Document3 == "CAL" && c.PostingDate < startdate).Sum(c => c.TotalCost + c.LineAmount) * 2;
                        sumReturn = JobLedgerNews.Where(c => c.JobNo == JobNo && c.JobLedgerEntry == std.JobTaskCut && c.TypeOfTask == 1 && c.PostingDate < startdate).Sum(c => c.TotalCost + c.LineAmount);
                        sumIssueExternal = JobLedgerNews.Where(c => c.JobNo != JobNo && c.JobLedgerEntry == std.JobTaskCut && c.TypeOfTask == 3 && c.FromLocation == JobNo && c.PostingDate < startdate).Sum(c => c.TotalCost + c.LineAmount) * 2;
                        sumReclassAPO = JobLedgerNews.Where(c => c.JobNo != JobNo && c.JobLedgerEntry == std.JobTaskCut && c.TypeOfTask == 0 && c.PostingDate < startdate && c.TotalCost == 0).Sum(c => c.Total);
                        OpenningAMT = (sumReclass + sumIssue + sumRental + sumReturnCal + sumReclassAPO) - (sumReturn + sumIssueExternal);
                        //current.OpenningAMT = OpenningAMT;

                        sumReclass = 0;
                        sumIssue = 0;
                        sumRental = 0;
                        sumReturnCal = 0;
                        sumReturn = 0;
                        sumIssueExternal = 0;
                        sumReclass = JobLedgerNews.Where(c => c.JobNo == JobNo && c.JobLedgerEntry == std.JobTaskCut && c.TypeOfTask == 0 && c.PostingDate <= enddate).Sum(c => c.Quantity);
                        sumIssue = JobLedgerNews.Where(c => c.JobNo == JobNo && c.JobLedgerEntry == std.JobTaskCut && c.TypeOfTask == 3 && c.PostingDate <= enddate).Sum(c => c.Quantity);
                        sumRental = JobLedgerNews.Where(c => c.JobNo == JobNo && c.JobLedgerEntry == std.JobTaskCut && c.TypeOfTask == 2 && c.PostingDate <= enddate).Sum(c => c.Quantity);
                        sumReturnCal = JobLedgerNews.Where(c => c.JobNo == JobNo && c.JobLedgerEntry == std.JobTaskCut && c.TypeOfTask == 1 && c.Document3 == "CAL" && c.PostingDate <= enddate).Sum(c => c.Quantity) * 2;
                        sumReturn = JobLedgerNews.Where(c => c.JobNo == JobNo && c.JobLedgerEntry == std.JobTaskCut && c.TypeOfTask == 1 && c.PostingDate <= enddate).Sum(c => c.Quantity);
                        sumIssueExternal = JobLedgerNews.Where(c => c.JobNo != JobNo && c.JobLedgerEntry == std.JobTaskCut && c.TypeOfTask == 3 && c.PostingDate <= enddate && c.FromLocation == JobNo).Sum(c => c.Quantity) * 2;
                        CumBalQTY = (sumReclass + sumIssue + sumRental + sumReturnCal) - (sumReturn + sumIssueExternal);
                        //current.CumBalQTY = CumBalQTY;

                        sumReclass = 0;
                        sumIssue = 0;
                        sumRental = 0;
                        sumReturnCal = 0;
                        sumReturn = 0;
                        sumIssueExternal = 0;
                        sumReclass = JobLedgerNews.Where(c => c.JobNo == JobNo && c.JobLedgerEntry == std.JobTaskCut && c.TypeOfTask == 0 && c.PostingDate <= enddate).Sum(c => c.TotalCost + c.LineAmount);
                        sumIssue = JobLedgerNews.Where(c => c.JobNo == JobNo && c.JobLedgerEntry == std.JobTaskCut && c.TypeOfTask == 3 && c.PostingDate <= enddate).Sum(c => c.TotalCost + c.LineAmount);
                        sumRental = JobLedgerNews.Where(c => c.JobNo == JobNo && c.JobLedgerEntry == std.JobTaskCut && c.TypeOfTask == 2 && c.PostingDate <= enddate).Sum(c => c.TotalCost + c.LineAmount);
                        sumReturnCal = JobLedgerNews.Where(c => c.JobNo == JobNo && c.JobLedgerEntry == std.JobTaskCut && c.TypeOfTask == 1 && c.Document3 == "CAL" && c.PostingDate <= enddate).Sum(c => c.TotalCost + c.LineAmount) * 2;
                        sumReturn = JobLedgerNews.Where(c => c.JobNo == JobNo && c.JobLedgerEntry == std.JobTaskCut && c.TypeOfTask == 1 && c.PostingDate <= enddate).Sum(c => c.TotalCost + c.LineAmount);
                        sumIssueExternal = JobLedgerNews.Where(c => c.JobNo != JobNo && c.JobLedgerEntry == std.JobTaskCut && c.TypeOfTask == 3 && c.FromLocation == JobNo && c.PostingDate <= enddate).Sum(c => c.TotalCost + c.LineAmount) * 2;
                        sumReclassAPO = JobLedgerNews.Where(c => c.JobNo != JobNo && c.JobLedgerEntry == std.JobTaskCut && c.TypeOfTask == 0 && c.PostingDate <= enddate && c.TotalCost == 0).Sum(c => c.Total);
                        CumBalAMT = (sumReclass + sumIssue + sumRental + sumReturnCal + sumReclassAPO) - (sumReturn + sumIssueExternal);
                        //current.CumBalAMT = CumBalAMT;



                        BudgetQTY = jobPlanningLines.Where(c => c.JobNo == JobNo && c.JobTaskNo == std.JobTaskCut).Sum(c => c.Quantity);
                        BudgetAMT = jobPlanningLines.Where(c => c.JobNo == JobNo && c.JobTaskNo == std.JobTaskCut).Sum(c => c.TotalCost);
                        if (BudgetQTY == 0)
                        {
                            PercenQTY = 0;
                        }
                        else
                        {
                            PercenQTY = (CumBalQTY / BudgetQTY) * 100;
                        }

                        if (BudgetAMT == 0)
                        {
                            PercenAMT = 0;
                        }
                        else
                        {
                            PercenAMT = (CumBalAMT / BudgetAMT) * 100;
                        }

                        //current.BudgetQTY = BudgetQTY;
                        //current.BudgetAMT = BudgetAMT;
                        //current.PercenAMT = PercenAMT;
                        //current.PercenQTY = PercenQTY;




                    }
                    else
                    {
                        sumReclass = 0;
                        sumIssue = 0;
                        sumRental = 0;
                        sumReturnCal = 0;
                        sumReturn = 0;
                        sumIssueExternal = 0;
                        sumReclass = JobLedgerNews.Where(c => c.JobNo == JobNo && c.JobLedgerEntry == std.JobTaskCut && c.TypeOfTask == 0 && c.PostingDate >= startdate && c.PostingDate <= enddate).Sum(c => c.Quantity);
                        sumIssue = JobLedgerNews.Where(c => c.JobNo == JobNo && c.JobLedgerEntry == std.JobTaskCut && c.TypeOfTask == 3 && c.PostingDate >= startdate && c.PostingDate <= enddate).Sum(c => c.Quantity);
                        sumRental = JobLedgerNews.Where(c => c.JobNo == JobNo && c.JobLedgerEntry == std.JobTaskCut && c.TypeOfTask == 2 && c.PostingDate >= startdate && c.PostingDate <= enddate).Sum(c => c.Quantity);
                        sumReturnCal = JobLedgerNews.Where(c => c.JobNo == JobNo && c.JobLedgerEntry == std.JobTaskCut && c.TypeOfTask == 1 && c.Document3 == "CAL" && c.PostingDate >= startdate && c.PostingDate <= enddate).Sum(c => c.Quantity) * 2;
                        sumReturn = JobLedgerNews.Where(c => c.JobNo == JobNo && c.JobLedgerEntry == std.JobTaskCut && c.TypeOfTask == 1 && c.PostingDate >= startdate && c.PostingDate <= enddate).Sum(c => c.Quantity);
                        sumIssueExternal = JobLedgerNews.Where(c => c.JobNo != JobNo && c.JobLedgerEntry == std.JobTaskCut && c.TypeOfTask == 3 && c.PostingDate >= startdate && c.FromLocation == JobNo && c.PostingDate <= enddate).Sum(c => c.Quantity) * 2;
                        ThisPeriodQty = (sumReclass + sumIssue + sumRental + sumReturnCal) - (sumReturn + sumIssueExternal);
                        //current.ThisPeriodQty = ThisPeriodQty;

                        sumReclass = 0;
                        sumIssue = 0;
                        sumRental = 0;
                        sumReturnCal = 0;
                        sumReturn = 0;
                        sumIssueExternal = 0;
                        sumReclass = JobLedgerNews.Where(c => c.JobNo == JobNo && c.JobLedgerEntry == std.JobTaskCut && c.TypeOfTask == 0 && c.PostingDate >= startdate && c.PostingDate <= enddate).Sum(c => c.TotalCost);
                        sumIssue = JobLedgerNews.Where(c => c.JobNo == JobNo && c.JobLedgerEntry == std.JobTaskCut && c.TypeOfTask == 3 && c.PostingDate >= startdate && c.PostingDate <= enddate).Sum(c => c.TotalCost);
                        sumRental = JobLedgerNews.Where(c => c.JobNo == JobNo && c.JobLedgerEntry == std.JobTaskCut && c.TypeOfTask == 2 && c.PostingDate >= startdate && c.PostingDate <= enddate).Sum(c => c.TotalCost);
                        sumReturnCal = JobLedgerNews.Where(c => c.JobNo == JobNo && c.JobLedgerEntry == std.JobTaskCut && c.TypeOfTask == 1 && c.Document3 == "CAL" && c.PostingDate >= startdate && c.PostingDate <= enddate).Sum(c => c.TotalCost) * 2;
                        sumReturn = JobLedgerNews.Where(c => c.JobNo == JobNo && c.JobLedgerEntry == std.JobTaskCut && c.TypeOfTask == 1 && c.PostingDate >= startdate && c.PostingDate <= enddate).Sum(c => c.TotalCost);
                        sumIssueExternal = JobLedgerNews.Where(c => c.JobNo != JobNo && c.JobLedgerEntry == std.JobTaskCut && c.TypeOfTask == 3 && c.FromLocation == JobNo && c.PostingDate >= startdate && c.PostingDate <= enddate).Sum(c => c.TotalCost) * 2;
                        sumReclassAPO = JobLedgerNews.Where(c => c.JobNo != JobNo && c.JobLedgerEntry == std.JobTaskCut && c.TypeOfTask == 0 && c.PostingDate >= startdate && c.PostingDate <= enddate && c.TotalCost == 0).Sum(c => c.Total);
                        ThisPeriodAMT = (sumReclass + sumIssue + sumRental + sumReturnCal + sumReclassAPO) - (sumReturn + sumIssueExternal);
                        //current.ThisPeriodAMT = ThisPeriodAMT;

                        sumReclass = 0;
                        sumIssue = 0;
                        sumRental = 0;
                        sumReturnCal = 0;
                        sumReturn = 0;
                        sumIssueExternal = 0;
                        sumReclass = JobLedgerNews.Where(c => c.JobNo == JobNo && c.JobLedgerEntry == std.JobTaskCut && c.TypeOfTask == 0 && c.PostingDate < startdate).Sum(c => c.Quantity);
                        sumIssue = JobLedgerNews.Where(c => c.JobNo == JobNo && c.JobLedgerEntry == std.JobTaskCut && c.TypeOfTask == 3 && c.PostingDate < startdate).Sum(c => c.Quantity);
                        sumRental = JobLedgerNews.Where(c => c.JobNo == JobNo && c.JobLedgerEntry == std.JobTaskCut && c.TypeOfTask == 2 && c.PostingDate < startdate).Sum(c => c.Quantity);
                        sumReturnCal = JobLedgerNews.Where(c => c.JobNo == JobNo && c.JobLedgerEntry == std.JobTaskCut && c.TypeOfTask == 1 && c.Document3 == "CAL" && c.PostingDate < startdate).Sum(c => c.Quantity) * 2;
                        sumReturn = JobLedgerNews.Where(c => c.JobNo == JobNo && c.JobLedgerEntry == std.JobTaskCut && c.TypeOfTask == 1 && c.PostingDate < startdate).Sum(c => c.Quantity);
                        sumIssueExternal = JobLedgerNews.Where(c => c.JobNo != JobNo && c.JobLedgerEntry == std.JobTaskCut && c.TypeOfTask == 3 && c.PostingDate < startdate && c.FromLocation == JobNo).Sum(c => c.Quantity) * 2;
                        OpenningQTY = (sumReclass + sumIssue + sumRental + sumReturnCal) - (sumReturn + sumIssueExternal);
                        //current.OpenningQTY = OpenningQTY;


                        sumReclass = 0;
                        sumIssue = 0;
                        sumRental = 0;
                        sumReturnCal = 0;
                        sumReturn = 0;
                        sumIssueExternal = 0;
                        sumReclass = JobLedgerNews.Where(c => c.JobNo == JobNo && c.JobLedgerEntry == std.JobTaskCut && c.TypeOfTask == 0 && c.PostingDate < startdate).Sum(c => c.TotalCost);
                        sumIssue = JobLedgerNews.Where(c => c.JobNo == JobNo && c.JobLedgerEntry == std.JobTaskCut && c.TypeOfTask == 3 && c.PostingDate < startdate).Sum(c => c.TotalCost);
                        sumRental = JobLedgerNews.Where(c => c.JobNo == JobNo && c.JobLedgerEntry == std.JobTaskCut && c.TypeOfTask == 2 && c.PostingDate < startdate).Sum(c => c.TotalCost);
                        sumReturnCal = JobLedgerNews.Where(c => c.JobNo == JobNo && c.JobLedgerEntry == std.JobTaskCut && c.TypeOfTask == 1 && c.Document3 == "CAL" && c.PostingDate < startdate).Sum(c => c.TotalCost) * 2;
                        sumReturn = JobLedgerNews.Where(c => c.JobNo == JobNo && c.JobLedgerEntry == std.JobTaskCut && c.TypeOfTask == 1 && c.PostingDate < startdate).Sum(c => c.TotalCost);
                        sumIssueExternal = JobLedgerNews.Where(c => c.JobNo != JobNo && c.JobLedgerEntry == std.JobTaskCut && c.TypeOfTask == 3 && c.FromLocation == JobNo && c.PostingDate < startdate).Sum(c => c.TotalCost) * 2;
                        sumReclassAPO = JobLedgerNews.Where(c => c.JobNo != JobNo && c.JobLedgerEntry == std.JobTaskCut && c.TypeOfTask == 0 && c.PostingDate < startdate && c.TotalCost == 0).Sum(c => c.Total);
                        OpenningAMT = (sumReclass + sumIssue + sumRental + sumReturnCal + sumReclassAPO) - (sumReturn + sumIssueExternal);
                        //current.OpenningAMT = OpenningAMT;

                        sumReclass = 0;
                        sumIssue = 0;
                        sumRental = 0;
                        sumReturnCal = 0;
                        sumReturn = 0;
                        sumIssueExternal = 0;
                        sumReclass = JobLedgerNews.Where(c => c.JobNo == JobNo && c.JobLedgerEntry == std.JobTaskCut && c.TypeOfTask == 0 && c.PostingDate <= enddate).Sum(c => c.Quantity);
                        sumIssue = JobLedgerNews.Where(c => c.JobNo == JobNo && c.JobLedgerEntry == std.JobTaskCut && c.TypeOfTask == 3 && c.PostingDate <= enddate).Sum(c => c.Quantity);
                        sumRental = JobLedgerNews.Where(c => c.JobNo == JobNo && c.JobLedgerEntry == std.JobTaskCut && c.TypeOfTask == 2 && c.PostingDate <= enddate).Sum(c => c.Quantity);
                        sumReturnCal = JobLedgerNews.Where(c => c.JobNo == JobNo && c.JobLedgerEntry == std.JobTaskCut && c.TypeOfTask == 1 && c.Document3 == "CAL" && c.PostingDate <= enddate).Sum(c => c.Quantity) * 2;
                        sumReturn = JobLedgerNews.Where(c => c.JobNo == JobNo && c.JobLedgerEntry == std.JobTaskCut && c.TypeOfTask == 1 && c.PostingDate <= enddate).Sum(c => c.Quantity);
                        sumIssueExternal = JobLedgerNews.Where(c => c.JobNo != JobNo && c.JobLedgerEntry == std.JobTaskCut && c.TypeOfTask == 3 && c.PostingDate <= enddate && c.FromLocation == JobNo).Sum(c => c.Quantity) * 2;
                        CumBalQTY = (sumReclass + sumIssue + sumRental + sumReturnCal) - (sumReturn + sumIssueExternal);
                        //current.CumBalQTY = CumBalQTY;

                        sumReclass = 0;
                        sumIssue = 0;
                        sumRental = 0;
                        sumReturnCal = 0;
                        sumReturn = 0;
                        sumIssueExternal = 0;
                        sumReclass = JobLedgerNews.Where(c => c.JobNo == JobNo && c.JobLedgerEntry == std.JobTaskCut && c.TypeOfTask == 0 && c.PostingDate <= enddate).Sum(c => c.TotalCost);
                        sumIssue = JobLedgerNews.Where(c => c.JobNo == JobNo && c.JobLedgerEntry == std.JobTaskCut && c.TypeOfTask == 3 && c.PostingDate <= enddate).Sum(c => c.TotalCost);
                        sumRental = JobLedgerNews.Where(c => c.JobNo == JobNo && c.JobLedgerEntry == std.JobTaskCut && c.TypeOfTask == 2 && c.PostingDate <= enddate).Sum(c => c.TotalCost);
                        sumReturnCal = JobLedgerNews.Where(c => c.JobNo == JobNo && c.JobLedgerEntry == std.JobTaskCut && c.TypeOfTask == 1 && c.Document3 == "CAL" && c.PostingDate <= enddate).Sum(c => c.TotalCost) * 2;
                        sumReturn = JobLedgerNews.Where(c => c.JobNo == JobNo && c.JobLedgerEntry == std.JobTaskCut && c.TypeOfTask == 1 && c.PostingDate <= enddate).Sum(c => c.TotalCost);
                        sumIssueExternal = JobLedgerNews.Where(c => c.JobNo != JobNo && c.JobLedgerEntry == std.JobTaskCut && c.TypeOfTask == 3 && c.FromLocation == JobNo && c.PostingDate <= enddate).Sum(c => c.TotalCost) * 2;
                        sumReclassAPO = JobLedgerNews.Where(c => c.JobNo != JobNo && c.JobLedgerEntry == std.JobTaskCut && c.TypeOfTask == 0 && c.PostingDate <= enddate && c.TotalCost == 0).Sum(c => c.Total);
                        CumBalAMT = (sumReclass + sumIssue + sumRental + sumReturnCal + sumReclassAPO) - (sumReturn + sumIssueExternal);
                        //current.CumBalAMT = CumBalAMT;

                        BudgetQTY = jobPlanningLines.Where(c => c.JobNo == JobNo && c.JobTaskNo == std.JobTaskCut).Sum(c => c.Quantity);
                        BudgetAMT = jobPlanningLines.Where(c => c.JobNo == JobNo && c.JobTaskNo == std.JobTaskCut).Sum(c => c.TotalCost);
                        if (BudgetQTY == 0)
                        {
                            PercenQTY = 0;
                        }
                        else
                        {
                            PercenQTY = (CumBalQTY / BudgetQTY) * 100;
                        }

                        if (BudgetAMT == 0)
                        {
                            PercenAMT = 0;
                        }
                        else
                        {
                            PercenAMT = (CumBalAMT / BudgetAMT) * 100;
                        }

                        //current.BudgetQTY = BudgetQTY;
                        //current.BudgetAMT = BudgetAMT;
                        //current.PercenAMT = PercenAMT;
                        //current.PercenQTY = PercenQTY;




                        if (std.MainJobID == std.JobTaskNo || std.SubJobID == std.JobTaskNo)
                        {

                        }
                        else
                        {
                            worksheet2.Cells[r, 1].Value = std.DescriptionFull;
                            worksheet2.Cells[r, 2].Value = ThisPeriodQty;
                            worksheet2.Cells[r, 3].Value = ThisPeriodAMT;
                            worksheet2.Cells[r, 4].Value = OpenningQTY;
                            worksheet2.Cells[r, 5].Value = OpenningAMT;
                            worksheet2.Cells[r, 6].Value = CumBalQTY;
                            worksheet2.Cells[r, 7].Value = CumBalAMT;
                            worksheet2.Cells[r, 8].Value = BudgetQTY;
                            worksheet2.Cells[r, 9].Value = PercenQTY.ToString("#,###.00") + "%";
                            worksheet2.Cells[r, 10].Value = BudgetAMT;
                            worksheet2.Cells[r, 11].Value = PercenAMT.ToString("#,###.00") + "%";
                            r++;
                       
                        }

                    }


                    if (std.JobTaskNo == "G0210")
                    {
                        G0210 = CumBalAMT;
                        ViewBag.G0210N = SetFontRed(CumBalAMT * -1, 1);
                        ViewBag.G0210P = SetFontRed(CumBalAMT, 1);
                    }
                    if (std.JobTaskNo == "C0110")
                    {
                        C0110 = CumBalAMT;
                        ViewBag.C0110 = SetFontRed(CumBalAMT, 1);
                    }
                    if (std.JobTaskNo == "C0120")
                    {
                        C0120 = CumBalAMT;
                        ViewBag.C0120 = SetFontRed(CumBalAMT, 1);
                    }

                    if (std.JobTaskNo == "C0130")
                    {
                        C0130 = CumBalAMT;
                        ViewBag.C0130 = SetFontRed(CumBalAMT, 1);
                    }

                    if (std.JobTaskNo == "C0410")
                    {
                        C0410 = CumBalAMT;
                        ViewBag.C0410 = SetFontRed(CumBalAMT, 1);
                    }

                    if (std.JobTaskNo == "C0510")
                    {
                        C0510 = CumBalAMT;
                        ViewBag.C0510 = SetFontRed(CumBalAMT, 1);
                    }
                    if (std.JobTaskNo == "C0910")
                    {
                        C0910 = CumBalAMT;
                        ViewBag.C0910 = SetFontRed(CumBalAMT, 1);
                    }
                    if (std.JobTaskNo == "C1010")
                    {
                        C1010 = CumBalAMT;
                        ViewBag.C1010 = SetFontRed(CumBalAMT, 1);
                    }
                    if (std.JobTaskNo == "C1110")
                    {
                        C1110 = CumBalAMT;
                        ViewBag.C1110 = SetFontRed(CumBalAMT, 1);
                    }
                    if (std.JobTaskNo == "D0450")
                    {
                        D0450 = CumBalAMT;
                        ViewBag.D0450 = SetFontRed(CumBalAMT, 1);
                    }
                    if (std.JobTaskNo == "H0110")
                    {
                        H0110 = CumBalAMT;
                        ViewBag.H0110 = SetFontRed(CumBalAMT, 1);
                    }
                    if (std.JobTaskNo == "H0130")
                    {
                        H0130 = CumBalAMT;
                        ViewBag.H0130 = SetFontRed(CumBalAMT, 1);
                    }


                    if (std.JobTaskNo == "E0910")
                    {
                        E0910 = CumBalAMT;
                        ViewBag.E0910 = SetFontRed(CumBalAMT, 1);
                    }
                    if (std.JobTaskNo == "E0920")
                    {
                        E0920 = CumBalAMT;
                        ViewBag.E0920 = SetFontRed(CumBalAMT, 1);
                    }
                    if (std.JobTaskNo == "E0930")
                    {
                        E0930 = CumBalAMT;
                        ViewBag.E0930 = SetFontRed(CumBalAMT, 1);
                    }
                    if (std.JobTaskNo == "E0940")
                    {
                        E0940 = CumBalAMT;
                        ViewBag.E0940 = SetFontRed(CumBalAMT, 1);
                    }

                    if (std.JobTaskNo == "F5999")
                    {
                        F5999 = CumBalAMT;
                        ViewBag.F5999 = SetFontRed(CumBalAMT, 1);
                    }
                }
            }



            worksheet3.Cells[5, 1].Value = "Total Income";
            worksheet3.Cells[5, 2].Value = OthisQ;
            worksheet3.Cells[5, 3].Value = OthisA;
            worksheet3.Cells[5, 4].Value = OopenQ;
            worksheet3.Cells[5, 5].Value = OopenA;
            worksheet3.Cells[5, 6].Value = OcumQ;
            worksheet3.Cells[5, 7].Value = OcumA;
            worksheet3.Cells[5, 8].Value = ObudQ;
            worksheet3.Cells[5, 9].Value = Percen(OcumQ, ObudQ) + "%";
            worksheet3.Cells[5, 10].Value = ObudA;
            worksheet3.Cells[5, 11].Value = Percen(OcumA, ObudA) + "%";





            worksheet3.Cells[rl, 1].Value = "Total Cost";
            worksheet3.Cells[rl, 2].Value = thisQ;
            worksheet3.Cells[rl, 3].Value = thisA;
            worksheet3.Cells[rl, 4].Value = openQ;
            worksheet3.Cells[rl, 5].Value = openA;
            worksheet3.Cells[rl, 6].Value = cumQ;
            worksheet3.Cells[rl, 7].Value = cumA;
            worksheet3.Cells[rl, 8].Value = budQ;
            worksheet3.Cells[rl, 9].Value = Percen(cumQ, budQ) + "%";
            worksheet3.Cells[rl, 10].Value = budA;
            worksheet3.Cells[rl, 11].Value = Percen(cumA, budA) + "%";

            rl += 1;

            worksheet3.Cells[rl, 1].Value = "PROFIT (LOSS)";
            worksheet3.Cells[rl, 2].Value = (OthisQ * -1) - thisQ;
            worksheet3.Cells[rl, 3].Value = (OthisA * -1) - thisA;
            worksheet3.Cells[rl, 4].Value = (OopenQ * -1) - openQ;
            worksheet3.Cells[rl, 5].Value = (OopenA * -1) - openA;
            worksheet3.Cells[rl, 6].Value = (OcumQ * -1) - cumQ;
            worksheet3.Cells[rl, 7].Value = (OcumA * -1) - cumA;
            worksheet3.Cells[rl, 8].Value = (ObudQ * -1) - budQ;
            worksheet3.Cells[rl, 9].Value = Percen((OcumQ * -1) - cumQ, (ObudQ * -1) - budQ) + "%";
            worksheet3.Cells[rl, 10].Value = (ObudA * -1) - budA;
            worksheet3.Cells[rl, 11].Value = Percen((OcumA * -1) - cumA, (ObudA * -1) - budA) + "%";


            // AutoFitColumns
            worksheet2.Cells[1, 1, r - 1, 11].AutoFitColumns();

            worksheet2.Column(2).Style.Numberformat.Format = "#,##0.00";
            worksheet2.Column(3).Style.Numberformat.Format = "#,##0.00";
            worksheet2.Column(4).Style.Numberformat.Format = "#,##0.00";
            worksheet2.Column(5).Style.Numberformat.Format = "#,##0.00";
            worksheet2.Column(6).Style.Numberformat.Format = "#,##0.00";
            worksheet2.Column(7).Style.Numberformat.Format = "#,##0.00";
            worksheet2.Column(8).Style.Numberformat.Format = "#,##0.00";
            worksheet2.Column(9).Style.Numberformat.Format = "#,##0.00";
            worksheet2.Column(10).Style.Numberformat.Format = "#,##0.00";
            worksheet2.Column(11).Style.Numberformat.Format = "#,##0.00";

            // Add to table / Add summary row
            var tbl2 = worksheet2.Tables.Add(new ExcelAddressBase(fromRow: 2, fromCol: 1, toRow: r - 1, toColumn: 11), "Data2");
            tbl2.ShowHeader = true;
            tbl2.TableStyle = TableStyles.Dark9;
            tbl2.ShowFilter = true;



            //AutoFitColumns
            worksheet3.Cells[1, 1, rl, 11].AutoFitColumns();

            worksheet3.Column(2).Style.Numberformat.Format = "#,##0.00";
            worksheet3.Column(3).Style.Numberformat.Format = "#,##0.00";
            worksheet3.Column(4).Style.Numberformat.Format = "#,##0.00";
            worksheet3.Column(5).Style.Numberformat.Format = "#,##0.00";
            worksheet3.Column(6).Style.Numberformat.Format = "#,##0.00";
            worksheet3.Column(7).Style.Numberformat.Format = "#,##0.00";
            worksheet3.Column(8).Style.Numberformat.Format = "#,##0.00";
            worksheet3.Column(9).Style.Numberformat.Format = "#,##0.00";
            worksheet3.Column(10).Style.Numberformat.Format = "#,##0.00";
            worksheet3.Column(11).Style.Numberformat.Format = "#,##0.00";



            //Ok now format the values;
            using (var range = worksheet3.Cells[5, 1, 5, 11])  //Address "A5:K5"
            {
                range.Style.Font.Bold = true;
                range.Style.Fill.PatternType = ExcelFillStyle.Solid;
                range.Style.Fill.BackgroundColor.SetColor(Color.Yellow);
                //range.Style.Font.Color.SetColor(Color.White);
            }

            //Ok now format the values;
            using (var range = worksheet3.Cells[14, 1, 14, 11])  //Address "A15:K15"
            {
                range.Style.Font.Bold = true;
                range.Style.Fill.PatternType = ExcelFillStyle.Solid;
                range.Style.Fill.BackgroundColor.SetColor(Color.Yellow);
                //range.Style.Font.Color.SetColor(Color.White);
            }


            //Ok now format the values;
            using (var range = worksheet3.Cells[15, 1, 15, 11])  //Address "A15:K15"
            {
                range.Style.Font.Bold = true;
                range.Style.Fill.PatternType = ExcelFillStyle.Solid;
                range.Style.Fill.BackgroundColor.SetColor(Color.Yellow);
                //range.Style.Font.Color.SetColor(Color.White);
            }



            var tbl3 = worksheet3.Tables.Add(new ExcelAddressBase(fromRow: 2, fromCol: 1, toRow: rl, toColumn: 11), "Data3");
            tbl3.ShowHeader = true;
            tbl3.TableStyle = TableStyles.Dark9;
            tbl3.ShowFilter = true;





            return package;
        }







        public static decimal Percen(decimal value1, decimal value2)
        {
            decimal result = 0;
            try
            {
                result = Math.Round(((value1 / value2) * 100), 2);
            }
            catch
            {
                result = 0;
            }
            return result;
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







        public IActionResult JobCostChart()
        {

            /*Check Session */
            var page = "238";
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




        public object JobCostData(DataSourceLoadOptions loadOptions)
        {

            /*Check Session */
            var page = "238";
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

     

            var userjob = _context.UserJobs.Where(s => s.UserId == HttpContext.Session.GetInt32("Userid")).OrderBy(s => s.UserJobDetail).ToList();
            var UserjobArray= userjob.Select(x => x.UserJobDetail).ToArray();
           

            var queryData = "SELECT ROW_NUMBER() OVER (ORDER BY a.PostingDate) as ID," +
                "a.PostingDate,a.DocumentNo," +
                "(SELECT CASE  " +
                "   WHEN dbo."+ Environment.GetEnvironmentVariable("Company") +"Job Ledger Entry].[Type of task]=3 and dbo."+ Environment.GetEnvironmentVariable("Company") +"Job Ledger Entry].[Job No_]<>dbo."+ Environment.GetEnvironmentVariable("Company") +"Job Ledger Entry].[From Location] and dbo."+ Environment.GetEnvironmentVariable("Company") +"Job Ledger Entry].[From Location]<>'' THEN a.FromLocation " +
                " 	ELSE a.JobNo END " +
                " FROM dbo."+ Environment.GetEnvironmentVariable("Company") +"Job Ledger Entry]  " +
                " WHERE dbo."+ Environment.GetEnvironmentVariable("Company") +"Job Ledger Entry].[Entry No_]=a.EntryNo )as JobNo " +
                ",a.JobMain,a.JobSub,a.JobLedgerEntry,a.JobLedgerEntry1,a.LineAmount,a.TotalCost,a.TypeOfTask,a.FromLocation,a.Document3," +
                "(SELECT  " +
                "CASE " +
                "	WHEN SUBSTRING(dbo."+ Environment.GetEnvironmentVariable("Company") +"Job Ledger Entry].[Job Task No_],1,1)='O' or SUBSTRING(dbo."+ Environment.GetEnvironmentVariable("Company") +"Job Ledger Entry].[Job Task No_],1,1)='N' THEN a.TotalCost+a.LineAmount " +
                "	WHEN dbo."+ Environment.GetEnvironmentVariable("Company") +"Job Ledger Entry].[Type of task]=3 and dbo."+ Environment.GetEnvironmentVariable("Company") +"Job Ledger Entry].[Job No_]<>dbo."+ Environment.GetEnvironmentVariable("Company") +"Job Ledger Entry].[From Location] and  dbo."+ Environment.GetEnvironmentVariable("Company") +"Job Ledger Entry].[From Location]<>'' THEN a.TotalCost*-1" +
                "	WHEN dbo."+ Environment.GetEnvironmentVariable("Company") +"Job Ledger Entry].[Type of task]=1 and SUBSTRING(dbo."+ Environment.GetEnvironmentVariable("Company") +"Job Ledger Entry].[Document No_],1,3)='CAL' THEN a.TotalCost " +
                "	WHEN dbo."+ Environment.GetEnvironmentVariable("Company") +"Job Ledger Entry].[Type of task]=0 THEN a.TotalCost " +
                "   WHEN dbo."+ Environment.GetEnvironmentVariable("Company") +"Job Ledger Entry].[Type of task]=2 THEN a.TotalCost " +
                "   WHEN dbo."+ Environment.GetEnvironmentVariable("Company") +"Job Ledger Entry].[Type of task]=3 THEN a.TotalCost " +
                "	WHEN dbo."+ Environment.GetEnvironmentVariable("Company") +"Job Ledger Entry].[Type of task]=1 THEN a.TotalCost*-1 " +
                "	ELSE 0 END " +
                "FROM   dbo."+ Environment.GetEnvironmentVariable("Company") +"Job Ledger Entry]   " +
                "WHERE dbo."+ Environment.GetEnvironmentVariable("Company") +"Job Ledger Entry].[Entry No_]=a.EntryNo ) as Total, " +
                "(SELECT  " +
                "CASE " +
                "	WHEN dbo."+ Environment.GetEnvironmentVariable("Company") +"Job Ledger Entry].[Type of task]=3 and dbo."+ Environment.GetEnvironmentVariable("Company") +"Job Ledger Entry].[Job No_]<>dbo."+ Environment.GetEnvironmentVariable("Company") +"Job Ledger Entry].[From Location] and dbo."+ Environment.GetEnvironmentVariable("Company") +"Job Ledger Entry].[From Location]<>''  THEN a.Quantity*-1 " +
                "	WHEN dbo."+ Environment.GetEnvironmentVariable("Company") +"Job Ledger Entry].[Type of task]=1 and SUBSTRING(dbo."+ Environment.GetEnvironmentVariable("Company") +"Job Ledger Entry].[Document No_],1,3)='CAL' THEN a.Quantity " +
                "	WHEN dbo."+ Environment.GetEnvironmentVariable("Company") +"Job Ledger Entry].[Type of task]=0 THEN a.Quantity " +
                "   WHEN dbo."+ Environment.GetEnvironmentVariable("Company") +"Job Ledger Entry].[Type of task]=2 THEN a.Quantity " +
                "   WHEN dbo."+ Environment.GetEnvironmentVariable("Company") +"Job Ledger Entry].[Type of task]=3 THEN a.Quantity " +
                "	WHEN dbo."+ Environment.GetEnvironmentVariable("Company") +"Job Ledger Entry].[Type of task]=1 THEN a.Quantity*-1 " +
                "	ELSE 0 END " +
                " FROM   dbo."+ Environment.GetEnvironmentVariable("Company") +"Job Ledger Entry]   " +
                " WHERE dbo."+ Environment.GetEnvironmentVariable("Company") +"Job Ledger Entry].[Entry No_]=a.EntryNo ) as Quantity " +
                " FROM( " +
                " SELECT  " +
                " dbo."+ Environment.GetEnvironmentVariable("Company") +"Job Ledger Entry].[Entry No_] AS EntryNo, " +
                " dbo."+ Environment.GetEnvironmentVariable("Company") +"Job Ledger Entry].[Posting Date] AS PostingDate, " +
                " dbo."+ Environment.GetEnvironmentVariable("Company") +"Job Ledger Entry].[Document No_] AS DocumentNo,  " +
                " SUBSTRING(dbo."+ Environment.GetEnvironmentVariable("Company") +"Job Ledger Entry].[Document No_],1,3) as Document3, " +
                " dbo."+ Environment.GetEnvironmentVariable("Company") +"Job Ledger Entry].[Job No_] AS JobNo," +
                " CONCAT(SUBSTRING(dbo."+ Environment.GetEnvironmentVariable("Company") +"Job Ledger Entry].[Job Task No_],1,1),'0000') AS JobMain,  " +
                " CONCAT(SUBSTRING(dbo."+ Environment.GetEnvironmentVariable("Company") +"Job Ledger Entry].[Job Task No_],1,3),'00') AS JobSub,  " +
                //" SUBSTRING(dbo."+ Environment.GetEnvironmentVariable("Company") +"Job Ledger Entry].[Job Task No_],1,5) AS JobLedgerEntry,  " +
                " CONCAT(SUBSTRING(dbo."+ Environment.GetEnvironmentVariable("Company") +"Job Ledger Entry].[Job Task No_],1,5),' ' ,(SELECT TOP 1 Description FROM [dbo]."+ Environment.GetEnvironmentVariable("Company") +"Job Planning Line] WHERE dbo."+ Environment.GetEnvironmentVariable("Company") +"Job Planning Line].[Job Task No_]=SUBSTRING(dbo."+ Environment.GetEnvironmentVariable("Company") +"Job Ledger Entry].[Job Task No_],1,5) and Description<>''))  AS JobLedgerEntry, "+
                " SUBSTRING(dbo."+ Environment.GetEnvironmentVariable("Company") +"Job Ledger Entry].[Job Task No_],1,1) AS JobLedgerEntry1, " +
                " dbo."+ Environment.GetEnvironmentVariable("Company") +"Job Ledger Entry].[Line Amount] AS LineAmount,  " +
                " dbo."+ Environment.GetEnvironmentVariable("Company") +"Job Ledger Entry].Quantity AS Quantity, " +
                " CASE WHEN dbo."+ Environment.GetEnvironmentVariable("Company") +"Job Ledger Entry].[Type of task]=0 and dbo."+ Environment.GetEnvironmentVariable("Company") +"Job Ledger Entry].[Original Total Cost]=0 THEN dbo."+ Environment.GetEnvironmentVariable("Company") +"Job Ledger Entry].[Total Cost] ELSE dbo."+ Environment.GetEnvironmentVariable("Company") +"Job Ledger Entry].[Original Total Cost] END AS TotalCost,  " +
                " dbo."+ Environment.GetEnvironmentVariable("Company") +"Job Ledger Entry].[Total Cost] AS Total,  " +
                " dbo."+ Environment.GetEnvironmentVariable("Company") +"Job Ledger Entry].[Type of task] AS TypeOfTask,  " +
                " dbo."+ Environment.GetEnvironmentVariable("Company") +"Job Ledger Entry].[From Location] AS FromLocation  " +
                " FROM  " +
                " dbo."+ Environment.GetEnvironmentVariable("Company") +"Job Ledger Entry]  " +
                " )as a " +
                " ORDER BY a.JobLedgerEntry ";
        

        

            //SqlParameter parameterJob = new SqlParameter("@job", job);
            var JobLedgerNews = _navcontext.JobLedgerNews.FromSqlRaw(queryData).ToList();
            var JobLedgerAfterQuery = (from JobLedgerNew in JobLedgerNews
                                       where UserjobArray.Contains(JobLedgerNew.JobNo)
                                       select JobLedgerNew).ToList();

            //var queryData1 = "";

            //foreach(string jobArray in UserjobArray) {

            //    var JobLedgerTmps = new List<JobLedgerNew>();

            //queryData1 = "SELECT a.PostingDate,a.DocumentNo,a.FromLocation as JobNo,a.JobMain,a.JobSub,a.JobLedgerEntry,a.JobLedgerEntry1,a.LineAmount,a.TotalCost,a.TypeOfTask,a.FromLocation,a.Document3," +
            //    "a.TotalCost*-1 as Total, " +
            //    "a.Quantity*-1 as Quantity " +
            //     " FROM( " +
            //    " SELECT  " +
            //    " dbo."+ Environment.GetEnvironmentVariable("Company") +"Job Ledger Entry].[Entry No_] AS EntryNo, " +
            //    " dbo."+ Environment.GetEnvironmentVariable("Company") +"Job Ledger Entry].[Posting Date] AS PostingDate, " +
            //    " dbo."+ Environment.GetEnvironmentVariable("Company") +"Job Ledger Entry].[Document No_] AS DocumentNo,  " +
            //    " SUBSTRING(dbo."+ Environment.GetEnvironmentVariable("Company") +"Job Ledger Entry].[Document No_],1,3) as Document3, " +
            //    " dbo."+ Environment.GetEnvironmentVariable("Company") +"Job Ledger Entry].[Job No_] AS JobNo," +
            //    " CONCAT(SUBSTRING(dbo."+ Environment.GetEnvironmentVariable("Company") +"Job Ledger Entry].[Job Task No_],1,1),'0000') AS JobMain,  " +
            //    " CONCAT(SUBSTRING(dbo."+ Environment.GetEnvironmentVariable("Company") +"Job Ledger Entry].[Job Task No_],1,3),'00') AS JobSub,  " +
            //    " SUBSTRING(dbo."+ Environment.GetEnvironmentVariable("Company") +"Job Ledger Entry].[Job Task No_],1,5) AS JobLedgerEntry,  " +
            //    " SUBSTRING(dbo."+ Environment.GetEnvironmentVariable("Company") +"Job Ledger Entry].[Job Task No_],1,1) AS JobLedgerEntry1, " +
            //    " dbo."+ Environment.GetEnvironmentVariable("Company") +"Job Ledger Entry].[Line Amount] AS LineAmount,  " +
            //    " dbo."+ Environment.GetEnvironmentVariable("Company") +"Job Ledger Entry].Quantity AS Quantity, " +
            //    " CASE WHEN dbo."+ Environment.GetEnvironmentVariable("Company") +"Job Ledger Entry].[Type of task]=0 and dbo."+ Environment.GetEnvironmentVariable("Company") +"Job Ledger Entry].[Original Total Cost]=0 THEN dbo."+ Environment.GetEnvironmentVariable("Company") +"Job Ledger Entry].[Total Cost] ELSE dbo."+ Environment.GetEnvironmentVariable("Company") +"Job Ledger Entry].[Original Total Cost] END AS TotalCost,  " +
            //    " dbo."+ Environment.GetEnvironmentVariable("Company") +"Job Ledger Entry].[Total Cost] AS Total,  " +
            //    " dbo."+ Environment.GetEnvironmentVariable("Company") +"Job Ledger Entry].[Type of task] AS TypeOfTask,  " +
            //    " dbo."+ Environment.GetEnvironmentVariable("Company") +"Job Ledger Entry].[From Location] AS FromLocation  " +
            //    " FROM  " +
            //    " dbo."+ Environment.GetEnvironmentVariable("Company") +"Job Ledger Entry]  " +
            //    " )as a " +
            //    " ORDER BY a.JobLedgerEntry WHERE a.JobNo<>@job and a.FromLocation=@job and a.TypeOfTask=3 ";

            //    SqlParameter parameterJob = new SqlParameter("@job", jobArray);

            //    JobLedgerTmps = _navcontext.JobLedgerNews.FromSqlRaw(queryData).ToList();
            //    JobLedgerNew JobLedgerTmp = null;

            //    foreach (var std in JobLedgerTmps as List<JobLedgerNew>)
            //    {


            //        JobLedgerTmp = new JobLedgerNew();

            //        JobLedgerTmp = std;
            //        JobLedgerAfterQuery.Add(JobLedgerTmp);


            //    }



            //}








            return DataSourceLoader.Load(JobLedgerAfterQuery, loadOptions);



        }

     }
}