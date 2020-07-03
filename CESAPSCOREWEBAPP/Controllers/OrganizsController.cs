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
    public class OrganizsController : BaseController
    {
        private readonly DatabaseContext _context;

        public OrganizsController(DatabaseContext context)
        {
            _context = context;
        }

        // GET: Organizs
        public async Task<IActionResult> Index()
        {
            /*Check Session */
            var page = "12";
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

            ViewData["Department1Id"] = new SelectList(_context.Department1s, "Department1Id", "Department1Name");
            ViewData["DepartmentId"] = new SelectList(_context.Departments, "DepartmentId", "DepartmentName");
            ViewData["PositionId"] = new SelectList(_context.Positions, "PositionId", "PositionName");

            var databaseContext = _context.Organizs.Include(o => o.Department1s).Include(o => o.Departments).Include(o => o.Positions);
            return View(await databaseContext.ToListAsync());
        }

        // GET: Organizs/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            /*Check Session */
            var page = "15";
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

            var organiz = await _context.Organizs
                .Include(o => o.Department1s)
                .Include(o => o.Departments)
                .Include(o => o.Positions)
                .FirstOrDefaultAsync(m => m.organizId == id);
            if (organiz == null)
            {
                return NotFound();
            }

            return View(organiz);
        }

        // GET: Organizs/Create
        public IActionResult Create()
        {
            ViewData["Department1Id"] = new SelectList(_context.Department1s, "Department1Id", "Department1Name");
            ViewData["DepartmentId"] = new SelectList(_context.Departments, "DepartmentId", "DepartmentName");
            ViewData["PositionId"] = new SelectList(_context.Positions, "PositionId", "PositionName");
            return View();
        }

        // POST: Organizs/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("organizId,DepartmentId,Department1Id,PositionId,Power")] Organiz organiz)
        {
            /*Check Session */
            var page = "13";
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
                _context.Add(organiz);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["Department1Id"] = new SelectList(_context.Department1s, "Department1Id", "Department1Name", organiz.Department1Id);
            ViewData["DepartmentId"] = new SelectList(_context.Departments, "DepartmentId", "DepartmentName", organiz.DepartmentId);
            ViewData["PositionId"] = new SelectList(_context.Positions, "PositionId", "PositionName", organiz.PositionId);
            return View(organiz);
        }

        // GET: Organizs/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var organiz = await _context.Organizs.FindAsync(id);
            if (organiz == null)
            {
                return NotFound();
            }
            ViewData["Department1Id"] = new SelectList(_context.Department1s, "Department1Id", "Department1Name", organiz.Department1Id);
            ViewData["DepartmentId"] = new SelectList(_context.Departments, "DepartmentId", "DepartmentName", organiz.DepartmentId);
            ViewData["PositionId"] = new SelectList(_context.Positions, "PositionId", "PositionName", organiz.PositionId);
            return View(organiz);
        }

        // POST: Organizs/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit([Bind("organizId,DepartmentId,Department1Id,PositionId,Power")] Organiz organiz)
        {
            /*Check Session */
            var page = "14";
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

                    //ViewData["Department1Id"] = new SelectList(_context.Department1s, "Department1Id", "Department1Name", organiz.Department1Id);
                    //ViewData["DepartmentId"] = new SelectList(_context.Departments, "DepartmentId", "DepartmentName", organiz.DepartmentId);
                    //ViewData["PositionId"] = new SelectList(_context.Positions, "PositionId", "PositionName", organiz.PositionId);

                    _context.Update(organiz);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!OrganizExists(organiz.organizId))
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
            //ViewData["Department1Id"] = new SelectList(_context.Department1s, "Department1Id", "Department1Name", organiz.Department1Id);
            //ViewData["DepartmentId"] = new SelectList(_context.Departments, "DepartmentId", "DepartmentName", organiz.DepartmentId);
            //ViewData["PositionId"] = new SelectList(_context.Positions, "PositionId", "PositionName", organiz.PositionId);
            return RedirectToAction("Index", "Home");
        }

        // GET: Organizs/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            /*Check Session */
            var page = "16";
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

            var organiz = await _context.Organizs
                .Include(o => o.Department1s)
                .Include(o => o.Departments)
                .Include(o => o.Positions)
                .FirstOrDefaultAsync(m => m.organizId == id);
            if (organiz == null)
            {
                return NotFound();
            }

            return View(organiz);
        }

        // POST: Organizs/Delete/5
        [HttpPost, ActionName("remove")]

        public async Task<IActionResult> remove(int id)
        {
            /*Check Session */
            var page = "16";
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
            var organiz = await _context.Organizs
                .Include(o => o.Department1s)
                .Include(o => o.Departments)
                .Include(o => o.Positions)
                .FirstOrDefaultAsync(m => m.organizId == id);
            var name = organiz.Positions.PositionName;

            _context.Organizs.Remove(organiz);
            await _context.SaveChangesAsync();

            response = Ok(new { name = name });
            return response;

        }

        private bool OrganizExists(int id)
        {
            return _context.Organizs.Any(e => e.organizId == id);
        }
    }
}
