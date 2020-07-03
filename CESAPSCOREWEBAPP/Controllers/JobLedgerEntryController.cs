using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using CESAPSCOREWEBAPP.Models;
using System.Data;
using System.Data.SqlClient;
using System.Diagnostics;
using System.Globalization;
using Microsoft.AspNetCore.Http;
using static CESAPSCOREWEBAPP.Models.Enums;
using CESAPSCOREWEBAPP.Helpers;
using Microsoft.AspNetCore.Hosting;
using System.IO;
using OfficeOpenXml;
using System.Net;
using System.Drawing;
using System.Text;
using OfficeOpenXml;
using OfficeOpenXml.Style;
using OfficeOpenXml.Table;

namespace CESAPSCOREWEBAPP.Controllers
{
    public class JobLedgerEntryController : BaseController
    {
        private readonly DatabaseContext _context;
        private readonly NAVContext _navcontext;
        private readonly NAVSuperContext _navsupercontext;


        private const string XlsxContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
        private readonly IHostingEnvironment _hostingEnvironment;


        public JobLedgerEntryController(DatabaseContext context, NAVContext navcontext,IHostingEnvironment hostingEnvironment,NAVSuperContext navsupercontext)
        {
            _context = context;
            _navcontext = navcontext;
            _hostingEnvironment = hostingEnvironment;
            _navsupercontext = navsupercontext;
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



            //var query = "SELECT dbo."+ Environment.GetEnvironmentVariable("Company") +"Job].No_ AS JobNo,dbo."+ Environment.GetEnvironmentVariable("Company") +"Job].[Location Code] AS LocationCode FROM dbo."+ Environment.GetEnvironmentVariable("Company") +"Job]";
            //ViewData["JobNo"] = new SelectList(_navcontext.v_Job.FromSqlRaw(query), "JobNo", "LocationCode");
            ViewData["JobNo"] = new SelectList(_context.UserJobs.Where(s => s.UserId == HttpContext.Session.GetInt32("Userid")).OrderBy(s => s.UserJobDetail).ToList(), "UserJobDetail", "UserJobDetail");



            ViewBag.StartDate = DateTime.Now.ToString("01-MM-yyyy", new CultureInfo("en-US")); 
            ViewBag.EndDate= DateTime.Now.ToString("dd-MM-yyyy", new CultureInfo("en-US"));

            return View();
        }





        // GET: JobLedgerEntry/Details/5
        public async Task<IActionResult> Details(int? id)
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

            if (id == null)
            {
                return NotFound();
            }

            var v_JobLedgerEntry = await _navcontext.V_JobLedgerEntry
                .FirstOrDefaultAsync(m => m.EntryID == id);
            if (v_JobLedgerEntry == null)
            {
                return NotFound();
            }

            return View(v_JobLedgerEntry);
        }

        // GET: JobLedgerEntry/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: JobLedgerEntry/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Index(string JobNo, string Startdate, string EndDate)
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
            IActionResult response = Unauthorized();


            /*Check Session */
            ViewData["JobNo"] = new SelectList(_context.UserJobs.Where(s => s.UserId == HttpContext.Session.GetInt32("Userid")).OrderBy(s => s.UserJobDetail).ToList(), "UserJobDetail", "UserJobDetail", JobNo);
            var date1 = Startdate.Substring(6,4) +"-"+Startdate.Substring(3,2)+"-"+Startdate.Substring(0,2)+" 00:00:00";
            var date2 = EndDate.Substring(6,4) +"-"+EndDate.Substring(3,2)+"-"+EndDate.Substring(0,2)+" 23:59:59";
            var sdate1 = Startdate;
            var sdate2 = EndDate;
            var rdate1 = Startdate;
            var rdate2 = EndDate;
            ViewBag.StartDate = rdate1;
            ViewBag.EndDate = rdate2;
            ViewBag.SStartDate = Startdate;
            ViewBag.SEndDate = EndDate;
            ViewBag.Job = JobNo;

            var queryData = " SELECT ROW_NUMBER() OVER (ORDER BY b.JobTaskNo) as ID," +
                "b.MainJobID,b.DescriptionMainJob,b.SubJobID,b.DescriptionSubJob,b.JobTaskNo,b.DescriptionFull,"
                        + " (((CASE WHEN SUBSTRING(b.MainJobID, 1, 1) = 'N' or SUBSTRING(b.MainJobID,1,1)= 'O' THEN "
                        + " (select isnull(sum([Quantity]) + sum([Line Amount]),0) FROM"+ Environment.GetEnvironmentVariable("Company") +"Job Ledger Entry] WHERE [Job No_] = {0} AND [Posting Date] > {1} AND [Posting Date] < {2} and SUBSTRING([Job Task No_],1,5) = b.JobTaskNo and [Type of task]=0 )	"
                        + " ELSE "
                        + " (select isnull(sum([Quantity]), 0) FROM"+ Environment.GetEnvironmentVariable("Company") +"Job Ledger Entry] WHERE[Job No_] = {0} AND [Posting Date] > {1} AND [Posting Date] < {2} and SUBSTRING([Job Task No_],1,5) = b.JobTaskNo and [Type of task]=0) END "
                        + " )+"
                        + " (CASE WHEN SUBSTRING(b.MainJobID, 1, 1) = 'N' or SUBSTRING(b.MainJobID,1,1)= 'O' THEN "
                        + " (select isnull(sum([Quantity]) + sum([Line Amount]),0) FROM"+ Environment.GetEnvironmentVariable("Company") +"Job Ledger Entry] WHERE[Job No_] = {0} AND [Posting Date] > {1} AND [Posting Date] < {2} and SUBSTRING([Job Task No_],1,5) = b.JobTaskNo and [Type of task]=3 )	"
                        + " ELSE "
                        + "	(select isnull(sum([Quantity]), 0) FROM"+ Environment.GetEnvironmentVariable("Company") +"Job Ledger Entry] WHERE[Job No_] = {0} AND [Posting Date] > {1} AND [Posting Date] < {2} and SUBSTRING([Job Task No_],1,5) = b.JobTaskNo and [Type of task]=3) END "
                        + " )+"
                        + " (CASE WHEN SUBSTRING(b.MainJobID, 1, 1) = 'N' or SUBSTRING(b.MainJobID,1,1)= 'O' THEN "
                        + " (select isnull(sum([Quantity]) + sum([Line Amount]),0) FROM"+ Environment.GetEnvironmentVariable("Company") +"Job Ledger Entry] WHERE[Job No_] = {0} AND [Posting Date] > {1} AND [Posting Date] < {2} and SUBSTRING([Job Task No_],1,5) = b.JobTaskNo and [Type of task]=2)	"
                        + " ELSE "
                        + " (select isnull(sum([Quantity]), 0) FROM"+ Environment.GetEnvironmentVariable("Company") +"Job Ledger Entry] WHERE[Job No_] = {0} AND [Posting Date] > {1} AND [Posting Date] < {2} and SUBSTRING([Job Task No_],1,5) = b.JobTaskNo and [Type of task]=2) END "
                        + " )" +
                        "+((select isnull(sum([Quantity]), 0) FROM "+ Environment.GetEnvironmentVariable("Company") +"Job Ledger Entry] WHERE [From Location] = {0} AND [Posting Date] > {1} AND [Posting Date] < {2} and SUBSTRING([Job Task No_],1,5) = b.JobTaskNo and [Type of task]=1 and [Document No_] Like 'CAL%' )*2)) -"
                        + " ((CASE WHEN SUBSTRING(b.MainJobID, 1, 1) = 'N' or SUBSTRING(b.MainJobID,1,1)= 'O' THEN "
                        + " (select isnull(sum([Quantity]) + sum([Line Amount]),0) FROM"+ Environment.GetEnvironmentVariable("Company") +"Job Ledger Entry] WHERE[Job No_] = {0} AND [Posting Date] > {1} AND [Posting Date] < {2} and SUBSTRING([Job Task No_],1,5) = b.JobTaskNo and [Type of task]=1)	"
                        + " ELSE "
                        + " (select isnull(sum([Quantity]), 0) FROM"+ Environment.GetEnvironmentVariable("Company") +"Job Ledger Entry] WHERE[Job No_] = {0} AND [Posting Date] > {1} AND [Posting Date] < {2} and SUBSTRING([Job Task No_],1,5) = b.JobTaskNo and [Type of task]=1) END "
                        + " )+ "
                        + " (select isnull(sum([Quantity]), 0) FROM "+ Environment.GetEnvironmentVariable("Company") +"Job Ledger Entry] WHERE [From Location] = {0} AND [Posting Date] > {1} AND [Posting Date] < {2} and SUBSTRING([Job Task No_],1,5) = b.JobTaskNo and [Type of task]=3 and [Job No_] <> {0} )))  AS ThisPeriodQTY "
                        + " , "

                        + " (((CASE WHEN SUBSTRING(b.MainJobID, 1, 1) = 'N' or SUBSTRING(b.MainJobID,1,1)= 'O' THEN "
                        + " (select isnull(sum([Original Total Cost]) + sum([Line Amount]),0) FROM"+ Environment.GetEnvironmentVariable("Company") +"Job Ledger Entry] WHERE [Job No_] = {0} AND [Posting Date] > {1} AND [Posting Date] < {2} and SUBSTRING([Job Task No_],1,5) = b.JobTaskNo and [Type of task]=0 )	"
                        + " ELSE "
                        + " (select isnull(sum([Original Total Cost]), 0) FROM"+ Environment.GetEnvironmentVariable("Company") +"Job Ledger Entry] WHERE[Job No_] = {0} AND [Posting Date] > {1} AND [Posting Date] < {2} and SUBSTRING([Job Task No_],1,5) = b.JobTaskNo and [Type of task]=0) END "
                        + " )+"
                        + " (CASE WHEN SUBSTRING(b.MainJobID, 1, 1) = 'N' or SUBSTRING(b.MainJobID,1,1)= 'O' THEN "
                        + " (select isnull(sum([Original Total Cost]) + sum([Line Amount]),0) FROM"+ Environment.GetEnvironmentVariable("Company") +"Job Ledger Entry] WHERE[Job No_] = {0} AND [Posting Date] > {1} AND [Posting Date] < {2} and SUBSTRING([Job Task No_],1,5) = b.JobTaskNo and [Type of task]=3 )	"
                        + " ELSE "
                        + "	(select isnull(sum([Original Total Cost]), 0) FROM"+ Environment.GetEnvironmentVariable("Company") +"Job Ledger Entry] WHERE[Job No_] = {0} AND [Posting Date] > {1} AND [Posting Date] < {2} and SUBSTRING([Job Task No_],1,5) = b.JobTaskNo and [Type of task]=3) END "
                        + " )+"
                        + " (CASE WHEN SUBSTRING(b.MainJobID, 1, 1) = 'N' or SUBSTRING(b.MainJobID,1,1)= 'O' THEN "
                        + " (select isnull(sum([Original Total Cost]) + sum([Line Amount]),0) FROM"+ Environment.GetEnvironmentVariable("Company") +"Job Ledger Entry] WHERE[Job No_] = {0} AND [Posting Date] > {1} AND [Posting Date] < {2} and SUBSTRING([Job Task No_],1,5) = b.JobTaskNo and [Type of task]=2)	"
                        + " ELSE "
                        + " (select isnull(sum([Original Total Cost]), 0) FROM"+ Environment.GetEnvironmentVariable("Company") +"Job Ledger Entry] WHERE[Job No_] = {0} AND [Posting Date] > {1} AND [Posting Date] < {2} and SUBSTRING([Job Task No_],1,5) = b.JobTaskNo and [Type of task]=2) END "
                        + " )"
                        + "+((select isnull(sum([Original Total Cost]), 0) FROM "+ Environment.GetEnvironmentVariable("Company") +"Job Ledger Entry] WHERE [From Location] = {0} AND [Posting Date] > {1} AND [Posting Date] < {2} and SUBSTRING([Job Task No_],1,5) = b.JobTaskNo and [Type of task]=1 and [Document No_] Like 'CAL%' )*2)) -"
                        + " ((CASE WHEN SUBSTRING(b.MainJobID, 1, 1) = 'N' or SUBSTRING(b.MainJobID,1,1)= 'O' THEN "
                        + " (select isnull(sum([Original Total Cost]) + sum([Line Amount]),0) FROM"+ Environment.GetEnvironmentVariable("Company") +"Job Ledger Entry] WHERE[Job No_] = {0} AND [Posting Date] > {1} AND [Posting Date] < {2} and SUBSTRING([Job Task No_],1,5) = b.JobTaskNo and [Type of task]=1)	"
                        + " ELSE "
                        + " (select isnull(sum([Original Total Cost]), 0) FROM"+ Environment.GetEnvironmentVariable("Company") +"Job Ledger Entry] WHERE[Job No_] = {0} AND [Posting Date] > {1} AND [Posting Date] < {2} and SUBSTRING([Job Task No_],1,5) = b.JobTaskNo and [Type of task]=1) END "
                        + " )+ "
                        + " (select isnull(sum([Original Total Cost]), 0) FROM "+ Environment.GetEnvironmentVariable("Company") +"Job Ledger Entry] WHERE [From Location] = {0} AND [Posting Date] > {1} AND [Posting Date] < {2} and SUBSTRING([Job Task No_],1,5) = b.JobTaskNo and [Type of task]=3 and [Job No_] <> {0} ))" +
                  
                          "+" 
                          + "(CASE WHEN SUBSTRING(b.MainJobID, 1, 1) = 'N' or SUBSTRING(b.MainJobID,1,1)= 'O' THEN 0 ELSE  	(select isnull(sum([Total Cost]), 0) FROM"+ Environment.GetEnvironmentVariable("Company") +"Job Ledger Entry] WHERE[Job No_] = {0}  AND [Posting Date] > {1} AND [Posting Date] < {2}  and SUBSTRING([Job Task No_],1,5) = b.JobTaskNo and [Type of task]=0 and [Original Total Cost]=0) END "
                         + " ))  AS ThisPeriodAMT "
                
                        + " , "

                        + " (((CASE WHEN SUBSTRING(b.MainJobID, 1, 1) = 'N' or SUBSTRING(b.MainJobID,1,1)= 'O' THEN "
                        + " (select isnull(sum([Quantity]) + sum([Line Amount]),0) FROM"+ Environment.GetEnvironmentVariable("Company") +"Job Ledger Entry] WHERE [Job No_] = {0} AND [Posting Date] < {1} and SUBSTRING([Job Task No_],1,5) = b.JobTaskNo and [Type of task]=0 )	"
                        + " ELSE "
                        + " (select isnull(sum([Quantity]), 0) FROM"+ Environment.GetEnvironmentVariable("Company") +"Job Ledger Entry] WHERE[Job No_] = {0} AND [Posting Date] < {1} and SUBSTRING([Job Task No_],1,5) = b.JobTaskNo and [Type of task]=0) END "
                        + " )+"
                        + " (CASE WHEN SUBSTRING(b.MainJobID, 1, 1) = 'N' or SUBSTRING(b.MainJobID,1,1)= 'O' THEN "
                        + " (select isnull(sum([Quantity]) + sum([Line Amount]),0) FROM"+ Environment.GetEnvironmentVariable("Company") +"Job Ledger Entry] WHERE[Job No_] = {0} AND [Posting Date] < {1} and SUBSTRING([Job Task No_],1,5) = b.JobTaskNo and [Type of task]=3 )	"
                        + " ELSE "
                        + "	(select isnull(sum([Quantity]), 0) FROM"+ Environment.GetEnvironmentVariable("Company") +"Job Ledger Entry] WHERE[Job No_] = {0} AND[Posting Date] < {1} and SUBSTRING([Job Task No_],1,5) = b.JobTaskNo and [Type of task]=3) END "
                        + " )+"
                        + " (CASE WHEN SUBSTRING(b.MainJobID, 1, 1) = 'N' or SUBSTRING(b.MainJobID,1,1)= 'O' THEN "
                        + " (select isnull(sum([Quantity]) + sum([Line Amount]),0) FROM"+ Environment.GetEnvironmentVariable("Company") +"Job Ledger Entry] WHERE[Job No_] = {0} AND [Posting Date] < {1} and SUBSTRING([Job Task No_],1,5) = b.JobTaskNo and [Type of task]=2)	"
                        + " ELSE "
                        + " (select isnull(sum([Quantity]), 0) FROM"+ Environment.GetEnvironmentVariable("Company") +"Job Ledger Entry] WHERE[Job No_] = {0} AND [Posting Date] < {1} and SUBSTRING([Job Task No_],1,5) = b.JobTaskNo and [Type of task]=2) END "
                        + " )"
                        + "+((select isnull(sum([Quantity]), 0) FROM "+ Environment.GetEnvironmentVariable("Company") +"Job Ledger Entry] WHERE [From Location] = {0} AND [Posting Date] < {1}  and SUBSTRING([Job Task No_],1,5) = b.JobTaskNo and [Type of task]=1 and [Document No_] Like 'CAL%' )*2)) -"
                        + " ((CASE WHEN SUBSTRING(b.MainJobID, 1, 1) = 'N' or SUBSTRING(b.MainJobID,1,1)= 'O' THEN "
                        + " (select isnull(sum([Quantity]) + sum([Line Amount]),0) FROM"+ Environment.GetEnvironmentVariable("Company") +"Job Ledger Entry] WHERE[Job No_] = {0} AND [Posting Date] < {1} and SUBSTRING([Job Task No_],1,5) = b.JobTaskNo and [Type of task]=1)	"
                        + " ELSE "
                        + " (select isnull(sum([Quantity]), 0) FROM"+ Environment.GetEnvironmentVariable("Company") +"Job Ledger Entry] WHERE[Job No_] = {0} AND [Posting Date] < {1} and SUBSTRING([Job Task No_],1,5) = b.JobTaskNo and [Type of task]=1) END "
                        + " )+ "
                        + " (select isnull(sum([Quantity]), 0) FROM "+ Environment.GetEnvironmentVariable("Company") +"Job Ledger Entry] WHERE [From Location] = {0} AND [Posting Date] < {1} and SUBSTRING([Job Task No_],1,5) = b.JobTaskNo and [Type of task]=3 and [Job No_] <> {0} )))  AS OpeningQTY "
                        + " , "

                        + " (((CASE WHEN SUBSTRING(b.MainJobID, 1, 1) = 'N' or SUBSTRING(b.MainJobID,1,1)= 'O' THEN "
                        + " (select isnull(sum([Original Total Cost]) + sum([Line Amount]),0) FROM"+ Environment.GetEnvironmentVariable("Company") +"Job Ledger Entry] WHERE [Job No_] = {0} AND  [Posting Date] < {1} and SUBSTRING([Job Task No_],1,5) = b.JobTaskNo and [Type of task]=0 )	"
                        + " ELSE "
                        + " (select isnull(sum([Original Total Cost]), 0) FROM"+ Environment.GetEnvironmentVariable("Company") +"Job Ledger Entry] WHERE[Job No_] = {0} AND [Posting Date] < {1} and SUBSTRING([Job Task No_],1,5) = b.JobTaskNo and [Type of task]=0) END "
                        + " )+"
                        + " (CASE WHEN SUBSTRING(b.MainJobID, 1, 1) = 'N' or SUBSTRING(b.MainJobID,1,1)= 'O' THEN "
                        + " (select isnull(sum([Original Total Cost]) + sum([Line Amount]),0) FROM"+ Environment.GetEnvironmentVariable("Company") +"Job Ledger Entry] WHERE[Job No_] = {0} AND [Posting Date] < {1} and SUBSTRING([Job Task No_],1,5) = b.JobTaskNo and [Type of task]=3 )	"
                        + " ELSE "
                        + "	(select isnull(sum([Original Total Cost]), 0) FROM"+ Environment.GetEnvironmentVariable("Company") +"Job Ledger Entry] WHERE[Job No_] = {0} AND [Posting Date] < {1} and SUBSTRING([Job Task No_],1,5) = b.JobTaskNo and [Type of task]=3) END "
                        + " )+"
                        + " (CASE WHEN SUBSTRING(b.MainJobID, 1, 1) = 'N' or SUBSTRING(b.MainJobID,1,1)= 'O' THEN "
                        + " (select isnull(sum([Original Total Cost]) + sum([Line Amount]),0) FROM"+ Environment.GetEnvironmentVariable("Company") +"Job Ledger Entry] WHERE[Job No_] = {0} AND [Posting Date] < {1} and SUBSTRING([Job Task No_],1,5) = b.JobTaskNo and [Type of task]=2)	"
                        + " ELSE "
                        + " (select isnull(sum([Original Total Cost]), 0) FROM"+ Environment.GetEnvironmentVariable("Company") +"Job Ledger Entry] WHERE[Job No_] = {0} AND [Posting Date] < {1} and SUBSTRING([Job Task No_],1,5) = b.JobTaskNo and [Type of task]=2) END "
                        + " )"
                        + " +((select isnull(sum([Original Total Cost]), 0) FROM "+ Environment.GetEnvironmentVariable("Company") +"Job Ledger Entry] WHERE [From Location] = {0} AND [Posting Date] < {1}  and SUBSTRING([Job Task No_],1,5) = b.JobTaskNo and [Type of task]=1 and [Document No_] Like 'CAL%' )*2)) -"
                        + " ((CASE WHEN SUBSTRING(b.MainJobID, 1, 1) = 'N' or SUBSTRING(b.MainJobID,1,1)= 'O' THEN "
                        + " (select isnull(sum([Original Total Cost]) + sum([Line Amount]),0) FROM"+ Environment.GetEnvironmentVariable("Company") +"Job Ledger Entry] WHERE[Job No_] = {0} AND [Posting Date] < {1} and SUBSTRING([Job Task No_],1,5) = b.JobTaskNo and [Type of task]=1)	"
                        + " ELSE "
                        + " (select isnull(sum([Original Total Cost]), 0) FROM"+ Environment.GetEnvironmentVariable("Company") +"Job Ledger Entry] WHERE[Job No_] = {0} AND [Posting Date] < {1} and SUBSTRING([Job Task No_],1,5) = b.JobTaskNo and [Type of task]=1) END "
                        + " )+ "
                        + " (select isnull(sum([Original Total Cost]), 0) FROM "+ Environment.GetEnvironmentVariable("Company") +"Job Ledger Entry] WHERE [From Location] = {0} AND [Posting Date] < {1} and SUBSTRING([Job Task No_],1,5) = b.JobTaskNo and [Type of task]=3 and [Job No_] <> {0} ))" +
                            "+" +
                          "(CASE WHEN SUBSTRING(b.MainJobID, 1, 1) = 'N' or SUBSTRING(b.MainJobID,1,1)= 'O' THEN 0 ELSE  	(select isnull(sum([Total Cost]), 0) FROM"+ Environment.GetEnvironmentVariable("Company") +"Job Ledger Entry] WHERE[Job No_] = {0} AND [Posting Date] < {1} and SUBSTRING([Job Task No_],1,5) = b.JobTaskNo and [Type of task]=0 and [Original Total Cost]=0) END" +
                          " ))  AS OpeningAMT "
                      


                        
                        
                        
                        + " , "

                        + " (((CASE WHEN SUBSTRING(b.MainJobID, 1, 1) = 'N' or SUBSTRING(b.MainJobID,1,1)= 'O' THEN "
                        + " (select isnull(sum([Quantity]) + sum([Line Amount]),0) FROM"+ Environment.GetEnvironmentVariable("Company") +"Job Ledger Entry] WHERE [Job No_] = {0} AND [Posting Date] < {2} and SUBSTRING([Job Task No_],1,5) = b.JobTaskNo and [Type of task]=0 )	"
                        + " ELSE "
                        + " (select isnull(sum([Quantity]), 0) FROM"+ Environment.GetEnvironmentVariable("Company") +"Job Ledger Entry] WHERE[Job No_] = {0} AND [Posting Date] < {2} and SUBSTRING([Job Task No_],1,5) = b.JobTaskNo and [Type of task]=0) END "
                        + " )+"
                        + " (CASE WHEN SUBSTRING(b.MainJobID, 1, 1) = 'N' or SUBSTRING(b.MainJobID,1,1)= 'O' THEN "
                        + " (select isnull(sum([Quantity]) + sum([Line Amount]),0) FROM"+ Environment.GetEnvironmentVariable("Company") +"Job Ledger Entry] WHERE[Job No_] = {0} AND [Posting Date] < {2} and SUBSTRING([Job Task No_],1,5) = b.JobTaskNo and [Type of task]=3 )	"
                        + " ELSE "
                        + "	(select isnull(sum([Quantity]), 0) FROM"+ Environment.GetEnvironmentVariable("Company") +"Job Ledger Entry] WHERE[Job No_] = {0} AND[Posting Date] < {2} and SUBSTRING([Job Task No_],1,5) = b.JobTaskNo and [Type of task]=3) END "
                        + " )+"
                        + " (CASE WHEN SUBSTRING(b.MainJobID, 1, 1) = 'N' or SUBSTRING(b.MainJobID,1,1)= 'O' THEN "
                        + " (select isnull(sum([Quantity]) + sum([Line Amount]),0) FROM"+ Environment.GetEnvironmentVariable("Company") +"Job Ledger Entry] WHERE[Job No_] = {0} AND [Posting Date] < {2} and SUBSTRING([Job Task No_],1,5) = b.JobTaskNo and [Type of task]=2)	"
                        + " ELSE "
                        + " (select isnull(sum([Quantity]), 0) FROM"+ Environment.GetEnvironmentVariable("Company") +"Job Ledger Entry] WHERE[Job No_] = {0} AND [Posting Date] < {2} and SUBSTRING([Job Task No_],1,5) = b.JobTaskNo and [Type of task]=2) END "
                        + " )) -"
                        + " ((CASE WHEN SUBSTRING(b.MainJobID, 1, 1) = 'N' or SUBSTRING(b.MainJobID,1,1)= 'O' THEN "
                        + " (select isnull(sum([Quantity]) + sum([Line Amount]),0) FROM"+ Environment.GetEnvironmentVariable("Company") +"Job Ledger Entry] WHERE[Job No_] = {0} AND [Posting Date] < {2} and SUBSTRING([Job Task No_],1,5) = b.JobTaskNo and [Type of task]=1)	"
                        + " ELSE "
                        + " (select isnull(sum([Quantity]), 0) FROM"+ Environment.GetEnvironmentVariable("Company") +"Job Ledger Entry] WHERE[Job No_] = {0} AND [Posting Date] < {2} and SUBSTRING([Job Task No_],1,5) = b.JobTaskNo and [Type of task]=1) END "
                        + " )+ "
                        + " (select isnull(sum([Quantity]), 0) FROM "+ Environment.GetEnvironmentVariable("Company") +"Job Ledger Entry] WHERE [From Location] = {0} AND [Posting Date] < {2} and SUBSTRING([Job Task No_],1,5) = b.JobTaskNo and [Type of task]=3 and [Job No_] <> {0} )))  AS CumBalQTY "
                        + " , "

                          + " (((CASE WHEN SUBSTRING(b.MainJobID, 1, 1) = 'N' or SUBSTRING(b.MainJobID,1,1)= 'O' THEN "
                          + " (select isnull(sum([Original Total Cost]) + sum([Line Amount]),0) FROM"+ Environment.GetEnvironmentVariable("Company") +"Job Ledger Entry] WHERE [Job No_] = {0} AND  [Posting Date] < {2} and SUBSTRING([Job Task No_],1,5) = b.JobTaskNo and [Type of task]=0 )	"
                          + " ELSE "
                          + " (select isnull(sum([Original Total Cost]), 0) FROM"+ Environment.GetEnvironmentVariable("Company") +"Job Ledger Entry] WHERE[Job No_] = {0} AND [Posting Date] < {2} and SUBSTRING([Job Task No_],1,5) = b.JobTaskNo and [Type of task]=0) END "
                          + " )+"
                          + " (CASE WHEN SUBSTRING(b.MainJobID, 1, 1) = 'N' or SUBSTRING(b.MainJobID,1,1)= 'O' THEN "
                          + " (select isnull(sum([Original Total Cost]) + sum([Line Amount]),0) FROM"+ Environment.GetEnvironmentVariable("Company") +"Job Ledger Entry] WHERE[Job No_] = {0} AND [Posting Date] < {2} and SUBSTRING([Job Task No_],1,5) = b.JobTaskNo and [Type of task]=3 )	"
                          + " ELSE "
                          + "	(select isnull(sum([Original Total Cost]), 0) FROM"+ Environment.GetEnvironmentVariable("Company") +"Job Ledger Entry] WHERE[Job No_] = {0} AND [Posting Date] < {2} and SUBSTRING([Job Task No_],1,5) = b.JobTaskNo and [Type of task]=3) END "
                          + " )+"
                          + " (CASE WHEN SUBSTRING(b.MainJobID, 1, 1) = 'N' or SUBSTRING(b.MainJobID,1,1)= 'O' THEN "
                          + " (select isnull(sum([Original Total Cost]) + sum([Line Amount]),0) FROM"+ Environment.GetEnvironmentVariable("Company") +"Job Ledger Entry] WHERE[Job No_] = {0} AND [Posting Date] < {2} and SUBSTRING([Job Task No_],1,5) = b.JobTaskNo and [Type of task]=2)	"
                          + " ELSE "
                          + " (select isnull(sum([Original Total Cost]), 0) FROM"+ Environment.GetEnvironmentVariable("Company") +"Job Ledger Entry] WHERE[Job No_] = {0} AND [Posting Date] < {2} and SUBSTRING([Job Task No_],1,5) = b.JobTaskNo and [Type of task]=2) END "
                          + " )"
                          + " +((select isnull(sum([Original Total Cost]), 0) FROM "+ Environment.GetEnvironmentVariable("Company") +"Job Ledger Entry] WHERE [From Location] = {0} AND [Posting Date] < {2}  and SUBSTRING([Job Task No_],1,5) = b.JobTaskNo and [Type of task]=1 and [Document No_] Like 'CAL%' )*2)) -"
                          + " ((CASE WHEN SUBSTRING(b.MainJobID, 1, 1) = 'N' or SUBSTRING(b.MainJobID,1,1)= 'O' THEN "
                          + " (select isnull(sum([Original Total Cost]) + sum([Line Amount]),0) FROM"+ Environment.GetEnvironmentVariable("Company") +"Job Ledger Entry] WHERE[Job No_] = {0} AND [Posting Date] < {2} and SUBSTRING([Job Task No_],1,5) = b.JobTaskNo and [Type of task]=1)	"
                          + " ELSE "
                          + " (select isnull(sum([Original Total Cost]), 0) FROM"+ Environment.GetEnvironmentVariable("Company") +"Job Ledger Entry] WHERE[Job No_] = {0} AND [Posting Date] < {2} and SUBSTRING([Job Task No_],1,5) = b.JobTaskNo and [Type of task]=1) END "
                          + " )+ "
                          + " (select isnull(sum([Original Total Cost]), 0) FROM "+ Environment.GetEnvironmentVariable("Company") +"Job Ledger Entry] WHERE [From Location] = {0} AND [Posting Date] < {2} and SUBSTRING([Job Task No_],1,5) = b.JobTaskNo and [Type of task]=3 and [Job No_] <> {0} ))" +
                          
                          
                          "+" +
                          "(CASE WHEN SUBSTRING(b.MainJobID, 1, 1) = 'N' or SUBSTRING(b.MainJobID,1,1)= 'O' THEN 0 ELSE  	(select isnull(sum([Total Cost]), 0) FROM"+ Environment.GetEnvironmentVariable("Company") +"Job Ledger Entry] WHERE[Job No_] = {0} AND [Posting Date] < {2} and SUBSTRING([Job Task No_],1,5) = b.JobTaskNo and [Type of task]=0 and [Original Total Cost]=0) END ))  AS CumBalAMT "
                        + " , "

                        + " (SELECT isnull(sum(Quantity), 0) FROM dbo."+ Environment.GetEnvironmentVariable("Company") +"Job Planning Line] WHERE[Job No_]={0} and SUBSTRING([Job Task No_],1,5)=b.JobTaskNo) as BudgetQTY, "
                        + " (SELECT isnull(sum([Total Cost (LCY)]),0) FROM dbo."+ Environment.GetEnvironmentVariable("Company") +"Job Planning Line] WHERE[Job No_]= {0} and SUBSTRING([Job Task No_],1,5)= b.JobTaskNo) as BudgetAMT "

                        + " FROM("
                         + " SELECT a.JobTaskNo,a.MainJobID,a.SubJobID,"
                         + " (SELECT TOP 1 dbo."+ Environment.GetEnvironmentVariable("Company") +"Job Task].Description FROM dbo."+ Environment.GetEnvironmentVariable("Company") +"Job Task] WHERE [Job Task No_]=a.JobTaskNo) as Description,"
                         + " (SELECT TOP 1 ([Job Task No_]+' '+Description) FROM dbo."+ Environment.GetEnvironmentVariable("Company") +"Job Task] WHERE [Job Task No_]=a.JobTaskNo) as DescriptionFull,"
                         + " (SELECT TOP 1 dbo."+ Environment.GetEnvironmentVariable("Company") +"Job Task].Description FROM dbo."+ Environment.GetEnvironmentVariable("Company") +"Job Task] WHERE SUBSTRING(dbo."+ Environment.GetEnvironmentVariable("Company") +"Job Task].[Job Task No_],1,1)+'0000'=a.MainJobID) as DescriptionMainJob,"
                         + " (SELECT TOP 1 dbo."+ Environment.GetEnvironmentVariable("Company") +"Job Task].Description FROM dbo."+ Environment.GetEnvironmentVariable("Company") +"Job Task] WHERE SUBSTRING(dbo."+ Environment.GetEnvironmentVariable("Company") +"Job Task].[Job Task No_],1,3)+'00'=a.SubJobID) as DescriptionSubJob"
                         + " FROM  (SELECT DISTINCT dbo."+ Environment.GetEnvironmentVariable("Company") +"Job Task].[Job Task No_] as JobTaskNo,"
                         + " SUBSTRING(dbo."+ Environment.GetEnvironmentVariable("Company") +"Job Task].[Job Task No_],1,1)+'0000' as MainJobID,SUBSTRING(dbo."+ Environment.GetEnvironmentVariable("Company") +"Job Task].[Job Task No_],1,3)+'00' as SubJobID FROM dbo."+ Environment.GetEnvironmentVariable("Company") +"Job Task]) as a) as b ORDER BY b.JobTaskNo";





            //var queryData=  " SELECT b.MainJobID,b.DescriptionMainJob,b.SubJobID,b.DescriptionSubJob,b.JobTaskNo,b.DescriptionFull,"
            //                + " (((CASE WHEN SUBSTRING(b.MainJobID, 1, 1) = 'N' or SUBSTRING(b.MainJobID,1,1)= 'O' THEN "
            //                + " (select isnull(sum([Quantity]) + sum([Line Amount]),0) FROM"+ Environment.GetEnvironmentVariable("Company") +"Job Ledger Entry] WHERE [Job No_] = {0} AND [Posting Date] > {1} AND [Posting Date] < {2} and SUBSTRING([Job Task No_],1,5) = b.JobTaskNo and [Type of task]=0 )	"
            //                + " ELSE "
            //                + " (select isnull(sum([Quantity]), 0) FROM"+ Environment.GetEnvironmentVariable("Company") +"Job Ledger Entry] WHERE[Job No_] = {0} AND [Posting Date] > {1} AND [Posting Date] < {2} and SUBSTRING([Job Task No_],1,5) = b.JobTaskNo and [Type of task]=0) END "
            //                + " )+"
            //                + " (CASE WHEN SUBSTRING(b.MainJobID, 1, 1) = 'N' or SUBSTRING(b.MainJobID,1,1)= 'O' THEN "
            //                + " (select isnull(sum([Quantity]) + sum([Line Amount]),0) FROM"+ Environment.GetEnvironmentVariable("Company") +"Job Ledger Entry] WHERE[Job No_] = {0} AND [Posting Date] > {1} AND [Posting Date] < {2} and SUBSTRING([Job Task No_],1,5) = b.JobTaskNo and [Type of task]=3 )	"
            //                + " ELSE "
            //                + "	(select isnull(sum([Quantity]), 0) FROM"+ Environment.GetEnvironmentVariable("Company") +"Job Ledger Entry] WHERE[Job No_] = {0} AND [Posting Date] > {1} AND [Posting Date] < {2} and SUBSTRING([Job Task No_],1,5) = b.JobTaskNo and [Type of task]=3) END "
            //                + " )+"
            //                + " (CASE WHEN SUBSTRING(b.MainJobID, 1, 1) = 'N' or SUBSTRING(b.MainJobID,1,1)= 'O' THEN "
            //                + " (select isnull(sum([Quantity]) + sum([Line Amount]),0) FROM"+ Environment.GetEnvironmentVariable("Company") +"Job Ledger Entry] WHERE[Job No_] = {0} AND [Posting Date] > {1} AND [Posting Date] < {2} and SUBSTRING([Job Task No_],1,5) = b.JobTaskNo and [Type of task]=2)	"
            //                + " ELSE "
            //                + " (select isnull(sum([Quantity]), 0) FROM"+ Environment.GetEnvironmentVariable("Company") +"Job Ledger Entry] WHERE[Job No_] = {0} AND [Posting Date] > {1} AND [Posting Date] < {2} and SUBSTRING([Job Task No_],1,5) = b.JobTaskNo and [Type of task]=2) END "
            //                + " )" +
            //                "+((select isnull(sum([Quantity]), 0) FROM "+ Environment.GetEnvironmentVariable("Company") +"Job Ledger Entry] WHERE [From Location] = {0} AND [Posting Date] > {1} AND [Posting Date] < {2} and SUBSTRING([Job Task No_],1,5) = b.JobTaskNo and [Type of task]=1 and [Document No_] Like 'CAL%' )*2)) -"
            //                + " ((CASE WHEN SUBSTRING(b.MainJobID, 1, 1) = 'N' or SUBSTRING(b.MainJobID,1,1)= 'O' THEN "
            //                + " (select isnull(sum([Quantity]) + sum([Line Amount]),0) FROM"+ Environment.GetEnvironmentVariable("Company") +"Job Ledger Entry] WHERE[Job No_] = {0} AND [Posting Date] > {1} AND [Posting Date] < {2} and SUBSTRING([Job Task No_],1,5) = b.JobTaskNo and [Type of task]=1)	"
            //                + " ELSE "
            //                + " (select isnull(sum([Quantity]), 0) FROM"+ Environment.GetEnvironmentVariable("Company") +"Job Ledger Entry] WHERE[Job No_] = {0} AND [Posting Date] > {1} AND [Posting Date] < {2} and SUBSTRING([Job Task No_],1,5) = b.JobTaskNo and [Type of task]=1) END "
            //                + " )+ "
            //                + " (select isnull(sum([Quantity]), 0) FROM "+ Environment.GetEnvironmentVariable("Company") +"Job Ledger Entry] WHERE [From Location] = {0} AND [Posting Date] > {1} AND [Posting Date] < {2} and SUBSTRING([Job Task No_],1,5) = b.JobTaskNo and [Type of task]=3 and [Job No_] <> {0} )))  AS ThisPeriodQTY "
            //                + " , "

