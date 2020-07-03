using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CESAPSCOREWEBAPP.Helpers;
using CESAPSCOREWEBAPP.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using static CESAPSCOREWEBAPP.Models.Enums;

namespace CESAPSCOREWEBAPP.Controllers
{
    public class MonitorUsersController : BaseController
    {
        private readonly DatabaseContext _context;

        public MonitorUsersController(DatabaseContext context)
        {
            _context = context;
        }

        public IActionResult Index()
        {
            return View();
        }
        public async Task<IActionResult> ResetPassword()
        {
            var login = await _context.Logins
                            .Include(p => p.Users)
                            .Include(p => p.Permisions)
                            .Include(p => p.Users.Organizs)
                            .Include(p => p.Users.Organizs.Department1s)
                            .Include(p => p.Users.Organizs.Departments)
                            .Include(p => p.Users.Organizs.Positions)
                            .Include(p => p.Users.Branchs)
                            .Include(p => p.TypeOfUsers)
                            .ToListAsync();


            return View(login);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        //public async Task<IActionResult> DefualtPwd([Bind("Password,Username,UserId,CheckUserId,PermisionId,TypeOfUserId,CreateDate")] Login login)
        //public async Task<IActionResult> DefualtPwd(int id, string Username, int PermisionId, int TypeOfUserId, int CheckUserId, int UserId)
        public async Task<IActionResult> DefualtPwd(int UserId, string users, string passw)
        {
            /*Check Session */
            var page = "201";
            var typeofuser = "";
            var PermisionAction = "";
            var usernamelogin = "";
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
            if (users != null && passw != null)
            {
                string hash = EncryptionHelper.Encrypt(passw);
                usernamelogin = HttpContext.Session.GetString("Username");

                //IQueryable<User> query = _context.Users;
                var login = _context.Logins.FirstOrDefault(p => p.Username.Equals(users) && p.Password.Equals(hash));

                if (login == null)
                {
                    Alert("รหัสผ่านผิดพลาด", NotificationType.error);
                    return RedirectToAction("ResetPassword");
                }
                if (users != usernamelogin)
                {
                    Alert("ยืนยันตัวตนไม่ถูกต้อง!!", NotificationType.error);
                    return RedirectToAction("ResetPassword");
                }

                var user = await _context.Users.FirstOrDefaultAsync(m => m.UserId == UserId);
                var login2 = await _context.Logins.FirstOrDefaultAsync(m => m.UserId == UserId);
                String pwd = "CES@1964";
                string hashed_password = EncryptionHelper.Encrypt(pwd);


                login2.Password = hashed_password;

      
                _context.Update(login2);
                await _context.SaveChangesAsync();
            }
            else
            {
                Alert("Invalid Account", NotificationType.error);
                return RedirectToAction("ResetPassword");


            }



            Alert("Reset Password เรียบร้อยแล้ว", NotificationType.success);
            return RedirectToAction("ResetPassword");
        }

           // GET: Users/EditUsername/5
            public async Task<IActionResult> TypOfUsers(int? id)
        {
            /*Check Session */
            var page = "223";
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
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> TypOfUsers(int id, string Username, int PermisionId, int TypeOfUserId, int CheckUserId, int UserId, string passwordold)
        {

            /*Check Session */
            var page = "223";
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

     

            typeofuser = HttpContext.Session.GetString("TypeOfUserId");
            PermisionAction = HttpContext.Session.GetString("PermisionAction");
            var permisionname = HttpContext.Session.GetString("PermisionName");




            var checkregis = await _context.Logins.FirstOrDefaultAsync(m => m.ID == id);

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


            //Login login = new Login();
            //login.ID = checkregis.ID;
            //login.Username = Username;
            checkregis.Username = Username;
            checkregis.CheckUserId = CheckUserId;
            checkregis.PermisionId = PermisionId;
            checkregis.TypeOfUserId = TypeOfUserId;
            //login.UserId = UserId;
            //login.CreateDate =checkregis.CreateDate;



            _context.Update(checkregis);
            await _context.SaveChangesAsync();


            if (typeofuser == "3")
            {
                Alert("แก้ไขข้อมูลเรียบร้อย", NotificationType.success);
                return RedirectToAction("ResetPassword");
            }
            else
            {
                Alert("แก้ไขข้อมูลสำเร็จ", NotificationType.success);
                return RedirectToAction("Index", "Home");
            }




        }


    }
}