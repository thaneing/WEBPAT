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
using Microsoft.AspNetCore.Authorization;

namespace CESAPSCOREWEBAPP.Controllers
{
    [Authorize]
    public class CheckLedgersController : BaseController
    {
        private readonly NAVContext _navcontext;

        private readonly IHostingEnvironment _hostingEnvironment;


        public CheckLedgersController(DatabaseContext context, NAVContext navcontext, IHostingEnvironment hostingEnvironment)
        {
            _navcontext = navcontext;
            _hostingEnvironment = hostingEnvironment;
        }


        // GET: JobLedgerEntry
        public IActionResult Index()
        {


            /*Check Session */
            var page = "53";
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



            ///*Check Session */
            //var page = "6";
            //var typeofuser = "";
            //var PermisionAction = "";
            //// CheckSession
            //if (string.IsNullOrEmpty(HttpContext.Session.GetString("Username")))
            //{
            //    Alert("คุณไม่มีสิทธิ์ใช้งานหน้าดังกล่าว", NotificationType.error);
            //    return RedirectToAction("Index", "Home");
            //}
            //else
            //{
            //    typeofuser = HttpContext.Session.GetString("TypeOfUserId");
            //    PermisionAction = HttpContext.Session.GetString("PermisionAction");
            //    if (PermisionHelper.CheckPermision(typeofuser, PermisionAction, page) == false)
            //    {
            //        Alert("คุณไม่มีสิทธิ์ใช้งานหน้าดังกล่าว", NotificationType.error);
            //        return RedirectToAction("Index", "Home");
            //    }
            //}
            ///*Check Session */



            ViewBag.StartDate = DateTime.Now.ToString("01-MM-yyyy", new CultureInfo("en-US"));
            ViewBag.EndDate = DateTime.Now.ToString("dd-MM-yyyy", new CultureInfo("en-US"));

            return View();
        }