            //                + " (((CASE WHEN SUBSTRING(b.MainJobID, 1, 1) = 'N' or SUBSTRING(b.MainJobID,1,1)= 'O' THEN "
            //                + " (select isnull(sum([Total Cost]) + sum([Line Amount]),0) FROM"+ Environment.GetEnvironmentVariable("Company") +"Job Ledger Entry] WHERE [Job No_] = {0} AND [Posting Date] > {1} AND [Posting Date] < {2} and SUBSTRING([Job Task No_],1,5) = b.JobTaskNo and [Type of task]=0 )	"
            //                + " ELSE "
            //                + " (select isnull(sum([Total Cost]), 0) FROM"+ Environment.GetEnvironmentVariable("Company") +"Job Ledger Entry] WHERE[Job No_] = {0} AND [Posting Date] > {1} AND [Posting Date] < {2} and SUBSTRING([Job Task No_],1,5) = b.JobTaskNo and [Type of task]=0) END "
            //                + " )+"
            //                + " (CASE WHEN SUBSTRING(b.MainJobID, 1, 1) = 'N' or SUBSTRING(b.MainJobID,1,1)= 'O' THEN "
            //                + " (select isnull(sum([Total Cost]) + sum([Line Amount]),0) FROM"+ Environment.GetEnvironmentVariable("Company") +"Job Ledger Entry] WHERE[Job No_] = {0} AND [Posting Date] > {1} AND [Posting Date] < {2} and SUBSTRING([Job Task No_],1,5) = b.JobTaskNo and [Type of task]=3 )	"
            //                + " ELSE "
            //                + "	(select isnull(sum([Total Cost]), 0) FROM"+ Environment.GetEnvironmentVariable("Company") +"Job Ledger Entry] WHERE[Job No_] = {0} AND [Posting Date] > {1} AND [Posting Date] < {2} and SUBSTRING([Job Task No_],1,5) = b.JobTaskNo and [Type of task]=3) END "
            //                + " )+"
            //                + " (CASE WHEN SUBSTRING(b.MainJobID, 1, 1) = 'N' or SUBSTRING(b.MainJobID,1,1)= 'O' THEN "
            //                + " (select isnull(sum([Total Cost]) + sum([Line Amount]),0) FROM"+ Environment.GetEnvironmentVariable("Company") +"Job Ledger Entry] WHERE[Job No_] = {0} AND [Posting Date] > {1} AND [Posting Date] < {2} and SUBSTRING([Job Task No_],1,5) = b.JobTaskNo and [Type of task]=2)	"
            //                + " ELSE "
            //                + " (select isnull(sum([Total Cost]), 0) FROM"+ Environment.GetEnvironmentVariable("Company") +"Job Ledger Entry] WHERE[Job No_] = {0} AND [Posting Date] > {1} AND [Posting Date] < {2} and SUBSTRING([Job Task No_],1,5) = b.JobTaskNo and [Type of task]=2) END "
            //                + " )" 
            //                + "+((select isnull(sum([Total Cost]), 0) FROM "+ Environment.GetEnvironmentVariable("Company") +"Job Ledger Entry] WHERE [From Location] = {0} AND [Posting Date] > {1} AND [Posting Date] < {2} and SUBSTRING([Job Task No_],1,5) = b.JobTaskNo and [Type of task]=1 and [Document No_] Like 'CAL%' )*2)) -"
            //                + " ((CASE WHEN SUBSTRING(b.MainJobID, 1, 1) = 'N' or SUBSTRING(b.MainJobID,1,1)= 'O' THEN "
            //                + " (select isnull(sum([Total Cost]) + sum([Line Amount]),0) FROM"+ Environment.GetEnvironmentVariable("Company") +"Job Ledger Entry] WHERE[Job No_] = {0} AND [Posting Date] > {1} AND [Posting Date] < {2} and SUBSTRING([Job Task No_],1,5) = b.JobTaskNo and [Type of task]=1)	"
            //                + " ELSE "
            //                + " (select isnull(sum([Total Cost]), 0) FROM"+ Environment.GetEnvironmentVariable("Company") +"Job Ledger Entry] WHERE[Job No_] = {0} AND [Posting Date] > {1} AND [Posting Date] < {2} and SUBSTRING([Job Task No_],1,5) = b.JobTaskNo and [Type of task]=1) END "
            //                + " )+ "
            //                + " (select isnull(sum([Total Cost]), 0) FROM "+ Environment.GetEnvironmentVariable("Company") +"Job Ledger Entry] WHERE [From Location] = {0} AND [Posting Date] > {1} AND [Posting Date] < {2} and SUBSTRING([Job Task No_],1,5) = b.JobTaskNo and [Type of task]=3 and [Job No_] <> {0} )))  AS ThisPeriodAMT "
            //                + " , "

            //                + " (((CASE WHEN SUBSTRING(b.MainJobID, 1, 1) = 'N' or SUBSTRING(b.MainJobID,1,1)= 'O' THEN "
            //                + " (select isnull(sum([Quantity]) + sum([Line Amount]),0) FROM"+ Environment.GetEnvironmentVariable("Company") +"Job Ledger Entry] WHERE [Job No_] = {0} AND [Posting Date] < {1} and SUBSTRING([Job Task No_],1,5) = b.JobTaskNo and [Type of task]=0 )	"
            //                + " ELSE "
            //                + " (select isnull(sum([Quantity]), 0) FROM"+ Environment.GetEnvironmentVariable("Company") +"Job Ledger Entry] WHERE[Job No_] = {0} AND [Posting Date] < {1} and SUBSTRING([Job Task No_],1,5) = b.JobTaskNo and [Type of task]=0) END "
            //                + " )+"
            //                + " (CASE WHEN SUBSTRING(b.MainJobID, 1, 1) = 'N' or SUBSTRING(b.MainJobID,1,1)= 'O' THEN "
            //                + " (select isnull(sum([Quantity]) + sum([Line Amount]),0) FROM"+ Environment.GetEnvironmentVariable("Company") +"Job Ledger Entry] WHERE[Job No_] = {0} AND [Posting Date] < {1} and SUBSTRING([Job Task No_],1,5) = b.JobTaskNo and [Type of task]=3 )	"
            //                + " ELSE "
            //                + "	(select isnull(sum([Quantity]), 0) FROM"+ Environment.GetEnvironmentVariable("Company") +"Job Ledger Entry] WHERE[Job No_] = {0} AND[Posting Date] < {1} and SUBSTRING([Job Task No_],1,5) = b.JobTaskNo and [Type of task]=3) END "
            //                + " )+"
            //                + " (CASE WHEN SUBSTRING(b.MainJobID, 1, 1) = 'N' or SUBSTRING(b.MainJobID,1,1)= 'O' THEN "
            //                + " (select isnull(sum([Quantity]) + sum([Line Amount]),0) FROM"+ Environment.GetEnvironmentVariable("Company") +"Job Ledger Entry] WHERE[Job No_] = {0} AND [Posting Date] < {1} and SUBSTRING([Job Task No_],1,5) = b.JobTaskNo and [Type of task]=2)	"
            //                + " ELSE "
            //                + " (select isnull(sum([Quantity]), 0) FROM"+ Environment.GetEnvironmentVariable("Company") +"Job Ledger Entry] WHERE[Job No_] = {0} AND [Posting Date] < {1} and SUBSTRING([Job Task No_],1,5) = b.JobTaskNo and [Type of task]=2) END "
            //                + " )"
            //                + "+((select isnull(sum([Quantity]), 0) FROM "+ Environment.GetEnvironmentVariable("Company") +"Job Ledger Entry] WHERE [From Location] = {0} AND [Posting Date] < {1}  and SUBSTRING([Job Task No_],1,5) = b.JobTaskNo and [Type of task]=1 and [Document No_] Like 'CAL%' )*2)) -"
            //                + " ((CASE WHEN SUBSTRING(b.MainJobID, 1, 1) = 'N' or SUBSTRING(b.MainJobID,1,1)= 'O' THEN "
            //                + " (select isnull(sum([Quantity]) + sum([Line Amount]),0) FROM"+ Environment.GetEnvironmentVariable("Company") +"Job Ledger Entry] WHERE[Job No_] = {0} AND [Posting Date] < {1} and SUBSTRING([Job Task No_],1,5) = b.JobTaskNo and [Type of task]=1)	"
            //                + " ELSE "
            //                + " (select isnull(sum([Quantity]), 0) FROM"+ Environment.GetEnvironmentVariable("Company") +"Job Ledger Entry] WHERE[Job No_] = {0} AND [Posting Date] < {1} and SUBSTRING([Job Task No_],1,5) = b.JobTaskNo and [Type of task]=1) END "
            //                + " )+ "
            //                + " (select isnull(sum([Quantity]), 0) FROM "+ Environment.GetEnvironmentVariable("Company") +"Job Ledger Entry] WHERE [From Location] = {0} AND [Posting Date] < {1} and SUBSTRING([Job Task No_],1,5) = b.JobTaskNo and [Type of task]=3 and [Job No_] <> {0} )))  AS OpeningQTY "
            //                + " , "

            //                + " (((CASE WHEN SUBSTRING(b.MainJobID, 1, 1) = 'N' or SUBSTRING(b.MainJobID,1,1)= 'O' THEN "
            //                + " (select isnull(sum([Total Cost]) + sum([Line Amount]),0) FROM"+ Environment.GetEnvironmentVariable("Company") +"Job Ledger Entry] WHERE [Job No_] = {0} AND  [Posting Date] < {1} and SUBSTRING([Job Task No_],1,5) = b.JobTaskNo and [Type of task]=0 )	"
            //                + " ELSE "
            //                + " (select isnull(sum([Total Cost]), 0) FROM"+ Environment.GetEnvironmentVariable("Company") +"Job Ledger Entry] WHERE[Job No_] = {0} AND [Posting Date] < {1} and SUBSTRING([Job Task No_],1,5) = b.JobTaskNo and [Type of task]=0) END "
            //                + " )+"
            //                + " (CASE WHEN SUBSTRING(b.MainJobID, 1, 1) = 'N' or SUBSTRING(b.MainJobID,1,1)= 'O' THEN "
            //                + " (select isnull(sum([Total Cost]) + sum([Line Amount]),0) FROM"+ Environment.GetEnvironmentVariable("Company") +"Job Ledger Entry] WHERE[Job No_] = {0} AND [Posting Date] < {1} and SUBSTRING([Job Task No_],1,5) = b.JobTaskNo and [Type of task]=3 )	"
            //                + " ELSE "
            //                + "	(select isnull(sum([Total Cost]), 0) FROM"+ Environment.GetEnvironmentVariable("Company") +"Job Ledger Entry] WHERE[Job No_] = {0} AND [Posting Date] < {1} and SUBSTRING([Job Task No_],1,5) = b.JobTaskNo and [Type of task]=3) END "
            //                + " )+"
            //                + " (CASE WHEN SUBSTRING(b.MainJobID, 1, 1) = 'N' or SUBSTRING(b.MainJobID,1,1)= 'O' THEN "
            //                + " (select isnull(sum([Total Cost]) + sum([Line Amount]),0) FROM"+ Environment.GetEnvironmentVariable("Company") +"Job Ledger Entry] WHERE[Job No_] = {0} AND [Posting Date] < {1} and SUBSTRING([Job Task No_],1,5) = b.JobTaskNo and [Type of task]=2)	"
            //                + " ELSE "
            //                + " (select isnull(sum([Total Cost]), 0) FROM"+ Environment.GetEnvironmentVariable("Company") +"Job Ledger Entry] WHERE[Job No_] = {0} AND [Posting Date] < {1} and SUBSTRING([Job Task No_],1,5) = b.JobTaskNo and [Type of task]=2) END "
            //                + " )"
            //                + " +((select isnull(sum([Total Cost]), 0) FROM "+ Environment.GetEnvironmentVariable("Company") +"Job Ledger Entry] WHERE [From Location] = {0} AND [Posting Date] < {1}  and SUBSTRING([Job Task No_],1,5) = b.JobTaskNo and [Type of task]=1 and [Document No_] Like 'CAL%' )*2)) -"
            //                + " ((CASE WHEN SUBSTRING(b.MainJobID, 1, 1) = 'N' or SUBSTRING(b.MainJobID,1,1)= 'O' THEN "
            //                + " (select isnull(sum([Total Cost]) + sum([Line Amount]),0) FROM"+ Environment.GetEnvironmentVariable("Company") +"Job Ledger Entry] WHERE[Job No_] = {0} AND [Posting Date] < {1} and SUBSTRING([Job Task No_],1,5) = b.JobTaskNo and [Type of task]=1)	"
            //                + " ELSE "
            //                + " (select isnull(sum([Total Cost]), 0) FROM"+ Environment.GetEnvironmentVariable("Company") +"Job Ledger Entry] WHERE[Job No_] = {0} AND [Posting Date] < {1} and SUBSTRING([Job Task No_],1,5) = b.JobTaskNo and [Type of task]=1) END "
            //                + " )+ "
            //                + " (select isnull(sum([Total Cost]), 0) FROM "+ Environment.GetEnvironmentVariable("Company") +"Job Ledger Entry] WHERE [From Location] = {0} AND [Posting Date] < {1} and SUBSTRING([Job Task No_],1,5) = b.JobTaskNo and [Type of task]=3 and [Job No_] <> {0} )))  AS OpeningAMT "
            //                + " , "

            //                + " (((CASE WHEN SUBSTRING(b.MainJobID, 1, 1) = 'N' or SUBSTRING(b.MainJobID,1,1)= 'O' THEN "
            //                + " (select isnull(sum([Quantity]) + sum([Line Amount]),0) FROM"+ Environment.GetEnvironmentVariable("Company") +"Job Ledger Entry] WHERE [Job No_] = {0} AND [Posting Date] < {2} and SUBSTRING([Job Task No_],1,5) = b.JobTaskNo and [Type of task]=0 )	"
            //                + " ELSE "
            //                + " (select isnull(sum([Quantity]), 0) FROM"+ Environment.GetEnvironmentVariable("Company") +"Job Ledger Entry] WHERE[Job No_] = {0} AND [Posting Date] < {2} and SUBSTRING([Job Task No_],1,5) = b.JobTaskNo and [Type of task]=0) END "
            //                + " )+"
            //                + " (CASE WHEN SUBSTRING(b.MainJobID, 1, 1) = 'N' or SUBSTRING(b.MainJobID,1,1)= 'O' THEN "
            //                + " (select isnull(sum([Quantity]) + sum([Line Amount]),0) FROM"+ Environment.GetEnvironmentVariable("Company") +"Job Ledger Entry] WHERE[Job No_] = {0} AND [Posting Date] < {2} and SUBSTRING([Job Task No_],1,5) = b.JobTaskNo and [Type of task]=3 )	"
            //                + " ELSE "
            //                + "	(select isnull(sum([Quantity]), 0) FROM"+ Environment.GetEnvironmentVariable("Company") +"Job Ledger Entry] WHERE[Job No_] = {0} AND[Posting Date] < {2} and SUBSTRING([Job Task No_],1,5) = b.JobTaskNo and [Type of task]=3) END "
            //                + " )+"
            //                + " (CASE WHEN SUBSTRING(b.MainJobID, 1, 1) = 'N' or SUBSTRING(b.MainJobID,1,1)= 'O' THEN "
            //                + " (select isnull(sum([Quantity]) + sum([Line Amount]),0) FROM"+ Environment.GetEnvironmentVariable("Company") +"Job Ledger Entry] WHERE[Job No_] = {0} AND [Posting Date] < {2} and SUBSTRING([Job Task No_],1,5) = b.JobTaskNo and [Type of task]=2)	"
            //                + " ELSE "
            //                + " (select isnull(sum([Quantity]), 0) FROM"+ Environment.GetEnvironmentVariable("Company") +"Job Ledger Entry] WHERE[Job No_] = {0} AND [Posting Date] < {2} and SUBSTRING([Job Task No_],1,5) = b.JobTaskNo and [Type of task]=2) END "
            //                + " )) -"
            //                + " ((CASE WHEN SUBSTRING(b.MainJobID, 1, 1) = 'N' or SUBSTRING(b.MainJobID,1,1)= 'O' THEN "
            //                + " (select isnull(sum([Quantity]) + sum([Line Amount]),0) FROM"+ Environment.GetEnvironmentVariable("Company") +"Job Ledger Entry] WHERE[Job No_] = {0} AND [Posting Date] < {2} and SUBSTRING([Job Task No_],1,5) = b.JobTaskNo and [Type of task]=1)	"
            //                + " ELSE "
            //                + " (select isnull(sum([Quantity]), 0) FROM"+ Environment.GetEnvironmentVariable("Company") +"Job Ledger Entry] WHERE[Job No_] = {0} AND [Posting Date] < {2} and SUBSTRING([Job Task No_],1,5) = b.JobTaskNo and [Type of task]=1) END "
            //                + " )+ "
            //                + " (select isnull(sum([Quantity]), 0) FROM "+ Environment.GetEnvironmentVariable("Company") +"Job Ledger Entry] WHERE [From Location] = {0} AND [Posting Date] < {2} and SUBSTRING([Job Task No_],1,5) = b.JobTaskNo and [Type of task]=3 and [Job No_] <> {0} )))  AS CumBalQTY "
            //                + " , "

            //                  + " (((CASE WHEN SUBSTRING(b.MainJobID, 1, 1) = 'N' or SUBSTRING(b.MainJobID,1,1)= 'O' THEN "
            //                  + " (select isnull(sum([Total Cost]) + sum([Line Amount]),0) FROM"+ Environment.GetEnvironmentVariable("Company") +"Job Ledger Entry] WHERE [Job No_] = {0} AND  [Posting Date] < {2} and SUBSTRING([Job Task No_],1,5) = b.JobTaskNo and [Type of task]=0 )	"
            //                  + " ELSE "
            //                  + " (select isnull(sum([Total Cost]), 0) FROM"+ Environment.GetEnvironmentVariable("Company") +"Job Ledger Entry] WHERE[Job No_] = {0} AND [Posting Date] < {2} and SUBSTRING([Job Task No_],1,5) = b.JobTaskNo and [Type of task]=0) END "
            //                  + " )+"
            //                  + " (CASE WHEN SUBSTRING(b.MainJobID, 1, 1) = 'N' or SUBSTRING(b.MainJobID,1,1)= 'O' THEN "
            //                  + " (select isnull(sum([Total Cost]) + sum([Line Amount]),0) FROM"+ Environment.GetEnvironmentVariable("Company") +"Job Ledger Entry] WHERE[Job No_] = {0} AND [Posting Date] < {2} and SUBSTRING([Job Task No_],1,5) = b.JobTaskNo and [Type of task]=3 )	"
            //                  + " ELSE "
            //                  + "	(select isnull(sum([Total Cost]), 0) FROM"+ Environment.GetEnvironmentVariable("Company") +"Job Ledger Entry] WHERE[Job No_] = {0} AND [Posting Date] < {2} and SUBSTRING([Job Task No_],1,5) = b.JobTaskNo and [Type of task]=3) END "
            //                  + " )+"
            //                  + " (CASE WHEN SUBSTRING(b.MainJobID, 1, 1) = 'N' or SUBSTRING(b.MainJobID,1,1)= 'O' THEN "
            //                  + " (select isnull(sum([Total Cost]) + sum([Line Amount]),0) FROM"+ Environment.GetEnvironmentVariable("Company") +"Job Ledger Entry] WHERE[Job No_] = {0} AND [Posting Date] < {2} and SUBSTRING([Job Task No_],1,5) = b.JobTaskNo and [Type of task]=2)	"
            //                  + " ELSE "
            //                  + " (select isnull(sum([Total Cost]), 0) FROM"+ Environment.GetEnvironmentVariable("Company") +"Job Ledger Entry] WHERE[Job No_] = {0} AND [Posting Date] < {2} and SUBSTRING([Job Task No_],1,5) = b.JobTaskNo and [Type of task]=2) END "
            //                  + " )"
            //                  + " +((select isnull(sum([Total Cost]), 0) FROM "+ Environment.GetEnvironmentVariable("Company") +"Job Ledger Entry] WHERE [From Location] = {0} AND [Posting Date] < {2}  and SUBSTRING([Job Task No_],1,5) = b.JobTaskNo and [Type of task]=1 and [Document No_] Like 'CAL%' )*2)) -"
            //                  + " ((CASE WHEN SUBSTRING(b.MainJobID, 1, 1) = 'N' or SUBSTRING(b.MainJobID,1,1)= 'O' THEN "
            //                  + " (select isnull(sum([Total Cost]) + sum([Line Amount]),0) FROM"+ Environment.GetEnvironmentVariable("Company") +"Job Ledger Entry] WHERE[Job No_] = {0} AND [Posting Date] < {2} and SUBSTRING([Job Task No_],1,5) = b.JobTaskNo and [Type of task]=1)	"
            //                  + " ELSE "
            //                  + " (select isnull(sum([Total Cost]), 0) FROM"+ Environment.GetEnvironmentVariable("Company") +"Job Ledger Entry] WHERE[Job No_] = {0} AND [Posting Date] < {2} and SUBSTRING([Job Task No_],1,5) = b.JobTaskNo and [Type of task]=1) END "
            //                  + " )+ "
            //                  + " (select isnull(sum([Total Cost]), 0) FROM "+ Environment.GetEnvironmentVariable("Company") +"Job Ledger Entry] WHERE [From Location] = {0} AND [Posting Date] < {2} and SUBSTRING([Job Task No_],1,5) = b.JobTaskNo and [Type of task]=3 and [Job No_] <> {0} )))  AS CumBalAMT "

            //                //  ///กรณีนำ GL
            //                //  + " (SELECT isnull(SUM([Amount]),0) as Total FROM dbo."+ Environment.GetEnvironmentVariable("Company") +"Analysis View Entry] WHERE [Analysis View Code]='JOB COST A' and Convert(Time,[Posting Date])<>'23:59:59'  and [Posting Date] <={2} and [Dimension 1 Value Code]={0} and [Dimension 2 Value Code]<>'' and [Dimension 2 Value Code]=b.JobTaskNo) as CumBalAMT"
            //                /////


            //                + " , "

            //                + " (SELECT isnull(sum(Quantity), 0) FROM dbo."+ Environment.GetEnvironmentVariable("Company") +"Job Planning Line] WHERE[Job No_]={0} and SUBSTRING([Job Task No_],1,5)=b.JobTaskNo) as BudgetQTY, "
            //                + " (SELECT isnull(sum([Total Cost (LCY)]),0) FROM dbo."+ Environment.GetEnvironmentVariable("Company") +"Job Planning Line] WHERE[Job No_]= {0} and SUBSTRING([Job Task No_],1,5)= b.JobTaskNo) as BudgetAMT "

            //                + " FROM("
            //                 + " SELECT a.JobTaskNo,a.MainJobID,a.SubJobID,"
            //                 + " (SELECT TOP 1 dbo."+ Environment.GetEnvironmentVariable("Company") +"Job Task].Description FROM dbo."+ Environment.GetEnvironmentVariable("Company") +"Job Task] WHERE [Job Task No_]=a.JobTaskNo) as Description,"
            //                 + " (SELECT TOP 1 ([Job Task No_]+' '+Description) FROM dbo."+ Environment.GetEnvironmentVariable("Company") +"Job Task] WHERE [Job Task No_]=a.JobTaskNo) as DescriptionFull,"
            //                 + " (SELECT TOP 1 dbo."+ Environment.GetEnvironmentVariable("Company") +"Job Task].Description FROM dbo."+ Environment.GetEnvironmentVariable("Company") +"Job Task] WHERE SUBSTRING(dbo."+ Environment.GetEnvironmentVariable("Company") +"Job Task].[Job Task No_],1,1)+'0000'=a.MainJobID) as DescriptionMainJob,"
            //                 + " (SELECT TOP 1 dbo."+ Environment.GetEnvironmentVariable("Company") +"Job Task].Description FROM dbo."+ Environment.GetEnvironmentVariable("Company") +"Job Task] WHERE SUBSTRING(dbo."+ Environment.GetEnvironmentVariable("Company") +"Job Task].[Job Task No_],1,3)+'00'=a.SubJobID) as DescriptionSubJob"
            //                 + " FROM  (SELECT DISTINCT dbo."+ Environment.GetEnvironmentVariable("Company") +"Job Task].[Job Task No_] as JobTaskNo,"
            //                 + " SUBSTRING(dbo."+ Environment.GetEnvironmentVariable("Company") +"Job Task].[Job Task No_],1,1)+'0000' as MainJobID,SUBSTRING(dbo."+ Environment.GetEnvironmentVariable("Company") +"Job Task].[Job Task No_],1,3)+'00' as SubJobID FROM dbo."+ Environment.GetEnvironmentVariable("Company") +"Job Task]) as a) as b ORDER BY b.JobTaskNo";


            //SqlParameter parameterJob = new SqlParameter("{0}", JobNo);
            //SqlParameter parameterStart = new SqlParameter("{1}", date1);
            //SqlParameter parameterDate1 = new SqlParameter("{2}", date2);



 
            ViewBag.sql = queryData;
            var v_ReportJobCost = _navcontext.V_ReportJobCost.FromSqlRaw(queryData, JobNo, date1, date2).ToList();
            ViewData["v_ReportJobCost"] = v_ReportJobCost;
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




            List<TableJobCost> TableJobCosts = new List<TableJobCost>
            {

            };

            headtable = "<table  id='JobcostDetail' ><thead><tr>"
                + "<th align ='center'>JobTask</th>"
                + "<th align ='center'>ThisPeriodQTY</th>"
                + "<th align ='center'>ThisPeriodAMT</th>"
                + "<th align ='center'>OpenningQTY</th>"
                + "<th align ='center'>OpenningAMT</th>"
                + "<th align ='center'>CumBalQTY</th>"
                + "<th align ='center'>CumBalAMT</th>"
                + "<th align ='center'>BudgetQty</th>"
                + "<th align ='center'>%</th>"
                + "<th align ='center'>BudgetAMT</th>"
                + "<th align ='center'>%</th>"
                + "</tr>"
                + "</thead>"
                + "<tbody id='myTable'>";

            Sumhead = "<table  id='SumDetail' ><thead><tr>"
                + "<th align ='center'>JobTask</th>"
                + "<th align ='center'>ThisPeriodQTY</th>"
                + "<th align ='center'>ThisPeriodAMT</th>"
                + "<th align ='center'>OpenningQTY</th>"
                + "<th align ='center'>OpenningAMT</th>"
                + "<th align ='center'>CumBalQTY</th>"
                + "<th align ='center'>CumBalAMT</th>"
                + "<th align ='center'>BudgetQty</th>"
                + "<th align ='center'>%</th>"
                + "<th align ='center'>BudgetAMT</th>"
                + "<th align ='center'>%</th>"
                + "</tr>"
                + "</thead>"
                + "<tbody id='sumTable'>";





            foreach (var std in v_ReportJobCost as IList<V_ReportJobCost>)
            {



                //Main
                if (mainID != std.MainJobID)
                {
                    mainID = std.MainJobID;
                    check = 0;
                }
                else
                {
                    check = 1;
                }


                if (check == 0)
                {





                    sumThisPeriodQTY = v_ReportJobCost.Where(c => c.MainJobID == std.MainJobID).Sum(c => c.ThisPeriodQTY);
                    sumThisPeriodAMT = v_ReportJobCost.Where(c => c.MainJobID == std.MainJobID).Sum(c => c.ThisperiodAMT);
                    sumOpeningQTY = v_ReportJobCost.Where(c => c.MainJobID == std.MainJobID).Sum(c => c.OpeningQTY);
                    sumOpeningAMT = v_ReportJobCost.Where(c => c.MainJobID == std.MainJobID).Sum(c => c.OpeningAMT);
                    sumCumBalQTY = v_ReportJobCost.Where(c => c.MainJobID == std.MainJobID).Sum(c => c.CumBalQTY);
                    sumCumBalAMT = v_ReportJobCost.Where(c => c.MainJobID == std.MainJobID).Sum(c => c.CumBalAMT);
                    sumBudgetQTY = v_ReportJobCost.Where(c => c.MainJobID == std.MainJobID).Sum(c => c.BudgetQTY);
                    sumBudgetAMT = v_ReportJobCost.Where(c => c.MainJobID == std.MainJobID).Sum(c => c.BudgetAMT);


                    if (std.MainJobID == "A0000")
                    {
                        A00 = sumCumBalAMT;
                        ViewBag.A00 = SetFontRed(sumCumBalAMT, 1);
                        TCXI += sumCumBalAMT;

                    }
                    if (std.MainJobID == "B0000")
                    {
                        B00 = sumCumBalAMT;
                        ViewBag.B00 = SetFontRed(sumCumBalAMT, 1);
                        TCXI += sumCumBalAMT;
                    }
                    if (std.MainJobID == "C0000")
                    {
                        C00 = sumCumBalAMT;
                        ViewBag.C00 = SetFontRed(sumCumBalAMT, 1);
                        TCXI += sumCumBalAMT;
                    }
                    if (std.MainJobID == "D0000")
                    {
                        D00 = sumCumBalAMT;
                        ViewBag.D00 = SetFontRed(sumCumBalAMT, 1);
                        TCXI += sumCumBalAMT;
                    }
                    if (std.MainJobID == "E0000")
                    {
                        E00 = sumCumBalAMT;
                        ViewBag.E00 = SetFontRed(sumCumBalAMT, 1);
                        TCXI += sumCumBalAMT;
                    }
                    if (std.MainJobID == "F0000")
                    {
                        F00 = sumCumBalAMT;
                        ViewBag.F00 = SetFontRed(sumCumBalAMT, 1);
                        TCXI += sumCumBalAMT;
                    }

                    if (std.MainJobID == "G0000")
                    {
                        G00 = sumCumBalAMT;
                        ViewBag.G00 = SetFontRed(sumCumBalAMT, 1);
                        TCXI += sumCumBalAMT;
                    }
                    if (std.MainJobID == "H0000")
                    {
                        H00 = sumCumBalAMT;
                        ViewBag.H00 = SetFontRed(sumCumBalAMT, 1);
                        TCXI += sumCumBalAMT;
                    }


                    //summain += "<tr>"
                    //         + "<td><b>" + std.MainJobID + " " + std.DescriptionMainJob + "</b></td>"
                    //         + "<td align ='right'>" + SetFontRed(sumThisPeriodQTY, 1) + "</td>"
                    //         + "<td align ='right'>" + SetFontRed(sumThisPeriodAMT, 1) + "</td>"
                    //         + "<td align ='right'>" + SetFontRed(sumOpeningQTY, 1) + "</td>"
                    //         + "<td align ='right'>" + SetFontRed(sumOpeningAMT, 1) + "</td>"
                    //         + "<td align ='right'>" + SetFontRed(sumCumBalQTY, 1) + "</td>"
                    //         + "<td align ='right'>" + SetFontRed(sumCumBalAMT, 1) + "</td>"
                    //         + "<td align ='right'>" + SetFontRed(sumBudgetQTY, 1) + "</td >"
                    //         + "<td align ='right'>" + SetFontRed(Percen(sumCumBalQTY, sumBudgetQTY), 1) + "<b>%</b>" + "</td>"
                    //         + "<td align ='right'>" + SetFontRed(sumBudgetAMT, 1) + "</td>"
                    //         + "<td align ='right'>" + SetFontRed(Percen(sumCumBalAMT, sumBudgetAMT), 1) + "<b>%</b>" + "</td>"
                    //         + "</tr>";

                    if (std.MainJobID == "O0000")
                    {
                        summainIncome += "<tr>"
                          + "<td><b>" + std.MainJobID + " " + std.DescriptionMainJob + "</b></td>"
                          + "<td align ='right'>" + SetFontRed(sumThisPeriodQTY, 1) + "</td>"
                          + "<td align ='right'>" + SetFontRed(sumThisPeriodAMT, 1) + "</td>"
                          + "<td align ='right'>" + SetFontRed(sumOpeningQTY, 1) + "</td>"
                          + "<td align ='right'>" + SetFontRed(sumOpeningAMT, 1) + "</td>"
                          + "<td align ='right'>" + SetFontRed(sumCumBalQTY, 1) + "</td>"
                          + "<td align ='right'>" + SetFontRed(sumCumBalAMT, 1) + "</td>"
                          + "<td align ='right'>" + SetFontRed(sumBudgetQTY, 1) + "</td >"
                          + "<td align ='right'>" + SetFontRed(Percen(sumCumBalQTY, sumBudgetQTY), 1) + "<b>%</b>" + "</td>"
                          + "<td align ='right'>" + SetFontRed(sumBudgetAMT, 1) + "</td>"
                          + "<td align ='right'>" + SetFontRed(Percen(sumCumBalAMT, sumBudgetAMT), 1) + "<b>%</b>" + "</td>"
                          + "</tr>";


                        OthisQ += sumThisPeriodQTY;
                        OthisA += sumThisPeriodAMT;
                        OopenQ += sumOpeningQTY;
                        OopenA += sumOpeningAMT;
                        OcumQ += sumCumBalQTY;
                        OcumA += sumCumBalAMT;
                        ObudQ += sumBudgetQTY;
                        ObudA += sumBudgetAMT;

                    }
                    else if (std.MainJobID == "N0000")
                    {

                        summainIncome += "<tr>"
                          + "<td><b>" + std.MainJobID + " " + std.DescriptionMainJob + "</b></td>"
                          + "<td align ='right'>" + SetFontRed(sumThisPeriodQTY, 1) + "</td>"
                          + "<td align ='right'>" + SetFontRed(sumThisPeriodAMT, 1) + "</td>"
                          + "<td align ='right'>" + SetFontRed(sumOpeningQTY, 1) + "</td>"
                          + "<td align ='right'>" + SetFontRed(sumOpeningAMT, 1) + "</td>"
                          + "<td align ='right'>" + SetFontRed(sumCumBalQTY, 1) + "</td>"
                          + "<td align ='right'>" + SetFontRed(sumCumBalAMT, 1) + "</td>"
                          + "<td align ='right'>" + SetFontRed(sumBudgetQTY, 1) + "</td >"
                          + "<td align ='right'>" + SetFontRed(Percen(sumCumBalQTY, sumBudgetQTY), 1) + "<b>%</b>" + "</td>"
                          + "<td align ='right'>" + SetFontRed(sumBudgetAMT, 1) + "</td>"
                          + "<td align ='right'>" + SetFontRed(Percen(sumCumBalAMT, sumBudgetAMT), 1) + "<b>%</b>" + "</td>"
                          + "</tr>";
                        OthisQ += sumThisPeriodQTY;
                        OthisA += sumThisPeriodAMT;
                        OopenQ += sumOpeningQTY;
                        OopenA += sumOpeningAMT;
                        OcumQ += sumCumBalQTY;
                        OcumA += sumCumBalAMT;
                        ObudQ += sumBudgetQTY;
                        ObudA += sumBudgetAMT;

                    }
                    else
                    {
                        summain += "<tr>"
                          + "<td><b>" + std.MainJobID + " " + std.DescriptionMainJob + "</b></td>"
                          + "<td align ='right'>" + SetFontRed(sumThisPeriodQTY, 1) + "</td>"
                          + "<td align ='right'>" + SetFontRed(sumThisPeriodAMT, 1) + "</td>"
                          + "<td align ='right'>" + SetFontRed(sumOpeningQTY, 1) + "</td>"
                          + "<td align ='right'>" + SetFontRed(sumOpeningAMT, 1) + "</td>"
                          + "<td align ='right'>" + SetFontRed(sumCumBalQTY, 1) + "</td>"
                          + "<td align ='right'>" + SetFontRed(sumCumBalAMT, 1) + "</td>"
                          + "<td align ='right'>" + SetFontRed(sumBudgetQTY, 1) + "</td >"
                          + "<td align ='right'>" + SetFontRed(Percen(sumCumBalQTY, sumBudgetQTY), 1) + "<b>%</b>" + "</td>"
                          + "<td align ='right'>" + SetFontRed(sumBudgetAMT, 1) + "</td>"
                          + "<td align ='right'>" + SetFontRed(Percen(sumCumBalAMT, sumBudgetAMT), 1) + "<b>%</b>" + "</td>"
                          + "</tr>";



                        thisQ += sumThisPeriodQTY;
                        thisA += sumThisPeriodAMT;
                        openQ += sumOpeningQTY;
                        openA += sumOpeningAMT;
                        cumQ += sumCumBalQTY;
                        cumA += sumCumBalAMT;
                        budQ += sumBudgetQTY;
                        budA += sumBudgetAMT;





                    }
                    //else
                    //{
                    //    OthisQ += sumThisPeriodQTY;
                    //    OthisA += sumThisPeriodAMT;
                    //    OopenQ += sumOpeningQTY;
                    //    OopenA += sumOpeningAMT;
                    //    OcumQ += sumCumBalQTY;
                    //    OcumA += sumCumBalAMT;
                    //    ObudQ += sumBudgetQTY;
                    //    ObudA += sumBudgetAMT;
                    //}


                    main = "<tr>"
                             + "<td><b>" + std.MainJobID + " " + std.DescriptionMainJob + "</b></td>"
                             + "<td align ='right'>" + SetFontRed(sumThisPeriodQTY, 1) + "</td>"
                             + "<td align ='right'>" + SetFontRed(sumThisPeriodAMT, 1) + "</td>"
                             + "<td align ='right'>" + SetFontRed(sumOpeningQTY, 1) + "</td>"
                             + "<td align ='right'>" + SetFontRed(sumOpeningAMT, 1) + "</td>"
                             + "<td align ='right'>" + SetFontRed(sumCumBalQTY, 1) + "</td>"
                             + "<td align ='right'>" + SetFontRed(sumCumBalAMT, 1) + "</td>"
                             + "<td align ='right'>" + SetFontRed(sumBudgetQTY, 1) + "</td >"
                             + "<td align ='right'>" + SetFontRed(Percen(sumCumBalQTY, sumBudgetQTY), 1) + "<b>%</b>" + "</td>"
                             + "<td align ='right'>" + SetFontRed(sumBudgetAMT, 1) + "</td>"
                             + "<td align ='right'>" + SetFontRed(Percen(sumCumBalAMT, sumBudgetAMT), 1) + "<b>%</b>" + "</td>"
                             + "</tr>";

                    TableJobCosts.Add(new TableJobCost { JobTask = std.MainJobID + " " + std.DescriptionMainJob,
                        ThisPeriodQTY = sumThisPeriodQTY,
                        ThisPeriodAMT = sumThisPeriodAMT,
                        OpenningQTY=sumOpeningQTY,
                        OpenningAMT=sumSubOpeningAMT,
                        CumBalQTY=sumCumBalQTY,
                        CumBalAMT=sumCumBalAMT,
                        BudgetQty=sumBudgetQTY,
                        PerQty= Percen(sumCumBalQTY, sumBudgetQTY),
                        BudgetAMT=sumBudgetAMT,
                        PerAMT= Percen(sumCumBalAMT, sumBudgetAMT)
                    });

                    check = 1;
                }
                else
                {
                    main = "";
                }

                //-------End main



                //SUB
                if (subID != std.SubJobID)
                {
                    subID = std.SubJobID;
                    check1 = 0;
                }
                else
                {
                    check1 = 1;
                }


                if (check1 == 0)
                {

                    if (std.MainJobID == std.SubJobID)
                    {
                        sub = "";
                    }
                    else
                    {



                        sumSubThisPeriodQTY = v_ReportJobCost.Where(c => c.SubJobID == std.SubJobID).Sum(c => c.ThisPeriodQTY);
                        sumSubThisPeriodAMT = v_ReportJobCost.Where(c => c.SubJobID == std.SubJobID).Sum(c => c.ThisperiodAMT);
                        sumSubOpeningQTY = v_ReportJobCost.Where(c => c.SubJobID == std.SubJobID).Sum(c => c.OpeningQTY);
                        sumSubOpeningAMT = v_ReportJobCost.Where(c => c.SubJobID == std.SubJobID).Sum(c => c.OpeningAMT);
                        sumSubCumBalQTY = v_ReportJobCost.Where(c => c.SubJobID == std.SubJobID).Sum(c => c.CumBalQTY);
                        sumSubCumBalAMT = v_ReportJobCost.Where(c => c.SubJobID == std.SubJobID).Sum(c => c.CumBalAMT);
                        sumSubBudgetQTY = v_ReportJobCost.Where(c => c.SubJobID == std.SubJobID).Sum(c => c.BudgetQTY);
                        sumSubBudgetAMT = v_ReportJobCost.Where(c => c.SubJobID == std.SubJobID).Sum(c => c.BudgetAMT);

                        if (std.JobTaskNo == "D2100")
                        {
                            D2100 = std.CumBalAMT;
                            ViewBag.D2100 = SetFontRed(sumSubCumBalAMT, 1);
                        }

                        if (std.JobTaskNo == "E0800")
                        {
                            E0800 = std.CumBalAMT;
                            ViewBag.E0800 = SetFontRed(sumSubCumBalAMT, 1);
                        }
                        if (std.JobTaskNo == "H0400")
                        {
                            H0400 = std.CumBalAMT;
                            ViewBag.H0400 = SetFontRed(sumSubCumBalAMT, 1);
                        }
                        if (std.JobTaskNo == "H0200")
                        {
                            H0200 = std.CumBalAMT;
                            ViewBag.H0200 = SetFontRed(sumSubCumBalAMT, 1);
                        }
                        if (std.JobTaskNo == "H0800")
                        {
                            H0800 = std.CumBalAMT;
                            ViewBag.H0800 = SetFontRed(sumSubCumBalAMT, 1);
                        }
                        if (std.JobTaskNo == "H0600")
                        {
                            H0600 = std.CumBalAMT;
                            ViewBag.H0600 = SetFontRed(sumSubCumBalAMT, 1);
                        }



                        sub = "<tr>"
                             + "<td><b>" + std.SubJobID + " " + std.DescriptionSubJob + "</b></td>"
                             + "<td align ='right'>" + SetFontRed(sumSubThisPeriodQTY, 1) + "</td>"
                             + "<td align ='right'>" + SetFontRed(sumSubThisPeriodAMT, 1) + "</td>"
                             + "<td align ='right'>" + SetFontRed(sumSubOpeningQTY, 1) + "</td>"
                             + "<td align ='right'>" + SetFontRed(sumSubOpeningAMT, 1) + "</td>"
                             + "<td align ='right'>" + SetFontRed(sumSubCumBalQTY, 1) + "</td>"
                             + "<td align ='right'>" + SetFontRed(sumSubCumBalAMT, 1) + "</td>"
                             + "<td align ='right'>" + SetFontRed(sumSubBudgetQTY, 1) + "</td >"
                             + "<td align ='right'>" + SetFontRed(Percen(sumSubCumBalQTY, sumSubBudgetQTY), 1) + "<b>%</b>" + "</td>"
                             + "<td align ='right'>" + SetFontRed(sumSubBudgetAMT, 1) + "</td>"
                             + "<td align ='right'>" + SetFontRed(Percen(sumSubCumBalAMT, sumSubBudgetAMT), 1) + "<b>%</b>" + "</td>"
                             + "</tr>";


                        TableJobCosts.Add(new TableJobCost
                        {
                            JobTask = std.SubJobID + " " + std.DescriptionSubJob,
                            ThisPeriodQTY = sumSubThisPeriodQTY,
                            ThisPeriodAMT = sumSubThisPeriodAMT,
                            OpenningQTY = sumSubOpeningQTY,
                            OpenningAMT = sumSubOpeningAMT,
                            CumBalQTY = sumSubCumBalQTY,
                            CumBalAMT = sumSubCumBalAMT,
                            BudgetQty = sumSubBudgetQTY,
                            PerQty = Percen(sumSubCumBalQTY, sumSubBudgetQTY),
                            BudgetAMT = sumSubBudgetAMT,
                            PerAMT = Percen(sumSubCumBalAMT, sumSubBudgetAMT)
                        });



                    }
                    check1 = 1;

                }
                else
                {
                    sub = "";
                }

                //-------End main


                if (std.JobTaskNo == "G0210")
                {
                    G0210 = std.CumBalAMT;
                    ViewBag.G0210N = SetFontRed(std.CumBalAMT * -1, 1);
                    ViewBag.G0210P = SetFontRed(std.CumBalAMT, 1);
                }
                if (std.JobTaskNo == "C0110")
                {
                    C0110 = std.CumBalAMT;
                    ViewBag.C0110 = SetFontRed(std.CumBalAMT, 1);
                }
                if (std.JobTaskNo == "C0120")
                {
                    C0120 = std.CumBalAMT;
                    ViewBag.C0120 = SetFontRed(std.CumBalAMT, 1);
                }

                if (std.JobTaskNo == "C0130")
                {
                    C0130 = std.CumBalAMT;
                    ViewBag.C0130 = SetFontRed(std.CumBalAMT, 1);
                }

                if (std.JobTaskNo == "C0410")
                {
                    C0410 = std.CumBalAMT;
                    ViewBag.C0410 = SetFontRed(std.CumBalAMT, 1);
                }

                if (std.JobTaskNo == "C0510")
                {
                    C0510 = std.CumBalAMT;
                    ViewBag.C0510 = SetFontRed(std.CumBalAMT, 1);
                }
                if (std.JobTaskNo == "C0910")
                {
                    C0910 = std.CumBalAMT;
                    ViewBag.C0910 = SetFontRed(std.CumBalAMT, 1);
                }
                if (std.JobTaskNo == "C1010")
                {
                    C1010 = std.CumBalAMT;
                    ViewBag.C1010 = SetFontRed(std.CumBalAMT, 1);
                }
                if (std.JobTaskNo == "C1110")
                {
                    C1110 = std.CumBalAMT;
                    ViewBag.C1110 = SetFontRed(std.CumBalAMT, 1);
                }
                if (std.JobTaskNo == "D0450")
                {
                    D0450 = std.CumBalAMT;
                    ViewBag.D0450 = SetFontRed(std.CumBalAMT, 1);
                }
                if (std.JobTaskNo == "H0110")
                {
                    H0110 = std.CumBalAMT;
                    ViewBag.H0110 = SetFontRed(std.CumBalAMT, 1);
                }
                if (std.JobTaskNo == "H0130")
                {
                    H0130 = std.CumBalAMT;
                    ViewBag.H0130 = SetFontRed(std.CumBalAMT, 1);
                }


                if (std.JobTaskNo == "E0910")
                {
                    E0910 = std.CumBalAMT;
                    ViewBag.E0910 = SetFontRed(std.CumBalAMT, 1);
                }
                if (std.JobTaskNo == "E0920")
                {
                    E0920 = std.CumBalAMT;
                    ViewBag.E0920 = SetFontRed(std.CumBalAMT, 1);
                }
                if (std.JobTaskNo == "E0930")
                {
                    E0930 = std.CumBalAMT;
                    ViewBag.E0930 = SetFontRed(std.CumBalAMT, 1);
                }
                if (std.JobTaskNo == "E0940")
                {
                    E0940 = std.CumBalAMT;
                    ViewBag.E0940 = SetFontRed(std.CumBalAMT, 1);
                }

                if (std.JobTaskNo == "F5999")
                {
                    F5999 = std.CumBalAMT;
                    ViewBag.F5999 = SetFontRed(std.CumBalAMT, 1);
                }



                if (std.MainJobID == std.JobTaskNo || std.SubJobID == std.JobTaskNo)
                {
                    table = "";
                }
                else
                {



                    table = "<tr>"
                         + "<td>" + std.DescriptionFull + "</td>"
                         + "<td align ='right'>" + SetFontRed(std.ThisPeriodQTY, 3) + "</td>"
                         + "<td align ='right'>" + SetFontRed(std.ThisperiodAMT, 3) + "</td>"
                         + "<td align ='right'>" + SetFontRed(std.OpeningQTY, 3) + "</td>"
                         + "<td align ='right'>" + SetFontRed(std.OpeningAMT, 3) + "</td>"
                         + "<td align ='right'>" + SetFontRed(std.CumBalQTY, 3) + "</td>"
                         + "<td align ='right'>" + SetFontRed(std.CumBalAMT, 3) + "</td>"
                         + "<td align ='right'>" + SetFontRed(std.BudgetQTY, 3) + "</td >"
                         + "<td align ='right'>" + SetFontRed(Percen(std.CumBalQTY, std.BudgetQTY), 3) + "%" + "</td>"
                         + "<td align ='right'>" + SetFontRed(std.BudgetAMT, 3) + "</td>"
                         + "<td align ='right'>" + SetFontRed(Percen(std.CumBalAMT, std.BudgetAMT), 3) + "%" + "</td>"
                         + "</tr>";


                    TableJobCosts.Add(new TableJobCost
                    {
                        JobTask = std.DescriptionFull,
                        ThisPeriodQTY = std.ThisPeriodQTY,
                        ThisPeriodAMT = std.ThisperiodAMT,
                        OpenningQTY = std.OpeningQTY,
                        OpenningAMT = std.OpeningAMT,
                        CumBalQTY = std.CumBalQTY,
                        CumBalAMT = std.CumBalAMT,
                        BudgetQty = std.BudgetQTY,
                        PerQty = Percen(std.CumBalQTY, std.BudgetQTY),
                        BudgetAMT = std.BudgetAMT,
                        PerAMT = Percen(std.CumBalAMT, std.BudgetAMT)
                    });



                }

                totalview += main + sub + table;
            }




