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
