using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using CESAPSCOREWEBAPP.Models;
using static CESAPSCOREWEBAPP.Models.Enums;
using CESAPSCOREWEBAPP.Controllers;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using iTextSharp;
using iTextSharp.text;
using iTextSharp.text.pdf;
using Rotativa.AspNetCore;
using System.Data.SqlClient;
using DevExpress.XtraReports.UI;
using System.IO;
using DevExpress.XtraReports.Parameters;
using CESAPSCOREWEBAPP.Helpers;
using Tesseract;
using OpenCvSharp;
using System.Net;
using CESAPSCOREWEBAPP.Middlewares;

namespace CESAPSCOREWEBAPP.Models
{
    public class HomeController : BaseController
    {

        private readonly NAVContext _navcontext;

        private readonly DatabaseContext _context;

        public HomeController(DatabaseContext context,NAVContext navcontext)
        {
           
            _context = context;
            _navcontext = navcontext;
    

        }




        public IActionResult Serial()
        {
            var data = HardwareInfoMiddleware.CheckRegitry();

            ViewBag.message = data;
            return View();
        }





        public IActionResult Index()
        {



            if (string.IsNullOrEmpty(HttpContext.Session.GetString("Username")))
            {
                Alert("กรุณา Login เข้าสู่ระบบ", NotificationType.error);
                return RedirectToAction("Login", "Accounts");
            }


            var blogPic = _context.BlogPics.ToList();

            ViewData["BlogPic"] = blogPic;

            var blog = _context.Blogs.OrderByDescending(s=>(s.BlogId)).ToList();

            ViewData["blog"] = blog;


            //float a = 147000;
            //float b = -700;
            //float c = 242;
            //float d = 20;
            //Console.WriteLine((((a - (b * -1)) * (d / 100)) * c) / 360);


            return View();



        }

        public IActionResult testExport()
        {
            string pathImage = "/Images/";
            string pathSave = $"wwwroot{pathImage}";
            var FilePath=Path.Combine(Directory.GetCurrentDirectory(), pathSave, "123456789.PNG");

            var Textdata = "";
            //var Ocr = new AutoOcr();
            //var Result = Ocr.Read(FilePath);
            //string imagePath = FilePath;
            //string tessDataFolder = OCROpenCV.DownloadAndExtractLanguagePack();
            //string result = "";
            //using (var engine = new TesseractEngine(tessDataFolder, "eng", EngineMode.Default))
            //{
            //    using (var img = Pix.LoadFromFile(imagePath))
            //    {
            //        var page = engine.Process(img);
            //        result = page.GetText();
            //        Console.WriteLine(result);
            //        Textdata = result;
            //    }
            //}



            string imagePath = FilePath;
            Mat img = Cv2.ImRead(imagePath, ImreadModes.Color);
            Mat imggray= img.CvtColor(ColorConversionCodes.BGR2GRAY);


            string tessDataPath = OCROpenCV.DownloadAndExtractLanguagePack();
            string result = "";
            OpenCvSharp.Rect[] textLocations = null;
            string[] componentTexts = null;
            float[] confidences = null;
            using (var engine = OpenCvSharp.Text.OCRTesseract.Create(tessDataPath, "tha"))
            {
                engine.Run(imggray, out result, out textLocations, out componentTexts, out confidences, OpenCvSharp.Text.ComponentLevels.TextLine);

            }

            Console.WriteLine(result);
            Textdata = result;

            IActionResult response = Unauthorized();
            //var text=PdfTextExtractorData.pdfText("http://intra.ces.co.th/File/Manual/Config%20Docker2019-10-16-09-17-32.pdf");
            var text =  Textdata;
            response = Ok(text);
            return response;
        }

