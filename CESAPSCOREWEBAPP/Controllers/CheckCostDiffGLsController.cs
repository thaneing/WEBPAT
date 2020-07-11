using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;
using CESAPSCOREWEBAPP.Helpers;
using CESAPSCOREWEBAPP.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using static CESAPSCOREWEBAPP.Models.Enums;

namespace CESAPSCOREWEBAPP.Controllers
{


    [Authorize]
    public class CheckCostDiffGLsController :BaseController
    {

        private readonly NAVContext _navcontext;
        private readonly DatabaseContext _context;


        public CheckCostDiffGLsController(NAVContext navcontext, DatabaseContext context)
        {
            _context = context;
            _navcontext = navcontext;
        }

        public IActionResult Index()
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
            ViewBag.StartDate = DateTime.Now.ToString("01-MM-yyyy", new CultureInfo("en-US"));
            ViewBag.EndDate = DateTime.Now.ToString("dd-MM-yyyy", new CultureInfo("en-US"));

            return View();
        }



        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Index(string JobNo, string Startdate, string EndDate)
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




            //var queryData = "SELECT b.MainJobID,b.DescriptionMainJob,b.SubJobID,b.DescriptionSubJob,b.JobTaskNo,b.DescriptionFull, " +
            //    " (((CASE WHEN SUBSTRING(b.MainJobID, 1, 1) = 'N' or SUBSTRING(b.MainJobID,1,1)= 'O' THEN (select isnull(sum([Original Total Cost]) + sum([Line Amount]),0) FROM"+ Environment.GetEnvironmentVariable("Company") +"Job Ledger Entry] WHERE [Job No_] = @job AND [Posting Date] > @start AND [Posting Date] < @date1 and SUBSTRING([Job Task No_],1,5) = b.JobTaskNo and [Type of task]=0 )	ELSE (select isnull(sum([Original Total Cost]), 0) FROM"+ Environment.GetEnvironmentVariable("Company") +"Job Ledger Entry] WHERE[Job No_] = @job AND [Posting Date] > @start AND [Posting Date] < @date1 and SUBSTRING([Job Task No_],1,5) = b.JobTaskNo and [Type of task]=0) END )+ (CASE WHEN SUBSTRING(b.MainJobID, 1, 1) = 'N' or SUBSTRING(b.MainJobID,1,1)= 'O' THEN (select isnull(sum([Original Total Cost]) + sum([Line Amount]),0) FROM"+ Environment.GetEnvironmentVariable("Company") +"Job Ledger Entry] WHERE[Job No_] = @job AND [Posting Date] > @start AND [Posting Date] < @date1 and SUBSTRING([Job Task No_],1,5) = b.JobTaskNo and [Type of task]=3 )	ELSE (select isnull(sum([Original Total Cost]), 0) FROM"+ Environment.GetEnvironmentVariable("Company") +"Job Ledger Entry] WHERE[Job No_] = @job AND [Posting Date] > @start AND [Posting Date] < @date1 and SUBSTRING([Job Task No_],1,5) = b.JobTaskNo and [Type of task]=3) END )+ (CASE WHEN SUBSTRING(b.MainJobID, 1, 1) = 'N' or SUBSTRING(b.MainJobID,1,1)= 'O' THEN (select isnull(sum([Original Total Cost]) + sum([Line Amount]),0) FROM"+ Environment.GetEnvironmentVariable("Company") +"Job Ledger Entry] WHERE[Job No_] = @job AND [Posting Date] < @date1 and SUBSTRING([Job Task No_],1,5) = b.JobTaskNo and [Type of task]=2)	ELSE (select isnull(sum([Original Total Cost]), 0) FROM"+ Environment.GetEnvironmentVariable("Company") +"Job Ledger Entry] WHERE[Job No_] = @job AND [Posting Date] < @date1 and SUBSTRING([Job Task No_],1,5) = b.JobTaskNo and [Type of task]=2) END ) +((select isnull(sum([Original Total Cost]), 0) FROM "+ Environment.GetEnvironmentVariable("Company") +"Job Ledger Entry] WHERE [From Location] = @job AND [Posting Date] < @date1 and SUBSTRING([Job Task No_],1,5) = b.JobTaskNo and [Type of task]=1 and [Document No_] Like 'CAL%' )*2)) - ((CASE WHEN SUBSTRING(b.MainJobID, 1, 1) = 'N' or SUBSTRING(b.MainJobID,1,1)= 'O' THEN (select isnull(sum([Original Total Cost]) + sum([Line Amount]),0) FROM"+ Environment.GetEnvironmentVariable("Company") +"Job Ledger Entry] WHERE[Job No_] = @job AND [Posting Date] < @date1 and SUBSTRING([Job Task No_],1,5) = b.JobTaskNo and [Type of task]=1)	ELSE (select isnull(sum([Original Total Cost]), 0) FROM"+ Environment.GetEnvironmentVariable("Company") +"Job Ledger Entry] WHERE[Job No_] = @job AND [Posting Date] < @date1 and SUBSTRING([Job Task No_],1,5) = b.JobTaskNo and [Type of task]=1) END )+ (select isnull(sum([Original Total Cost]), 0) FROM "+ Environment.GetEnvironmentVariable("Company") +"Job Ledger Entry] WHERE [From Location] = @job AND [Posting Date] < @date1 and SUBSTRING([Job Task No_],1,5) = b.JobTaskNo and [Type of task]=3 and [Job No_] <> @job ))) AS CumBalAMT , " +
            //    " (SELECT isnull(SUM([Amount]),0) as Total FROM dbo."+ Environment.GetEnvironmentVariable("Company") +"Analysis View Entry] WHERE [Analysis View Code]='JOB COST A' and Convert(Time,[Posting Date])<>'23:59:59'  and [Posting Date]>=@start and [Posting Date] <=@date1 and [Dimension 1 Value Code]=@job and [Dimension 2 Value Code]<>'' and [Dimension 2 Value Code]=b.JobTaskNo) as GLAcc," +
            //    " ((SELECT isnull(SUM([Amount]),0) as Total FROM dbo."+ Environment.GetEnvironmentVariable("Company") +"Analysis View Entry] WHERE [Analysis View Code]='JOB COST A' and Convert(Time,[Posting Date])<>'23:59:59'  and [Posting Date]>=@start and [Posting Date] <=@date1 and [Dimension 1 Value Code]=@job and [Dimension 2 Value Code]<>'' and [Dimension 2 Value Code]=b.JobTaskNo) -" +
            //    " (((CASE WHEN SUBSTRING(b.MainJobID, 1, 1) = 'N' or SUBSTRING(b.MainJobID,1,1)= 'O' THEN (select isnull(sum([Original Total Cost]) + sum([Line Amount]),0) FROM"+ Environment.GetEnvironmentVariable("Company") +"Job Ledger Entry] WHERE [Job No_] = @job AND [Posting Date] > @start AND [Posting Date] < @date1 and SUBSTRING([Job Task No_],1,5) = b.JobTaskNo and [Type of task]=0 )	ELSE (select isnull(sum([Original Total Cost]), 0) FROM"+ Environment.GetEnvironmentVariable("Company") +"Job Ledger Entry] WHERE[Job No_] = @job AND [Posting Date] > @start AND [Posting Date] < @date1 and SUBSTRING([Job Task No_],1,5) = b.JobTaskNo and [Type of task]=0) END )+ (CASE WHEN SUBSTRING(b.MainJobID, 1, 1) = 'N' or SUBSTRING(b.MainJobID,1,1)= 'O' THEN (select isnull(sum([Original Total Cost]) + sum([Line Amount]),0) FROM"+ Environment.GetEnvironmentVariable("Company") +"Job Ledger Entry] WHERE[Job No_] = @job AND [Posting Date] > @start AND [Posting Date] < @date1 and SUBSTRING([Job Task No_],1,5) = b.JobTaskNo and [Type of task]=3 )	ELSE (select isnull(sum([Original Total Cost]), 0) FROM"+ Environment.GetEnvironmentVariable("Company") +"Job Ledger Entry] WHERE[Job No_] = @job AND [Posting Date] > @start AND [Posting Date] < @date1 and SUBSTRING([Job Task No_],1,5) = b.JobTaskNo and [Type of task]=3) END )+ (CASE WHEN SUBSTRING(b.MainJobID, 1, 1) = 'N' or SUBSTRING(b.MainJobID,1,1)= 'O' THEN (select isnull(sum([Original Total Cost]) + sum([Line Amount]),0) FROM"+ Environment.GetEnvironmentVariable("Company") +"Job Ledger Entry] WHERE[Job No_] = @job AND [Posting Date] < @date1 and SUBSTRING([Job Task No_],1,5) = b.JobTaskNo and [Type of task]=2)	ELSE (select isnull(sum([Original Total Cost]), 0) FROM"+ Environment.GetEnvironmentVariable("Company") +"Job Ledger Entry] WHERE[Job No_] = @job AND [Posting Date] < @date1 and SUBSTRING([Job Task No_],1,5) = b.JobTaskNo and [Type of task]=2) END ) +((select isnull(sum([Original Total Cost]), 0) FROM "+ Environment.GetEnvironmentVariable("Company") +"Job Ledger Entry] WHERE [From Location] = @job AND [Posting Date] < @date1 and SUBSTRING([Job Task No_],1,5) = b.JobTaskNo and [Type of task]=1 and [Document No_] Like 'CAL%' )*2)) - ((CASE WHEN SUBSTRING(b.MainJobID, 1, 1) = 'N' or SUBSTRING(b.MainJobID,1,1)= 'O' THEN (select isnull(sum([Original Total Cost]) + sum([Line Amount]),0) FROM"+ Environment.GetEnvironmentVariable("Company") +"Job Ledger Entry] WHERE[Job No_] = @job AND [Posting Date] < @date1 and SUBSTRING([Job Task No_],1,5) = b.JobTaskNo and [Type of task]=1)	ELSE (select isnull(sum([Original Total Cost]), 0) FROM"+ Environment.GetEnvironmentVariable("Company") +"Job Ledger Entry] WHERE[Job No_] = @job AND [Posting Date] < @date1 and SUBSTRING([Job Task No_],1,5) = b.JobTaskNo and [Type of task]=1) END )+ (select isnull(sum([Original Total Cost]), 0) FROM "+ Environment.GetEnvironmentVariable("Company") +"Job Ledger Entry] WHERE [From Location] = @job AND [Posting Date] < @date1 and SUBSTRING([Job Task No_],1,5) = b.JobTaskNo and [Type of task]=3 and [Job No_] <> @job )))) as Diff" +
            //    " FROM( SELECT a.JobTaskNo,a.MainJobID,a.SubJobID, " +
            //    " (SELECT TOP 1 dbo."+ Environment.GetEnvironmentVariable("Company") +"Job Task].Description FROM dbo."+ Environment.GetEnvironmentVariable("Company") +"Job Task] WHERE [Job Task No_]=a.JobTaskNo) as Description, " +
            //    " (SELECT TOP 1 ([Job Task No_]+' '+Description) FROM dbo."+ Environment.GetEnvironmentVariable("Company") +"Job Task] WHERE [Job Task No_]=a.JobTaskNo) as DescriptionFull, " +
            //    " (SELECT TOP 1 dbo."+ Environment.GetEnvironmentVariable("Company") +"Job Task].Description FROM dbo."+ Environment.GetEnvironmentVariable("Company") +"Job Task] WHERE SUBSTRING(dbo."+ Environment.GetEnvironmentVariable("Company") +"Job Task].[Job Task No_],1,1)+'0000'=a.MainJobID) as DescriptionMainJob, " +
            //    " (SELECT TOP 1 dbo."+ Environment.GetEnvironmentVariable("Company") +"Job Task].Description FROM dbo."+ Environment.GetEnvironmentVariable("Company") +"Job Task] WHERE SUBSTRING(dbo."+ Environment.GetEnvironmentVariable("Company") +"Job Task].[Job Task No_],1,3)+'00'=a.SubJobID) as DescriptionSubJob " +
            //    "  FROM (SELECT DISTINCT dbo."+ Environment.GetEnvironmentVariable("Company") +"Job Task].[Job Task No_] as JobTaskNo, SUBSTRING(dbo."+ Environment.GetEnvironmentVariable("Company") +"Job Task].[Job Task No_],1,1)+'0000' as MainJobID,SUBSTRING(dbo."+ Environment.GetEnvironmentVariable("Company") +"Job Task].[Job Task No_],1,3)+'00' as SubJobID FROM dbo."+ Environment.GetEnvironmentVariable("Company") +"Job Task]) as a) as b ORDER BY b.JobTaskNo";




