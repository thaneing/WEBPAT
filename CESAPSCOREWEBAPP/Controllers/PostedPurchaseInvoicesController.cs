using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;
using CESAPSCOREWEBAPP.Helpers;
using CESAPSCOREWEBAPP.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using static CESAPSCOREWEBAPP.Models.Enums;

namespace CESAPSCOREWEBAPP.Controllers
{
    [Authorize]
    public class PostedPurchaseInvoicesController : BaseController
    {
        private readonly NAVContext _navcontext;
        private readonly DatabaseContext _context;

        private IConfiguration _config { get; }
        public PostedPurchaseInvoicesController(NAVContext navcontext, DatabaseContext context, IConfiguration config)
        {
            _context = context;
            _navcontext = navcontext;
            _config = config;
        }
        public IActionResult Index()
        {

            /*Check Session */
            var page = "276";
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

            ViewBag.StartDate = DateTime.Now.ToString("01/MM/yyyy", new CultureInfo("en-US"));
            ViewBag.EndDate = DateTime.Now.ToString("dd/MM/yyyy", new CultureInfo("en-US"));


            var queryData1 = "SELECT dbo." + Environment.GetEnvironmentVariable("Company") + "Location].Code AS name,dbo." + Environment.GetEnvironmentVariable("Company") + "Location].Name as code FROM [dbo]." + Environment.GetEnvironmentVariable("Company") + "Location] order by dbo." + Environment.GetEnvironmentVariable("Company") + "Location].Code asc ";
            var sourceAutoCompletes = _navcontext.sourceAutoCompletes.FromSqlRaw(queryData1).ToList();
            ViewData["SourceAutoCompletes"] = sourceAutoCompletes;

            return View();
        }


        public IActionResult GetData(string date1, string date2,string site)
        {
            /*Check Session */
            var page = "276";
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

            var postedPurchaseInvoicesDATA = QueryData(date1, date2, site);

           

            response = Ok(new { data = postedPurchaseInvoicesDATA });


            return response;
        }
        public IActionResult Detailmail()
        {
            /*Check Session */
            var page = "277";
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

            var name = _config["Email:NameAccount"].ToString();
            var email = _config["Email:MailAccount"];

            response = Ok(new { name = name, email = email});

            return response;
        }
        public IActionResult SendApplov(string tomail,string site, string date1, string date2,string mailCC)
        {

            /*Check Session */
            var page = "277";
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


            var postedPurchaseInvoicesDATA = QueryData(date1, date2, site);
           

            var main = "";
            var header = "<h4>รายการใบรับสินค้า(GR) ที่บัญชีตั้งหนี้แล้วของ หน่วยงาน " + site +" มีดังนี้ </h4><br/>";

            var headtable = "<table style='border: 1px solid #ddd;border-collapse: collapse;' ><thead><tr>"
                + "<th align ='center' style='border: 1px solid #ddd;border-collapse: collapse;padding: 8px 16px;'>PostingDate</th>"
                + "<th align ='center' style='border: 1px solid #ddd;border-collapse: collapse;padding: 8px 16px;'>Ref_ Receipt No_</th>"
                + "<th align ='center' style='border: 1px solid #ddd;border-collapse: collapse;padding: 8px 16px;'>Document No.</th>"
                + "<th align ='center' style='border: 1px solid #ddd;border-collapse: collapse;padding: 8px 16px;'>Order No.</th>"
                + "<th align ='center' style='border: 1px solid #ddd;border-collapse: collapse;padding: 8px 16px;'>Buy-from Vendor Name</th>"
                + "<th align ='center' style='border: 1px solid #ddd;border-collapse: collapse;padding: 8px 16px;'>Vendor Invoice No.</th>"
                + "<th align ='center' style='border: 1px solid #ddd;border-collapse: collapse;padding: 8px 16px;'>Amount</th>"
                // + "<th align ='center' style='border: 1px solid #ddd;border-collapse: collapse;padding: 8px 16px;'>Progress Term</th>"
                + "</tr>"
                + "</thead>"
               + "<tbody id='myTable'>";

            foreach (var std in postedPurchaseInvoicesDATA as List<PostedPurchaseInvoices>)
            {
               

                main += "<tr>"
                         //+ "<td style='border: 1px solid #ddd;border-collapse: collapse;padding: 8px 16px;'>" + std.Statuss + "</td>"
                         + "<td style='border: 1px solid #ddd;border-collapse: collapse;padding: 8px 16px;'>" + std.PostingDate + "</td>"
                          + "<td style='border: 1px solid #ddd;border-collapse: collapse;padding: 8px 16px;'>" + std.RefReceiptNo + "</td>"
                         + "<td style='border: 1px solid #ddd;border-collapse: collapse;padding: 8px 16px;'>" + std.DocumentNo + "</td>"
                         + "<td style='border: 1px solid #ddd;border-collapse: collapse;padding: 8px 16px;'>" + std.OrderNo + "</td>"
                         + "<td style='border: 1px solid #ddd;border-collapse: collapse;padding: 8px 16px;'>" + std.VendorName + "</td>" 
                         + "<td style='border: 1px solid #ddd;border-collapse: collapse;padding: 8px 16px;'>" + std.VendorInvoiceNo + "</td>"
                          + "<td style='border: 1px solid #ddd;border-collapse: collapse;padding: 8px 16px;text-align:right'>" + std.Amount.ToString("#,##0.00") + "</td>"

                         // + "<td style='border: 1px solid #ddd;border-collapse: collapse;padding: 8px 16px;'>" + std.ProgressTerm + "</td >"
                         + "</tr>";


            }


            var table = headtable + main + "</tbody></table> </br>" /*+ bodyfile*/;
            var datamail = header + table;


                    var checksucess = 0;
                    var mailto = tomail;///ผู้รับ

                    var form = "thanyalak@ces.co.th";
                    var toform = form;//ถึงผู้ส่ง
                    var cc = mailCC;
                    var title = "ใบรับสินค้า(GR) ที่บัญชีตั้งหนี้แล้ว";
                    var body = datamail;

                  var test = SentMail.MailSent(_config["Email:Server"], Int32.Parse(_config["Email:Port"]), bool.Parse(_config["Email:Security"]), "", "", form, mailto +";"+ toform, title, body,cc);
            

                    if (test == true)
                    {
                        checksucess = 0;
                    }
                    else
                    {
                        checksucess = 1;
                    }
                
                response = Ok(new
                {
                    check = checksucess,mailto=mailto,tomail=tomail,to=toform
                });
            
            return response;

        }