        // POST: JobLedgerEntry/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Index(string Startdate, string EndDate)
        {

            /*Check Session */
            var page = "53";
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

            var date1 = Startdate.Substring(6,4) +"-"+Startdate.Substring(3,2)+"-"+Startdate.Substring(0,2)+" 00:00:00";
            var date2 = EndDate.Substring(6,4) +"-"+EndDate.Substring(3,2)+"-"+EndDate.Substring(0,2)+" 23:59:59";


            var sdate1 = Startdate;
            var sdate2 = EndDate;
            var rdate1 = Startdate;
            var rdate2 = EndDate;

            ViewBag.StartDate = rdate1;
            ViewBag.EndDate = rdate2;
            ViewBag.SStartDate = sdate1;
            ViewBag.SEndDate = sdate2;

            var queryData = "SELECT ROW_NUMBER() OVER (ORDER BY b.Posdate) as ID, b.Posdate,b.Item,b.Description,b.Document,b.ItemNeg,b.ItemPos,b.DiffItem,b.Job,b.DiffItemPosAndJob,b.DiffItemNegAndJob FROM( SELECT "
                            + " a.Posdate,a.Item,a.Description,a.Document,"
                            + " (SELECT sum([Cost Amount (Actual)]) as sumdoc FROM dbo."+ Environment.GetEnvironmentVariable("Company") + "Value Entry] WHERE [Item No_]=a.Item and[Document No_] = a.Document and  [Source Type]=1 ) as ItemNeg,"
                            + " (SELECT sum([Cost Amount (Actual)]) as sumdoc FROM dbo."+Environment.GetEnvironmentVariable("Company")+"Value Entry] WHERE [Item No_]=a.Item and[Document No_] = a.Document and  [Source Type]=0 ) as ItemPos,"

                            + " (ABS((SELECT sum([Cost Amount (Actual)]) as sumdoc FROM dbo."+ Environment.GetEnvironmentVariable("Company") + "Value Entry] WHERE [Item No_]=a.Item and[Document No_] = a.Document and  [Source Type]=1 )) - ABS((SELECT sum([Cost Amount (Actual)]) as sumdoc FROM dbo."+ Environment.GetEnvironmentVariable("Company") + "Value Entry] WHERE [Item No_]=a.Item and [Document No_] = a.Document and  [Source Type]=0 ))) AS DiffItem,"
                            + " (SELECT sum([Cost Amount (Actual)]) as sumdoc FROM dbo."+ Environment.GetEnvironmentVariable("Company") + "Value Entry] WHERE [Item No_]=a.Item and [Item No_]=a.Item and [Document No_] = a.Document and  [Job Ledger Entry No_]<>0 ) as Job,"
                            + " ABS((SELECT sum([Cost Amount (Actual)]) as sumdoc FROM dbo."+ Environment.GetEnvironmentVariable("Company") + "Value Entry] WHERE [Item No_]=a.Item and [Document No_] = a.Document and  [Source Type]=0 ))-ABS((SELECT sum([Cost Amount (Actual)]) as sumdoc FROM dbo."+ Environment.GetEnvironmentVariable("Company") + "Value Entry] WHERE [Item No_]=a.Item and [Document No_] = a.Document and  [Job Ledger Entry No_]<>0 )) as DiffItemPosAndJob,"
                            + " ABS((SELECT sum([Cost Amount (Actual)]) as sumdoc FROM dbo."+ Environment.GetEnvironmentVariable("Company") + "Value Entry] WHERE [Item No_]=a.Item and [Document No_] = a.Document and  [Source Type]=1 ))-ABS((SELECT sum([Cost Amount (Actual)]) as sumdoc FROM dbo."+ Environment.GetEnvironmentVariable("Company") + "Value Entry] WHERE [Item No_]=a.Item and [Document No_] = a.Document and  [Job Ledger Entry No_]<>0 )) as DiffItemNegAndJob"
                            + " FROM ( SELECT DISTINCT [Item No_] as Item, Description as Description,[Document No_] as Document,[Posting Date] as Posdate FROM dbo."+Environment.GetEnvironmentVariable("Company")+"Value Entry] WHERE ([Document No_] Like 'C%' or [Document No_] Like 'SC%') AND [Posting Date]>='" + date1 + "' AND [Posting Date]<='"+ date2 + "') as a) as b WHERE b.DiffItem <>0 or b.DiffItem <>null order by b.Document,Posdate ";



            //ViewBag.sql = queryData;

            var v_CheckLedgers = _navcontext.V_CheckLedgers.FromSqlRaw(queryData).ToList();


            ViewData["v_CheckLedgers"] = v_CheckLedgers;

            var headtable = "";
            var main = "";

            headtable = "<table  id='v_CheckLedgers' ><thead><tr>"
                + "<th align ='center'>วันที่</th>"
                + "<th align ='center'>เลขที่เอกสาร</th>"
                + "<th align ='center'>รหัส Item</th>"
                + "<th align ='center'>รายละเอียด</th>"
                + "<th align ='center'>Itm. Negative(1)</th>"
                + "<th align ='center'>Itm. Positive(2)</th>"
                + "<th align ='center'>ผลต่าง (1)-(2)</th>"
                + "<th align ='center'>Job Cost(3)</th>"
                + "<th align ='center'>ผลต่าง (2)-(3) </th>"
                + "<th align ='center'>ผลต่าง (1)-(3)</th>"
            + "</tr>"
            + "</thead>"
           + "<tbody id='myTable'>";

            foreach (var std in v_CheckLedgers as IList<V_CheckLedger>)
            {


                main += "<tr>"
                         + "<td>" + std.Posdate.ToString("yyyy-MM-dd") + "</td>"
                         + "<td>" + std.Document + "</td>"
                         + "<td>" + std.Item + "</td>"
                         + "<td>" + std.Description + "</td>"
                         + "<td align ='right'>" + SetFontRed(std.ItemNeg, 1) + "</td>"
                         + "<td align ='right'>" + SetFontRed(std.ItemPos, 1) + "</td>"
                         + "<td align ='right'>" + SetFontRed(std.DiffItem, 1) + "</td>"
                         + "<td align ='right'>" + SetFontRed(std.Job, 1) + "</td >"
                         + "<td align ='right'>" + SetFontRed(std.DiffItemPosAndJob, 1) + "</td>"
                         + "<td align ='right'>" + SetFontRed(std.DiffItemNegAndJob, 1) + "</td>"
                         + "</tr>";


            }


            ViewBag.table = headtable + main + "</tbody></table>";

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



        public IActionResult GetDataError()
        {




            IActionResult response = Unauthorized();
            var queryData = "SELECT ROW_NUMBER() OVER (ORDER BY b.Posdate) as ID,b.Posdate,b.Item,b.Description,b.Document,b.ItemNeg,b.ItemPos,b.DiffItem,b.Job,b.DiffItemPosAndJob,b.DiffItemNegAndJob FROM( SELECT "
                            + " a.Posdate,a.Item,a.Description,a.Document,"
                            + " (SELECT sum([Cost Amount (Actual)]) as sumdoc FROM dbo."+ Environment.GetEnvironmentVariable("Company") +"Value Entry] WHERE [Item No_]=a.Item and[Document No_] = a.Document and  [Source Type]=1 ) as ItemNeg,"
                            + " (SELECT sum([Cost Amount (Actual)]) as sumdoc FROM dbo."+ Environment.GetEnvironmentVariable("Company") +"Value Entry] WHERE [Item No_]=a.Item and[Document No_] = a.Document and  [Source Type]=0 ) as ItemPos,"

                            + " (ABS((SELECT sum([Cost Amount (Actual)]) as sumdoc FROM dbo."+ Environment.GetEnvironmentVariable("Company") +"Value Entry] WHERE [Item No_]=a.Item and[Document No_] = a.Document and  [Source Type]=1 )) - ABS((SELECT sum([Cost Amount (Actual)]) as sumdoc FROM dbo."+ Environment.GetEnvironmentVariable("Company") +"Value Entry] WHERE [Item No_]=a.Item and [Document No_] = a.Document and  [Source Type]=0 ))) AS DiffItem,"
                            + " (SELECT sum([Cost Amount (Actual)]) as sumdoc FROM dbo."+ Environment.GetEnvironmentVariable("Company") +"Value Entry] WHERE [Item No_]=a.Item and [Item No_]=a.Item and [Document No_] = a.Document and  [Job Ledger Entry No_]<>0 ) as Job,"
                            + " ABS((SELECT sum([Cost Amount (Actual)]) as sumdoc FROM dbo."+ Environment.GetEnvironmentVariable("Company") +"Value Entry] WHERE [Item No_]=a.Item and [Document No_] = a.Document and  [Source Type]=0 ))-ABS((SELECT sum([Cost Amount (Actual)]) as sumdoc FROM dbo."+ Environment.GetEnvironmentVariable("Company") +"Value Entry] WHERE [Item No_]=a.Item and [Document No_] = a.Document and  [Job Ledger Entry No_]<>0 )) as DiffItemPosAndJob,"
                            + " ABS((SELECT sum([Cost Amount (Actual)]) as sumdoc FROM dbo."+ Environment.GetEnvironmentVariable("Company") +"Value Entry] WHERE [Item No_]=a.Item and [Document No_] = a.Document and  [Source Type]=1 ))-ABS((SELECT sum([Cost Amount (Actual)]) as sumdoc FROM dbo."+ Environment.GetEnvironmentVariable("Company") +"Value Entry] WHERE [Item No_]=a.Item and [Document No_] = a.Document and  [Job Ledger Entry No_]<>0 )) as DiffItemNegAndJob"
                            + " FROM ( SELECT DISTINCT [Item No_] as Item, Description as Description,[Document No_] as Document,[Posting Date] as Posdate FROM dbo."+ Environment.GetEnvironmentVariable("Company") +"Value Entry] WHERE ([Document No_] Like 'C%' or [Document No_] Like 'SC%') AND [Posting Date]>='2019-01-01 00:00:00') as a) as b WHERE b.DiffItem <>0 or b.DiffItem <>null order by b.Document,Posdate ";



            //ViewBag.sql = queryData;

            var v_CheckLedgers = _navcontext.V_CheckLedgers.FromSqlRaw(queryData).ToList();

           
            var countdata = v_CheckLedgers.Count;


            response = Ok(new { countdata = countdata });

            return response;
        }

    }

}