        public IActionResult BlogNew(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            if (string.IsNullOrEmpty(HttpContext.Session.GetString("Username")))
            {
                Alert("กรุณา Login เข้าสู่ระบบ", NotificationType.error);
                return RedirectToAction("Login", "Accounts");
            }


            var blogPic = _context.BlogPics.Where(s => s.BlogId == id).ToList();

            ViewData["BlogPic"] = blogPic;

            var blog = _context.Blogs.Where(s => s.BlogId == id).ToList();

            ViewData["blog"] = blog;


            var blogFile = _context.BlogFiles
                            .Where(s => s.BlogId == id)
                            .ToList();
            ViewData["blogFile"] = blogFile;



            return View();

        }

        public IActionResult test1()
        {

            IActionResult response = Unauthorized();





            var query = "SELECT case WHEN DATEPART(week,GETDATE())=DATEPART(week, AppDate) THEN CONCAT(DATEPART(week, AppDate),' Now') ELSE  CONCAT(DATEPART(week, AppDate),'') END as X,count(DATEPART(week, AppDate)) AS Y  FROM appointments WHERE year(AppDate)=2019 GROUP BY DATEPART(week, AppDate)";
            var a = _context.dataXY.FromSqlRaw(query).ToList();

            response = Ok(a);


            return response;
        }

        public IActionResult test()
        {



            //    var doc = new HtmlToPdfDocument()
            //    {
            //        GlobalSettings = {
            //            ColorMode = ColorMode.Color,
            //            Orientation = Orientation.Landscape,
            //            PaperSize = PaperKind.A4Plus,
            //        },
            //                    Objects = {
            //            new ObjectSettings() {
            //                PagesCount = true,
            //                HtmlContent = @"Lorem ipsum dolor sit amet, consectetur adipiscing elit. In consectetur mauris eget ultrices  iaculis. Utodio viverra, molestie lectus nec, venenatis turpis.",
            //                WebSettings = { DefaultEncoding = "utf-8" },
            //                HeaderSettings = { FontSize = 9, Right = "Page [page] of [toPage]", Line = true, Spacing = 2.812 }
            //            }
            //        }
            //    };


            //    byte[] pdf = _converter.Convert(doc);

            //    return new FileContentResult(pdf, "application/pdf");


            return View();


        }


        public IActionResult CreatePdf()
        {



            return new ViewAsPdf("test");

        }


        public IActionResult Data()
        {
            IActionResult response1 = Unauthorized();

            var request = (HttpWebRequest)WebRequest.Create("http://192.168.18.14:1150/Home/Data");


            request.Method = "GET";
            request.ContentType = "application/json";
            var response = (HttpWebResponse)request.GetResponse();
            StreamReader sr = new StreamReader(response.GetResponseStream());
            var jsonString = sr.ReadToEnd();
            sr.Close();

            response1 = Ok(jsonString);
            return response1;
        }


        public IActionResult DataExcange()
        {
            IActionResult response1 = Unauthorized();

            var request = (HttpWebRequest)WebRequest.Create("http://192.168.18.14:1150/Home/DataExcange");


            request.Method = "GET";
            request.ContentType = "application/json";
            var response = (HttpWebResponse)request.GetResponse();
            StreamReader sr = new StreamReader(response.GetResponseStream());
            var jsonString = sr.ReadToEnd();
            sr.Close();

            response1 = Ok(jsonString);
            return response1;
        }


        public IActionResult Datapr()
        {
            IActionResult response1 = Unauthorized();

            var request = (HttpWebRequest)WebRequest.Create("http://192.168.18.14:1150/Home/Datapr");


            request.Method = "GET";
            request.ContentType = "application/json";
            var response = (HttpWebResponse)request.GetResponse();
            StreamReader sr = new StreamReader(response.GetResponseStream());
            var jsonString = sr.ReadToEnd();
            sr.Close();

            response1 = Ok(jsonString);
            return response1;
        }



