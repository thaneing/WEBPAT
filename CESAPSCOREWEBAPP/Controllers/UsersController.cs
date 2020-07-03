using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using CESAPSCOREWEBAPP.Models;
using Microsoft.AspNetCore.Http;
using CESAPSCOREWEBAPP.Helpers;
using System.Security.Cryptography;
using System.IO;
using CESAPSCOREWEBAPP.Controllers;
using Microsoft.AspNetCore.Hosting;
using static CESAPSCOREWEBAPP.Models.Enums;
using System.Globalization;
using Rotativa.AspNetCore;

namespace CES.Controllers
{
    public class UsersController : BaseController
    {
        private readonly DatabaseContext _context;

        private readonly NAVContext _navcontext;

        public UsersController(DatabaseContext context,NAVContext navcontext)
        {
            _context = context;
            _navcontext = navcontext;
        }


        // GET: Users
        public async Task<IActionResult> Index()
        {

            /*Check Session */
            var page = "33";
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
            ViewBag.Departments = _context.Departments.ToList();
            ViewBag.branchs = _context.Branchs.ToList();
            ViewBag.dep1s = _context.Department1s.ToList();
            ViewBag.Positiones = _context.Positions.ToList();

            ViewBag.statusUsers = _context.statusUsers.ToList();
            ViewBag.typeOfEmp = _context.typeOfEmployee.ToList();
            ViewBag.levels = _context.Levels.ToList();

            return View(await _context.Users
                        .Include(p => p.Organizs.Positions)
                        .Include(p => p.Organizs.Department1s)
                        .Include(p => p.Organizs.Departments)
                        .Include(p => p.TitleOfUsers)
                        .Include(p => p.StatusUser)
                        .Include(p => p.Branchs)
                        .Include(p => p.Bloods)
                        .Include(p => p.nationality)
                        .Include(p => p.povince)
                        .Include(p => p.TypeCongrates)
                        .Include(p => p.religions)
                        .Include(p => p.typeOfEmployee)
                        .Include(p => p.Levels)
                        .ToListAsync());
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Index(int ddlTypeOfEmp, int ddlDep, int ddlDep1s, int ddlPosition, int ddlBranch, int ddlLevels, int ddlStatus)
        {
            var users = await _context.Users
                         .Include(p => p.Organizs.Positions)
                         .Include(p => p.Organizs.Department1s)
                         .Include(p => p.Organizs.Departments)
                         .Include(p => p.TitleOfUsers)
                         .Include(p => p.StatusUser)
                         .Include(p => p.Branchs)
                         .Include(p => p.Bloods)
                         .Include(p => p.nationality)
                         .Include(p => p.povince)
                         .Include(p => p.TypeCongrates)
                         .Include(p => p.religions)
                         .Include(p => p.typeOfEmployee)
                         .Include(p => p.Levels)
                         .ToListAsync();

            ViewBag.Departments = _context.Departments.ToList();
            ViewBag.branchs = _context.Branchs.ToList();
            ViewBag.dep1s = _context.Department1s.ToList();
            ViewBag.Positiones = _context.Positions.ToList();

            ViewBag.statusUsers = _context.statusUsers.ToList();
            ViewBag.typeOfEmp = _context.typeOfEmployee.ToList();
            ViewBag.levels = _context.Levels.ToList();

            if (ddlTypeOfEmp != -1)
            {
                users = users.Where(p =>
                    p.typeOfEmployee.TypeOfEmployeeId == ddlTypeOfEmp).ToList();
            }


            else if (ddlBranch != -1)
            {
                users = users.Where(p =>
                    p.Branchs.BranchId == ddlBranch).ToList();
            }
            else if (ddlDep != -1)
            {
                users = users.Where(p =>
                     p.Organizs.Departments.DepartmentId == ddlDep).ToList();

            }
            else if (ddlDep1s != -1)
            {
                users = users.Where(p =>
                     p.Organizs.Department1s.Department1Id == ddlDep1s).ToList();

            }
            else if (ddlPosition != -1)
            {
                users = users.Where(p =>
                     p.Organizs.Positions.PositionId == ddlPosition).ToList();

            }
            else if (ddlLevels != -1)
            {
                users = users.Where(p =>
                     p.Levels.LevelId == ddlLevels).ToList();

            }
            if (ddlStatus != -1)
            {
                users = users.Where(p =>
                     p.StatusUser.StatusUserId == ddlStatus).ToList();

            }
            else
            {
                users = users.ToList();
            }
            return View(users);
        }




        // GET: Users/Details/5
        public IActionResult Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var user1 = _context.Users
                        .Include(p => p.Organizs.Positions)
                        .Include(p => p.Organizs.Department1s)
                        .Include(p => p.Organizs.Departments)
                        .Include(p => p.TitleOfUsers)
                        .Include(p => p.StatusUser)
                        .Include(p => p.Branchs)
                        .Include(p => p.Bloods)
                        .Include(p => p.nationality)
                        .Include(p => p.povince)
                        .Include(p => p.TypeCongrates)
                        .Include(p => p.religions)
                        .Include(p => p.typeOfEmployee)
                        .Include(p => p.Levels)
                        .Where(p => p.UserId == id)
                        .ToList();
            ViewData["users"] = user1;
            return View();
        }

        // GET: Users/Create
        public IActionResult Create()
        {
            /*Check Session */
            var page = "61";
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


            ViewData["TitleOfUserId"] = new SelectList(_context.TitleOfUsers, "TitleOfUserId", "TitleOfUserName");
            ViewData["LevelId"] = new SelectList(_context.Levels, "LevelId", "LevelName");
            ViewData["BranchId"] = new SelectList(_context.Branchs, "BranchId", "BranchName");
            ViewData["StatusUserId"] = new SelectList(_context.statusUsers, "StatusUserId", "StatusUserName");
            ViewData["TypeOfEmployeeId"] = new SelectList(_context.typeOfEmployee, "TypeOfEmployeeId", "TypeOfEmployeeName");
            ViewData["ReligionId"] = new SelectList(_context.religions, "ReligionId", "ReligionName");
            ViewData["NationalityId"] = new SelectList(_context.nationalities, "NationalityId", "NationalityName");
            ViewData["BloodId"] = new SelectList(_context.Bloods, "BloodId", "BloodName");
            ViewData["PovinceData"] = new SelectList(_context.povinces, "PovinceId", "PovinceName");
            ViewData["TypeCongrateId"]=new SelectList(_context.TypeCongrates,"TypeCongrateId","TypeCongrateName");


            var organiz= _context.Organizs
                                    .Include(p => p.Departments)
                                    .Include(p => p.Department1s)
                                    .Include(p => p.Positions)
                                    .ToList();

            ViewData["organiz"] = organiz;

            return View();
        }

        // POST: Users/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("UserId,TitleOfUserId,Firstname,Lastname,EFirstName,ELastname,Nickname,BirthName,LevelId,organizId,Pic,EmailContact,ExtTel,MobileTel,BranchId,StatusUserId,EmpId,UserCreateDate,BloodId,TypeCongrateId,CongrateDetail,NationalityId,PovinceId,Weight,Height,Waistline,Certificate,Reference,ReferenceTel,ResignationDate,TypeOfEmployeeId,ReligionId,Reletion")] User users, IFormFile uploadPic,string birth,string startwork,string endwork)
        {
            /*Check Session */
            var page = "61";
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



            ViewData["TitleOfUserId"] = new SelectList(_context.TitleOfUsers, "TitleOfUserId", "TitleOfUserName", users.TitleOfUserId);
            ViewData["LevelId"] = new SelectList(_context.Levels, "LevelId", "LevelName", users.LevelId);
            ViewData["BranchId"] = new SelectList(_context.Branchs, "BranchId", "BranchName",users.BranchId);
            ViewData["StatusUserId"] = new SelectList(_context.statusUsers, "StatusUserId", "StatusUserName",users.StatusUserId);
            ViewData["TypeOfEmployeeId"] =new SelectList(_context.typeOfEmployee,"TypeOfEmployeeId","TypeOfEmployeeName",users.TypeOfEmployeeId);
            ViewData["ReligionId"] = new SelectList(_context.religions, "ReligionId", "ReligionName", users.ReligionId);
            ViewData["NationalityId"] = new SelectList(_context.nationalities, "NationalityId", "NationalityName", users.NationalityId);
            ViewData["BloodId"] = new SelectList(_context.Bloods, "BloodId", "BloodName", users.BloodId);
            ViewData["PovinceData"] = new SelectList(_context.povinces, "PovinceId","PovinceName", users.PovinceId);
            ViewData["TypeCongrateId"] = new SelectList(_context.TypeCongrates, "TypeCongrateId", "TypeCongrateName", users.TypeCongrateId);



            var organiz = _context.Organizs
                                  .Include(p => p.Departments)
                                  .Include(p => p.Department1s)
                                  .Include(p => p.Positions)
                                  .ToList();

            ViewData["organiz"] = organiz;

            if (ModelState.IsValid)
            {
                var EndWorkDate = "";

                var date1 = birth.Substring(6, 4) + "-" + birth.Substring(3, 2) + "-" + birth.Substring(0, 2) + " 00:00:00";
                var StartWorkDate = startwork.Substring(6, 4) + "-" + startwork.Substring(3, 2) + "-" + startwork.Substring(0, 2) + " 00:00:00";
                try {
                     EndWorkDate = endwork.Substring(6, 4) + "-" + endwork.Substring(3, 2) + "-" + endwork.Substring(0, 2) + " 00:00:00";
                }
                catch
                {
                    
                }
                var check = 0;
                if (uploadPic == null || uploadPic.Length == 0)
                {
                    users.Pic = "NoImage2019-04-09-09-05-46.jpg";
                    check = 1;
                }
                if (check == 0)
                {
                    if (uploadPic.ContentType.IndexOf("image", StringComparison.OrdinalIgnoreCase) < 0)
                    {
                        users.Pic = "NoImage2019-04-09-09-05-46.jpg";
                        check = 1;
                    }
                }


                if (check == 0)
                {
                    //Upload file
                    string pathImage = "/images/Users/";
                    string pathSave = $"wwwroot{pathImage}";
                    if (!Directory.Exists(pathSave))
                    {
                        Directory.CreateDirectory(pathSave);
                    }
                    if (!Directory.Exists(pathSave + "/32/"))
                    {
                        Directory.CreateDirectory(pathSave + "/32/");
                    }
                    if (!Directory.Exists(pathSave + "/64/"))
                    {
                        Directory.CreateDirectory(pathSave + "/64/");
                    }
                    if (!Directory.Exists(pathSave + "/128/"))
                    {
                        Directory.CreateDirectory(pathSave + "/128/");
                    }
                    if (!Directory.Exists(pathSave + "/256/"))
                    {
                        Directory.CreateDirectory(pathSave + "/256/");
                    }
                    if (!Directory.Exists(pathSave + "/512/"))
                    {
                        Directory.CreateDirectory(pathSave + "/512/");
                    }
                    if (!Directory.Exists(pathSave + "/1028/"))
                    {
                        Directory.CreateDirectory(pathSave + "/1028/");
                    }

                    string fileName = Path.GetFileNameWithoutExtension(uploadPic.FileName);
                    string extension = Path.GetExtension(uploadPic.FileName);
                    fileName = fileName + DateTime.Now.ToString("yyyy-MM-dd-hh-mm-ss") + extension;
                    var path = Path.Combine(Directory.GetCurrentDirectory(), pathSave, fileName);
                    using (var stream = new FileStream(path, FileMode.Create))
                    {
                        await uploadPic.CopyToAsync(stream);
                    }
                    ImageResizeHelper.Image_resize(pathSave + fileName, pathSave + "/32/" + fileName, 32);
                    ImageResizeHelper.Image_resize(pathSave + fileName, pathSave + "/64/" + fileName, 64);
                    ImageResizeHelper.Image_resize(pathSave + fileName, pathSave + "/128/" + fileName, 128);
                    ImageResizeHelper.Image_resize(pathSave + fileName, pathSave + "/256/" + fileName, 256);
                    ImageResizeHelper.Image_resize(pathSave + fileName, pathSave + "/512/" + fileName, 512);
                    ImageResizeHelper.Image_resize(pathSave + fileName, pathSave + "/1028/" + fileName, 1028);

                    users.Pic = fileName;

                }

                //string hashed_password = EncryptionHelper.Encrypt(users.Password);
                //users.Password = hashed_password;


                users.BirthName = Convert.ToDateTime(date1);

                users.UserCreateDate = Convert.ToDateTime(StartWorkDate);
                try { 
                    users.ResignationDate = Convert.ToDateTime(EndWorkDate);
                }
                catch
                {

                }
                _context.Add(users);
                await _context.SaveChangesAsync();
                Alert("สมัครสมาชิกเรียบร้อย", NotificationType.success);
                return RedirectToAction("Index", "Home");
            }

            return View(users);
        }