            N00 = v_ReportJobCost.Where(c => c.MainJobID == "N0000").Sum(c => c.CumBalAMT);
            O00 = v_ReportJobCost.Where(c => c.MainJobID == "O0000").Sum(c => c.CumBalAMT);

            TIC += C0110 + C0120 + C0130 + C0410 + C0510 + C0910 + C1010 + C1110 + D0450 + H0110 + H0130 + H0200 + H0800;



            ViewBag.TIC = SetFontRed(TIC, 1);



            ViewBag.TCXI = SetFontRed(TCXI + (G0210 * -1), 1);  ///TCXI-TIC

            ViewBag.N00 = SetFontRed(N00, 1);
            ViewBag.O00 = SetFontRed(O00, 1);
            ViewBag.INCOME = SetFontRed((N00 + O00), 1);
            ViewBag.INCOME1 = SetFontRed((N00 + O00) * -1, 1);
            ViewBag.ExternalCost = SetFontRed(((TCXI + (G0210 * -1)) - TIC), 1);
            ViewBag.GrossProfit = SetFontRed(((N00 + O00) * -1) - ((TCXI + (G0210 * -1)) - TIC), 1);

            ViewBag.NetProfit = SetFontRed(((((N00 + O00) * -1) - ((TCXI + (G0210 * -1)) - TIC)) - TIC), 1);
            ViewBag.PerGross = SetFontRed(Percen(((((N00 + O00) * -1) - ((TCXI + (G0210 * -1)) - TIC))), (N00 + O00) * -1), 1) + "<b>%</b>";
            ViewBag.PerNet = SetFontRed(Percen(((((N00 + O00) * -1) - ((TCXI + (G0210 * -1)) - TIC)) - TIC), (N00 + O00) * -1), 1) + "<b>%</b>";

            ViewBag.table = headtable + totalview + "</tbody></table>";




            var footer = "<tr bgcolor='#ddd'>"
                 + "<td><b>Total Cost</b></td>"
                 + "<td align ='right'>" + SetFontRed(thisQ, 1) + "</td>"
                 + "<td align ='right'>" + SetFontRed(thisA, 1) + "</td>"
                 + "<td align ='right'>" + SetFontRed(openQ, 1) + "</td>"
                 + "<td align ='right'>" + SetFontRed(openA, 1) + "</td>"
                 + "<td align ='right'>" + SetFontRed(cumQ, 1) + "</td>"
                 + "<td align ='right'>" + SetFontRed(cumA, 1) + "</td>"
                 + "<td align ='right'>" + SetFontRed(budQ, 1) + "</td >"
                 + "<td align ='right'>" + SetFontRed(Percen(cumQ, budQ), 1) + "<b>%</b>" + "</td>"
                 + "<td align ='right'>" + SetFontRed(budA, 1) + "</td>"
                 + "<td align ='right'>" + SetFontRed(Percen(cumA, budA), 1) + "<b>%</b>" + "</td>"
                 + "</tr>"
                    +"<tr bgcolor='#ddd'><td><b>PROFIT(LOSS)</b></td> "
                 + "<td align ='right'>" + SetFontRed((OthisQ*-1)- thisQ, 1) + "</td>"
                 + "<td align ='right'>" + SetFontRed((OthisA*-1)- thisA, 1) + "</td>"
                 + "<td align ='right'>" + SetFontRed((OopenQ*-1)- openQ, 1) + "</td>"
                 + "<td align ='right'>" + SetFontRed((OopenA*-1)- openA, 1) + "</td>"
                 + "<td align ='right'>" + SetFontRed((OcumQ*-1)-cumQ, 1) + "</td>"
                 + "<td align ='right'>" + SetFontRed((OcumA*-1)-cumA, 1) + "</td>"
                 + "<td align ='right'>" + SetFontRed((ObudQ*-1)-budQ, 1) + "</td>"
                 + "<td align ='right'>" + SetFontRed(Percen((OcumQ*-1)- cumQ ,  (ObudQ*-1))-budQ, 1) + "<b>%</b>" + "</td>"
                 + "<td align ='right'>" + SetFontRed((ObudA*-1)- budA, 1) + "</th>"
                 + "<td align ='right'>" + SetFontRed(Percen((OcumA*-1)-cumA,(ObudA*-1)-budA), 1) + "<b>%</b>" + "</td>"
                 + "</tr>";


            var footerIncome= "<tr bgcolor='#ddd'><td><b> Total Income </b></td> "
                 + "<td align ='right'>" + SetFontRed(OthisQ, 1) + "</td>"
                 + "<td align ='right'>" + SetFontRed(OthisA, 1) + "</td>"
                 + "<td align ='right'>" + SetFontRed(OopenQ, 1) + "</td>"
                 + "<td align ='right'>" + SetFontRed(OopenA, 1) + "</td>"
                 + "<td align ='right'>" + SetFontRed(OcumQ, 1) + "</td>"
                 + "<td align ='right'>" + SetFontRed(OcumA, 1) + "</td>"
                 + "<td align ='right'>" + SetFontRed(ObudQ, 1) + "</td>"
                 + "<td align ='right'>" + SetFontRed(Percen(OcumQ, ObudQ), 1) + "<b>%</b>" + "</td>"
                 + "<td align ='right'>" + SetFontRed(ObudA, 1) + "</th>"
                 + "<td align ='right'>" + SetFontRed(Percen(OcumA, ObudA), 1) + "<b>%</b>" + "</td>"
                 + "</tr>";



           // ViewBag.table2 = Sumhead + summain + footer + "</tbody></table>";

            ViewBag.table3 = Sumhead + summainIncome + footerIncome + summain + footer+"</tbody></table>";

            //response = Ok(new { link = link });






            // return Ok(TableJobCosts);

