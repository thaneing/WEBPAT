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
using Microsoft.AspNetCore.Authorization;

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

        [Authorize]
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
            ViewBag.branchs = _context.Branchs.ToList();  

            ViewBag.statusUsers = _context.statusUsers.ToList();
            ViewBag.typeOfEmp = _context.typeOfEmployee.ToList();
     

            return View(await _context.Users
                        .Include(p => p.TitleOfUsers)
                        .Include(p => p.StatusUser)
                        .Include(p => p.Branchs)
                        .Include(p => p.Bloods)
                        .Include(p => p.nationality)
                        .Include(p => p.povince)
                        .Include(p => p.TypeCongrates)
                        .Include(p => p.religions)
                        .Include(p => p.typeOfEmployee)
                
                        .ToListAsync());
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Index(int ddlTypeOfEmp, int ddlDep, int ddlDep1s, int ddlPosition, int ddlBranch, int ddlLevels, int ddlStatus)
        {
            var users = await _context.Users
                         .Include(p => p.TitleOfUsers)
                         .Include(p => p.StatusUser)
                         .Include(p => p.Branchs)
                         .Include(p => p.Bloods)
                         .Include(p => p.nationality)
                         .Include(p => p.povince)
                         .Include(p => p.TypeCongrates)
                         .Include(p => p.religions)
                         .Include(p => p.typeOfEmployee)
                 
                         .ToListAsync();

    
            ViewBag.branchs = _context.Branchs.ToList();
            ViewBag.statusUsers = _context.statusUsers.ToList();
            ViewBag.typeOfEmp = _context.typeOfEmployee.ToList();


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
                        .Include(p => p.TitleOfUsers)
                        .Include(p => p.StatusUser)
                        .Include(p => p.Branchs)
                        .Include(p => p.Bloods)
                        .Include(p => p.nationality)
                        .Include(p => p.povince)
                        .Include(p => p.TypeCongrates)
                        .Include(p => p.religions)
                        .Include(p => p.typeOfEmployee)
                        .Where(p => p.UserId == id)
                        .ToList();
            ViewData["users"] = user1;
            return View();
        }

        [Authorize]
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
            ViewData["BranchId"] = new SelectList(_context.Branchs, "BranchId", "BranchName");
            ViewData["StatusUserId"] = new SelectList(_context.statusUsers, "StatusUserId", "StatusUserName");
            ViewData["TypeOfEmployeeId"] = new SelectList(_context.typeOfEmployee, "TypeOfEmployeeId", "TypeOfEmployeeName");
            ViewData["ReligionId"] = new SelectList(_context.religions, "ReligionId", "ReligionName");
            ViewData["NationalityId"] = new SelectList(_context.nationalities, "NationalityId", "NationalityName");
            ViewData["BloodId"] = new SelectList(_context.Bloods, "BloodId", "BloodName");
            ViewData["PovinceData"] = new SelectList(_context.povinces, "PovinceId", "PovinceName");
            ViewData["TypeCongrateId"]=new SelectList(_context.TypeCongrates,"TypeCongrateId","TypeCongrateName");



            return View();
        }

        // POST: Users/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [Authorize]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("UserId,TitleOfUserId,Firstname,Lastname,EFirstName,ELastname,Nickname,BirthName,Pic,EmailContact,ExtTel,MobileTel,BranchId,StatusUserId,EmpId,UserCreateDate,BloodId,TypeCongrateId,CongrateDetail,NationalityId,PovinceId,Certificate,Reference,ReferenceTel,ResignationDate,TypeOfEmployeeId,ReligionId,Reletion")] User users, IFormFile uploadPic,string birth,string startwork,string endwork)
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
         
            ViewData["BranchId"] = new SelectList(_context.Branchs, "BranchId", "BranchName",users.BranchId);
            ViewData["StatusUserId"] = new SelectList(_context.statusUsers, "StatusUserId", "StatusUserName",users.StatusUserId);
            ViewData["TypeOfEmployeeId"] =new SelectList(_context.typeOfEmployee,"TypeOfEmployeeId","TypeOfEmployeeName",users.TypeOfEmployeeId);
            ViewData["ReligionId"] = new SelectList(_context.religions, "ReligionId", "ReligionName", users.ReligionId);
            ViewData["NationalityId"] = new SelectList(_context.nationalities, "NationalityId", "NationalityName", users.NationalityId);
            ViewData["BloodId"] = new SelectList(_context.Bloods, "BloodId", "BloodName", users.BloodId);
            ViewData["PovinceData"] = new SelectList(_context.povinces, "PovinceId","PovinceName", users.PovinceId);
            ViewData["TypeCongrateId"] = new SelectList(_context.TypeCongrates, "TypeCongrateId", "TypeCongrateName", users.TypeCongrateId);

          
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
        [Authorize]
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
            if (typeofuser == "3" || permisionname == "HR" || HttpContext.Session.GetInt32("Userid") == id)
            {

            }
            else
            {
                Alert("คุณไม่มีสิทธิ์ใช้งานหน้าดังกล่าว", NotificationType.error);
                return RedirectToAction("Index", "Home");
            }

            // GET: WebModuls/Details/5




            var users = await _context.Users
                            .Include(p => p.TitleOfUsers)
                            .Include(p => p.StatusUser)
                            .Include(p => p.Branchs)
                            .Include(p => p.Bloods)
                            .Include(p => p.nationality)
                            .Include(p => p.povince)
                            .Include(p => p.TypeCongrates)
                            .Include(p => p.religions)
                            .Include(p => p.typeOfEmployee)
                            .FirstOrDefaultAsync(m => m.UserId == id);
            if (users == null)
            {
                return NotFound();
            }
            ViewData["TitleOfUserId"] = new SelectList(_context.TitleOfUsers, "TitleOfUserId", "TitleOfUserName", users.TitleOfUserId);
          
            ViewData["BranchId"] = new SelectList(_context.Branchs, "BranchId", "BranchName", users.BranchId);
            ViewData["StatusUserId"] = new SelectList(_context.statusUsers, "StatusUserId", "StatusUserName", users.StatusUserId);
            ViewData["TypeOfEmployeeId"] = new SelectList(_context.typeOfEmployee, "TypeOfEmployeeId", "TypeOfEmployeeName", users.TypeOfEmployeeId);
            ViewData["ReligionId"] = new SelectList(_context.religions, "ReligionId", "ReligionName", users.ReligionId);
            ViewData["NationalityId"] = new SelectList(_context.nationalities, "NationalityId", "NationalityName", users.NationalityId);
            ViewData["BloodId"] = new SelectList(_context.Bloods, "BloodId", "BloodName", users.BloodId);
            ViewData["PovinceData"] = new SelectList(_context.povinces, "PovinceId", "PovinceName", users.PovinceId);
            ViewData["TypeCongrateId"] = new SelectList(_context.TypeCongrates, "TypeCongrateId", "TypeCongrateName", users.TypeCongrateId);
        

          
            var bitrh = users.BirthName;
            var startwork = users.UserCreateDate;
            var endwork = users.ResignationDate;

            ViewBag.startwork = startwork.ToString("dd-MM-yyyy", new CultureInfo("en-US"));

            try
            {
                ViewBag.date = Convert.ToDateTime(bitrh).ToString("dd-MM-yyyy", new CultureInfo("en-US"));
                if (users.ResignationDate == null)
                {
                    ViewBag.endwork = "";
                }
                else
                {

                    ViewBag.endwork = Convert.ToDateTime(endwork).ToString("dd-MM-yyyy", new CultureInfo("en-US"));
                }
            }
            catch
            {

            }
        
            ViewBag.branch = users.Branchs.BranchName;
            ViewBag.typeofemployee = users.typeOfEmployee.TypeOfEmployeeName;
            ViewBag.empid = users.EmpId;
            ViewBag.typeofcongrate = users.TypeCongrates.TypeCongrateName;
            ViewBag.Statusname = users.StatusUser.StatusUserName;
            ViewBag.Pic = users.Pic;

            return View(users);
        }

        // POST: Users/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [Authorize]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("UserId,TitleOfUserId,Firstname,Lastname,EFirstName,ELastname,Nickname,BirthName,Pic,EmailContact,ExtTel,MobileTel,BranchId,StatusUserId,EmpId,UserCreateDate,BloodId,TypeCongrateId,CongrateDetail,NationalityId,PovinceId,Weight,Height,Waistline,Certificate,Reference,ReferenceTel,ResignationDate,TypeOfEmployeeId,ReligionId,Reletion")] User users, IFormFile Pic, string PicDB, string birth, string startwork, string endwork)
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
            if (typeofuser == "3" || permisionname == "HR" || HttpContext.Session.GetInt32("Userid") == id)
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
          //  ViewData["LevelId"] = new SelectList(_context.Levels, "LevelId", "LevelName", users.LevelId);
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
                    //ViewData["LevelId"] = new SelectList(_context.Levels, "LevelId", "LevelName", users.LevelId);
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

                permisionname = HttpContext.Session.GetString("PermisionName");
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
        [Authorize]
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
                        .Include(p => p.TitleOfUsers)
                        .Include(p => p.StatusUser)
                        .Include(p => p.Branchs)
                        .Include(p => p.Bloods)
                        .Include(p => p.nationality)
                        .Include(p => p.povince)
                        .Include(p => p.TypeCongrates)
                        .Include(p => p.religions)
                        .Include(p => p.typeOfEmployee)
                       .ToList();

                if (term != "")
                {
                    users = users.Where(p =>
                         p.Firstname.Contains(term) ||
                         p.Lastname.Contains(term) ||
                         p.Nickname.Contains(term)).ToList();

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
                    obj += "<div class='col-lg-3'><div class='contact-box center-version'><a href='/users/Details/" + user.UserId + "'>";
                    obj += "<img alt='image' class='img-circle'  src='../" + pathSave + user.Pic + "'/>";
                    obj += "<h3 class='m-b-xs'><strong>" + user.TitleOfUsers.TitleOfUserName + user.Firstname + " " + user.Lastname + "</strong></h3>";
                    obj += "<address class='m-t-md'><strong>สาขา : " + user.Branchs.BranchName + "</strong></address>";

                    obj += "<i class='glyphicon glyphicon-envelope'></i> Email : " + user.EmailContact + "<br/>";
                    obj += "<i class='glyphicon glyphicon-phone-alt'></i> เบอร์ภายใน : " + user.ExtTel + "<br/>";
                    obj += "</div></div>";
                    i = i + 1;
                }
                return Ok(new { table3 = obj, sumdata = i });
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
                        .Include(p => p.TitleOfUsers)
                        .Include(p => p.StatusUser)
                        .Include(p => p.Branchs)
                        .Include(p => p.Bloods)
                        .Include(p => p.nationality)
                        .Include(p => p.povince)
                        .Include(p => p.TypeCongrates)
                        .Include(p => p.religions)
                        .Include(p => p.typeOfEmployee)
                .FirstOrDefaultAsync(m => m.UserId == id);
            if (user == null)
            {
                return NotFound();
            }


            var login = await _context.Logins.FirstOrDefaultAsync(m => m.UserId == id);
            if (login == null)
            {

                ViewData["CheckUserId"] = new SelectList(_context.CheckUsers, "CheckUserId", "CheckUserName");
                ViewData["PermisionId"] = new SelectList(_context.Permisions, "PermisionId", "PermisionName");
                ViewData["TypeOfUserId"] = new SelectList(_context.TypeOfUsers, "TypeOfUserId", "TypeOfUserName");
                return View(user);
            }
            else
            {

                ViewData["CheckUserId"] = new SelectList(_context.CheckUsers, "CheckUserId", "CheckUserName", login.CheckUserId);
                ViewData["PermisionId"] = new SelectList(_context.Permisions, "PermisionId", "PermisionName", login.PermisionId);
                ViewData["TypeOfUserId"] = new SelectList(_context.TypeOfUsers, "TypeOfUserId", "TypeOfUserName", login.TypeOfUserId);

                return RedirectToAction("EditUsername", "Users", new { id = id });

            }

        }


        // GET: Users/EditUsername/5
        [Authorize]
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
                    .Include(p => p.TitleOfUsers)
                    .Include(p => p.StatusUser)
                    .Include(p => p.Branchs)
                    .Include(p => p.Bloods)
                    .Include(p => p.nationality)
                    .Include(p => p.povince)
                    .Include(p => p.TypeCongrates)
                    .Include(p => p.religions)
                    .Include(p => p.typeOfEmployee)
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
        public async Task<IActionResult> AddUsername(string Username, string Password, string Repassword, int PermisionId, int TypeOfUserId, int CheckUserId, int UserId)
        {



            if (Password != Repassword)
            {
                Alert("Password ไม่เหมือนกัน", NotificationType.error);
                return RedirectToAction("AddUsername", "Users", new { id = UserId });

            }


            var user = await _context.Users

                        .Include(p => p.TitleOfUsers)
                        .Include(p => p.StatusUser)
                        .Include(p => p.Branchs)
                        .Include(p => p.Bloods)
                        .Include(p => p.nationality)
                        .Include(p => p.povince)
                        .Include(p => p.TypeCongrates)
                        .Include(p => p.religions)
                        .Include(p => p.typeOfEmployee)
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





            if (qry == null)
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
        [Authorize]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditUsername(int id, string Username, int PermisionId, int TypeOfUserId, int CheckUserId, int UserId, string passwordold, string passwordnew, string passwordnew1)
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
                if (passwordnew != null)
                {
                    if (checkregis.Password != hashed_passwordOld)
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


            var user = await _context.Users
                        .Include(p => p.TitleOfUsers)
                        .Include(p => p.StatusUser)
                        .Include(p => p.Branchs)
                        .Include(p => p.Bloods)
                        .Include(p => p.nationality)
                        .Include(p => p.povince)
                        .Include(p => p.TypeCongrates)
                        .Include(p => p.religions)
                        .Include(p => p.typeOfEmployee)
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

        // POST: Users/Delete/5
        [Authorize]
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