        public IActionResult Devexpress()
        {
            //var queryData = "" +
            //    "SELECT 0 as ID,b.Des," +
            //    "SUM(b.NowTotal) as NowTotal, " +
            //    "SUM(b.OldTotal) as OldTotal, " +
            //    "SUM(b.SumTotal) as SumTotal " +
            //    "FROM (SELECT *, " +
            //    "isnull((SELECT SUM([Original Total Cost]) FROM dbo.[C_E_S_ CO_, LTD_$Job Ledger Entry]  " +
            //    "LEFT JOIN dbo.[C_E_S_ CO_, LTD_$Resource] ON dbo.[C_E_S_ CO_, LTD_$Job Ledger Entry].No_ = dbo.[C_E_S_ CO_, LTD_$Resource].No_ " +
            //    "LEFT JOIN dbo.[C_E_S_ CO_, LTD_$Item] ON dbo.[C_E_S_ CO_, LTD_$Resource].[Link to Item No_] = dbo.[C_E_S_ CO_, LTD_$Item].No_ " +
            //    "LEFT JOIN dbo.[C_E_S_ CO_, LTD_$Item Category] ON dbo.[C_E_S_ CO_, LTD_$Item].[Item Category Code] = dbo.[C_E_S_ CO_, LTD_$Item Category].Code " +
            //    "WHERE  " +
            //    "	dbo.[C_E_S_ CO_, LTD_$Job Ledger Entry].[Job No_] =a.JobNo and  " +
            //    "	dbo.[C_E_S_ CO_, LTD_$Job Ledger Entry].[From Location] =a.FromLocation and " +
            //    "	dbo.[C_E_S_ CO_, LTD_$Job Ledger Entry].[To Location] = a.ToLocation and " +
            //    "	dbo.[C_E_S_ CO_, LTD_$Resource].[Link to Item No_] = a.ItemNo and " +
            //    "	dbo.[C_E_S_ CO_, LTD_$Item].[Item Category Code] =a.ItemCat and " +
            //    "	dbo.[C_E_S_ CO_, LTD_$Item Category].Description  = a.Des and  " +
            //    " dbo.[C_E_S_ CO_, LTD_$Job Ledger Entry].[Posting Date]<= {1}),0) as SumTotal, " +
            //    "isnull((SELECT SUM([Original Total Cost]) FROM dbo.[C_E_S_ CO_, LTD_$Job Ledger Entry]  " +
            //    "LEFT JOIN dbo.[C_E_S_ CO_, LTD_$Resource] ON dbo.[C_E_S_ CO_, LTD_$Job Ledger Entry].No_ = dbo.[C_E_S_ CO_, LTD_$Resource].No_ " +
            //    "LEFT JOIN dbo.[C_E_S_ CO_, LTD_$Item] ON dbo.[C_E_S_ CO_, LTD_$Resource].[Link to Item No_] = dbo.[C_E_S_ CO_, LTD_$Item].No_ " +
            //    "LEFT JOIN dbo.[C_E_S_ CO_, LTD_$Item Category] ON dbo.[C_E_S_ CO_, LTD_$Item].[Item Category Code] = dbo.[C_E_S_ CO_, LTD_$Item Category].Code " +
            //    "WHERE  " +
            //    "	dbo.[C_E_S_ CO_, LTD_$Job Ledger Entry].[Job No_] =a.JobNo and  " +
            //    "	dbo.[C_E_S_ CO_, LTD_$Job Ledger Entry].[From Location] =a.FromLocation and " +
            //    "	dbo.[C_E_S_ CO_, LTD_$Job Ledger Entry].[To Location] = a.ToLocation and " +
            //    "	dbo.[C_E_S_ CO_, LTD_$Resource].[Link to Item No_] = a.ItemNo and " +
            //    "	dbo.[C_E_S_ CO_, LTD_$Item].[Item Category Code] =a.ItemCat and " +
            //    "	dbo.[C_E_S_ CO_, LTD_$Item Category].Description  = a.Des and  " +
            //    "   dbo.[C_E_S_ CO_, LTD_$Job Ledger Entry].[Posting Date]< {0}),0) as OldTotal, " +
            //    " isnull((SELECT SUM([Original Total Cost]) FROM dbo.[C_E_S_ CO_, LTD_$Job Ledger Entry] " +
            //    "LEFT JOIN dbo.[C_E_S_ CO_, LTD_$Resource] ON dbo.[C_E_S_ CO_, LTD_$Job Ledger Entry].No_ = dbo.[C_E_S_ CO_, LTD_$Resource].No_ " +
            //    "LEFT JOIN dbo.[C_E_S_ CO_, LTD_$Item] ON dbo.[C_E_S_ CO_, LTD_$Resource].[Link to Item No_] = dbo.[C_E_S_ CO_, LTD_$Item].No_ " +
            //    "LEFT JOIN dbo.[C_E_S_ CO_, LTD_$Item Category] ON dbo.[C_E_S_ CO_, LTD_$Item].[Item Category Code] = dbo.[C_E_S_ CO_, LTD_$Item Category].Code " +
            //    "WHERE  " +
            //    "	dbo.[C_E_S_ CO_, LTD_$Job Ledger Entry].[Job No_] =a.JobNo and  " +
            //    "	dbo.[C_E_S_ CO_, LTD_$Job Ledger Entry].[From Location] =a.FromLocation and " +
            //    "	dbo.[C_E_S_ CO_, LTD_$Job Ledger Entry].[To Location] = a.ToLocation and " +
            //    "	dbo.[C_E_S_ CO_, LTD_$Resource].[Link to Item No_] = a.ItemNo and " +
            //    "	dbo.[C_E_S_ CO_, LTD_$Item].[Item Category Code] =a.ItemCat and " +
            //    "	dbo.[C_E_S_ CO_, LTD_$Item Category].Description  = a.Des and  " +
            //    "   dbo.[C_E_S_ CO_, LTD_$Job Ledger Entry].[Posting Date]>= {0} and " +
            //    "   dbo.[C_E_S_ CO_, LTD_$Job Ledger Entry].[Posting Date]<= {1} ),0) as NowTotal " +
            //    "  FROM( " +
            //    "	SELECT " +
            //    "	dbo.[C_E_S_ CO_, LTD_$Job Ledger Entry].[Job No_] as JobNo, " +
            //    "	dbo.[C_E_S_ CO_, LTD_$Job Ledger Entry].[From Location] as FromLocation, " +
            //    " 	dbo.[C_E_S_ CO_, LTD_$Job Ledger Entry].[To Location] as ToLocation, " +
            //    "	dbo.[C_E_S_ CO_, LTD_$Resource].[Link to Item No_] as ItemNo, " +
            //    "	dbo.[C_E_S_ CO_, LTD_$Item].[Item Category Code] as ItemCat, " +
            //    "	dbo.[C_E_S_ CO_, LTD_$Item Category].Description as Des " +
            //    "FROM " +
            //    "	dbo.[C_E_S_ CO_, LTD_$Job Ledger Entry] " +
            //    "	LEFT JOIN dbo.[C_E_S_ CO_, LTD_$Resource] ON dbo.[C_E_S_ CO_, LTD_$Job Ledger Entry].No_ = dbo.[C_E_S_ CO_, LTD_$Resource].No_ " +
            //    "	LEFT JOIN dbo.[C_E_S_ CO_, LTD_$Item] ON dbo.[C_E_S_ CO_, LTD_$Resource].[Link to Item No_] = dbo.[C_E_S_ CO_, LTD_$Item].No_ " +
            //    "	LEFT JOIN dbo.[C_E_S_ CO_, LTD_$Item Category] ON dbo.[C_E_S_ CO_, LTD_$Item].[Item Category Code] = dbo.[C_E_S_ CO_, LTD_$Item Category].Code " +
            //    "	WHERE " +
            //    "	dbo.[C_E_S_ CO_, LTD_$Job Ledger Entry].[Type of task] = 2 AND " +
            //    "	dbo.[C_E_S_ CO_, LTD_$Job Ledger Entry].Type = 0 AND " +
            //    "	dbo.[C_E_S_ CO_, LTD_$Job Ledger Entry].[Recurring Entry No_] = 0 AND " +
            //    "   dbo.[C_E_S_ CO_, LTD_$Job Ledger Entry].[Posting Date]<= {1} " +
            //    "	GROUP BY " +
            //    "	dbo.[C_E_S_ CO_, LTD_$Job Ledger Entry].[Job No_], " +
            //    " 	dbo.[C_E_S_ CO_, LTD_$Job Ledger Entry].[From Location], " +
            //    "	dbo.[C_E_S_ CO_, LTD_$Job Ledger Entry].[To Location], " +
            //    "	dbo.[C_E_S_ CO_, LTD_$Resource].[Link to Item No_], " +
            //    "	dbo.[C_E_S_ CO_, LTD_$Item].[Item Category Code], " +
            //    "	dbo.[C_E_S_ CO_, LTD_$Item Category].Description) " +
            //    "as a  " +
            //    " WHERE a.ItemCat>='C01' " +
            //    ") as b " +
            //    "GROUP BY " +
            //    " b.ItemCat,b.Des ";


      

            //date1 = "2019-08-01 00:00:00";
            //date2 = "2019-08-31 23:59:59";
            ////ViewBag.sql = queryData;
            ////SqlParameter parameterStart = new SqlParameter("@start", date1);
            ////SqlParameter parameterDate1 = new SqlParameter("@date1", date2);



            //var ReportRentals = _navcontext.ReportRentals.FromSqlRaw(queryData, date1, date2).ToList();



            ////XtraReport report = XtraReport.FromFile("reports\\RentalReport.repx");
            ////report.DataSource = ReportRentals;

            //XtraReport report = XtraReport.FromFile("reports\\RentalReport.repx");
            //report.SourceUrl = "http://intra.ces.co.th/home/Devexpress1";





            //report.CreateDocument(true);
            //var @out = new MemoryStream();
            //report.ExportToPdf(@out);
            //@out.Position = 0;
       


            //return new FileStreamResult(@out, "application/pdf");

            //ViewData["report"] = report;
            //return report;

            //var report = new XtraReport();
            //report.Name =  "ReportRental";
            //report.DataSource = ReportRentals;
            //var cachedReportSource = new CachedReportSourceWeb(report);
            return View();
            //return View(report);




            //return View();


        }