            return View();
        }




        // GET: Levels/Details/5
 
        public IActionResult Add([Bind("EstimateJob,EstimateStart,EstimateEnd,Contractwork,ExtraWork,OtherIncome,AECT,UECT,EC,AICT,NetProfit,defalse,a1,a2, a3, a4, a5,  a6,  a7,  a8,  a9,  a10, a11, a12,  a13, a14, a15, a16, a17, b1, b2, b3, b4, c1, c2, c3, c4, c5,  c6,  c7,  c8,  c9,  c10,  c11,  c12,  c13,  c14,  c15,  c16,  c17,  c18,  c19,  c20,  c21,  c22,  c23,  c24,  c25,  c26,  c27,  c28,  c29,  c30,  c31,  c32,  c33,  c34,  c35,  c36,  c37")] Estimate estimate)
        {

            estimate.EstimateBy= HttpContext.Session.GetString("Username");
            estimate.DateCreate= DateTime.Now;

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
                link += "<button class='btn btn-info' data-animal-type='" + es.EstimateId + "' onclick='showdatahis(this);'>ไฟล์ " +i+"_"+ es.DateCreate.ToString("yyyyMMdd") +"</button>";
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






        public static string SetFontRed(decimal value1,int level) //Check Color
        {
            string result="";

            if (level == 1) {

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


        public static decimal Percen(decimal value1, decimal value2) 
        {
            decimal result = 0;
            try
            {
                result = Math.Round(((value1/value2) * 100),2); 
            }
            catch
            {
                result = 0;
            }
            return result;
        }


        /// <summary>
        /// /Home/FileReport
        /// </summary>
        public IActionResult FileReport([Bind("EstimateJob,EstimateStart,EstimateEnd,Contractwork,ExtraWork,OtherIncome,AECT,UECT,EC,AICT,NetProfit,defalse,a1,a2, a3, a4, a5,  a6,  a7,  a8,  a9,  a10, a11, a12,  a13, a14, a15, a16, a17, b1, b2, b3, b4, c1, c2, c3, c4, c5,  c6,  c7,  c8,  c9,  c10,  c11,  c12,  c13,  c14,  c15,  c16,  c17,  c18,  c19,  c20,  c21,  c22,  c23,  c24,  c25,  c26,  c27,  c28,  c29,  c30,  c31,  c32,  c33,  c34,  c35,  c36,  c37")] Estimate estimate)
        {
            var fileDownloadName = "report.xlsx";
            var reportsFolder = "reports";

            using (var package = createExcelPackage(estimate))
            {
                package.SaveAs(new FileInfo(Path.Combine(_hostingEnvironment.WebRootPath, reportsFolder, fileDownloadName)));

            }


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
            worksheet.Cells[1, 1].Value = "Job : " +estimate.EstimateJob +" ช่วงเวลา : "+ estimate.EstimateStart+" - " + estimate.EstimateEnd ;
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
            worksheet.Cells[5, 5].Value= decimal.Parse(estimate.c3);
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
            worksheet.Cells[11, 3].Value ="";
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
            worksheet.Cells[30, 5].Value= decimal.Parse(estimate.c27);
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




            var queryData = " SELECT ROW_NUMBER() OVER (ORDER BY b.JobTaskNo) as ID,b.MainJobID,b.DescriptionMainJob,b.SubJobID,b.DescriptionSubJob,b.JobTaskNo,b.DescriptionFull,"
            + " (((CASE WHEN SUBSTRING(b.MainJobID, 1, 1) = 'N' or SUBSTRING(b.MainJobID,1,1)= 'O' THEN "
            + " (select isnull(sum([Quantity]) + sum([Line Amount]),0) FROM"+ Environment.GetEnvironmentVariable("Company") +"Job Ledger Entry] WHERE [Job No_] = {0} AND [Posting Date] > {1} AND [Posting Date] < {2} and SUBSTRING([Job Task No_],1,5) = b.JobTaskNo and [Type of task]=0 )	"
            + " ELSE "
            + " (select isnull(sum([Quantity]), 0) FROM"+ Environment.GetEnvironmentVariable("Company") +"Job Ledger Entry] WHERE[Job No_] = {0} AND [Posting Date] > {1} AND [Posting Date] < {2} and SUBSTRING([Job Task No_],1,5) = b.JobTaskNo and [Type of task]=0) END "
            + " )+"
            + " (CASE WHEN SUBSTRING(b.MainJobID, 1, 1) = 'N' or SUBSTRING(b.MainJobID,1,1)= 'O' THEN "
            + " (select isnull(sum([Quantity]) + sum([Line Amount]),0) FROM"+ Environment.GetEnvironmentVariable("Company") +"Job Ledger Entry] WHERE[Job No_] = {0} AND [Posting Date] > {1} AND [Posting Date] < {2} and SUBSTRING([Job Task No_],1,5) = b.JobTaskNo and [Type of task]=3 )	"
            + " ELSE "
            + "	(select isnull(sum([Quantity]), 0) FROM"+ Environment.GetEnvironmentVariable("Company") +"Job Ledger Entry] WHERE[Job No_] = {0} AND [Posting Date] > {1} AND [Posting Date] < {2} and SUBSTRING([Job Task No_],1,5) = b.JobTaskNo and [Type of task]=3) END "
            + " )+"
            + " (CASE WHEN SUBSTRING(b.MainJobID, 1, 1) = 'N' or SUBSTRING(b.MainJobID,1,1)= 'O' THEN "
            + " (select isnull(sum([Quantity]) + sum([Line Amount]),0) FROM"+ Environment.GetEnvironmentVariable("Company") +"Job Ledger Entry] WHERE[Job No_] = {0} AND [Posting Date] > {1} AND [Posting Date] < {2} and SUBSTRING([Job Task No_],1,5) = b.JobTaskNo and [Type of task]=2)	"
            + " ELSE "
            + " (select isnull(sum([Quantity]), 0) FROM"+ Environment.GetEnvironmentVariable("Company") +"Job Ledger Entry] WHERE[Job No_] = {0} AND [Posting Date] > {1} AND [Posting Date] < {2} and SUBSTRING([Job Task No_],1,5) = b.JobTaskNo and [Type of task]=2) END "
            + " )" +
            "+((select isnull(sum([Quantity]), 0) FROM "+ Environment.GetEnvironmentVariable("Company") +"Job Ledger Entry] WHERE [From Location] = {0} AND [Posting Date] > {1} AND [Posting Date] < {2} and SUBSTRING([Job Task No_],1,5) = b.JobTaskNo and [Type of task]=1 and [Document No_] Like 'CAL%' )*2)) -"
            + " ((CASE WHEN SUBSTRING(b.MainJobID, 1, 1) = 'N' or SUBSTRING(b.MainJobID,1,1)= 'O' THEN "
            + " (select isnull(sum([Quantity]) + sum([Line Amount]),0) FROM"+ Environment.GetEnvironmentVariable("Company") +"Job Ledger Entry] WHERE[Job No_] = {0} AND [Posting Date] > {1} AND [Posting Date] < {2} and SUBSTRING([Job Task No_],1,5) = b.JobTaskNo and [Type of task]=1)	"
            + " ELSE "
            + " (select isnull(sum([Quantity]), 0) FROM"+ Environment.GetEnvironmentVariable("Company") +"Job Ledger Entry] WHERE[Job No_] = {0} AND [Posting Date] > {1} AND [Posting Date] < {2} and SUBSTRING([Job Task No_],1,5) = b.JobTaskNo and [Type of task]=1) END "
            + " )+ "
            + " (select isnull(sum([Quantity]), 0) FROM "+ Environment.GetEnvironmentVariable("Company") +"Job Ledger Entry] WHERE [From Location] = {0} AND [Posting Date] > {1} AND [Posting Date] < {2} and SUBSTRING([Job Task No_],1,5) = b.JobTaskNo and [Type of task]=3 and [Job No_] <> {0} )))  AS ThisPeriodQTY "
            + " , "

            + " (((CASE WHEN SUBSTRING(b.MainJobID, 1, 1) = 'N' or SUBSTRING(b.MainJobID,1,1)= 'O' THEN "
            + " (select isnull(sum([Original Total Cost]) + sum([Line Amount]),0) FROM"+ Environment.GetEnvironmentVariable("Company") +"Job Ledger Entry] WHERE [Job No_] = {0} AND [Posting Date] > {1} AND [Posting Date] < {2} and SUBSTRING([Job Task No_],1,5) = b.JobTaskNo and [Type of task]=0 )	"
            + " ELSE "
            + " (select isnull(sum([Original Total Cost]), 0) FROM"+ Environment.GetEnvironmentVariable("Company") +"Job Ledger Entry] WHERE[Job No_] = {0} AND [Posting Date] > {1} AND [Posting Date] < {2} and SUBSTRING([Job Task No_],1,5) = b.JobTaskNo and [Type of task]=0) END "
            + " )+"
            + " (CASE WHEN SUBSTRING(b.MainJobID, 1, 1) = 'N' or SUBSTRING(b.MainJobID,1,1)= 'O' THEN "
            + " (select isnull(sum([Original Total Cost]) + sum([Line Amount]),0) FROM"+ Environment.GetEnvironmentVariable("Company") +"Job Ledger Entry] WHERE[Job No_] = {0} AND [Posting Date] > {1} AND [Posting Date] < {2} and SUBSTRING([Job Task No_],1,5) = b.JobTaskNo and [Type of task]=3 )	"
            + " ELSE "
            + "	(select isnull(sum([Original Total Cost]), 0) FROM"+ Environment.GetEnvironmentVariable("Company") +"Job Ledger Entry] WHERE[Job No_] = {0} AND [Posting Date] > {1} AND [Posting Date] < {2} and SUBSTRING([Job Task No_],1,5) = b.JobTaskNo and [Type of task]=3) END "
            + " )+"
            + " (CASE WHEN SUBSTRING(b.MainJobID, 1, 1) = 'N' or SUBSTRING(b.MainJobID,1,1)= 'O' THEN "
            + " (select isnull(sum([Original Total Cost]) + sum([Line Amount]),0) FROM"+ Environment.GetEnvironmentVariable("Company") +"Job Ledger Entry] WHERE[Job No_] = {0} AND [Posting Date] > {1} AND [Posting Date] < {2} and SUBSTRING([Job Task No_],1,5) = b.JobTaskNo and [Type of task]=2)	"
            + " ELSE "
            + " (select isnull(sum([Original Total Cost]), 0) FROM"+ Environment.GetEnvironmentVariable("Company") +"Job Ledger Entry] WHERE[Job No_] = {0} AND [Posting Date] > {1} AND [Posting Date] < {2} and SUBSTRING([Job Task No_],1,5) = b.JobTaskNo and [Type of task]=2) END "
            + " )"
            + "+((select isnull(sum([Original Total Cost]), 0) FROM "+ Environment.GetEnvironmentVariable("Company") +"Job Ledger Entry] WHERE [From Location] = {0} AND [Posting Date] > {1} AND [Posting Date] < {2} and SUBSTRING([Job Task No_],1,5) = b.JobTaskNo and [Type of task]=1 and [Document No_] Like 'CAL%' )*2)) -"
            + " ((CASE WHEN SUBSTRING(b.MainJobID, 1, 1) = 'N' or SUBSTRING(b.MainJobID,1,1)= 'O' THEN "
            + " (select isnull(sum([Original Total Cost]) + sum([Line Amount]),0) FROM"+ Environment.GetEnvironmentVariable("Company") +"Job Ledger Entry] WHERE[Job No_] = {0} AND [Posting Date] > {1} AND [Posting Date] < {2} and SUBSTRING([Job Task No_],1,5) = b.JobTaskNo and [Type of task]=1)	"
            + " ELSE "
            + " (select isnull(sum([Original Total Cost]), 0) FROM"+ Environment.GetEnvironmentVariable("Company") +"Job Ledger Entry] WHERE[Job No_] = {0} AND [Posting Date] > {1} AND [Posting Date] < {2} and SUBSTRING([Job Task No_],1,5) = b.JobTaskNo and [Type of task]=1) END "
            + " )+ "
            + " (select isnull(sum([Original Total Cost]), 0) FROM "+ Environment.GetEnvironmentVariable("Company") +"Job Ledger Entry] WHERE [From Location] = {0} AND [Posting Date] > {1} AND [Posting Date] < {2} and SUBSTRING([Job Task No_],1,5) = b.JobTaskNo and [Type of task]=3 and [Job No_] <> {0} ))" +

              "+"
              + "(CASE WHEN SUBSTRING(b.MainJobID, 1, 1) = 'N' or SUBSTRING(b.MainJobID,1,1)= 'O' THEN 0 ELSE  	(select isnull(sum([Total Cost]), 0) FROM"+ Environment.GetEnvironmentVariable("Company") +"Job Ledger Entry] WHERE[Job No_] = {0}  AND [Posting Date] > {1} AND [Posting Date] < {2}  and SUBSTRING([Job Task No_],1,5) = b.JobTaskNo and [Type of task]=0 and [Original Total Cost]=0) END "
             + " ))  AS ThisPeriodAMT "

            + " , "

            + " (((CASE WHEN SUBSTRING(b.MainJobID, 1, 1) = 'N' or SUBSTRING(b.MainJobID,1,1)= 'O' THEN "
            + " (select isnull(sum([Quantity]) + sum([Line Amount]),0) FROM"+ Environment.GetEnvironmentVariable("Company") +"Job Ledger Entry] WHERE [Job No_] = {0} AND [Posting Date] < {1} and SUBSTRING([Job Task No_],1,5) = b.JobTaskNo and [Type of task]=0 )	"
            + " ELSE "
            + " (select isnull(sum([Quantity]), 0) FROM"+ Environment.GetEnvironmentVariable("Company") +"Job Ledger Entry] WHERE[Job No_] = {0} AND [Posting Date] < {1} and SUBSTRING([Job Task No_],1,5) = b.JobTaskNo and [Type of task]=0) END "
            + " )+"
            + " (CASE WHEN SUBSTRING(b.MainJobID, 1, 1) = 'N' or SUBSTRING(b.MainJobID,1,1)= 'O' THEN "
            + " (select isnull(sum([Quantity]) + sum([Line Amount]),0) FROM"+ Environment.GetEnvironmentVariable("Company") +"Job Ledger Entry] WHERE[Job No_] = {0} AND [Posting Date] < {1} and SUBSTRING([Job Task No_],1,5) = b.JobTaskNo and [Type of task]=3 )	"
            + " ELSE "
            + "	(select isnull(sum([Quantity]), 0) FROM"+ Environment.GetEnvironmentVariable("Company") +"Job Ledger Entry] WHERE[Job No_] = {0} AND[Posting Date] < {1} and SUBSTRING([Job Task No_],1,5) = b.JobTaskNo and [Type of task]=3) END "
            + " )+"
            + " (CASE WHEN SUBSTRING(b.MainJobID, 1, 1) = 'N' or SUBSTRING(b.MainJobID,1,1)= 'O' THEN "
            + " (select isnull(sum([Quantity]) + sum([Line Amount]),0) FROM"+ Environment.GetEnvironmentVariable("Company") +"Job Ledger Entry] WHERE[Job No_] = {0} AND [Posting Date] < {1} and SUBSTRING([Job Task No_],1,5) = b.JobTaskNo and [Type of task]=2)	"
            + " ELSE "
            + " (select isnull(sum([Quantity]), 0) FROM"+ Environment.GetEnvironmentVariable("Company") +"Job Ledger Entry] WHERE[Job No_] = {0} AND [Posting Date] < {1} and SUBSTRING([Job Task No_],1,5) = b.JobTaskNo and [Type of task]=2) END "
            + " )"
            + "+((select isnull(sum([Quantity]), 0) FROM "+ Environment.GetEnvironmentVariable("Company") +"Job Ledger Entry] WHERE [From Location] = {0} AND [Posting Date] < {1}  and SUBSTRING([Job Task No_],1,5) = b.JobTaskNo and [Type of task]=1 and [Document No_] Like 'CAL%' )*2)) -"
            + " ((CASE WHEN SUBSTRING(b.MainJobID, 1, 1) = 'N' or SUBSTRING(b.MainJobID,1,1)= 'O' THEN "
            + " (select isnull(sum([Quantity]) + sum([Line Amount]),0) FROM"+ Environment.GetEnvironmentVariable("Company") +"Job Ledger Entry] WHERE[Job No_] = {0} AND [Posting Date] < {1} and SUBSTRING([Job Task No_],1,5) = b.JobTaskNo and [Type of task]=1)	"
            + " ELSE "
            + " (select isnull(sum([Quantity]), 0) FROM"+ Environment.GetEnvironmentVariable("Company") +"Job Ledger Entry] WHERE[Job No_] = {0} AND [Posting Date] < {1} and SUBSTRING([Job Task No_],1,5) = b.JobTaskNo and [Type of task]=1) END "
            + " )+ "
            + " (select isnull(sum([Quantity]), 0) FROM "+ Environment.GetEnvironmentVariable("Company") +"Job Ledger Entry] WHERE [From Location] = {0} AND [Posting Date] < {1} and SUBSTRING([Job Task No_],1,5) = b.JobTaskNo and [Type of task]=3 and [Job No_] <> {0} )))  AS OpeningQTY "
            + " , "

            + " (((CASE WHEN SUBSTRING(b.MainJobID, 1, 1) = 'N' or SUBSTRING(b.MainJobID,1,1)= 'O' THEN "
            + " (select isnull(sum([Original Total Cost]) + sum([Line Amount]),0) FROM"+ Environment.GetEnvironmentVariable("Company") +"Job Ledger Entry] WHERE [Job No_] = {0} AND  [Posting Date] < {1} and SUBSTRING([Job Task No_],1,5) = b.JobTaskNo and [Type of task]=0 )	"
            + " ELSE "
            + " (select isnull(sum([Original Total Cost]), 0) FROM"+ Environment.GetEnvironmentVariable("Company") +"Job Ledger Entry] WHERE[Job No_] = {0} AND [Posting Date] < {1} and SUBSTRING([Job Task No_],1,5) = b.JobTaskNo and [Type of task]=0) END "
            + " )+"
            + " (CASE WHEN SUBSTRING(b.MainJobID, 1, 1) = 'N' or SUBSTRING(b.MainJobID,1,1)= 'O' THEN "
            + " (select isnull(sum([Original Total Cost]) + sum([Line Amount]),0) FROM"+ Environment.GetEnvironmentVariable("Company") +"Job Ledger Entry] WHERE[Job No_] = {0} AND [Posting Date] < {1} and SUBSTRING([Job Task No_],1,5) = b.JobTaskNo and [Type of task]=3 )	"
            + " ELSE "
            + "	(select isnull(sum([Original Total Cost]), 0) FROM"+ Environment.GetEnvironmentVariable("Company") +"Job Ledger Entry] WHERE[Job No_] = {0} AND [Posting Date] < {1} and SUBSTRING([Job Task No_],1,5) = b.JobTaskNo and [Type of task]=3) END "
            + " )+"
            + " (CASE WHEN SUBSTRING(b.MainJobID, 1, 1) = 'N' or SUBSTRING(b.MainJobID,1,1)= 'O' THEN "
            + " (select isnull(sum([Original Total Cost]) + sum([Line Amount]),0) FROM"+ Environment.GetEnvironmentVariable("Company") +"Job Ledger Entry] WHERE[Job No_] = {0} AND [Posting Date] < {1} and SUBSTRING([Job Task No_],1,5) = b.JobTaskNo and [Type of task]=2)	"
            + " ELSE "
            + " (select isnull(sum([Original Total Cost]), 0) FROM"+ Environment.GetEnvironmentVariable("Company") +"Job Ledger Entry] WHERE[Job No_] = {0} AND [Posting Date] < {1} and SUBSTRING([Job Task No_],1,5) = b.JobTaskNo and [Type of task]=2) END "
            + " )"
            + " +((select isnull(sum([Original Total Cost]), 0) FROM "+ Environment.GetEnvironmentVariable("Company") +"Job Ledger Entry] WHERE [From Location] = {0} AND [Posting Date] < {1}  and SUBSTRING([Job Task No_],1,5) = b.JobTaskNo and [Type of task]=1 and [Document No_] Like 'CAL%' )*2)) -"
            + " ((CASE WHEN SUBSTRING(b.MainJobID, 1, 1) = 'N' or SUBSTRING(b.MainJobID,1,1)= 'O' THEN "
            + " (select isnull(sum([Original Total Cost]) + sum([Line Amount]),0) FROM"+ Environment.GetEnvironmentVariable("Company") +"Job Ledger Entry] WHERE[Job No_] = {0} AND [Posting Date] < {1} and SUBSTRING([Job Task No_],1,5) = b.JobTaskNo and [Type of task]=1)	"
            + " ELSE "
            + " (select isnull(sum([Original Total Cost]), 0) FROM"+ Environment.GetEnvironmentVariable("Company") +"Job Ledger Entry] WHERE[Job No_] = {0} AND [Posting Date] < {1} and SUBSTRING([Job Task No_],1,5) = b.JobTaskNo and [Type of task]=1) END "
            + " )+ "
            + " (select isnull(sum([Original Total Cost]), 0) FROM "+ Environment.GetEnvironmentVariable("Company") +"Job Ledger Entry] WHERE [From Location] = {0} AND [Posting Date] < {1} and SUBSTRING([Job Task No_],1,5) = b.JobTaskNo and [Type of task]=3 and [Job No_] <> {0} ))" +
                "+" +
              "(CASE WHEN SUBSTRING(b.MainJobID, 1, 1) = 'N' or SUBSTRING(b.MainJobID,1,1)= 'O' THEN 0 ELSE  	(select isnull(sum([Total Cost]), 0) FROM"+ Environment.GetEnvironmentVariable("Company") +"Job Ledger Entry] WHERE[Job No_] = {0} AND [Posting Date] < {1} and SUBSTRING([Job Task No_],1,5) = b.JobTaskNo and [Type of task]=0 and [Original Total Cost]=0) END" +
              " ))  AS OpeningAMT "






            + " , "

            + " (((CASE WHEN SUBSTRING(b.MainJobID, 1, 1) = 'N' or SUBSTRING(b.MainJobID,1,1)= 'O' THEN "
            + " (select isnull(sum([Quantity]) + sum([Line Amount]),0) FROM"+ Environment.GetEnvironmentVariable("Company") +"Job Ledger Entry] WHERE [Job No_] = {0} AND [Posting Date] < {2} and SUBSTRING([Job Task No_],1,5) = b.JobTaskNo and [Type of task]=0 )	"
            + " ELSE "
            + " (select isnull(sum([Quantity]), 0) FROM"+ Environment.GetEnvironmentVariable("Company") +"Job Ledger Entry] WHERE[Job No_] = {0} AND [Posting Date] < {2} and SUBSTRING([Job Task No_],1,5) = b.JobTaskNo and [Type of task]=0) END "
            + " )+"
            + " (CASE WHEN SUBSTRING(b.MainJobID, 1, 1) = 'N' or SUBSTRING(b.MainJobID,1,1)= 'O' THEN "
            + " (select isnull(sum([Quantity]) + sum([Line Amount]),0) FROM"+ Environment.GetEnvironmentVariable("Company") +"Job Ledger Entry] WHERE[Job No_] = {0} AND [Posting Date] < {2} and SUBSTRING([Job Task No_],1,5) = b.JobTaskNo and [Type of task]=3 )	"
            + " ELSE "
            + "	(select isnull(sum([Quantity]), 0) FROM"+ Environment.GetEnvironmentVariable("Company") +"Job Ledger Entry] WHERE[Job No_] = {0} AND[Posting Date] < {2} and SUBSTRING([Job Task No_],1,5) = b.JobTaskNo and [Type of task]=3) END "
            + " )+"
            + " (CASE WHEN SUBSTRING(b.MainJobID, 1, 1) = 'N' or SUBSTRING(b.MainJobID,1,1)= 'O' THEN "
            + " (select isnull(sum([Quantity]) + sum([Line Amount]),0) FROM"+ Environment.GetEnvironmentVariable("Company") +"Job Ledger Entry] WHERE[Job No_] = {0} AND [Posting Date] < {2} and SUBSTRING([Job Task No_],1,5) = b.JobTaskNo and [Type of task]=2)	"
            + " ELSE "
            + " (select isnull(sum([Quantity]), 0) FROM"+ Environment.GetEnvironmentVariable("Company") +"Job Ledger Entry] WHERE[Job No_] = {0} AND [Posting Date] < {2} and SUBSTRING([Job Task No_],1,5) = b.JobTaskNo and [Type of task]=2) END "
            + " )) -"
            + " ((CASE WHEN SUBSTRING(b.MainJobID, 1, 1) = 'N' or SUBSTRING(b.MainJobID,1,1)= 'O' THEN "
            + " (select isnull(sum([Quantity]) + sum([Line Amount]),0) FROM"+ Environment.GetEnvironmentVariable("Company") +"Job Ledger Entry] WHERE[Job No_] = {0} AND [Posting Date] < {2} and SUBSTRING([Job Task No_],1,5) = b.JobTaskNo and [Type of task]=1)	"
            + " ELSE "
            + " (select isnull(sum([Quantity]), 0) FROM"+ Environment.GetEnvironmentVariable("Company") +"Job Ledger Entry] WHERE[Job No_] = {0} AND [Posting Date] < {2} and SUBSTRING([Job Task No_],1,5) = b.JobTaskNo and [Type of task]=1) END "
            + " )+ "
            + " (select isnull(sum([Quantity]), 0) FROM "+ Environment.GetEnvironmentVariable("Company") +"Job Ledger Entry] WHERE [From Location] = {0} AND [Posting Date] < {2} and SUBSTRING([Job Task No_],1,5) = b.JobTaskNo and [Type of task]=3 and [Job No_] <> {0} )))  AS CumBalQTY "
            + " , "

              + " (((CASE WHEN SUBSTRING(b.MainJobID, 1, 1) = 'N' or SUBSTRING(b.MainJobID,1,1)= 'O' THEN "
              + " (select isnull(sum([Original Total Cost]) + sum([Line Amount]),0) FROM"+ Environment.GetEnvironmentVariable("Company") +"Job Ledger Entry] WHERE [Job No_] = {0} AND  [Posting Date] < {2} and SUBSTRING([Job Task No_],1,5) = b.JobTaskNo and [Type of task]=0 )	"
              + " ELSE "
              + " (select isnull(sum([Original Total Cost]), 0) FROM"+ Environment.GetEnvironmentVariable("Company") +"Job Ledger Entry] WHERE[Job No_] = {0} AND [Posting Date] < {2} and SUBSTRING([Job Task No_],1,5) = b.JobTaskNo and [Type of task]=0) END "
              + " )+"
              + " (CASE WHEN SUBSTRING(b.MainJobID, 1, 1) = 'N' or SUBSTRING(b.MainJobID,1,1)= 'O' THEN "
              + " (select isnull(sum([Original Total Cost]) + sum([Line Amount]),0) FROM"+ Environment.GetEnvironmentVariable("Company") +"Job Ledger Entry] WHERE[Job No_] = {0} AND [Posting Date] < {2} and SUBSTRING([Job Task No_],1,5) = b.JobTaskNo and [Type of task]=3 )	"
              + " ELSE "
              + "	(select isnull(sum([Original Total Cost]), 0) FROM"+ Environment.GetEnvironmentVariable("Company") +"Job Ledger Entry] WHERE[Job No_] = {0} AND [Posting Date] < {2} and SUBSTRING([Job Task No_],1,5) = b.JobTaskNo and [Type of task]=3) END "
              + " )+"
              + " (CASE WHEN SUBSTRING(b.MainJobID, 1, 1) = 'N' or SUBSTRING(b.MainJobID,1,1)= 'O' THEN "
              + " (select isnull(sum([Original Total Cost]) + sum([Line Amount]),0) FROM"+ Environment.GetEnvironmentVariable("Company") +"Job Ledger Entry] WHERE[Job No_] = {0} AND [Posting Date] < {2} and SUBSTRING([Job Task No_],1,5) = b.JobTaskNo and [Type of task]=2)	"
              + " ELSE "
              + " (select isnull(sum([Original Total Cost]), 0) FROM"+ Environment.GetEnvironmentVariable("Company") +"Job Ledger Entry] WHERE[Job No_] = {0} AND [Posting Date] < {2} and SUBSTRING([Job Task No_],1,5) = b.JobTaskNo and [Type of task]=2) END "
              + " )"
              + " +((select isnull(sum([Original Total Cost]), 0) FROM "+ Environment.GetEnvironmentVariable("Company") +"Job Ledger Entry] WHERE [From Location] = {0} AND [Posting Date] < {2}  and SUBSTRING([Job Task No_],1,5) = b.JobTaskNo and [Type of task]=1 and [Document No_] Like 'CAL%' )*2)) -"
              + " ((CASE WHEN SUBSTRING(b.MainJobID, 1, 1) = 'N' or SUBSTRING(b.MainJobID,1,1)= 'O' THEN "
              + " (select isnull(sum([Original Total Cost]) + sum([Line Amount]),0) FROM"+ Environment.GetEnvironmentVariable("Company") +"Job Ledger Entry] WHERE[Job No_] = {0} AND [Posting Date] < {2} and SUBSTRING([Job Task No_],1,5) = b.JobTaskNo and [Type of task]=1)	"
              + " ELSE "
              + " (select isnull(sum([Original Total Cost]), 0) FROM"+ Environment.GetEnvironmentVariable("Company") +"Job Ledger Entry] WHERE[Job No_] = {0} AND [Posting Date] < {2} and SUBSTRING([Job Task No_],1,5) = b.JobTaskNo and [Type of task]=1) END "
              + " )+ "
              + " (select isnull(sum([Original Total Cost]), 0) FROM "+ Environment.GetEnvironmentVariable("Company") +"Job Ledger Entry] WHERE [From Location] = {0} AND [Posting Date] < {2} and SUBSTRING([Job Task No_],1,5) = b.JobTaskNo and [Type of task]=3 and [Job No_] <> {0} ))" +


              "+" +
              "(CASE WHEN SUBSTRING(b.MainJobID, 1, 1) = 'N' or SUBSTRING(b.MainJobID,1,1)= 'O' THEN 0 ELSE  	(select isnull(sum([Total Cost]), 0) FROM"+ Environment.GetEnvironmentVariable("Company") +"Job Ledger Entry] WHERE[Job No_] = {0} AND [Posting Date] < {2} and SUBSTRING([Job Task No_],1,5) = b.JobTaskNo and [Type of task]=0 and [Original Total Cost]=0) END ))  AS CumBalAMT "
            + " , "

            + " (SELECT isnull(sum(Quantity), 0) FROM dbo."+ Environment.GetEnvironmentVariable("Company") +"Job Planning Line] WHERE[Job No_]={0} and SUBSTRING([Job Task No_],1,5)=b.JobTaskNo) as BudgetQTY, "
            + " (SELECT isnull(sum([Total Cost (LCY)]),0) FROM dbo."+ Environment.GetEnvironmentVariable("Company") +"Job Planning Line] WHERE[Job No_]= {0} and SUBSTRING([Job Task No_],1,5)= b.JobTaskNo) as BudgetAMT "

            + " FROM("
             + " SELECT a.JobTaskNo,a.MainJobID,a.SubJobID,"
             + " (SELECT TOP 1 dbo."+ Environment.GetEnvironmentVariable("Company") +"Job Task].Description FROM dbo."+ Environment.GetEnvironmentVariable("Company") +"Job Task] WHERE [Job Task No_]=a.JobTaskNo) as Description,"
             + " (SELECT TOP 1 ([Job Task No_]+' '+Description) FROM dbo."+ Environment.GetEnvironmentVariable("Company") +"Job Task] WHERE [Job Task No_]=a.JobTaskNo) as DescriptionFull,"
             + " (SELECT TOP 1 dbo."+ Environment.GetEnvironmentVariable("Company") +"Job Task].Description FROM dbo."+ Environment.GetEnvironmentVariable("Company") +"Job Task] WHERE SUBSTRING(dbo."+ Environment.GetEnvironmentVariable("Company") +"Job Task].[Job Task No_],1,1)+'0000'=a.MainJobID) as DescriptionMainJob,"
             + " (SELECT TOP 1 dbo."+ Environment.GetEnvironmentVariable("Company") +"Job Task].Description FROM dbo."+ Environment.GetEnvironmentVariable("Company") +"Job Task] WHERE SUBSTRING(dbo."+ Environment.GetEnvironmentVariable("Company") +"Job Task].[Job Task No_],1,3)+'00'=a.SubJobID) as DescriptionSubJob"
             + " FROM  (SELECT DISTINCT dbo."+ Environment.GetEnvironmentVariable("Company") +"Job Task].[Job Task No_] as JobTaskNo,"
             + " SUBSTRING(dbo."+ Environment.GetEnvironmentVariable("Company") +"Job Task].[Job Task No_],1,1)+'0000' as MainJobID,SUBSTRING(dbo."+ Environment.GetEnvironmentVariable("Company") +"Job Task].[Job Task No_],1,3)+'00' as SubJobID FROM dbo."+ Environment.GetEnvironmentVariable("Company") +"Job Task]) as a) as b ORDER BY b.JobTaskNo";



            //var queryData = " SELECT b.MainJobID,b.DescriptionMainJob,b.SubJobID,b.DescriptionSubJob,b.JobTaskNo,b.DescriptionFull,"
            //            + " (((CASE WHEN SUBSTRING(b.MainJobID, 1, 1) = 'N' or SUBSTRING(b.MainJobID,1,1)= 'O' THEN "
            //            + " (select isnull(sum([Quantity]) + sum([Line Amount]),0) FROM"+ Environment.GetEnvironmentVariable("Company") +"Job Ledger Entry] WHERE [Job No_] = {0} AND [Posting Date] > {1} AND [Posting Date] < {2} and SUBSTRING([Job Task No_],1,5) = b.JobTaskNo and [Type of task]=0 )	"
            //            + " ELSE "
            //            + " (select isnull(sum([Quantity]), 0) FROM"+ Environment.GetEnvironmentVariable("Company") +"Job Ledger Entry] WHERE[Job No_] = {0} AND [Posting Date] > {1} AND [Posting Date] < {2} and SUBSTRING([Job Task No_],1,5) = b.JobTaskNo and [Type of task]=0) END "
            //            + " )+"
            //            + " (CASE WHEN SUBSTRING(b.MainJobID, 1, 1) = 'N' or SUBSTRING(b.MainJobID,1,1)= 'O' THEN "
            //            + " (select isnull(sum([Quantity]) + sum([Line Amount]),0) FROM"+ Environment.GetEnvironmentVariable("Company") +"Job Ledger Entry] WHERE[Job No_] = {0} AND [Posting Date] > {1} AND [Posting Date] < {2} and SUBSTRING([Job Task No_],1,5) = b.JobTaskNo and [Type of task]=3 )	"
            //            + " ELSE "
            //            + "	(select isnull(sum([Quantity]), 0) FROM"+ Environment.GetEnvironmentVariable("Company") +"Job Ledger Entry] WHERE[Job No_] = {0} AND [Posting Date] > {1} AND [Posting Date] < {2} and SUBSTRING([Job Task No_],1,5) = b.JobTaskNo and [Type of task]=3) END "
            //            + " )+"
            //            + " (CASE WHEN SUBSTRING(b.MainJobID, 1, 1) = 'N' or SUBSTRING(b.MainJobID,1,1)= 'O' THEN "
            //            + " (select isnull(sum([Quantity]) + sum([Line Amount]),0) FROM"+ Environment.GetEnvironmentVariable("Company") +"Job Ledger Entry] WHERE[Job No_] = {0} AND [Posting Date] > {1} AND [Posting Date] < {2} and SUBSTRING([Job Task No_],1,5) = b.JobTaskNo and [Type of task]=2)	"
            //            + " ELSE "
            //            + " (select isnull(sum([Quantity]), 0) FROM"+ Environment.GetEnvironmentVariable("Company") +"Job Ledger Entry] WHERE[Job No_] = {0} AND [Posting Date] > {1} AND [Posting Date] < {2} and SUBSTRING([Job Task No_],1,5) = b.JobTaskNo and [Type of task]=2) END "
            //            + " )" +
            //            "+((select isnull(sum([Quantity]), 0) FROM "+ Environment.GetEnvironmentVariable("Company") +"Job Ledger Entry] WHERE [From Location] = {0} AND [Posting Date] > {1} AND [Posting Date] < {2} and SUBSTRING([Job Task No_],1,5) = b.JobTaskNo and [Type of task]=1 and [Document No_] Like 'CAL%' )*2)) -"
            //            + " ((CASE WHEN SUBSTRING(b.MainJobID, 1, 1) = 'N' or SUBSTRING(b.MainJobID,1,1)= 'O' THEN "
            //            + " (select isnull(sum([Quantity]) + sum([Line Amount]),0) FROM"+ Environment.GetEnvironmentVariable("Company") +"Job Ledger Entry] WHERE[Job No_] = {0} AND [Posting Date] > {1} AND [Posting Date] < {2} and SUBSTRING([Job Task No_],1,5) = b.JobTaskNo and [Type of task]=1)	"
            //            + " ELSE "
            //            + " (select isnull(sum([Quantity]), 0) FROM"+ Environment.GetEnvironmentVariable("Company") +"Job Ledger Entry] WHERE[Job No_] = {0} AND [Posting Date] > {1} AND [Posting Date] < {2} and SUBSTRING([Job Task No_],1,5) = b.JobTaskNo and [Type of task]=1) END "
            //            + " )+ "
            //            + " (select isnull(sum([Quantity]), 0) FROM "+ Environment.GetEnvironmentVariable("Company") +"Job Ledger Entry] WHERE [From Location] = {0} AND [Posting Date] > {1} AND [Posting Date] < {2} and SUBSTRING([Job Task No_],1,5) = b.JobTaskNo and [Type of task]=3 and [Job No_] <> {0} )))  AS ThisPeriodQTY "
            //            + " , "

            //            + " (((CASE WHEN SUBSTRING(b.MainJobID, 1, 1) = 'N' or SUBSTRING(b.MainJobID,1,1)= 'O' THEN "
            //            + " (select isnull(sum([Original Total Cost]) + sum([Line Amount]),0) FROM"+ Environment.GetEnvironmentVariable("Company") +"Job Ledger Entry] WHERE [Job No_] = {0} AND [Posting Date] > {1} AND [Posting Date] < {2} and SUBSTRING([Job Task No_],1,5) = b.JobTaskNo and [Type of task]=0 )	"
            //            + " ELSE "
            //            + " (select isnull(sum([Original Total Cost]), 0) FROM"+ Environment.GetEnvironmentVariable("Company") +"Job Ledger Entry] WHERE[Job No_] = {0} AND [Posting Date] > {1} AND [Posting Date] < {2} and SUBSTRING([Job Task No_],1,5) = b.JobTaskNo and [Type of task]=0) END "
            //            + " )+"
            //            + " (CASE WHEN SUBSTRING(b.MainJobID, 1, 1) = 'N' or SUBSTRING(b.MainJobID,1,1)= 'O' THEN "
            //            + " (select isnull(sum([Original Total Cost]) + sum([Line Amount]),0) FROM"+ Environment.GetEnvironmentVariable("Company") +"Job Ledger Entry] WHERE[Job No_] = {0} AND [Posting Date] > {1} AND [Posting Date] < {2} and SUBSTRING([Job Task No_],1,5) = b.JobTaskNo and [Type of task]=3 )	"
            //            + " ELSE "
            //            + "	(select isnull(sum([Original Total Cost]), 0) FROM"+ Environment.GetEnvironmentVariable("Company") +"Job Ledger Entry] WHERE[Job No_] = {0} AND [Posting Date] > {1} AND [Posting Date] < {2} and SUBSTRING([Job Task No_],1,5) = b.JobTaskNo and [Type of task]=3) END "
            //            + " )+"
            //            + " (CASE WHEN SUBSTRING(b.MainJobID, 1, 1) = 'N' or SUBSTRING(b.MainJobID,1,1)= 'O' THEN "
            //            + " (select isnull(sum([Original Total Cost]) + sum([Line Amount]),0) FROM"+ Environment.GetEnvironmentVariable("Company") +"Job Ledger Entry] WHERE[Job No_] = {0} AND [Posting Date] > {1} AND [Posting Date] < {2} and SUBSTRING([Job Task No_],1,5) = b.JobTaskNo and [Type of task]=2)	"
            //            + " ELSE "
            //            + " (select isnull(sum([Original Total Cost]), 0) FROM"+ Environment.GetEnvironmentVariable("Company") +"Job Ledger Entry] WHERE[Job No_] = {0} AND [Posting Date] > {1} AND [Posting Date] < {2} and SUBSTRING([Job Task No_],1,5) = b.JobTaskNo and [Type of task]=2) END "
            //            + " )"
            //            + "+((select isnull(sum([Original Total Cost]), 0) FROM "+ Environment.GetEnvironmentVariable("Company") +"Job Ledger Entry] WHERE [From Location] = {0} AND [Posting Date] > {1} AND [Posting Date] < {2} and SUBSTRING([Job Task No_],1,5) = b.JobTaskNo and [Type of task]=1 and [Document No_] Like 'CAL%' )*2)) -"
            //            + " ((CASE WHEN SUBSTRING(b.MainJobID, 1, 1) = 'N' or SUBSTRING(b.MainJobID,1,1)= 'O' THEN "
            //            + " (select isnull(sum([Original Total Cost]) + sum([Line Amount]),0) FROM"+ Environment.GetEnvironmentVariable("Company") +"Job Ledger Entry] WHERE[Job No_] = {0} AND [Posting Date] > {1} AND [Posting Date] < {2} and SUBSTRING([Job Task No_],1,5) = b.JobTaskNo and [Type of task]=1)	"
            //            + " ELSE "
            //            + " (select isnull(sum([Original Total Cost]), 0) FROM"+ Environment.GetEnvironmentVariable("Company") +"Job Ledger Entry] WHERE[Job No_] = {0} AND [Posting Date] > {1} AND [Posting Date] < {2} and SUBSTRING([Job Task No_],1,5) = b.JobTaskNo and [Type of task]=1) END "
            //            + " )+ "
            //            + " (select isnull(sum([Original Total Cost]), 0) FROM "+ Environment.GetEnvironmentVariable("Company") +"Job Ledger Entry] WHERE [From Location] = {0} AND [Posting Date] > {1} AND [Posting Date] < {2} and SUBSTRING([Job Task No_],1,5) = b.JobTaskNo and [Type of task]=3 and [Job No_] <> {0} )))  AS ThisPeriodAMT "
            //            + " , "

            //            + " (((CASE WHEN SUBSTRING(b.MainJobID, 1, 1) = 'N' or SUBSTRING(b.MainJobID,1,1)= 'O' THEN "
            //            + " (select isnull(sum([Quantity]) + sum([Line Amount]),0) FROM"+ Environment.GetEnvironmentVariable("Company") +"Job Ledger Entry] WHERE [Job No_] = {0} AND [Posting Date] < {1} and SUBSTRING([Job Task No_],1,5) = b.JobTaskNo and [Type of task]=0 )	"
            //            + " ELSE "
            //            + " (select isnull(sum([Quantity]), 0) FROM"+ Environment.GetEnvironmentVariable("Company") +"Job Ledger Entry] WHERE[Job No_] = {0} AND [Posting Date] < {1} and SUBSTRING([Job Task No_],1,5) = b.JobTaskNo and [Type of task]=0) END "
            //            + " )+"
            //            + " (CASE WHEN SUBSTRING(b.MainJobID, 1, 1) = 'N' or SUBSTRING(b.MainJobID,1,1)= 'O' THEN "
            //            + " (select isnull(sum([Quantity]) + sum([Line Amount]),0) FROM"+ Environment.GetEnvironmentVariable("Company") +"Job Ledger Entry] WHERE[Job No_] = {0} AND [Posting Date] < {1} and SUBSTRING([Job Task No_],1,5) = b.JobTaskNo and [Type of task]=3 )	"
            //            + " ELSE "
            //            + "	(select isnull(sum([Quantity]), 0) FROM"+ Environment.GetEnvironmentVariable("Company") +"Job Ledger Entry] WHERE[Job No_] = {0} AND[Posting Date] < {1} and SUBSTRING([Job Task No_],1,5) = b.JobTaskNo and [Type of task]=3) END "
            //            + " )+"
            //            + " (CASE WHEN SUBSTRING(b.MainJobID, 1, 1) = 'N' or SUBSTRING(b.MainJobID,1,1)= 'O' THEN "
            //            + " (select isnull(sum([Quantity]) + sum([Line Amount]),0) FROM"+ Environment.GetEnvironmentVariable("Company") +"Job Ledger Entry] WHERE[Job No_] = {0} AND [Posting Date] < {1} and SUBSTRING([Job Task No_],1,5) = b.JobTaskNo and [Type of task]=2)	"
            //            + " ELSE "
            //            + " (select isnull(sum([Quantity]), 0) FROM"+ Environment.GetEnvironmentVariable("Company") +"Job Ledger Entry] WHERE[Job No_] = {0} AND [Posting Date] < {1} and SUBSTRING([Job Task No_],1,5) = b.JobTaskNo and [Type of task]=2) END "
            //            + " )"
            //            + "+((select isnull(sum([Quantity]), 0) FROM "+ Environment.GetEnvironmentVariable("Company") +"Job Ledger Entry] WHERE [From Location] = {0} AND [Posting Date] < {1}  and SUBSTRING([Job Task No_],1,5) = b.JobTaskNo and [Type of task]=1 and [Document No_] Like 'CAL%' )*2)) -"
            //            + " ((CASE WHEN SUBSTRING(b.MainJobID, 1, 1) = 'N' or SUBSTRING(b.MainJobID,1,1)= 'O' THEN "
            //            + " (select isnull(sum([Quantity]) + sum([Line Amount]),0) FROM"+ Environment.GetEnvironmentVariable("Company") +"Job Ledger Entry] WHERE[Job No_] = {0} AND [Posting Date] < {1} and SUBSTRING([Job Task No_],1,5) = b.JobTaskNo and [Type of task]=1)	"
            //            + " ELSE "
            //            + " (select isnull(sum([Quantity]), 0) FROM"+ Environment.GetEnvironmentVariable("Company") +"Job Ledger Entry] WHERE[Job No_] = {0} AND [Posting Date] < {1} and SUBSTRING([Job Task No_],1,5) = b.JobTaskNo and [Type of task]=1) END "
            //            + " )+ "
            //            + " (select isnull(sum([Quantity]), 0) FROM "+ Environment.GetEnvironmentVariable("Company") +"Job Ledger Entry] WHERE [From Location] = {0} AND [Posting Date] < {1} and SUBSTRING([Job Task No_],1,5) = b.JobTaskNo and [Type of task]=3 and [Job No_] <> {0} )))  AS OpeningQTY "
            //            + " , "

            //            + " (((CASE WHEN SUBSTRING(b.MainJobID, 1, 1) = 'N' or SUBSTRING(b.MainJobID,1,1)= 'O' THEN "
            //            + " (select isnull(sum([Original Total Cost]) + sum([Line Amount]),0) FROM"+ Environment.GetEnvironmentVariable("Company") +"Job Ledger Entry] WHERE [Job No_] = {0} AND  [Posting Date] < {1} and SUBSTRING([Job Task No_],1,5) = b.JobTaskNo and [Type of task]=0 )	"
            //            + " ELSE "
            //            + " (select isnull(sum([Original Total Cost]), 0) FROM"+ Environment.GetEnvironmentVariable("Company") +"Job Ledger Entry] WHERE[Job No_] = {0} AND [Posting Date] < {1} and SUBSTRING([Job Task No_],1,5) = b.JobTaskNo and [Type of task]=0) END "
            //            + " )+"
            //            + " (CASE WHEN SUBSTRING(b.MainJobID, 1, 1) = 'N' or SUBSTRING(b.MainJobID,1,1)= 'O' THEN "
            //            + " (select isnull(sum([Original Total Cost]) + sum([Line Amount]),0) FROM"+ Environment.GetEnvironmentVariable("Company") +"Job Ledger Entry] WHERE[Job No_] = {0} AND [Posting Date] < {1} and SUBSTRING([Job Task No_],1,5) = b.JobTaskNo and [Type of task]=3 )	"
            //            + " ELSE "
            //            + "	(select isnull(sum([Original Total Cost]), 0) FROM"+ Environment.GetEnvironmentVariable("Company") +"Job Ledger Entry] WHERE[Job No_] = {0} AND [Posting Date] < {1} and SUBSTRING([Job Task No_],1,5) = b.JobTaskNo and [Type of task]=3) END "
            //            + " )+"
            //            + " (CASE WHEN SUBSTRING(b.MainJobID, 1, 1) = 'N' or SUBSTRING(b.MainJobID,1,1)= 'O' THEN "
            //            + " (select isnull(sum([Original Total Cost]) + sum([Line Amount]),0) FROM"+ Environment.GetEnvironmentVariable("Company") +"Job Ledger Entry] WHERE[Job No_] = {0} AND [Posting Date] < {1} and SUBSTRING([Job Task No_],1,5) = b.JobTaskNo and [Type of task]=2)	"
            //            + " ELSE "
            //            + " (select isnull(sum([Original Total Cost]), 0) FROM"+ Environment.GetEnvironmentVariable("Company") +"Job Ledger Entry] WHERE[Job No_] = {0} AND [Posting Date] < {1} and SUBSTRING([Job Task No_],1,5) = b.JobTaskNo and [Type of task]=2) END "
            //            + " )"
            //            + " +((select isnull(sum([Original Total Cost]), 0) FROM "+ Environment.GetEnvironmentVariable("Company") +"Job Ledger Entry] WHERE [From Location] = {0} AND [Posting Date] < {1}  and SUBSTRING([Job Task No_],1,5) = b.JobTaskNo and [Type of task]=1 and [Document No_] Like 'CAL%' )*2)) -"
            //            + " ((CASE WHEN SUBSTRING(b.MainJobID, 1, 1) = 'N' or SUBSTRING(b.MainJobID,1,1)= 'O' THEN "
            //            + " (select isnull(sum([Original Total Cost]) + sum([Line Amount]),0) FROM"+ Environment.GetEnvironmentVariable("Company") +"Job Ledger Entry] WHERE[Job No_] = {0} AND [Posting Date] < {1} and SUBSTRING([Job Task No_],1,5) = b.JobTaskNo and [Type of task]=1)	"
            //            + " ELSE "
            //            + " (select isnull(sum([Original Total Cost]), 0) FROM"+ Environment.GetEnvironmentVariable("Company") +"Job Ledger Entry] WHERE[Job No_] = {0} AND [Posting Date] < {1} and SUBSTRING([Job Task No_],1,5) = b.JobTaskNo and [Type of task]=1) END "
            //            + " )+ "
            //            + " (select isnull(sum([Original Total Cost]), 0) FROM "+ Environment.GetEnvironmentVariable("Company") +"Job Ledger Entry] WHERE [From Location] = {0} AND [Posting Date] < {1} and SUBSTRING([Job Task No_],1,5) = b.JobTaskNo and [Type of task]=3 and [Job No_] <> {0} )))  AS OpeningAMT "
            //            + " , "

            //            + " (((CASE WHEN SUBSTRING(b.MainJobID, 1, 1) = 'N' or SUBSTRING(b.MainJobID,1,1)= 'O' THEN "
            //            + " (select isnull(sum([Quantity]) + sum([Line Amount]),0) FROM"+ Environment.GetEnvironmentVariable("Company") +"Job Ledger Entry] WHERE [Job No_] = {0} AND [Posting Date] < {2} and SUBSTRING([Job Task No_],1,5) = b.JobTaskNo and [Type of task]=0 )	"
            //            + " ELSE "
            //            + " (select isnull(sum([Quantity]), 0) FROM"+ Environment.GetEnvironmentVariable("Company") +"Job Ledger Entry] WHERE[Job No_] = {0} AND [Posting Date] < {2} and SUBSTRING([Job Task No_],1,5) = b.JobTaskNo and [Type of task]=0) END "
            //            + " )+"
            //            + " (CASE WHEN SUBSTRING(b.MainJobID, 1, 1) = 'N' or SUBSTRING(b.MainJobID,1,1)= 'O' THEN "
            //            + " (select isnull(sum([Quantity]) + sum([Line Amount]),0) FROM"+ Environment.GetEnvironmentVariable("Company") +"Job Ledger Entry] WHERE[Job No_] = {0} AND [Posting Date] < {2} and SUBSTRING([Job Task No_],1,5) = b.JobTaskNo and [Type of task]=3 )	"
            //            + " ELSE "
            //            + "	(select isnull(sum([Quantity]), 0) FROM"+ Environment.GetEnvironmentVariable("Company") +"Job Ledger Entry] WHERE[Job No_] = {0} AND[Posting Date] < {2} and SUBSTRING([Job Task No_],1,5) = b.JobTaskNo and [Type of task]=3) END "
            //            + " )+"
            //            + " (CASE WHEN SUBSTRING(b.MainJobID, 1, 1) = 'N' or SUBSTRING(b.MainJobID,1,1)= 'O' THEN "
            //            + " (select isnull(sum([Quantity]) + sum([Line Amount]),0) FROM"+ Environment.GetEnvironmentVariable("Company") +"Job Ledger Entry] WHERE[Job No_] = {0} AND [Posting Date] < {2} and SUBSTRING([Job Task No_],1,5) = b.JobTaskNo and [Type of task]=2)	"
            //            + " ELSE "
            //            + " (select isnull(sum([Quantity]), 0) FROM"+ Environment.GetEnvironmentVariable("Company") +"Job Ledger Entry] WHERE[Job No_] = {0} AND [Posting Date] < {2} and SUBSTRING([Job Task No_],1,5) = b.JobTaskNo and [Type of task]=2) END "
            //            + " )) -"
            //            + " ((CASE WHEN SUBSTRING(b.MainJobID, 1, 1) = 'N' or SUBSTRING(b.MainJobID,1,1)= 'O' THEN "
            //            + " (select isnull(sum([Quantity]) + sum([Line Amount]),0) FROM"+ Environment.GetEnvironmentVariable("Company") +"Job Ledger Entry] WHERE[Job No_] = {0} AND [Posting Date] < {2} and SUBSTRING([Job Task No_],1,5) = b.JobTaskNo and [Type of task]=1)	"
            //            + " ELSE "
            //            + " (select isnull(sum([Quantity]), 0) FROM"+ Environment.GetEnvironmentVariable("Company") +"Job Ledger Entry] WHERE[Job No_] = {0} AND [Posting Date] < {2} and SUBSTRING([Job Task No_],1,5) = b.JobTaskNo and [Type of task]=1) END "
            //            + " )+ "
            //            + " (select isnull(sum([Quantity]), 0) FROM "+ Environment.GetEnvironmentVariable("Company") +"Job Ledger Entry] WHERE [From Location] = {0} AND [Posting Date] < {2} and SUBSTRING([Job Task No_],1,5) = b.JobTaskNo and [Type of task]=3 and [Job No_] <> {0} )))  AS CumBalQTY "
            //            + " , "

            //              + " (((CASE WHEN SUBSTRING(b.MainJobID, 1, 1) = 'N' or SUBSTRING(b.MainJobID,1,1)= 'O' THEN "
            //              + " (select isnull(sum([Original Total Cost]) + sum([Line Amount]),0) FROM"+ Environment.GetEnvironmentVariable("Company") +"Job Ledger Entry] WHERE [Job No_] = {0} AND  [Posting Date] < {2} and SUBSTRING([Job Task No_],1,5) = b.JobTaskNo and [Type of task]=0 )	"
            //              + " ELSE "
            //              + " (select isnull(sum([Original Total Cost]), 0) FROM"+ Environment.GetEnvironmentVariable("Company") +"Job Ledger Entry] WHERE[Job No_] = {0} AND [Posting Date] < {2} and SUBSTRING([Job Task No_],1,5) = b.JobTaskNo and [Type of task]=0) END "
            //              + " )+"
            //              + " (CASE WHEN SUBSTRING(b.MainJobID, 1, 1) = 'N' or SUBSTRING(b.MainJobID,1,1)= 'O' THEN "
            //              + " (select isnull(sum([Original Total Cost]) + sum([Line Amount]),0) FROM"+ Environment.GetEnvironmentVariable("Company") +"Job Ledger Entry] WHERE[Job No_] = {0} AND [Posting Date] < {2} and SUBSTRING([Job Task No_],1,5) = b.JobTaskNo and [Type of task]=3 )	"
            //              + " ELSE "
            //              + "	(select isnull(sum([Original Total Cost]), 0) FROM"+ Environment.GetEnvironmentVariable("Company") +"Job Ledger Entry] WHERE[Job No_] = {0} AND [Posting Date] < {2} and SUBSTRING([Job Task No_],1,5) = b.JobTaskNo and [Type of task]=3) END "
            //              + " )+"
            //              + " (CASE WHEN SUBSTRING(b.MainJobID, 1, 1) = 'N' or SUBSTRING(b.MainJobID,1,1)= 'O' THEN "
            //              + " (select isnull(sum([Original Total Cost]) + sum([Line Amount]),0) FROM"+ Environment.GetEnvironmentVariable("Company") +"Job Ledger Entry] WHERE[Job No_] = {0} AND [Posting Date] < {2} and SUBSTRING([Job Task No_],1,5) = b.JobTaskNo and [Type of task]=2)	"
            //              + " ELSE "
            //              + " (select isnull(sum([Original Total Cost]), 0) FROM"+ Environment.GetEnvironmentVariable("Company") +"Job Ledger Entry] WHERE[Job No_] = {0} AND [Posting Date] < {2} and SUBSTRING([Job Task No_],1,5) = b.JobTaskNo and [Type of task]=2) END "
            //              + " )"
            //              + " +((select isnull(sum([Original Total Cost]), 0) FROM "+ Environment.GetEnvironmentVariable("Company") +"Job Ledger Entry] WHERE [From Location] = {0} AND [Posting Date] < {2}  and SUBSTRING([Job Task No_],1,5) = b.JobTaskNo and [Type of task]=1 and [Document No_] Like 'CAL%' )*2)) -"
            //              + " ((CASE WHEN SUBSTRING(b.MainJobID, 1, 1) = 'N' or SUBSTRING(b.MainJobID,1,1)= 'O' THEN "
            //              + " (select isnull(sum([Original Total Cost]) + sum([Line Amount]),0) FROM"+ Environment.GetEnvironmentVariable("Company") +"Job Ledger Entry] WHERE[Job No_] = {0} AND [Posting Date] < {2} and SUBSTRING([Job Task No_],1,5) = b.JobTaskNo and [Type of task]=1)	"
            //              + " ELSE "
            //              + " (select isnull(sum([Original Total Cost]), 0) FROM"+ Environment.GetEnvironmentVariable("Company") +"Job Ledger Entry] WHERE[Job No_] = {0} AND [Posting Date] < {2} and SUBSTRING([Job Task No_],1,5) = b.JobTaskNo and [Type of task]=1) END "
            //              + " )+ "
            //              + " (select isnull(sum([Original Total Cost]), 0) FROM "+ Environment.GetEnvironmentVariable("Company") +"Job Ledger Entry] WHERE [From Location] = {0} AND [Posting Date] < {2} and SUBSTRING([Job Task No_],1,5) = b.JobTaskNo and [Type of task]=3 and [Job No_] <> {0} )))  AS CumBalAMT "


            //            + " , "

            //            + " (SELECT isnull(sum(Quantity), 0) FROM dbo."+ Environment.GetEnvironmentVariable("Company") +"Job Planning Line] WHERE[Job No_]={0} and SUBSTRING([Job Task No_],1,5)=b.JobTaskNo) as BudgetQTY, "
            //            + " (SELECT isnull(sum([Total Cost (LCY)]),0) FROM dbo."+ Environment.GetEnvironmentVariable("Company") +"Job Planning Line] WHERE[Job No_]= {0} and SUBSTRING([Job Task No_],1,5)= b.JobTaskNo) as BudgetAMT "

            //            + " FROM("
            //             + " SELECT a.JobTaskNo,a.MainJobID,a.SubJobID,"
            //             + " (SELECT TOP 1 dbo."+ Environment.GetEnvironmentVariable("Company") +"Job Task].Description FROM dbo."+ Environment.GetEnvironmentVariable("Company") +"Job Task] WHERE [Job Task No_]=a.JobTaskNo) as Description,"
            //             + " (SELECT TOP 1 ([Job Task No_]+' '+Description) FROM dbo."+ Environment.GetEnvironmentVariable("Company") +"Job Task] WHERE [Job Task No_]=a.JobTaskNo) as DescriptionFull,"
            //             + " (SELECT TOP 1 dbo."+ Environment.GetEnvironmentVariable("Company") +"Job Task].Description FROM dbo."+ Environment.GetEnvironmentVariable("Company") +"Job Task] WHERE SUBSTRING(dbo."+ Environment.GetEnvironmentVariable("Company") +"Job Task].[Job Task No_],1,1)+'0000'=a.MainJobID) as DescriptionMainJob,"
            //             + " (SELECT TOP 1 dbo."+ Environment.GetEnvironmentVariable("Company") +"Job Task].Description FROM dbo."+ Environment.GetEnvironmentVariable("Company") +"Job Task] WHERE SUBSTRING(dbo."+ Environment.GetEnvironmentVariable("Company") +"Job Task].[Job Task No_],1,3)+'00'=a.SubJobID) as DescriptionSubJob"
            //             + " FROM  (SELECT DISTINCT dbo."+ Environment.GetEnvironmentVariable("Company") +"Job Task].[Job Task No_] as JobTaskNo,"
            //             + " SUBSTRING(dbo."+ Environment.GetEnvironmentVariable("Company") +"Job Task].[Job Task No_],1,1)+'0000' as MainJobID,SUBSTRING(dbo."+ Environment.GetEnvironmentVariable("Company") +"Job Task].[Job Task No_],1,3)+'00' as SubJobID FROM dbo."+ Environment.GetEnvironmentVariable("Company") +"Job Task]) as a) as b ORDER BY b.JobTaskNo";







            //var queryData = " SELECT b.MainJobID,b.DescriptionMainJob,b.SubJobID,b.DescriptionSubJob,b.JobTaskNo,b.DescriptionFull,"
            //                + " (((CASE WHEN SUBSTRING(b.MainJobID, 1, 1) = 'N' or SUBSTRING(b.MainJobID,1,1)= 'O' THEN "
            //                + " (select isnull(sum([Quantity]) + sum([Line Amount]),0) FROM"+ Environment.GetEnvironmentVariable("Company") +"Job Ledger Entry] WHERE [Job No_] = {0} AND [Posting Date] > {1} AND [Posting Date] < {2} and SUBSTRING([Job Task No_],1,5) = b.JobTaskNo and [Type of task]=0 )	"
            //                + " ELSE "
            //                + " (select isnull(sum([Quantity]), 0) FROM"+ Environment.GetEnvironmentVariable("Company") +"Job Ledger Entry] WHERE[Job No_] = {0} AND [Posting Date] > {1} AND [Posting Date] < {2} and SUBSTRING([Job Task No_],1,5) = b.JobTaskNo and [Type of task]=0) END "
            //                + " )+"
            //                + " (CASE WHEN SUBSTRING(b.MainJobID, 1, 1) = 'N' or SUBSTRING(b.MainJobID,1,1)= 'O' THEN "
            //                + " (select isnull(sum([Quantity]) + sum([Line Amount]),0) FROM"+ Environment.GetEnvironmentVariable("Company") +"Job Ledger Entry] WHERE[Job No_] = {0} AND [Posting Date] > {1} AND [Posting Date] < {2} and SUBSTRING([Job Task No_],1,5) = b.JobTaskNo and [Type of task]=3 )	"
            //                + " ELSE "
            //                + "	(select isnull(sum([Quantity]), 0) FROM"+ Environment.GetEnvironmentVariable("Company") +"Job Ledger Entry] WHERE[Job No_] = {0} AND [Posting Date] > {1} AND [Posting Date] < {2} and SUBSTRING([Job Task No_],1,5) = b.JobTaskNo and [Type of task]=3) END "
            //                + " )+"
            //                + " (CASE WHEN SUBSTRING(b.MainJobID, 1, 1) = 'N' or SUBSTRING(b.MainJobID,1,1)= 'O' THEN "
            //                + " (select isnull(sum([Quantity]) + sum([Line Amount]),0) FROM"+ Environment.GetEnvironmentVariable("Company") +"Job Ledger Entry] WHERE[Job No_] = {0} AND [Posting Date] > {1} AND [Posting Date] < {2} and SUBSTRING([Job Task No_],1,5) = b.JobTaskNo and [Type of task]=2)	"
            //                + " ELSE "
            //                + " (select isnull(sum([Quantity]), 0) FROM"+ Environment.GetEnvironmentVariable("Company") +"Job Ledger Entry] WHERE[Job No_] = {0} AND [Posting Date] > {1} AND [Posting Date] < {2} and SUBSTRING([Job Task No_],1,5) = b.JobTaskNo and [Type of task]=2) END "
            //                + " )" +
            //                "+((select isnull(sum([Quantity]), 0) FROM "+ Environment.GetEnvironmentVariable("Company") +"Job Ledger Entry] WHERE [From Location] = {0} AND [Posting Date] > {1} AND [Posting Date] < {2} and SUBSTRING([Job Task No_],1,5) = b.JobTaskNo and [Type of task]=1 and [Document No_] Like 'CAL%' )*2)) -"
            //                + " ((CASE WHEN SUBSTRING(b.MainJobID, 1, 1) = 'N' or SUBSTRING(b.MainJobID,1,1)= 'O' THEN "
            //                + " (select isnull(sum([Quantity]) + sum([Line Amount]),0) FROM"+ Environment.GetEnvironmentVariable("Company") +"Job Ledger Entry] WHERE[Job No_] = {0} AND [Posting Date] > {1} AND [Posting Date] < {2} and SUBSTRING([Job Task No_],1,5) = b.JobTaskNo and [Type of task]=1)	"
            //                + " ELSE "
            //                + " (select isnull(sum([Quantity]), 0) FROM"+ Environment.GetEnvironmentVariable("Company") +"Job Ledger Entry] WHERE[Job No_] = {0} AND [Posting Date] > {1} AND [Posting Date] < {2} and SUBSTRING([Job Task No_],1,5) = b.JobTaskNo and [Type of task]=1) END "
            //                + " )+ "
            //                + " (select isnull(sum([Quantity]), 0) FROM "+ Environment.GetEnvironmentVariable("Company") +"Job Ledger Entry] WHERE [From Location] = {0} AND [Posting Date] > {1} AND [Posting Date] < {2} and SUBSTRING([Job Task No_],1,5) = b.JobTaskNo and [Type of task]=3 and [Job No_] <> {0} )))  AS ThisPeriodQTY "
            //                + " , "

            //                + " (((CASE WHEN SUBSTRING(b.MainJobID, 1, 1) = 'N' or SUBSTRING(b.MainJobID,1,1)= 'O' THEN "
            //                + " (select isnull(sum([Total Cost]) + sum([Line Amount]),0) FROM"+ Environment.GetEnvironmentVariable("Company") +"Job Ledger Entry] WHERE [Job No_] = {0} AND [Posting Date] > {1} AND [Posting Date] < {2} and SUBSTRING([Job Task No_],1,5) = b.JobTaskNo and [Type of task]=0 )	"
            //                + " ELSE "
            //                + " (select isnull(sum([Total Cost]), 0) FROM"+ Environment.GetEnvironmentVariable("Company") +"Job Ledger Entry] WHERE[Job No_] = {0} AND [Posting Date] > {1} AND [Posting Date] < {2} and SUBSTRING([Job Task No_],1,5) = b.JobTaskNo and [Type of task]=0) END "
            //                + " )+"
            //                + " (CASE WHEN SUBSTRING(b.MainJobID, 1, 1) = 'N' or SUBSTRING(b.MainJobID,1,1)= 'O' THEN "
            //                + " (select isnull(sum([Total Cost]) + sum([Line Amount]),0) FROM"+ Environment.GetEnvironmentVariable("Company") +"Job Ledger Entry] WHERE[Job No_] = {0} AND [Posting Date] > {1} AND [Posting Date] < {2} and SUBSTRING([Job Task No_],1,5) = b.JobTaskNo and [Type of task]=3 )	"
            //                + " ELSE "
            //                + "	(select isnull(sum([Total Cost]), 0) FROM"+ Environment.GetEnvironmentVariable("Company") +"Job Ledger Entry] WHERE[Job No_] = {0} AND [Posting Date] > {1} AND [Posting Date] < {2} and SUBSTRING([Job Task No_],1,5) = b.JobTaskNo and [Type of task]=3) END "
            //                + " )+"
            //                + " (CASE WHEN SUBSTRING(b.MainJobID, 1, 1) = 'N' or SUBSTRING(b.MainJobID,1,1)= 'O' THEN "
            //                + " (select isnull(sum([Total Cost]) + sum([Line Amount]),0) FROM"+ Environment.GetEnvironmentVariable("Company") +"Job Ledger Entry] WHERE[Job No_] = {0} AND [Posting Date] > {1} AND [Posting Date] < {2} and SUBSTRING([Job Task No_],1,5) = b.JobTaskNo and [Type of task]=2)	"
            //                + " ELSE "
            //                + " (select isnull(sum([Total Cost]), 0) FROM"+ Environment.GetEnvironmentVariable("Company") +"Job Ledger Entry] WHERE[Job No_] = {0} AND [Posting Date] > {1} AND [Posting Date] < {2} and SUBSTRING([Job Task No_],1,5) = b.JobTaskNo and [Type of task]=2) END "
            //                + " )"
            //                + "+((select isnull(sum([Total Cost]), 0) FROM "+ Environment.GetEnvironmentVariable("Company") +"Job Ledger Entry] WHERE [From Location] = {0} AND [Posting Date] > {1} AND [Posting Date] < {2} and SUBSTRING([Job Task No_],1,5) = b.JobTaskNo and [Type of task]=1 and [Document No_] Like 'CAL%' )*2)) -"
            //                + " ((CASE WHEN SUBSTRING(b.MainJobID, 1, 1) = 'N' or SUBSTRING(b.MainJobID,1,1)= 'O' THEN "
            //                + " (select isnull(sum([Total Cost]) + sum([Line Amount]),0) FROM"+ Environment.GetEnvironmentVariable("Company") +"Job Ledger Entry] WHERE[Job No_] = {0} AND [Posting Date] > {1} AND [Posting Date] < {2} and SUBSTRING([Job Task No_],1,5) = b.JobTaskNo and [Type of task]=1)	"
            //                + " ELSE "
            //                + " (select isnull(sum([Total Cost]), 0) FROM"+ Environment.GetEnvironmentVariable("Company") +"Job Ledger Entry] WHERE[Job No_] = {0} AND [Posting Date] > {1} AND [Posting Date] < {2} and SUBSTRING([Job Task No_],1,5) = b.JobTaskNo and [Type of task]=1) END "
            //                + " )+ "
            //                + " (select isnull(sum([Total Cost]), 0) FROM "+ Environment.GetEnvironmentVariable("Company") +"Job Ledger Entry] WHERE [From Location] = {0} AND [Posting Date] > {1} AND [Posting Date] < {2} and SUBSTRING([Job Task No_],1,5) = b.JobTaskNo and [Type of task]=3 and [Job No_] <> {0} )))  AS ThisPeriodAMT "
            //                + " , "

            //                + " (((CASE WHEN SUBSTRING(b.MainJobID, 1, 1) = 'N' or SUBSTRING(b.MainJobID,1,1)= 'O' THEN "
            //                + " (select isnull(sum([Quantity]) + sum([Line Amount]),0) FROM"+ Environment.GetEnvironmentVariable("Company") +"Job Ledger Entry] WHERE [Job No_] = {0} AND [Posting Date] < {1} and SUBSTRING([Job Task No_],1,5) = b.JobTaskNo and [Type of task]=0 )	"
            //                + " ELSE "
            //                + " (select isnull(sum([Quantity]), 0) FROM"+ Environment.GetEnvironmentVariable("Company") +"Job Ledger Entry] WHERE[Job No_] = {0} AND [Posting Date] < {1} and SUBSTRING([Job Task No_],1,5) = b.JobTaskNo and [Type of task]=0) END "
            //                + " )+"
            //                + " (CASE WHEN SUBSTRING(b.MainJobID, 1, 1) = 'N' or SUBSTRING(b.MainJobID,1,1)= 'O' THEN "
            //                + " (select isnull(sum([Quantity]) + sum([Line Amount]),0) FROM"+ Environment.GetEnvironmentVariable("Company") +"Job Ledger Entry] WHERE[Job No_] = {0} AND [Posting Date] < {1} and SUBSTRING([Job Task No_],1,5) = b.JobTaskNo and [Type of task]=3 )	"
            //                + " ELSE "
            //                + "	(select isnull(sum([Quantity]), 0) FROM"+ Environment.GetEnvironmentVariable("Company") +"Job Ledger Entry] WHERE[Job No_] = {0} AND[Posting Date] < {1} and SUBSTRING([Job Task No_],1,5) = b.JobTaskNo and [Type of task]=3) END "
            //                + " )+"
            //                + " (CASE WHEN SUBSTRING(b.MainJobID, 1, 1) = 'N' or SUBSTRING(b.MainJobID,1,1)= 'O' THEN "
            //                + " (select isnull(sum([Quantity]) + sum([Line Amount]),0) FROM"+ Environment.GetEnvironmentVariable("Company") +"Job Ledger Entry] WHERE[Job No_] = {0} AND [Posting Date] < {1} and SUBSTRING([Job Task No_],1,5) = b.JobTaskNo and [Type of task]=2)	"
            //                + " ELSE "
            //                + " (select isnull(sum([Quantity]), 0) FROM"+ Environment.GetEnvironmentVariable("Company") +"Job Ledger Entry] WHERE[Job No_] = {0} AND [Posting Date] < {1} and SUBSTRING([Job Task No_],1,5) = b.JobTaskNo and [Type of task]=2) END "
            //                + " )"
            //                + "+((select isnull(sum([Quantity]), 0) FROM "+ Environment.GetEnvironmentVariable("Company") +"Job Ledger Entry] WHERE [From Location] = {0} AND [Posting Date] < {1}  and SUBSTRING([Job Task No_],1,5) = b.JobTaskNo and [Type of task]=1 and [Document No_] Like 'CAL%' )*2)) -"
            //                + " ((CASE WHEN SUBSTRING(b.MainJobID, 1, 1) = 'N' or SUBSTRING(b.MainJobID,1,1)= 'O' THEN "
            //                + " (select isnull(sum([Quantity]) + sum([Line Amount]),0) FROM"+ Environment.GetEnvironmentVariable("Company") +"Job Ledger Entry] WHERE[Job No_] = {0} AND [Posting Date] < {1} and SUBSTRING([Job Task No_],1,5) = b.JobTaskNo and [Type of task]=1)	"
            //                + " ELSE "
            //                + " (select isnull(sum([Quantity]), 0) FROM"+ Environment.GetEnvironmentVariable("Company") +"Job Ledger Entry] WHERE[Job No_] = {0} AND [Posting Date] < {1} and SUBSTRING([Job Task No_],1,5) = b.JobTaskNo and [Type of task]=1) END "
            //                + " )+ "
            //                + " (select isnull(sum([Quantity]), 0) FROM "+ Environment.GetEnvironmentVariable("Company") +"Job Ledger Entry] WHERE [From Location] = {0} AND [Posting Date] < {1} and SUBSTRING([Job Task No_],1,5) = b.JobTaskNo and [Type of task]=3 and [Job No_] <> {0} )))  AS OpeningQTY "
            //                + " , "

            //                + " (((CASE WHEN SUBSTRING(b.MainJobID, 1, 1) = 'N' or SUBSTRING(b.MainJobID,1,1)= 'O' THEN "
            //                + " (select isnull(sum([Total Cost]) + sum([Line Amount]),0) FROM"+ Environment.GetEnvironmentVariable("Company") +"Job Ledger Entry] WHERE [Job No_] = {0} AND  [Posting Date] < {1} and SUBSTRING([Job Task No_],1,5) = b.JobTaskNo and [Type of task]=0 )	"
            //                + " ELSE "
            //                + " (select isnull(sum([Total Cost]), 0) FROM"+ Environment.GetEnvironmentVariable("Company") +"Job Ledger Entry] WHERE[Job No_] = {0} AND [Posting Date] < {1} and SUBSTRING([Job Task No_],1,5) = b.JobTaskNo and [Type of task]=0) END "
            //                + " )+"
            //                + " (CASE WHEN SUBSTRING(b.MainJobID, 1, 1) = 'N' or SUBSTRING(b.MainJobID,1,1)= 'O' THEN "
            //                + " (select isnull(sum([Total Cost]) + sum([Line Amount]),0) FROM"+ Environment.GetEnvironmentVariable("Company") +"Job Ledger Entry] WHERE[Job No_] = {0} AND [Posting Date] < {1} and SUBSTRING([Job Task No_],1,5) = b.JobTaskNo and [Type of task]=3 )	"
            //                + " ELSE "
            //                + "	(select isnull(sum([Total Cost]), 0) FROM"+ Environment.GetEnvironmentVariable("Company") +"Job Ledger Entry] WHERE[Job No_] = {0} AND [Posting Date] < {1} and SUBSTRING([Job Task No_],1,5) = b.JobTaskNo and [Type of task]=3) END "
            //                + " )+"
            //                + " (CASE WHEN SUBSTRING(b.MainJobID, 1, 1) = 'N' or SUBSTRING(b.MainJobID,1,1)= 'O' THEN "
            //                + " (select isnull(sum([Total Cost]) + sum([Line Amount]),0) FROM"+ Environment.GetEnvironmentVariable("Company") +"Job Ledger Entry] WHERE[Job No_] = {0} AND [Posting Date] < {1} and SUBSTRING([Job Task No_],1,5) = b.JobTaskNo and [Type of task]=2)	"
            //                + " ELSE "
            //                + " (select isnull(sum([Total Cost]), 0) FROM"+ Environment.GetEnvironmentVariable("Company") +"Job Ledger Entry] WHERE[Job No_] = {0} AND [Posting Date] < {1} and SUBSTRING([Job Task No_],1,5) = b.JobTaskNo and [Type of task]=2) END "
            //                + " )"
            //                + " +((select isnull(sum([Total Cost]), 0) FROM "+ Environment.GetEnvironmentVariable("Company") +"Job Ledger Entry] WHERE [From Location] = {0} AND [Posting Date] < {1}  and SUBSTRING([Job Task No_],1,5) = b.JobTaskNo and [Type of task]=1 and [Document No_] Like 'CAL%' )*2)) -"
            //                + " ((CASE WHEN SUBSTRING(b.MainJobID, 1, 1) = 'N' or SUBSTRING(b.MainJobID,1,1)= 'O' THEN "
            //                + " (select isnull(sum([Total Cost]) + sum([Line Amount]),0) FROM"+ Environment.GetEnvironmentVariable("Company") +"Job Ledger Entry] WHERE[Job No_] = {0} AND [Posting Date] < {1} and SUBSTRING([Job Task No_],1,5) = b.JobTaskNo and [Type of task]=1)	"
            //                + " ELSE "
            //                + " (select isnull(sum([Total Cost]), 0) FROM"+ Environment.GetEnvironmentVariable("Company") +"Job Ledger Entry] WHERE[Job No_] = {0} AND [Posting Date] < {1} and SUBSTRING([Job Task No_],1,5) = b.JobTaskNo and [Type of task]=1) END "
            //                + " )+ "
            //                + " (select isnull(sum([Total Cost]), 0) FROM "+ Environment.GetEnvironmentVariable("Company") +"Job Ledger Entry] WHERE [From Location] = {0} AND [Posting Date] < {1} and SUBSTRING([Job Task No_],1,5) = b.JobTaskNo and [Type of task]=3 and [Job No_] <> {0} )))  AS OpeningAMT "
            //                + " , "

            //                + " (((CASE WHEN SUBSTRING(b.MainJobID, 1, 1) = 'N' or SUBSTRING(b.MainJobID,1,1)= 'O' THEN "
            //                + " (select isnull(sum([Quantity]) + sum([Line Amount]),0) FROM"+ Environment.GetEnvironmentVariable("Company") +"Job Ledger Entry] WHERE [Job No_] = {0} AND [Posting Date] < {2} and SUBSTRING([Job Task No_],1,5) = b.JobTaskNo and [Type of task]=0 )	"
            //                + " ELSE "
            //                + " (select isnull(sum([Quantity]), 0) FROM"+ Environment.GetEnvironmentVariable("Company") +"Job Ledger Entry] WHERE[Job No_] = {0} AND [Posting Date] < {2} and SUBSTRING([Job Task No_],1,5) = b.JobTaskNo and [Type of task]=0) END "
            //                + " )+"
            //                + " (CASE WHEN SUBSTRING(b.MainJobID, 1, 1) = 'N' or SUBSTRING(b.MainJobID,1,1)= 'O' THEN "
            //                + " (select isnull(sum([Quantity]) + sum([Line Amount]),0) FROM"+ Environment.GetEnvironmentVariable("Company") +"Job Ledger Entry] WHERE[Job No_] = {0} AND [Posting Date] < {2} and SUBSTRING([Job Task No_],1,5) = b.JobTaskNo and [Type of task]=3 )	"
            //                + " ELSE "
            //                + "	(select isnull(sum([Quantity]), 0) FROM"+ Environment.GetEnvironmentVariable("Company") +"Job Ledger Entry] WHERE[Job No_] = {0} AND[Posting Date] < {2} and SUBSTRING([Job Task No_],1,5) = b.JobTaskNo and [Type of task]=3) END "
            //                + " )+"
            //                + " (CASE WHEN SUBSTRING(b.MainJobID, 1, 1) = 'N' or SUBSTRING(b.MainJobID,1,1)= 'O' THEN "
            //                + " (select isnull(sum([Quantity]) + sum([Line Amount]),0) FROM"+ Environment.GetEnvironmentVariable("Company") +"Job Ledger Entry] WHERE[Job No_] = {0} AND [Posting Date] < {2} and SUBSTRING([Job Task No_],1,5) = b.JobTaskNo and [Type of task]=2)	"
            //                + " ELSE "
            //                + " (select isnull(sum([Quantity]), 0) FROM"+ Environment.GetEnvironmentVariable("Company") +"Job Ledger Entry] WHERE[Job No_] = {0} AND [Posting Date] < {2} and SUBSTRING([Job Task No_],1,5) = b.JobTaskNo and [Type of task]=2) END "
            //                + " )) -"
            //                + " ((CASE WHEN SUBSTRING(b.MainJobID, 1, 1) = 'N' or SUBSTRING(b.MainJobID,1,1)= 'O' THEN "
            //                + " (select isnull(sum([Quantity]) + sum([Line Amount]),0) FROM"+ Environment.GetEnvironmentVariable("Company") +"Job Ledger Entry] WHERE[Job No_] = {0} AND [Posting Date] < {2} and SUBSTRING([Job Task No_],1,5) = b.JobTaskNo and [Type of task]=1)	"
            //                + " ELSE "
            //                + " (select isnull(sum([Quantity]), 0) FROM"+ Environment.GetEnvironmentVariable("Company") +"Job Ledger Entry] WHERE[Job No_] = {0} AND [Posting Date] < {2} and SUBSTRING([Job Task No_],1,5) = b.JobTaskNo and [Type of task]=1) END "
            //                + " )+ "
            //                + " (select isnull(sum([Quantity]), 0) FROM "+ Environment.GetEnvironmentVariable("Company") +"Job Ledger Entry] WHERE [From Location] = {0} AND [Posting Date] < {2} and SUBSTRING([Job Task No_],1,5) = b.JobTaskNo and [Type of task]=3 and [Job No_] <> {0} )))  AS CumBalQTY "
            //                + " , "

            //                  + " (((CASE WHEN SUBSTRING(b.MainJobID, 1, 1) = 'N' or SUBSTRING(b.MainJobID,1,1)= 'O' THEN "
            //                  + " (select isnull(sum([Total Cost]) + sum([Line Amount]),0) FROM"+ Environment.GetEnvironmentVariable("Company") +"Job Ledger Entry] WHERE [Job No_] = {0} AND  [Posting Date] < {2} and SUBSTRING([Job Task No_],1,5) = b.JobTaskNo and [Type of task]=0 )	"
            //                  + " ELSE "
            //                  + " (select isnull(sum([Total Cost]), 0) FROM"+ Environment.GetEnvironmentVariable("Company") +"Job Ledger Entry] WHERE[Job No_] = {0} AND [Posting Date] < {2} and SUBSTRING([Job Task No_],1,5) = b.JobTaskNo and [Type of task]=0) END "
            //                  + " )+"
            //                  + " (CASE WHEN SUBSTRING(b.MainJobID, 1, 1) = 'N' or SUBSTRING(b.MainJobID,1,1)= 'O' THEN "
            //                  + " (select isnull(sum([Total Cost]) + sum([Line Amount]),0) FROM"+ Environment.GetEnvironmentVariable("Company") +"Job Ledger Entry] WHERE[Job No_] = {0} AND [Posting Date] < {2} and SUBSTRING([Job Task No_],1,5) = b.JobTaskNo and [Type of task]=3 )	"
            //                  + " ELSE "
            //                  + "	(select isnull(sum([Total Cost]), 0) FROM"+ Environment.GetEnvironmentVariable("Company") +"Job Ledger Entry] WHERE[Job No_] = {0} AND [Posting Date] < {2} and SUBSTRING([Job Task No_],1,5) = b.JobTaskNo and [Type of task]=3) END "
            //                  + " )+"
            //                  + " (CASE WHEN SUBSTRING(b.MainJobID, 1, 1) = 'N' or SUBSTRING(b.MainJobID,1,1)= 'O' THEN "
            //                  + " (select isnull(sum([Total Cost]) + sum([Line Amount]),0) FROM"+ Environment.GetEnvironmentVariable("Company") +"Job Ledger Entry] WHERE[Job No_] = {0} AND [Posting Date] < {2} and SUBSTRING([Job Task No_],1,5) = b.JobTaskNo and [Type of task]=2)	"
            //                  + " ELSE "
            //                  + " (select isnull(sum([Total Cost]), 0) FROM"+ Environment.GetEnvironmentVariable("Company") +"Job Ledger Entry] WHERE[Job No_] = {0} AND [Posting Date] < {2} and SUBSTRING([Job Task No_],1,5) = b.JobTaskNo and [Type of task]=2) END "
            //                  + " )"
            //                  + " +((select isnull(sum([Total Cost]), 0) FROM "+ Environment.GetEnvironmentVariable("Company") +"Job Ledger Entry] WHERE [From Location] = {0} AND [Posting Date] < {2}  and SUBSTRING([Job Task No_],1,5) = b.JobTaskNo and [Type of task]=1 and [Document No_] Like 'CAL%' )*2)) -"
            //                  + " ((CASE WHEN SUBSTRING(b.MainJobID, 1, 1) = 'N' or SUBSTRING(b.MainJobID,1,1)= 'O' THEN "
            //                  + " (select isnull(sum([Total Cost]) + sum([Line Amount]),0) FROM"+ Environment.GetEnvironmentVariable("Company") +"Job Ledger Entry] WHERE[Job No_] = {0} AND [Posting Date] < {2} and SUBSTRING([Job Task No_],1,5) = b.JobTaskNo and [Type of task]=1)	"
            //                  + " ELSE "
            //                  + " (select isnull(sum([Total Cost]), 0) FROM"+ Environment.GetEnvironmentVariable("Company") +"Job Ledger Entry] WHERE[Job No_] = {0} AND [Posting Date] < {2} and SUBSTRING([Job Task No_],1,5) = b.JobTaskNo and [Type of task]=1) END "
            //                  + " )+ "
            //                  + " (select isnull(sum([Total Cost]), 0) FROM "+ Environment.GetEnvironmentVariable("Company") +"Job Ledger Entry] WHERE [From Location] = {0} AND [Posting Date] < {2} and SUBSTRING([Job Task No_],1,5) = b.JobTaskNo and [Type of task]=3 and [Job No_] <> {0} )))  AS CumBalAMT "

            //                  /////กรณีนำ GL
            //                  //+ " (SELECT isnull(SUM([Amount]),0) as Total FROM dbo."+ Environment.GetEnvironmentVariable("Company") +"Analysis View Entry] WHERE [Analysis View Code]='JOB COST A' and Convert(Time,[Posting Date])<>'23:59:59'  and [Posting Date] <={2} and [Dimension 1 Value Code]={0} and [Dimension 2 Value Code]<>'' and [Dimension 2 Value Code]=b.JobTaskNo) as CumBalAMT"
            //                  //  ///


            //                + " , "

            //                + " (SELECT isnull(sum(Quantity), 0) FROM dbo."+ Environment.GetEnvironmentVariable("Company") +"Job Planning Line] WHERE[Job No_]={0} and SUBSTRING([Job Task No_],1,5)=b.JobTaskNo) as BudgetQTY, "
            //                + " (SELECT isnull(sum([Total Cost (LCY)]),0) FROM dbo."+ Environment.GetEnvironmentVariable("Company") +"Job Planning Line] WHERE[Job No_]= {0} and SUBSTRING([Job Task No_],1,5)= b.JobTaskNo) as BudgetAMT "

            //                + " FROM("
            //                 + " SELECT a.JobTaskNo,a.MainJobID,a.SubJobID,"
            //                 + " (SELECT TOP 1 dbo."+ Environment.GetEnvironmentVariable("Company") +"Job Task].Description FROM dbo."+ Environment.GetEnvironmentVariable("Company") +"Job Task] WHERE [Job Task No_]=a.JobTaskNo) as Description,"
            //                 + " (SELECT TOP 1 ([Job Task No_]+' '+Description) FROM dbo."+ Environment.GetEnvironmentVariable("Company") +"Job Task] WHERE [Job Task No_]=a.JobTaskNo) as DescriptionFull,"
            //                 + " (SELECT TOP 1 dbo."+ Environment.GetEnvironmentVariable("Company") +"Job Task].Description FROM dbo."+ Environment.GetEnvironmentVariable("Company") +"Job Task] WHERE SUBSTRING(dbo."+ Environment.GetEnvironmentVariable("Company") +"Job Task].[Job Task No_],1,1)+'0000'=a.MainJobID) as DescriptionMainJob,"
            //                 + " (SELECT TOP 1 dbo."+ Environment.GetEnvironmentVariable("Company") +"Job Task].Description FROM dbo."+ Environment.GetEnvironmentVariable("Company") +"Job Task] WHERE SUBSTRING(dbo."+ Environment.GetEnvironmentVariable("Company") +"Job Task].[Job Task No_],1,3)+'00'=a.SubJobID) as DescriptionSubJob"
            //                 + " FROM  (SELECT DISTINCT dbo."+ Environment.GetEnvironmentVariable("Company") +"Job Task].[Job Task No_] as JobTaskNo,"
            //                 + " SUBSTRING(dbo."+ Environment.GetEnvironmentVariable("Company") +"Job Task].[Job Task No_],1,1)+'0000' as MainJobID,SUBSTRING(dbo."+ Environment.GetEnvironmentVariable("Company") +"Job Task].[Job Task No_],1,3)+'00' as SubJobID FROM dbo."+ Environment.GetEnvironmentVariable("Company") +"Job Task]) as a) as b ORDER BY b.JobTaskNo";



            //SqlParameter parameterJob = new SqlParameter("{0}", JobNo);
            //SqlParameter parameterStart = new SqlParameter("{1}", date1);
            //SqlParameter parameterDate1 = new SqlParameter("{2}", date2);




            //ViewBag.sql = queryData;
            var v_ReportJobCost = _navcontext.V_ReportJobCost.FromSqlRaw(queryData, JobNo, date1, date2).ToList();


            var r = 3;

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




            int check = 0;
            int check1 = 0;



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
            int rl = 6;




            foreach (var std in v_ReportJobCost as IList<V_ReportJobCost>)
            {



                //Main
                if (mainID != std.MainJobID)
                {
                    mainID = std.MainJobID;
                    check = 0;
                }
                else
                {
                    check = 1;
                }


                if (check == 0)
                {
                    sumThisPeriodQTY = v_ReportJobCost.Where(c => c.MainJobID == std.MainJobID).Sum(c => c.ThisPeriodQTY);
                    sumThisPeriodAMT = v_ReportJobCost.Where(c => c.MainJobID == std.MainJobID).Sum(c => c.ThisperiodAMT);
                    sumOpeningQTY = v_ReportJobCost.Where(c => c.MainJobID == std.MainJobID).Sum(c => c.OpeningQTY);
                    sumOpeningAMT = v_ReportJobCost.Where(c => c.MainJobID == std.MainJobID).Sum(c => c.OpeningAMT);
                    sumCumBalQTY = v_ReportJobCost.Where(c => c.MainJobID == std.MainJobID).Sum(c => c.CumBalQTY);
                    sumCumBalAMT = v_ReportJobCost.Where(c => c.MainJobID == std.MainJobID).Sum(c => c.CumBalAMT);
                    sumBudgetQTY = v_ReportJobCost.Where(c => c.MainJobID == std.MainJobID).Sum(c => c.BudgetQTY);
                    sumBudgetAMT = v_ReportJobCost.Where(c => c.MainJobID == std.MainJobID).Sum(c => c.BudgetAMT);
                    if (std.MainJobID == "A0000")
                    {
                        A00 = sumCumBalAMT;
                        TCXI += sumCumBalAMT;

                    }
                    if (std.MainJobID == "B0000")
                    {
                        B00 = sumCumBalAMT;
                        TCXI += sumCumBalAMT;
                    }
                    if (std.MainJobID == "C0000")
                    {
                        C00 = sumCumBalAMT;
                        TCXI += sumCumBalAMT;
                    }
                    if (std.MainJobID == "D0000")
                    {
                        D00 = sumCumBalAMT;
                        TCXI += sumCumBalAMT;
                    }
                    if (std.MainJobID == "E0000")
                    {
                        E00 = sumCumBalAMT;
                        TCXI += sumCumBalAMT;
                    }
                    if (std.MainJobID == "F0000")
                    {
                        F00 = sumCumBalAMT;
                        TCXI += sumCumBalAMT;
                    }

                    if (std.MainJobID == "G0000")
                    {
                        G00 = sumCumBalAMT;
                        TCXI += sumCumBalAMT;
                    }
                    if (std.MainJobID == "H0000")
                    {
                        H00 = sumCumBalAMT;
                        TCXI += sumCumBalAMT;
                    }






                    if (std.MainJobID == "O0000")
                    {

                        worksheet3.Cells[4, 1].Value = std.MainJobID + " " + std.DescriptionMainJob;
                        worksheet3.Cells[4, 2].Value = sumThisPeriodQTY;
                        worksheet3.Cells[4, 3].Value = sumThisPeriodAMT;
                        worksheet3.Cells[4, 4].Value = sumOpeningQTY;
                        worksheet3.Cells[4, 5].Value = sumOpeningAMT;
                        worksheet3.Cells[4, 6].Value = sumCumBalQTY;
                        worksheet3.Cells[4, 7].Value = sumCumBalAMT;
                        worksheet3.Cells[4, 8].Value = sumBudgetQTY;
                        worksheet3.Cells[4, 9].Value = Percen(sumCumBalQTY, sumBudgetQTY) + "%";
                        worksheet3.Cells[4, 10].Value = sumBudgetAMT;
                        worksheet3.Cells[4, 11].Value = Percen(sumCumBalAMT, sumBudgetAMT) + "%";




                        OthisQ += sumThisPeriodQTY;
                        OthisA += sumThisPeriodAMT;
                        OopenQ += sumOpeningQTY;
                        OopenA += sumOpeningAMT;
                        OcumQ += sumCumBalQTY;
                        OcumA += sumCumBalAMT;
                        ObudQ += sumBudgetQTY;
                        ObudA += sumBudgetAMT;

                    }
                    else if (std.MainJobID == "N0000")
                    {

                        worksheet3.Cells[3, 1].Value = std.MainJobID + " " + std.DescriptionMainJob;
                        worksheet3.Cells[3, 2].Value = sumThisPeriodQTY;
                        worksheet3.Cells[3, 3].Value = sumThisPeriodAMT;
                        worksheet3.Cells[3, 4].Value = sumOpeningQTY;
                        worksheet3.Cells[3, 5].Value = sumOpeningAMT;
                        worksheet3.Cells[3, 6].Value = sumCumBalQTY;
                        worksheet3.Cells[3, 7].Value = sumCumBalAMT;
                        worksheet3.Cells[3, 8].Value = sumBudgetQTY;
                        worksheet3.Cells[3, 9].Value = Percen(sumCumBalQTY, sumBudgetQTY) + "%";
                        worksheet3.Cells[3, 10].Value = sumBudgetAMT;
                        worksheet3.Cells[3, 11].Value = Percen(sumCumBalAMT, sumBudgetAMT) + "%";




                        OthisQ += sumThisPeriodQTY;
                        OthisA += sumThisPeriodAMT;
                        OopenQ += sumOpeningQTY;
                        OopenA += sumOpeningAMT;
                        OcumQ += sumCumBalQTY;
                        OcumA += sumCumBalAMT;
                        ObudQ += sumBudgetQTY;
                        ObudA += sumBudgetAMT;

                    }
                    else
                    {
                        worksheet3.Cells[rl, 1].Value = std.MainJobID + " " + std.DescriptionMainJob;
                        worksheet3.Cells[rl, 2].Value = sumThisPeriodQTY;
                        worksheet3.Cells[rl, 3].Value = sumThisPeriodAMT;
                        worksheet3.Cells[rl, 4].Value = sumOpeningQTY;
                        worksheet3.Cells[rl, 5].Value = sumOpeningAMT;
                        worksheet3.Cells[rl, 6].Value = sumCumBalQTY;
                        worksheet3.Cells[rl, 7].Value = sumCumBalAMT;
                        worksheet3.Cells[rl, 8].Value = sumBudgetQTY;
                        worksheet3.Cells[rl, 9].Value = Percen(sumCumBalQTY, sumBudgetQTY) + "%";
                        worksheet3.Cells[rl, 10].Value = sumBudgetAMT;
                        worksheet3.Cells[rl, 11].Value = Percen(sumCumBalAMT, sumBudgetAMT) + "%";



                        thisQ += sumThisPeriodQTY;
                        thisA += sumThisPeriodAMT;
                        openQ += sumOpeningQTY;
                        openA += sumOpeningAMT;
                        cumQ += sumCumBalQTY;
                        cumA += sumCumBalAMT;
                        budQ += sumBudgetQTY;
                        budA += sumBudgetAMT;

                        rl += 1;



                    }




                    worksheet2.Cells[r, 1].Value = std.MainJobID + " " + std.DescriptionMainJob;
                    worksheet2.Cells[r, 2].Value = sumThisPeriodQTY;
                    worksheet2.Cells[r, 3].Value = sumThisPeriodAMT;
                    worksheet2.Cells[r, 4].Value = sumOpeningQTY;
                    worksheet2.Cells[r, 5].Value = sumOpeningAMT;
                    worksheet2.Cells[r, 6].Value = sumCumBalQTY;
                    worksheet2.Cells[r, 7].Value = sumCumBalAMT;
                    worksheet2.Cells[r, 8].Value = sumBudgetQTY;
                    worksheet2.Cells[r, 9].Value = Percen(sumCumBalQTY, sumBudgetQTY) + "%";
                    worksheet2.Cells[r, 10].Value = sumBudgetAMT;
                    worksheet2.Cells[r, 11].Value = Percen(sumCumBalAMT, sumBudgetAMT) + "%";
                    r++;
                    check = 1;
                }
                else
                {

                }

                //-------End main



                //SUB
                if (subID != std.SubJobID)
                {
                    subID = std.SubJobID;
                    check1 = 0;
                }
                else
                {
                    check1 = 1;
                }
                if (check1 == 0)
                {

                    if (std.MainJobID == std.SubJobID)
                    {

                    }
                    else
                    {
                        sumSubThisPeriodQTY = v_ReportJobCost.Where(c => c.SubJobID == std.SubJobID).Sum(c => c.ThisPeriodQTY);
                        sumSubThisPeriodAMT = v_ReportJobCost.Where(c => c.SubJobID == std.SubJobID).Sum(c => c.ThisperiodAMT);
                        sumSubOpeningQTY = v_ReportJobCost.Where(c => c.SubJobID == std.SubJobID).Sum(c => c.OpeningQTY);
                        sumSubOpeningAMT = v_ReportJobCost.Where(c => c.SubJobID == std.SubJobID).Sum(c => c.OpeningAMT);
                        sumSubCumBalQTY = v_ReportJobCost.Where(c => c.SubJobID == std.SubJobID).Sum(c => c.CumBalQTY);
                        sumSubCumBalAMT = v_ReportJobCost.Where(c => c.SubJobID == std.SubJobID).Sum(c => c.CumBalAMT);
                        sumSubBudgetQTY = v_ReportJobCost.Where(c => c.SubJobID == std.SubJobID).Sum(c => c.BudgetQTY);
                        sumSubBudgetAMT = v_ReportJobCost.Where(c => c.SubJobID == std.SubJobID).Sum(c => c.BudgetAMT);

                        if (std.JobTaskNo == "D2100")
                        {
                            D2100 = std.CumBalAMT;
                        }

                        if (std.JobTaskNo == "E0800")
                        {
                            E0800 = std.CumBalAMT;
                        }
                        if (std.JobTaskNo == "H0400")
                        {
                            H0400 = std.CumBalAMT;
                        }
                        if (std.JobTaskNo == "H0200")
                        {
                            H0200 = std.CumBalAMT;
                        }
                        if (std.JobTaskNo == "H0800")
                        {
                            H0800 = std.CumBalAMT;

                        }
                        if (std.JobTaskNo == "H0600")
                        {
                            H0600 = std.CumBalAMT;
                        }


                        worksheet2.Cells[r, 1].Value = std.SubJobID + " " + std.DescriptionSubJob;
                        worksheet2.Cells[r, 2].Value = sumSubThisPeriodQTY;
                        worksheet2.Cells[r, 3].Value = sumSubThisPeriodAMT;
                        worksheet2.Cells[r, 4].Value = sumSubOpeningQTY;
                        worksheet2.Cells[r, 5].Value = sumSubOpeningAMT;
                        worksheet2.Cells[r, 6].Value = sumSubCumBalQTY;
                        worksheet2.Cells[r, 7].Value = sumSubCumBalAMT;
                        worksheet2.Cells[r, 8].Value = sumSubBudgetQTY;
                        worksheet2.Cells[r, 9].Value = Percen(sumSubCumBalQTY, sumSubBudgetQTY) + "%";
                        worksheet2.Cells[r, 10].Value = sumSubBudgetAMT;
                        worksheet2.Cells[r, 11].Value = Percen(sumSubCumBalAMT, sumSubBudgetAMT) + "%";


                        r++;
                    }
                    check1 = 1;

                }
                else
                {

                }

                //-------End main


                if (std.JobTaskNo == "G0210")
                {
                    G0210 = std.CumBalAMT;
                }
                if (std.JobTaskNo == "C0110")
                {
                    C0110 = std.CumBalAMT;
                }
                if (std.JobTaskNo == "C0120")
                {
                    C0120 = std.CumBalAMT;
                }
                if (std.JobTaskNo == "C0130")
                {
                    C0130 = std.CumBalAMT;
                }
                if (std.JobTaskNo == "C0410")
                {
                    C0410 = std.CumBalAMT;
                }
                if (std.JobTaskNo == "C0510")
                {
                    C0510 = std.CumBalAMT;
                }
                if (std.JobTaskNo == "C0910")
                {
                    C0910 = std.CumBalAMT;
                }
                if (std.JobTaskNo == "C1010")
                {
                    C1010 = std.CumBalAMT;
                }
                if (std.JobTaskNo == "C1110")
                {
                    C1110 = std.CumBalAMT;
                }
                if (std.JobTaskNo == "D0450")
                {
                    D0450 = std.CumBalAMT;
                }
                if (std.JobTaskNo == "H0110")
                {
                    H0110 = std.CumBalAMT;
                }
                if (std.JobTaskNo == "H0130")
                {
                    H0130 = std.CumBalAMT;
                }
                if (std.JobTaskNo == "E0910")
                {
                    E0910 = std.CumBalAMT;
                }
                if (std.JobTaskNo == "E0920")
                {
                    E0920 = std.CumBalAMT;
                }
                if (std.JobTaskNo == "E0930")
                {
                    E0930 = std.CumBalAMT;
                }
                if (std.JobTaskNo == "E0940")
                {
                    E0940 = std.CumBalAMT;
                }

                if (std.JobTaskNo == "F5999")
                {
                    F5999 = std.CumBalAMT;
                }



                if (std.MainJobID == std.JobTaskNo || std.SubJobID == std.JobTaskNo)
                {

                }
                else
                {
                    worksheet2.Cells[r, 1].Value = std.DescriptionFull;
                    worksheet2.Cells[r, 2].Value = std.ThisPeriodQTY;
                    worksheet2.Cells[r, 3].Value = std.ThisperiodAMT;
                    worksheet2.Cells[r, 4].Value = std.OpeningQTY;
                    worksheet2.Cells[r, 5].Value = std.OpeningAMT;
                    worksheet2.Cells[r, 6].Value = std.CumBalQTY;
                    worksheet2.Cells[r, 7].Value = std.CumBalAMT;
                    worksheet2.Cells[r, 8].Value = std.BudgetQTY;
                    worksheet2.Cells[r, 9].Value = Percen(std.CumBalQTY, std.BudgetQTY) + "%";
                    worksheet2.Cells[r, 10].Value = std.BudgetAMT;
                    worksheet2.Cells[r, 11].Value = Percen(std.CumBalAMT, std.BudgetAMT) + "%";
                    r++;
                }

            }



            worksheet3.Cells[5, 1].Value ="Total Income";
            worksheet3.Cells[5, 2].Value =OthisQ;
            worksheet3.Cells[5, 3].Value = OthisA;
            worksheet3.Cells[5, 4].Value =  OopenQ;
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
            worksheet3.Cells[rl, 2].Value = (OthisQ*-1)-thisQ;
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
            using (var range = worksheet3.Cells[15, 1, 15, 11])  //Address "A15:K15"
            {
                range.Style.Font.Bold = true;
                range.Style.Fill.PatternType = ExcelFillStyle.Solid;
                range.Style.Fill.BackgroundColor.SetColor(Color.Yellow);
                //range.Style.Font.Color.SetColor(Color.White);
            }


            //Ok now format the values;
            using (var range = worksheet3.Cells[16, 1, 16, 11])  //Address "A15:K15"
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






        // POST: JobLedgerEntry/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.

        public IActionResult Data(string JobNo, string Startdate, string EndDate)
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
            IActionResult response = Unauthorized();


            /*Check Session */
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


            var queryData = " SELECT ROW_NUMBER() OVER (ORDER BY b.JobTaskNo) as ID,b.MainJobID,b.DescriptionMainJob,b.SubJobID,b.DescriptionSubJob,b.JobTaskNo,b.DescriptionFull,"
            + " (((CASE WHEN SUBSTRING(b.MainJobID, 1, 1) = 'N' or SUBSTRING(b.MainJobID,1,1)= 'O' THEN "
            + " (select isnull(sum([Quantity]) + sum([Line Amount]),0) FROM"+ Environment.GetEnvironmentVariable("Company") +"Job Ledger Entry] WHERE [Job No_] = {0} AND [Posting Date] > {1} AND [Posting Date] < {2} and SUBSTRING([Job Task No_],1,5) = b.JobTaskNo and [Type of task]=0 )	"
            + " ELSE "
            + " (select isnull(sum([Quantity]), 0) FROM"+ Environment.GetEnvironmentVariable("Company") +"Job Ledger Entry] WHERE[Job No_] = {0} AND [Posting Date] > {1} AND [Posting Date] < {2} and SUBSTRING([Job Task No_],1,5) = b.JobTaskNo and [Type of task]=0) END "
            + " )+"
            + " (CASE WHEN SUBSTRING(b.MainJobID, 1, 1) = 'N' or SUBSTRING(b.MainJobID,1,1)= 'O' THEN "
            + " (select isnull(sum([Quantity]) + sum([Line Amount]),0) FROM"+ Environment.GetEnvironmentVariable("Company") +"Job Ledger Entry] WHERE[Job No_] = {0} AND [Posting Date] > {1} AND [Posting Date] < {2} and SUBSTRING([Job Task No_],1,5) = b.JobTaskNo and [Type of task]=3 )	"
            + " ELSE "
            + "	(select isnull(sum([Quantity]), 0) FROM"+ Environment.GetEnvironmentVariable("Company") +"Job Ledger Entry] WHERE[Job No_] = {0} AND [Posting Date] > {1} AND [Posting Date] < {2} and SUBSTRING([Job Task No_],1,5) = b.JobTaskNo and [Type of task]=3) END "
            + " )+"
            + " (CASE WHEN SUBSTRING(b.MainJobID, 1, 1) = 'N' or SUBSTRING(b.MainJobID,1,1)= 'O' THEN "
            + " (select isnull(sum([Quantity]) + sum([Line Amount]),0) FROM"+ Environment.GetEnvironmentVariable("Company") +"Job Ledger Entry] WHERE[Job No_] = {0} AND [Posting Date] > {1} AND [Posting Date] < {2} and SUBSTRING([Job Task No_],1,5) = b.JobTaskNo and [Type of task]=2)	"
            + " ELSE "
            + " (select isnull(sum([Quantity]), 0) FROM"+ Environment.GetEnvironmentVariable("Company") +"Job Ledger Entry] WHERE[Job No_] = {0} AND [Posting Date] > {1} AND [Posting Date] < {2} and SUBSTRING([Job Task No_],1,5) = b.JobTaskNo and [Type of task]=2) END "
            + " )" +
            "+((select isnull(sum([Quantity]), 0) FROM "+ Environment.GetEnvironmentVariable("Company") +"Job Ledger Entry] WHERE [From Location] = {0} AND [Posting Date] > {1} AND [Posting Date] < {2} and SUBSTRING([Job Task No_],1,5) = b.JobTaskNo and [Type of task]=1 and [Document No_] Like 'CAL%' )*2)) -"
            + " ((CASE WHEN SUBSTRING(b.MainJobID, 1, 1) = 'N' or SUBSTRING(b.MainJobID,1,1)= 'O' THEN "
            + " (select isnull(sum([Quantity]) + sum([Line Amount]),0) FROM"+ Environment.GetEnvironmentVariable("Company") +"Job Ledger Entry] WHERE[Job No_] = {0} AND [Posting Date] > {1} AND [Posting Date] < {2} and SUBSTRING([Job Task No_],1,5) = b.JobTaskNo and [Type of task]=1)	"
            + " ELSE "
            + " (select isnull(sum([Quantity]), 0) FROM"+ Environment.GetEnvironmentVariable("Company") +"Job Ledger Entry] WHERE[Job No_] = {0} AND [Posting Date] > {1} AND [Posting Date] < {2} and SUBSTRING([Job Task No_],1,5) = b.JobTaskNo and [Type of task]=1) END "
            + " )+ "
            + " (select isnull(sum([Quantity]), 0) FROM "+ Environment.GetEnvironmentVariable("Company") +"Job Ledger Entry] WHERE [From Location] = {0} AND [Posting Date] > {1} AND [Posting Date] < {2} and SUBSTRING([Job Task No_],1,5) = b.JobTaskNo and [Type of task]=3 and [Job No_] <> {0} )))  AS ThisPeriodQTY "
            + " , "

            + " (((CASE WHEN SUBSTRING(b.MainJobID, 1, 1) = 'N' or SUBSTRING(b.MainJobID,1,1)= 'O' THEN "
            + " (select isnull(sum([Original Total Cost]) + sum([Line Amount]),0) FROM"+ Environment.GetEnvironmentVariable("Company") +"Job Ledger Entry] WHERE [Job No_] = {0} AND [Posting Date] > {1} AND [Posting Date] < {2} and SUBSTRING([Job Task No_],1,5) = b.JobTaskNo and [Type of task]=0 )	"
            + " ELSE "
            + " (select isnull(sum([Original Total Cost]), 0) FROM"+ Environment.GetEnvironmentVariable("Company") +"Job Ledger Entry] WHERE[Job No_] = {0} AND [Posting Date] > {1} AND [Posting Date] < {2} and SUBSTRING([Job Task No_],1,5) = b.JobTaskNo and [Type of task]=0) END "
            + " )+"
            + " (CASE WHEN SUBSTRING(b.MainJobID, 1, 1) = 'N' or SUBSTRING(b.MainJobID,1,1)= 'O' THEN "
            + " (select isnull(sum([Original Total Cost]) + sum([Line Amount]),0) FROM"+ Environment.GetEnvironmentVariable("Company") +"Job Ledger Entry] WHERE[Job No_] = {0} AND [Posting Date] > {1} AND [Posting Date] < {2} and SUBSTRING([Job Task No_],1,5) = b.JobTaskNo and [Type of task]=3 )	"
            + " ELSE "
            + "	(select isnull(sum([Original Total Cost]), 0) FROM"+ Environment.GetEnvironmentVariable("Company") +"Job Ledger Entry] WHERE[Job No_] = {0} AND [Posting Date] > {1} AND [Posting Date] < {2} and SUBSTRING([Job Task No_],1,5) = b.JobTaskNo and [Type of task]=3) END "
            + " )+"
            + " (CASE WHEN SUBSTRING(b.MainJobID, 1, 1) = 'N' or SUBSTRING(b.MainJobID,1,1)= 'O' THEN "
            + " (select isnull(sum([Original Total Cost]) + sum([Line Amount]),0) FROM"+ Environment.GetEnvironmentVariable("Company") +"Job Ledger Entry] WHERE[Job No_] = {0} AND [Posting Date] > {1} AND [Posting Date] < {2} and SUBSTRING([Job Task No_],1,5) = b.JobTaskNo and [Type of task]=2)	"
            + " ELSE "
            + " (select isnull(sum([Original Total Cost]), 0) FROM"+ Environment.GetEnvironmentVariable("Company") +"Job Ledger Entry] WHERE[Job No_] = {0} AND [Posting Date] > {1} AND [Posting Date] < {2} and SUBSTRING([Job Task No_],1,5) = b.JobTaskNo and [Type of task]=2) END "
            + " )"
            + "+((select isnull(sum([Original Total Cost]), 0) FROM "+ Environment.GetEnvironmentVariable("Company") +"Job Ledger Entry] WHERE [From Location] = {0} AND [Posting Date] > {1} AND [Posting Date] < {2} and SUBSTRING([Job Task No_],1,5) = b.JobTaskNo and [Type of task]=1 and [Document No_] Like 'CAL%' )*2)) -"
            + " ((CASE WHEN SUBSTRING(b.MainJobID, 1, 1) = 'N' or SUBSTRING(b.MainJobID,1,1)= 'O' THEN "
            + " (select isnull(sum([Original Total Cost]) + sum([Line Amount]),0) FROM"+ Environment.GetEnvironmentVariable("Company") +"Job Ledger Entry] WHERE[Job No_] = {0} AND [Posting Date] > {1} AND [Posting Date] < {2} and SUBSTRING([Job Task No_],1,5) = b.JobTaskNo and [Type of task]=1)	"
            + " ELSE "
            + " (select isnull(sum([Original Total Cost]), 0) FROM"+ Environment.GetEnvironmentVariable("Company") +"Job Ledger Entry] WHERE[Job No_] = {0} AND [Posting Date] > {1} AND [Posting Date] < {2} and SUBSTRING([Job Task No_],1,5) = b.JobTaskNo and [Type of task]=1) END "
            + " )+ "
            + " (select isnull(sum([Original Total Cost]), 0) FROM "+ Environment.GetEnvironmentVariable("Company") +"Job Ledger Entry] WHERE [From Location] = {0} AND [Posting Date] > {1} AND [Posting Date] < {2} and SUBSTRING([Job Task No_],1,5) = b.JobTaskNo and [Type of task]=3 and [Job No_] <> {0} ))" +

              "+"
              + "(CASE WHEN SUBSTRING(b.MainJobID, 1, 1) = 'N' or SUBSTRING(b.MainJobID,1,1)= 'O' THEN 0 ELSE  	(select isnull(sum([Total Cost]), 0) FROM"+ Environment.GetEnvironmentVariable("Company") +"Job Ledger Entry] WHERE[Job No_] = {0}  AND [Posting Date] > {1} AND [Posting Date] < {2}  and SUBSTRING([Job Task No_],1,5) = b.JobTaskNo and [Type of task]=0 and [Original Total Cost]=0) END "
             + " ))  AS ThisPeriodAMT "

            + " , "

            + " (((CASE WHEN SUBSTRING(b.MainJobID, 1, 1) = 'N' or SUBSTRING(b.MainJobID,1,1)= 'O' THEN "
            + " (select isnull(sum([Quantity]) + sum([Line Amount]),0) FROM"+ Environment.GetEnvironmentVariable("Company") +"Job Ledger Entry] WHERE [Job No_] = {0} AND [Posting Date] < {1} and SUBSTRING([Job Task No_],1,5) = b.JobTaskNo and [Type of task]=0 )	"
            + " ELSE "
            + " (select isnull(sum([Quantity]), 0) FROM"+ Environment.GetEnvironmentVariable("Company") +"Job Ledger Entry] WHERE[Job No_] = {0} AND [Posting Date] < {1} and SUBSTRING([Job Task No_],1,5) = b.JobTaskNo and [Type of task]=0) END "
            + " )+"
            + " (CASE WHEN SUBSTRING(b.MainJobID, 1, 1) = 'N' or SUBSTRING(b.MainJobID,1,1)= 'O' THEN "
            + " (select isnull(sum([Quantity]) + sum([Line Amount]),0) FROM"+ Environment.GetEnvironmentVariable("Company") +"Job Ledger Entry] WHERE[Job No_] = {0} AND [Posting Date] < {1} and SUBSTRING([Job Task No_],1,5) = b.JobTaskNo and [Type of task]=3 )	"
            + " ELSE "
            + "	(select isnull(sum([Quantity]), 0) FROM"+ Environment.GetEnvironmentVariable("Company") +"Job Ledger Entry] WHERE[Job No_] = {0} AND[Posting Date] < {1} and SUBSTRING([Job Task No_],1,5) = b.JobTaskNo and [Type of task]=3) END "
            + " )+"
            + " (CASE WHEN SUBSTRING(b.MainJobID, 1, 1) = 'N' or SUBSTRING(b.MainJobID,1,1)= 'O' THEN "
            + " (select isnull(sum([Quantity]) + sum([Line Amount]),0) FROM"+ Environment.GetEnvironmentVariable("Company") +"Job Ledger Entry] WHERE[Job No_] = {0} AND [Posting Date] < {1} and SUBSTRING([Job Task No_],1,5) = b.JobTaskNo and [Type of task]=2)	"
            + " ELSE "
            + " (select isnull(sum([Quantity]), 0) FROM"+ Environment.GetEnvironmentVariable("Company") +"Job Ledger Entry] WHERE[Job No_] = {0} AND [Posting Date] < {1} and SUBSTRING([Job Task No_],1,5) = b.JobTaskNo and [Type of task]=2) END "
            + " )"
            + "+((select isnull(sum([Quantity]), 0) FROM "+ Environment.GetEnvironmentVariable("Company") +"Job Ledger Entry] WHERE [From Location] = {0} AND [Posting Date] < {1}  and SUBSTRING([Job Task No_],1,5) = b.JobTaskNo and [Type of task]=1 and [Document No_] Like 'CAL%' )*2)) -"
            + " ((CASE WHEN SUBSTRING(b.MainJobID, 1, 1) = 'N' or SUBSTRING(b.MainJobID,1,1)= 'O' THEN "
            + " (select isnull(sum([Quantity]) + sum([Line Amount]),0) FROM"+ Environment.GetEnvironmentVariable("Company") +"Job Ledger Entry] WHERE[Job No_] = {0} AND [Posting Date] < {1} and SUBSTRING([Job Task No_],1,5) = b.JobTaskNo and [Type of task]=1)	"
            + " ELSE "
            + " (select isnull(sum([Quantity]), 0) FROM"+ Environment.GetEnvironmentVariable("Company") +"Job Ledger Entry] WHERE[Job No_] = {0} AND [Posting Date] < {1} and SUBSTRING([Job Task No_],1,5) = b.JobTaskNo and [Type of task]=1) END "
            + " )+ "
            + " (select isnull(sum([Quantity]), 0) FROM "+ Environment.GetEnvironmentVariable("Company") +"Job Ledger Entry] WHERE [From Location] = {0} AND [Posting Date] < {1} and SUBSTRING([Job Task No_],1,5) = b.JobTaskNo and [Type of task]=3 and [Job No_] <> {0} )))  AS OpeningQTY "
            + " , "

            + " (((CASE WHEN SUBSTRING(b.MainJobID, 1, 1) = 'N' or SUBSTRING(b.MainJobID,1,1)= 'O' THEN "
            + " (select isnull(sum([Original Total Cost]) + sum([Line Amount]),0) FROM"+ Environment.GetEnvironmentVariable("Company") +"Job Ledger Entry] WHERE [Job No_] = {0} AND  [Posting Date] < {1} and SUBSTRING([Job Task No_],1,5) = b.JobTaskNo and [Type of task]=0 )	"
            + " ELSE "
            + " (select isnull(sum([Original Total Cost]), 0) FROM"+ Environment.GetEnvironmentVariable("Company") +"Job Ledger Entry] WHERE[Job No_] = {0} AND [Posting Date] < {1} and SUBSTRING([Job Task No_],1,5) = b.JobTaskNo and [Type of task]=0) END "
            + " )+"
            + " (CASE WHEN SUBSTRING(b.MainJobID, 1, 1) = 'N' or SUBSTRING(b.MainJobID,1,1)= 'O' THEN "
            + " (select isnull(sum([Original Total Cost]) + sum([Line Amount]),0) FROM"+ Environment.GetEnvironmentVariable("Company") +"Job Ledger Entry] WHERE[Job No_] = {0} AND [Posting Date] < {1} and SUBSTRING([Job Task No_],1,5) = b.JobTaskNo and [Type of task]=3 )	"
            + " ELSE "
            + "	(select isnull(sum([Original Total Cost]), 0) FROM"+ Environment.GetEnvironmentVariable("Company") +"Job Ledger Entry] WHERE[Job No_] = {0} AND [Posting Date] < {1} and SUBSTRING([Job Task No_],1,5) = b.JobTaskNo and [Type of task]=3) END "
            + " )+"
            + " (CASE WHEN SUBSTRING(b.MainJobID, 1, 1) = 'N' or SUBSTRING(b.MainJobID,1,1)= 'O' THEN "
            + " (select isnull(sum([Original Total Cost]) + sum([Line Amount]),0) FROM"+ Environment.GetEnvironmentVariable("Company") +"Job Ledger Entry] WHERE[Job No_] = {0} AND [Posting Date] < {1} and SUBSTRING([Job Task No_],1,5) = b.JobTaskNo and [Type of task]=2)	"
            + " ELSE "
            + " (select isnull(sum([Original Total Cost]), 0) FROM"+ Environment.GetEnvironmentVariable("Company") +"Job Ledger Entry] WHERE[Job No_] = {0} AND [Posting Date] < {1} and SUBSTRING([Job Task No_],1,5) = b.JobTaskNo and [Type of task]=2) END "
            + " )"
            + " +((select isnull(sum([Original Total Cost]), 0) FROM "+ Environment.GetEnvironmentVariable("Company") +"Job Ledger Entry] WHERE [From Location] = {0} AND [Posting Date] < {1}  and SUBSTRING([Job Task No_],1,5) = b.JobTaskNo and [Type of task]=1 and [Document No_] Like 'CAL%' )*2)) -"
            + " ((CASE WHEN SUBSTRING(b.MainJobID, 1, 1) = 'N' or SUBSTRING(b.MainJobID,1,1)= 'O' THEN "
            + " (select isnull(sum([Original Total Cost]) + sum([Line Amount]),0) FROM"+ Environment.GetEnvironmentVariable("Company") +"Job Ledger Entry] WHERE[Job No_] = {0} AND [Posting Date] < {1} and SUBSTRING([Job Task No_],1,5) = b.JobTaskNo and [Type of task]=1)	"
            + " ELSE "
            + " (select isnull(sum([Original Total Cost]), 0) FROM"+ Environment.GetEnvironmentVariable("Company") +"Job Ledger Entry] WHERE[Job No_] = {0} AND [Posting Date] < {1} and SUBSTRING([Job Task No_],1,5) = b.JobTaskNo and [Type of task]=1) END "
            + " )+ "
            + " (select isnull(sum([Original Total Cost]), 0) FROM "+ Environment.GetEnvironmentVariable("Company") +"Job Ledger Entry] WHERE [From Location] = {0} AND [Posting Date] < {1} and SUBSTRING([Job Task No_],1,5) = b.JobTaskNo and [Type of task]=3 and [Job No_] <> {0} ))" +
                "+" +
              "(CASE WHEN SUBSTRING(b.MainJobID, 1, 1) = 'N' or SUBSTRING(b.MainJobID,1,1)= 'O' THEN 0 ELSE  	(select isnull(sum([Total Cost]), 0) FROM"+ Environment.GetEnvironmentVariable("Company") +"Job Ledger Entry] WHERE[Job No_] = {0} AND [Posting Date] < {1} and SUBSTRING([Job Task No_],1,5) = b.JobTaskNo and [Type of task]=0 and [Original Total Cost]=0) END" +
              " ))  AS OpeningAMT "






            + " , "

            + " (((CASE WHEN SUBSTRING(b.MainJobID, 1, 1) = 'N' or SUBSTRING(b.MainJobID,1,1)= 'O' THEN "
            + " (select isnull(sum([Quantity]) + sum([Line Amount]),0) FROM"+ Environment.GetEnvironmentVariable("Company") +"Job Ledger Entry] WHERE [Job No_] = {0} AND [Posting Date] < {2} and SUBSTRING([Job Task No_],1,5) = b.JobTaskNo and [Type of task]=0 )	"
            + " ELSE "
            + " (select isnull(sum([Quantity]), 0) FROM"+ Environment.GetEnvironmentVariable("Company") +"Job Ledger Entry] WHERE[Job No_] = {0} AND [Posting Date] < {2} and SUBSTRING([Job Task No_],1,5) = b.JobTaskNo and [Type of task]=0) END "
            + " )+"
            + " (CASE WHEN SUBSTRING(b.MainJobID, 1, 1) = 'N' or SUBSTRING(b.MainJobID,1,1)= 'O' THEN "
            + " (select isnull(sum([Quantity]) + sum([Line Amount]),0) FROM"+ Environment.GetEnvironmentVariable("Company") +"Job Ledger Entry] WHERE[Job No_] = {0} AND [Posting Date] < {2} and SUBSTRING([Job Task No_],1,5) = b.JobTaskNo and [Type of task]=3 )	"
            + " ELSE "
            + "	(select isnull(sum([Quantity]), 0) FROM"+ Environment.GetEnvironmentVariable("Company") +"Job Ledger Entry] WHERE[Job No_] = {0} AND[Posting Date] < {2} and SUBSTRING([Job Task No_],1,5) = b.JobTaskNo and [Type of task]=3) END "
            + " )+"
            + " (CASE WHEN SUBSTRING(b.MainJobID, 1, 1) = 'N' or SUBSTRING(b.MainJobID,1,1)= 'O' THEN "
            + " (select isnull(sum([Quantity]) + sum([Line Amount]),0) FROM"+ Environment.GetEnvironmentVariable("Company") +"Job Ledger Entry] WHERE[Job No_] = {0} AND [Posting Date] < {2} and SUBSTRING([Job Task No_],1,5) = b.JobTaskNo and [Type of task]=2)	"
            + " ELSE "
            + " (select isnull(sum([Quantity]), 0) FROM"+ Environment.GetEnvironmentVariable("Company") +"Job Ledger Entry] WHERE[Job No_] = {0} AND [Posting Date] < {2} and SUBSTRING([Job Task No_],1,5) = b.JobTaskNo and [Type of task]=2) END "
            + " )) -"
            + " ((CASE WHEN SUBSTRING(b.MainJobID, 1, 1) = 'N' or SUBSTRING(b.MainJobID,1,1)= 'O' THEN "
            + " (select isnull(sum([Quantity]) + sum([Line Amount]),0) FROM"+ Environment.GetEnvironmentVariable("Company") +"Job Ledger Entry] WHERE[Job No_] = {0} AND [Posting Date] < {2} and SUBSTRING([Job Task No_],1,5) = b.JobTaskNo and [Type of task]=1)	"
            + " ELSE "
            + " (select isnull(sum([Quantity]), 0) FROM"+ Environment.GetEnvironmentVariable("Company") +"Job Ledger Entry] WHERE[Job No_] = {0} AND [Posting Date] < {2} and SUBSTRING([Job Task No_],1,5) = b.JobTaskNo and [Type of task]=1) END "
            + " )+ "
            + " (select isnull(sum([Quantity]), 0) FROM "+ Environment.GetEnvironmentVariable("Company") +"Job Ledger Entry] WHERE [From Location] = {0} AND [Posting Date] < {2} and SUBSTRING([Job Task No_],1,5) = b.JobTaskNo and [Type of task]=3 and [Job No_] <> {0} )))  AS CumBalQTY "
            + " , "

              + " (((CASE WHEN SUBSTRING(b.MainJobID, 1, 1) = 'N' or SUBSTRING(b.MainJobID,1,1)= 'O' THEN "
              + " (select isnull(sum([Original Total Cost]) + sum([Line Amount]),0) FROM"+ Environment.GetEnvironmentVariable("Company") +"Job Ledger Entry] WHERE [Job No_] = {0} AND  [Posting Date] < {2} and SUBSTRING([Job Task No_],1,5) = b.JobTaskNo and [Type of task]=0 )	"
              + " ELSE "
              + " (select isnull(sum([Original Total Cost]), 0) FROM"+ Environment.GetEnvironmentVariable("Company") +"Job Ledger Entry] WHERE[Job No_] = {0} AND [Posting Date] < {2} and SUBSTRING([Job Task No_],1,5) = b.JobTaskNo and [Type of task]=0) END "
              + " )+"
              + " (CASE WHEN SUBSTRING(b.MainJobID, 1, 1) = 'N' or SUBSTRING(b.MainJobID,1,1)= 'O' THEN "
              + " (select isnull(sum([Original Total Cost]) + sum([Line Amount]),0) FROM"+ Environment.GetEnvironmentVariable("Company") +"Job Ledger Entry] WHERE[Job No_] = {0} AND [Posting Date] < {2} and SUBSTRING([Job Task No_],1,5) = b.JobTaskNo and [Type of task]=3 )	"
              + " ELSE "
              + "	(select isnull(sum([Original Total Cost]), 0) FROM"+ Environment.GetEnvironmentVariable("Company") +"Job Ledger Entry] WHERE[Job No_] = {0} AND [Posting Date] < {2} and SUBSTRING([Job Task No_],1,5) = b.JobTaskNo and [Type of task]=3) END "
              + " )+"
              + " (CASE WHEN SUBSTRING(b.MainJobID, 1, 1) = 'N' or SUBSTRING(b.MainJobID,1,1)= 'O' THEN "
              + " (select isnull(sum([Original Total Cost]) + sum([Line Amount]),0) FROM"+ Environment.GetEnvironmentVariable("Company") +"Job Ledger Entry] WHERE[Job No_] = {0} AND [Posting Date] < {2} and SUBSTRING([Job Task No_],1,5) = b.JobTaskNo and [Type of task]=2)	"
              + " ELSE "
              + " (select isnull(sum([Original Total Cost]), 0) FROM"+ Environment.GetEnvironmentVariable("Company") +"Job Ledger Entry] WHERE[Job No_] = {0} AND [Posting Date] < {2} and SUBSTRING([Job Task No_],1,5) = b.JobTaskNo and [Type of task]=2) END "
              + " )"
              + " +((select isnull(sum([Original Total Cost]), 0) FROM "+ Environment.GetEnvironmentVariable("Company") +"Job Ledger Entry] WHERE [From Location] = {0} AND [Posting Date] < {2}  and SUBSTRING([Job Task No_],1,5) = b.JobTaskNo and [Type of task]=1 and [Document No_] Like 'CAL%' )*2)) -"
              + " ((CASE WHEN SUBSTRING(b.MainJobID, 1, 1) = 'N' or SUBSTRING(b.MainJobID,1,1)= 'O' THEN "
              + " (select isnull(sum([Original Total Cost]) + sum([Line Amount]),0) FROM"+ Environment.GetEnvironmentVariable("Company") +"Job Ledger Entry] WHERE[Job No_] = {0} AND [Posting Date] < {2} and SUBSTRING([Job Task No_],1,5) = b.JobTaskNo and [Type of task]=1)	"
              + " ELSE "
              + " (select isnull(sum([Original Total Cost]), 0) FROM"+ Environment.GetEnvironmentVariable("Company") +"Job Ledger Entry] WHERE[Job No_] = {0} AND [Posting Date] < {2} and SUBSTRING([Job Task No_],1,5) = b.JobTaskNo and [Type of task]=1) END "
              + " )+ "
              + " (select isnull(sum([Original Total Cost]), 0) FROM "+ Environment.GetEnvironmentVariable("Company") +"Job Ledger Entry] WHERE [From Location] = {0} AND [Posting Date] < {2} and SUBSTRING([Job Task No_],1,5) = b.JobTaskNo and [Type of task]=3 and [Job No_] <> {0} ))" +


              "+" +
              "(CASE WHEN SUBSTRING(b.MainJobID, 1, 1) = 'N' or SUBSTRING(b.MainJobID,1,1)= 'O' THEN 0 ELSE  	(select isnull(sum([Total Cost]), 0) FROM"+ Environment.GetEnvironmentVariable("Company") +"Job Ledger Entry] WHERE[Job No_] = {0} AND [Posting Date] < {2} and SUBSTRING([Job Task No_],1,5) = b.JobTaskNo and [Type of task]=0 and [Original Total Cost]=0) END ))  AS CumBalAMT "
            + " , "

            + " (SELECT isnull(sum(Quantity), 0) FROM dbo."+ Environment.GetEnvironmentVariable("Company") +"Job Planning Line] WHERE[Job No_]={0} and SUBSTRING([Job Task No_],1,5)=b.JobTaskNo) as BudgetQTY, "
            + " (SELECT isnull(sum([Total Cost (LCY)]),0) FROM dbo."+ Environment.GetEnvironmentVariable("Company") +"Job Planning Line] WHERE[Job No_]= {0} and SUBSTRING([Job Task No_],1,5)= b.JobTaskNo) as BudgetAMT "

            + " FROM("
             + " SELECT a.JobTaskNo,a.MainJobID,a.SubJobID,"
             + " (SELECT TOP 1 dbo."+ Environment.GetEnvironmentVariable("Company") +"Job Task].Description FROM dbo."+ Environment.GetEnvironmentVariable("Company") +"Job Task] WHERE [Job Task No_]=a.JobTaskNo) as Description,"
             + " (SELECT TOP 1 ([Job Task No_]+' '+Description) FROM dbo."+ Environment.GetEnvironmentVariable("Company") +"Job Task] WHERE [Job Task No_]=a.JobTaskNo) as DescriptionFull,"
             + " (SELECT TOP 1 dbo."+ Environment.GetEnvironmentVariable("Company") +"Job Task].Description FROM dbo."+ Environment.GetEnvironmentVariable("Company") +"Job Task] WHERE SUBSTRING(dbo."+ Environment.GetEnvironmentVariable("Company") +"Job Task].[Job Task No_],1,1)+'0000'=a.MainJobID) as DescriptionMainJob,"
             + " (SELECT TOP 1 dbo."+ Environment.GetEnvironmentVariable("Company") +"Job Task].Description FROM dbo."+ Environment.GetEnvironmentVariable("Company") +"Job Task] WHERE SUBSTRING(dbo."+ Environment.GetEnvironmentVariable("Company") +"Job Task].[Job Task No_],1,3)+'00'=a.SubJobID) as DescriptionSubJob"
             + " FROM  (SELECT DISTINCT dbo."+ Environment.GetEnvironmentVariable("Company") +"Job Task].[Job Task No_] as JobTaskNo,"
             + " SUBSTRING(dbo."+ Environment.GetEnvironmentVariable("Company") +"Job Task].[Job Task No_],1,1)+'0000' as MainJobID,SUBSTRING(dbo."+ Environment.GetEnvironmentVariable("Company") +"Job Task].[Job Task No_],1,3)+'00' as SubJobID FROM dbo."+ Environment.GetEnvironmentVariable("Company") +"Job Task]) as a) as b ORDER BY b.JobTaskNo";






            //var queryData = " SELECT b.MainJobID,b.DescriptionMainJob,b.SubJobID,b.DescriptionSubJob,b.JobTaskNo,b.DescriptionFull,"
            //            + " (((CASE WHEN SUBSTRING(b.MainJobID, 1, 1) = 'N' or SUBSTRING(b.MainJobID,1,1)= 'O' THEN "
            //            + " (select isnull(sum([Quantity]) + sum([Line Amount]),0) FROM"+ Environment.GetEnvironmentVariable("Company") +"Job Ledger Entry] WHERE [Job No_] = {0} AND [Posting Date] > {1} AND [Posting Date] < {2} and SUBSTRING([Job Task No_],1,5) = b.JobTaskNo and [Type of task]=0 )	"
            //            + " ELSE "
            //            + " (select isnull(sum([Quantity]), 0) FROM"+ Environment.GetEnvironmentVariable("Company") +"Job Ledger Entry] WHERE[Job No_] = {0} AND [Posting Date] > {1} AND [Posting Date] < {2} and SUBSTRING([Job Task No_],1,5) = b.JobTaskNo and [Type of task]=0) END "
            //            + " )+"
            //            + " (CASE WHEN SUBSTRING(b.MainJobID, 1, 1) = 'N' or SUBSTRING(b.MainJobID,1,1)= 'O' THEN "
            //            + " (select isnull(sum([Quantity]) + sum([Line Amount]),0) FROM"+ Environment.GetEnvironmentVariable("Company") +"Job Ledger Entry] WHERE[Job No_] = {0} AND [Posting Date] > {1} AND [Posting Date] < {2} and SUBSTRING([Job Task No_],1,5) = b.JobTaskNo and [Type of task]=3 )	"
            //            + " ELSE "
            //            + "	(select isnull(sum([Quantity]), 0) FROM"+ Environment.GetEnvironmentVariable("Company") +"Job Ledger Entry] WHERE[Job No_] = {0} AND [Posting Date] > {1} AND [Posting Date] < {2} and SUBSTRING([Job Task No_],1,5) = b.JobTaskNo and [Type of task]=3) END "
            //            + " )+"
            //            + " (CASE WHEN SUBSTRING(b.MainJobID, 1, 1) = 'N' or SUBSTRING(b.MainJobID,1,1)= 'O' THEN "
            //            + " (select isnull(sum([Quantity]) + sum([Line Amount]),0) FROM"+ Environment.GetEnvironmentVariable("Company") +"Job Ledger Entry] WHERE[Job No_] = {0} AND [Posting Date] > {1} AND [Posting Date] < {2} and SUBSTRING([Job Task No_],1,5) = b.JobTaskNo and [Type of task]=2)	"
            //            + " ELSE "
            //            + " (select isnull(sum([Quantity]), 0) FROM"+ Environment.GetEnvironmentVariable("Company") +"Job Ledger Entry] WHERE[Job No_] = {0} AND [Posting Date] > {1} AND [Posting Date] < {2} and SUBSTRING([Job Task No_],1,5) = b.JobTaskNo and [Type of task]=2) END "
            //            + " )" +
            //            "+((select isnull(sum([Quantity]), 0) FROM "+ Environment.GetEnvironmentVariable("Company") +"Job Ledger Entry] WHERE [From Location] = {0} AND [Posting Date] > {1} AND [Posting Date] < {2} and SUBSTRING([Job Task No_],1,5) = b.JobTaskNo and [Type of task]=1 and [Document No_] Like 'CAL%' )*2)) -"
            //            + " ((CASE WHEN SUBSTRING(b.MainJobID, 1, 1) = 'N' or SUBSTRING(b.MainJobID,1,1)= 'O' THEN "
            //            + " (select isnull(sum([Quantity]) + sum([Line Amount]),0) FROM"+ Environment.GetEnvironmentVariable("Company") +"Job Ledger Entry] WHERE[Job No_] = {0} AND [Posting Date] > {1} AND [Posting Date] < {2} and SUBSTRING([Job Task No_],1,5) = b.JobTaskNo and [Type of task]=1)	"
            //            + " ELSE "
            //            + " (select isnull(sum([Quantity]), 0) FROM"+ Environment.GetEnvironmentVariable("Company") +"Job Ledger Entry] WHERE[Job No_] = {0} AND [Posting Date] > {1} AND [Posting Date] < {2} and SUBSTRING([Job Task No_],1,5) = b.JobTaskNo and [Type of task]=1) END "
            //            + " )+ "
            //            + " (select isnull(sum([Quantity]), 0) FROM "+ Environment.GetEnvironmentVariable("Company") +"Job Ledger Entry] WHERE [From Location] = {0} AND [Posting Date] > {1} AND [Posting Date] < {2} and SUBSTRING([Job Task No_],1,5) = b.JobTaskNo and [Type of task]=3 and [Job No_] <> {0} )))  AS ThisPeriodQTY "
            //            + " , "

            //            + " (((CASE WHEN SUBSTRING(b.MainJobID, 1, 1) = 'N' or SUBSTRING(b.MainJobID,1,1)= 'O' THEN "
            //            + " (select isnull(sum([Original Total Cost]) + sum([Line Amount]),0) FROM"+ Environment.GetEnvironmentVariable("Company") +"Job Ledger Entry] WHERE [Job No_] = {0} AND [Posting Date] > {1} AND [Posting Date] < {2} and SUBSTRING([Job Task No_],1,5) = b.JobTaskNo and [Type of task]=0 )	"
            //            + " ELSE "
            //            + " (select isnull(sum([Original Total Cost]), 0) FROM"+ Environment.GetEnvironmentVariable("Company") +"Job Ledger Entry] WHERE[Job No_] = {0} AND [Posting Date] > {1} AND [Posting Date] < {2} and SUBSTRING([Job Task No_],1,5) = b.JobTaskNo and [Type of task]=0) END "
            //            + " )+"
            //            + " (CASE WHEN SUBSTRING(b.MainJobID, 1, 1) = 'N' or SUBSTRING(b.MainJobID,1,1)= 'O' THEN "
            //            + " (select isnull(sum([Original Total Cost]) + sum([Line Amount]),0) FROM"+ Environment.GetEnvironmentVariable("Company") +"Job Ledger Entry] WHERE[Job No_] = {0} AND [Posting Date] > {1} AND [Posting Date] < {2} and SUBSTRING([Job Task No_],1,5) = b.JobTaskNo and [Type of task]=3 )	"
            //            + " ELSE "
            //            + "	(select isnull(sum([Original Total Cost]), 0) FROM"+ Environment.GetEnvironmentVariable("Company") +"Job Ledger Entry] WHERE[Job No_] = {0} AND [Posting Date] > {1} AND [Posting Date] < {2} and SUBSTRING([Job Task No_],1,5) = b.JobTaskNo and [Type of task]=3) END "
            //            + " )+"
            //            + " (CASE WHEN SUBSTRING(b.MainJobID, 1, 1) = 'N' or SUBSTRING(b.MainJobID,1,1)= 'O' THEN "
            //            + " (select isnull(sum([Original Total Cost]) + sum([Line Amount]),0) FROM"+ Environment.GetEnvironmentVariable("Company") +"Job Ledger Entry] WHERE[Job No_] = {0} AND [Posting Date] > {1} AND [Posting Date] < {2} and SUBSTRING([Job Task No_],1,5) = b.JobTaskNo and [Type of task]=2)	"
            //            + " ELSE "
            //            + " (select isnull(sum([Original Total Cost]), 0) FROM"+ Environment.GetEnvironmentVariable("Company") +"Job Ledger Entry] WHERE[Job No_] = {0} AND [Posting Date] > {1} AND [Posting Date] < {2} and SUBSTRING([Job Task No_],1,5) = b.JobTaskNo and [Type of task]=2) END "
            //            + " )"
            //            + "+((select isnull(sum([Original Total Cost]), 0) FROM "+ Environment.GetEnvironmentVariable("Company") +"Job Ledger Entry] WHERE [From Location] = {0} AND [Posting Date] > {1} AND [Posting Date] < {2} and SUBSTRING([Job Task No_],1,5) = b.JobTaskNo and [Type of task]=1 and [Document No_] Like 'CAL%' )*2)) -"
            //            + " ((CASE WHEN SUBSTRING(b.MainJobID, 1, 1) = 'N' or SUBSTRING(b.MainJobID,1,1)= 'O' THEN "
            //            + " (select isnull(sum([Original Total Cost]) + sum([Line Amount]),0) FROM"+ Environment.GetEnvironmentVariable("Company") +"Job Ledger Entry] WHERE[Job No_] = {0} AND [Posting Date] > {1} AND [Posting Date] < {2} and SUBSTRING([Job Task No_],1,5) = b.JobTaskNo and [Type of task]=1)	"
            //            + " ELSE "
            //            + " (select isnull(sum([Original Total Cost]), 0) FROM"+ Environment.GetEnvironmentVariable("Company") +"Job Ledger Entry] WHERE[Job No_] = {0} AND [Posting Date] > {1} AND [Posting Date] < {2} and SUBSTRING([Job Task No_],1,5) = b.JobTaskNo and [Type of task]=1) END "
            //            + " )+ "
            //            + " (select isnull(sum([Original Total Cost]), 0) FROM "+ Environment.GetEnvironmentVariable("Company") +"Job Ledger Entry] WHERE [From Location] = {0} AND [Posting Date] > {1} AND [Posting Date] < {2} and SUBSTRING([Job Task No_],1,5) = b.JobTaskNo and [Type of task]=3 and [Job No_] <> {0} )))  AS ThisPeriodAMT "
            //            + " , "

            //            + " (((CASE WHEN SUBSTRING(b.MainJobID, 1, 1) = 'N' or SUBSTRING(b.MainJobID,1,1)= 'O' THEN "
            //            + " (select isnull(sum([Quantity]) + sum([Line Amount]),0) FROM"+ Environment.GetEnvironmentVariable("Company") +"Job Ledger Entry] WHERE [Job No_] = {0} AND [Posting Date] < {1} and SUBSTRING([Job Task No_],1,5) = b.JobTaskNo and [Type of task]=0 )	"
            //            + " ELSE "
            //            + " (select isnull(sum([Quantity]), 0) FROM"+ Environment.GetEnvironmentVariable("Company") +"Job Ledger Entry] WHERE[Job No_] = {0} AND [Posting Date] < {1} and SUBSTRING([Job Task No_],1,5) = b.JobTaskNo and [Type of task]=0) END "
            //            + " )+"
            //            + " (CASE WHEN SUBSTRING(b.MainJobID, 1, 1) = 'N' or SUBSTRING(b.MainJobID,1,1)= 'O' THEN "
            //            + " (select isnull(sum([Quantity]) + sum([Line Amount]),0) FROM"+ Environment.GetEnvironmentVariable("Company") +"Job Ledger Entry] WHERE[Job No_] = {0} AND [Posting Date] < {1} and SUBSTRING([Job Task No_],1,5) = b.JobTaskNo and [Type of task]=3 )	"
            //            + " ELSE "
            //            + "	(select isnull(sum([Quantity]), 0) FROM"+ Environment.GetEnvironmentVariable("Company") +"Job Ledger Entry] WHERE[Job No_] = {0} AND[Posting Date] < {1} and SUBSTRING([Job Task No_],1,5) = b.JobTaskNo and [Type of task]=3) END "
            //            + " )+"
            //            + " (CASE WHEN SUBSTRING(b.MainJobID, 1, 1) = 'N' or SUBSTRING(b.MainJobID,1,1)= 'O' THEN "
            //            + " (select isnull(sum([Quantity]) + sum([Line Amount]),0) FROM"+ Environment.GetEnvironmentVariable("Company") +"Job Ledger Entry] WHERE[Job No_] = {0} AND [Posting Date] < {1} and SUBSTRING([Job Task No_],1,5) = b.JobTaskNo and [Type of task]=2)	"
            //            + " ELSE "
            //            + " (select isnull(sum([Quantity]), 0) FROM"+ Environment.GetEnvironmentVariable("Company") +"Job Ledger Entry] WHERE[Job No_] = {0} AND [Posting Date] < {1} and SUBSTRING([Job Task No_],1,5) = b.JobTaskNo and [Type of task]=2) END "
            //            + " )"
            //            + "+((select isnull(sum([Quantity]), 0) FROM "+ Environment.GetEnvironmentVariable("Company") +"Job Ledger Entry] WHERE [From Location] = {0} AND [Posting Date] < {1}  and SUBSTRING([Job Task No_],1,5) = b.JobTaskNo and [Type of task]=1 and [Document No_] Like 'CAL%' )*2)) -"
            //            + " ((CASE WHEN SUBSTRING(b.MainJobID, 1, 1) = 'N' or SUBSTRING(b.MainJobID,1,1)= 'O' THEN "
            //            + " (select isnull(sum([Quantity]) + sum([Line Amount]),0) FROM"+ Environment.GetEnvironmentVariable("Company") +"Job Ledger Entry] WHERE[Job No_] = {0} AND [Posting Date] < {1} and SUBSTRING([Job Task No_],1,5) = b.JobTaskNo and [Type of task]=1)	"
            //            + " ELSE "
            //            + " (select isnull(sum([Quantity]), 0) FROM"+ Environment.GetEnvironmentVariable("Company") +"Job Ledger Entry] WHERE[Job No_] = {0} AND [Posting Date] < {1} and SUBSTRING([Job Task No_],1,5) = b.JobTaskNo and [Type of task]=1) END "
            //            + " )+ "
            //            + " (select isnull(sum([Quantity]), 0) FROM "+ Environment.GetEnvironmentVariable("Company") +"Job Ledger Entry] WHERE [From Location] = {0} AND [Posting Date] < {1} and SUBSTRING([Job Task No_],1,5) = b.JobTaskNo and [Type of task]=3 and [Job No_] <> {0} )))  AS OpeningQTY "
            //            + " , "

            //            + " (((CASE WHEN SUBSTRING(b.MainJobID, 1, 1) = 'N' or SUBSTRING(b.MainJobID,1,1)= 'O' THEN "
            //            + " (select isnull(sum([Original Total Cost]) + sum([Line Amount]),0) FROM"+ Environment.GetEnvironmentVariable("Company") +"Job Ledger Entry] WHERE [Job No_] = {0} AND  [Posting Date] < {1} and SUBSTRING([Job Task No_],1,5) = b.JobTaskNo and [Type of task]=0 )	"
            //            + " ELSE "
            //            + " (select isnull(sum([Original Total Cost]), 0) FROM"+ Environment.GetEnvironmentVariable("Company") +"Job Ledger Entry] WHERE[Job No_] = {0} AND [Posting Date] < {1} and SUBSTRING([Job Task No_],1,5) = b.JobTaskNo and [Type of task]=0) END "
            //            + " )+"
            //            + " (CASE WHEN SUBSTRING(b.MainJobID, 1, 1) = 'N' or SUBSTRING(b.MainJobID,1,1)= 'O' THEN "
            //            + " (select isnull(sum([Original Total Cost]) + sum([Line Amount]),0) FROM"+ Environment.GetEnvironmentVariable("Company") +"Job Ledger Entry] WHERE[Job No_] = {0} AND [Posting Date] < {1} and SUBSTRING([Job Task No_],1,5) = b.JobTaskNo and [Type of task]=3 )	"
            //            + " ELSE "
            //            + "	(select isnull(sum([Original Total Cost]), 0) FROM"+ Environment.GetEnvironmentVariable("Company") +"Job Ledger Entry] WHERE[Job No_] = {0} AND [Posting Date] < {1} and SUBSTRING([Job Task No_],1,5) = b.JobTaskNo and [Type of task]=3) END "
            //            + " )+"
            //            + " (CASE WHEN SUBSTRING(b.MainJobID, 1, 1) = 'N' or SUBSTRING(b.MainJobID,1,1)= 'O' THEN "
            //            + " (select isnull(sum([Original Total Cost]) + sum([Line Amount]),0) FROM"+ Environment.GetEnvironmentVariable("Company") +"Job Ledger Entry] WHERE[Job No_] = {0} AND [Posting Date] < {1} and SUBSTRING([Job Task No_],1,5) = b.JobTaskNo and [Type of task]=2)	"
            //            + " ELSE "
            //            + " (select isnull(sum([Original Total Cost]), 0) FROM"+ Environment.GetEnvironmentVariable("Company") +"Job Ledger Entry] WHERE[Job No_] = {0} AND [Posting Date] < {1} and SUBSTRING([Job Task No_],1,5) = b.JobTaskNo and [Type of task]=2) END "
            //            + " )"
            //            + " +((select isnull(sum([Original Total Cost]), 0) FROM "+ Environment.GetEnvironmentVariable("Company") +"Job Ledger Entry] WHERE [From Location] = {0} AND [Posting Date] < {1}  and SUBSTRING([Job Task No_],1,5) = b.JobTaskNo and [Type of task]=1 and [Document No_] Like 'CAL%' )*2)) -"
            //            + " ((CASE WHEN SUBSTRING(b.MainJobID, 1, 1) = 'N' or SUBSTRING(b.MainJobID,1,1)= 'O' THEN "
            //            + " (select isnull(sum([Original Total Cost]) + sum([Line Amount]),0) FROM"+ Environment.GetEnvironmentVariable("Company") +"Job Ledger Entry] WHERE[Job No_] = {0} AND [Posting Date] < {1} and SUBSTRING([Job Task No_],1,5) = b.JobTaskNo and [Type of task]=1)	"
            //            + " ELSE "
            //            + " (select isnull(sum([Original Total Cost]), 0) FROM"+ Environment.GetEnvironmentVariable("Company") +"Job Ledger Entry] WHERE[Job No_] = {0} AND [Posting Date] < {1} and SUBSTRING([Job Task No_],1,5) = b.JobTaskNo and [Type of task]=1) END "
            //            + " )+ "
            //            + " (select isnull(sum([Original Total Cost]), 0) FROM "+ Environment.GetEnvironmentVariable("Company") +"Job Ledger Entry] WHERE [From Location] = {0} AND [Posting Date] < {1} and SUBSTRING([Job Task No_],1,5) = b.JobTaskNo and [Type of task]=3 and [Job No_] <> {0} )))  AS OpeningAMT "
            //            + " , "

            //            + " (((CASE WHEN SUBSTRING(b.MainJobID, 1, 1) = 'N' or SUBSTRING(b.MainJobID,1,1)= 'O' THEN "
            //            + " (select isnull(sum([Quantity]) + sum([Line Amount]),0) FROM"+ Environment.GetEnvironmentVariable("Company") +"Job Ledger Entry] WHERE [Job No_] = {0} AND [Posting Date] < {2} and SUBSTRING([Job Task No_],1,5) = b.JobTaskNo and [Type of task]=0 )	"
            //            + " ELSE "
            //            + " (select isnull(sum([Quantity]), 0) FROM"+ Environment.GetEnvironmentVariable("Company") +"Job Ledger Entry] WHERE[Job No_] = {0} AND [Posting Date] < {2} and SUBSTRING([Job Task No_],1,5) = b.JobTaskNo and [Type of task]=0) END "
            //            + " )+"
            //            + " (CASE WHEN SUBSTRING(b.MainJobID, 1, 1) = 'N' or SUBSTRING(b.MainJobID,1,1)= 'O' THEN "
            //            + " (select isnull(sum([Quantity]) + sum([Line Amount]),0) FROM"+ Environment.GetEnvironmentVariable("Company") +"Job Ledger Entry] WHERE[Job No_] = {0} AND [Posting Date] < {2} and SUBSTRING([Job Task No_],1,5) = b.JobTaskNo and [Type of task]=3 )	"
            //            + " ELSE "
            //            + "	(select isnull(sum([Quantity]), 0) FROM"+ Environment.GetEnvironmentVariable("Company") +"Job Ledger Entry] WHERE[Job No_] = {0} AND[Posting Date] < {2} and SUBSTRING([Job Task No_],1,5) = b.JobTaskNo and [Type of task]=3) END "
            //            + " )+"
            //            + " (CASE WHEN SUBSTRING(b.MainJobID, 1, 1) = 'N' or SUBSTRING(b.MainJobID,1,1)= 'O' THEN "
            //            + " (select isnull(sum([Quantity]) + sum([Line Amount]),0) FROM"+ Environment.GetEnvironmentVariable("Company") +"Job Ledger Entry] WHERE[Job No_] = {0} AND [Posting Date] < {2} and SUBSTRING([Job Task No_],1,5) = b.JobTaskNo and [Type of task]=2)	"
            //            + " ELSE "
            //            + " (select isnull(sum([Quantity]), 0) FROM"+ Environment.GetEnvironmentVariable("Company") +"Job Ledger Entry] WHERE[Job No_] = {0} AND [Posting Date] < {2} and SUBSTRING([Job Task No_],1,5) = b.JobTaskNo and [Type of task]=2) END "
            //            + " )) -"
            //            + " ((CASE WHEN SUBSTRING(b.MainJobID, 1, 1) = 'N' or SUBSTRING(b.MainJobID,1,1)= 'O' THEN "
            //            + " (select isnull(sum([Quantity]) + sum([Line Amount]),0) FROM"+ Environment.GetEnvironmentVariable("Company") +"Job Ledger Entry] WHERE[Job No_] = {0} AND [Posting Date] < {2} and SUBSTRING([Job Task No_],1,5) = b.JobTaskNo and [Type of task]=1)	"
            //            + " ELSE "
            //            + " (select isnull(sum([Quantity]), 0) FROM"+ Environment.GetEnvironmentVariable("Company") +"Job Ledger Entry] WHERE[Job No_] = {0} AND [Posting Date] < {2} and SUBSTRING([Job Task No_],1,5) = b.JobTaskNo and [Type of task]=1) END "
            //            + " )+ "
            //            + " (select isnull(sum([Quantity]), 0) FROM "+ Environment.GetEnvironmentVariable("Company") +"Job Ledger Entry] WHERE [From Location] = {0} AND [Posting Date] < {2} and SUBSTRING([Job Task No_],1,5) = b.JobTaskNo and [Type of task]=3 and [Job No_] <> {0} )))  AS CumBalQTY "
            //            + " , "

            //              + " (((CASE WHEN SUBSTRING(b.MainJobID, 1, 1) = 'N' or SUBSTRING(b.MainJobID,1,1)= 'O' THEN "
            //              + " (select isnull(sum([Original Total Cost]) + sum([Line Amount]),0) FROM"+ Environment.GetEnvironmentVariable("Company") +"Job Ledger Entry] WHERE [Job No_] = {0} AND  [Posting Date] < {2} and SUBSTRING([Job Task No_],1,5) = b.JobTaskNo and [Type of task]=0 )	"
            //              + " ELSE "
            //              + " (select isnull(sum([Original Total Cost]), 0) FROM"+ Environment.GetEnvironmentVariable("Company") +"Job Ledger Entry] WHERE[Job No_] = {0} AND [Posting Date] < {2} and SUBSTRING([Job Task No_],1,5) = b.JobTaskNo and [Type of task]=0) END "
            //              + " )+"
            //              + " (CASE WHEN SUBSTRING(b.MainJobID, 1, 1) = 'N' or SUBSTRING(b.MainJobID,1,1)= 'O' THEN "
            //              + " (select isnull(sum([Original Total Cost]) + sum([Line Amount]),0) FROM"+ Environment.GetEnvironmentVariable("Company") +"Job Ledger Entry] WHERE[Job No_] = {0} AND [Posting Date] < {2} and SUBSTRING([Job Task No_],1,5) = b.JobTaskNo and [Type of task]=3 )	"
            //              + " ELSE "
            //              + "	(select isnull(sum([Original Total Cost]), 0) FROM"+ Environment.GetEnvironmentVariable("Company") +"Job Ledger Entry] WHERE[Job No_] = {0} AND [Posting Date] < {2} and SUBSTRING([Job Task No_],1,5) = b.JobTaskNo and [Type of task]=3) END "
            //              + " )+"
            //              + " (CASE WHEN SUBSTRING(b.MainJobID, 1, 1) = 'N' or SUBSTRING(b.MainJobID,1,1)= 'O' THEN "
            //              + " (select isnull(sum([Original Total Cost]) + sum([Line Amount]),0) FROM"+ Environment.GetEnvironmentVariable("Company") +"Job Ledger Entry] WHERE[Job No_] = {0} AND [Posting Date] < {2} and SUBSTRING([Job Task No_],1,5) = b.JobTaskNo and [Type of task]=2)	"
            //              + " ELSE "
            //              + " (select isnull(sum([Original Total Cost]), 0) FROM"+ Environment.GetEnvironmentVariable("Company") +"Job Ledger Entry] WHERE[Job No_] = {0} AND [Posting Date] < {2} and SUBSTRING([Job Task No_],1,5) = b.JobTaskNo and [Type of task]=2) END "
            //              + " )"
            //              + " +((select isnull(sum([Original Total Cost]), 0) FROM "+ Environment.GetEnvironmentVariable("Company") +"Job Ledger Entry] WHERE [From Location] = {0} AND [Posting Date] < {2}  and SUBSTRING([Job Task No_],1,5) = b.JobTaskNo and [Type of task]=1 and [Document No_] Like 'CAL%' )*2)) -"
            //              + " ((CASE WHEN SUBSTRING(b.MainJobID, 1, 1) = 'N' or SUBSTRING(b.MainJobID,1,1)= 'O' THEN "
            //              + " (select isnull(sum([Original Total Cost]) + sum([Line Amount]),0) FROM"+ Environment.GetEnvironmentVariable("Company") +"Job Ledger Entry] WHERE[Job No_] = {0} AND [Posting Date] < {2} and SUBSTRING([Job Task No_],1,5) = b.JobTaskNo and [Type of task]=1)	"
            //              + " ELSE "
            //              + " (select isnull(sum([Original Total Cost]), 0) FROM"+ Environment.GetEnvironmentVariable("Company") +"Job Ledger Entry] WHERE[Job No_] = {0} AND [Posting Date] < {2} and SUBSTRING([Job Task No_],1,5) = b.JobTaskNo and [Type of task]=1) END "
            //              + " )+ "
            //              + " (select isnull(sum([Original Total Cost]), 0) FROM "+ Environment.GetEnvironmentVariable("Company") +"Job Ledger Entry] WHERE [From Location] = {0} AND [Posting Date] < {2} and SUBSTRING([Job Task No_],1,5) = b.JobTaskNo and [Type of task]=3 and [Job No_] <> {0} )))  AS CumBalAMT "

            //            //  ///กรณีนำ GL
            //            //  + " (SELECT isnull(SUM([Amount]),0) as Total FROM dbo."+ Environment.GetEnvironmentVariable("Company") +"Analysis View Entry] WHERE [Analysis View Code]='JOB COST A' and Convert(Time,[Posting Date])<>'23:59:59'  and [Posting Date] <={2} and [Dimension 1 Value Code]={0} and [Dimension 2 Value Code]<>'' and [Dimension 2 Value Code]=b.JobTaskNo) as CumBalAMT"
            //            /////


            //            + " , "

            //            + " (SELECT isnull(sum(Quantity), 0) FROM dbo."+ Environment.GetEnvironmentVariable("Company") +"Job Planning Line] WHERE[Job No_]={0} and SUBSTRING([Job Task No_],1,5)=b.JobTaskNo) as BudgetQTY, "
            //            + " (SELECT isnull(sum([Total Cost (LCY)]),0) FROM dbo."+ Environment.GetEnvironmentVariable("Company") +"Job Planning Line] WHERE[Job No_]= {0} and SUBSTRING([Job Task No_],1,5)= b.JobTaskNo) as BudgetAMT "

            //            + " FROM("
            //             + " SELECT a.JobTaskNo,a.MainJobID,a.SubJobID,"
            //             + " (SELECT TOP 1 dbo."+ Environment.GetEnvironmentVariable("Company") +"Job Task].Description FROM dbo."+ Environment.GetEnvironmentVariable("Company") +"Job Task] WHERE [Job Task No_]=a.JobTaskNo) as Description,"
            //             + " (SELECT TOP 1 ([Job Task No_]+' '+Description) FROM dbo."+ Environment.GetEnvironmentVariable("Company") +"Job Task] WHERE [Job Task No_]=a.JobTaskNo) as DescriptionFull,"
            //             + " (SELECT TOP 1 dbo."+ Environment.GetEnvironmentVariable("Company") +"Job Task].Description FROM dbo."+ Environment.GetEnvironmentVariable("Company") +"Job Task] WHERE SUBSTRING(dbo."+ Environment.GetEnvironmentVariable("Company") +"Job Task].[Job Task No_],1,1)+'0000'=a.MainJobID) as DescriptionMainJob,"
            //             + " (SELECT TOP 1 dbo."+ Environment.GetEnvironmentVariable("Company") +"Job Task].Description FROM dbo."+ Environment.GetEnvironmentVariable("Company") +"Job Task] WHERE SUBSTRING(dbo."+ Environment.GetEnvironmentVariable("Company") +"Job Task].[Job Task No_],1,3)+'00'=a.SubJobID) as DescriptionSubJob"
            //             + " FROM  (SELECT DISTINCT dbo."+ Environment.GetEnvironmentVariable("Company") +"Job Task].[Job Task No_] as JobTaskNo,"
            //             + " SUBSTRING(dbo."+ Environment.GetEnvironmentVariable("Company") +"Job Task].[Job Task No_],1,1)+'0000' as MainJobID,SUBSTRING(dbo."+ Environment.GetEnvironmentVariable("Company") +"Job Task].[Job Task No_],1,3)+'00' as SubJobID FROM dbo."+ Environment.GetEnvironmentVariable("Company") +"Job Task]) as a) as b ORDER BY b.JobTaskNo";



            //var queryData = " SELECT b.MainJobID,b.DescriptionMainJob,b.SubJobID,b.DescriptionSubJob,b.JobTaskNo,b.DescriptionFull,"
            //                + " (((CASE WHEN SUBSTRING(b.MainJobID, 1, 1) = 'N' or SUBSTRING(b.MainJobID,1,1)= 'O' THEN "
            //                + " (select isnull(sum([Quantity]) + sum([Line Amount]),0) FROM"+ Environment.GetEnvironmentVariable("Company") +"Job Ledger Entry] WHERE [Job No_] = {0} AND [Posting Date] > {1} AND [Posting Date] < {2} and SUBSTRING([Job Task No_],1,5) = b.JobTaskNo and [Type of task]=0 )	"
            //                + " ELSE "
            //                + " (select isnull(sum([Quantity]), 0) FROM"+ Environment.GetEnvironmentVariable("Company") +"Job Ledger Entry] WHERE[Job No_] = {0} AND [Posting Date] > {1} AND [Posting Date] < {2} and SUBSTRING([Job Task No_],1,5) = b.JobTaskNo and [Type of task]=0) END "
            //                + " )+"
            //                + " (CASE WHEN SUBSTRING(b.MainJobID, 1, 1) = 'N' or SUBSTRING(b.MainJobID,1,1)= 'O' THEN "
            //                + " (select isnull(sum([Quantity]) + sum([Line Amount]),0) FROM"+ Environment.GetEnvironmentVariable("Company") +"Job Ledger Entry] WHERE[Job No_] = {0} AND [Posting Date] > {1} AND [Posting Date] < {2} and SUBSTRING([Job Task No_],1,5) = b.JobTaskNo and [Type of task]=3 )	"
            //                + " ELSE "
            //                + "	(select isnull(sum([Quantity]), 0) FROM"+ Environment.GetEnvironmentVariable("Company") +"Job Ledger Entry] WHERE[Job No_] = {0} AND [Posting Date] > {1} AND [Posting Date] < {2} and SUBSTRING([Job Task No_],1,5) = b.JobTaskNo and [Type of task]=3) END "
            //                + " )+"
            //                + " (CASE WHEN SUBSTRING(b.MainJobID, 1, 1) = 'N' or SUBSTRING(b.MainJobID,1,1)= 'O' THEN "
            //                + " (select isnull(sum([Quantity]) + sum([Line Amount]),0) FROM"+ Environment.GetEnvironmentVariable("Company") +"Job Ledger Entry] WHERE[Job No_] = {0} AND [Posting Date] > {1} AND [Posting Date] < {2} and SUBSTRING([Job Task No_],1,5) = b.JobTaskNo and [Type of task]=2)	"
            //                + " ELSE "
            //                + " (select isnull(sum([Quantity]), 0) FROM"+ Environment.GetEnvironmentVariable("Company") +"Job Ledger Entry] WHERE[Job No_] = {0} AND [Posting Date] > {1} AND [Posting Date] < {2} and SUBSTRING([Job Task No_],1,5) = b.JobTaskNo and [Type of task]=2) END "
            //                + " )" +
            //                "+((select isnull(sum([Quantity]), 0) FROM "+ Environment.GetEnvironmentVariable("Company") +"Job Ledger Entry] WHERE [From Location] = {0} AND [Posting Date] > {1} AND [Posting Date] < {2} and SUBSTRING([Job Task No_],1,5) = b.JobTaskNo and [Type of task]=1 and [Document No_] Like 'CAL%' )*2)) -"
            //                + " ((CASE WHEN SUBSTRING(b.MainJobID, 1, 1) = 'N' or SUBSTRING(b.MainJobID,1,1)= 'O' THEN "
            //                + " (select isnull(sum([Quantity]) + sum([Line Amount]),0) FROM"+ Environment.GetEnvironmentVariable("Company") +"Job Ledger Entry] WHERE[Job No_] = {0} AND [Posting Date] > {1} AND [Posting Date] < {2} and SUBSTRING([Job Task No_],1,5) = b.JobTaskNo and [Type of task]=1)	"
            //                + " ELSE "
            //                + " (select isnull(sum([Quantity]), 0) FROM"+ Environment.GetEnvironmentVariable("Company") +"Job Ledger Entry] WHERE[Job No_] = {0} AND [Posting Date] > {1} AND [Posting Date] < {2} and SUBSTRING([Job Task No_],1,5) = b.JobTaskNo and [Type of task]=1) END "
            //                + " )+ "
            //                + " (select isnull(sum([Quantity]), 0) FROM "+ Environment.GetEnvironmentVariable("Company") +"Job Ledger Entry] WHERE [From Location] = {0} AND [Posting Date] > {1} AND [Posting Date] < {2} and SUBSTRING([Job Task No_],1,5) = b.JobTaskNo and [Type of task]=3 and [Job No_] <> {0} )))  AS ThisPeriodQTY "
            //                + " , "

            //                + " (((CASE WHEN SUBSTRING(b.MainJobID, 1, 1) = 'N' or SUBSTRING(b.MainJobID,1,1)= 'O' THEN "
            //                + " (select isnull(sum([Total Cost]) + sum([Line Amount]),0) FROM"+ Environment.GetEnvironmentVariable("Company") +"Job Ledger Entry] WHERE [Job No_] = {0} AND [Posting Date] > {1} AND [Posting Date] < {2} and SUBSTRING([Job Task No_],1,5) = b.JobTaskNo and [Type of task]=0 )	"
            //                + " ELSE "
            //                + " (select isnull(sum([Total Cost]), 0) FROM"+ Environment.GetEnvironmentVariable("Company") +"Job Ledger Entry] WHERE[Job No_] = {0} AND [Posting Date] > {1} AND [Posting Date] < {2} and SUBSTRING([Job Task No_],1,5) = b.JobTaskNo and [Type of task]=0) END "
            //                + " )+"
            //                + " (CASE WHEN SUBSTRING(b.MainJobID, 1, 1) = 'N' or SUBSTRING(b.MainJobID,1,1)= 'O' THEN "
            //                + " (select isnull(sum([Total Cost]) + sum([Line Amount]),0) FROM"+ Environment.GetEnvironmentVariable("Company") +"Job Ledger Entry] WHERE[Job No_] = {0} AND [Posting Date] > {1} AND [Posting Date] < {2} and SUBSTRING([Job Task No_],1,5) = b.JobTaskNo and [Type of task]=3 )	"
            //                + " ELSE "
            //                + "	(select isnull(sum([Total Cost]), 0) FROM"+ Environment.GetEnvironmentVariable("Company") +"Job Ledger Entry] WHERE[Job No_] = {0} AND [Posting Date] > {1} AND [Posting Date] < {2} and SUBSTRING([Job Task No_],1,5) = b.JobTaskNo and [Type of task]=3) END "
            //                + " )+"
            //                + " (CASE WHEN SUBSTRING(b.MainJobID, 1, 1) = 'N' or SUBSTRING(b.MainJobID,1,1)= 'O' THEN "
            //                + " (select isnull(sum([Total Cost]) + sum([Line Amount]),0) FROM"+ Environment.GetEnvironmentVariable("Company") +"Job Ledger Entry] WHERE[Job No_] = {0} AND [Posting Date] > {1} AND [Posting Date] < {2} and SUBSTRING([Job Task No_],1,5) = b.JobTaskNo and [Type of task]=2)	"
            //                + " ELSE "
            //                + " (select isnull(sum([Total Cost]), 0) FROM"+ Environment.GetEnvironmentVariable("Company") +"Job Ledger Entry] WHERE[Job No_] = {0} AND [Posting Date] > {1} AND [Posting Date] < {2} and SUBSTRING([Job Task No_],1,5) = b.JobTaskNo and [Type of task]=2) END "
            //                + " )"
            //                + "+((select isnull(sum([Total Cost]), 0) FROM "+ Environment.GetEnvironmentVariable("Company") +"Job Ledger Entry] WHERE [From Location] = {0} AND [Posting Date] > {1} AND [Posting Date] < {2} and SUBSTRING([Job Task No_],1,5) = b.JobTaskNo and [Type of task]=1 and [Document No_] Like 'CAL%' )*2)) -"
            //                + " ((CASE WHEN SUBSTRING(b.MainJobID, 1, 1) = 'N' or SUBSTRING(b.MainJobID,1,1)= 'O' THEN "
            //                + " (select isnull(sum([Total Cost]) + sum([Line Amount]),0) FROM"+ Environment.GetEnvironmentVariable("Company") +"Job Ledger Entry] WHERE[Job No_] = {0} AND [Posting Date] > {1} AND [Posting Date] < {2} and SUBSTRING([Job Task No_],1,5) = b.JobTaskNo and [Type of task]=1)	"
            //                + " ELSE "
            //                + " (select isnull(sum([Total Cost]), 0) FROM"+ Environment.GetEnvironmentVariable("Company") +"Job Ledger Entry] WHERE[Job No_] = {0} AND [Posting Date] > {1} AND [Posting Date] < {2} and SUBSTRING([Job Task No_],1,5) = b.JobTaskNo and [Type of task]=1) END "
            //                + " )+ "
            //                + " (select isnull(sum([Total Cost]), 0) FROM "+ Environment.GetEnvironmentVariable("Company") +"Job Ledger Entry] WHERE [From Location] = {0} AND [Posting Date] > {1} AND [Posting Date] < {2} and SUBSTRING([Job Task No_],1,5) = b.JobTaskNo and [Type of task]=3 and [Job No_] <> {0} )))  AS ThisPeriodAMT "
            //                + " , "

            //                + " (((CASE WHEN SUBSTRING(b.MainJobID, 1, 1) = 'N' or SUBSTRING(b.MainJobID,1,1)= 'O' THEN "
            //                + " (select isnull(sum([Quantity]) + sum([Line Amount]),0) FROM"+ Environment.GetEnvironmentVariable("Company") +"Job Ledger Entry] WHERE [Job No_] = {0} AND [Posting Date] < {1} and SUBSTRING([Job Task No_],1,5) = b.JobTaskNo and [Type of task]=0 )	"
            //                + " ELSE "
            //                + " (select isnull(sum([Quantity]), 0) FROM"+ Environment.GetEnvironmentVariable("Company") +"Job Ledger Entry] WHERE[Job No_] = {0} AND [Posting Date] < {1} and SUBSTRING([Job Task No_],1,5) = b.JobTaskNo and [Type of task]=0) END "
            //                + " )+"
            //                + " (CASE WHEN SUBSTRING(b.MainJobID, 1, 1) = 'N' or SUBSTRING(b.MainJobID,1,1)= 'O' THEN "
            //                + " (select isnull(sum([Quantity]) + sum([Line Amount]),0) FROM"+ Environment.GetEnvironmentVariable("Company") +"Job Ledger Entry] WHERE[Job No_] = {0} AND [Posting Date] < {1} and SUBSTRING([Job Task No_],1,5) = b.JobTaskNo and [Type of task]=3 )	"
            //                + " ELSE "
            //                + "	(select isnull(sum([Quantity]), 0) FROM"+ Environment.GetEnvironmentVariable("Company") +"Job Ledger Entry] WHERE[Job No_] = {0} AND[Posting Date] < {1} and SUBSTRING([Job Task No_],1,5) = b.JobTaskNo and [Type of task]=3) END "
            //                + " )+"
            //                + " (CASE WHEN SUBSTRING(b.MainJobID, 1, 1) = 'N' or SUBSTRING(b.MainJobID,1,1)= 'O' THEN "
            //                + " (select isnull(sum([Quantity]) + sum([Line Amount]),0) FROM"+ Environment.GetEnvironmentVariable("Company") +"Job Ledger Entry] WHERE[Job No_] = {0} AND [Posting Date] < {1} and SUBSTRING([Job Task No_],1,5) = b.JobTaskNo and [Type of task]=2)	"
            //                + " ELSE "
            //                + " (select isnull(sum([Quantity]), 0) FROM"+ Environment.GetEnvironmentVariable("Company") +"Job Ledger Entry] WHERE[Job No_] = {0} AND [Posting Date] < {1} and SUBSTRING([Job Task No_],1,5) = b.JobTaskNo and [Type of task]=2) END "
            //                + " )"
            //                + "+((select isnull(sum([Quantity]), 0) FROM "+ Environment.GetEnvironmentVariable("Company") +"Job Ledger Entry] WHERE [From Location] = {0} AND [Posting Date] < {1}  and SUBSTRING([Job Task No_],1,5) = b.JobTaskNo and [Type of task]=1 and [Document No_] Like 'CAL%' )*2)) -"
            //                + " ((CASE WHEN SUBSTRING(b.MainJobID, 1, 1) = 'N' or SUBSTRING(b.MainJobID,1,1)= 'O' THEN "
            //                + " (select isnull(sum([Quantity]) + sum([Line Amount]),0) FROM"+ Environment.GetEnvironmentVariable("Company") +"Job Ledger Entry] WHERE[Job No_] = {0} AND [Posting Date] < {1} and SUBSTRING([Job Task No_],1,5) = b.JobTaskNo and [Type of task]=1)	"
            //                + " ELSE "
            //                + " (select isnull(sum([Quantity]), 0) FROM"+ Environment.GetEnvironmentVariable("Company") +"Job Ledger Entry] WHERE[Job No_] = {0} AND [Posting Date] < {1} and SUBSTRING([Job Task No_],1,5) = b.JobTaskNo and [Type of task]=1) END "
            //                + " )+ "
            //                + " (select isnull(sum([Quantity]), 0) FROM "+ Environment.GetEnvironmentVariable("Company") +"Job Ledger Entry] WHERE [From Location] = {0} AND [Posting Date] < {1} and SUBSTRING([Job Task No_],1,5) = b.JobTaskNo and [Type of task]=3 and [Job No_] <> {0} )))  AS OpeningQTY "
            //                + " , "

            //                + " (((CASE WHEN SUBSTRING(b.MainJobID, 1, 1) = 'N' or SUBSTRING(b.MainJobID,1,1)= 'O' THEN "
            //                + " (select isnull(sum([Total Cost]) + sum([Line Amount]),0) FROM"+ Environment.GetEnvironmentVariable("Company") +"Job Ledger Entry] WHERE [Job No_] = {0} AND  [Posting Date] < {1} and SUBSTRING([Job Task No_],1,5) = b.JobTaskNo and [Type of task]=0 )	"
            //                + " ELSE "
            //                + " (select isnull(sum([Total Cost]), 0) FROM"+ Environment.GetEnvironmentVariable("Company") +"Job Ledger Entry] WHERE[Job No_] = {0} AND [Posting Date] < {1} and SUBSTRING([Job Task No_],1,5) = b.JobTaskNo and [Type of task]=0) END "
            //                + " )+"
            //                + " (CASE WHEN SUBSTRING(b.MainJobID, 1, 1) = 'N' or SUBSTRING(b.MainJobID,1,1)= 'O' THEN "
            //                + " (select isnull(sum([Total Cost]) + sum([Line Amount]),0) FROM"+ Environment.GetEnvironmentVariable("Company") +"Job Ledger Entry] WHERE[Job No_] = {0} AND [Posting Date] < {1} and SUBSTRING([Job Task No_],1,5) = b.JobTaskNo and [Type of task]=3 )	"
            //                + " ELSE "
            //                + "	(select isnull(sum([Total Cost]), 0) FROM"+ Environment.GetEnvironmentVariable("Company") +"Job Ledger Entry] WHERE[Job No_] = {0} AND [Posting Date] < {1} and SUBSTRING([Job Task No_],1,5) = b.JobTaskNo and [Type of task]=3) END "
            //                + " )+"
            //                + " (CASE WHEN SUBSTRING(b.MainJobID, 1, 1) = 'N' or SUBSTRING(b.MainJobID,1,1)= 'O' THEN "
            //                + " (select isnull(sum([Total Cost]) + sum([Line Amount]),0) FROM"+ Environment.GetEnvironmentVariable("Company") +"Job Ledger Entry] WHERE[Job No_] = {0} AND [Posting Date] < {1} and SUBSTRING([Job Task No_],1,5) = b.JobTaskNo and [Type of task]=2)	"
            //                + " ELSE "
            //                + " (select isnull(sum([Total Cost]), 0) FROM"+ Environment.GetEnvironmentVariable("Company") +"Job Ledger Entry] WHERE[Job No_] = {0} AND [Posting Date] < {1} and SUBSTRING([Job Task No_],1,5) = b.JobTaskNo and [Type of task]=2) END "
            //                + " )"
            //                + " +((select isnull(sum([Total Cost]), 0) FROM "+ Environment.GetEnvironmentVariable("Company") +"Job Ledger Entry] WHERE [From Location] = {0} AND [Posting Date] < {1}  and SUBSTRING([Job Task No_],1,5) = b.JobTaskNo and [Type of task]=1 and [Document No_] Like 'CAL%' )*2)) -"
            //                + " ((CASE WHEN SUBSTRING(b.MainJobID, 1, 1) = 'N' or SUBSTRING(b.MainJobID,1,1)= 'O' THEN "
            //                + " (select isnull(sum([Total Cost]) + sum([Line Amount]),0) FROM"+ Environment.GetEnvironmentVariable("Company") +"Job Ledger Entry] WHERE[Job No_] = {0} AND [Posting Date] < {1} and SUBSTRING([Job Task No_],1,5) = b.JobTaskNo and [Type of task]=1)	"
            //                + " ELSE "
            //                + " (select isnull(sum([Total Cost]), 0) FROM"+ Environment.GetEnvironmentVariable("Company") +"Job Ledger Entry] WHERE[Job No_] = {0} AND [Posting Date] < {1} and SUBSTRING([Job Task No_],1,5) = b.JobTaskNo and [Type of task]=1) END "
            //                + " )+ "
            //                + " (select isnull(sum([Total Cost]), 0) FROM "+ Environment.GetEnvironmentVariable("Company") +"Job Ledger Entry] WHERE [From Location] = {0} AND [Posting Date] < {1} and SUBSTRING([Job Task No_],1,5) = b.JobTaskNo and [Type of task]=3 and [Job No_] <> {0} )))  AS OpeningAMT "
            //                + " , "

            //                + " (((CASE WHEN SUBSTRING(b.MainJobID, 1, 1) = 'N' or SUBSTRING(b.MainJobID,1,1)= 'O' THEN "
            //                + " (select isnull(sum([Quantity]) + sum([Line Amount]),0) FROM"+ Environment.GetEnvironmentVariable("Company") +"Job Ledger Entry] WHERE [Job No_] = {0} AND [Posting Date] < {2} and SUBSTRING([Job Task No_],1,5) = b.JobTaskNo and [Type of task]=0 )	"
            //                + " ELSE "
            //                + " (select isnull(sum([Quantity]), 0) FROM"+ Environment.GetEnvironmentVariable("Company") +"Job Ledger Entry] WHERE[Job No_] = {0} AND [Posting Date] < {2} and SUBSTRING([Job Task No_],1,5) = b.JobTaskNo and [Type of task]=0) END "
            //                + " )+"
            //                + " (CASE WHEN SUBSTRING(b.MainJobID, 1, 1) = 'N' or SUBSTRING(b.MainJobID,1,1)= 'O' THEN "
            //                + " (select isnull(sum([Quantity]) + sum([Line Amount]),0) FROM"+ Environment.GetEnvironmentVariable("Company") +"Job Ledger Entry] WHERE[Job No_] = {0} AND [Posting Date] < {2} and SUBSTRING([Job Task No_],1,5) = b.JobTaskNo and [Type of task]=3 )	"
            //                + " ELSE "
            //                + "	(select isnull(sum([Quantity]), 0) FROM"+ Environment.GetEnvironmentVariable("Company") +"Job Ledger Entry] WHERE[Job No_] = {0} AND[Posting Date] < {2} and SUBSTRING([Job Task No_],1,5) = b.JobTaskNo and [Type of task]=3) END "
            //                + " )+"
            //                + " (CASE WHEN SUBSTRING(b.MainJobID, 1, 1) = 'N' or SUBSTRING(b.MainJobID,1,1)= 'O' THEN "
            //                + " (select isnull(sum([Quantity]) + sum([Line Amount]),0) FROM"+ Environment.GetEnvironmentVariable("Company") +"Job Ledger Entry] WHERE[Job No_] = {0} AND [Posting Date] < {2} and SUBSTRING([Job Task No_],1,5) = b.JobTaskNo and [Type of task]=2)	"
            //                + " ELSE "
            //                + " (select isnull(sum([Quantity]), 0) FROM"+ Environment.GetEnvironmentVariable("Company") +"Job Ledger Entry] WHERE[Job No_] = {0} AND [Posting Date] < {2} and SUBSTRING([Job Task No_],1,5) = b.JobTaskNo and [Type of task]=2) END "
            //                + " )) -"
            //                + " ((CASE WHEN SUBSTRING(b.MainJobID, 1, 1) = 'N' or SUBSTRING(b.MainJobID,1,1)= 'O' THEN "
            //                + " (select isnull(sum([Quantity]) + sum([Line Amount]),0) FROM"+ Environment.GetEnvironmentVariable("Company") +"Job Ledger Entry] WHERE[Job No_] = {0} AND [Posting Date] < {2} and SUBSTRING([Job Task No_],1,5) = b.JobTaskNo and [Type of task]=1)	"
            //                + " ELSE "
            //                + " (select isnull(sum([Quantity]), 0) FROM"+ Environment.GetEnvironmentVariable("Company") +"Job Ledger Entry] WHERE[Job No_] = {0} AND [Posting Date] < {2} and SUBSTRING([Job Task No_],1,5) = b.JobTaskNo and [Type of task]=1) END "
            //                + " )+ "
            //                + " (select isnull(sum([Quantity]), 0) FROM "+ Environment.GetEnvironmentVariable("Company") +"Job Ledger Entry] WHERE [From Location] = {0} AND [Posting Date] < {2} and SUBSTRING([Job Task No_],1,5) = b.JobTaskNo and [Type of task]=3 and [Job No_] <> {0} )))  AS CumBalQTY "
            //                + " , "

            //                  + " (((CASE WHEN SUBSTRING(b.MainJobID, 1, 1) = 'N' or SUBSTRING(b.MainJobID,1,1)= 'O' THEN "
            //                  + " (select isnull(sum([Total Cost]) + sum([Line Amount]),0) FROM"+ Environment.GetEnvironmentVariable("Company") +"Job Ledger Entry] WHERE [Job No_] = {0} AND  [Posting Date] < {2} and SUBSTRING([Job Task No_],1,5) = b.JobTaskNo and [Type of task]=0 )	"
            //                  + " ELSE "
            //                  + " (select isnull(sum([Total Cost]), 0) FROM"+ Environment.GetEnvironmentVariable("Company") +"Job Ledger Entry] WHERE[Job No_] = {0} AND [Posting Date] < {2} and SUBSTRING([Job Task No_],1,5) = b.JobTaskNo and [Type of task]=0) END "
            //                  + " )+"
            //                  + " (CASE WHEN SUBSTRING(b.MainJobID, 1, 1) = 'N' or SUBSTRING(b.MainJobID,1,1)= 'O' THEN "
            //                  + " (select isnull(sum([Total Cost]) + sum([Line Amount]),0) FROM"+ Environment.GetEnvironmentVariable("Company") +"Job Ledger Entry] WHERE[Job No_] = {0} AND [Posting Date] < {2} and SUBSTRING([Job Task No_],1,5) = b.JobTaskNo and [Type of task]=3 )	"
            //                  + " ELSE "
            //                  + "	(select isnull(sum([Total Cost]), 0) FROM"+ Environment.GetEnvironmentVariable("Company") +"Job Ledger Entry] WHERE[Job No_] = {0} AND [Posting Date] < {2} and SUBSTRING([Job Task No_],1,5) = b.JobTaskNo and [Type of task]=3) END "
            //                  + " )+"
            //                  + " (CASE WHEN SUBSTRING(b.MainJobID, 1, 1) = 'N' or SUBSTRING(b.MainJobID,1,1)= 'O' THEN "
            //                  + " (select isnull(sum([Total Cost]) + sum([Line Amount]),0) FROM"+ Environment.GetEnvironmentVariable("Company") +"Job Ledger Entry] WHERE[Job No_] = {0} AND [Posting Date] < {2} and SUBSTRING([Job Task No_],1,5) = b.JobTaskNo and [Type of task]=2)	"
            //                  + " ELSE "
            //                  + " (select isnull(sum([Total Cost]), 0) FROM"+ Environment.GetEnvironmentVariable("Company") +"Job Ledger Entry] WHERE[Job No_] = {0} AND [Posting Date] < {2} and SUBSTRING([Job Task No_],1,5) = b.JobTaskNo and [Type of task]=2) END "
            //                  + " )"
            //                  + " +((select isnull(sum([Total Cost]), 0) FROM "+ Environment.GetEnvironmentVariable("Company") +"Job Ledger Entry] WHERE [From Location] = {0} AND [Posting Date] < {2}  and SUBSTRING([Job Task No_],1,5) = b.JobTaskNo and [Type of task]=1 and [Document No_] Like 'CAL%' )*2)) -"
            //                  + " ((CASE WHEN SUBSTRING(b.MainJobID, 1, 1) = 'N' or SUBSTRING(b.MainJobID,1,1)= 'O' THEN "
            //                  + " (select isnull(sum([Total Cost]) + sum([Line Amount]),0) FROM"+ Environment.GetEnvironmentVariable("Company") +"Job Ledger Entry] WHERE[Job No_] = {0} AND [Posting Date] < {2} and SUBSTRING([Job Task No_],1,5) = b.JobTaskNo and [Type of task]=1)	"
            //                  + " ELSE "
            //                  + " (select isnull(sum([Total Cost]), 0) FROM"+ Environment.GetEnvironmentVariable("Company") +"Job Ledger Entry] WHERE[Job No_] = {0} AND [Posting Date] < {2} and SUBSTRING([Job Task No_],1,5) = b.JobTaskNo and [Type of task]=1) END "
            //                  + " )+ "
            //                  + " (select isnull(sum([Total Cost]), 0) FROM "+ Environment.GetEnvironmentVariable("Company") +"Job Ledger Entry] WHERE [From Location] = {0} AND [Posting Date] < {2} and SUBSTRING([Job Task No_],1,5) = b.JobTaskNo and [Type of task]=3 and [Job No_] <> {0} )))  AS CumBalAMT "



            //                  /////กรณีนำ GL
            //                  //+ " (SELECT isnull(SUM([Amount]),0) as Total FROM dbo."+ Environment.GetEnvironmentVariable("Company") +"Analysis View Entry] WHERE [Analysis View Code]='JOB COST A' and Convert(Time,[Posting Date])<>'23:59:59'  and [Posting Date] <={2} and [Dimension 1 Value Code]={0} and [Dimension 2 Value Code]<>'' and [Dimension 2 Value Code]=b.JobTaskNo) as CumBalAMT"
            //                  /////



            //                + " , "

            //                + " (SELECT isnull(sum(Quantity), 0) FROM dbo."+ Environment.GetEnvironmentVariable("Company") +"Job Planning Line] WHERE[Job No_]={0} and SUBSTRING([Job Task No_],1,5)=b.JobTaskNo) as BudgetQTY, "
            //                + " (SELECT isnull(sum([Total Cost (LCY)]),0) FROM dbo."+ Environment.GetEnvironmentVariable("Company") +"Job Planning Line] WHERE[Job No_]= {0} and SUBSTRING([Job Task No_],1,5)= b.JobTaskNo) as BudgetAMT "

            //                + " FROM("
            //                 + " SELECT a.JobTaskNo,a.MainJobID,a.SubJobID,"
            //                 + " (SELECT TOP 1 dbo."+ Environment.GetEnvironmentVariable("Company") +"Job Task].Description FROM dbo."+ Environment.GetEnvironmentVariable("Company") +"Job Task] WHERE [Job Task No_]=a.JobTaskNo) as Description,"
            //                 + " (SELECT TOP 1 ([Job Task No_]+' '+Description) FROM dbo."+ Environment.GetEnvironmentVariable("Company") +"Job Task] WHERE [Job Task No_]=a.JobTaskNo) as DescriptionFull,"
            //                 + " (SELECT TOP 1 dbo."+ Environment.GetEnvironmentVariable("Company") +"Job Task].Description FROM dbo."+ Environment.GetEnvironmentVariable("Company") +"Job Task] WHERE SUBSTRING(dbo."+ Environment.GetEnvironmentVariable("Company") +"Job Task].[Job Task No_],1,1)+'0000'=a.MainJobID) as DescriptionMainJob,"
            //                 + " (SELECT TOP 1 dbo."+ Environment.GetEnvironmentVariable("Company") +"Job Task].Description FROM dbo."+ Environment.GetEnvironmentVariable("Company") +"Job Task] WHERE SUBSTRING(dbo."+ Environment.GetEnvironmentVariable("Company") +"Job Task].[Job Task No_],1,3)+'00'=a.SubJobID) as DescriptionSubJob"
            //                 + " FROM  (SELECT DISTINCT dbo."+ Environment.GetEnvironmentVariable("Company") +"Job Task].[Job Task No_] as JobTaskNo,"
            //                 + " SUBSTRING(dbo."+ Environment.GetEnvironmentVariable("Company") +"Job Task].[Job Task No_],1,1)+'0000' as MainJobID,SUBSTRING(dbo."+ Environment.GetEnvironmentVariable("Company") +"Job Task].[Job Task No_],1,3)+'00' as SubJobID FROM dbo."+ Environment.GetEnvironmentVariable("Company") +"Job Task]) as a) as b ORDER BY b.JobTaskNo";


            //SqlParameter parameterJob = new SqlParameter("{0}", JobNo);
            //SqlParameter parameterStart = new SqlParameter("{1}", date1);
            //SqlParameter parameterDate1 = new SqlParameter("{2}", date2);




            //ViewBag.sql = queryData;
            var v_ReportJobCost = _navcontext.V_ReportJobCost.FromSqlRaw(queryData, JobNo, date1, date2).ToList();
            ViewData["v_ReportJobCost"] = v_ReportJobCost;
            //string totalview = "";
            //string headtable = "";
            //string table = "";
            //string main = "";
            //string sub = "";
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
            var G0210N = "";
            var G0210P = "";

            List<Estimate> estimates = new List<Estimate> { };

            List<TableJobCost> TableJobCosts = new List<TableJobCost>{};

            //headtable = "<table  id='JobcostDetail' ><thead><tr>"
            //    + "<th align ='center'>JobTask</th>"
            //    + "<th align ='center'>ThisPeriodQTY</th>"
            //    + "<th align ='center'>ThisPeriodAMT</th>"
            //    + "<th align ='center'>OpenningQTY</th>"
            //    + "<th align ='center'>OpenningAMT</th>"
            //    + "<th align ='center'>CumBalQTY</th>"
            //    + "<th align ='center'>CumBalAMT</th>"
            //    + "<th align ='center'>BudgetQty</th>"
            //    + "<th align ='center'>%</th>"
            //    + "<th align ='center'>BudgetAMT</th>"
            //    + "<th align ='center'>%</th>"
            //    + "</tr>"
            //    + "</thead>"
            //    + "<tbody id='myTable'>"; ;




            foreach (var std in v_ReportJobCost as IList<V_ReportJobCost>)
            {



                //Main
                if (mainID != std.MainJobID)
                {
                    mainID = std.MainJobID;
                    check = 0;
                }
                else
                {
                    check = 1;
                }


                if (check == 0)
                {





                    sumThisPeriodQTY = v_ReportJobCost.Where(c => c.MainJobID == std.MainJobID).Sum(c => c.ThisPeriodQTY);
                    sumThisPeriodAMT = v_ReportJobCost.Where(c => c.MainJobID == std.MainJobID).Sum(c => c.ThisperiodAMT);
                    sumOpeningQTY = v_ReportJobCost.Where(c => c.MainJobID == std.MainJobID).Sum(c => c.OpeningQTY);
                    sumOpeningAMT = v_ReportJobCost.Where(c => c.MainJobID == std.MainJobID).Sum(c => c.OpeningAMT);
                    sumCumBalQTY = v_ReportJobCost.Where(c => c.MainJobID == std.MainJobID).Sum(c => c.CumBalQTY);
                    sumCumBalAMT = v_ReportJobCost.Where(c => c.MainJobID == std.MainJobID).Sum(c => c.CumBalAMT);
                    sumBudgetQTY = v_ReportJobCost.Where(c => c.MainJobID == std.MainJobID).Sum(c => c.BudgetQTY);
                    sumBudgetAMT = v_ReportJobCost.Where(c => c.MainJobID == std.MainJobID).Sum(c => c.BudgetAMT);


                    if (std.MainJobID == "A0000")
                    {
                        A00 = sumCumBalAMT;
                        TCXI += sumCumBalAMT;

                    }
                    if (std.MainJobID == "B0000")
                    {
                        B00 = sumCumBalAMT;
                        TCXI += sumCumBalAMT;
                    }
                    if (std.MainJobID == "C0000")
                    {
                        C00 = sumCumBalAMT;
                        TCXI += sumCumBalAMT;
                    }
                    if (std.MainJobID == "D0000")
                    {
                        D00 = sumCumBalAMT;
                        TCXI += sumCumBalAMT;
                    }
                    if (std.MainJobID == "E0000")
                    {
                        E00 = sumCumBalAMT;
                        TCXI += sumCumBalAMT;
                    }
                    if (std.MainJobID == "F0000")
                    {
                        F00 = sumCumBalAMT;
                        TCXI += sumCumBalAMT;
                    }

                    if (std.MainJobID == "G0000")
                    {
                        G00 = sumCumBalAMT;
                        TCXI += sumCumBalAMT;
                    }
                    if (std.MainJobID == "H0000")
                    {
                        H00 = sumCumBalAMT;
                        TCXI += sumCumBalAMT;
                    }


                    //main = "<tr>"
                    //         + "<td><b>" + std.MainJobID + " " + std.DescriptionMainJob + "</b></td>"
                    //         + "<td align ='right'>" + SetFontRed(sumThisPeriodQTY, 1) + "</td>"
                    //         + "<td align ='right'>" + SetFontRed(sumThisPeriodAMT, 1) + "</td>"
                    //         + "<td align ='right'>" + SetFontRed(sumOpeningQTY, 1) + "</td>"
                    //         + "<td align ='right'>" + SetFontRed(sumOpeningAMT, 1) + "</td>"
                    //         + "<td align ='right'>" + SetFontRed(sumCumBalQTY, 1) + "</td>"
                    //         + "<td align ='right'>" + SetFontRed(sumCumBalAMT, 1) + "</td>"
                    //         + "<td align ='right'>" + SetFontRed(sumBudgetQTY, 1) + "</td >"
                    //         + "<td align ='right'>" + SetFontRed(Percen(sumCumBalQTY, sumBudgetQTY), 1) + "<b>%</b>" + "</td>"
                    //         + "<td align ='right'>" + SetFontRed(sumBudgetAMT, 1) + "</td>"
                    //         + "<td align ='right'>" + SetFontRed(Percen(sumCumBalAMT, sumBudgetAMT), 1) + "<b>%</b>" + "</td>"
                    //         + "</tr>";

                    TableJobCosts.Add(new TableJobCost
                    {
                        JobTask = std.MainJobID + " " + std.DescriptionMainJob,
                        ThisPeriodQTY = sumThisPeriodQTY,
                        ThisPeriodAMT = sumThisPeriodAMT,
                        OpenningQTY = sumOpeningQTY,
                        OpenningAMT = sumSubOpeningAMT,
                        CumBalQTY = sumCumBalQTY,
                        CumBalAMT = sumCumBalAMT,
                        BudgetQty = sumBudgetQTY,
                        PerQty = Percen(sumCumBalQTY, sumBudgetQTY),
                        BudgetAMT = sumBudgetAMT,
                        PerAMT = Percen(sumCumBalAMT, sumBudgetAMT)
                    });

                    check = 1;
                }
                else
                {
                    //main = "";
                }

                //-------End main



                //SUB
                if (subID != std.SubJobID)
                {
                    subID = std.SubJobID;
                    check1 = 0;
                }
                else
                {
                    check1 = 1;
                }


                if (check1 == 0)
                {

                    if (std.MainJobID == std.SubJobID)
                    {
                        //sub = "";
                    }
                    else
                    {



                        sumSubThisPeriodQTY = v_ReportJobCost.Where(c => c.SubJobID == std.SubJobID).Sum(c => c.ThisPeriodQTY);
                        sumSubThisPeriodAMT = v_ReportJobCost.Where(c => c.SubJobID == std.SubJobID).Sum(c => c.ThisperiodAMT);
                        sumSubOpeningQTY = v_ReportJobCost.Where(c => c.SubJobID == std.SubJobID).Sum(c => c.OpeningQTY);
                        sumSubOpeningAMT = v_ReportJobCost.Where(c => c.SubJobID == std.SubJobID).Sum(c => c.OpeningAMT);
                        sumSubCumBalQTY = v_ReportJobCost.Where(c => c.SubJobID == std.SubJobID).Sum(c => c.CumBalQTY);
                        sumSubCumBalAMT = v_ReportJobCost.Where(c => c.SubJobID == std.SubJobID).Sum(c => c.CumBalAMT);
                        sumSubBudgetQTY = v_ReportJobCost.Where(c => c.SubJobID == std.SubJobID).Sum(c => c.BudgetQTY);
                        sumSubBudgetAMT = v_ReportJobCost.Where(c => c.SubJobID == std.SubJobID).Sum(c => c.BudgetAMT);

                        if (std.JobTaskNo == "D2100")
                        {
                            D2100 = std.CumBalAMT;
                        }

                        if (std.JobTaskNo == "E0800")
                        {
                            E0800 = std.CumBalAMT;
                        }
                        if (std.JobTaskNo == "H0400")
                        {
                            H0400 = std.CumBalAMT;
                        }
                        if (std.JobTaskNo == "H0200")
                        {
                            H0200 = std.CumBalAMT;
                        }
                        if (std.JobTaskNo == "H0800")
                        {
                            H0800 = std.CumBalAMT;
                        }
                        if (std.JobTaskNo == "H0600")
                        {
                            H0600 = std.CumBalAMT;
                        }



                        //sub = "<tr>"
                        //     + "<td><b>" + std.SubJobID + " " + std.DescriptionSubJob + "</b></td>"
                        //     + "<td align ='right'>" + SetFontRed(sumSubThisPeriodQTY, 1) + "</td>"
                        //     + "<td align ='right'>" + SetFontRed(sumSubThisPeriodAMT, 1) + "</td>"
                        //     + "<td align ='right'>" + SetFontRed(sumSubOpeningQTY, 1) + "</td>"
                        //     + "<td align ='right'>" + SetFontRed(sumSubOpeningAMT, 1) + "</td>"
                        //     + "<td align ='right'>" + SetFontRed(sumSubCumBalQTY, 1) + "</td>"
                        //     + "<td align ='right'>" + SetFontRed(sumSubCumBalAMT, 1) + "</td>"
                        //     + "<td align ='right'>" + SetFontRed(sumSubBudgetQTY, 1) + "</td >"
                        //     + "<td align ='right'>" + SetFontRed(Percen(sumSubCumBalQTY, sumSubBudgetQTY), 1) + "<b>%</b>" + "</td>"
                        //     + "<td align ='right'>" + SetFontRed(sumSubBudgetAMT, 1) + "</td>"
                        //     + "<td align ='right'>" + SetFontRed(Percen(sumSubCumBalAMT, sumSubBudgetAMT), 1) + "<b>%</b>" + "</td>"
                        //     + "</tr>";


                        TableJobCosts.Add(new TableJobCost
                        {
                            JobTask = std.SubJobID + " " + std.DescriptionSubJob,
                            ThisPeriodQTY = sumSubThisPeriodQTY,
                            ThisPeriodAMT = sumSubThisPeriodAMT,
                            OpenningQTY = sumSubOpeningQTY,
                            OpenningAMT = sumSubOpeningAMT,
                            CumBalQTY = sumSubCumBalQTY,
                            CumBalAMT = sumSubCumBalAMT,
                            BudgetQty = sumSubBudgetQTY,
                            PerQty = Percen(sumSubCumBalQTY, sumSubBudgetQTY),
                            BudgetAMT = sumSubBudgetAMT,
                            PerAMT = Percen(sumSubCumBalAMT, sumSubBudgetAMT)
                        });



                    }
                    check1 = 1;

                }
                else
                {
                    //sub = "";
                }

                //-------End main
     

                if (std.JobTaskNo == "G0210")
                {
                    G0210 = std.CumBalAMT;
                    G0210N = (std.CumBalAMT * -1).ToString("##,###.00");
                    G0210P = (std.CumBalAMT).ToString("##,###.00");
                }
                if (std.JobTaskNo == "C0110")
                {
                    C0110 = std.CumBalAMT;
                    ViewBag.C0110 = SetFontRed(std.CumBalAMT, 1);
                }
                if (std.JobTaskNo == "C0120")
                {
                    C0120 = std.CumBalAMT;
                    ViewBag.C0120 = SetFontRed(std.CumBalAMT, 1);
                }

                if (std.JobTaskNo == "C0130")
                {
                    C0130 = std.CumBalAMT;
                    ViewBag.C0130 = SetFontRed(std.CumBalAMT, 1);
                }

                if (std.JobTaskNo == "C0410")
                {
                    C0410 = std.CumBalAMT;
                    ViewBag.C0410 = SetFontRed(std.CumBalAMT, 1);
                }

                if (std.JobTaskNo == "C0510")
                {
                    C0510 = std.CumBalAMT;
                    ViewBag.C0510 = SetFontRed(std.CumBalAMT, 1);
                }
                if (std.JobTaskNo == "C0910")
                {
                    C0910 = std.CumBalAMT;
                    ViewBag.C0910 = SetFontRed(std.CumBalAMT, 1);
                }
                if (std.JobTaskNo == "C1010")
                {
                    C1010 = std.CumBalAMT;
                    ViewBag.C1010 = SetFontRed(std.CumBalAMT, 1);
                }
                if (std.JobTaskNo == "C1110")
                {
                    C1110 = std.CumBalAMT;
                    ViewBag.C1110 = SetFontRed(std.CumBalAMT, 1);
                }
                if (std.JobTaskNo == "D0450")
                {
                    D0450 = std.CumBalAMT;
                    ViewBag.D0450 = SetFontRed(std.CumBalAMT, 1);
                }
                if (std.JobTaskNo == "H0110")
                {
                    H0110 = std.CumBalAMT;
                    ViewBag.H0110 = SetFontRed(std.CumBalAMT, 1);
                }
                if (std.JobTaskNo == "H0130")
                {
                    H0130 = std.CumBalAMT;
                    ViewBag.H0130 = SetFontRed(std.CumBalAMT, 1);
                }


                if (std.JobTaskNo == "E0910")
                {
                    E0910 = std.CumBalAMT;
                    ViewBag.E0910 = SetFontRed(std.CumBalAMT, 1);
                }
                if (std.JobTaskNo == "E0920")
                {
                    E0920 = std.CumBalAMT;
                    ViewBag.E0920 = SetFontRed(std.CumBalAMT, 1);
                }
                if (std.JobTaskNo == "E0930")
                {
                    E0930 = std.CumBalAMT;
                    ViewBag.E0930 = SetFontRed(std.CumBalAMT, 1);
                }
                if (std.JobTaskNo == "E0940")
                {
                    E0940 = std.CumBalAMT;
                    ViewBag.E0940 = SetFontRed(std.CumBalAMT, 1);
                }

                if (std.JobTaskNo == "F5999")
                {
                    F5999 = std.CumBalAMT;
                    ViewBag.F5999 = SetFontRed(std.CumBalAMT, 1);
                }



                if (std.MainJobID == std.JobTaskNo || std.SubJobID == std.JobTaskNo)
                {
                    //table = "";
                }
                else
                {



                    //table = "<tr>"
                    //     + "<td>" + std.DescriptionFull + "</td>"
                    //     + "<td align ='right'>" + SetFontRed(std.ThisPeriodQTY, 3) + "</td>"
                    //     + "<td align ='right'>" + SetFontRed(std.ThisperiodAMT, 3) + "</td>"
                    //     + "<td align ='right'>" + SetFontRed(std.OpeningQTY, 3) + "</td>"
                    //     + "<td align ='right'>" + SetFontRed(std.OpeningAMT, 3) + "</td>"
                    //     + "<td align ='right'>" + SetFontRed(std.CumBalQTY, 3) + "</td>"
                    //     + "<td align ='right'>" + SetFontRed(std.CumBalAMT, 3) + "</td>"
                    //     + "<td align ='right'>" + SetFontRed(std.BudgetQTY, 3) + "</td >"
                    //     + "<td align ='right'>" + SetFontRed(Percen(std.CumBalQTY, std.BudgetQTY), 3) + "%" + "</td>"
                    //     + "<td align ='right'>" + SetFontRed(std.BudgetAMT, 3) + "</td>"
                    //     + "<td align ='right'>" + SetFontRed(Percen(std.CumBalAMT, std.BudgetAMT), 3) + "%" + "</td>"
                    //     + "</tr>";


                    TableJobCosts.Add(new TableJobCost
                    {
                        JobTask = std.DescriptionFull,
                        ThisPeriodQTY = std.ThisPeriodQTY,
                        ThisPeriodAMT = std.ThisperiodAMT,
                        OpenningQTY = std.OpeningQTY,
                        OpenningAMT = std.OpeningAMT,
                        CumBalQTY = std.CumBalQTY,
                        CumBalAMT = std.CumBalAMT,
                        BudgetQty = std.BudgetQTY,
                        PerQty = Percen(std.CumBalQTY, std.BudgetQTY),
                        BudgetAMT = std.BudgetAMT,
                        PerAMT = Percen(std.CumBalAMT, std.BudgetAMT)
                    });






                }

                //totalview += main + sub + table;
            }




            N00 = v_ReportJobCost.Where(c => c.MainJobID == "N0000").Sum(c => c.CumBalAMT);
            O00 = v_ReportJobCost.Where(c => c.MainJobID == "O0000").Sum(c => c.CumBalAMT);

            TIC += C0110 + C0120 + C0130 + C0410 + C0510 + C0910 + C1010 + C1110 + D0450 + H0110 + H0130 + H0200 + H0800;



            ViewBag.TIC = SetFontRed(TIC, 1);



            //ViewBag.TCXI = SetFontRed(TCXI + (G0210 * -1), 1);  ///TCXI-TIC

           
    
            var INCOME = (N00 + O00).ToString("##,###.00");
            //ViewBag.INCOME1 = SetFontRed((N00 + O00) * -1, 1);
            var ExternalCost = ((TCXI + (G0210 * -1)) - TIC).ToString("##,###.00");
            var GrossProfit = (((N00 + O00) * -1) - ((TCXI + (G0210 * -1)) - TIC)).ToString("##,###.00");

            var NetProfit = (((((N00 + O00) * -1) - ((TCXI + (G0210 * -1)) - TIC)) - TIC)).ToString("##,###.00");
            var PerGross =(Percen(((((N00 + O00) * -1) - ((TCXI + (G0210 * -1)) - TIC))), (N00 + O00) * -1)).ToString("##,###.00%");
            var PerNet = (Percen(((((N00 + O00) * -1) - ((TCXI + (G0210 * -1)) - TIC)) - TIC), (N00 + O00) * -1)).ToString("##,###.00%");

            //ViewBag.table = headtable + totalview + "</tbody></table>";


            estimates.Add(new Estimate
            {
                EstimateJob = JobNo,
                EstimateStart = date1,
                EstimateEnd = date2,
                a1 = INCOME,
                a2 = ExternalCost,
                a3 = GrossProfit,
                a4 = TIC.ToString("##,###.00"),
                a5 = NetProfit,
                b1 = PerGross,
                b2 = PerNet,
                c1 = N00.ToString("##,###.00"),
                c2 = O00.ToString("##,###.00"),
                c3 = INCOME,
                c4 = A00.ToString("##,###.00"),
                c5 = B00.ToString("##,###.00"),
                c6 = C00.ToString("##,###.00"),
                c7 = D00.ToString("##,###.00"),
                c8 = E00.ToString("##,###.00"),
                c9 = F00.ToString("##,###.00"),
                c10 = G00.ToString("##,###.00"),
                c11 = G0210N,
                c12 = H00.ToString("##,###.00"),
                c13 = TCXI.ToString("##,###.00"),
                c14 = C0110.ToString("##,###.00"),
                c15 = C0120.ToString("##,###.00"),
                c16 = C0130.ToString("##,###.00"),
                c17 = C0410.ToString("##,###.00"),
                c18 = C0510.ToString("##,###.00"),
                c19 = C0910.ToString("##,###.00"),
                c20 = C1010.ToString("##,###.00"),
                c21 = C1110.ToString("##,###.00"),
                c22 = D0450.ToString("##,###.00"),
                c23 = H0110.ToString("##,###.00"),
                c24 = H0130.ToString("##,###.00"),
                c25 = H0200.ToString("##,###.00"),
                c26 = H0800.ToString("##,###.00"),
                c27 = TIC.ToString("##,###.00"),
                c28 = D2100.ToString("##,###.00"),
                c29 = E0800.ToString("##,###.00"),
                c30 = E0910.ToString("##,###.00"),
                c31 = E0920.ToString("##,###.00"),
                c32 = E0930.ToString("##,###.00"),
                c33 = E0940.ToString("##,###.00"),
                c34 = G0210P,
                c35 = H0600.ToString("##,###.00"),
                c36 = F5999.ToString("##,###.00"),
                c37 = H0400.ToString("##,###.00")
          

             
            });




            response = Ok(new { data = TableJobCosts ,data1=estimates});






           return response;

            //return View();
        }





        public IActionResult GetEvents()
        {
            /*Check Session */
            var page = "155";
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
            var queryData = "SELECT [User ID] AS X,CONCAT(CONVERT(varchar,CONVERT(date,[Date and Time])),' ',CONVERT(varchar,[Time],108)) AS Y FROM dbo."+ Environment.GetEnvironmentVariable("Company") +"Change Log Entry]  WHERE [Entry No_]>((SELECT count(*) FROM dbo."+ Environment.GetEnvironmentVariable("Company") +"Change Log Entry])+6700000) ORDER BY [User ID],[Date and Time]  DESC";

            var databaseContext = _navcontext.dataXYString.FromSqlRaw(queryData).ToList();

            List<DataXYString> instances = new List<DataXYString>();
            DataXYString current = null;
            var checkid = "";
            foreach (var std in databaseContext as IList<DataXYString>)
            {

                if (std.X == checkid)
                {

                }
                else
                {
                    current = new DataXYString();
                    current.X = std.X;
                    current.Y = std.Y;
                    instances.Add(current);
                    checkid = std.X;
                }             
            }

            response = Ok(new { data = instances });

            return response;

        }




        public IActionResult GetSession()
        {
            /*Check Session */
            var page = "155";
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
            var queryData = "SELECT a.SID,a.ID,(SELECT top 1 CONVERT(VARCHAR,SWITCHOFFSET(dbo.[Active Session].[Login Datetime],'+07:00'))  FROM dbo.[Active Session] WHERE [User SID]=a.SID ORDER BY [Login Datetime] DESC) AS DateLogin FROM(SELECT DISTINCT dbo.[Active Session].[User SID] as SID,dbo.[Active Session].[User ID] As ID FROM dbo.[Active Session]) as a Order BY DateLogin DESC";
            var databaseContext = _navcontext.sessionNavs.FromSqlRaw(queryData).ToList();
            response = Ok(new { data = databaseContext });
            return response;

        }


        
        public IActionResult GetSessionNew()
        {

            /*Check Session */
            var page = "155";
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
            var queryData = "SELECT a.SID,a.ID,(SELECT top 1 CONVERT(VARCHAR,SWITCHOFFSET(dbo.[Active Session].[Login Datetime],'+07:00'))  FROM dbo.[Active Session] WHERE [User SID]=a.SID ORDER BY [Login Datetime] DESC) AS DateLogin,'' as DateDoIt FROM(SELECT DISTINCT dbo.[Active Session].[User SID] as SID,dbo.[Active Session].[User ID] As ID FROM dbo.[Active Session]) as a Order BY DateLogin DESC";
            var databaseContext = _navcontext.sessionNavs.FromSqlRaw(queryData).ToList();


            var queryData1 = "SELECT [User ID] AS X,CONCAT(CONVERT(varchar,CONVERT(date,[Date and Time])),' ',CONVERT(varchar,[Time],108)) AS Y FROM dbo."+ Environment.GetEnvironmentVariable("Company") +"Change Log Entry]  WHERE [Entry No_]>((SELECT count(*) FROM dbo."+ Environment.GetEnvironmentVariable("Company") +"Change Log Entry])+6700000) ORDER BY [User ID],[Date and Time]  DESC";
            var databaseContext1 = _navcontext.dataXYString.FromSqlRaw(queryData1).ToList();

            List<DataXYString> instances1 = new List<DataXYString>();
            DataXYString current1 = null;
            var checkid = "";
            foreach (var std1 in databaseContext1 as IList<DataXYString>)
            {
                if (std1.X == checkid)
                {

                }
                else
                {
                    current1 = new DataXYString();
                    current1.X = std1.X;
                    current1.Y = std1.Y;
                    instances1.Add(current1);
                    checkid = std1.X;
                }
            }



            List<SessionNav> instances = new List<SessionNav>();
            SessionNav current = null;

            foreach (var std in databaseContext as IList<SessionNav>)
            {
                current = new SessionNav();
                current.SID = std.SID;
                current.ID = std.ID;
                current.DateLogin = std.DateLogin;
                current.DateDoIt = instances1.Where(c => c.X == std.ID).Select(c => c.Y).SingleOrDefault();

                instances.Add(current);
             
            }




            response = Ok(new { data = instances ,data2= instances.Count()});

            return response;

        }






        [HttpGet]
        public IActionResult CheckUserDoIt()
        {
            /*Check Session */
            var page = "155";
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

        [HttpGet]
        public async Task<IActionResult> remove2(string id)
        {
            /*Check Session */
            var page = "155";
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


            string Qry = "DELETE FROM [dbo].[Active Session] WHERE [User ID]='"+id+"'";
            using (var connection = _navsupercontext.Database.GetDbConnection())
            {
                await connection.OpenAsync();
                using (var command = connection.CreateCommand())
                {
                    command.CommandText = Qry;
                    var result = await command.ExecuteNonQueryAsync();
                }
            }


            //ar databaseContext = await _navcontext.ExecuteSqlCommandAsync(Qry);
            IActionResult response = Unauthorized();
            response = Ok(new { data = id});

            return response;

            //return View();
        }



    }
}