            var queryData = "SELECT ROW_NUMBER() OVER (ORDER BY b.JobTaskNo) as ID,b.MainJobID,b.DescriptionMainJob,b.SubJobID,b.DescriptionSubJob,b.JobTaskNo,b.DescriptionFull," +
                "((((CASE WHEN SUBSTRING(b.MainJobID, 1, 1) = 'N' or SUBSTRING(b.MainJobID,1,1)= 'O' THEN (select isnull(sum([Original Total Cost]) + sum([Line Amount]),0) FROM"+ Environment.GetEnvironmentVariable("Company") + "Job Ledger Entry] WHERE [Job No_] = {0} AND [Posting Date] > {1} AND [Posting Date] < {2} and SUBSTRING([Job Task No_],1,5) = b.JobTaskNo and [Type of task]=0 )	ELSE (select isnull(sum([Original Total Cost]), 0) FROM" + Environment.GetEnvironmentVariable("Company") + "Job Ledger Entry] WHERE[Job No_] = {0} AND [Posting Date] > {1} AND [Posting Date] < {2} and SUBSTRING([Job Task No_],1,5) = b.JobTaskNo and [Type of task]=0) END )+ (CASE WHEN SUBSTRING(b.MainJobID, 1, 1) = 'N' or SUBSTRING(b.MainJobID,1,1)= 'O' THEN (select isnull(sum([Original Total Cost]) + sum([Line Amount]),0) FROM" + Environment.GetEnvironmentVariable("Company") + "Job Ledger Entry] WHERE[Job No_] = {0} AND [Posting Date] > {1} AND [Posting Date] < {2} and SUBSTRING([Job Task No_],1,5) = b.JobTaskNo and [Type of task]=3 )	ELSE (select isnull(sum([Original Total Cost]), 0) FROM" + Environment.GetEnvironmentVariable("Company") + "Job Ledger Entry] WHERE[Job No_] = {0} AND [Posting Date] > {1} AND [Posting Date] < {2} and SUBSTRING([Job Task No_],1,5) = b.JobTaskNo and [Type of task]=3) END )+ (CASE WHEN SUBSTRING(b.MainJobID, 1, 1) = 'N' or SUBSTRING(b.MainJobID,1,1)= 'O' THEN (select isnull(sum([Original Total Cost]) + sum([Line Amount]),0) FROM" + Environment.GetEnvironmentVariable("Company") + "Job Ledger Entry] WHERE[Job No_] = {0} AND [Posting Date] < {2} and SUBSTRING([Job Task No_],1,5) = b.JobTaskNo and [Type of task]=2)	ELSE (select isnull(sum([Original Total Cost]), 0) FROM" + Environment.GetEnvironmentVariable("Company") + "Job Ledger Entry] WHERE[Job No_] ={0} AND [Posting Date] < {2} and SUBSTRING([Job Task No_],1,5) = b.JobTaskNo and [Type of task]=2) END ) +((select isnull(sum([Original Total Cost]), 0) FROM " + Environment.GetEnvironmentVariable("Company") + "Job Ledger Entry] WHERE [From Location] = {0} AND [Posting Date] < {2} and SUBSTRING([Job Task No_],1,5) = b.JobTaskNo and [Type of task]=1 and [Document No_] Like 'CAL%' )*2)) - ((CASE WHEN SUBSTRING(b.MainJobID, 1, 1) = 'N' or SUBSTRING(b.MainJobID,1,1)= 'O' THEN (select isnull(sum([Original Total Cost]) + sum([Line Amount]),0) FROM" + Environment.GetEnvironmentVariable("Company") + "Job Ledger Entry] WHERE[Job No_] = {0} AND [Posting Date] < {2} and SUBSTRING([Job Task No_],1,5) = b.JobTaskNo and [Type of task]=1)	ELSE (select isnull(sum([Original Total Cost]), 0) FROM" + Environment.GetEnvironmentVariable("Company") + "Job Ledger Entry] WHERE[Job No_] = {0} AND [Posting Date] < {2} and SUBSTRING([Job Task No_],1,5) = b.JobTaskNo and [Type of task]=1) END )+ (select isnull(sum([Original Total Cost]), 0) FROM " + Environment.GetEnvironmentVariable("Company") + "Job Ledger Entry] WHERE [From Location] ={0} AND [Posting Date] < {2} and SUBSTRING([Job Task No_],1,5) = b.JobTaskNo and [Type of task]=3 and [Job No_] <>{0})))+" +
                " (CASE WHEN SUBSTRING(b.MainJobID, 1, 1) = 'N' or SUBSTRING(b.MainJobID,1,1)= 'O' THEN 0" +
                " ELSE " +
                " (select isnull(sum([Total Cost]), 0) FROM"+ Environment.GetEnvironmentVariable("Company") +"Job Ledger Entry] WHERE[Job No_] = {0} AND [Posting Date] < {2} and SUBSTRING([Job Task No_],1,5) = b.JobTaskNo and [Type of task]=0 and [Original Total Cost]=0) END " +
                " )) AS CumBalAMT , " +
                " (SELECT isnull(SUM([Amount]),0) as Total FROM dbo."+ Environment.GetEnvironmentVariable("Company") + "Analysis View Entry] WHERE [Analysis View Code]='JOB COST A' and Convert(Time,[Posting Date])<>'23:59:59'  and [Posting Date]>={1} and [Posting Date] <={2} and [Dimension 1 Value Code]={0} and [Dimension 2 Value Code]<>'' and [Dimension 2 Value Code]=b.JobTaskNo) as GLAcc," +
                "((SELECT isnull(SUM([Amount]),0) as Total FROM dbo."+ Environment.GetEnvironmentVariable("Company") + "Analysis View Entry] WHERE [Analysis View Code]='JOB COST A' and Convert(Time,[Posting Date])<>'23:59:59'  and [Posting Date]>={1} and [Posting Date] <={2} and [Dimension 1 Value Code]={0} and [Dimension 2 Value Code]<>'' and [Dimension 2 Value Code]=b.JobTaskNo) -" +
                "((((CASE WHEN SUBSTRING(b.MainJobID, 1, 1) = 'N' or SUBSTRING(b.MainJobID,1,1)= 'O' THEN (select isnull(sum([Original Total Cost]) + sum([Line Amount]),0) FROM"+ Environment.GetEnvironmentVariable("Company") + "Job Ledger Entry] WHERE [Job No_] = {0} AND [Posting Date] > {1} AND [Posting Date] < {2} and SUBSTRING([Job Task No_],1,5) = b.JobTaskNo and [Type of task]=0 )	ELSE (select isnull(sum([Original Total Cost]), 0) FROM" + Environment.GetEnvironmentVariable("Company") + "Job Ledger Entry] WHERE[Job No_] = {0} AND [Posting Date] > {1} AND [Posting Date] < {2} and SUBSTRING([Job Task No_],1,5) = b.JobTaskNo and [Type of task]=0) END )+ (CASE WHEN SUBSTRING(b.MainJobID, 1, 1) = 'N' or SUBSTRING(b.MainJobID,1,1)= 'O' THEN (select isnull(sum([Original Total Cost]) + sum([Line Amount]),0) FROM" + Environment.GetEnvironmentVariable("Company") + "Job Ledger Entry] WHERE[Job No_] = {0} AND [Posting Date] > {1} AND [Posting Date] < {2} and SUBSTRING([Job Task No_],1,5) = b.JobTaskNo and [Type of task]=3 )	ELSE (select isnull(sum([Original Total Cost]), 0) FROM" + Environment.GetEnvironmentVariable("Company") + "Job Ledger Entry] WHERE[Job No_] = {0} AND [Posting Date] > {1} AND [Posting Date] < {2} and SUBSTRING([Job Task No_],1,5) = b.JobTaskNo and [Type of task]=3) END )+ (CASE WHEN SUBSTRING(b.MainJobID, 1, 1) = 'N' or SUBSTRING(b.MainJobID,1,1)= 'O' THEN (select isnull(sum([Original Total Cost]) + sum([Line Amount]),0) FROM" + Environment.GetEnvironmentVariable("Company") + "Job Ledger Entry] WHERE[Job No_] = {0} AND [Posting Date] < {2} and SUBSTRING([Job Task No_],1,5) = b.JobTaskNo and [Type of task]=2)	ELSE (select isnull(sum([Original Total Cost]), 0) FROM" + Environment.GetEnvironmentVariable("Company") + "Job Ledger Entry] WHERE[Job No_] ={0} AND [Posting Date] < {2} and SUBSTRING([Job Task No_],1,5) = b.JobTaskNo and [Type of task]=2) END ) +((select isnull(sum([Original Total Cost]), 0) FROM " + Environment.GetEnvironmentVariable("Company") + "Job Ledger Entry] WHERE [From Location] = {0} AND [Posting Date] < {2} and SUBSTRING([Job Task No_],1,5) = b.JobTaskNo and [Type of task]=1 and [Document No_] Like 'CAL%' )*2)) - ((CASE WHEN SUBSTRING(b.MainJobID, 1, 1) = 'N' or SUBSTRING(b.MainJobID,1,1)= 'O' THEN (select isnull(sum([Original Total Cost]) + sum([Line Amount]),0) FROM" + Environment.GetEnvironmentVariable("Company") + "Job Ledger Entry] WHERE[Job No_] = {0} AND [Posting Date] < {2} and SUBSTRING([Job Task No_],1,5) = b.JobTaskNo and [Type of task]=1)	ELSE (select isnull(sum([Original Total Cost]), 0) FROM" + Environment.GetEnvironmentVariable("Company") + "Job Ledger Entry] WHERE[Job No_] = {0} AND [Posting Date] < {2} and SUBSTRING([Job Task No_],1,5) = b.JobTaskNo and [Type of task]=1) END )+ (select isnull(sum([Original Total Cost]), 0) FROM " + Environment.GetEnvironmentVariable("Company") + "Job Ledger Entry] WHERE [From Location] = {0} AND [Posting Date] < {2} and SUBSTRING([Job Task No_],1,5) = b.JobTaskNo and [Type of task]=3 and [Job No_] <> {0})))+" +
                "(CASE WHEN SUBSTRING(b.MainJobID, 1, 1) = 'N' or SUBSTRING(b.MainJobID,1,1)= 'O' THEN 0 " +
                " ELSE " +
                " 	(select isnull(sum([Total Cost]), 0) FROM"+ Environment.GetEnvironmentVariable("Company") + "Job Ledger Entry] WHERE[Job No_] = {0} AND [Posting Date] < {2} and SUBSTRING([Job Task No_],1,5) = b.JobTaskNo and [Type of task]=0 and [Original Total Cost]=0) END" +
                " ))) as Diff " +
                "  FROM( SELECT a.JobTaskNo,a.MainJobID,a.SubJobID, " +
                " (SELECT TOP 1 dbo."+ Environment.GetEnvironmentVariable("Company") +"Job Task].Description FROM dbo."+ Environment.GetEnvironmentVariable("Company") +"Job Task] WHERE [Job Task No_]=a.JobTaskNo) as Description, " +
                " (SELECT TOP 1 ([Job Task No_]+' '+Description) FROM dbo."+ Environment.GetEnvironmentVariable("Company") +"Job Task] WHERE [Job Task No_]=a.JobTaskNo) as DescriptionFull, " +
                " (SELECT TOP 1 dbo."+ Environment.GetEnvironmentVariable("Company") +"Job Task].Description FROM dbo."+ Environment.GetEnvironmentVariable("Company") +"Job Task] WHERE SUBSTRING(dbo."+ Environment.GetEnvironmentVariable("Company") +"Job Task].[Job Task No_],1,1)+'0000'=a.MainJobID) as DescriptionMainJob, " +
                " (SELECT TOP 1 dbo."+ Environment.GetEnvironmentVariable("Company") +"Job Task].Description FROM dbo."+ Environment.GetEnvironmentVariable("Company") +"Job Task] WHERE SUBSTRING(dbo."+ Environment.GetEnvironmentVariable("Company") +"Job Task].[Job Task No_],1,3)+'00'=a.SubJobID) as DescriptionSubJob " +
                "  FROM (SELECT DISTINCT dbo."+ Environment.GetEnvironmentVariable("Company") +"Job Task].[Job Task No_] as JobTaskNo, SUBSTRING(dbo."+ Environment.GetEnvironmentVariable("Company") +"Job Task].[Job Task No_],1,1)+'0000' as MainJobID,SUBSTRING(dbo."+ Environment.GetEnvironmentVariable("Company") +"Job Task].[Job Task No_],1,3)+'00' as SubJobID FROM dbo."+ Environment.GetEnvironmentVariable("Company") +"Job Task]) as a) as b ORDER BY b.JobTaskNo";


