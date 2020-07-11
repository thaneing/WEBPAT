using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using CESAPSCOREWEBAPP.Models;
using System.Data.SqlClient;
using System.Globalization;
using Microsoft.AspNetCore.Http;
using System.IO;
using static CESAPSCOREWEBAPP.Models.Enums;
using CESAPSCOREWEBAPP.Helpers;
using MimeKit;
using Microsoft.Extensions.Configuration;
using System.Text;
using Rotativa.AspNetCore;
using DevExpress.XtraReports.UI;
using Microsoft.AspNetCore.Authorization;

namespace CESAPSCOREWEBAPP.Controllers
{
    [Authorize]
    public class HouseRentalsController : BaseController
    {
        private readonly DatabaseContext _context;
        private IConfiguration _config { get; }


        public HouseRentalsController(DatabaseContext context, IConfiguration config)
        {
            _context = context;
            _config = config;
        }

        // GET: HouseRentals
        public IActionResult Index()
        {
          
            /*Check Session */
            var page = "244";
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

            List<UserJob> instances = new List<UserJob>();
            UserJob current = null;
            current = new UserJob();
            current.UserId = 0;
            current.UserJobId = 0;
            current.UserJobDetail = "All";
            instances.Add(current);

            var jobdata = _context.UserJobs.Where(s => s.UserId == HttpContext.Session.GetInt32("Userid")).OrderBy(s => s.UserJobDetail).ToList();
            foreach (var std in jobdata as IList<UserJob>)
            {
                current = new UserJob();
                current = std;
                instances.Add(current);
            }
            var job = new SelectList(instances, "UserJobDetail", "UserJobDetail");

            ViewData["JobNo"] = job;

            var job2 = new SelectList(_context.UserJobs.Where(s => s.UserId == HttpContext.Session.GetInt32("Userid")).OrderBy(s => s.UserJobDetail).ToList(), "UserJobDetail", "UserJobDetail");
            ViewData["JobNo2"] = job2;

            ViewBag.StartDate = DateTime.Now.ToString("yyyy-MM", new CultureInfo("en-US"));
            ViewBag.StartDateCreate = DateTime.Now.ToString("dd-MM-yyyy", new CultureInfo("en-US"));

            return View();
        }


        public IActionResult GetData(string Job, string Month, int id)
        {
            /*Check Session */
            var page = "244";
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
            var lastday = DateTime.DaysInMonth(Int32.Parse(Month.Substring(0, 4)), Int32.Parse(Month.Substring(5, 2)));
            var date2 = Convert.ToDateTime(Month.Substring(0, 4) + "-" + Month.Substring(5, 2) + "-" + lastday + " 23:59:59");
            var date1 = Convert.ToDateTime(Month.Substring(0, 4) + "-" + Month.Substring(5, 2) + "-01  00:00:00");
           
            var HouseRentals = new List<HouseRental>();
        
            var jobdata = _context.UserJobs.Where(s => s.UserId == HttpContext.Session.GetInt32("Userid")).OrderBy(s => s.UserJobDetail).ToList();
   
            if (Job == "All")
            {

                if (typeofuser == "3")
                {
                    HouseRentals = _context.HouseRentals.Where(p => p.PostingDate >= date1 && p.PostingDate <= date2).ToList();
                }
                else
                {

                    List<string> site = new List<string>();

                    foreach (var a in jobdata as IList<UserJob>)
                    {

                        site.Add(a.UserJobDetail);


                    }
                    var HouseRentalses = (from s in _context.HouseRentals
                                          where site.Contains(s.Site)
                                          orderby s.HouseName, s.RoomNumber
                                          select s).ToList<HouseRental>();

                    HouseRentals = HouseRentalses.Where(p => p.PostingDate >= date1 && p.PostingDate <= date2).ToList();

                }

            }

            else
            {

                HouseRentals = _context.HouseRentals.Where(p => p.PostingDate >= date1 && p.PostingDate <= date2 && p.Site == Job).ToList();

            }

            List<Tmp_House> instances = new List<Tmp_House>();
            Tmp_House tmp = null;
            foreach (var std in HouseRentals as IList<HouseRental>)
            {
                var HID = std.ID;

                tmp = new Tmp_House();
                tmp.ID = std.ID;
                tmp.EmpId = std.EmpId;
                tmp.EmpName = std.EmpName;
                tmp.EmpPosition = std.EmpPosition;
                tmp.PostingDate = std.PostingDate;
                tmp.Site = std.Site;
                tmp.Deposit = std.Deposit;
                tmp.DepositText = std.DepositText;
                tmp.Advanced = std.Advanced;
                tmp.AdvancedText = std.AdvancedText;
                tmp.Price = std.Price;
                tmp.Thaibath = std.Thaibath;
                tmp.Etc = std.Etc;
                tmp.HouseName = std.HouseName;
                tmp.RoomNumber = std.RoomNumber;
                tmp.CreateBy = std.CreateBy;
                tmp.CreateDate = std.CreateDate;
                tmp.UpdateBy = std.UpdateBy;
                tmp.UpdateDate = std.UpdateDate;
                tmp.Statuss = (int)std.Statuss;
                tmp.TypeRooms = (int)std.TypeRooms;
                tmp.Period = std.Period;
                tmp.Paymentdate = std.Paymentdate;
                tmp.Count = _context.HouseRentalFiles.Where(p => p.ID == HID).Count();
                tmp.RoomPrice = std.RoomPrice;
                instances.Add(tmp);
            }
            //response = Ok(new { /*data = HouseRentals*/data =  });
            response = Ok(new { data = instances,jobdata= jobdata,typeofuser=typeofuser,HouseRental=HouseRentals});


            return response;
        }

        // GET: HouseRentals
        public IActionResult PageApply()
        {
            /*Check Session */
            var page = "248";
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
            List<UserJob> instances = new List<UserJob>();
            UserJob current = null;
            current = new UserJob();
            current.UserId = 0;
            current.UserJobId = 0;
            current.UserJobDetail = "All";
            instances.Add(current);



            var jobdata = _context.UserJobs.Where(s => s.UserId == HttpContext.Session.GetInt32("Userid")).OrderBy(s => s.UserJobDetail).ToList();
            foreach (var std in jobdata as IList<UserJob>)
            {
                current = new UserJob();
                current = std;
                instances.Add(current);
            }
            var job = new SelectList(instances, "UserJobDetail", "UserJobDetail");

            ViewData["JobNo"] = job;


            ViewBag.StartDate = DateTime.Now.ToString("yyyy-MM", new CultureInfo("en-US"));

            
            return View();
        }


        public IActionResult GetForApplyData(string Job, string Month,int status)
        {
            /*Check Session */
            var page = "248";
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
            var lastday = DateTime.DaysInMonth(Int32.Parse(Month.Substring(0, 4)), Int32.Parse(Month.Substring(5, 2)));
            var date2 = Convert.ToDateTime(Month.Substring(0, 4) + "-" + Month.Substring(5, 2) + "-" + lastday + " 23:59:59");
            var date1 = Convert.ToDateTime(Month.Substring(0, 4) + "-" + Month.Substring(5, 2) + "-01  00:00:00");


            var HouseRentals = new List<HouseRental>();
            List<Tmp_House> instances = new List<Tmp_House>();
            Tmp_House tmp = null;

            if (Job == "All")
            {
                HouseRentals = _context.HouseRentals.Where(p => p.PostingDate >= date1 && p.PostingDate <= date2).OrderBy(p => p.HouseName).ThenBy(p => p.RoomNumber).ToList();
            }
            else
            {
                HouseRentals = _context.HouseRentals.Where(p => p.Site == Job && p.PostingDate >= date1 && p.PostingDate <= date2).OrderBy(p => p.HouseName).ThenBy(p=>p.RoomNumber).ToList();
            }
            foreach (var std in HouseRentals as IList<HouseRental>)
            {
                var HID = std.ID;

                tmp = new Tmp_House();
                tmp.ID = std.ID;
                tmp.EmpId = std.EmpId;
                tmp.EmpName = std.EmpName;
                tmp.EmpPosition = std.EmpPosition;
                tmp.PostingDate = std.PostingDate;
                tmp.Site = std.Site;
                tmp.Deposit = std.Deposit;
                tmp.DepositText = std.DepositText;
                tmp.Advanced = std.Advanced;
                tmp.AdvancedText = std.AdvancedText;
                tmp.Price = std.Price;
                tmp.Thaibath = std.Thaibath;
                tmp.Etc = std.Etc;
                tmp.HouseName = std.HouseName;
                tmp.RoomNumber = std.RoomNumber;
                tmp.CreateBy = std.CreateBy;
                tmp.CreateDate = std.CreateDate;
                tmp.UpdateBy = std.UpdateBy;
                tmp.UpdateDate = std.UpdateDate;
                tmp.Statuss = (int)std.Statuss;
                tmp.TypeRooms = (int)std.TypeRooms;
                tmp.Period = std.Period;
                tmp.Paymentdate = std.Paymentdate;
                tmp.Count = _context.HouseRentalFiles.Where(p => p.ID == HID).Count();
                tmp.RoomPrice = std.RoomPrice;
                instances.Add(tmp);
            }
            //response = Ok(new { /*data = HouseRentals*/data =  });

            response = Ok(new { data = instances });


            return response;
            ////response = Ok(new { data = HouseRentals });


            ////return response;
        }


