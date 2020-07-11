using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using CESAPSCOREWEBAPP.Models;
using DevExtreme.AspNet.Mvc;
using DevExtreme.AspNet.Data;
using Newtonsoft.Json;
using static CESAPSCOREWEBAPP.Models.Enums;
using Microsoft.AspNetCore.Http;
using CESAPSCOREWEBAPP.Helpers;
using Microsoft.AspNetCore.Authorization;

namespace CESAPSCOREWEBAPP.Controllers
{
    [Authorize]
    public class DetailTableERPsController : BaseController
    {
        private readonly DatabaseContext _context;

        public DetailTableERPsController(DatabaseContext context)
        {
            _context = context;
        }

        // GET: DetailTableERPs
        public  IActionResult Index()
        {
            /*Check Session */
            var page = "217";
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

        [HttpGet]
        public object Get(DataSourceLoadOptions loadOptions)
        {

            /*Check Session */
            var page = "217";
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

            return DataSourceLoader.Load(_context.DetailTableERPs.ToList(), loadOptions);
        }

        [HttpPost]
        public IActionResult Post(string values)
        {
            /*Check Session */
            var page = "218";
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



            var detailTableERP = new DetailTableERP();
            JsonConvert.PopulateObject(values, detailTableERP);

            if (!TryValidateModel(detailTableERP))
                return BadRequest();

            _context.DetailTableERPs.Add(detailTableERP);
            _context.SaveChanges();

            return Ok();
        }


        [HttpPut]
        public IActionResult Put(int key, string values)
        {

            /*Check Session */
            var page = "219";
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

            var detailTableERP = _context.DetailTableERPs.First(a => a.ID == key);
            JsonConvert.PopulateObject(values, detailTableERP);

            if (!TryValidateModel(detailTableERP))
                return BadRequest();

            _context.SaveChanges();

            return Ok();
        }


        [HttpDelete]
        public void Delete(int key)
        {
            /*Check Session */
            var page = "221";
            var typeofuser = "";
            var PermisionAction = "";
            // CheckSession
            if (string.IsNullOrEmpty(HttpContext.Session.GetString("Username")))
            {
                Alert("กรุณา Login เข้าสู่ระบบ", NotificationType.error);
             
            }
            else
            {
                typeofuser = HttpContext.Session.GetString("TypeOfUserId");
                PermisionAction = HttpContext.Session.GetString("PermisionAction");
                if (PermisionHelper.CheckPermision(typeofuser, PermisionAction, page) == false)
                {
                    Alert("คุณไม่มีสิทธิ์ใช้งานหน้าดังกล่าว", NotificationType.error);
          
                }


                var detailTableERP = _context.DetailTableERPs.First(a => a.ID == key);
                _context.DetailTableERPs.Remove(detailTableERP);
                _context.SaveChanges();


            }
            /*Check Session */
          
        }





        // GET: DetailTableERPs/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var detailTableERP = await _context.DetailTableERPs
                .FirstOrDefaultAsync(m => m.ID == id);
            if (detailTableERP == null)
            {
                return NotFound();
            }

            return View(detailTableERP);
        }

        // GET: DetailTableERPs/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: DetailTableERPs/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("ID,TableID,TableName,FieldID,FieldName,Detail")] DetailTableERP detailTableERP)
        {
            if (ModelState.IsValid)
            {
                _context.Add(detailTableERP);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(detailTableERP);
        }

        // GET: DetailTableERPs/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var detailTableERP = await _context.DetailTableERPs.FindAsync(id);
            if (detailTableERP == null)
            {
                return NotFound();
            }
            return View(detailTableERP);
        }

        // POST: DetailTableERPs/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("ID,TableID,TableName,FieldID,FieldName,Detail")] DetailTableERP detailTableERP)
        {
            if (id != detailTableERP.ID)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(detailTableERP);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!DetailTableERPExists(detailTableERP.ID))
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
            return View(detailTableERP);
        }

        // GET: DetailTableERPs/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var detailTableERP = await _context.DetailTableERPs
                .FirstOrDefaultAsync(m => m.ID == id);
            if (detailTableERP == null)
            {
                return NotFound();
            }

            return View(detailTableERP);
        }

        // POST: DetailTableERPs/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var detailTableERP = await _context.DetailTableERPs.FindAsync(id);
            _context.DetailTableERPs.Remove(detailTableERP);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool DetailTableERPExists(int id)
        {
            return _context.DetailTableERPs.Any(e => e.ID == id);
        }
    }
}
