using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CESAPSCOREWEBAPP.Helpers;
using CESAPSCOREWEBAPP.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using static CESAPSCOREWEBAPP.Models.Enums;

namespace CESAPSCOREWEBAPP.Controllers
{
    public class UserJobController : BaseController
    {
        private readonly DatabaseContext _context;

        private readonly NAVContext _navcontext;

        public UserJobController(DatabaseContext context, NAVContext navcontext)
        {
            _context = context;
            _navcontext = navcontext;

        }


        // GET: UserJob/Index/5
        public async Task<IActionResult> Index(int? id)
        {

            /*Check Session */
            var page = "32";
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


            var query = "SELECT dbo."+ Environment.GetEnvironmentVariable("Company") +"Job].No_ AS JobNo,dbo."+ Environment.GetEnvironmentVariable("Company") +"Job].[Location Code] AS LocationCode FROM dbo."+ Environment.GetEnvironmentVariable("Company") +"Job]";
            ViewData["JobNo"] = _navcontext.v_Job.FromSqlRaw(query).ToList();


            if (id == null)
            {
                return NotFound();
            }

            var users = await _context.Users
       
                        .Include(p => p.TitleOfUsers)
                        .Include(p => p.StatusUser)
                        .Include(p => p.Branchs)
     
                        .FirstOrDefaultAsync(m => m.UserId == id);
            if (users == null)
            {
                return NotFound();
            }


            var userjob = _context.UserJobs
               .Where(s => s.UserId == id)
               .ToList();
            ViewData["UserJob"] = userjob;

    



            return View(users);



        }




        // POST: UserJob/Add/5
        [HttpPost, ActionName("Add")]
        public IActionResult Add(int id,string job)
        {


            /*Check Session */
            var page = "32";
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

            UserJob userJob = new UserJob();
            userJob.UserId = id;
            userJob.UserJobDetail = job;
   
        
                _context.UserJobs.Add(userJob);
                _context.SaveChanges();
    

            return Ok(userJob);
        }


        // POST: UserJob/remov/5
        [HttpPost, ActionName("remove")]
        public IActionResult remove(int id, string job)
        {

            /*Check Session */
            var page = "32";
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

            List<UserJob> userJobs = _context.UserJobs
                .Where(s => s.UserId == id && s.UserJobDetail== job)
                .ToList();

            _context.UserJobs.RemoveRange(userJobs);
            _context.SaveChanges();


            return Ok(userJobs);
        }



    }
}