        // GET: HouseRentals
        public IActionResult PrintData()
        {
            /*Check Session */
            var page = "250";
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
            List<UserJob> instances = new List<UserJob>();
            UserJob current = null;
            current = new UserJob();
            current.UserId = 0;
            current.UserJobId = 0;
            current.UserJobDetail = "All";
            instances.Add(current);

            var jobdata = _context.UserJobs.Where(s => s.UserId == HttpContext.Session.GetInt32("Userid")).OrderBy(s => s.UserJobDetail).ToList();
            foreach (var std in jobdata as IList<UserJob>)
            {
                current = new UserJob();
                current = std;
                instances.Add(current);
            }
            var job = new SelectList(instances, "UserJobDetail", "UserJobDetail");

            ViewData["JobNo"] = job;
            ViewBag.StartDate = DateTime.Now.ToString("yyyy-MM", new CultureInfo("en-US"));

            return View();
        }



        public IActionResult GetForPrintData(string Job, string Month, int id)
        {
            /*Check Session */
            var page = "250";
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
            var lastday = DateTime.DaysInMonth(Int32.Parse(Month.Substring(0, 4)), Int32.Parse(Month.Substring(5, 2)));
            var date2 = Convert.ToDateTime(Month.Substring(0, 4) + "-" + Month.Substring(5, 2) + "-" + lastday + " 23:59:59");
            var date1 = Convert.ToDateTime(Month.Substring(0, 4) + "-" + Month.Substring(5, 2) + "-01  00:00:00");
            var HouseRentals = new List<HouseRental>();

            if (Job == "All")
            {
                HouseRentals = _context.HouseRentals.Where(p => p.PostingDate >= date1 && p.PostingDate <= date2 && p.Statuss == HouseRental.Status.สำเร็จ).OrderBy(p => p.HouseName).ThenBy(p => p.RoomNumber).ToList();
            }
            else
            {
                HouseRentals = _context.HouseRentals.Where(p => p.Site == Job && p.PostingDate >= date1 && p.PostingDate <= date2 && p.Statuss == HouseRental.Status.สำเร็จ).OrderBy(p => p.HouseName).ThenBy(p => p.RoomNumber).ToList();
            }


            response = Ok(new { data = HouseRentals });


            return response;
        }




        public IActionResult HouseReport(string Job, string Month, int id)//printall
        {
            /*Check Session */
            var page = "250";
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

            var lastday = DateTime.DaysInMonth(Int32.Parse(Month.Substring(0, 4)), Int32.Parse(Month.Substring(5, 2)));
            var date2 = Convert.ToDateTime(Month.Substring(0, 4) + "-" + Month.Substring(5, 2) + "-" + lastday + " 23:59:59");
            var date1 = Convert.ToDateTime(Month.Substring(0, 4) + "-" + Month.Substring(5, 2) + "-01  00:00:00");

            var HouseRentals = new List<HouseRental>();

            if (Job == "All")
            {
                HouseRentals = _context.HouseRentals.Where(p => p.PostingDate >= date1 && p.PostingDate <= date2 && p.Statuss == HouseRental.Status.สำเร็จ).OrderBy(p => p.HouseName).ThenBy(p => p.RoomNumber).ToList();
            }
            else
            {
                HouseRentals = _context.HouseRentals.Where(p => p.Site == Job && p.PostingDate >= date1 && p.PostingDate <= date2 && p.Statuss == HouseRental.Status.สำเร็จ).OrderBy(p => p.HouseName).ThenBy(p => p.RoomNumber).ToList();
            }


            XtraReport report = XtraReport.FromFile("reports\\XtraReport.repx");
            report.DataSource = HouseRentals;


            report.CreateDocument(true);
            var @out = new MemoryStream();
            report.ExportToPdf(@out);
            @out.Position = 0;



            return new FileStreamResult(@out, "application/pdf");

            //return View();


        }
        public IActionResult HouseReportid(string Job, string Month, int id) //print by ID
        {
            /*Check Session */
            var page = "250";
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
            var lastday = DateTime.DaysInMonth(Int32.Parse(Month.Substring(0, 4)), Int32.Parse(Month.Substring(5, 2)));
            var date2 = Convert.ToDateTime(Month.Substring(0, 4) + "-" + Month.Substring(5, 2) + "-" + lastday + " 23:59:59");
            var date1 = Convert.ToDateTime(Month.Substring(0, 4) + "-" + Month.Substring(5, 2) + "-01  00:00:00");

            var HouseRentals = new List<HouseRental>();
            

            if (Job == "All")
            {
                HouseRentals = _context.HouseRentals.Where(p => p.PostingDate >= date1 && p.PostingDate <= date2 && p.Statuss == HouseRental.Status.สำเร็จ && p.ID == id).OrderBy(p => p.HouseName).ThenBy(p => p.RoomNumber).ToList();
            }
            else
            {
                HouseRentals = _context.HouseRentals.Where(p => p.Site == Job && p.PostingDate >= date1 && p.PostingDate <= date2 && p.Statuss == HouseRental.Status.สำเร็จ && p.ID == id).OrderBy(p => p.HouseName).ThenBy(p => p.RoomNumber).ToList();
            }

            XtraReport report = XtraReport.FromFile("reports\\XtraReport.repx");
            report.DataSource = HouseRentals;


            report.CreateDocument(true);
            var @out = new MemoryStream();
            report.ExportToPdf(@out);
            @out.Position = 0;


            //return View();
            return new FileStreamResult(@out, "application/pdf");

            //response = Ok(new { data = HouseRentals });

            //return response;


        }

        public IActionResult ApplyReports(string Job, string Month)//print ธุรการให้ HR
        {
            var lastday = DateTime.DaysInMonth(Int32.Parse(Month.Substring(0, 4)), Int32.Parse(Month.Substring(5, 2)));
            var date2 = Convert.ToDateTime(Month.Substring(0, 4) + "-" + Month.Substring(5, 2) + "-" + lastday + " 23:59:59");
            var date1 = Convert.ToDateTime(Month.Substring(0, 4) + "-" + Month.Substring(5, 2) + "-01  00:00:00");

            var HouseRentals = new List<HouseRental>();

            if (Job == "All")
            {
                HouseRentals = _context.HouseRentals.Where(p =>  p.PostingDate >= date1 && p.PostingDate <= date2 && p.Statuss == HouseRental.Status.สำเร็จ).ToList();
            }
            else
            {
                HouseRentals = _context.HouseRentals.Where(p =>  p.Site == Job && p.PostingDate >= date1 && p.PostingDate <= date2 && p.Statuss == HouseRental.Status.สำเร็จ).OrderBy(p => p.HouseName).ThenBy(p => p.RoomNumber).ToList();
            }
            //HouseRentals = _context.HouseRentals.ToList();

            XtraReport report = XtraReport.FromFile("reports\\ApplyStatus.repx");
            report.DataSource = HouseRentals;


            report.CreateDocument(true);
            var @out = new MemoryStream();
            report.ExportToPdf(@out);
            @out.Position = 0;



            return new FileStreamResult(@out, "application/pdf");

            //return View();


        }
        public IActionResult CheckApplyReports(string Job, string Month)//print ธุรการให้ HR count
        {
            IActionResult response = Unauthorized();
            var lastday = DateTime.DaysInMonth(Int32.Parse(Month.Substring(0, 4)), Int32.Parse(Month.Substring(5, 2)));
            var date2 = Convert.ToDateTime(Month.Substring(0, 4) + "-" + Month.Substring(5, 2) + "-" + lastday + " 23:59:59");
            var date1 = Convert.ToDateTime(Month.Substring(0, 4) + "-" + Month.Substring(5, 2) + "-01  00:00:00");

            var HouseRentals = new List<HouseRental>();

            if (Job == "All")
            {
                HouseRentals = _context.HouseRentals.Where(p =>  p.PostingDate >= date1 && p.PostingDate <= date2 && p.Statuss == HouseRental.Status.สำเร็จ).OrderBy(p => p.HouseName).ThenBy(p => p.RoomNumber).ToList();
            }
            else
            {
                HouseRentals = _context.HouseRentals.Where(p => p.Site == Job && p.PostingDate >= date1 && p.PostingDate <= date2 && p.Statuss == HouseRental.Status.สำเร็จ).OrderBy(p => p.HouseName).ThenBy(p => p.RoomNumber).ToList();
            }
            //HouseRentals = _context.HouseRentals.ToList();


            var count = HouseRentals.Count();

            response = Ok(new { count = count });

            return response;


        }

        // GET: HouseRentals/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            /*Check Session */
            var page = "247";
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

            var houseRental = await _context.HouseRentals.FirstOrDefaultAsync(m => m.ID == id);
            if (houseRental == null)
            {
                return NotFound();
            }

            return View(houseRental);
        }



