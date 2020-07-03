using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using CESAPSCOREWEBAPP.Models;
using System.Globalization;
using Microsoft.AspNetCore.Http;
using System.Data.SqlClient;
using static CESAPSCOREWEBAPP.Models.Enums;
using CESAPSCOREWEBAPP.Helpers;

namespace CESAPSCOREWEBAPP.Controllers
{
    public class HRRecruitesController : BaseController
    {
        private readonly DatabaseContext _context;

        public HRRecruitesController(DatabaseContext context)
        {
            _context = context;
        }

        // GET: HRRecruites
        public async Task<IActionResult> Index()
        {
            var databaseContext = _context.HRRecruites
                .Include(h => h.Bloods)
                .Include(h => h.Faculties)
                .Include(h => h.HRRecruiteStatuses)
                .Include(h => h.Majors)
                .Include(h => h.TypeCongrates)
                .Include(h => h.TypeOfResigns)
                .Include(h => h.TypeOfSalaries)
                .Include(h => h.TitleOfUsers)
                .Include(h =>h.Organizs.Positions)
                .Include(h =>h.Organizs.Departments)
                .Include(h => h.Organizs.Department1s)
                .Include(h =>h.Levels)
                .Include(h=>h.Universities)
                .Include(h=>h.HRRecruiteGroups);


            return View(await databaseContext.ToListAsync());
        }

        // GET: HRRecruites/Details/5
        public IActionResult Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }


            var hRRecruite1 = _context.HRRecruites
                .Include(h => h.Bloods)
                .Include(h => h.Faculties)
                .Include(h => h.HRRecruiteStatuses)
                .Include(h => h.Majors)
                .Include(h => h.TypeCongrates)
                .Include(h => h.TypeOfResigns)
                .Include(h => h.TypeOfSalaries)
                .Include(h => h.TitleOfUsers)
                .Include(h => h.Organizs.Positions)
                .Include(h => h.Organizs.Departments)
                .Include(h => h.Organizs.Department1s)
                .Include(h => h.Levels)
                .Include(h => h.Universities)
                .Include(h => h.HRRecruiteGroups)
                .Where(h => h.HRRecruiteID == id)
                .ToList();

            ViewData["hRRecruite"] = hRRecruite1;

            ViewData["HRRecruiteStatusId"] = new SelectList(_context.HRRecruiteStatuses, "HRRecruiteStatusId", "HRRecruiteStatusName");



            ViewData["AppEtcId"] = new SelectList(_context.appEtcs, "AppEtcId", "AppEtcName");
            ViewData["AppResultId"] = new SelectList(_context.appResults, "AppResultId", "AppResultName");
            ViewData["AppStatusId"] = new SelectList(_context.appStatuses, "AppStatusId", "AppStatusName");
            ViewData["AppSuccessId"] = new SelectList(_context.appSuccesses, "AppSuccessId", "AppSuccessName");
            ViewData["AppTelTypeId"] = new SelectList(_context.appTelTypes, "AppTelTypeId", "AppTelTypeName");
            ViewData["AppRoomId"] = new SelectList(_context.appRooms, "AppRoomId", "AppRoomName");
            ViewData["AppStatusEdit"] = new SelectList(_context.appStatuses, "AppStatusId", "AppStatusName");

            ViewBag.hrid = id;


            var databaseContext = _context.appointments
                .Include(a => a.appEtcs)
                .Include(a => a.appResults)
                .Include(a => a.appStatuses)
                .Include(a => a.appSuccesses)
                .Include(a => a.appTelTypes)
                .Where(a => a.HRRecruiteID == id)
                .ToList();