            //SqlParameter parameterJob = new SqlParameter("@job", JobNo);
            //SqlParameter parameterStart = new SqlParameter("@start", date1);
            //SqlParameter parameterDate1 = new SqlParameter("@date1", date2);






            ViewBag.sql = queryData;
            var checkCostDiffGLs = _navcontext.CheckCostDiffGLs.FromSqlRaw(queryData, JobNo, date1, date2).ToList();
            var table = "<table id='dt1' class='table'><thead><tr>"
             + "<th align ='center'>MainJobId</th>"
             + "<th align ='center'>DescriptMainJob</th>"
             + "<th align ='center'>SubJobID</th>"
             + "<th align ='center'>DescriptionSubJob</th>"
             + "<th align ='center'>JobTaskNo</th>"
             + "<th align ='center'>DescriptFull</th>"
             + "<th align ='center'>CumBalAMT</th>"
             + "<th align ='center'>GLAcc</th>"
             + "<th align ='center'>Diff</th>"
             + "</tr>"
             + "</thead>"
             + "<tbody>";


            foreach (var std in checkCostDiffGLs as IList<V_CheckCostDiffGL>)
            {

                table += "<tr>"
                          + "<td>" + std.MainJobID + "</td>"
                          + "<td>" + std.DescriptionMainJob + "</td>"
                          + "<td>" + std.SubJobID + "</td>"
                          + "<td>" + std.DescriptionSubJob + "</td>"
                          + "<td>" + std.JobTaskNo + "</td>"
                          + "<td>" + std.DescriptionFull + "</td>"
                          + "<td align ='right'>" + SetFontRed(std.CumBalAMT, 3) + "</td>"
                          + "<td align ='right'>" + SetFontRed(std.GLAcc, 3) + "</td >"
                          + "<td align ='right'>" + SetFontRed(std.Diff, 3) + "</td >"
                          + "</tr>";
            }

            table += "</tbody></table>";

            ViewBag.table = table;

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