        // GET: HouseRentals/Create
        public IActionResult Create()
        {
            /*Check Session */
            var page = "245";
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
            var username = "";

            username = HttpContext.Session.GetString("Username");

            ViewBag.StartDate = DateTime.Now.ToString("dd-MM-yyyy", new CultureInfo("en-US"));
            var queryData = "SELECT dbo.Users.EmpId as name,CONCAT(dbo.TitleOfUsers.TitleOfUserName ,dbo.Users.Firstname,' ',dbo.Users.Lastname, ' /    ' ,PositionName, ' /    ' ,BranchName) as code " +
                " FROM dbo.Users  " +
                " INNER JOIN dbo.TitleOfUsers ON dbo.Users.TitleOfUserId = dbo.TitleOfUsers.TitleOfUserId " +
                " INNER JOIN dbo.Organizs ON dbo.Users.organizId = dbo.Organizs.organizId " +
                " INNER JOIN dbo.Positions ON dbo.Organizs.PositionId = dbo.Positions.PositionId " +
                " INNER JOIN dbo.Departments ON dbo.Organizs.DepartmentId = dbo.Departments.DepartmentId " +
                " INNER JOIN dbo.Department1s ON dbo.Organizs.Department1Id = dbo.Department1s.Department1Id " +
                " INNER JOIN dbo.Branchs ON dbo.Users.BranchId = dbo.Branchs.BranchId " +
                " WHERE  LevelId!=10 AND dbo.Users.StatusUserId = 1 AND  dbo.Branchs.BranchName " +
                "   IN(SELECT dbo.UserJobs.UserJobDetail FROM dbo.UserJobs INNER JOIN dbo.Logins ON dbo.UserJobs.UserId = dbo.Logins.UserId  WHERE dbo.Logins.Username = {0})" +
                " order by dbo.Users.Firstname";

            //SqlParameter parameterUsername = new SqlParameter("@username", username);
            var sourceAutoCompletes = _context.SourceAutoCompletes.FromSqlRaw(queryData, username).ToList();
            ViewData["SourceAutoCompletes"] = sourceAutoCompletes;


            return View();
        }

        public async Task<IActionResult> CheckEmployee(string empid, string CheckDate)
        {
            IActionResult response = Unauthorized();
            var lastday = DateTime.DaysInMonth(Int32.Parse(CheckDate.Substring(6, 4)), Int32.Parse(CheckDate.Substring(3, 2)));

            var date1 = Convert.ToDateTime(CheckDate.Substring(6, 4) + "-" + CheckDate.Substring(3, 2) + "-01 00:00:00");
            var date2 = Convert.ToDateTime(CheckDate.Substring(6, 4) + "-" + CheckDate.Substring(3, 2) + "-" + lastday + " 23:59:59");

            //var date2 = Convert.ToDateTime(CheckDate.Substring(0, 4) + "-" + CheckDate.Substring(5, 2) + "-" + lastday + " 23:59:59");
            //var date1 = Convert.ToDateTime(CheckDate.Substring(0, 4) + "-" + CheckDate.Substring(5, 2) + "-01  00:00:00");


            var queryData = "SELECT " +
                "dbo.Users.EmpId AS Code, " +
                "CONCAT(dbo.TitleOfUsers.TitleOfUserName ,dbo.Users.Firstname,' ',dbo.Users.Lastname) AS Name, " +
                "dbo.Positions.PositionName as PositionName, " +
                "dbo.Branchs.BranchName as Site " +
                "FROM " +
                "dbo.Users " +
                "INNER JOIN dbo.TitleOfUsers ON dbo.Users.TitleOfUserId = dbo.TitleOfUsers.TitleOfUserId " +
                "INNER JOIN dbo.Organizs ON dbo.Users.organizId = dbo.Organizs.organizId " +
                "INNER JOIN dbo.Positions ON dbo.Organizs.PositionId = dbo.Positions.PositionId " +
                "INNER JOIN dbo.Departments ON dbo.Organizs.DepartmentId = dbo.Departments.DepartmentId " +
                "INNER JOIN dbo.Department1s ON dbo.Organizs.Department1Id = dbo.Department1s.Department1Id " +
                "INNER JOIN dbo.Branchs ON dbo.Users.BranchId = dbo.Branchs.BranchId " +
                "WHERE dbo.Users.EmpId={0} AND dbo.Users.StatusUserId = 1";
            //SqlParameter parameterEmpId = new SqlParameter("@empid", empid);
            var DetailEmpHouseReports = await _context.DetailEmpHouseReports.FromSqlRaw(queryData, empid).ToListAsync();


            var HouseRentals = _context.HouseRentals.Where(p => p.EmpId == empid).OrderByDescending(s => s.ID).ToList();

            var CheckInMonth = _context.HouseRentals.Where(p => p.EmpId == empid && p.PostingDate >= date1 && p.PostingDate <= date2).ToList();




            response = Ok(new { dataemp = DetailEmpHouseReports, houserental = HouseRentals, CheckInMonth = CheckInMonth.Count });

            //return Ok(DetailEmpHouseReports);
            return response;
        }


        // POST: HouseRentals/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("ID,EmpId,EmpName,EmpPosition,PostingDate,Site,Price,Thaibath,Etc,HouseName,RoomNumber,CreateBy,CreateDate,UpdateBy,UpdateDate,Statuss,TypeRooms,Advanced,Deposit,Period,Paymentdate,DepositText,AdvancedText,RoomPrice")] HouseRental houseRental, string CreateDate, List<IFormFile> filesUpload)
        {
            /*Check Session */
            var page = "245";
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
            var user = HttpContext.Session.GetString("Username");

            if (ModelState.IsValid)
            {
                var date1 = Convert.ToDateTime(CreateDate.Substring(6, 4) + " " + CreateDate.Substring(3, 2) + " " + CreateDate.Substring(0, 2) + " 00:00:00");
                var lastday = DateTime.DaysInMonth(Int32.Parse(CreateDate.Substring(6, 4)), Int32.Parse(CreateDate.Substring(3, 2)));
                string[] thmonth = new string[] { "มกราคม", "กุมภาพันธ์", "มีนาคม", "เมษายน", "พฤษภาคม", "มิถุนายน", "กรกฎาคม", "สิงหาคม", "กันยายน", "ตุลาคม", "พฤศจิกายน", "ธันวาคม" };
                var res = CreateDate.Substring(3, 2);
                var Thailmonth = thmonth[Int32.Parse(res) - 1];

                //System.Globalization.CultureInfo _cultureTHInfo = new System.Globalization.CultureInfo("th-TH");
                //DateTime dateThai = Convert.ToDateTime(date1, _cultureTHInfo);
                //var MonthThai = dateThai.ToString("MMMM", _cultureTHInfo);
                houseRental.Period = Thailmonth + " " + CreateDate.Substring(6, 4);
                houseRental.Paymentdate = lastday.ToString("00") + " " + Thailmonth + " " + CreateDate.Substring(6, 4);


                houseRental.PostingDate = date1;
                houseRental.CreateDate = DateTime.Now;
                houseRental.CreateBy = HttpContext.Session.GetString("Username");
                _context.Add(houseRental);
                await _context.SaveChangesAsync();

                var HID = houseRental.ID;

                if (filesUpload != null && filesUpload.Count > 0)
                {
                    //Upload file
                    string pathImage = "/File/HouseRental/";
                    string pathSave = $"wwwroot{pathImage}";
                    if (!Directory.Exists(pathSave))
                    {
                        Directory.CreateDirectory(pathSave);
                    }
                    foreach (IFormFile item in filesUpload)
                    {
                        if (item.Length > 0)
                        {
                            string fileName = Path.GetFileNameWithoutExtension(item.FileName);
                            string extension = Path.GetExtension(item.FileName);
                            //fileName = fileName + DateTime.Now.ToString("yyyy-MM-dd-hh-mm-ss") + extension;
                            fileName = fileName + DateTime.Now.ToString("yyyyMM-ss") + extension;
                            var path = Path.Combine(Directory.GetCurrentDirectory(), pathSave, fileName);
                            using (var stream = new FileStream(path, FileMode.Create))
                            {
                                await item.CopyToAsync(stream);
                            }
                            var houseFile = new HouseRentalFile { HouseRentalFileName = fileName, HouseRentalFileType = extension, ID = HID, username = user };


                            _context.HouseRentalFiles.Add(houseFile);
                            _context.SaveChanges();
                        }
                    }

                }


                return RedirectToAction(nameof(Index));
            }

            return View(houseRental);
        }

        // GET: HouseRentals/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            /*Check Session */
            var page = "246";
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

            var queryData = "SELECT dbo.Users.EmpId as name,CONCAT(dbo.TitleOfUsers.TitleOfUserName ,dbo.Users.Firstname,' ',dbo.Users.Lastname, ' ตำแหน่ง  :   ' ,PositionName) as code " +
               " FROM dbo.Users  " +
               " INNER JOIN dbo.TitleOfUsers ON dbo.Users.TitleOfUserId = dbo.TitleOfUsers.TitleOfUserId " +
               " INNER JOIN dbo.Organizs ON dbo.Users.organizId = dbo.Organizs.organizId " +
               " INNER JOIN dbo.Positions ON dbo.Organizs.PositionId = dbo.Positions.PositionId " +
               " INNER JOIN dbo.Departments ON dbo.Organizs.DepartmentId = dbo.Departments.DepartmentId " +
               " INNER JOIN dbo.Department1s ON dbo.Organizs.Department1Id = dbo.Department1s.Department1Id " +
               " WHERE  LevelId!=10 order by dbo.Users.Firstname";
            var sourceAutoCompletes = _context.SourceAutoCompletes.FromSqlRaw(queryData).ToList();
            ViewData["SourceAutoCompletes"] = sourceAutoCompletes;