            ViewData["applist"] = databaseContext;
            return View();



   
        }

        // GET: HRRecruites/Create
        public IActionResult Create()
        {
            ViewData["BloodId"] = new SelectList(_context.Bloods, "BloodId", "BloodName");
            ViewData["FacultyId"] = new SelectList(_context.Faculties, "FacultyId", "FacultyName");
            ViewData["HRRecruiteStatusId"] = new SelectList(_context.HRRecruiteStatuses, "HRRecruiteStatusId", "HRRecruiteStatusName");
            ViewData["MajorId"] = new SelectList(_context.Majors, "MajorId", "MajorName");
            ViewData["TypeCongrateId"] = new SelectList(_context.TypeCongrates, "TypeCongrateId", "TypeCongrateName");
            ViewData["TypeOfResignId"] = new SelectList(_context.TypeOfResigns, "TypeOfResignId", "TypeOfResignName");
            ViewData["TypeOfSalaryId"] = new SelectList(_context.TypeOfSalaries, "TypeOfSalaryId", "TypeOfSalaryName");
            ViewData["UniversityId"] = new SelectList(_context.Universities, "UniversityId", "UniversiryName");
            ViewData["TitleId"] = new SelectList(_context.TitleOfUsers, "TitleOfUserId", "TitleOfUserName");
            ViewData["LevelId"] = new SelectList(_context.Levels, "LevelId", "LevelName");
            ViewData["HRRecruiteGroupId"]= new SelectList(_context.HRRecruiteGroups, "HRRecruiteGroupId", "HRRecruiteGroupDetail");

            var organiz = _context.Organizs
                                  .Include(p => p.Departments)
                                  .Include(p => p.Department1s)
                                  .Include(p => p.Positions)
                                  .ToList();

            ViewData["organiz"] = organiz;


            return View();
        }


        // POST: HRRecruites/SaveData
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> SaveData([Bind("AppId,HRRecruiteID,AppTelTypeId,AppTelDate,AppStatusId,AppDate,AppResultId,AppEtcId,AppSuccessId,AppRoomId,AppCreateBy,AppCreateDate,AppUpdateBy,AppUpdateDate")] Appointment appointment,string app)
        {
            ViewData["HRRecruiteStatusId"] = new SelectList(_context.HRRecruiteStatuses, "HRRecruiteStatusId", "HRRecruiteStatusName");




            var hRRecruite1 = _context.HRRecruites
                            .Include(h => h.Bloods)
                            .Include(h => h.Faculties)
                            .Include(h => h.HRRecruiteStatuses)
                            .Include(h => h.Majors)
                            .Include(h => h.TypeCongrates)
                            .Include(h => h.TypeOfResigns)
                            .Include(h => h.TypeOfSalaries)
                            .Include(h => h.TitleOfUsers)
                            //.Include(h => h.Departments)
                            //.Include(h => h.Department1s)
                            //.Include(h => h.Positions)
                            .Include(h => h.Levels)
                            .Include(h => h.Universities)
                            .Include(h => h.HRRecruiteGroups)
                            //.Where(h => h.HRRecruiteID == appointment.HRRecruiteID)
                            .First(h => h.HRRecruiteID == appointment.HRRecruiteID);

            ViewData["hRRecruite"] = hRRecruite1;

            var laststatus = 0;
            DateTime? SignDate = null;
            DateTime? Startwork = null;

            laststatus = hRRecruite1.HRRecruiteStatusId;
            SignDate = hRRecruite1.SignDate;
            Startwork = hRRecruite1.StartWork;


            var appdata = 0;
            var i = 0;
            var j = 0;

            if (appointment.AppStatusId == 1) //สถานะการโทร เป็น -
            {
                appdata = 1;  //สถานะไม่ระบุ
            }
            else
            {
                appdata = 2;  //สถานะการโทร โทรรับหรือไม่รับ
            }


            if (appointment.AppResultId == 1) //สถานะการนัด เป็น -
            {

            }
            else
            {
                appdata = 2; //สถานะการนัด เป็น 2-3 สถานะรับนัด ไม่รับนัด จะเป็น สถานะ นัดทันที
            }


            if (appointment.AppSuccessId == 1) //สถานะผลการนัด เป็น -
            {

            }
            else if (appointment.AppSuccessId == 2) //สถานะผลการนัด ไม่ผ่าน
            {
                appdata = 3; //สถานะการนัด เป็นไม่ผ่านสัมภาษณ์
            }
            else if (appointment.AppSuccessId == 3) //สถานะผลการนัด ผ่านการสัมภาณ์
            {
                appdata = 4; //สถานะผ่านการสัมภาษณ์
            }
            else if (appointment.AppSuccessId == 4) //สถานะผลการนัด ไม่มาสัมภาษณ์
            {
                appdata = 8; //สถานะ เป็นไม่มาสัมภาษณ์
            }
            else if (appointment.AppSuccessId == 5) //สถานะรอมาสัมภาณ์
            {
                appdata = 2; //สถานะ เป็นนัด
            }

            if (SignDate != null)  //มีวันที่เซ็นสัญญา
            {
                appdata = 6;
            }
            if (Startwork != null)  //มีวันที่เริ่มทำงาน
            {
                appdata = 6;
            }





           laststatus = appdata;
         




            hRRecruite1.HRRecruiteStatusId = laststatus;

            using (var dbContextTransaction = _context.Database.BeginTransaction())
            {
                try
                {
                    _context.HRRecruites.Update(hRRecruite1);
                    _context.SaveChanges();



                    dbContextTransaction.Commit();

                    //Alert(i+" "+j, NotificationType.error);
                }
                catch (Exception)
                {
                    dbContextTransaction.Rollback();
                }
            }





            ViewData["AppEtcId"] = new SelectList(_context.appEtcs, "AppEtcId", "AppEtcName");
            ViewData["AppResultId"] = new SelectList(_context.appResults, "AppResultId", "AppResultName");
            ViewData["AppStatusId"] = new SelectList(_context.appStatuses, "AppStatusId", "AppStatusName");
            ViewData["AppSuccessId"] = new SelectList(_context.appSuccesses, "AppSuccessId", "AppSuccessName");
            ViewData["AppTelTypeId"] = new SelectList(_context.appTelTypes, "AppTelTypeId", "AppTelTypeName");
            ViewData["AppRoomId"] = new SelectList(_context.appRooms, "AppRoomId", "AppRoomName");

            ViewBag.hrid = appointment.HRRecruiteID;


            var databaseContext = _context.appointments
                .Include(a => a.appEtcs)
                .Include(a => a.appResults)
                .Include(a => a.appStatuses)
                .Include(a => a.appSuccesses)
                .Include(a => a.appTelTypes)
                .Where(a => a.HRRecruiteID == appointment.HRRecruiteID)
                .ToList();

            ViewData["applist"] = databaseContext;

            if (ModelState.IsValid)
            {
                
                appointment.AppCreateDate = DateTime.Now;
                appointment.AppTelDate = DateTime.Now;
                appointment.AppCreateBy = HttpContext.Session.GetString("Username");
                try
                {
                    appointment.AppDate = DateTime.Parse(app.Substring(6, 4) + "-" + app.Substring(3, 2) + "-" + app.Substring(0, 2) + " " + app.Substring(11, 5) + ":00");
                }
                catch
                {
                    appointment.AppDate = null;
                }
                
               


                _context.Add(appointment);
                await _context.SaveChangesAsync();
                return RedirectToAction("Details", "HRRecruites", new { id = appointment.HRRecruiteID });
            }
            ViewData["AppEtcId"] = new SelectList(_context.appEtcs, "AppEtcId", "AppEtcName", appointment.AppEtcId);
            ViewData["AppResultId"] = new SelectList(_context.appResults, "AppResultId", "AppResultName", appointment.AppResultId);
            ViewData["AppStatusId"] = new SelectList(_context.appStatuses, "AppStatusId", "AppStatusName", appointment.AppStatusId);
            ViewData["AppSuccessId"] = new SelectList(_context.appSuccesses, "AppSuccessId", "AppSuccessName", appointment.AppSuccessId);
            ViewData["AppTelTypeId"] = new SelectList(_context.appTelTypes, "AppTelTypeId", "AppTelTypeName", appointment.AppTelTypeId);
            ViewData["AppRoomId"] = new SelectList(_context.appRooms, "AppRoomId", "AppRoomName",appointment.AppRoomId);

            return RedirectToAction("Details", "HRRecruites", new { id = appointment.HRRecruiteID });
        }



        //[HttpPost]
        //[ValidateAntiForgeryToken]
        public IActionResult UpdateData(int appno,int apphrid,int appTelTypeId,int AppStatusId,int AppRoomId, int AppResultId,string app,int AppSuccessId,int AppEtcId)
        {
            ViewData["HRRecruiteStatusId"] = new SelectList(_context.HRRecruiteStatuses, "HRRecruiteStatusId", "HRRecruiteStatusName");

            var hRRecruite1 = _context.HRRecruites.First(h => h.HRRecruiteID == apphrid);
            if (hRRecruite1 == null) {
                return NotFound();
            }
            var laststatus =0;
            DateTime? SignDate = null; 
            DateTime? Startwork = null;
     
            laststatus = hRRecruite1.HRRecruiteStatusId;
            SignDate = hRRecruite1.SignDate;
            Startwork = hRRecruite1.StartWork;

               
         

            ViewData["hRRecruite"] = hRRecruite1;
            ViewData["AppEtcId"] = new SelectList(_context.appEtcs, "AppEtcId", "AppEtcName");
            ViewData["AppResultId"] = new SelectList(_context.appResults, "AppResultId", "AppResultName");
            ViewData["AppStatusId"] = new SelectList(_context.appStatuses, "AppStatusId", "AppStatusName");
            ViewData["AppSuccessId"] = new SelectList(_context.appSuccesses, "AppSuccessId", "AppSuccessName");
            ViewData["AppTelTypeId"] = new SelectList(_context.appTelTypes, "AppTelTypeId", "AppTelTypeName");

            ViewBag.hrid = apphrid;
     

            //var std1 = _context.appointments.First(a=>a.AppId== appno);

           // Alert(hRRecruite1.HRRecruiteID+" "+hRRecruite1.HRRecruiteStatusId, NotificationType.error);

            var appdata = 0;
            var i = 0;
            var j = 0;
       
                if (AppStatusId == 1) //สถานะการโทร เป็น -
                {
                    appdata = 1;  //สถานะไม่ระบุ
                }
                else
                {
                    appdata = 2;  //สถานะการโทร โทรรับหรือไม่รับ
                }


                if (AppResultId == 1) //สถานะการนัด เป็น -
                {
                   
                }
                else
                {
                    appdata = 2; //สถานะการนัด เป็น 2-3 สถานะรับนัด ไม่รับนัด จะเป็น สถานะ นัดทันที
                }


                if (AppSuccessId == 1) //สถานะผลการนัด เป็น -
                {

                }
                else if (AppSuccessId == 2) //สถานะผลการนัด ไม่ผ่าน
                {
                    appdata = 3; //สถานะการนัด เป็นไม่ผ่านสัมภาษณ์
                }
                else if (AppSuccessId == 3) //สถานะผลการนัด ผ่านการสัมภาณ์
                {
                    appdata = 4; //สถานะผ่านการสัมภาษณ์
                }
                else if (AppSuccessId == 4) //สถานะผลการนัด ไม่มาสัมภาษณ์
                {
                    appdata = 8; //สถานะ เป็นไม่มาสัมภาษณ์
                }
                else if (AppSuccessId == 5) //สถานะรอมาสัมภาณ์
                {
                    appdata = 2; //สถานะ เป็นนัด
                }

                if (SignDate !=null)  //มีวันที่เซ็นสัญญา
                {
                    appdata = 6; 
                }
                if (Startwork != null)  //มีวันที่เริ่มทำงาน
                {
                    appdata = 6;
                }





                //Score
                if (appdata == 1)
                {
                    i = 10;
                }
                else if (appdata == 2)
                {
                    i =20;
                }
                else if (appdata == 3)
                {
                    i = 30;
                }
                else if (appdata == 4)
                {
                    i = 50;
                }
                else if (appdata == 5)
                {
                    i = 60;
                }
                else if (appdata == 6)
                {
                    i = 70;
                }
                else if (appdata == 7)
                {
                    i = 30;
                }
                else if (appdata == 8)
                {
                    i = 30;
                }




                //Score
                if (laststatus == 1)
                {
                    j = 10;
                }
                else if (laststatus == 2)
                {
                    j = 20;
                }
                else if (laststatus == 3)
                {
                    j = 30;
                }
                else if (laststatus == 4)
                {
                    j = 50;
                }
                else if (laststatus == 5)
                {
                    j = 60;
                }
                else if (laststatus == 6)
                {
                    j = 70;
                }
                else if (laststatus == 7)
                {
                    j = 30;
                }
                else if (laststatus == 8)
                {
                    j = 30;
                }

                if (i > j)
                {
                    laststatus = appdata;
                }
          

        

            hRRecruite1.HRRecruiteStatusId = laststatus;

            using (var dbContextTransaction = _context.Database.BeginTransaction())
            {
                try
                {
                   _context.HRRecruites.Update(hRRecruite1);
                   _context.SaveChanges();
                

              
                    dbContextTransaction.Commit();

                    //Alert(i+" "+j, NotificationType.error);
                }
                catch (Exception)
                {
                    dbContextTransaction.Rollback();
                }
            }


            //ViewData["applist"] = std1;
            //ViewBag.messagedata = laststatus + " " + appdata;




            if (ModelState.IsValid)
            {
                var appointment =_context.appointments.First(m => m.AppId == appno);

                appointment.AppTelTypeId = appTelTypeId;
                appointment.AppStatusId = AppStatusId;
                appointment.AppResultId = AppResultId;
                appointment.AppSuccessId = AppSuccessId;
                appointment.AppEtcId = AppEtcId;
                appointment.AppRoomId = AppRoomId;
                appointment.AppUpdateDate = DateTime.Now;
                appointment.AppUpdateBy = HttpContext.Session.GetString("Username");
                try
                {
                    appointment.AppDate = DateTime.Parse(app.Substring(6, 4) + "-" + app.Substring(3, 2) + "-" + app.Substring(0, 2) + " " + app.Substring(11, 5) + ":00");
                }
                catch
                {

                }

                _context.appointments.Update(appointment);
                _context.SaveChanges();
                ////return RedirectToAction("Details", "HRRecruites", new { id = apphrid });



                return RedirectToAction("Details", "HRRecruites", new { id = apphrid });
            }
            ViewData["AppEtcId"] = new SelectList(_context.appEtcs, "AppEtcId", "AppEtcName");
            ViewData["AppResultId"] = new SelectList(_context.appResults, "AppResultId", "AppResultName");
            ViewData["AppStatusId"] = new SelectList(_context.appStatuses, "AppStatusId", "AppStatusName");
            ViewData["AppSuccessId"] = new SelectList(_context.appSuccesses, "AppSuccessId", "AppSuccessName");
            ViewData["AppTelTypeId"] = new SelectList(_context.appTelTypes, "AppTelTypeId", "AppTelTypeName");

            return RedirectToAction("Details", "HRRecruites", new { id = apphrid });
        }





        // POST: HRRecruites/Deletedata/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Deletedata(int appno, int apphrid, int appTelTypeId, int AppStatusId, int AppResultId, string app, int AppSuccessId, int AppEtcId)
        {
            var appointment= await _context.appointments.FindAsync(appno);
            _context.appointments.Remove(appointment);
            await _context.SaveChangesAsync();
            return RedirectToAction("Details", "HRRecruites", new { id = apphrid });
        }



        // POST: HRRecruites/EditData
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Editdata(int hrid,string SignDate, string StartWork,int Status)
        {
            ViewData["HRRecruiteStatusId"] = new SelectList(_context.HRRecruiteStatuses, "HRRecruiteStatusId", "HRRecruiteStatusName");
      

            var hRRecruite = await _context.HRRecruites
                .FirstOrDefaultAsync(m => m.HRRecruiteID == hrid);






            hRRecruite.HRRecruiteStatusId = Status;
            try
            {

                 hRRecruite.StartWork= DateTime.Parse(StartWork.Substring(6, 4) + "-" + StartWork.Substring(3, 2) + "-" + StartWork.Substring(0, 2));
            }
            catch
            {
                hRRecruite.StartWork = null;
            }

            try
            {

                hRRecruite.SignDate = DateTime.Parse(SignDate.Substring(6, 4) + "-" + SignDate.Substring(3, 2) + "-" + SignDate.Substring(0, 2));
            }
            catch
            {
                hRRecruite.SignDate = null;
            }
               



            var laststatus = 0;


            ViewData["hRRecruite"] = hRRecruite;

            DateTime? SignDate1 = null;
            DateTime? Startwork = null;

            laststatus = hRRecruite.HRRecruiteStatusId;
            SignDate1 = hRRecruite.SignDate;
            Startwork = hRRecruite.StartWork;









            ViewData["AppEtcId"] = new SelectList(_context.appEtcs, "AppEtcId", "AppEtcName");
            ViewData["AppResultId"] = new SelectList(_context.appResults, "AppResultId", "AppResultName");
            ViewData["AppStatusId"] = new SelectList(_context.appStatuses, "AppStatusId", "AppStatusName");
            ViewData["AppSuccessId"] = new SelectList(_context.appSuccesses, "AppSuccessId", "AppSuccessName");
            ViewData["AppTelTypeId"] = new SelectList(_context.appTelTypes, "AppTelTypeId", "AppTelTypeName");





            ViewBag.hrid = hrid;



            if (SignDate != null)  //มีวันที่เซ็นสัญญา
            {
                laststatus = 6;
            }
            if (Startwork != null)  //มีวันที่เริ่มทำงาน
            {
                laststatus = 6;
            }





      
            hRRecruite.HRRecruiteStatusId = laststatus;






            if (ModelState.IsValid)
            {



                _context.Update(hRRecruite);
                await _context.SaveChangesAsync();
                return RedirectToAction("Details", "HRRecruites", new { id = hrid });
            }


            return RedirectToAction("Details", "HRRecruites", new { id = hrid });
        }








        // POST: HRRecruites/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("HRRecruiteID,HRRecruiteCardId,TitleOfUserId,HRRecruiteFName,HRRecruiteLName,HRRecruiteNickname,HRRecruitBirth,HRRecruiteTel,HRRecruiteEmail,HRRecruiteLineId,LevelId,HRRecruitDate,HRRecruiteGroupId,UniversityId,GPA,YearCongrate,TypeCongrateId,FacultyId,MajorId,LastWorkYear,ExWorkYear,LastPosition,TypeOfResignId,TypeOfSalaryId,BloodId,StartWork,HRRecruiteStatusId,organizId")] HRRecruite hRRecruite,string birth)
        {


            if (ModelState.IsValid)
            {
                hRRecruite.HRRecruitBirth= DateTime.Parse(birth.Substring(6, 4) + "-" + birth.Substring(3, 2) + "-" + birth.Substring(0, 2));


                hRRecruite.HRRecruitDate = DateTime.Now;
                hRRecruite.HRRecruiteCreateDate= DateTime.Now;
                hRRecruite.HRRecruiteBy= HttpContext.Session.GetString("Username");
                _context.Add(hRRecruite);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["BloodId"] = new SelectList(_context.Bloods, "BloodId", "BloodId", hRRecruite.BloodId);
            ViewData["FacultyId"] = new SelectList(_context.Faculties, "FacultyId", "FacultyId", hRRecruite.FacultyId);
            ViewData["HRRecruiteStatusId"] = new SelectList(_context.HRRecruiteStatuses, "HRRecruiteStatusId", "HRRecruiteStatusName", hRRecruite.HRRecruiteStatusId);
            ViewData["MajorId"] = new SelectList(_context.Majors, "MajorId", "MajorId", hRRecruite.MajorId);
            ViewData["TypeCongrateId"] = new SelectList(_context.TypeCongrates, "TypeCongrateId", "TypeCongrateId", hRRecruite.TypeCongrateId);
            ViewData["TypeOfResignId"] = new SelectList(_context.TypeOfResigns, "TypeOfResignId", "TypeOfResignId", hRRecruite.TypeOfResignId);
            ViewData["TypeOfSalaryId"] = new SelectList(_context.TypeOfSalaries, "TypeOfSalaryId", "TypeOfSalaryId", hRRecruite.TypeOfSalaryId);


            ViewData["BloodId"] = new SelectList(_context.Bloods, "BloodId", "BloodName", hRRecruite.BloodId);
            ViewData["FacultyId"] = new SelectList(_context.Faculties, "FacultyId", "FacultyName", hRRecruite.FacultyId);
            ViewData["HRRecruiteStatusId"] = new SelectList(_context.HRRecruiteStatuses, "HRRecruiteStatusId", "HRRecruiteStatusName", hRRecruite.HRRecruiteStatusId);
            ViewData["MajorId"] = new SelectList(_context.Majors, "MajorId", "MajorName", hRRecruite.MajorId);
            ViewData["TypeCongrateId"] = new SelectList(_context.TypeCongrates, "TypeCongrateId", "TypeCongrateName", hRRecruite.TypeCongrateId);
            ViewData["TypeOfResignId"] = new SelectList(_context.TypeOfResigns, "TypeOfResignId", "TypeOfResignName", hRRecruite.TypeOfResignId);
            ViewData["TypeOfSalaryId"] = new SelectList(_context.TypeOfSalaries, "TypeOfSalaryId", "TypeOfSalaryName", hRRecruite.TypeOfSalaryId);
            ViewData["UniversityId"] = new SelectList(_context.Universities, "UniversityId", "UniversiryName", hRRecruite.UniversityId);
            ViewData["TitleId"] = new SelectList(_context.TitleOfUsers, "TitleOfUserId", "TitleOfUserName", hRRecruite.TitleOfUserId);

            //ViewData["DepartmentId"] = new SelectList(_context.Departments, "DepartmentId", "DepartmentName", hRRecruite.DepartmentId);
            //ViewData["Department1Id"] = new SelectList(_context.Department1s, "Department1Id", "Department1Name", hRRecruite.Department1Id);
            //ViewData["PositionId"] = new SelectList(_context.Positions, "PositionId", "PositionName", hRRecruite.PositionId);
            ViewData["LevelId"] = new SelectList(_context.Levels, "LevelId", "LevelName", hRRecruite.LevelId);
            ViewData["HRRecruiteGroupId"] = new SelectList(_context.HRRecruiteGroups, "HRRecruiteGroupId", "HRRecruiteGroupDetail", hRRecruite.HRRecruiteGroupId);



            return View(hRRecruite);
        }

        // GET: HRRecruites/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var hRRecruite = await _context.HRRecruites.FindAsync(id);
            if (hRRecruite == null)
            {
                return NotFound();
            }

            ViewData["BloodId"] = new SelectList(_context.Bloods, "BloodId", "BloodName", hRRecruite.BloodId);
            ViewData["FacultyId"] = new SelectList(_context.Faculties, "FacultyId", "FacultyName", hRRecruite.FacultyId);
            ViewData["HRRecruiteStatusId"] = new SelectList(_context.HRRecruiteStatuses, "HRRecruiteStatusId", "HRRecruiteStatusName", hRRecruite.HRRecruiteStatusId);
            ViewData["MajorId"] = new SelectList(_context.Majors, "MajorId", "MajorName", hRRecruite.MajorId);
            ViewData["TypeCongrateId"] = new SelectList(_context.TypeCongrates, "TypeCongrateId", "TypeCongrateName", hRRecruite.TypeCongrateId);
            ViewData["TypeOfResignId"] = new SelectList(_context.TypeOfResigns, "TypeOfResignId", "TypeOfResignName", hRRecruite.TypeOfResignId);
            ViewData["TypeOfSalaryId"] = new SelectList(_context.TypeOfSalaries, "TypeOfSalaryId", "TypeOfSalaryName", hRRecruite.TypeOfSalaryId);


            ViewData["BloodId"] = new SelectList(_context.Bloods, "BloodId", "BloodName",hRRecruite.BloodId);
            ViewData["FacultyId"] = new SelectList(_context.Faculties, "FacultyId", "FacultyName",hRRecruite.FacultyId);
            ViewData["HRRecruiteStatusId"] = new SelectList(_context.HRRecruiteStatuses, "HRRecruiteStatusId", "HRRecruiteStatusName",hRRecruite.HRRecruiteStatusId);
            ViewData["MajorId"] = new SelectList(_context.Majors, "MajorId", "MajorName",hRRecruite.MajorId);
            ViewData["TypeCongrateId"] = new SelectList(_context.TypeCongrates, "TypeCongrateId", "TypeCongrateName",hRRecruite.TypeCongrateId);
            ViewData["TypeOfResignId"] = new SelectList(_context.TypeOfResigns, "TypeOfResignId", "TypeOfResignName",hRRecruite.TypeOfResignId);
            ViewData["TypeOfSalaryId"] = new SelectList(_context.TypeOfSalaries, "TypeOfSalaryId", "TypeOfSalaryName", hRRecruite.TypeOfSalaryId);
            ViewData["UniversityId"] = new SelectList(_context.Universities, "UniversityId", "UniversiryName",hRRecruite.UniversityId);
            ViewData["TitleId"] = new SelectList(_context.TitleOfUsers, "TitleOfUserId", "TitleOfUserName",hRRecruite.TitleOfUserId);

            ViewData["LevelId"] = new SelectList(_context.Levels, "LevelId", "LevelName",hRRecruite.LevelId);
            ViewData["HRRecruiteGroupId"] = new SelectList(_context.HRRecruiteGroups, "HRRecruiteGroupId", "HRRecruiteGroupDetail",hRRecruite.HRRecruiteGroupId);
            ViewBag.Birth = hRRecruite.HRRecruitBirth.ToString("dd-MM-yyyy", new CultureInfo("en-US"));
            var organiz = _context.Organizs
                      .Include(p => p.Departments)
                      .Include(p => p.Department1s)
                      .Include(p => p.Positions)
                      .ToList();
            ViewBag.Position = hRRecruite.Organizs.Positions.PositionName;
            ViewBag.Department1 = hRRecruite.Organizs.Department1s.Department1Name;
            ViewBag.Department = hRRecruite.Organizs.Departments.DepartmentName;



            ViewData["organiz"] = organiz;

            return View(hRRecruite);
        }

        // POST: HRRecruites/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("HRRecruiteID,HRRecruiteCardId,TitleOfUserId,HRRecruiteFName,HRRecruiteLName,HRRecruiteNickname,HRRecruitBirth,HRRecruiteTel,HRRecruiteEmail,HRRecruiteLineId,LevelId,HRRecruitDate,HRRecruiteGroupId,UniversityId,GPA,YearCongrate,TypeCongrateId,FacultyId,MajorId,LastWorkYear,ExWorkYear,LastPosition,TypeOfResignId,TypeOfSalaryId,BloodId,StartWork,HRRecruiteStatusId,organizId")] HRRecruite hRRecruite,string birth)
        {
            if (id != hRRecruite.HRRecruiteID)
            {
                return NotFound();
            }

            hRRecruite.HRRecruitBirth = DateTime.Parse(birth.Substring(6, 4) + "-" + birth.Substring(3, 2) + "-" + birth.Substring(0, 2));


            hRRecruite.HRRecruiteUpdateDate = DateTime.Now;
            hRRecruite.HRRecruiteUpdateBy = HttpContext.Session.GetString("Username");



            if (ModelState.IsValid)
            {
                try
                {


                    _context.Update(hRRecruite);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!HRRecruiteExists(hRRecruite.HRRecruiteID))
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
            ViewData["BloodId"] = new SelectList(_context.Bloods, "BloodId", "BloodId", hRRecruite.BloodId);
            ViewData["FacultyId"] = new SelectList(_context.Faculties, "FacultyId", "FacultyId", hRRecruite.FacultyId);
            ViewData["HRRecruiteStatusId"] = new SelectList(_context.HRRecruiteStatuses, "HRRecruiteStatusId", "HRRecruiteStatusId", hRRecruite.HRRecruiteStatusId);
            ViewData["MajorId"] = new SelectList(_context.Majors, "MajorId", "MajorId", hRRecruite.MajorId);
            ViewData["TypeCongrateId"] = new SelectList(_context.TypeCongrates, "TypeCongrateId", "TypeCongrateId", hRRecruite.TypeCongrateId);
            ViewData["TypeOfResignId"] = new SelectList(_context.TypeOfResigns, "TypeOfResignId", "TypeOfResignId", hRRecruite.TypeOfResignId);
            ViewData["TypeOfSalaryId"] = new SelectList(_context.TypeOfSalaries, "TypeOfSalaryId", "TypeOfSalaryId", hRRecruite.TypeOfSalaryId);
            ViewData["BloodId"] = new SelectList(_context.Bloods, "BloodId", "BloodName", hRRecruite.BloodId);
            ViewData["FacultyId"] = new SelectList(_context.Faculties, "FacultyId", "FacultyName", hRRecruite.FacultyId);
            ViewData["HRRecruiteStatusId"] = new SelectList(_context.HRRecruiteStatuses, "HRRecruiteStatusId", "HRRecruiteStatusName", hRRecruite.HRRecruiteStatusId);
            ViewData["MajorId"] = new SelectList(_context.Majors, "MajorId", "MajorName", hRRecruite.MajorId);
            ViewData["TypeCongrateId"] = new SelectList(_context.TypeCongrates, "TypeCongrateId", "TypeCongrateName", hRRecruite.TypeCongrateId);
            ViewData["TypeOfResignId"] = new SelectList(_context.TypeOfResigns, "TypeOfResignId", "TypeOfResignName", hRRecruite.TypeOfResignId);
            ViewData["TypeOfSalaryId"] = new SelectList(_context.TypeOfSalaries, "TypeOfSalaryId", "TypeOfSalaryName", hRRecruite.TypeOfSalaryId);
            ViewData["UniversityId"] = new SelectList(_context.Universities, "UniversityId", "UniversiryName", hRRecruite.UniversityId);
            ViewData["TitleId"] = new SelectList(_context.TitleOfUsers, "TitleOfUserId", "TitleOfUserName", hRRecruite.TitleOfUserId);

            ViewData["LevelId"] = new SelectList(_context.Levels, "LevelId", "LevelName", hRRecruite.LevelId);
            ViewData["HRRecruiteGroupId"] = new SelectList(_context.HRRecruiteGroups, "HRRecruiteGroupId", "HRRecruiteGroupDetail", hRRecruite.HRRecruiteGroupId);
            ViewBag.Birth = hRRecruite.HRRecruitBirth.ToString("dd-MM-yyyy", new CultureInfo("en-US"));
            var organiz = _context.Organizs
                      .Include(p => p.Departments)
                      .Include(p => p.Department1s)
                      .Include(p => p.Positions)
                      .ToList();
            ViewBag.Position = hRRecruite.Organizs.Positions.PositionName;
            ViewBag.Department1 = hRRecruite.Organizs.Department1s.Department1Name;
            ViewBag.Department = hRRecruite.Organizs.Departments.DepartmentName;



            ViewData["organiz"] = organiz;


            return View(hRRecruite);
        }

        // GET: HRRecruites/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            var hRRecruite1 = _context.HRRecruites
                        .Include(h => h.Bloods)
                        .Include(h => h.Faculties)
                        .Include(h => h.HRRecruiteStatuses)
                        .Include(h => h.Majors)
                        .Include(h => h.TypeCongrates)
                        .Include(h => h.TypeOfResigns)
                        .Include(h => h.TypeOfSalaries)
                        .Include(h => h.TitleOfUsers)
                        .Include(h => h.Organizs.Positions)
                        .Include(h => h.Organizs.Departments)
                        .Include(h => h.Organizs.Department1s)
                        .Include(h => h.Levels)
                        .Include(h => h.Universities)
                        .Include(h => h.HRRecruiteGroups)
                        .Where(h => h.HRRecruiteID == id)
                        .ToList();

            ViewData["hRRecruite"] = hRRecruite1;

            ViewData["HRRecruiteStatusId"] = new SelectList(_context.HRRecruiteStatuses, "HRRecruiteStatusId", "HRRecruiteStatusName");



            ViewData["AppEtcId"] = new SelectList(_context.appEtcs, "AppEtcId", "AppEtcName");
            ViewData["AppResultId"] = new SelectList(_context.appResults, "AppResultId", "AppResultName");
            ViewData["AppStatusId"] = new SelectList(_context.appStatuses, "AppStatusId", "AppStatusName");
            ViewData["AppSuccessId"] = new SelectList(_context.appSuccesses, "AppSuccessId", "AppSuccessName");
            ViewData["AppTelTypeId"] = new SelectList(_context.appTelTypes, "AppTelTypeId", "AppTelTypeName");
            ViewData["AppRoomId"] = new SelectList(_context.appRooms, "AppRoomId", "AppRoomName");
            ViewData["AppStatusEdit"] = new SelectList(_context.appStatuses, "AppStatusId", "AppStatusName");

            ViewBag.hrid = id;


            var databaseContext = _context.appointments
                .Include(a => a.appEtcs)
                .Include(a => a.appResults)
                .Include(a => a.appStatuses)
                .Include(a => a.appSuccesses)
                .Include(a => a.appTelTypes)
                .Where(a => a.HRRecruiteID == id)
                .ToList();

            ViewData["applist"] = databaseContext;
            return View();
        }

        // POST: HRRecruites/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var hRRecruite = await _context.HRRecruites.FindAsync(id);
            _context.HRRecruites.Remove(hRRecruite);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool HRRecruiteExists(int id)
        {
            return _context.HRRecruites.Any(e => e.HRRecruiteID == id);
        }


        [HttpGet]
        public IActionResult GetData()
        {

            
            IActionResult response = Unauthorized();
            var query = "SELECT a.organizId as id,a.DepartmentName as Department,a.Department1Name as Department1,a.PositionName as PositionData," +
                "(SELECT count(*) FROM Users WHERE organizId=a.organizId and StatusUserId=1) as CountData,a.Power as Power," +
                "CASE  WHEN (((SELECT count(*) FROM Users WHERE organizId=a.organizId and StatusUserId=1)-a.Power)>=1) THEN CONCAT('เกิน ',((SELECT count(*) FROM Users WHERE organizId=a.organizId and StatusUserId=1)-a.Power),' ตำแหน่ง')" +
                " WHEN (((SELECT count(*) FROM Users WHERE organizId=a.organizId and StatusUserId=1)-a.Power)<0)  THEN CONCAT('ขาด ',(a.Power-(SELECT count(*) FROM Users WHERE organizId=a.organizId and StatusUserId=1)),'ตำแหน่ง')" +
                " WHEN (((SELECT count(*) FROM Users WHERE organizId=a.organizId and StatusUserId=1)-a.Power)=0)  THEN 'พอดี '" +
                " END AS Diff" +
                " FROM(" +
                " SELECT dbo.Organizs.organizId,dbo.Departments.DepartmentName,dbo.Department1s.Department1Name,dbo.Positions.PositionName,dbo.Organizs.Power " +
                " FROM dbo.Organizs" +
                " INNER JOIN dbo.Departments ON dbo.Organizs.DepartmentId = dbo.Departments.DepartmentId" +
                " INNER JOIN dbo.Department1s ON dbo.Organizs.Department1Id = dbo.Department1s.Department1Id" +
                " INNER JOIN dbo.Positions ON dbo.Organizs.PositionId = dbo.Positions.PositionId" +
                "	) as a";

            var dataresult = _context.reportManPowers.FromSqlRaw(query).ToList();


            response = Ok(new { data = dataresult });
            //response = Ok(new { table = Job });
            //response = Ok(new { table = queryLocation });
            return response;
        }



        [HttpGet]
        public IActionResult PositionPower()
        {
            return View();
        }



        [HttpGet]
        public IActionResult Calendar()
        {
            return View();
        }


        [HttpGet]
        public IActionResult Report()
        {
            /*Check Session */
            var page = "233";
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
            ViewBag.StartDate = DateTime.Now.ToString("01-MM-yyyy", new CultureInfo("en-US"));
            ViewBag.EndDate = DateTime.Now.ToString("dd-MM-yyyy", new CultureInfo("en-US"));
            return View();
        }

        [HttpGet]
        public IActionResult GetApp(string date1,string date2)
        {
            var datestart = DateTime.Parse(date1.Substring(6, 4) + "-" + date1.Substring(3, 2) + "-" + date1.Substring(0, 2)+" 00:00:00");
            var dateend = DateTime.Parse(date2.Substring(6, 4) + "-" + date2.Substring(3, 2) + "-" + date2.Substring(0, 2) + " 23:59:59");
            IActionResult response = Unauthorized();
            var query = from a in _context.HRRecruites
                     
                        .Where(a => a.HRRecruitDate >= datestart && a.HRRecruitDate <= dateend)
                        .GroupBy(a => new { a.HRRecruiteStatuses.HRRecruiteStatusName })
                        select new
                        {
                            x = a.Key.HRRecruiteStatusName,
                            y = a.Count()
                        };

 



            response = Ok(query);

            return response;


        }



        [HttpGet]
        public IActionResult GetAddWeek(string date1, string date2)
        {
            var dates = DateTime.Parse(date1.Substring(6, 4) + "-" + date1.Substring(3, 2) + "-" + date1.Substring(0, 2) + " 00:00:00");
            var dateend = DateTime.Parse(date2.Substring(6, 4) + "-" + date2.Substring(3, 2) + "-" + date2.Substring(0, 2) + " 23:59:59");
            IActionResult response = Unauthorized();

 


            var query = "select CONVERT(varchar,CONVERT(date,AppDate)) as X,count(CONVERT(varchar,CONVERT(date,AppDate))) as Y  from appointments  WHERE AppDate>={0} and AppDate<={1}  GROUP BY CONVERT(date,AppDate) ORDER BY X";

            //SqlParameter parameterDate1 = new SqlParameter("@date1", dates);
            //SqlParameter parameterDate2 = new SqlParameter("@date2", dateend);
            var a = _context.dataXY.FromSqlRaw(query,dates,dateend).ToList();






            response = Ok(a);

            return response;


        }

        public IActionResult GetEvents()
        {
            IActionResult response = Unauthorized();
            var databaseContext = _context.appointments
                .Include(a => a.appEtcs)
                .Include(a => a.appResults)
                .Include(a => a.appStatuses)
                .Include(a => a.appSuccesses)
                .Include(a => a.appTelTypes)
                .Include(a=>a.appRooms)
                .Include(a=>a.HRRecruites)
                .Include(a=>a.HRRecruites.Organizs.Positions)
                .Include(a => a.HRRecruites.Organizs.Department1s)
                .Include(a => a.HRRecruites.Organizs.Departments)
                .Where(a => a.AppDate!=null)
                .ToList();



            List<Event> instances = new List<Event>();
            Event current = null;

            foreach (var std in databaseContext as IList<Appointment>)
            {

                
                current = new Event();
                current.Id = std.AppId;
                current.Subject = std.HRRecruites.HRRecruiteFName +" "+ std.HRRecruites.HRRecruiteLName;
                current.Description ="สถานที่ : "+std.appRooms.AppRoomName +"<br>วันที่ Recruite : "+std.HRRecruites.HRRecruitDate.ToString("dd/MM/yyyy HH:mm:ss") +"<br>ตำแหน่งที่ต้องการ : "+std.HRRecruites.Organizs.Positions.PositionName+"<br>แผนก : "+std.HRRecruites.Organizs.Department1s.Department1Name+"<br>ฝ่าย : "+std.HRRecruites.Organizs.Departments.DepartmentName;
                current.Start = (DateTime)std.AppDate;
                current.ThemeColor = std.appRooms.AppRoomColor;
            
                instances.Add(current);
            }



            response = Ok(instances);

            return response;

        }






        [HttpGet]
        public IActionResult GetSumWeek(string year)
        {
        
            IActionResult response = Unauthorized();




            var query = "SELECT case WHEN DATEPART(week,GETDATE())=DATEPART(week, AppDate) THEN CONCAT(DATEPART(week, AppDate),' Now') ELSE  CONCAT(DATEPART(week, AppDate),'') END as X,count(DATEPART(week, AppDate)) AS Y  FROM appointments WHERE year(AppDate)=2019 GROUP BY DATEPART(week, AppDate)";

            //SqlParameter parameterYear = new SqlParameter("@year", year);

            var a = _context.dataXY.FromSqlRaw(query, year).ToList();


            response = Ok(new { data = a });


            return response;


        }




            public IActionResult GetTableResuiteData(string StartDate,string EndDate)
            {
            var date1 = StartDate.Substring(6, 4) + "-" + StartDate.Substring(3, 2) + "-" + StartDate.Substring(0, 2) + " 00:00:00";
            var date2 = EndDate.Substring(6, 4) + "-" + EndDate.Substring(3, 2) + "-" + EndDate.Substring(0, 2) + " 23:59:59";
            

            IActionResult response = Unauthorized();

            var query = "SELECT ROW_NUMBER() OVER (ORDER BY c.DepartmentName) AS RowNumber,c.DepartmentName,c.Department1Name,c.PositionName,c.RecuireNow,c.Apptotal,c.AppTel,"
                        + " CONCAT(convert(varchar,FORMAT(((CAST(c.AppTel As FLOAT))/ CAST(NULLIF(c.Apptotal,0) as Float )*100),'###,###,###.00','en-US')),'%') as PerAppTel,"
                        + "c.AppCome,"
                        + " CONCAT(convert(varchar,FORMAT((CAST(c.AppCome AS float)/(CAST(NULLIF(c.Apptotal,0) AS float)))*100,'###,###,###.00','en-US')),'%') as PerAppCome,"
                        + "c.AppWait,"
                        + " CONCAT(convert(varchar,FORMAT((CAST(c.AppWait AS float)/(CAST(NULLIF(c.Apptotal,0) AS float)))*100,'###,###,###.00','en-US')),'%') as PerAppWait,"
                        + "c.AppSucc,"
                        + " CONCAT(convert(varchar,FORMAT((CAST(c.AppSucc AS float)/(CAST(NULLIF(c.Apptotal,0) AS float)))*100,'###,###,###.00','en-US')),'%') as PerAppSucc,"
                        + "c.AppStart,"
                        + " CONCAT(convert(varchar,FORMAT((CAST(c.AppStart AS float)/(CAST(NULLIF(c.Apptotal,0) AS float)))*100,'###,###,###.00','en-US')),'%') as PerAppStart," +
                        "c.AppEtc2,c.AppEtc3,c.AppEtc4,c.AppEtc5 "
                        + " FROM("
                        + " SELECT b.DepartmentName,b.Department1Name,b.PositionName,b.RecuireNow,"
                        + "(SELECT count(DISTINCT dbo.appointments.HRRecruiteID) FROM dbo.appointments INNER JOIN dbo.HRRecruites ON dbo.appointments.HRRecruiteID = dbo.HRRecruites.HRRecruiteID "
                        + " WHERE organizId=b.organizId and appTelDate>={0} and appTelDate<={1}) as Apptotal,"
                        + "	(SELECT count(DISTINCT dbo.appointments.HRRecruiteID) FROM dbo.appointments INNER JOIN dbo.HRRecruites ON dbo.appointments.HRRecruiteID = dbo.HRRecruites.HRRecruiteID "
                        + "	WHERE organizId=b.organizId and appTelDate>={0} and appTelDate<={1} and AppResultId=3 and appStatusId=3) as appTel,"
                        + "	(SELECT count(DISTINCT dbo.appointments.HRRecruiteID) FROM dbo.appointments INNER JOIN dbo.HRRecruites ON dbo.appointments.HRRecruiteID = dbo.HRRecruites.HRRecruiteID  "
                        + "	WHERE organizId=b.organizId and appTelDate>={0} and appTelDate<={1} and (appSuccessId=2 or appSuccessId=3 or appSuccessId=6)and AppResultId=3 and appStatusId=3) as AppCome,"
                        + "	(SELECT count(DISTINCT dbo.appointments.HRRecruiteID) FROM dbo.appointments INNER JOIN dbo.HRRecruites ON dbo.appointments.HRRecruiteID = dbo.HRRecruites.HRRecruiteID  "
                        + " WHERE organizId=b.organizId and appTelDate>={0} and appTelDate<={1} and appSuccessId=6 and AppResultId=3) as AppWait,"
                        + "	(SELECT count(DISTINCT dbo.appointments.HRRecruiteID) FROM dbo.appointments INNER JOIN dbo.HRRecruites ON dbo.appointments.HRRecruiteID = dbo.HRRecruites.HRRecruiteID  "
                        + " WHERE organizId=b.organizId and appTelDate>={0} and appTelDate<={1} and appSuccessId=3) as AppSucc,"
                        + "	(SELECT count(DISTINCT dbo.appointments.HRRecruiteID) FROM dbo.appointments INNER JOIN dbo.HRRecruites ON dbo.appointments.HRRecruiteID = dbo.HRRecruites.HRRecruiteID  "
                        + " WHERE organizId=b.organizId and appTelDate>={0} and appTelDate<={1} and HRRecruiteStatusId=6) as AppStart, " +
                          " (SELECT count(DISTINCT dbo.appointments.AppEtcId) FROM dbo.appointments INNER JOIN dbo.HRRecruites ON dbo.appointments.HRRecruiteID = dbo.HRRecruites.HRRecruiteID " +
                        " WHERE organizId=b.organizId and appTelDate>={0} and appTelDate<={1} and AppResultId=3 and dbo.appointments.appSuccessId=4 and dbo.appointments.AppEtcId=2) as AppEtc2, " +
                        " (SELECT count(DISTINCT dbo.appointments.AppEtcId) FROM dbo.appointments INNER JOIN dbo.HRRecruites ON dbo.appointments.HRRecruiteID = dbo.HRRecruites.HRRecruiteID " +
                        " 	WHERE organizId=b.organizId and appTelDate>={0} and appTelDate<={1} and AppResultId=3 and dbo.appointments.appSuccessId=4 and dbo.appointments.AppEtcId=3) as AppEtc3, " +
                        " (SELECT count(DISTINCT dbo.appointments.AppEtcId) FROM dbo.appointments INNER JOIN dbo.HRRecruites ON dbo.appointments.HRRecruiteID = dbo.HRRecruites.HRRecruiteID  " +
                        " 	WHERE organizId=b.organizId and appTelDate>={0} and appTelDate<={1} and AppResultId=3 and dbo.appointments.appSuccessId=4 and dbo.appointments.AppEtcId=4) as AppEtc4, " +
                        " 	(SELECT count(DISTINCT dbo.appointments.AppEtcId) FROM dbo.appointments INNER JOIN dbo.HRRecruites ON dbo.appointments.HRRecruiteID = dbo.HRRecruites.HRRecruiteID  " +
                        " 	WHERE organizId=b.organizId and appTelDate>={0} and appTelDate<={1} and AppResultId=3 and dbo.appointments.appSuccessId=4 and dbo.appointments.AppEtcId>=5) as AppEtc5 "
                        + "	FROM(SELECT a.organizId,a.DepartmentName,a.Department1Name,a.PositionName,"
                        + "		a.Power-(SELECT count(*) FROM Users WHERE organizId=a.organizId and UserCreateDate<{0} and StatusUserId=1)as RecuireNow"
                        + "		FROM("
                        + "			SELECT dbo.Organizs.organizId,dbo.Departments.DepartmentName,dbo.Department1s.Department1Name,dbo.Positions.PositionName,dbo.Organizs.Power "
                        + "			FROM dbo.Organizs"
                        + "			INNER JOIN dbo.Departments ON dbo.Organizs.DepartmentId = dbo.Departments.DepartmentId"
                        + "			INNER JOIN dbo.Department1s ON dbo.Organizs.Department1Id = dbo.Department1s.Department1Id"
                        + "			INNER JOIN dbo.Positions ON dbo.Organizs.PositionId = dbo.Positions.PositionId"
                        + "		) as a  "
                        + "	)as b "
                        + ")as c "

                        + "WHERE c.RecuireNow<>0 or c.Apptotal<>0"
                        + "ORDER BY c.DepartmentName,c.Department1Name,c.PositionName ASC";
            //SqlParameter parameterStartDate = new SqlParameter("@startdate", date1);
            //SqlParameter parameterEndDate = new SqlParameter("@enddate", date2);

            var a = _context.v_RecruiteReports.FromSqlRaw(query, date1,date2).ToList();

            response = Ok(new { data = a });


                return response;
            }







        public IActionResult GetTableReport2(string StartDate, string EndDate)
        {
            var date1 = StartDate.Substring(6, 4) + "-" + StartDate.Substring(3, 2) + "-" + StartDate.Substring(0, 2) + " 00:00:00";
            var date2 = EndDate.Substring(6, 4) + "-" + EndDate.Substring(3, 2) + "-" + EndDate.Substring(0, 2) + " 23:59:59";


            IActionResult response = Unauthorized();

            var query = "SELECT ROW_NUMBER() OVER (ORDER BY c.DepartmentName) AS RowNumber,c.DepartmentName,c.Department1Name,c.PositionName,c.RecuireNow,c.Apptotal,c.AppTel,"
                      + " CONCAT(convert(varchar,FORMAT(((CAST(c.AppTel As FLOAT))/ CAST(NULLIF(c.Apptotal,0) as Float )*100),'###,###,###.00','en-US')),'%') as PerAppTel,"
                      + "c.AppCome,"
                      + " CONCAT(convert(varchar,FORMAT((CAST(c.AppCome AS float)/(CAST(NULLIF(c.Apptotal,0) AS float)))*100,'###,###,###.00','en-US')),'%') as PerAppCome,"
                      + "c.AppWait,"
                      + " CONCAT(convert(varchar,FORMAT((CAST(c.AppWait AS float)/(CAST(NULLIF(c.Apptotal,0) AS float)))*100,'###,###,###.00','en-US')),'%') as PerAppWait,"
                      + "c.AppSucc,"
                      + " CONCAT(convert(varchar,FORMAT((CAST(c.AppSucc AS float)/(CAST(NULLIF(c.Apptotal,0) AS float)))*100,'###,###,###.00','en-US')),'%') as PerAppSucc,"
                      + "c.AppStart,"
                      + " CONCAT(convert(varchar,FORMAT((CAST(c.AppStart AS float)/(CAST(NULLIF(c.Apptotal,0) AS float)))*100,'###,###,###.00','en-US')),'%') as PerAppStart," +
                      "c.AppEtc2,c.AppEtc3,c.AppEtc4,c.AppEtc5 "
                      + " FROM("
                      + " SELECT b.DepartmentName,b.Department1Name,b.PositionName,b.RecuireNow,"
                      + "(SELECT count(DISTINCT dbo.appointments.HRRecruiteID) FROM dbo.appointments INNER JOIN dbo.HRRecruites ON dbo.appointments.HRRecruiteID = dbo.HRRecruites.HRRecruiteID "
                      + " WHERE organizId=b.organizId and appTelDate>={0} and appTelDate<={1}) as Apptotal,"
                      + "	(SELECT count(DISTINCT dbo.appointments.HRRecruiteID) FROM dbo.appointments INNER JOIN dbo.HRRecruites ON dbo.appointments.HRRecruiteID = dbo.HRRecruites.HRRecruiteID "
                      + "	WHERE organizId=b.organizId and appTelDate>={0}e and appTelDate<={1} and AppResultId=3 and appStatusId=3) as appTel,"
                      + "	(SELECT count(DISTINCT dbo.appointments.HRRecruiteID) FROM dbo.appointments INNER JOIN dbo.HRRecruites ON dbo.appointments.HRRecruiteID = dbo.HRRecruites.HRRecruiteID  "
                      + "	WHERE organizId=b.organizId and appTelDate>={0} and appTelDate<={1} and (appSuccessId=2 or appSuccessId=3 or appSuccessId=6)and AppResultId=3 and appStatusId=3) as AppCome,"
                      + "	(SELECT count(DISTINCT dbo.appointments.HRRecruiteID) FROM dbo.appointments INNER JOIN dbo.HRRecruites ON dbo.appointments.HRRecruiteID = dbo.HRRecruites.HRRecruiteID  "
                      + " WHERE organizId=b.organizId and appTelDate>={0} and appTelDate<={1} and appSuccessId=6 and AppResultId=3) as AppWait,"
                      + "	(SELECT count(DISTINCT dbo.appointments.HRRecruiteID) FROM dbo.appointments INNER JOIN dbo.HRRecruites ON dbo.appointments.HRRecruiteID = dbo.HRRecruites.HRRecruiteID  "
                      + " WHERE organizId=b.organizId and appTelDate>={0} and appTelDate<={1} and appSuccessId=3) as AppSucc,"
                      + "	(SELECT count(DISTINCT dbo.appointments.HRRecruiteID) FROM dbo.appointments INNER JOIN dbo.HRRecruites ON dbo.appointments.HRRecruiteID = dbo.HRRecruites.HRRecruiteID  "
                      + " WHERE organizId=b.organizId and appTelDate>={0} and appTelDate<={1} and HRRecruiteStatusId=6) as AppStart, " +
                        " (SELECT count(DISTINCT dbo.appointments.AppEtcId) FROM dbo.appointments INNER JOIN dbo.HRRecruites ON dbo.appointments.HRRecruiteID = dbo.HRRecruites.HRRecruiteID " +
                      " WHERE organizId=b.organizId and appTelDate>={0} and appTelDate<={1} and AppResultId=3 and dbo.appointments.appSuccessId=4 and dbo.appointments.AppEtcId=2) as AppEtc2, " +
                      " (SELECT count(DISTINCT dbo.appointments.AppEtcId) FROM dbo.appointments INNER JOIN dbo.HRRecruites ON dbo.appointments.HRRecruiteID = dbo.HRRecruites.HRRecruiteID " +
                      " 	WHERE organizId=b.organizId and appTelDate>={0}e and appTelDate<={1} and AppResultId=3 and dbo.appointments.appSuccessId=4 and dbo.appointments.AppEtcId=3) as AppEtc3, " +
                      " (SELECT count(DISTINCT dbo.appointments.AppEtcId) FROM dbo.appointments INNER JOIN dbo.HRRecruites ON dbo.appointments.HRRecruiteID = dbo.HRRecruites.HRRecruiteID  " +
                      " 	WHERE organizId=b.organizId and appTelDate>={0} and appTelDate<={1} and AppResultId=3 and dbo.appointments.appSuccessId=4 and dbo.appointments.AppEtcId=4) as AppEtc4, " +
                      " 	(SELECT count(DISTINCT dbo.appointments.AppEtcId) FROM dbo.appointments INNER JOIN dbo.HRRecruites ON dbo.appointments.HRRecruiteID = dbo.HRRecruites.HRRecruiteID  " +
                      " 	WHERE organizId=b.organizId and appTelDate>={0} and appTelDate<={1}e and AppResultId=3 and dbo.appointments.appSuccessId=4 and dbo.appointments.AppEtcId>=5) as AppEtc5 "
                      + "	FROM(SELECT a.organizId,a.DepartmentName,a.Department1Name,a.PositionName,"
                      + "		a.Power-(SELECT count(*) FROM Users WHERE organizId=a.organizId and UserCreateDate<{0} and StatusUserId=1)as RecuireNow"
                      + "		FROM("
                      + "			SELECT dbo.Organizs.organizId,dbo.Departments.DepartmentName,dbo.Department1s.Department1Name,dbo.Positions.PositionName,dbo.Organizs.Power "
                      + "			FROM dbo.Organizs"
                      + "			INNER JOIN dbo.Departments ON dbo.Organizs.DepartmentId = dbo.Departments.DepartmentId"
                      + "			INNER JOIN dbo.Department1s ON dbo.Organizs.Department1Id = dbo.Department1s.Department1Id"
                      + "			INNER JOIN dbo.Positions ON dbo.Organizs.PositionId = dbo.Positions.PositionId"
                      + "		) as a  "
                      + "	)as b "
                      + ")as c "

                      + "WHERE c.RecuireNow<>0 or c.Apptotal<>0"
                      + "ORDER BY c.DepartmentName,c.Department1Name,c.PositionName ASC";
            //SqlParameter parameterStartDate = new SqlParameter("@startdate", date1);
            //SqlParameter parameterEndDate = new SqlParameter("@enddate", date2);

            var a = _context.v_RecruiteReports.FromSqlRaw(query, date1, date2).ToList();

            response = Ok(new { data = a });


            return response;
        }




        [HttpGet]
        public IActionResult Dashboard()
        {
            /*Check Session */
            var page = "234";
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

            return View();
        }
        public IActionResult InteriewReport()
        {
            /*Check Session */
            var page = "234";
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
            ViewBag.StartDate = DateTime.Now.ToString("01-MM-yyyy", new CultureInfo("en-US"));
            ViewBag.EndDate = DateTime.Now.ToString("dd-MM-yyyy", new CultureInfo("en-US"));

            return View();
        }




    }
}