        public IActionResult Devexpress1()
        {
            string date1 = "";
            string date2 = "";

            IActionResult response = Unauthorized();
            var queryData = "" +
                "SELECT 0 as ID,b.Des," +
                "SUM(b.NowTotal) as NowTotal, " +
                "SUM(b.OldTotal) as OldTotal, " +
                "SUM(b.SumTotal) as SumTotal " +
                "FROM (SELECT *, " +
                "isnull((SELECT SUM([Original Total Cost]) FROM dbo.[C_E_S_ CO_, LTD_$Job Ledger Entry]  " +
                "LEFT JOIN dbo.[C_E_S_ CO_, LTD_$Resource] ON dbo.[C_E_S_ CO_, LTD_$Job Ledger Entry].No_ = dbo.[C_E_S_ CO_, LTD_$Resource].No_ " +
                "LEFT JOIN dbo.[C_E_S_ CO_, LTD_$Item] ON dbo.[C_E_S_ CO_, LTD_$Resource].[Link to Item No_] = dbo.[C_E_S_ CO_, LTD_$Item].No_ " +
                "LEFT JOIN dbo.[C_E_S_ CO_, LTD_$Item Category] ON dbo.[C_E_S_ CO_, LTD_$Item].[Item Category Code] = dbo.[C_E_S_ CO_, LTD_$Item Category].Code " +
                "WHERE  " +
                "	dbo.[C_E_S_ CO_, LTD_$Job Ledger Entry].[Job No_] =a.JobNo and  " +
                "	dbo.[C_E_S_ CO_, LTD_$Job Ledger Entry].[From Location] =a.FromLocation and " +
                "	dbo.[C_E_S_ CO_, LTD_$Job Ledger Entry].[To Location] = a.ToLocation and " +
                "	dbo.[C_E_S_ CO_, LTD_$Resource].[Link to Item No_] = a.ItemNo and " +
                "	dbo.[C_E_S_ CO_, LTD_$Item].[Item Category Code] =a.ItemCat and " +
                "	dbo.[C_E_S_ CO_, LTD_$Item Category].Description  = a.Des and  " +
                " dbo.[C_E_S_ CO_, LTD_$Job Ledger Entry].[Posting Date]<= {1}),0) as SumTotal, " +
                "isnull((SELECT SUM([Original Total Cost]) FROM dbo.[C_E_S_ CO_, LTD_$Job Ledger Entry]  " +
                "LEFT JOIN dbo.[C_E_S_ CO_, LTD_$Resource] ON dbo.[C_E_S_ CO_, LTD_$Job Ledger Entry].No_ = dbo.[C_E_S_ CO_, LTD_$Resource].No_ " +
                "LEFT JOIN dbo.[C_E_S_ CO_, LTD_$Item] ON dbo.[C_E_S_ CO_, LTD_$Resource].[Link to Item No_] = dbo.[C_E_S_ CO_, LTD_$Item].No_ " +
                "LEFT JOIN dbo.[C_E_S_ CO_, LTD_$Item Category] ON dbo.[C_E_S_ CO_, LTD_$Item].[Item Category Code] = dbo.[C_E_S_ CO_, LTD_$Item Category].Code " +
                "WHERE  " +
                "	dbo.[C_E_S_ CO_, LTD_$Job Ledger Entry].[Job No_] =a.JobNo and  " +
                "	dbo.[C_E_S_ CO_, LTD_$Job Ledger Entry].[From Location] =a.FromLocation and " +
                "	dbo.[C_E_S_ CO_, LTD_$Job Ledger Entry].[To Location] = a.ToLocation and " +
                "	dbo.[C_E_S_ CO_, LTD_$Resource].[Link to Item No_] = a.ItemNo and " +
                "	dbo.[C_E_S_ CO_, LTD_$Item].[Item Category Code] =a.ItemCat and " +
                "	dbo.[C_E_S_ CO_, LTD_$Item Category].Description  = a.Des and  " +
                "   dbo.[C_E_S_ CO_, LTD_$Job Ledger Entry].[Posting Date]< {0}),0) as OldTotal, " +
                " isnull((SELECT SUM([Original Total Cost]) FROM dbo.[C_E_S_ CO_, LTD_$Job Ledger Entry] " +
                "LEFT JOIN dbo.[C_E_S_ CO_, LTD_$Resource] ON dbo.[C_E_S_ CO_, LTD_$Job Ledger Entry].No_ = dbo.[C_E_S_ CO_, LTD_$Resource].No_ " +
                "LEFT JOIN dbo.[C_E_S_ CO_, LTD_$Item] ON dbo.[C_E_S_ CO_, LTD_$Resource].[Link to Item No_] = dbo.[C_E_S_ CO_, LTD_$Item].No_ " +
                "LEFT JOIN dbo.[C_E_S_ CO_, LTD_$Item Category] ON dbo.[C_E_S_ CO_, LTD_$Item].[Item Category Code] = dbo.[C_E_S_ CO_, LTD_$Item Category].Code " +
                "WHERE  " +
                "	dbo.[C_E_S_ CO_, LTD_$Job Ledger Entry].[Job No_] =a.JobNo and  " +
                "	dbo.[C_E_S_ CO_, LTD_$Job Ledger Entry].[From Location] =a.FromLocation and " +
                "	dbo.[C_E_S_ CO_, LTD_$Job Ledger Entry].[To Location] = a.ToLocation and " +
                "	dbo.[C_E_S_ CO_, LTD_$Resource].[Link to Item No_] = a.ItemNo and " +
                "	dbo.[C_E_S_ CO_, LTD_$Item].[Item Category Code] =a.ItemCat and " +
                "	dbo.[C_E_S_ CO_, LTD_$Item Category].Description  = a.Des and  " +
                "   dbo.[C_E_S_ CO_, LTD_$Job Ledger Entry].[Posting Date]>= {0} and " +
                "   dbo.[C_E_S_ CO_, LTD_$Job Ledger Entry].[Posting Date]<= {1} ),0) as NowTotal " +
                "  FROM( " +
                "	SELECT " +
                "	dbo.[C_E_S_ CO_, LTD_$Job Ledger Entry].[Job No_] as JobNo, " +
                "	dbo.[C_E_S_ CO_, LTD_$Job Ledger Entry].[From Location] as FromLocation, " +
                " 	dbo.[C_E_S_ CO_, LTD_$Job Ledger Entry].[To Location] as ToLocation, " +
                "	dbo.[C_E_S_ CO_, LTD_$Resource].[Link to Item No_] as ItemNo, " +
                "	dbo.[C_E_S_ CO_, LTD_$Item].[Item Category Code] as ItemCat, " +
                "	dbo.[C_E_S_ CO_, LTD_$Item Category].Description as Des " +
                "FROM " +
                "	dbo.[C_E_S_ CO_, LTD_$Job Ledger Entry] " +
                "	LEFT JOIN dbo.[C_E_S_ CO_, LTD_$Resource] ON dbo.[C_E_S_ CO_, LTD_$Job Ledger Entry].No_ = dbo.[C_E_S_ CO_, LTD_$Resource].No_ " +
                "	LEFT JOIN dbo.[C_E_S_ CO_, LTD_$Item] ON dbo.[C_E_S_ CO_, LTD_$Resource].[Link to Item No_] = dbo.[C_E_S_ CO_, LTD_$Item].No_ " +
                "	LEFT JOIN dbo.[C_E_S_ CO_, LTD_$Item Category] ON dbo.[C_E_S_ CO_, LTD_$Item].[Item Category Code] = dbo.[C_E_S_ CO_, LTD_$Item Category].Code " +
                "	WHERE " +
                "	dbo.[C_E_S_ CO_, LTD_$Job Ledger Entry].[Type of task] = 2 AND " +
                "	dbo.[C_E_S_ CO_, LTD_$Job Ledger Entry].Type = 0 AND " +
                "	dbo.[C_E_S_ CO_, LTD_$Job Ledger Entry].[Recurring Entry No_] = 0 AND " +
                "   dbo.[C_E_S_ CO_, LTD_$Job Ledger Entry].[Posting Date]<= {1} " +
                "	GROUP BY " +
                "	dbo.[C_E_S_ CO_, LTD_$Job Ledger Entry].[Job No_], " +
                " 	dbo.[C_E_S_ CO_, LTD_$Job Ledger Entry].[From Location], " +
                "	dbo.[C_E_S_ CO_, LTD_$Job Ledger Entry].[To Location], " +
                "	dbo.[C_E_S_ CO_, LTD_$Resource].[Link to Item No_], " +
                "	dbo.[C_E_S_ CO_, LTD_$Item].[Item Category Code], " +
                "	dbo.[C_E_S_ CO_, LTD_$Item Category].Description) " +
                "as a  " +
                " WHERE a.ItemCat>='C01' " +
                ") as b " +
                "GROUP BY " +
                " b.ItemCat,b.Des ";




            date1 = "2019-08-01 00:00:00";
            date2 = "2019-08-31 23:59:59";
            //ViewBag.sql = queryData;
            //SqlParameter parameterStart = new SqlParameter("@start", date1);
            //SqlParameter parameterDate1 = new SqlParameter("@date1", date2);



            var ReportRentals = _navcontext.ReportRentals.FromSqlRaw(queryData, date1, date2).ToList();



        
            //ViewData["report"] = report;
            //return report;

            //var report = new XtraReport();
            //report.Name =  "ReportRental";
            //report.DataSource = ReportRentals;
            //var cachedReportSource = new CachedReportSourceWeb(report);
            //return View();
            //return View(report);
            response = Ok(ReportRentals);
            return response;


            //return View();

        }


