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

namespace CESAPSCOREWEBAPP.Controllers
{
    public class TypeCongratesController : BaseController
    {
        private readonly DatabaseContext _context;

        public TypeCongratesController(DatabaseContext context)
        {
            _context = context;
        }

        // GET: TypeCongrates
        public async Task<IActionResult> Index()
        {
            /*Check Session */
            var page = "114";
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

            return View(await _context.TypeCongrates.ToListAsync());
        }

        // GET: TypeCongrates/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            /*Check Session */
            var page = "117";
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

            var typeCongrate = await _context.TypeCongrates
                .FirstOrDefaultAsync(m => m.TypeCongrateId == id);
            if (typeCongrate == null)
            {
                return NotFound();
            }

            return View(typeCongrate);
        }

        // GET: TypeCongrates/Create
        public IActionResult Create()
        {
            /*Check Session */
            var page = "115";
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
            return View();
        }

        // POST: TypeCongrates/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("TypeCongrateId,TypeCongrateName")] TypeCongrate typeCongrate)
        {
            /*Check Session */
            var page = "115";
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
                _context.Add(typeCongrate);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(typeCongrate);
        }

        // GET: TypeCongrates/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            /*Check Session */
            var page = "116";
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

            var typeCongrate = await _context.TypeCongrates.FindAsync(id);
            if (typeCongrate == null)
            {
                return NotFound();
            }
            return View(typeCongrate);
        }

        // POST: TypeCongrates/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("TypeCongrateId,TypeCongrateName")] TypeCongrate typeCongrate)
        {
            /*Check Session */
            var page = "116";
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

            if (id != typeCongrate.TypeCongrateId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(typeCongrate);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!TypeCongrateExists(typeCongrate.TypeCongrateId))
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
            return View(typeCongrate);
        }

        // GET: TypeCongrates/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            /*Check Session */
            var page = "118";
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

            var typeCongrate = await _context.TypeCongrates
                .FirstOrDefaultAsync(m => m.TypeCongrateId == id);
            if (typeCongrate == null)
            {
                return NotFound();
            }

            return View(typeCongrate);
        }

        // POST: TypeCongrates/Delete/5
        [HttpPost, ActionName("remove")]
   
        public async Task<IActionResult> remove(int id)
        {
            /*Check Session */
            var page = "118";
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

            var typeCongrate = await _context.TypeCongrates.FindAsync(id);
            var name = typeCongrate.TypeCongrateName;

            _context.TypeCongrates.Remove(typeCongrate);
            await _context.SaveChangesAsync();

            response = Ok(new { name = name });
            return response;

        }

        private bool TypeCongrateExists(int id)
        {
            return _context.TypeCongrates.Any(e => e.TypeCongrateId == id);
        }
    }
}