            var houseRental = await _context.HouseRentals.FindAsync(id);
            if (houseRental == null)
            {
                return NotFound();
            }

            if (houseRental.Statuss != HouseRental.Status.รอ)
            {
                Alert("คุณไม่มีสิทธิ์แก้ไขรายการดังกล่าว", NotificationType.error);
                return RedirectToAction(nameof(Index));
            }

            ViewBag.CreateDate = houseRental.CreateDate.ToString("dd-MM-yyyy HH:mm:ss", new CultureInfo("en-US"));

            ViewBag.StartDate = houseRental.PostingDate.ToString("dd-MM-yyyy", new CultureInfo("en-US"));

            ViewBag.EmpId = houseRental.EmpId;

            //var file = _context.HouseRentalFiles
            // .Where(s => s.HouseRentalFileID == id)
            // .ToList();
            //ViewData["file"] = file;

            return View(houseRental);
        }


        // POST: HouseRentals/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.

        public IActionResult EditStatus(string id, string status)
        {
            /*Check Session */
            var page = "248";
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

            var identity = Int32.Parse(status);
            string [] codelist = id.Split(",");
            string strId = "";
            int code = 0;
            var list = "";
           List<HouseRental> houseRentals = new List<HouseRental>();

            for(var i=0;i<codelist.Length;i++)
            {
                strId = codelist[i];
                code = Int32.Parse(strId);
        
                //Int32.TryParse(strId);
                houseRentals = new List<HouseRental>();
                houseRentals  = _context.HouseRentals.Where(p=>p.ID== code).ToList();
            
                foreach (var houseRental in houseRentals)
                {
                   list =""+ houseRental.ID +" ," + houseRental.EmpName;

                    if (identity == 0)
                    {
                        houseRental.Statuss = HouseRental.Status.รอ;
                    }
                    else if (identity == 1)
                    {
                        houseRental.Statuss = HouseRental.Status.สำเร็จ;
                    }
                    else
                    {
                        houseRental.Statuss = HouseRental.Status.ไม่สำเร็จ;
                    }


                    _context.HouseRentals.Update(houseRental);
                    _context.SaveChanges();

                }
              

            }

            response = Ok(new { strId = strId, identity=identity,list=list,code = code});

            return response;


        }


        public IActionResult Checkstring(string id,string status)//print รายงานของเลขา count
        {
            IActionResult response = Unauthorized();

            var identity = Int32.Parse(status);
            string[] codelist = id.Split(",");
            string strId = "";
            var code = 0;
            foreach (string codelists in codelist)
            {
                strId += codelists;

                code = Int32.Parse(strId);

         
            }


            response = Ok(new { strId = strId , code = code, status = status , identity = identity });

            return response;


        }



        // POST: HouseRentals/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("ID,EmpId,EmpName,EmpPosition,PostingDate,Site,Price,Thaibath,Etc,HouseName,RoomNumber,CreateBy,CreateDate,UpdateBy,UpdateDate,Statuss,TypeRooms,Advanced,Deposit,Period,Paymentdate,DepositText,AdvancedText,RoomPrice")] HouseRental houseRental, string CreateDate, List<IFormFile> filesUpload)
        {
            /*Check Session */
            var page = "246";
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

            if (id != houseRental.ID)
            {
                return NotFound();
            }

            if (houseRental.Statuss != HouseRental.Status.รอ)
            {
                Alert("คุณไม่มีสิทธิ์แก้ไขรายการดังกล่าว", NotificationType.error);
                return RedirectToAction(nameof(Index));
            }

            var queryData = "SELECT dbo.Users.EmpId as name,CONCAT(dbo.TitleOfUsers.TitleOfUserName ,dbo.Users.Firstname,' ',dbo.Users.Lastname, ' ตำแหน่ง  :   ' ,PositionName) as code " +
             " FROM dbo.Users  " +
             " INNER JOIN dbo.TitleOfUsers ON dbo.Users.TitleOfUserId = dbo.TitleOfUsers.TitleOfUserId " +
             " INNER JOIN dbo.Organizs ON dbo.Users.organizId = dbo.Organizs.organizId " +
             " INNER JOIN dbo.Positions ON dbo.Organizs.PositionId = dbo.Positions.PositionId " +
             " INNER JOIN dbo.Departments ON dbo.Organizs.DepartmentId = dbo.Departments.DepartmentId " +
             " INNER JOIN dbo.Department1s ON dbo.Organizs.Department1Id = dbo.Department1s.Department1Id " +
             " WHERE  LevelId!=10  order by dbo.Users.Firstname";
            var sourceAutoCompletes = _context.SourceAutoCompletes.FromSqlRaw(queryData).ToList();
            ViewData["SourceAutoCompletes"] = sourceAutoCompletes;
            //var houseRentaltmp = _context.HouseRentals.Where(s => s.ID == houseRental.ID).ToList();
            //houseRental.CreateDate = houseRentaltmp[0].CreateDate;

            if (ModelState.IsValid)
            {
                try
                {
                    var lastday = DateTime.DaysInMonth(Int32.Parse(CreateDate.Substring(6, 4)), Int32.Parse(CreateDate.Substring(3, 2)));
                    var date1 = Convert.ToDateTime(CreateDate.Substring(6, 4) + "-" + CreateDate.Substring(3, 2) + "-" + CreateDate.Substring(0, 2) + " 00:00:00");
                    string[] thmonth = new string[] { "มกราคม", "กุมภาพันธ์", "มีนาคม", "เมษายน", "พฤษภาคม", "มิถุนายน", "กรกฎาคม", "สิงหาคม", "กันยายน", "ตุลาคม", "พฤศจิกายน", "ธันวาคม" };
                    var res = CreateDate.Substring(3, 2);
                    var Thailmonth = thmonth[Int32.Parse(res) - 1];

                    //System.Globalization.CultureInfo _cultureTHInfo = new System.Globalization.CultureInfo("th-TH");
                    //DateTime dateThai = Convert.ToDateTime(date1);
                    //var MonthThai = dateThai.ToString("MMMM");
                    houseRental.Period = Thailmonth + " " + CreateDate.Substring(6, 4);
                    houseRental.Paymentdate = lastday.ToString("00") + " " + Thailmonth + " " + CreateDate.Substring(6, 4);

                    //houseRental.CreateDate = date2;
                    houseRental.PostingDate = date1;
                    houseRental.UpdateDate = DateTime.Now;
                    houseRental.UpdateBy = HttpContext.Session.GetString("Username");

                    var user = HttpContext.Session.GetString("Username");

                    _context.Update(houseRental);
                    await _context.SaveChangesAsync();

                    var HID = houseRental.ID;

                    if (filesUpload != null && filesUpload.Count > 0)
                    {
                        //Upload file
                        string pathImage = "/File/HouseRental/";
                        string pathSave = $"wwwroot{pathImage}";
                        if (!Directory.Exists(pathSave))
                        {
                            Directory.CreateDirectory(pathSave);
                        }
                        foreach (IFormFile item in filesUpload)
                        {
                            if (item.Length > 0)
                            {
                                string fileName = Path.GetFileNameWithoutExtension(item.FileName);
                                string extension = Path.GetExtension(item.FileName);
                                //fileName = fileName + DateTime.Now.ToString("yyyy-MM-dd-hh-mm-ss") + extension;
                                fileName = fileName + DateTime.Now.ToString("yyyyMM-ss") + extension;
                                var path = Path.Combine(Directory.GetCurrentDirectory(), pathSave, fileName);
                                using (var stream = new FileStream(path, FileMode.Create))
                                {
                                    await item.CopyToAsync(stream);
                                }
                                var houseFile = new HouseRentalFile { HouseRentalFileName = fileName, HouseRentalFileType = extension, ID = HID, username = user };


                                _context.HouseRentalFiles.Add(houseFile);
                                _context.SaveChanges();
                            }
                        }

                    }

                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!HouseRentalExists(houseRental.ID))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            return View(houseRental);
        }

        // GET: HouseRentals/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            /*Check Session */
            var page = "241";
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

            var houseRental = await _context.HouseRentals
                .FirstOrDefaultAsync(m => m.ID == id);
            if (houseRental == null)
            {
                return NotFound();
            }

            return View(houseRental);
        }

        // POST: HouseRentals/Delete/5
        [HttpPost, ActionName("DeleteRantal")]
        //[ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteRantal(int id)
        {
            /*Check Session */
            var page = "241";
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


            var houseRental = await _context.HouseRentals.FindAsync(id);
            var name = houseRental.HouseName;


            _context.HouseRentals.Remove(houseRental);
            await _context.SaveChangesAsync();

            response = Ok(new { name = name });
            return response;
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Upload(List<IFormFile> filesUpload, int ID)
        {
            /*Check Session */
            var page = "243";
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
            var user = HttpContext.Session.GetString("Username");

            if (ModelState.IsValid)
            {
                //var IDHOUSE = houseRental.ID;
                //upload File

                if (filesUpload != null && filesUpload.Count > 0)
                {
                    //Upload file
                    string pathImage = "/File/HouseRental/";
                    string pathSave = $"wwwroot{pathImage}";
                    if (!Directory.Exists(pathSave))
                    {
                        Directory.CreateDirectory(pathSave);
                    }
                    foreach (IFormFile item in filesUpload)
                    {
                        if (item.Length > 0)
                        {
                            string fileName = Path.GetFileNameWithoutExtension(item.FileName);
                            string extension = Path.GetExtension(item.FileName);
                            //fileName = fileName + DateTime.Now.ToString("yyyy-MM-dd-hh-mm-ss") + extension;
                            fileName = fileName + DateTime.Now.ToString("yyyyMM-ss") + extension;
                            var path = Path.Combine(Directory.GetCurrentDirectory(), pathSave, fileName);
                            using (var stream = new FileStream(path, FileMode.Create))
                            {
                                await item.CopyToAsync(stream);
                            }
                            var houseFile = new HouseRentalFile { HouseRentalFileName = fileName, HouseRentalFileType = extension, ID = ID, username = user };


                            _context.HouseRentalFiles.Add(houseFile);
                            _context.SaveChanges();
                        }
                    }
                    return RedirectToAction(nameof(Index));
                }
            }
            return View();

        }
        public IActionResult Showfile(int id)
        {

            IActionResult response = Unauthorized();

            var show = _context.HouseRentalFiles.Where(c => c.ID == id).ToList();

            var bodylist = "";
            var bodyAdmin = "";
            //int i = 0;

            foreach (var std in show as IList<HouseRentalFile>)
            {
                //bodylist += "<div class='col-lg-12'>";

                bodylist += "<p></span><a href='/File/HouseRental/" + std.HouseRentalFileName + "' data-toggle='tooltip' data-placement='top' title='" + std.HouseRentalFileName + "'><span><img src='/Images/folder.png'></span> " + std.HouseRentalFileName + "</a> <a data-id-type=" + std.HouseRentalFileID + " onclick='DeleteFile(this);'><span style=' color: red '><i class='fa fa-times'></i></span></a><p/> ";
                //i = i + 1;


            }

            var listFile = bodylist;
            var listAdmin = bodyAdmin;

            response = Ok(new
            {
                listFile = listFile,



            });
            return response;
        }
        public IActionResult Viewfile(int id)
        {

            IActionResult response = Unauthorized();

            var show = _context.HouseRentalFiles.Where(c => c.ID == id).ToList();

            var bodylist = "";
            var count = show.Count();

            foreach (var std in show as IList<HouseRentalFile>)
            {
                //bodylist += "<div class='col-lg-12'>";

                bodylist += "<p></span><a href='/File/HouseRental/" + std.HouseRentalFileName + "' data-toggle='tooltip' data-placement='top' title='" + std.HouseRentalFileName + "'><span><img src='/Images/folder.png'></span> " + std.HouseRentalFileName + "</a>";
            }

            var listFile = bodylist;

            
            response = Ok(new
            {
                listFile = listFile,count=count


            });
            return response;
        }
        // POST: Blogs/Deletefile/5
        [HttpPost, ActionName("Deletefile")]
        public async Task<IActionResult> Deletefile(int id)
        {
            /*Check Session */
            var page = "242";
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
            var userdo = HttpContext.Session.GetString("Username");

            var Files = await _context.HouseRentalFiles.FindAsync(id);
            var name = Files.HouseRentalFileName;
            var user = Files.username;

            if (user != userdo)
            {
                Alert("คุณไม่มีสิทธิ์ลบไฟล์ดังกล่าว", NotificationType.error);
                return RedirectToAction("Index");
            }
            else
            {
                _context.HouseRentalFiles.Remove(Files);
                await _context.SaveChangesAsync();

                response = Ok(new { name = name });
                return response;
            }

        }
        private bool HouseRentalExists(int id)
        {
            return _context.HouseRentals.Any(e => e.ID == id);
        }
        //upload for month & job
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> UploadMonth(List<IFormFile> filesUpload2, string joba, string month, HouseRentalFileMonth houseRentalFileMonth)
        {
            /*Check Session */
            var page = "239";
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

            var lastday = DateTime.DaysInMonth(Int32.Parse(month.Substring(0, 4)), Int32.Parse(month.Substring(5, 2)));
            var date1 = Convert.ToDateTime(month.Substring(0, 4) + "-" + month.Substring(5, 2) + "-" + lastday + " 00:00:00");


            System.Globalization.CultureInfo _cultureTHInfo = new System.Globalization.CultureInfo("th-TH");
            DateTime dateThai = Convert.ToDateTime(date1, _cultureTHInfo);
            var MonthThai = dateThai.ToString("MMMM", _cultureTHInfo);
            var Period = MonthThai + " " + month.Substring(0, 4);


            //houseRentalFileMonth.FileMonthDate = date1;

            if (ModelState.IsValid)
            {
                var date = houseRentalFileMonth.FileMonthDate;

                //upload File

                if (filesUpload2 != null && filesUpload2.Count > 0)
                {
                    //Upload file
                    string pathImage = "/File/HouseRental/";
                    string pathSave = $"wwwroot{pathImage}";
                    if (!Directory.Exists(pathSave))
                    {
                        Directory.CreateDirectory(pathSave);
                    }
                    foreach (IFormFile item in filesUpload2)
                    {

                        if (item.Length > 0)
                        {

                            string fileName = Path.GetFileNameWithoutExtension(item.FileName);
                            string extension = Path.GetExtension(item.FileName);
                            fileName = fileName + DateTime.Now.ToString("yyyyMM-ss") + extension;
                            var path = Path.Combine(Directory.GetCurrentDirectory(), pathSave, fileName);

                            using (var stream = new FileStream(path, FileMode.Create))
                            {
                                await item.CopyToAsync(stream);
                            }
                            var fileMonth = new HouseRentalFileMonth { FileMonthName = fileName, FileMonthType = extension, FileMonthDate = date1, Job = joba,Period = Period };


                            _context.HouseRentalFileMonths.Add(fileMonth);
                            _context.SaveChanges();

                        }
                    }
                    return RedirectToAction(nameof(Index));
                }
            }
            return View();

        }
        public IActionResult ShowfileMonth(string Job, string Month)
        {
            IActionResult response = Unauthorized();
            var lastday = DateTime.DaysInMonth(Int32.Parse(Month.Substring(0, 4)), Int32.Parse(Month.Substring(5, 2)));
            var date2 = Convert.ToDateTime(Month.Substring(0, 4) + "-" + Month.Substring(5, 2) + "-" + lastday + " 23:59:59");
            var date1 = Convert.ToDateTime(Month.Substring(0, 4) + "-" + Month.Substring(5, 2) + "-01  00:00:00");

            var show = _context.HouseRentalFileMonths.Where(c => c.Job == Job && c.FileMonthDate >= date1 && c.FileMonthDate <= date2).ToList();

            var bodylist = "";
            var bodyAdmin = "";
            bodylist += "<div style='border - radius: 10px; border: 1px dashed #c4dff6;'>";
            foreach (var std in show as IList<HouseRentalFileMonth>)
            {


                bodylist += "<p></span  style='padding: 2px 2px 2px 5px'><a href='/File/HouseRental/" + std.FileMonthName + "' data-toggle='tooltip' data-placement='top' title='" + std.FileMonthName + "'><span><img src='/Images/file.png' width ='12px'></span>" + std.FileMonthName + " </a> <a data-id-type=" + std.FileMonthID + " onclick='DeleteFileMonth(this);'><span style=' color: red '><i class='fa fa-times'></i></span></a><p/> ";

                bodyAdmin += "<p></span  style='padding: 2px 2px 2px 5px'><a href='/File/HouseRental/" + std.FileMonthName + "' data-toggle='tooltip' data-placement='top' title='" + std.FileMonthName + "'><span><img src='/Images/file.png' width ='12px'> </span>" + std.FileMonthName + " </a><p/> ";



            }
            bodylist += "</div>";
            var listshow = bodylist;
            var listAdmin = bodyAdmin;

            response = Ok(new
            {
                listshow = listshow,
                listAdmin = listAdmin


            });
            return response;
        }
        [HttpPost, ActionName("DeletefileMonth")]
        public async Task<IActionResult> DeletefileMonth(int id)
        {

            /*Check Session */
            var page = "240";
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


            var Files = await _context.HouseRentalFileMonths.FindAsync(id);
            var name = Files.FileMonthName;

            _context.HouseRentalFileMonths.Remove(Files);
            await _context.SaveChangesAsync();

            response = Ok(new { name = name });
            return response;
        }


        public async Task<IActionResult> ShowDetails(int? id)
        {
            /*Check Session */
            var page = "247";
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

            var details = await _context.HouseRentals.FirstOrDefaultAsync(m => m.ID == id);

            var EmpName = details.EmpName;
            var EmpID = details.EmpId;
            var EmpPosition = details.EmpPosition;
            var Site = details.Site;
            var HouseName = details.HouseName;
            var RoomNumber = details.RoomNumber;
            var PostingDate = details.PostingDate.ToString("dd-MMMM-yyyy");
            var Price = details.Price.ToString("#,##0.00");//ค่าเช่า
            var Deposit = details.Deposit.ToString("#,##0.00");//เงินประกัน
            var Advance = details.Advanced.ToString("#,##0.00");//เงินล่วงหน้า
            var Etc = details.Etc;
            var Thaibath = details.Thaibath;
            var DepositText = details.DepositText;
            var AdvancedText = details.AdvancedText;
            var TypeRooms = details.TypeRooms;
            var Statuss = details.Statuss;

            response = Ok(new
            {
                EmpName = EmpName,
                EmpID = EmpID,
                EmpPosition = EmpPosition,
                Site = Site,
                HouseName = HouseName,
                RoomNumber = RoomNumber,
                PostingDate = PostingDate,
                Price = Price,
                Etc = Etc,
                Thaibath = Thaibath,
                TypeRooms,
                Statuss = Statuss,
                Deposit = Deposit,
                DepositText = DepositText,
                Advance = Advance,
                AdvancedText = AdvancedText

            });
            return response;
        }
        public IActionResult SendRental(string Job, string Month, string mailform)
        {
            IActionResult response = Unauthorized();
            var lastday = DateTime.DaysInMonth(Int32.Parse(Month.Substring(0, 4)), Int32.Parse(Month.Substring(5, 2)));
            var date2 = Convert.ToDateTime(Month.Substring(0, 4) + "-" + Month.Substring(5, 2) + "-" + lastday + " 23:59:59");
            var date1 = Convert.ToDateTime(Month.Substring(0, 4) + "-" + Month.Substring(5, 2) + "-01  00:00:00");

            System.Globalization.CultureInfo _cultureTHInfo = new System.Globalization.CultureInfo("th-TH");
            DateTime dateThai = Convert.ToDateTime(date1, _cultureTHInfo);
            var MonthThai = dateThai.ToString("MMMM", _cultureTHInfo);
            var Period = MonthThai + " " + Month.Substring(0, 4);

            var file = new List<HouseRentalFileMonth>();
            var HouseRentals = new List<HouseRental>();


            var checksucess = 0;

            if (Job == "All")
            {
                checksucess = 1;

            }
            else
            {
                HouseRentals = _context.HouseRentals.Where(p => p.Site == Job && p.PostingDate >= date1 && p.PostingDate <= date2 && p.Statuss == HouseRental.Status.รอ).ToList();
                file = _context.HouseRentalFileMonths.Where(c => c.Job == Job && c.FileMonthDate >= date1 && c.FileMonthDate <= date2).ToList();

                var count = file.Count();


                var headtable = "";
                var main = "";
                var header = "<h4>ตารางรายการขออนุมัติเบิกค่าเช่าห้องพักของหน่วยงาน " + Job + " ประจำงวด " + Period + "</h4><br/>";

                headtable = "<table style='border: 1px solid #ddd;border-collapse: collapse;' ><thead><tr>"
                    //+ "<th align ='center' style='border: 1px solid #ddd;border-collapse: collapse;padding: 8px 16px;'>สถานะ</th>"
                    + "<th align ='center' style='border: 1px solid #ddd;border-collapse: collapse;padding: 8px 16px;'>วันที่จ่าย</th>"
                    + "<th align ='center' style='border: 1px solid #ddd;border-collapse: collapse;padding: 8px 16px;'>รหัสพนักงาน</th>"
                    + "<th align ='center' style='border: 1px solid #ddd;border-collapse: collapse;padding: 8px 16px;'>ชื่อ-สกุล</th>"
                    + "<th align ='center' style='border: 1px solid #ddd;border-collapse: collapse;padding: 8px 16px;'>ตำแหน่ง</th>"
                    + "<th align ='center' style='border: 1px solid #ddd;border-collapse: collapse;padding: 8px 16px;'>หน่วยงาน</th>"
                    + "<th align ='center' style='border: 1px solid #ddd;border-collapse: collapse;padding: 8px 16px;'>การพัก</th>"
                    + "<th align ='center' style='border: 1px solid #ddd;border-collapse: collapse;padding: 8px 16px;'>ชื่อหอพัก</th>"
                    + "<th align ='center' style='border: 1px solid #ddd;border-collapse: collapse;padding: 8px 16px;'>ห้อง</th>"
                    + "<th align ='center' style='border: 1px solid #ddd;border-collapse: collapse;padding: 8px 16px;'>เงินประกัน</th>"
                    + "<th align ='center' style='border: 1px solid #ddd;border-collapse: collapse;padding: 8px 16px;'>ล่วงหน้า</th>"
                    + "<th align ='center' style='border: 1px solid #ddd;border-collapse: collapse;padding: 8px 16px;'>ค่าที่พัก</th>"


                + "</tr>"
                + "</thead>"
               + "<tbody id='myTable'>";

                foreach (var std in HouseRentals as IList<HouseRental>)
                {
                    var ID = std.ID;

                    main += "<tr>"
                             //+ "<td style='border: 1px solid #ddd;border-collapse: collapse;padding: 8px 16px;'>" + std.Statuss + "</td>"
                             + "<td style='border: 1px solid #ddd;border-collapse: collapse;padding: 8px 16px;'>" + std.Paymentdate + "</td>"
                             + "<td style='border: 1px solid #ddd;border-collapse: collapse;padding: 8px 16px;'>" + std.EmpId + "</td>"
                             + "<td style='border: 1px solid #ddd;border-collapse: collapse;padding: 8px 16px;'>" + std.EmpName + "</td>"
                             + "<td style='border: 1px solid #ddd;border-collapse: collapse;padding: 8px 16px;'>" + std.EmpPosition + "</td>"
                             + "<td style='border: 1px solid #ddd;border-collapse: collapse;padding: 8px 16px;'>" + std.Site + "</td>"
                             + "<td style='border: 1px solid #ddd;border-collapse: collapse;padding: 8px 16px;'>" + std.TypeRooms + "</td>"
                             + "<td style='border: 1px solid #ddd;border-collapse: collapse;padding: 8px 16px;'>" + std.HouseName + "</td >"
                             + "<td style='border: 1px solid #ddd;border-collapse: collapse;padding: 8px 16px;'>" + std.RoomNumber + "</td>"
                             + "<td align ='right' style='border: 1px solid #ddd;border-collapse: collapse;padding: 8px 16px;'>" + std.Deposit.ToString("#,##0.00") + "</td>"
                             + "<td align ='right' style='border: 1px solid #ddd;border-collapse: collapse;padding: 8px 16px;'>" + std.Advanced.ToString("#,##0.00") + "</td>"
                             + "<td align ='right' style='border: 1px solid #ddd;border-collapse: collapse;padding: 8px 16px;'>" + std.Price.ToString("#,##0.00") + "</td>"
                             + "</tr>";


                }


                var table = headtable + main + "</tbody></table> </br>" /*+ bodyfile*/;
                var datamail = header + table;


                if (count > 0)
                {
                    var mailto = _config["Email:To"];///ผู้รับ
                    var form = mailform + "@ces.co.th";
                    //var send = /*"thaneing@gmail.com;"*/mailto+";"+form;
                    //var to = "thanyalak@ces.co.th";
                    var to = form;//ถึงผู้ส่ง


                    //foreach (var address in to.Split(new[] { ";" }, StringSplitOptions.RemoveEmptyEntries))
                    //{
                    //    to = (address);
                    //}

                    var title = "รายการขออนุมัติเบิกค่าเช่าห้องพักของหน่วยงาน " + Job + "ประจำงวด " + Period;
                    var body = datamail;

                    var test = SentMail.MailSent(_config["Email:Server"], Int32.Parse(_config["Email:Port"]), bool.Parse(_config["Email:Security"]), "", "", form, mailto+";"+to, title, body,"");

                    if (test == true)
                    {
                        checksucess = 0;
                    }
                    else
                    {
                        checksucess = 1;
                    }
                }
                else
                {
                    checksucess = 1;

                }


                //return RedirectToAction("Index", "Home");
                response = Ok(new
                {
                    check = checksucess
                });
            }
            return response;

        }

        public IActionResult Detailmail(string Job, string Month)
        {

            IActionResult response = Unauthorized();

            var name = _config["Email:NameTo"].ToString();
            var email = _config["Email:To"];

            var lastday = DateTime.DaysInMonth(Int32.Parse(Month.Substring(0, 4)), Int32.Parse(Month.Substring(5, 2)));
            var date2 = Convert.ToDateTime(Month.Substring(0, 4) + "-" + Month.Substring(5, 2) + "-" + lastday + " 23:59:59");
            var date1 = Convert.ToDateTime(Month.Substring(0, 4) + "-" + Month.Substring(5, 2) + "-01  00:00:00");

            System.Globalization.CultureInfo _cultureTHInfo = new System.Globalization.CultureInfo("th-TH");
            DateTime dateThai = Convert.ToDateTime(date1, _cultureTHInfo);
            var MonthThai = dateThai.ToString("MMMM", _cultureTHInfo);
            var Period = MonthThai + " " + Month.Substring(0, 4);

            var file = new List<HouseRentalFileMonth>();
            var HouseRentals = new List<HouseRental>();

            if (Job == "All")
            {


            }
            else
            {
                HouseRentals = _context.HouseRentals.Where(p => p.Site == Job && p.PostingDate >= date1 && p.PostingDate <= date2 && p.Statuss == HouseRental.Status.รอ).ToList();

                var countfile = 0;
                countfile = _context.HouseRentalFileMonths.Where(f => f.Period == Period && f.Job == Job).Count();

                var count = HouseRentals.Count();

                var headtable = "";
                var main = "";
                var header = "<h4>ตารางรายการขออนุมัติเบิกค่าเช่าห้องพักของหน่วยงาน " + Job + " ประจำงวด " + Period + "</h4><br/>";

                headtable = "<table style='border: 1px solid #ddd;border-collapse: collapse;'  ><thead><tr>"
                      //+ "<th align ='center' style='border: 1px solid #ddd;border-collapse: collapse;padding: 8px 16px;'>สถานะ</th>"
                      + "<th align ='center' style='border: 1px solid #ddd;border-collapse: collapse;padding: 8px 16px;'>วันที่จ่าย</th>"
                    + "<th align ='center' style='border: 1px solid #ddd;border-collapse: collapse;padding: 8px 16px;'>รหัสพนักงาน</th>"
                    + "<th align ='center' style='border: 1px solid #ddd;border-collapse: collapse;padding: 8px 16px;'>ชื่อ-สกุล</th>"
                    + "<th align ='center' style='border: 1px solid #ddd;border-collapse: collapse;padding: 8px 16px;'>ตำแหน่ง</th>"
                    + "<th align ='center' style='border: 1px solid #ddd;border-collapse: collapse;padding: 8px 16px;'>หน่วยงาน</th>"
                    + "<th align ='center' style='border: 1px solid #ddd;border-collapse: collapse;padding: 8px 16px;'>การพัก</th>"
                    + "<th align ='center' style='border: 1px solid #ddd;border-collapse: collapse;padding: 8px 16px;'>ชื่อหอพัก</th>"
                    + "<th align ='center' style='border: 1px solid #ddd;border-collapse: collapse;padding: 8px 16px;'>ห้อง</th>"
                    + "<th align ='center' style='border: 1px solid #ddd;border-collapse: collapse;padding: 8px 16px;'>เงินประกัน</th>"
                    + "<th align ='center' style='border: 1px solid #ddd;border-collapse: collapse;padding: 8px 16px;'>ล่วงหน้า</th>"
                    + "<th align ='center' style='border: 1px solid #ddd;border-collapse: collapse;padding: 8px 16px;'>ค่าที่พัก</th>"


                + "</tr>"
                + "</thead>"
               + "<tbody id='myTable'>";

                foreach (var std in HouseRentals as IList<HouseRental>)
                {
                    var ID = std.ID;

                   


                    main += "<tr>"
                             //+ "<td style='border: 1px solid #ddd;border-collapse: collapse;padding: 8px 16px;'>" + std.Statuss + "</td>"
                             + "<td style='border: 1px solid #ddd;border-collapse: collapse;padding: 8px 16px;'>" + std.Paymentdate + "</td>"
                             + "<td style='border: 1px solid #ddd;border-collapse: collapse;padding: 8px 16px;'>" + std.EmpId + "</td>"
                             + "<td style='border: 1px solid #ddd;border-collapse: collapse;padding: 8px 16px;'>" + std.EmpName + "</td>"
                             + "<td style='border: 1px solid #ddd;border-collapse: collapse;padding: 8px 16px;'>" + std.EmpPosition + "</td>"
                             + "<td style='border: 1px solid #ddd;border-collapse: collapse;padding: 8px 16px;'>" + std.Site + "</td>"
                             + "<td style='border: 1px solid #ddd;border-collapse: collapse;padding: 8px 16px;'>" + std.TypeRooms + "</td>"
                             + "<td style='border: 1px solid #ddd;border-collapse: collapse;padding: 8px 16px;'>" + std.HouseName + "</td >"
                             + "<td style='border: 1px solid #ddd;border-collapse: collapse;padding: 8px 16px;'>" + std.RoomNumber + "</td>"
                             + "<td align ='right' style='border: 1px solid #ddd;border-collapse: collapse;padding: 8px 16px;'>" + std.Deposit.ToString("#,##0.00") + "</td>"
                             + "<td align ='right' style='border: 1px solid #ddd;border-collapse: collapse;padding: 8px 16px;'>" + std.Advanced.ToString("#,##0.00") + "</td>"
                             + "<td align ='right' style='border: 1px solid #ddd;border-collapse: collapse;padding: 8px 16px;'>" + std.Price.ToString("#,##0.00") + "</td>"
                             + "</tr>";


                }

          

                var table = headtable + main + "</tbody></table> </br>" /*+ bodyfile*/;
                var datamail = header + table;


                response = Ok(new { name = name, email = email, lists = datamail, countfile = countfile, count=count });

            }

            return response;
        }
        public IActionResult HouseRentalReport()
        {
            /*Check Session */
            var page = "249";
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

            List<UserJob> instances = new List<UserJob>();
            UserJob current = null;
            current = new UserJob();
            current.UserId = 0;
            current.UserJobId = 0;
            current.UserJobDetail = "All";
            instances.Add(current);

            var jobdata = _context.UserJobs.Where(s => s.UserId == HttpContext.Session.GetInt32("Userid")).OrderBy(s => s.UserJobDetail).ToList();
            foreach (var std in jobdata as IList<UserJob>)
            {
                current = new UserJob();
                current = std;
                instances.Add(current);
            }
            var job = new SelectList(instances, "UserJobDetail", "UserJobDetail");

            ViewData["JobNo"] = job;

            var job2 = new SelectList(_context.UserJobs.Where(s => s.UserId == HttpContext.Session.GetInt32("Userid")).OrderBy(s => s.UserJobDetail).ToList(), "UserJobDetail", "UserJobDetail");
            ViewData["JobNo2"] = job2;

            return View();
        }

        public IActionResult Checkdata(string Job, string Month)
        {

            IActionResult response = Unauthorized();
            var lastday = DateTime.DaysInMonth(Int32.Parse(Month.Substring(0, 4)), Int32.Parse(Month.Substring(5, 2)));
            var date2 = Convert.ToDateTime(Month.Substring(0, 4) + "-" + Month.Substring(5, 2) + "-" + lastday + " 23:59:59");
            var date1 = Convert.ToDateTime(Month.Substring(0, 4) + "-" + Month.Substring(5, 2) + "-01  00:00:00");



            var HouseRentals = new List<HouseRental>();

            if (Job == "All")
            {
                HouseRentals = _context.HouseRentals.Where(p => p.PostingDate >= date1 && p.PostingDate <= date2 && p.Statuss == HouseRental.Status.สำเร็จ).ToList();
            }
            else
            {
                HouseRentals = _context.HouseRentals.Where(p => p.Site == Job && p.PostingDate >= date1 && p.PostingDate <= date2 && p.Statuss == HouseRental.Status.สำเร็จ).ToList();
            }

            var count = HouseRentals.Count();
            var check = 0;
            if (count > 0)
            {
                check = 0;
            }
            else
            {
                check = 1;
            }



            //XtraReport report = XtraReport.FromFile("reports\\XtraReport.repx");
            //report.DataSource = HouseRentals;

            response = Ok(new { check = check });

            return response;
            //return View();



        }


        public IActionResult GetReportDiff(string Job)
        {
            /*Check Session */
            var page = "249";
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
            var HouseRentals = new List<HouseRental>();
            List<Tmp_House> instances = new List<Tmp_House>();
            Tmp_House tmp = null;

            HouseRentals = _context.HouseRentals.ToList();


            foreach (var std in HouseRentals as IList<HouseRental>)
            {
                var empID = std.EmpId;

                tmp = new Tmp_House();
                tmp.ID = std.ID;
                tmp.EmpId = std.EmpId;
                tmp.EmpName = std.EmpName;
                tmp.EmpPosition = std.EmpPosition;
                tmp.PostingDate = std.PostingDate;
                tmp.Site = std.Site;
                tmp.Deposit = std.Deposit;
                tmp.DepositText = std.DepositText;
                tmp.Advanced = std.Advanced;
                tmp.AdvancedText = std.AdvancedText;
                tmp.Price = std.Price;
                tmp.Thaibath = std.Thaibath;
                tmp.Etc = std.Etc;
                tmp.HouseName = std.HouseName;
                tmp.RoomNumber = std.RoomNumber;
                tmp.CreateBy = std.CreateBy;
                tmp.CreateDate = std.CreateDate;
                tmp.UpdateBy = std.UpdateBy;
                tmp.UpdateDate = std.UpdateDate;
                tmp.Statuss = (int)std.Statuss;
                tmp.TypeRooms = (int)std.TypeRooms;
                tmp.Period = std.Period;
                tmp.Paymentdate = std.Paymentdate;
                instances.Add(tmp);
            }
            //response = Ok(new { /*data = HouseRentals*/data =  });
          
           
            if (Job == "All")
            {
                if (typeofuser == "3")
                {
                    var sells = instances
                       .GroupBy(p => new { p.EmpId, p.EmpName, p.Statuss, p.Site, p.HouseName, p.RoomNumber })
                       .Select(p => new { total = p.Sum(b => b.Price + b.Advanced + b.Deposit), EmpId = p.Key.EmpId, EmpName = p.Key.EmpName, Status = p.Key.Statuss, Site = p.Key.Site, HouseName = p.Key.HouseName, RoomNumber = p.Key.RoomNumber, Advanced = p.Sum(b => b.Advanced), Deposit = p.Sum(b => b.Deposit) })
                       .Where(p => (p.Deposit != 0 || p.Advanced != 0) && p.Status == 1)
                       .OrderByDescending(p => p.EmpId)
                       .ToList();


                    response = Ok(new { data = sells });
                }
                else
                {
                    var jobdata = _context.UserJobs.Where(s => s.UserId == HttpContext.Session.GetInt32("Userid")).OrderBy(s => s.UserJobDetail).ToList();

                    List<string> site = new List<string>();
                    foreach (var a in jobdata as IList<UserJob>)
                    {
                        site.Add(a.UserJobDetail);
                        var sells = instances
                      .GroupBy(p => new { p.EmpId, p.EmpName, p.Statuss, p.Site, p.HouseName, p.RoomNumber })
                      .Select(p => new { total = p.Sum(b => b.Price + b.Advanced + b.Deposit), EmpId = p.Key.EmpId, EmpName = p.Key.EmpName, Status = p.Key.Statuss, Site = p.Key.Site, HouseName = p.Key.HouseName, RoomNumber = p.Key.RoomNumber, Advanced = p.Sum(b => b.Advanced), Deposit = p.Sum(b => b.Deposit) })
                      .Where(p => (p.Deposit != 0 || p.Advanced != 0) && p.Status == 1 && site.Contains(p.Site))
                      .OrderByDescending(p => p.EmpId)
                      .ToList();


                        response = Ok(new { data = sells });
                    }
                   
;


                }

             
            }
            else
            {
                var sells = instances
                        .GroupBy(p => new { p.EmpId, p.EmpName, p.Statuss, p.Site, p.HouseName ,p.RoomNumber})
                        .Select(p => new { total = p.Sum(b => b.Price + b.Advanced + b.Deposit), EmpId = p.Key.EmpId, EmpName = p.Key.EmpName, Status = p.Key.Statuss, Site = p.Key.Site, HouseName = p.Key.HouseName,RoomNumber = p.Key.RoomNumber, Advanced = p.Sum(b => b.Advanced), Deposit = p.Sum(b => b.Deposit) })
                        .Where(p => (p.Deposit != 0 || p.Advanced != 0) && p.Status == 1 && p.Site==Job)
                        .OrderByDescending(p => p.EmpId)
                        .ToList();
                response = Ok(new { data = sells });
            }

          
            return response;
        }

        public IActionResult RentalsEmp(string Job, string Month)//print รายงานของเลขา
        {
            var lastday = DateTime.DaysInMonth(Int32.Parse(Month.Substring(0, 4)), Int32.Parse(Month.Substring(5, 2)));
            var date2 = Convert.ToDateTime(Month.Substring(0, 4) + "-" + Month.Substring(5, 2) + "-" + lastday + " 23:59:59");
            var date1 = Convert.ToDateTime(Month.Substring(0, 4) + "-" + Month.Substring(5, 2) + "-01  00:00:00");

            var HouseRentals = new List<HouseRental>();

            if (Job == "All")
            {
                //Alert("กรุณาเลือกไซต์ก่อน", NotificationType.error);
                //return RedirectToAction("Index", "Home");
            }
            else
            {
                HouseRentals = _context.HouseRentals.Where(p => p.Site == Job && p.PostingDate >= date1 && p.PostingDate <= date2 && p.Statuss == HouseRental.Status.รอ).ToList();
            }
            //HouseRentals = _context.HouseRentals.ToList();

            XtraReport report = XtraReport.FromFile("reports\\ApplyReport.repx");
            report.DataSource = HouseRentals;


            report.CreateDocument(true);
            var @out = new MemoryStream();
            report.ExportToPdf(@out);
            @out.Position = 0;



            return new FileStreamResult(@out, "application/pdf");

            //return View();


        }
        public IActionResult CheckRentalsEmp(string Job, string Month)//print รายงานของเลขา count
        {
            IActionResult response = Unauthorized();
            var lastday = DateTime.DaysInMonth(Int32.Parse(Month.Substring(0, 4)), Int32.Parse(Month.Substring(5, 2)));
            var date2 = Convert.ToDateTime(Month.Substring(0, 4) + "-" + Month.Substring(5, 2) + "-" + lastday + " 23:59:59");
            var date1 = Convert.ToDateTime(Month.Substring(0, 4) + "-" + Month.Substring(5, 2) + "-01  00:00:00");

            var HouseRentals = new List<HouseRental>();

            if (Job == "All")
            {
                //Alert("กรุณาเลือกไซต์ก่อน", NotificationType.error);
                //return RedirectToAction("Index", "Home");
            }
            else
            {
                HouseRentals = _context.HouseRentals.Where(p =>  p.Site == Job && p.PostingDate >= date1 && p.PostingDate <= date2 && p.Statuss == HouseRental.Status.รอ).ToList();
            }
            //HouseRentals = _context.HouseRentals.ToList();


            var count = HouseRentals.Count();

            response = Ok(new { count= count });

            return response;


        }
        public IActionResult CreateNextPeriod(string id,string post)
        {
            
            IActionResult response = Unauthorized();
            //var post = DateTime.Now;
         
            var date1 = Convert.ToDateTime(post.Substring(6, 4) + " " + post.Substring(3, 2) + " " + post.Substring(0, 2) + " 00:00:00");
            var lastday = DateTime.DaysInMonth(Int32.Parse(post.Substring(6, 4)), Int32.Parse(post.Substring(3, 2)));


            string[] thmonth = new string[] { "มกราคม", "กุมภาพันธ์", "มีนาคม", "เมษายน", "พฤษภาคม", "มิถุนายน", "กรกฎาคม", "สิงหาคม", "กันยายน", "ตุลาคม", "พฤศจิกายน", "ธันวาคม" };
            var res = post.Substring(3, 2);
            var Thailmonth = thmonth[Int32.Parse(res) - 1];

            string [] codelist = id.Split(",");
            string strId = "";
            int code = 0;
            var list = "";
            int checkS = 0;
   
           List<HouseRental> houseRentals = new List<HouseRental>();
            List<HouseRental> houseRental1s = new List<HouseRental>();

            for (var i=0;i<codelist.Length;i++)
            {
                strId = codelist[i];
                code = Int32.Parse(strId);
        
                //Int32.TryParse(strId);
                houseRentals = new List<HouseRental>();
                houseRental1s = new List<HouseRental>();
                HouseRental houseRental1;
                houseRentals  = _context.HouseRentals.Where(p=>p.ID== code).ToList();
            
                foreach (var houseRental in houseRentals)
                {
                   list =""+ houseRental.ID +" ," + houseRental.EmpName;
                    //foreach (var houseRental1 in houseRental1s)
                    //{
                    
                    if (houseRental.Statuss == HouseRental.Status.รอ || houseRental.Statuss == HouseRental.Status.ไม่สำเร็จ)
                    {
                        checkS= 1;
                    }
                    else
                    {
                        checkS = 0;
                        houseRental1 = new HouseRental();
                        houseRental1.EmpId = houseRental.EmpId;
                        houseRental1.EmpName = houseRental.EmpName;
                        houseRental1.EmpPosition = houseRental.EmpPosition;
                        houseRental1.RoomPrice = houseRental.RoomPrice;
                        houseRental1.RoomNumber = houseRental.RoomNumber;
                        houseRental1.TypeRooms = houseRental.TypeRooms;
                        houseRental1.Site = houseRental.Site;
                        houseRental1.Price = houseRental.Price;
                        houseRental1.Etc = houseRental.Etc;
                        houseRental1.HouseName = houseRental.HouseName;
                        houseRental1.RoomNumber = houseRental.RoomNumber;
                        houseRental1.Thaibath = houseRental.Thaibath;
                        houseRental1.Statuss = HouseRental.Status.รอ;
                        houseRental1.Deposit = 0;
                        houseRental1.Advanced = 0;
                        houseRental1.DepositText = houseRental.DepositText;
                        houseRental1.AdvancedText = houseRental.AdvancedText;
                        houseRental1.Period = Thailmonth + " " + post.Substring(6,4);
                        houseRental1.Paymentdate = lastday.ToString("00") + " " + Thailmonth + " " + post.Substring(6, 4);
                        houseRental1.PostingDate = date1;
                        houseRental1.CreateDate = DateTime.Now;
                        houseRental1.CreateBy = HttpContext.Session.GetString("Username");

                        houseRental1s.Add(houseRental1);

                    }
                }

                _context.HouseRentals.AddRange(houseRental1s);
                _context.SaveChanges();
            }

            response = Ok(new { strId = strId,list=list,code = code,checkS=checkS});

            return response;


        }

    }
}
