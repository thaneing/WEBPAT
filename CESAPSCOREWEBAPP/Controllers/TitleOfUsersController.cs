using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using CESAPSCOREWEBAPP.Models;
using Microsoft.AspNetCore.Http;
using static CESAPSCOREWEBAPP.Models.Enums;
using CESAPSCOREWEBAPP.Helpers;
using Microsoft.AspNetCore.Authorization;

namespace CESAPSCOREWEBAPP.Controllers
{
    [Authorize]
    public class TitleOfUsersController : BaseController
    {
        private readonly DatabaseContext _context;

        public TitleOfUsersController(DatabaseContext context)
        {
            _context = context;
        }

        // GET: TitleOfUsers
        public async Task<IActionResult> Index()
        {

            /*Check Session */
            var page = "42";
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

            //var titleOfUser = await _context.TitleOfUsers
            //    .FirstOrDefaultAsync(m => m.TitleOfUserId == id);

            //ViewData["TitleOfUserId"] = titleOfUser.TitleOfUserId;
            //ViewData["TitleOfUserName"] = titleOfUser.TitleOfUserName;

            return View(await _context.TitleOfUsers.ToListAsync());
        }

        // GET: TitleOfUsers/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            /*Check Session */
            var page = "45";
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

            var titleOfUser = await _context.TitleOfUsers
                .FirstOrDefaultAsync(m => m.TitleOfUserId == id);
            if (titleOfUser == null)
            {
                return NotFound();
            }

            return View(titleOfUser);
        }

        // GET: TitleOfUsers/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: TitleOfUsers/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("TitleOfUserId,TitleOfUserName")] TitleOfUser titleOfUser)
        {
            /*Check Session */
            var page = "43";
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

            if (ModelState.IsValid)
            {
                _context.Add(titleOfUser);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(titleOfUser);
        }

        // GET: TitleOfUsers/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var titleOfUser = await _context.TitleOfUsers.FindAsync(id);
            if (titleOfUser == null)
            {
                return NotFound();
            }
            return View(titleOfUser);
        }

        // POST: TitleOfUsers/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit([Bind("TitleOfUserId,TitleOfUserName")] TitleOfUser titleOfUser)
        {
            /*Check Session */
            var page = "44";
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

            if (ModelState.IsValid)
            {
                try
                {
                    //ViewData["TitleOfUserId"] = titleOfUser.TitleOfUserId;
                    //ViewData["TitleOfUserName"] = titleOfUser.TitleOfUserName;
          
                    _context.Update(titleOfUser);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!TitleOfUserExists(titleOfUser.TitleOfUserId))
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


            return RedirectToAction("Index", "Home");
        }

        // GET: TitleOfUsers/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            /*Check Session */
            var page = "46";
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

            var titleOfUser = await _context.TitleOfUsers
                .FirstOrDefaultAsync(m => m.TitleOfUserId == id);
            if (titleOfUser == null)
            {
                return NotFound();
            }


            return View(titleOfUser);
        }

        // POST: TitleOfUsers/Delete/5
        [HttpPost, ActionName("remove")]
    
        public async Task<IActionResult> remove(int id)
        {
            /*Check Session */
            var page = "46";
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
            var titleOfUser = await _context.TitleOfUsers.FindAsync(id);
            var name = titleOfUser.TitleOfUserName;

     
            _context.TitleOfUsers.Remove(titleOfUser);
            await _context.SaveChangesAsync();

            response = Ok(new { name = name});
            return response;
        }

        private bool TitleOfUserExists(int id)
        {
            return _context.TitleOfUsers.Any(e => e.TitleOfUserId == id);
        }
    }
}