        public IActionResult AllRental()
        {


            IActionResult response = Unauthorized();
            var queryData = "" +
                "SELECT ROW_NUMBER() OVER (ORDER BY dbo.[C_E_S_ CO_, LTD_$Job Ledger Entry].[Posting Date]) as ID," +
                "dbo.[C_E_S_ CO_, LTD_$Job Ledger Entry].[Posting Date] as PostingDate, " +
                "dbo.[C_E_S_ CO_, LTD_$Job Ledger Entry].[Job No_] as JobNo, " +
                "dbo.[C_E_S_ CO_, LTD_$Job Ledger Entry].[From Location] as FromLocation, " +
                "dbo.[C_E_S_ CO_, LTD_$Job Ledger Entry].[To Location] as ToLocation, " +
                "dbo.[C_E_S_ CO_, LTD_$Resource].[Link to Item No_] as ItemNo, " +
                "dbo.[C_E_S_ CO_, LTD_$Item].[Item Category Code] as ItemCat, " +
                "dbo.[C_E_S_ CO_, LTD_$Item Category].Description as Des, " +
                "dbo.[C_E_S_ CO_, LTD_$Job Ledger Entry].[Original Total Cost] as TotalCost " +
                "FROM " +
                "dbo.[C_E_S_ CO_, LTD_$Job Ledger Entry] " +
                "INNER JOIN dbo.[C_E_S_ CO_, LTD_$Resource] ON dbo.[C_E_S_ CO_, LTD_$Job Ledger Entry].No_ = dbo.[C_E_S_ CO_, LTD_$Resource].No_ " +
                "INNER JOIN dbo.[C_E_S_ CO_, LTD_$Item] ON dbo.[C_E_S_ CO_, LTD_$Resource].[Link to Item No_] = dbo.[C_E_S_ CO_, LTD_$Item].No_ " +
                "INNER JOIN dbo.[C_E_S_ CO_, LTD_$Item Category] ON dbo.[C_E_S_ CO_, LTD_$Item].[Item Category Code] = dbo.[C_E_S_ CO_, LTD_$Item Category].Code " +
                "WHERE " +
                "dbo.[C_E_S_ CO_, LTD_$Job Ledger Entry].[Type of task] = 2 AND " +
                "dbo.[C_E_S_ CO_, LTD_$Job Ledger Entry].Type = 0 AND " +
                "dbo.[C_E_S_ CO_, LTD_$Job Ledger Entry].[Recurring Entry No_] = 0 ";
               




            //ViewBag.sql = queryData;




            var DetailRentals = _navcontext.DetailRentals.FromSqlRaw(queryData).ToList();




            //ViewData["report"] = report;
            //return report;

            //var report = new XtraReport();
            //report.Name =  "ReportRental";
            //report.DataSource = ReportRentals;
            //var cachedReportSource = new CachedReportSourceWeb(report);
            //return View();
            //return View(report);
            response = Ok(DetailRentals);
            return response;


            //return View();

        }


        public async Task<IActionResult> HouseReport()
        {





            var ReportRentals = await _context.HouseRentals.ToListAsync();


            XtraReport report = XtraReport.FromFile("reports\\XtraReport.repx");
            report.DataSource = ReportRentals;

            //XtraReport report = XtraReport.FromFile("reports\\RentalReport.repx");
            //report.SourceUrl = "http://intra.ces.co.th/home/Devexpress1";





            report.CreateDocument(true);
            var @out = new MemoryStream();
            report.ExportToPdf(@out);
            @out.Position = 0;

            

            return new FileStreamResult(@out, "application/pdf");

            //ViewData["report"] = report;
            //return report;

            //var report = new XtraReport();
            //report.Name =  "ReportRental";
            //report.DataSource = ReportRentals;
            //var cachedReportSource = new CachedReportSourceWeb(report);
           // return View();
            //return View(report);




            //return View();


        }



    }
}