        // GET: Users/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            /*Check Session */
            var page = "34";
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
                if (HttpContext.Session.GetInt32("Userid") == id) {

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
            }

      
            
            var permisionname = HttpContext.Session.GetString("PermisionName");
            if (typeofuser == "3" || permisionname == "HR" || HttpContext.Session.GetInt32("Userid")== id)
            {

            }
            else
            {
                Alert("คุณไม่มีสิทธิ์ใช้งานหน้าดังกล่าว", NotificationType.error);
                return RedirectToAction("Index", "Home");
            }

        // GET: WebModuls/Details/5




        var users = await _context.Users
                        .Include(p => p.Organizs.Positions)
                        .Include(p => p.Organizs.Department1s)
                        .Include(p => p.Organizs.Departments)
                        .Include(p => p.TitleOfUsers)
                        .Include(p => p.StatusUser)
                        .Include(p => p.Branchs)
                        .Include(p => p.Bloods)
                        .Include(p => p.nationality)
                        .Include(p => p.povince)
                        .Include(p => p.TypeCongrates)
                        .Include(p => p.religions)
                        .Include(p => p.typeOfEmployee)
                        .Include(p => p.Levels)
                        .FirstOrDefaultAsync(m => m.UserId == id);
            if (users == null)
            {
                return NotFound();
            }
            ViewData["TitleOfUserId"] = new SelectList(_context.TitleOfUsers, "TitleOfUserId", "TitleOfUserName", users.TitleOfUserId);
            ViewData["LevelId"] = new SelectList(_context.Levels, "LevelId", "LevelName", users.LevelId);
            ViewData["BranchId"] = new SelectList(_context.Branchs, "BranchId", "BranchName", users.BranchId);
            ViewData["StatusUserId"] = new SelectList(_context.statusUsers, "StatusUserId", "StatusUserName", users.StatusUserId);
            ViewData["TypeOfEmployeeId"] = new SelectList(_context.typeOfEmployee, "TypeOfEmployeeId", "TypeOfEmployeeName", users.TypeOfEmployeeId);
            ViewData["ReligionId"] = new SelectList(_context.religions, "ReligionId", "ReligionName", users.ReligionId);
            ViewData["NationalityId"] = new SelectList(_context.nationalities, "NationalityId", "NationalityName", users.NationalityId);
            ViewData["BloodId"] = new SelectList(_context.Bloods, "BloodId", "BloodName", users.BloodId);
            ViewData["PovinceData"] = new SelectList(_context.povinces, "PovinceId", "PovinceName", users.PovinceId);
            ViewData["TypeCongrateId"] = new SelectList(_context.TypeCongrates, "TypeCongrateId", "TypeCongrateName", users.TypeCongrateId);
            var organiz = _context.Organizs
                                  .Include(p => p.Departments)
                                  .Include(p => p.Department1s)
                                  .Include(p => p.Positions)
                                  .ToList();

            ViewData["organiz"] = organiz;
            var bitrh = users.BirthName;
            var startwork = users.UserCreateDate;
            var endwork = users.ResignationDate;

            ViewBag.startwork = startwork.ToString("dd-MM-yyyy", new CultureInfo("en-US"));

            try {
            ViewBag.date = Convert.ToDateTime(bitrh).ToString("dd-MM-yyyy", new CultureInfo("en-US"));
                if (users.ResignationDate == null) {
                    ViewBag.endwork = "";
                }
                else {

                 ViewBag.endwork = Convert.ToDateTime(endwork).ToString("dd-MM-yyyy", new CultureInfo("en-US"));
                }
            }
            catch
            {

            }
            ViewBag.Level = users.Levels.LevelName;
            ViewBag.branch = users.Branchs.BranchName;
            ViewBag.typeofemployee = users.typeOfEmployee.TypeOfEmployeeName;
            ViewBag.empid = users.EmpId;
            ViewBag.certificate = users.Certificate;
            ViewBag.typeofcongrate = users.TypeCongrates.TypeCongrateName;
            ViewBag.Reference = users.Reference;
            ViewBag.reftel = users.ReferenceTel;
            ViewBag.Statusname = users.StatusUser.StatusUserName;
            ViewBag.Pic = users.Pic;
            ViewBag.Position = users.Organizs.Positions.PositionName;
            ViewBag.Department1 = users.Organizs.Department1s.Department1Name;
            ViewBag.Department = users.Organizs.Departments.DepartmentName;
            return View(users);
        }

        // POST: Users/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("UserId,TitleOfUserId,Firstname,Lastname,EFirstName,ELastname,Nickname,BirthName,LevelId,organizId,Pic,EmailContact,ExtTel,MobileTel,BranchId,StatusUserId,EmpId,UserCreateDate,BloodId,TypeCongrateId,CongrateDetail,NationalityId,PovinceId,Weight,Height,Waistline,Certificate,Reference,ReferenceTel,ResignationDate,TypeOfEmployeeId,ReligionId,Reletion")] User users, IFormFile Pic, string PicDB, string birth, string startwork, string endwork)
        {

            /*Check Session */
            var page = "34";
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
                if (HttpContext.Session.GetInt32("Userid") == id)
                {

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
            }








            var permisionname = HttpContext.Session.GetString("PermisionName");
            if (typeofuser == "3" || permisionname == "HR" || HttpContext.Session.GetInt32("Userid")== id)
            {

            }
            else
            {
                Alert("คุณไม่มีสิทธิ์ใช้งานหน้าดังกล่าว", NotificationType.error);
                return RedirectToAction("Index", "Home");
            }




            //// CheckSession
            //if (string.IsNullOrEmpty(HttpContext.Session.GetString("Username")))
            //{
            //    Alert("คุณไม่มีสิทธิ์ใช้งานหน้าดังกล่าว", NotificationType.error);
            //    return RedirectToAction("Index", "Home");
            //}

            //if (HttpContext.Session.GetString("TypeOfUserId") != "3")
            //{
            //    if (HttpContext.Session.GetInt32("Userid") != id)
            //    {
            //        Alert("เกิดข้อผิดพลาด", NotificationType.error);
            //        return RedirectToAction("Index", "Home");
            //    }
            //}






            ViewData["TitleOfUserId"] = new SelectList(_context.TitleOfUsers, "TitleOfUserId", "TitleOfUserName", users.TitleOfUserId);
            ViewData["LevelId"] = new SelectList(_context.Levels, "LevelId", "LevelName", users.LevelId);
            ViewData["BranchId"] = new SelectList(_context.Branchs, "BranchId", "BranchName", users.BranchId);
            ViewData["StatusUserId"] = new SelectList(_context.statusUsers, "StatusUserId", "StatusUserName", users.StatusUserId);
            ViewData["TypeOfEmployeeId"] = new SelectList(_context.typeOfEmployee, "TypeOfEmployeeId", "TypeOfEmployeeName", users.TypeOfEmployeeId);
            ViewData["ReligionId"] = new SelectList(_context.religions, "ReligionId", "ReligionName", users.ReligionId);
            ViewData["NationalityId"] = new SelectList(_context.nationalities, "NationalityId", "NationalityName", users.NationalityId);
            ViewData["BloodId"] = new SelectList(_context.Bloods, "BloodId", "BloodName", users.BloodId);
            ViewData["PovinceData"] = new SelectList(_context.povinces, "PovinceId", "PovinceName", users.PovinceId);
            ViewData["TypeCongrateId"] = new SelectList(_context.TypeCongrates, "TypeCongrateId", "TypeCongrateName", users.TypeCongrateId);




            if (id != users.UserId)
            {
                return NotFound();
            }


            if (ModelState.IsValid)
            {

                try
                {

                    //if (passwordold != null)  //ถ้ามีการเปลี่ยนพาสเวิร์ด
                    //{
                    //    if (EncryptionHelper.Decrypt(PasswordDB) != passwordold)
                    //    {
                    //        Alert("Password ไม่ถูกต้อง", NotificationType.error);
                    //        return View(users);
                    //    }

                    //    if (passwordnew == null)
                    //    {
                    //        Alert("กรุณากรอก Password ใหม่", NotificationType.error);
                    //        return View(users);
                    //    }
                    //    if (passwordnew != passwordnew1)
                    //    {
                    //        Alert("Password ใหม่ไม่เหมือนกัน", NotificationType.error);
                    //        return View(users);
                    //    }
                    //    //users.Password = EncryptionHelper.Encrypt(passwordnew);
                    //}
                    //else
                    //{
                    //    //users.Password = PasswordDB;
                    //}


                    //if (Pic == null || Pic.Length == 0)
                    //{
                    //    users.Pic = PicDB;
                    //}
                    //else
                    //{
                    //    //upload file
                    //    string pathImage = "/images/Users/";
                    //    string pathSave = $"wwwroot{pathImage}";
                    //    if (!Directory.Exists(pathSave))
                    //    {
                    //        Directory.CreateDirectory(pathSave);
                    //    }

                    //    string fileName = Path.GetFileNameWithoutExtension(Pic.FileName);
                    //    string extension = Path.GetExtension(Pic.FileName);
                    //    fileName = fileName + DateTime.Now.ToString("yyyy-MM-dd-hh-mm-ss") + extension;
                    //    var path = Path.Combine(Directory.GetCurrentDirectory(), pathSave, fileName);


                    //    using (var stream = new FileStream(path, FileMode.Create))
                    //    {
                    //        await Pic.CopyToAsync(stream);
                    //    }
                    //    DateTime dateNoew = DateTime.Now;
                    //    users.Pic = pathImage + fileName;

                    //}

                    var EndWorkDate = "";

                    var date1 = birth.Substring(6, 4) + "-" + birth.Substring(3, 2) + "-" + birth.Substring(0, 2) + " 00:00:00";
                    var StartWorkDate = startwork.Substring(6, 4) + "-" + startwork.Substring(3, 2) + "-" + startwork.Substring(0, 2) + " 00:00:00";
                    try
                    {
                        EndWorkDate = endwork.Substring(6, 4) + "-" + endwork.Substring(3, 2) + "-" + endwork.Substring(0, 2) + " 00:00:00";
                    }
                    catch
                    {

                    }
                    var check = 0;
                    if (Pic == null || Pic.Length == 0)
                    {
                        users.Pic = PicDB;
                        check = 1;
                    }
                    if (check == 0)
                    {
                        if (Pic.ContentType.IndexOf("image", StringComparison.OrdinalIgnoreCase) < 0)
                        {
                            users.Pic = PicDB;
                            check = 1;
                        }
                    }


                    if (check == 0)
                    {
                        //Upload file
                        string pathImage = "/images/Users/";
                        string pathSave = $"wwwroot{pathImage}";
                        if (!Directory.Exists(pathSave))
                        {
                            Directory.CreateDirectory(pathSave);
                        }
                        if (!Directory.Exists(pathSave + "/32/"))
                        {
                            Directory.CreateDirectory(pathSave + "/32/");
                        }
                        if (!Directory.Exists(pathSave + "/64/"))
                        {
                            Directory.CreateDirectory(pathSave + "/64/");
                        }
                        if (!Directory.Exists(pathSave + "/128/"))
                        {
                            Directory.CreateDirectory(pathSave + "/128/");
                        }
                        if (!Directory.Exists(pathSave + "/256/"))
                        {
                            Directory.CreateDirectory(pathSave + "/256/");
                        }
                        if (!Directory.Exists(pathSave + "/512/"))
                        {
                            Directory.CreateDirectory(pathSave + "/512/");
                        }
                        if (!Directory.Exists(pathSave + "/1028/"))
                        {
                            Directory.CreateDirectory(pathSave + "/1028/");
                        }

                        string fileName = Path.GetFileNameWithoutExtension(Pic.FileName);
                        string extension = Path.GetExtension(Pic.FileName);
                        fileName = fileName + DateTime.Now.ToString("yyyy-MM-dd-hh-mm-ss") + extension;
                        var path = Path.Combine(Directory.GetCurrentDirectory(), pathSave, fileName);
                        using (var stream = new FileStream(path, FileMode.Create))
                        {
                            await Pic.CopyToAsync(stream);
                        }
                        ImageResizeHelper.Image_resize(pathSave + fileName, pathSave + "/32/" + fileName, 32);
                        ImageResizeHelper.Image_resize(pathSave + fileName, pathSave + "/64/" + fileName, 64);
                        ImageResizeHelper.Image_resize(pathSave + fileName, pathSave + "/128/" + fileName, 128);
                        ImageResizeHelper.Image_resize(pathSave + fileName, pathSave + "/256/" + fileName, 256);
                        ImageResizeHelper.Image_resize(pathSave + fileName, pathSave + "/512/" + fileName, 512);
                        ImageResizeHelper.Image_resize(pathSave + fileName, pathSave + "/1028/" + fileName, 1028);

                        users.Pic = fileName;

                    }

                    ViewData["TitleOfUserId"] = new SelectList(_context.TitleOfUsers, "TitleOfUserId", "TitleOfUserName", users.TitleOfUserId);
                    ViewData["LevelId"] = new SelectList(_context.Levels, "LevelId", "LevelName", users.LevelId);
                    ViewData["BranchId"] = new SelectList(_context.Branchs, "BranchId", "BranchName", users.BranchId);
                    ViewData["StatusUserId"] = new SelectList(_context.statusUsers, "StatusUserId", "StatusUserName", users.StatusUserId);
                    ViewData["TypeOfEmployeeId"] = new SelectList(_context.typeOfEmployee, "TypeOfEmployeeId", "TypeOfEmployeeName", users.TypeOfEmployeeId);
                    ViewData["ReligionId"] = new SelectList(_context.religions, "ReligionId", "ReligionName", users.ReligionId);
                    ViewData["NationalityId"] = new SelectList(_context.nationalities, "NationalityId", "NationalityName", users.NationalityId);
                    ViewData["BloodId"] = new SelectList(_context.Bloods, "BloodId", "BloodName", users.BloodId);
                    ViewData["PovinceData"] = new SelectList(_context.povinces, "PovinceId", "PovinceName", users.povince);
                    ViewData["TypeCongrateId"] = new SelectList(_context.TypeCongrates, "TypeCongrateId", "TypeCongrateName", users.TypeCongrateId);
                    users.BirthName = Convert.ToDateTime(date1);
                    try
                    {
                        users.ResignationDate = Convert.ToDateTime(EndWorkDate);
                    }
                    catch //กรณีเป็นค่าว่าง
                    {

                    }

                    _context.Update(users);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!UserExists(users.UserId))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }

                typeofuser = HttpContext.Session.GetString("TypeOfUserId");

               permisionname=HttpContext.Session.GetString("PermisionName");
                if (typeofuser == "3" || permisionname == "HR")
                {
                    Alert("แก้ไขข้อมูลสำเร็จ", NotificationType.success);
                    return RedirectToAction(nameof(Index));
                }
                else
                {
                    Alert("แก้ไขข้อมูลสำเร็จ", NotificationType.success);
                    return RedirectToAction("Index", "Home");
                }


                //return RedirectToAction("Index", "Home");
               //return RedirectToAction(nameof(Index));
            }