        private List<PostedPurchaseInvoices> QueryData (string date1,string date2,string site)
        {
            var StartDate = date1.Substring(6, 4) + "-" + date1.Substring(3, 2) + "-" + date1.Substring(0, 2) + " 00:00:00";
            var EndDate = date2.Substring(6, 4) + "-" + date2.Substring(3, 2) + "-" + date2.Substring(0, 2) + " 23:59:59";


            List<PostedPurchaseInvoices> postedPurchaseInvoicesDATA;
            if (site == null)
            {
                var querydata = "SELECT *," +
                    " (SELECT SUM(dbo."+ Environment.GetEnvironmentVariable("Company") +"Purch_ Inv_ Line].Amount) FROM dbo."+ Environment.GetEnvironmentVariable("Company") +"Purch_ Inv_ Line] WHERE dbo."+ Environment.GetEnvironmentVariable("Company") +"Purch_ Inv_ Line].[Document No_] = a.DocumentNo) AS Amount, " +
                      " (SELECT Top 1 dbo."+ Environment.GetEnvironmentVariable("Company") +"Purch_ Rcpt_ Header].[Order No_] FROM dbo."+ Environment.GetEnvironmentVariable("Company") +"Purch_ Rcpt_ Header] WHERE dbo."+ Environment.GetEnvironmentVariable("Company") +"Purch_ Rcpt_ Header].[No_]=a.RefReceiptNo) as OrderNo" +
                    " From( " +
                    " SELECT convert(varchar,dbo."+ Environment.GetEnvironmentVariable("Company") +"Purch_ Inv_ Header].[Posting Date],23) AS PostingDate," +
                               " dbo."+ Environment.GetEnvironmentVariable("Company") +"Purch_ Inv_ Header].[No_] AS DocumentNo," +
                               " dbo."+ Environment.GetEnvironmentVariable("Company") +"Purch_ Inv_ Header].[Ref_ Receipt No_] AS RefReceiptNo," +
                               " dbo."+ Environment.GetEnvironmentVariable("Company") +"Purch_ Inv_ Header].[Buy-from Vendor Name] AS VendorName," +
                               " dbo."+ Environment.GetEnvironmentVariable("Company") +"Purch_ Inv_ Header].[Ref_ Job Order No_] AS JobOrderNo," +
                               " dbo."+ Environment.GetEnvironmentVariable("Company") +"Purch_ Inv_ Header].[Vendor Invoice No_] AS VendorInvoiceNo ," +

                               " dbo."+ Environment.GetEnvironmentVariable("Company") +"Purch_ Inv_ Header].[Progress Term] AS ProgressTerm" +
                               " FROM" +
                               " dbo."+ Environment.GetEnvironmentVariable("Company") +"Purch_ Inv_ Header] " +
                               " WHERE dbo."+ Environment.GetEnvironmentVariable("Company") +"Purch_ Inv_ Header].[Posting Date] >= {0} AND dbo."+ Environment.GetEnvironmentVariable("Company") +"Purch_ Inv_ Header].[Posting Date] <= {1} AND  dbo."+ Environment.GetEnvironmentVariable("Company") +"Purch_ Inv_ Header].[No_] NOT LIKE 'APA%'" +
                               ") AS a ";

                //SqlParameter parameterStartdate = new SqlParameter("@startdate", StartDate);
                //SqlParameter parameterEndDate = new SqlParameter("@enddate", EndDate);

                postedPurchaseInvoicesDATA = _navcontext.PostedPurchaseInvoices.FromSqlRaw(querydata, StartDate, EndDate).ToList();
            }
            else
            {
                var querydata = "SELECT *," +
                    " (SELECT SUM(dbo."+ Environment.GetEnvironmentVariable("Company") +"Purch_ Inv_ Line].Amount) FROM dbo."+ Environment.GetEnvironmentVariable("Company") +"Purch_ Inv_ Line] WHERE dbo."+ Environment.GetEnvironmentVariable("Company") +"Purch_ Inv_ Line].[Document No_] = a.DocumentNo) AS Amount ," +
                    " (SELECT Top 1 dbo."+ Environment.GetEnvironmentVariable("Company") +"Purch_ Rcpt_ Header].[Order No_] FROM dbo."+ Environment.GetEnvironmentVariable("Company") +"Purch_ Rcpt_ Header] WHERE dbo."+ Environment.GetEnvironmentVariable("Company") +"Purch_ Rcpt_ Header].[No_]=a.RefReceiptNo) as OrderNo" +
                    " From( " +
                    " SELECT convert(varchar,dbo."+ Environment.GetEnvironmentVariable("Company") +"Purch_ Inv_ Header].[Posting Date],23) AS PostingDate," +
                               " dbo."+ Environment.GetEnvironmentVariable("Company") +"Purch_ Inv_ Header].[No_] AS DocumentNo," +
                               " dbo."+ Environment.GetEnvironmentVariable("Company") +"Purch_ Inv_ Header].[Ref_ Receipt No_] AS RefReceiptNo," +
                               " dbo."+ Environment.GetEnvironmentVariable("Company") +"Purch_ Inv_ Header].[Buy-from Vendor Name] AS VendorName," +
                               " dbo."+ Environment.GetEnvironmentVariable("Company") +"Purch_ Inv_ Header].[Ref_ Job Order No_] AS JobOrderNo," +
                               " dbo."+ Environment.GetEnvironmentVariable("Company") +"Purch_ Inv_ Header].[Vendor Invoice No_] AS VendorInvoiceNo ," +

                               " dbo."+ Environment.GetEnvironmentVariable("Company") +"Purch_ Inv_ Header].[Progress Term] AS ProgressTerm" +
                               " FROM" +
                               " dbo."+ Environment.GetEnvironmentVariable("Company") +"Purch_ Inv_ Header] " +
                               " WHERE dbo."+ Environment.GetEnvironmentVariable("Company") +"Purch_ Inv_ Header].[Posting Date] >= {0} AND dbo."+ Environment.GetEnvironmentVariable("Company") +"Purch_ Inv_ Header].[Posting Date] <= {1} AND dbo."+ Environment.GetEnvironmentVariable("Company") +"Purch_ Inv_ Header].[Location Code] = {2} AND  dbo."+ Environment.GetEnvironmentVariable("Company") +"Purch_ Inv_ Header].[No_] NOT LIKE 'APA%'" +
                               " ) AS a";

                //SqlParameter parameterStartdate = new SqlParameter("@startdate", StartDate);
                //SqlParameter parameterEndDate = new SqlParameter("@enddate", EndDate);

                postedPurchaseInvoicesDATA = _navcontext.PostedPurchaseInvoices.FromSqlRaw(querydata, StartDate, EndDate, site).ToList();
            }

            return postedPurchaseInvoicesDATA;
        }
    }
}