            return View(users);
        }


        // GET: Users/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            // CheckSession
            if (string.IsNullOrEmpty(HttpContext.Session.GetString("Username")))
            {
                Alert("คุณไม่มีสิทธิ์ใช้งานหน้าดังกล่าว", NotificationType.error);
                return RedirectToAction("Index", "Home");
            }

            if (HttpContext.Session.GetString("TypeOfUserId") != "3")
            {
                if (HttpContext.Session.GetInt32("Userid") != id)
                {
                    Alert("เกิดข้อผิดพลาด", NotificationType.error);
                    return RedirectToAction("Index", "Home");
                }
            }
            if (id == null)
            {
                return NotFound();
            }

            var user = await _context.Users
                        .Include(p => p.Organizs.Positions)
                        .Include(p => p.Organizs.Department1s)
                        .Include(p => p.Organizs.Departments)
                        .Include(p => p.TitleOfUsers)
                        .Include(p => p.StatusUser)
                        .Include(p => p.Branchs)
                        .Include(p => p.Bloods)
                        .Include(p => p.nationality)
                        .Include(p => p.povince)
                        .Include(p => p.TypeCongrates)
                        .Include(p => p.religions)
                        .Include(p => p.typeOfEmployee)
                        .Include(p => p.Levels)
                .FirstOrDefaultAsync(m => m.UserId == id);
            if (user == null)
            {
                return NotFound();
            }


            return View(user);
        }

        //// POST: Users/Delete/5
        //[HttpPost, ActionName("Delete")]
        //[ValidateAntiForgeryToken]
        //public async Task<IActionResult> DeleteConfirmed(int id)
        //{

        //    // CheckSession
        //    if (string.IsNullOrEmpty(HttpContext.Session.GetString("Username")))
        //    {
        //        Alert("คุณไม่มีสิทธิ์ใช้งานหน้าดังกล่าว", NotificationType.error);
        //        return RedirectToAction("Index", "Home");
        //    }

        //    if (HttpContext.Session.GetString("TypeOfUserId") != "3")
        //    {
        //        if (HttpContext.Session.GetInt32("Userid") != id)
        //        {
        //            Alert("เกิดข้อผิดพลาด", NotificationType.error);
        //            return RedirectToAction("Index", "Home");
        //        }
        //    }

        //    var user = await _context.Users.FindAsync(id);
        //    _context.Users.Remove(user);
        //    await _context.SaveChangesAsync();
        //    return RedirectToAction(nameof(Index));
        //}

        // POST: Users/Delete/5
        [HttpPost, ActionName("remove")]

        public async Task<IActionResult> remove(int id)
        {

            // CheckSession
            if (string.IsNullOrEmpty(HttpContext.Session.GetString("Username")))
            {
                Alert("คุณไม่มีสิทธิ์ใช้งานหน้าดังกล่าว", NotificationType.error);
                return RedirectToAction("Index", "Home");
            }

            if (HttpContext.Session.GetString("TypeOfUserId") != "3")
            {
                if (HttpContext.Session.GetInt32("Userid") != id)
                {
                    Alert("เกิดข้อผิดพลาด", NotificationType.error);
                    return RedirectToAction("Index", "Home");
                }
            }

            IActionResult response = Unauthorized();

            var user = await _context.Users.FindAsync(id);
            var name = user.Firstname;

            _context.Users.Remove(user);
            await _context.SaveChangesAsync();

            response = Ok(new { name = name });
            return response;
        }



        private bool UserExists(int id)
        {
            return _context.Users.Any(e => e.UserId == id);
        }



        // GET: Users
        public async Task<IActionResult> Employee()
        {


            /*Check Session */
            var page = "52";
            var typeofuser = "";
            var PermisionAction = "";
            // CheckSession
            if (string.IsNullOrEmpty(HttpContext.Session.GetString("Username")))
            {
                return NotFound();
            }
            else
            {
                typeofuser = HttpContext.Session.GetString("TypeOfUserId");
                PermisionAction = HttpContext.Session.GetString("PermisionAction");
                if (PermisionHelper.CheckPermision(typeofuser, PermisionAction, page) == false)
                {
                    return NotFound();
                }
            }

            ViewData["BranchId"] = new SelectList(_context.Branchs, "BranchId", "BranchName");
            /*Check Session */

            return View(await _context.Users
                        .Include(p => p.Organizs.Positions)
                        .Include(p => p.Organizs.Department1s)
                        .Include(p => p.Organizs.Departments)
                        .Include(p => p.TitleOfUsers)
                        .Include(p => p.StatusUser)
                        .Include(p => p.Branchs)
                        .Include(p => p.Bloods)
                        .Include(p => p.nationality)
                        .Include(p => p.povince)
                        .Include(p => p.TypeCongrates)
                        .Include(p => p.religions)
                        .Include(p => p.typeOfEmployee)
                        .Include(p => p.Levels)
                        .ToListAsync());
  
        }


        public IActionResult Search()
        {
            /*Check Session */
            var page = "52";
            var typeofuser = "";
            var PermisionAction = "";
            // CheckSession
            if (string.IsNullOrEmpty(HttpContext.Session.GetString("Username")))
            {
                return NotFound();
            }
            else
            {
                typeofuser = HttpContext.Session.GetString("TypeOfUserId");
                PermisionAction = HttpContext.Session.GetString("PermisionAction");
                if (PermisionHelper.CheckPermision(typeofuser, PermisionAction, page) == false)
                {
                    return NotFound();
                }
            }

            /*Check Session */
            char[] charsToTrim = { '*', ' ', '\'' };

            string pathImage = "/images/Users/512/";
            string pathSave = pathImage;
            List<User> users;
            try
            {

                
                string term = HttpContext.Request.Query["term"].ToString().Trim(charsToTrim);
                string department1 = HttpContext.Request.Query["department1"].ToString().Trim(charsToTrim);
                string department = HttpContext.Request.Query["department"].ToString().Trim(charsToTrim);
                string position = HttpContext.Request.Query["position"].ToString().Trim(charsToTrim);
                string branch = HttpContext.Request.Query["locationcode"].ToString().Trim(charsToTrim);


                Alert(term, NotificationType.error);
                //if (term == "")
                //{
                //     users = _context.Users
                //        .Include(p => p.Organizs.Positions)
                //        .Include(p => p.Organizs.Department1s)
                //        .Include(p => p.Organizs.Departments)
                //        .Include(p => p.TitleOfUsers)
                //        .Include(p => p.StatusUser)
                //        .Include(p => p.Branchs)
                //        .Include(p => p.Levels)
                //        .ToList();
                //}
                //else
                //{

                var a = "term : " + term + "department1 : " + department1 + "department : " + department + " position : " + position + " branch :" + branch;
                users = _context.Users
                        .Include(p => p.Organizs.Positions)
                        .Include(p => p.Organizs.Department1s)
                        .Include(p => p.Organizs.Departments)
                        .Include(p => p.TitleOfUsers)
                        .Include(p => p.StatusUser)
                        .Include(p => p.Branchs)
                        .Include(p => p.Bloods)
                        .Include(p => p.nationality)
                        .Include(p => p.povince)
                        .Include(p => p.TypeCongrates)
                        .Include(p => p.religions)
                        .Include(p => p.typeOfEmployee)
                        .Include(p => p.Levels)
                       .ToList();

                if (term != "")
                {
                    users = users.Where(p =>
                         p.Firstname.Contains(term) ||
                         p.Lastname.Contains(term) ||
                         p.Nickname.Contains(term)).ToList();

                }
                if(department!="")
                {
                    users = users.Where(p =>
                         p.Organizs.Departments.DepartmentName.Contains(department)).ToList();

                }
                if (department1 != "")
                {
                    users = users.Where(p =>
                          p.Organizs.Department1s.Department1Name.Contains(department1)).ToList();
                }
                if (position != "")
                {
                    users = users.Where(p =>
                        p.Organizs.Positions.PositionName.Contains(position)).ToList();
                }
                if (branch != "")
                {
                    users = users.Where(p =>
                        p.Branchs.BranchName.Contains(branch)).ToList();
                }


                //users = _context.Users
                //        .Include(p => p.Organizs.Positions)
                //        .Include(p => p.Organizs.Department1s)
                //        .Include(p => p.Organizs.Departments)
                //        .Include(p => p.TitleOfUsers)
                //        .Include(p => p.StatusUser)
                //        .Include(p => p.Branchs)
                //        .Include(p => p.Levels)

                //        .Where(p => 
                //        p.Firstname.Contains(term) || 
                //        p.Lastname.Contains(term) || 
                //        p.Nickname.Contains(term) ||
                //        p.Organizs.Department1s.Department1Name.Contains(department1) ||
                //        p.Organizs.Departments.DepartmentName.Contains(department)||
                //        p.Organizs.Positions.PositionName.Contains(position) ||
                //        p.Branchs.BranchName.Contains(branch) 


                //        ).ToList();
                //}

                int i = 0;

                string obj = "";
                foreach (var user in users as IList<User>) //เช็คค่าตาม Location
                {
                    obj += "<div class='col-lg-3'><div class='contact-box center-version'><a href='/users/Details/" + user.UserId+"'>";
                    obj += "<img alt='image' class='img-circle'  src='../" + pathSave + user.Pic +"'/>";
                    obj += "<h3 class='m-b-xs'><strong>"+ user.TitleOfUsers.TitleOfUserName + user.Firstname + " " + user.Lastname + "</strong></h3>";
                    obj += "<div class='font-bold'> ฝ่าย " + user.Organizs.Departments.DepartmentName + " </div>";
                    obj += "<div class='font-bold'> แผนก " + user.Organizs.Department1s.Department1Name + " </div>";
                    obj += "<div class='font-bold'> ตำแหน่ง " + user.Organizs.Positions.PositionName + " </div>";

                    obj += "<address class='m-t-md'><strong>สาขา : "+user.Branchs.BranchName+"</strong></address>";

                    obj +="<i class='glyphicon glyphicon-envelope'></i> Email : "+ user.EmailContact +"<br/>";
                    obj +="<i class='glyphicon glyphicon-phone-alt'></i> เบอร์ภายใน : " + user.ExtTel+"<br/>";
                    obj +="</div></div>";
                    i = i + 1;
                }
                return Ok(new { table3 = obj ,sumdata=i});
            }
            catch
            {
                return BadRequest();
            }
        }

        // GET: Users/AddUsername/5
        public async Task<IActionResult> AddUsername(int? id)
        {
         

            if (id == null)
            {
                return NotFound();
            }

            var user = await _context.Users
                        .Include(p => p.Organizs.Positions)
                        .Include(p => p.Organizs.Department1s)
                        .Include(p => p.Organizs.Departments)
                        .Include(p => p.TitleOfUsers)
                        .Include(p => p.StatusUser)
                        .Include(p => p.Branchs)
                        .Include(p => p.Bloods)
                        .Include(p => p.nationality)
                        .Include(p => p.povince)
                        .Include(p => p.TypeCongrates)
                        .Include(p => p.religions)
                        .Include(p => p.typeOfEmployee)
                        .Include(p => p.Levels)
                .FirstOrDefaultAsync(m => m.UserId == id);
            if (user == null)
            {
                return NotFound();
            }


            var login =await _context.Logins.FirstOrDefaultAsync(m => m.UserId == id);
            if (login == null)
            {

                ViewData["CheckUserId"] = new SelectList(_context.CheckUsers, "CheckUserId", "CheckUserName");
                ViewData["PermisionId"] = new SelectList(_context.Permisions, "PermisionId", "PermisionName");
                ViewData["TypeOfUserId"] = new SelectList(_context.TypeOfUsers, "TypeOfUserId", "TypeOfUserName");
                return View(user);
            }
            else
            {

                ViewData["CheckUserId"] = new SelectList(_context.CheckUsers, "CheckUserId", "CheckUserName",login.CheckUserId);
                ViewData["PermisionId"] = new SelectList(_context.Permisions, "PermisionId", "PermisionName", login.PermisionId);
                ViewData["TypeOfUserId"] = new SelectList(_context.TypeOfUsers, "TypeOfUserId", "TypeOfUserName", login.TypeOfUserId);

                return RedirectToAction("EditUsername", "Users", new { id = id });

            }

        }


        // GET: Users/EditUsername/5
        public async Task<IActionResult> EditUsername(int? id)
        {
            /*Check Session */

            var typeofuser = "";
            var PermisionAction = "";
     
                typeofuser = HttpContext.Session.GetString("TypeOfUserId");
                PermisionAction = HttpContext.Session.GetString("PermisionAction");


            var permisionname = HttpContext.Session.GetString("PermisionName");
            if (typeofuser == "3" || permisionname == "HR" || HttpContext.Session.GetInt32("Userid") == id)
            {

            }
            else
            {
                Alert("คุณไม่มีสิทธิ์ใช้งานหน้าดังกล่าว", NotificationType.error);
                return RedirectToAction("Index", "Home");
            }




            if (id == null)
                {
                    return NotFound();
                }

                var user = await _context.Users
                        .Include(p => p.Organizs.Positions)
                        .Include(p => p.Organizs.Department1s)
                        .Include(p => p.Organizs.Departments)
                        .Include(p => p.TitleOfUsers)
                        .Include(p => p.StatusUser)
                        .Include(p => p.Branchs)
                        .Include(p => p.Bloods)
                        .Include(p => p.nationality)
                        .Include(p => p.povince)
                        .Include(p => p.TypeCongrates)
                        .Include(p => p.religions)
                        .Include(p => p.typeOfEmployee)
                        .Include(p => p.Levels)
                    .FirstOrDefaultAsync(m => m.UserId == id);
                if (user == null)
                {
                    return NotFound();
                }


                var login = await _context.Logins
                         .Include(m => m.Permisions)
                         .Include(m => m.CheckUsers)
                         .Include(m => m.TypeOfUsers)
                         .FirstOrDefaultAsync(m => m.UserId == id);

            ViewData["CheckUserId"] = new SelectList(_context.CheckUsers, "CheckUserId", "CheckUserName", login.CheckUserId);
            ViewData["PermisionId"] = new SelectList(_context.Permisions, "PermisionId", "PermisionName", login.PermisionId);
            ViewData["TypeOfUserId"] = new SelectList(_context.TypeOfUsers, "TypeOfUserId", "TypeOfUserName", login.TypeOfUserId);

            ViewBag.Id = login.ID;
            ViewBag.Username = login.Username;
            ViewBag.Password = login.Password;

            //ViewBag.PermisionId = login.PermisionId;
            //ViewBag.CheckUserId = login.CheckUserId;
            //ViewBag.TypeOfUserId = login.TypeOfUserId;

            ViewBag.PermistionName = login.Permisions.PermisionName;
            ViewBag.CheckUserName = login.CheckUsers.CheckUserName;
            ViewBag.TypeOfUserName = login.TypeOfUsers.TypeOfUserName;

            return View(user);

        }



        // POST: Users/AddUsername
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AddUsername(string Username, string Password, string Repassword, int PermisionId, int TypeOfUserId, int CheckUserId,  int UserId)
        {



            if (Password != Repassword)
            {
                Alert("Password ไม่เหมือนกัน", NotificationType.error);
                return RedirectToAction("AddUsername", "Users", new { id = UserId });

            }


            var user = await _context.Users
                        .Include(p => p.Organizs.Positions)
                        .Include(p => p.Organizs.Department1s)
                        .Include(p => p.Organizs.Departments)
                        .Include(p => p.TitleOfUsers)
                        .Include(p => p.StatusUser)
                        .Include(p => p.Branchs)
                        .Include(p => p.Bloods)
                        .Include(p => p.nationality)
                        .Include(p => p.povince)
                        .Include(p => p.TypeCongrates)
                        .Include(p => p.religions)
                        .Include(p => p.typeOfEmployee)
                        .Include(p => p.Levels)
                .FirstOrDefaultAsync(m => m.UserId == UserId);
            if (user == null)
            {
                return NotFound();
            }

            var qry = await _context.Logins.FirstOrDefaultAsync(m => m.UserId == UserId);


            Login login = new Login();
            login.Username = Username;
            string hashed_password = EncryptionHelper.Encrypt(Password);
            login.Password = hashed_password;
            login.CheckUserId = CheckUserId;
            login.PermisionId = PermisionId;
            login.TypeOfUserId = TypeOfUserId;
            login.UserId = UserId;
            login.CreateDate = DateTime.Now;



     

            if (qry== null)
            {
                _context.Logins.Add(login);
                await _context.SaveChangesAsync();
                Alert("สร้าง Username Password เรียบร้อย", NotificationType.success);
                return RedirectToAction("Index", "Users");
            }
            else
            {
                Alert("Username ซ้ำ", NotificationType.error);
                return RedirectToAction("AddUsername", "Users", new { id = UserId });
            }
        }





        // POST: Users/AddUsername
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditUsername(int id,string Username, int PermisionId, int TypeOfUserId, int CheckUserId, int UserId,string passwordold,string passwordnew,string passwordnew1)
        {
            var typeofuser = "";
            var PermisionAction = "";

            typeofuser = HttpContext.Session.GetString("TypeOfUserId");
            PermisionAction = HttpContext.Session.GetString("PermisionAction");
            var permisionname = HttpContext.Session.GetString("PermisionName");


            var checkregis = await _context.Logins.FirstOrDefaultAsync(m => m.ID == id);

            string hashed_password = "";
            if (passwordold != null)
            {

                string hashed_passwordOld = EncryptionHelper.Encrypt(passwordold);
                if(passwordnew != null){
                    if (checkregis.Password!=hashed_passwordOld)
                    {
                        Alert("Password เดิมไม่ถูกต้อง", NotificationType.error);
                        return RedirectToAction("EditUsername", "Users", new { id = UserId });
                    }
                    if (passwordnew != passwordnew1)
                    {
                        Alert("Password ใหม่ไม่เหมือนกัน", NotificationType.error);
                        return RedirectToAction("EditUsername", "Users", new { id = UserId });
                    }
                    hashed_password = EncryptionHelper.Encrypt(passwordnew);
                }
                else
                {
                    Alert("Password ใหม่ห้ามเป็นค่าว่าง", NotificationType.error);
                    return RedirectToAction("EditUsername", "Users", new { id = UserId });
                }
            }
            else
            {

                hashed_password = checkregis.Password;
            }


            var user =await _context.Users
                        .Include(p => p.Organizs.Positions)
                        .Include(p => p.Organizs.Department1s)
                        .Include(p => p.Organizs.Departments)
                        .Include(p => p.TitleOfUsers)
                        .Include(p => p.StatusUser)
                        .Include(p => p.Branchs)
                        .Include(p => p.Bloods)
                        .Include(p => p.nationality)
                        .Include(p => p.povince)
                        .Include(p => p.TypeCongrates)
                        .Include(p => p.religions)
                        .Include(p => p.typeOfEmployee)
                        .Include(p => p.Levels)
                .FirstOrDefaultAsync(m => m.UserId == UserId);

            if (user == null)
            {
                return NotFound();
            }


            //Login login = new Login();
            //login.ID = checkregis.ID;
            //login.Username = Username;
            checkregis.Password = hashed_password;
            checkregis.CheckUserId = CheckUserId;
            checkregis.PermisionId = PermisionId;
            checkregis.TypeOfUserId = TypeOfUserId;
            //login.UserId = UserId;
            //login.CreateDate =checkregis.CreateDate;



            _context.Logins.Update(checkregis);
            await _context.SaveChangesAsync();


            if (typeofuser == "3" || permisionname == "HR")
            {
                Alert("แก้ไขข้อมูลเรียบร้อย", NotificationType.success);
                return RedirectToAction("Index", "Users");
            }
            else
            {
                Alert("แก้ไขข้อมูลสำเร็จ", NotificationType.success);
                return RedirectToAction("Index", "Home");
            }


         

        }

        public IActionResult GetData()
        {


            IActionResult response = Unauthorized();
            var TotalEmployee = _context.Users.Where(p => p.StatusUserId == 1).Count();
            var TotalMale = _context.Users.Where(p => p.StatusUserId == 1 && p.TitleOfUserId == 1).Count();
            var TotalFemale = _context.Users.Where(p => p.StatusUserId == 1 && (p.TitleOfUserId == 2 || p.TitleOfUserId ==3)).Count();

            var Monly = _context.Users.Where(p => p.StatusUserId == 1 && p.TypeOfEmployeeId==1).Count();
            var Dayly = _context.Users.Where(p => p.StatusUserId == 1 && p.TypeOfEmployeeId == 2).Count();

            var Ho = _context.Users.Where(p => p.StatusUserId == 1 && p.BranchId == 3).Count();
            var Store = _context.Users.Where(p => p.StatusUserId == 1 && p.BranchId == 10).Count();
            var Sites = _context.Users.Where(p => p.StatusUserId == 1 && (p.BranchId != 10 && p.BranchId != 3)).Count();

            decimal percenMale = Percen(TotalMale, TotalEmployee);
            decimal percenFemale = Percen(TotalFemale, TotalEmployee);
            decimal percenMonly = Percen(Monly, TotalEmployee);
            decimal percenDayly = Percen(Dayly, TotalEmployee);
            decimal percenHo = Percen(Ho, TotalEmployee);
            decimal percenStores = Percen(Store, TotalEmployee);
            decimal percenSites = Percen(Sites, TotalEmployee);


            //Generation
            var queryGen = "SELECT 'Baby Boomer (2489-2507)' As Head,'มีอายุระหว่าง 57-73 ปี' As Etc,COUNT(*) AS CountGen,'' AS Progressbar " +
                " FROM dbo.Users " +
                " WHERE YEAR ( dbo.Users.BirthName ) +543 >= 2489 AND YEAR ( dbo.Users.BirthName ) +543 <= 2507 AND dbo.Users.StatusUserId = 1  " +
                " UNION SELECT 'Gen X (2508-2522)' As Head,'มีอายุระหว่าง 40-50 ปี' As Etc,COUNT(*) AS  CountGen ,'' AS Progressbar " +
                " FROM dbo.Users " +
                " WHERE YEAR ( dbo.Users.BirthName ) +543 >= 2508 AND YEAR ( dbo.Users.BirthName ) +543 <= 2522 AND dbo.Users.StatusUserId = 1 " +
                " UNION SELECT 'Gen Y (2523-2540)' As Head,'มีอายุระหว่าง 22-39 ปี' As Etc,COUNT(*) AS  CountGen, '' AS Progressbar " +
                " FROM dbo.Users " +
                " WHERE YEAR ( dbo.Users.BirthName ) +543 >= 2523 AND YEAR ( dbo.Users.BirthName ) +543 <= 2540 AND dbo.Users.StatusUserId = 1 " +
                " UNION SELECT 'Gen Z (2540 +)' As Head,'มีอายุน้อยกว่า 22 ปี' As Etc,COUNT(*) AS  CountGen, '' AS Progressbar " +
                " FROM dbo.Users " +
                " WHERE YEAR ( dbo.Users.BirthName ) +543 > 2540 AND dbo.Users.StatusUserId = 1 ";

            var reportGens = _context.ReportGens.FromSqlRaw(queryGen).ToList();
            var totaleGens = reportGens.Sum(p => p.CountGen);
            var header = "<table>" +
                " <thead> " +
                "<tr> " +
                "<th width='300px'> " +
                "</th> " +
                "<th width='40px' style='text-align:center'>" +
                "<sapn>คน</sapn>" +
                "</th>" +
                "<th style='text-align:right' width='400px'>" +
                "<sapn>%</sapn>" +
                "</th>" +
                "</tr>" +
                "</thead>" +
                "<tbody>";

            var footer = "</tbody>" +
                         "</table>";
            var body = "";
            decimal percent = 0;
            var countgen = 0;
            var totalemp = 0;
            foreach (var std in reportGens as IList<ReportGen>)
            {
                countgen = std.CountGen;
                totalemp = totaleGens;       
                

                percent = Percen(countgen,totalemp);


                body += " <tr>";
                body += " <td valign='top'>";
                body += " <div>"+std.Head+"</div>" +
                    "<small>"+std.Etc+"</small>";
                body += " </td>";
                body += " <td valign='top' style='text-align:right;padding-right:6px;'>";
                body += " <span>" + std.CountGen + "</span>";
                body += " </td>";
                body += " <td style='text-align:right;' valign='top'>";
                body += " <div class='progress'>"; 
                body += "<div class='progress-bar' role='progress-bar' aria-valuenow='40' style='width:" + percent+ "%' id='bg-progressGen'><span style='color:#4d4f53;font-weight: bold; font-size:10px;'>" + percent+"%</span>";
                body += "</div>";
                body += " </td>";
                body += " </tr>";

            }
            var tableGen = header+body+footer;

            //Age ช่วงอายุ
            var queryAge = "SELECT '21 - 25' AS Range,COUNT(*) AS countAge,'' AS progressbarAge " +
                " FROM dbo.Users " +
                " WHERE YEAR(getdate())- YEAR(dbo.Users.BirthName) >=21 AND YEAR(getdate())- YEAR(dbo.Users.BirthName) <=25 AND dbo.Users.StatusUserId = 1 " +
                " UNION SELECT '26 - 30' AS Range,COUNT(*) AS CountAge,'' AS progressbarAge " +
                " FROM dbo.Users " +
                " WHERE YEAR(getdate())- YEAR(dbo.Users.BirthName) >=26 AND YEAR(getdate())- YEAR(dbo.Users.BirthName) <=30 AND dbo.Users.StatusUserId = 1 " +
                " UNION SELECT '31 - 35' AS Range,COUNT(*) AS CountAge,'' AS progressbar " +
                " FROM dbo.Users " +
                " WHERE YEAR(getdate())- YEAR(dbo.Users.BirthName) >=31 AND YEAR(getdate())- YEAR(dbo.Users.BirthName) <=35 AND dbo.Users.StatusUserId = 1 " +
                " UNION SELECT '36 - 40' AS Range,COUNT(*) AS CountAge,'' AS progressbarAge " +
                " FROM dbo.Users " +
                " WHERE YEAR(getdate())- YEAR(dbo.Users.BirthName) >=36 AND YEAR(getdate())- YEAR(dbo.Users.BirthName) <=40 AND dbo.Users.StatusUserId = 1 " +
                " UNION SELECT '41 - 45' AS Range,COUNT(*) AS CountAge,'' AS progressbarAge " +
                " FROM dbo.Users " +
                " WHERE YEAR(getdate())- YEAR(dbo.Users.BirthName) >=41 AND YEAR(getdate())- YEAR(dbo.Users.BirthName) <=45 AND dbo.Users.StatusUserId = 1 " +
                " UNION SELECT '46 - 50' AS Range,COUNT(*) AS CountAge,'' AS progressbarAge " +
                " FROM dbo.Users " +
                " WHERE YEAR(getdate())- YEAR(dbo.Users.BirthName) >=46 AND YEAR(getdate())- YEAR(dbo.Users.BirthName) <=50 AND dbo.Users.StatusUserId = 1 " +
                " UNION SELECT '51 - 55' AS Range,COUNT(*) AS CountAge,'' AS progressbarAge " +
                " FROM dbo.Users " +
                " WHERE YEAR(getdate())- YEAR(dbo.Users.BirthName) >=51 AND YEAR(getdate())- YEAR(dbo.Users.BirthName) <=55 AND dbo.Users.StatusUserId = 1 " +
                " UNION SELECT '56 - 60' AS Range,COUNT(*) AS CountAge,'' AS progressbarAge " +
                " FROM dbo.Users " +
                " WHERE YEAR(getdate())- YEAR(dbo.Users.BirthName) >=56 AND YEAR(getdate())- YEAR(dbo.Users.BirthName) <=60 AND dbo.Users.StatusUserId = 1 " +
                " UNION SELECT '60 ปีขึ้นไป' AS Range,COUNT(*) AS CountAge,'' AS progressbarAge " +
                " FROM dbo.Users " +
                " WHERE YEAR(getdate())- YEAR(dbo.Users.BirthName) >60 ";

            var reportAge = _context.ReportAges.FromSqlRaw(queryAge).ToList();
            var totaleAge = reportAge.Sum(p => p.CountAge);

            var headerAge = "<table>" +
                " <thead> " +
                "<tr> " +
                "<th width='100px'> " +
                "</th> " +
                "<th width='20px' style='text-align:center'>" +
                "<sapn>คน</sapn>" +
                "</th>" +
                "<th style='text-align:right' width='400px'>" +
                "<sapn>%</sapn>" +
                "</th>" +
                "</tr>" +
                "</thead>" +
                "<tbody>";
            var footerAge = "</tbody>" +
                            "</table>";
            var bodyAge = "";
            decimal percentAge = 0;
            var countAge = 0;
            var totalempAge = 0;


            foreach (var std in reportAge as IList<ReportAge>)
            {
                countAge = std.CountAge;
                totalempAge = totaleAge;


                percentAge = Percen(countAge, totalempAge);


                bodyAge += " <tr>";
                bodyAge += " <td valign='top'>";
                bodyAge += " <span>" + std.Range + "</span>";
                bodyAge += " </td>";
                bodyAge += " <td style='text-align:right;padding-right:10px;' valign='top'>";
                bodyAge += " <span>" + std.CountAge + "</span>";
                bodyAge += " </td>";
                bodyAge += " <td style='text-align:right' >";
                bodyAge += " <div class='progress'>";
                bodyAge += "<div class='progress-bar progress-bar-warning' role='progress-bar' aria-valuenow='40' style='width:" + percentAge + "%' id='bg-progressAge'><span style='color:#4d4f53;font-weight: bold;font-size:10px;'>" + percentAge + "%</span>";
                bodyAge += "</div>";
                bodyAge += " </td>";
                bodyAge += " </tr>";

            }

            var tableAge = headerAge + bodyAge + footerAge;

            //Position PC Level
            var queryLevels= "SELECT 'PC-1' AS HeadLevel,Count(*) AS CountLevels,'' AS ProgressbarLevel" +
                " FROM dbo.Users " +
                " WHERE dbo.Users.StatusUserId = 1 AND dbo.Users.LevelId = 1 " +
                " UNION SELECT 'PC-2' AS HeadLevel,Count(*) AS CountLevels,'' AS ProgressbarLevel " +
                " FROM dbo.Users " +
                " WHERE dbo.Users.StatusUserId = 1 AND dbo.Users.LevelId = 2 " +
                " UNION SELECT 'PC-3' AS HeadLevel,Count(*) AS CountLevels,'' AS ProgressbarLevel" +
                " FROM dbo.Users " +
                " WHERE dbo.Users.StatusUserId = 1 AND dbo.Users.LevelId = 3 " +
                " UNION SELECT 'PC-4' AS HeadLevel,Count(*) AS CountLevels,'' AS ProgressbarLevel " +
                " FROM dbo.Users " +
                " WHERE dbo.Users.StatusUserId = 1 AND dbo.Users.LevelId = 4 " +
                " UNION SELECT 'PC-5' AS HeadLevel,Count(*) AS CountLevels,'' AS ProgressbarLevel " +
                " FROM dbo.Users " +
                " WHERE dbo.Users.StatusUserId = 1 AND dbo.Users.LevelId = 5 " +
                " UNION SELECT 'PC-6' AS HeadLevel, Count(*) AS CountLevels,'' AS ProgressbarLevel " +
                " FROM dbo.Users " +
                " WHERE dbo.Users.StatusUserId = 1 AND dbo.Users.LevelId = 6 " +
                " UNION SELECT 'PC-7' AS HeadLevel,Count(*) AS CountLevels,'' AS ProgressbarLevel" +
                " FROM dbo.Users " +
                " WHERE dbo.Users.StatusUserId = 1 AND dbo.Users.LevelId = 7 " +
                " UNION SELECT 'PC-8' AS HeadLevel,Count(*) AS CountLevels,'' AS ProgressbarLevel " +
                " FROM dbo.Users " +
                " WHERE dbo.Users.StatusUserId = 1 AND dbo.Users.LevelId = 8 " +
                " UNION SELECT 'PC-9' AS HeadLevel,Count(*) AS CountLevels,'' AS ProgressbarLevel " +
                " FROM dbo.Users " +
                " WHERE dbo.Users.StatusUserId = 1 AND dbo.Users.LevelId = 9 " +
                " UNION SELECT 'Un' AS HeadLevel,Count(*) AS CountLevelsม,'' AS ProgressbarLevel " +
                " FROM dbo.Users " +
                " WHERE dbo.Users.StatusUserId = 1 AND dbo.Users.LevelId = 10";

            var reportLevel = _context.ReportLevels.FromSqlRaw(queryLevels).ToList();
            var totaleLevel = reportLevel.Sum(p => p.CountLevels);

            var headerLevel = "<table>" +
                " <thead> " +
                "<tr> " +
                "<th width='100px'> " +
                "</th> " +
                "<th width='20px' style='text-align:center'>" +
                "<sapn>คน</sapn>" +
                "</th>" +
                "<th style='text-align:right' width='400px'>" +
                "<sapn>%</sapn>" +
                "</th>" +
                "</tr>" +
                "</thead>" +
                "<tbody>";
            var footerLevel = "</tbody>" +
                            "</table>";
            var bodyLevel = "";
            decimal percentLevel = 0;
            var countlevel = 0;
            var totalLavelemp = 0;

            foreach (var std in reportLevel as IList<ReportLevel>)
            {
                countlevel = std.CountLevels;
                totalLavelemp = totaleGens;
                percentLevel = Percen(countlevel, totalLavelemp);

                bodyLevel += " <tr valign='top'>";
                bodyLevel += " <td>";
                bodyLevel += " <span>"+ std.HeadLevel +"</span>";
                bodyLevel += " </td>";
                bodyLevel += " <td style='text-align:right;padding-right:10px;'>";
                bodyLevel += " <span>" + std.CountLevels + "</span>";
                bodyLevel += " </td>";
                bodyLevel += " <td>";
                bodyLevel += " <div class='progress'>";
                bodyLevel += "<div class='progress-bar progress-bar-warning' role='progress-bar' aria-valuenow='40' style='width:" + percentLevel + "%' id='bg-progressPC'><span style='color:#4d4f53;font-weight: bold;font-size:10px;'>" + percentLevel + "%</span>";
                bodyLevel += "</div>";
                bodyLevel += " </td>";
                bodyLevel += " </tr>";

            }

            var tableLevel = headerLevel + bodyLevel + footerLevel;

                       
            //Management to staff Ratio
            var queryManagement = " SELECT 'Employee count (PC1-6)' AS LevelHead,COUNT(*) AS CountMnagement,'' AS ProgressbarManagement " +
                " FROM dbo.Users " +
                " WHERE dbo.Users.LevelId >=1 AND  dbo.Users.LevelId <=6 AND dbo.Users.StatusUserId = 1  " +
                " UNION SELECT 'Manager count (PC7 +)' AS LevelHead,COUNT(*) AS CountMnagement,'' AS ProgressbarManagement" +
                " FROM dbo.Users " +
                " WHERE dbo.Users.LevelId >=7 AND dbo.Users.StatusUserId = 1 ";

            var reportMn = _context.ReportManagements.FromSqlRaw(queryManagement).ToList();
            var totaleMn = reportMn.Sum(p => p.CountMnagement);

            var headerMn = "<table>" +
                " <thead> " +
                "<tr> " +
                "<th width='300px'> " +
                "</th> " +
                "<th width='20px' style='text-align:center'>" +
                "<sapn>คน</sapn>" +
                "</th>" +
                "<th style='text-align:right' width='400px'>" +
                "<sapn>%</sapn>" +
                "</th>" +
                "</tr>" +
                "</thead>" +
                "<tbody>";
            var footerMn = "</tbody>" +
                            "</table>";
            var bodyMn = "";
            decimal percentMn = 0;
            var countMn= 0;
            var totalempMn = 0;

            foreach (var std in reportMn as IList<ReportManagement>)
            {
                countMn = std.CountMnagement;
                totalempMn = totaleAge;

                percentMn = Percen(countMn, totalempMn);

                bodyMn += " <tr valign='top'>";
                bodyMn += " <td>";
                bodyMn += " <i class='fa fa-user-secret'></i> <span>" + std.LevelHead + "</span>";
                bodyMn += " </td>";
                bodyMn += " <td style='text-align:right;padding-right:10px;'>";
                bodyMn += " <span>" + std.CountMnagement + "</span>";
                bodyMn += " </td>";
                bodyMn += " <td>";
                bodyMn += " <div class='progress'>";
                bodyMn += "<div class='progress-bar' role='progress-bar' aria-valuenow='40' style='width:" + percentMn + "%' id='bg-progressMn'><span style='color:#4d4f53;font-weight: bold;font-size:10px;'>" + percentMn + "%</span>";
                bodyMn += "</div>";
                bodyMn += " </td>";
                bodyMn += " </tr>";

            }

            var tableManagement = headerMn + bodyMn + footerMn;

            //HeadCount --สายงาน
            //Construction
            var queryConstruction = "SELECT 'สายงานก่อสร้าง' as HeadDepartment,COUNT(*) AS CountDepartment, " +
                " (SELECT count(*) FROM dbo.Users WHERE dbo.Users.StatusUserId =1) AS TotalEmployee" +
                " FROM dbo.Users AS u " +
                " INNER JOIN dbo.Organizs AS o ON u.organizId = o.organizId " +
                " INNER JOIN dbo.Departments AS d ON o.DepartmentId = d.DepartmentId " +
                " WHERE (d.DepartmentId=2 OR d.DepartmentId =3 OR d.DepartmentId=9) AND u.StatusUserId =1 ";

            var reportCon = _context.ReportDeparments.FromSqlRaw(queryConstruction).ToList();

            var headerCon = "<table>" +
                        " <thead> " +
                        "<tr> " +
                        "<th width='250px'> " +
                        "</th> " +
                        "<th width='100px'> " +
                        "</th> " +
                        "</tr>" +
                        "</thead>" +
                        "<tbody>";
            var footerCon = "</tbody>" +
                            "</table>";

            var bodyCon = "";
            decimal percentCon = 0;
            var countCon = 0;
            var totalempCon = 0;

            foreach (var std in reportCon as IList<ReportDeparment>)
            {
                countCon = std.CountDepartment;
                totalempCon = std.TotalEmployee;
                percentCon = Percen(countCon, totalempCon);

                bodyCon += " <tr><td><h1></h1></td><tr>";
                bodyCon += " <tr>";
                bodyCon += " <td>";
                bodyCon += " <i class='fa fa-cubes fa-2x'></i> <span class='info - box - text'>" + std.HeadDepartment + "</span>";
                bodyCon += " <p>Construction</p>";
                bodyCon += " </td>";
                bodyCon += " <td>";
                bodyCon += " <span><h3>" + std.CountDepartment + "</h3></span>";
                bodyCon += " <p>" + percentCon + "%</p>";
                bodyCon += " </td>";
                bodyCon += " </tr>";

            }

            var tableCon = headerCon + bodyCon + footerCon;

            //admin&financial
            var queryAdminFinancail = "SELECT 'สายงานบริหารและการเงิน' as HeadDepartment,COUNT(*) AS CountDepartment, " +
               " (SELECT count(*) FROM dbo.Users WHERE dbo.Users.StatusUserId =1) AS TotalEmployee" +
               " FROM dbo.Users AS u " +
               " INNER JOIN dbo.Organizs AS o ON u.organizId = o.organizId " +
               " INNER JOIN dbo.Departments AS d ON o.DepartmentId = d.DepartmentId " +
               " WHERE (d.DepartmentId=5 OR d.DepartmentId =8 OR d.DepartmentId=11 OR d.DepartmentId=12 OR d.DepartmentId=10) AND u.StatusUserId =1 ";

            var reportAdminF = _context.ReportDeparments.FromSqlRaw(queryAdminFinancail).ToList();

            var headerAF = "<table>" +
                        " <thead> " +
                        "<tr> " +
                        "<th width='250px'> " +
                        "</th> " +
                        "<th width='100px'> " +
                        "</th> " +
                        "</tr>" +
                        "</thead>" +
                        "<tbody>";
            var footerAF = "</tbody>" +
                            "</table>";

            var bodyAF = "";
            decimal percentAF = 0;
            var countAF = 0;
            var totalempAF = 0;

            foreach (var std in reportAdminF as IList<ReportDeparment>)
            {
                countAF = std.CountDepartment;
                totalempAF = std.TotalEmployee;
                percentAF = Percen(countAF, totalempAF);

                bodyAF += " <tr><td><h2></h2></td><tr>";
                bodyAF += " <tr>";
                bodyAF += " <td>";
                bodyAF += " <i class='fa fa-money fa-2x'></i> <span class='info-box-text'>" + std.HeadDepartment + "</span>";
                bodyAF += " <p>Admin & Financail</p>";
                bodyAF += " </td>";
                bodyAF += " <td colspan='2'>";
                bodyAF += " <span><h3>" + std.CountDepartment + "</h3></span>";
                bodyAF += " <p>" + percentAF + "%</p>";
                bodyAF += " </td>";
                bodyAF += " </tr>";

            }

            var tableAdminFinancail = headerAF + bodyAF + footerAF;

            //Tech&Est
            var queryTechEst = "SELECT 'สายงานเทคนิคและประเมินราคา' as HeadDepartment,COUNT(*) AS CountDepartment, " +
               " (SELECT count(*) FROM dbo.Users WHERE dbo.Users.StatusUserId =1) AS TotalEmployee" +
               " FROM dbo.Users AS u " +
               " INNER JOIN dbo.Organizs AS o ON u.organizId = o.organizId " +
               " INNER JOIN dbo.Departments AS d ON o.DepartmentId = d.DepartmentId " +
               " WHERE (d.DepartmentId=4 OR d.DepartmentId =6 OR d.DepartmentId =7 OR d.DepartmentId=13 OR d.DepartmentId=14 OR d.DepartmentId=15) AND u.StatusUserId =1 ";

            var reportTechEst = _context.ReportDeparments.FromSqlRaw(queryTechEst).ToList();

            var headerTE = "<table>" +
                        " <thead> " +
                        "<tr> " +
                        "<th width='250px'> " +
                        "</th> " +
                        "<th width='100px'> " +
                        "</th> " +
                        "</tr>" +
                        "</thead>" +
                        "<tbody>";
            var footerTE = "</tbody>" +
                            "</table>";

            var bodyTE = "";
            decimal percentTE = 0;
            var countTE = 0;
            var totalempTE = 0;

            foreach (var std in reportTechEst as IList<ReportDeparment>)
            {
                countTE = std.CountDepartment;
                totalempTE = std.TotalEmployee;
                percentTE = Percen(countTE, totalempTE);

                bodyTE += " <tr><td><h2></h2></td><tr>";
                bodyTE += " <tr>";
                bodyTE += " <td>";
                bodyTE += " <i class='fa fa-tags fa-2x'></i> <span'>" + std.HeadDepartment + "</span>";
                bodyTE += " <p>Tech & Est</p>";
                bodyTE += " </td>";
                bodyTE += " <td colspan='2'>";
                bodyTE += " <span><h3>" + std.CountDepartment + "</h3></span>";
                bodyTE += " <p>" + percentTE + "%</p>";
                bodyTE += " </td>";
                bodyTE += " </tr>";

            }

            var tableTechEst = headerTE + bodyTE + footerTE;

            //Deparment ฝ่าย
            //Construct Factory
            var queryDepCon = "SELECT 'Construction' as HeadDepartment1,COUNT(*) as CountDepartment1," +
               " (SELECT count(*) FROM dbo.Users WHERE dbo.Users.StatusUserId =1) AS TotalEmployee1 ,'' AS ProgressbarDepartment1 " +
               " FROM dbo.Users AS u " +
               " INNER JOIN dbo.Organizs AS o ON u.organizId = o.organizId " +
               " INNER JOIN dbo.Departments AS d ON o.DepartmentId = d.DepartmentId " +
               " WHERE d.DepartmentId=2 AND u.StatusUserId =1 " +
               " UNION SELECT 'Facrory +' as HeadDepartment1,COUNT(*) as CountDepartment1," +
               " (SELECT count(*) FROM dbo.Users WHERE dbo.Users.StatusUserId =1) as TotalEmployee ,'' AS ProgressbarDepartment1 " +
               " FROM dbo.Users AS u " +
               " INNER JOIN dbo.Organizs AS o ON u.organizId = o.organizId " +
               " INNER JOIN dbo.Departments AS d ON o.DepartmentId = d.DepartmentId " +
               " WHERE d.DepartmentId=9 AND u.StatusUserId =1 ";

            var reportDepCon = _context.ReportDeparment1s.FromSqlRaw(queryDepCon).ToList();

            var headerDepCon = "<table>" +
                    " <thead> " +
                    "<tr> " +
                    "<th width='180px'>" +
                    "<sapn><h2></h2></sapn>" +
                    "</th>" +
                    "<th width='50px'>" +
                    "<sapn>คน</sapn>" +
                    "</th>" +
                    "<th style='text-align:right' width='200px'>" +
                    "<sapn>%</sapn>" +
                    "</th>" +
                    "</tr>" +
                    "</thead>" +
                    "<tbody>";
            var footerDepCon = "</tbody>" +
                            "</table>";

            var bodyDepCon = "";
            decimal percentDepCon = 0;
            var countDepCon = 0;
            var totalempDepCon = 0;

            foreach (var std in reportDepCon as IList<ReportDepartment1>)
            {
                countDepCon = std.CountDepartment1;
                totalempDepCon = std.TotalEmployee1;
                percentDepCon = Percen(countDepCon, totalempDepCon);

                bodyDepCon += " <tr valign='top'>";
                bodyDepCon += " <td style='text-align:right;padding-right:10px'>";
                bodyDepCon += " <span>" + std.HeadDepartment1 + "</span>          ";
                bodyDepCon += " </td>";
                bodyDepCon += " <td>";
                bodyDepCon += " <span>" + std.CountDepartment1 + "</span>";
                bodyDepCon += " </td>";
                bodyDepCon += " <td>";
                bodyDepCon += " <div class='progress'>";
                bodyDepCon += "<div class='progress-bar' role='progress-bar' aria-valuenow='40' style='width:" + percentDepCon + "%' id='bg-progressDep'><span style='color:#4d4f53;font-weight: bold;font-size:10px;'>" + percentDepCon + "%</span>";
                bodyDepCon += "</div>";
                bodyDepCon += "</td>";
                bodyDepCon += " </tr>";
            }

            var tableDepCon = headerDepCon + bodyDepCon + footerDepCon;

            //Account HR IT
            var queryDepAF = "SELECT 'Accounting & Financial +' as HeadDepartment1,COUNT(*) as CountDepartment1," +
               " (SELECT count(*) FROM dbo.Users WHERE dbo.Users.StatusUserId =1) AS TotalEmployee1 ,'' AS ProgressbarDepartment1 " +
               " FROM dbo.Users AS u " +
               " INNER JOIN dbo.Organizs AS o ON u.organizId = o.organizId " +
               " INNER JOIN dbo.Departments AS d ON o.DepartmentId = d.DepartmentId " +
               " WHERE d.DepartmentId=11 AND u.StatusUserId =1 " +
               " UNION SELECT 'HR & Admin +' as HeadDepartment1,COUNT(*) as CountDepartment1," +
               " (SELECT count(*) FROM dbo.Users WHERE dbo.Users.StatusUserId =1) as TotalEmployee ,'' AS ProgressbarDepartment1 " +
               " FROM dbo.Users AS u " +
               " INNER JOIN dbo.Organizs AS o ON u.organizId = o.organizId " +
               " INNER JOIN dbo.Departments AS d ON o.DepartmentId = d.DepartmentId " +
               " WHERE d.DepartmentId=12 AND u.StatusUserId =1 " +
               " UNION SELECT 'IT +' as HeadDepartment1,COUNT(*) as CountDepartment1," +
               " (SELECT count(*) FROM dbo.Users WHERE dbo.Users.StatusUserId =1) as TotalEmployee ,'' AS ProgressbarDepartment1 " +
               " FROM dbo.Users AS u " +
               " INNER JOIN dbo.Organizs AS o ON u.organizId = o.organizId " +
               " INNER JOIN dbo.Departments AS d ON o.DepartmentId = d.DepartmentId " +
               " WHERE d.DepartmentId=8 AND u.StatusUserId =1 ";



            var reportDepAF = _context.ReportDeparment1s.FromSqlRaw(queryDepAF).ToList();

            var headerDepAF = "<table>" +
                    " <thead> " +
                    "<tr> " +
                    "<th width='180px'>" +
                    "<sapn><h2></h2></sapn>" +
                    "</th>" +
                    "<th width='50px'>" +
                    "<sapn></sapn>" +
                    "</th>" +
                    "<th style='text-align:right' width='200px'>" +
                    "<sapn></sapn>" +
                    "</th>" +
                    "</tr>" +
                    "</thead>" +
                    "<tbody>";
            var footerDepAF = "</tbody>" +
                            "</table>";

            var bodyDepAF = "";
            decimal percentDepAF = 0;
            var countDepAF = 0;
            var totalempDepAF = 0;

            foreach (var std in reportDepAF as IList<ReportDepartment1>)
            {
                countDepAF = std.CountDepartment1;
                totalempDepAF = std.TotalEmployee1;
                percentDepAF = Percen(countDepCon, totalempDepCon);

                bodyDepAF += " <tr valign='top'>";
                bodyDepAF += " <td style='text-align:right;padding-right:10px'>";
                bodyDepAF += " <span>" + std.HeadDepartment1 + "</span>          ";
                bodyDepAF += " </td>";
                bodyDepAF += " <td>";
                bodyDepAF += " <span>" + std.CountDepartment1 + "</span>";
                bodyDepAF += " </td>";
                bodyDepAF += " <td>";
                bodyDepAF += " <div class='progress'>";
                bodyDepAF += "<div class='progress-bar' role='progress-bar' aria-valuenow='40' style='width:" + percentDepAF + "%' id='bg-progressDep'><span style='color:#4d4f53;font-weight: bold;font-size:10px;'>" + percentDepAF + "%</span>";
                bodyDepAF += "</div>";
                bodyDepAF += "</td>";
                bodyDepAF += " </tr>";

            }

            var tableDepAF = headerDepAF + bodyDepAF + footerDepAF;

            //Purchasing---Design
            var queryDepTE = "SELECT 'Puchasing' as HeadDepartment1,COUNT(*) as CountDepartment1," +
               " (SELECT count(*) FROM dbo.Users WHERE dbo.Users.StatusUserId =1) AS TotalEmployee1 ,'' AS ProgressbarDepartment1 " +
               " FROM dbo.Users AS u " +
               " INNER JOIN dbo.Organizs AS o ON u.organizId = o.organizId " +
               " INNER JOIN dbo.Departments AS d ON o.DepartmentId = d.DepartmentId " +
               " WHERE d.DepartmentId=4 AND u.StatusUserId =1 " +
               " UNION SELECT 'Cost Management' as HeadDepartment1,COUNT(*) as CountDepartment1," +
               " (SELECT count(*) FROM dbo.Users WHERE dbo.Users.StatusUserId =1) as TotalEmployee ,'' AS ProgressbarDepartment1 " +
               " FROM dbo.Users AS u " +
               " INNER JOIN dbo.Organizs AS o ON u.organizId = o.organizId " +
               " INNER JOIN dbo.Departments AS d ON o.DepartmentId = d.DepartmentId " +
               " WHERE d.DepartmentId=15 AND u.StatusUserId =1 " +
               " UNION SELECT 'Technical' as HeadDepartment1,COUNT(*) as CountDepartment1," +
               " (SELECT count(*) FROM dbo.Users WHERE dbo.Users.StatusUserId =1) as TotalEmployee ,'' AS ProgressbarDepartment1 " +
               " FROM dbo.Users AS u " +
               " INNER JOIN dbo.Organizs AS o ON u.organizId = o.organizId " +
               " INNER JOIN dbo.Departments AS d ON o.DepartmentId = d.DepartmentId " +
               " WHERE d.DepartmentId=6 AND u.StatusUserId =1 " +
               " UNION SELECT 'Estimation' as HeadDepartment1,COUNT(*) as CountDepartment1," +
               " (SELECT count(*) FROM dbo.Users WHERE dbo.Users.StatusUserId =1) as TotalEmployee ,'' AS ProgressbarDepartment1 " +
               " FROM dbo.Users AS u " +
               " INNER JOIN dbo.Organizs AS o ON u.organizId = o.organizId " +
               " INNER JOIN dbo.Departments AS d ON o.DepartmentId = d.DepartmentId " +
               " WHERE d.DepartmentId=13 AND u.StatusUserId =1 " +
               " UNION SELECT 'Design' as HeadDepartment1,COUNT(*) as CountDepartment1," +
               " (SELECT count(*) FROM dbo.Users WHERE dbo.Users.StatusUserId =1) as TotalEmployee ,'' AS ProgressbarDepartment1 " +
               " FROM dbo.Users AS u " +
               " INNER JOIN dbo.Organizs AS o ON u.organizId = o.organizId " +
               " INNER JOIN dbo.Departments AS d ON o.DepartmentId = d.DepartmentId " +
               " WHERE d.DepartmentId=14 AND u.StatusUserId =1 ";

            var reportDepTE = _context.ReportDeparment1s.FromSqlRaw(queryDepTE).ToList();

            var headerDepTE = "<table>" +
                    " <thead> " +
                    "<tr> " +
                    "<th width='180px'>" +
                    "<sapn><h2></h2></sapn>" +
                    "</th>" +
                    "<th width='50px'>" +
                    "<sapn></sapn>" +
                    "</th>" +
                    "<th style='text-align:right' width='200px'>" +
                    "<sapn></sapn>" +
                    "</th>" +
                    "</tr>" +
                    "</thead>" +
                    "<tbody>";
            var footerDepTE = "</tbody>" +
                            "</table>";

            var bodyDepTE = "";
            decimal percentDepTE = 0;
            var countDepTE = 0;
            var totalempDepTE = 0;


            foreach (var std in reportDepTE as IList<ReportDepartment1>)
            {
                countDepTE = std.CountDepartment1;
                totalempDepTE = std.TotalEmployee1;
                percentDepTE = Percen(countDepTE, totalempDepTE);

                bodyDepTE += " <tr valign='top'>";
                bodyDepTE += " <td style='text-align:right;padding-right:10px'>";
                bodyDepTE += " <span>" + std.HeadDepartment1 + "</span>          ";
                bodyDepTE += " </td>";
                bodyDepTE += " <td>";
                bodyDepTE += " <span>" + std.CountDepartment1 + "</span>";
                bodyDepTE += " </td>";
                bodyDepTE += " <td>";
                bodyDepTE += " <div class='progress'>";
                bodyDepTE += "<div class='progress-bar' role='progress-bar' aria-valuenow='40' style='width:" + percentDepTE + "%' id='bg-progressDep'>" +
                              "<span style='color:#4d4f53;font-weight: bold;font-size:10px;'>" + percentDepTE + "%</span>";
                bodyDepTE += "</div>";
                bodyDepTE += " </td>";
                bodyDepTE += " </tr>";

            }

            var tableDepTE = headerDepTE + bodyDepTE + footerDepTE;


            //Department1
            //--1--
            var queryDep1Cons = "SELECT 'Construction' as HeadDepartment1,COUNT(*) as CountDepartment1," +
               " (SELECT count(*) FROM dbo.Users WHERE dbo.Users.StatusUserId =1) AS TotalEmployee1 ,'' AS ProgressbarDepartment1 " +
               " FROM dbo.Users AS u " +
               " INNER JOIN dbo.Organizs AS o ON u.organizId = o.organizId " +
               " INNER JOIN dbo.Department1s AS dp ON o.Department1Id = dp.Department1Id " +
               " WHERE dp.Department1Id=3 AND u.StatusUserId =1 " +
               " UNION SELECT 'Wherehouse' as HeadDepartment1,COUNT(*) as CountDepartment1," +
               " (SELECT count(*) FROM dbo.Users WHERE dbo.Users.StatusUserId =1) as TotalEmployee ,'' AS ProgressbarDepartment1 " +
               " FROM dbo.Users AS u " +
               " INNER JOIN dbo.Organizs AS o ON u.organizId = o.organizId " +
               " INNER JOIN dbo.Department1s AS dp ON o.Department1Id = dp.Department1Id " +
               " WHERE dp.Department1Id=12 AND u.StatusUserId =1" +
               " UNION SELECT 'Machinery & Equipment' as HeadDepartment1,COUNT(*) as CountDepartment1," +
               " (SELECT count(*) FROM dbo.Users WHERE dbo.Users.StatusUserId =1) as TotalEmployee ,'' AS ProgressbarDepartment1 " +
               " FROM dbo.Users AS u " +
               " INNER JOIN dbo.Organizs AS o ON u.organizId = o.organizId " +
               " INNER JOIN dbo.Department1s AS dp ON o.Department1Id = dp.Department1Id " +
               " WHERE dp.Department1Id=6 AND u.StatusUserId =1 ";

            var reportDep1Cons = _context.ReportDeparment1s.FromSqlRaw(queryDep1Cons).ToList();

            var headerDep1Cons = "<table>" +
                    " <thead> " +
                    "<tr> " +
                    "<th width='180px'>" +
                    "<sapn><h2></h2></sapn>" +
                    "</th>" +
                    "<th width='50px'>" +
                    "<sapn>คน</sapn>" +
                    "</th>" +
                    "<th style='text-align:right' width='200px'>" +
                    "<sapn>%</sapn>" +
                    "</th>" +
                    "</tr>" +
                    "</thead>" +
                    "<tbody>";
            var footerDep1Cons = "</tbody>" +
                            "</table>";

            var bodyDep1Cons = "";
            decimal percentDep1Cons = 0;
            var countDep1Cons = 0;
            var totalempDep1Cons = 0;

            foreach (var std in reportDep1Cons as IList<ReportDepartment1>)
            {
                countDep1Cons = std.CountDepartment1;
                totalempDep1Cons = std.TotalEmployee1;
                percentDep1Cons = Percen(countDep1Cons, totalempDep1Cons);

                bodyDep1Cons += " <tr valign='top'>";
                bodyDep1Cons += " <td style='text-align:right;padding-right:10px'>";
                bodyDep1Cons += " <span>" + std.HeadDepartment1 + "</span>";
                bodyDep1Cons += " </td>";
                bodyDep1Cons += " <td>";
                bodyDep1Cons += " <span>" + std.CountDepartment1 + "</span>";
                bodyDep1Cons += " </td>";
                bodyDep1Cons += " <td>";
                bodyDep1Cons += " <div class='progress'>";
                bodyDep1Cons += "<div class='progress-bar' role='progress-bar' aria-valuenow='40' style='width:" + percentDep1Cons + "%' id='bg-progressDep1'>" +
                                "<span style='color:#4d4f53;font-weight: bold;font-size:10px;'>" + percentDep1Cons + "%</span>";
                bodyDep1Cons += "</div>";
                bodyDep1Cons += " </td>";
                bodyDep1Cons += " </tr>";

            }

            var tableDep1Cons = headerDep1Cons + bodyDep1Cons + footerDep1Cons;

            //--2--
            var queryDep1AdminF = "SELECT 'Acounting' as HeadDepartment1,COUNT(*) as CountDepartment1," +
               " (SELECT count(*) FROM dbo.Users WHERE dbo.Users.StatusUserId =1) AS TotalEmployee1 ,'' AS ProgressbarDepartment1 " +
               " FROM dbo.Users AS u " +
               " INNER JOIN dbo.Organizs AS o ON u.organizId = o.organizId " +
               " INNER JOIN dbo.Department1s AS dp ON o.Department1Id = dp.Department1Id " +
               " WHERE dp.Department1Id=13 AND u.StatusUserId =1 " +
               " UNION SELECT 'Financial' as HeadDepartment1,COUNT(*) as CountDepartment1," +
               " (SELECT count(*) FROM dbo.Users WHERE dbo.Users.StatusUserId =1) as TotalEmployee ,'' AS ProgressbarDepartment1 " +
               " FROM dbo.Users AS u " +
               " INNER JOIN dbo.Organizs AS o ON u.organizId = o.organizId " +
               " INNER JOIN dbo.Department1s AS dp ON o.Department1Id = dp.Department1Id " +
               " WHERE dp.Department1Id=5 AND u.StatusUserId =1" +
               " UNION SELECT 'HR' as HeadDepartment1,COUNT(*) as CountDepartment1," +
               " (SELECT count(*) FROM dbo.Users WHERE dbo.Users.StatusUserId =1) as TotalEmployee ,'' AS ProgressbarDepartment1 " +
               " FROM dbo.Users AS u " +
               " INNER JOIN dbo.Organizs AS o ON u.organizId = o.organizId " +
               " INNER JOIN dbo.Department1s AS dp ON o.Department1Id = dp.Department1Id " +
               " WHERE dp.Department1Id=14 AND u.StatusUserId =1 " +
               " UNION SELECT 'Admin' as HeadDepartment1,COUNT(*) as CountDepartment1," +
               " (SELECT count(*) FROM dbo.Users WHERE dbo.Users.StatusUserId =1) as TotalEmployee ,'' AS ProgressbarDepartment1 " +
               " FROM dbo.Users AS u " +
               " INNER JOIN dbo.Organizs AS o ON u.organizId = o.organizId " +
               " INNER JOIN dbo.Department1s AS dp ON o.Department1Id = dp.Department1Id " +
               " WHERE dp.Department1Id=11 AND u.StatusUserId =1 " +
               " UNION SELECT 'Law' as HeadDepartment1,COUNT(*) as CountDepartment1," +
               " (SELECT count(*) FROM dbo.Users WHERE dbo.Users.StatusUserId =1) as TotalEmployee ,'' AS ProgressbarDepartment1 " +
               " FROM dbo.Users AS u " +
               " INNER JOIN dbo.Organizs AS o ON u.organizId = o.organizId " +
               " INNER JOIN dbo.Department1s AS dp ON o.Department1Id = dp.Department1Id " +
               " WHERE dp.Department1Id=2 AND u.StatusUserId =1 " +
               " UNION SELECT 'NSS' as HeadDepartment1,COUNT(*) as CountDepartment1," +
               " (SELECT count(*) FROM dbo.Users WHERE dbo.Users.StatusUserId =1) as TotalEmployee ,'' AS ProgressbarDepartment1 " +
               " FROM dbo.Users AS u " +
               " INNER JOIN dbo.Organizs AS o ON u.organizId = o.organizId " +
               " INNER JOIN dbo.Department1s AS dp ON o.Department1Id = dp.Department1Id " +
               " WHERE dp.Department1Id=17 AND u.StatusUserId =1 " +
               " UNION SELECT 'SD & Im' as HeadDepartment1,COUNT(*) as CountDepartment1," +
               " (SELECT count(*) FROM dbo.Users WHERE dbo.Users.StatusUserId =1) as TotalEmployee ,'' AS ProgressbarDepartment1 " +
               " FROM dbo.Users AS u " +
               " INNER JOIN dbo.Organizs AS o ON u.organizId = o.organizId " +
               " INNER JOIN dbo.Department1s AS dp ON o.Department1Id = dp.Department1Id " +
               " WHERE dp.Department1Id=16 AND u.StatusUserId =1 ";



            var reportDep1AdminF = _context.ReportDeparment1s.FromSqlRaw(queryDep1AdminF).ToList();

            var headerDep1AdminF = "<table>" +
                    " <thead> " +
                    "<tr> " +
                    "<th width='180px'>" +
                    "<sapn><h2></h2></sapn>" +
                    "</th>" +
                    "<th width='50px'>" +
                    "</th>" +
                    "<th style='text-align:right' width='200px'>" +
                    "</th>" +
                    "</tr>" +
                    "</thead>" +
                    "<tbody>";
            var footerDep1AdminF = "</tbody>" +
                            "</table>";

            var bodyDep1AdminF = "";
            decimal percentDep1AdminF = 0;
            var countDep1AdminF = 0;
            var totalempDep1AdminF = 0;


            foreach (var std in reportDep1AdminF as IList<ReportDepartment1>)
            {
                countDep1AdminF = std.CountDepartment1;
                totalempDep1AdminF = std.TotalEmployee1;
                percentDep1AdminF = Percen(countDep1AdminF, totalempDep1AdminF);

                bodyDep1AdminF += " <tr valign='top'>";
                bodyDep1AdminF += " <td style='text-align:right;padding-right:10px'>";
                bodyDep1AdminF += " <span>" + std.HeadDepartment1 + "</span>          ";
                bodyDep1AdminF += " </td>";
                bodyDep1AdminF += " <td>";
                bodyDep1AdminF += " <span>" + std.CountDepartment1 + "</span>";
                bodyDep1AdminF += " <td>";
                bodyDep1AdminF += " <div class='progress'>";
                bodyDep1AdminF += "<div class='progress-bar' role='progress-bar' aria-valuenow='40' style='width:" + percentDep1AdminF + "%' id='bg-progressDep1'>" +
                                  "<span style='color:#4d4f53;font-weight: bold;font-size:10px;'>" + percentDep1AdminF + "%</span>";
                bodyDep1AdminF += "</div>";
                bodyDep1AdminF += " </td>";
                bodyDep1AdminF += " </tr>";

            }

            var tableDep1AdminF = headerDep1AdminF + bodyDep1AdminF + footerDep1AdminF;

            //--3--
            var queryDep1TechE = "SELECT 'Purchasing' as HeadDepartment1,COUNT(*) as CountDepartment1," +
               " (SELECT count(*) FROM dbo.Users WHERE dbo.Users.StatusUserId =1) AS TotalEmployee1 ,'' AS ProgressbarDepartment1 " +
               " FROM dbo.Users AS u " +
               " INNER JOIN dbo.Organizs AS o ON u.organizId = o.organizId " +
               " INNER JOIN dbo.Department1s AS dp ON o.Department1Id = dp.Department1Id " +
               " WHERE dp.Department1Id=7 AND u.StatusUserId =1 " +
               " UNION SELECT 'Cost Management' as HeadDepartment1,COUNT(*) as CountDepartment1," +
               " (SELECT count(*) FROM dbo.Users WHERE dbo.Users.StatusUserId =1) as TotalEmployee ,'' AS ProgressbarDepartment1 " +
               " FROM dbo.Users AS u " +
               " INNER JOIN dbo.Organizs AS o ON u.organizId = o.organizId " +
               " INNER JOIN dbo.Department1s AS dp ON o.Department1Id = dp.Department1Id " +
               " WHERE dp.Department1Id=19 AND u.StatusUserId =1" +
               " UNION SELECT 'Technical' as HeadDepartment1,COUNT(*) as CountDepartment1," +
               " (SELECT count(*) FROM dbo.Users WHERE dbo.Users.StatusUserId =1) as TotalEmployee ,'' AS ProgressbarDepartment1 " +
               " FROM dbo.Users AS u " +
               " INNER JOIN dbo.Organizs AS o ON u.organizId = o.organizId " +
               " INNER JOIN dbo.Department1s AS dp ON o.Department1Id = dp.Department1Id " +
               " WHERE dp.Department1Id=10 AND u.StatusUserId =1 " +
               " UNION SELECT 'Estimation' as HeadDepartment1,COUNT(*) as CountDepartment1," +
               " (SELECT count(*) FROM dbo.Users WHERE dbo.Users.StatusUserId =1) as TotalEmployee ,'' AS ProgressbarDepartment1 " +
               " FROM dbo.Users AS u " +
               " INNER JOIN dbo.Organizs AS o ON u.organizId = o.organizId " +
               " INNER JOIN dbo.Department1s AS dp ON o.Department1Id = dp.Department1Id " +
               " WHERE dp.Department1Id=15 AND u.StatusUserId =1 " +
               " UNION SELECT 'Design' as HeadDepartment1,COUNT(*) as CountDepartment1," +
               " (SELECT count(*) FROM dbo.Users WHERE dbo.Users.StatusUserId =1) as TotalEmployee ,'' AS ProgressbarDepartment1 " +
               " FROM dbo.Users AS u " +
               " INNER JOIN dbo.Organizs AS o ON u.organizId = o.organizId " +
               " INNER JOIN dbo.Department1s AS dp ON o.Department1Id = dp.Department1Id " +
               " WHERE dp.Department1Id=18 AND u.StatusUserId =1 ";



            var reportDep1TechE = _context.ReportDeparment1s.FromSqlRaw(queryDep1TechE).ToList();

            var headerDep1TechE = "<table>" +
                    " <thead> " +
                    "<tr> " +
                    "<th width='180px'>" +
                    "<sapn><h2></h2></sapn>" +
                    "</th>" +
                    "<th width='50px'>" +
                    "</th>" +
                    "<th style='text-align:right' width='150px'>" +
                    "</th>" +
                    "</tr>" +
                    "</thead>" +
                    "<tbody>";
            var footerDep1TechE = "</tbody>" +
                            "</table>";

            var bodyDep1TechE = "";
            decimal percentDep1TechE = 0;
            var countDep1TechE = 0;
            var totalempDep1TechE = 0;

            foreach (var std in reportDep1TechE as IList<ReportDepartment1>)
            {
                countDep1TechE = std.CountDepartment1;
                totalempDep1TechE = std.TotalEmployee1;
                percentDep1TechE = Percen(countDep1TechE, totalempDep1TechE);

                bodyDep1TechE += " <tr valign='top'>";
                bodyDep1TechE += " <td style='text-align:right;padding-right:10px'>";
                bodyDep1TechE += " <span>" + std.HeadDepartment1 + "</span> ";
                bodyDep1TechE += " </td>";
                bodyDep1TechE += " <td >";
                bodyDep1TechE += " <span>" + std.CountDepartment1 + "</span>";
                bodyDep1TechE += " </td>";
                bodyDep1TechE += " <td>";
                bodyDep1TechE += " <div class='progress'>";
                bodyDep1TechE += " <div class='progress-bar' role='progress-bar' aria-valuenow='40' style='width:" + percentDep1TechE + "%' id='bg-progressDep1'>" +
                                " <span style='color:#4d4f53;font-weight: bold;font-size:10px;'>" + percentDep1TechE + "%</span>";
                bodyDep1TechE += " </div>";
                bodyDep1TechE += " </td>";
                bodyDep1TechE += " </tr>";

            }

            var tableDep1TechE = headerDep1TechE + bodyDep1TechE + footerDep1TechE;

            response = Ok(new { TotalEmployee = TotalEmployee,TotalMale=TotalMale,TotalFemale=TotalFemale,Monly=Monly,Dayly=Dayly,Ho=Ho,Store=Store,Sites=Sites,tableGen=tableGen,reportGens=reportGens,tableAge=tableAge,tableManagement=tableManagement, tableLevel = tableLevel,tableCon=tableCon, tableAdminFinancail= tableAdminFinancail,tableTechEst=tableTechEst,tableDepCon=tableDepCon,tableDepAF= tableDepAF,tableDepTE= tableDepTE,tableDep1Cons=tableDep1Cons,tableDep1AdminF=tableDep1AdminF,tableDep1TechE=tableDep1TechE, percenMale=percenMale, percenFemale= percenFemale , percenMonly = percenMonly , percenDayly = percenDayly , percenHo = percenHo , percenStores = percenStores , percenSites = percenSites });


            return response;
           

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



    }

}
