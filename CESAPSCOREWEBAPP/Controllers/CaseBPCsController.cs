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
    public class CaseBPCsController : BaseController
    {
        private readonly DatabaseContext _context;

        public CaseBPCsController(DatabaseContext context)
        {
            _context = context;
        }

        // GET: CaseBPCs
        public async Task<IActionResult> Index()
        {
            /*Check Session */
            var page = "189";
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

            return View(await _context.caseBPCs.ToListAsync());
        }

        // GET: CaseBPCs/Details/5
        public async Task<IActionResult> Details(int? id)
        {

            /*Check Session */
            var page = "192";
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

            var caseBPC = await _context.caseBPCs
                .FirstOrDefaultAsync(m => m.CaseBPCId == id);
            if (caseBPC == null)
            {
                return NotFound();
            }

            return View(caseBPC);
        }

        // GET: CaseBPCs/Create
        public IActionResult Create()
        {
            /*Check Session */
            var page = "190";
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

        // POST: CaseBPCs/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("CaseBPCId,CaseBPCDate,CaseMA,CaseBPCSubject,CaseBPCDetail,caseBPCStatus,caseBPCPLevel,CaseBPCPDateFix,EditBy,openCaseBy")] CaseBPC caseBPC)
        {

            /*Check Session */
            var page = "190";
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


                _context.Add(caseBPC);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(caseBPC);
        }

        // GET: CaseBPCs/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            /*Check Session */
            var page = "191";
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

            var caseBPC = await _context.caseBPCs.FindAsync(id);
            if (caseBPC == null)
            {
                return NotFound();
            }
            return View(caseBPC);
        }

        // POST: CaseBPCs/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("CaseBPCId,CaseBPCDate,CaseMA,CaseBPCSubject,CaseBPCDetail,caseBPCStatus,caseBPCPLevel,CaseBPCPDateFix,EditBy,openCaseBy")] CaseBPC caseBPC)
        {
            /*Check Session */
            var page = "191";
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





            if (id != caseBPC.CaseBPCId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(caseBPC);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!CaseBPCExists(caseBPC.CaseBPCId))
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
            return View(caseBPC);
        }

        // GET: CaseBPCs/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            /*Check Session */
            var page = "193";
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

            var caseBPC = await _context.caseBPCs
                .FirstOrDefaultAsync(m => m.CaseBPCId == id);
            if (caseBPC == null)
            {
                return NotFound();
            }

            return View(caseBPC);
        }

        // POST: TitleOfUsers/Delete/5
        [HttpPost, ActionName("remove")]
        public async Task<IActionResult> remove(int id)
        {
            /*Check Session */
            var page = "193";
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
            var caseBPC = await _context.caseBPCs.FindAsync(id);
            _context.caseBPCs.Remove(caseBPC);

            var name = caseBPC.CaseBPCSubject;


            await _context.SaveChangesAsync();
            response = Ok(new { name = name });
            return response;
        }

        private bool CaseBPCExists(int id)
        {
            return _context.caseBPCs.Any(e => e.CaseBPCId == id);
        }

        public async Task<IActionResult> LineAlertAPI()
        {
            /*Check Session */
            var page = "189";
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
            TimeSpan t;
            var txt = "";
            int elapsedDays;
            var token = "";
            DateTime date1 = DateTime.Now;
            //var status = _context.caseBPCs.Select(p => p.caseBPCStatus != CaseBPC.CaseBPCStatus.Complete);
            var caseBPCs = _context.caseBPCs.Where(p => p.caseBPCStatus != CaseBPC.CaseBPCStatus.Complete).ToList();

            var LineApi =await _context.LineAPIs.FindAsync(1);
            token = LineApi.LineToken;
            var i = 0;
            foreach (var std in caseBPCs as IList<CaseBPC>)
            {
               
                t = date1 - std.CaseBPCDate;
                elapsedDays = t.Days;
                txt = "วันที่ " + std.CaseBPCDate.ToString("dd/MM/yyyy") +" MA : "+std.CaseMA + " เรื่อง : " + std.CaseBPCSubject + " ระยะเวลา : " + elapsedDays;

                LineAlert.LineNotifyBPC(txt, token);
                i++;

            }


            response = Ok(new { name = i });
            return response;


            //return 
        }

        public async Task<IActionResult> Gendata()
        {
            /*Check Session */
            IActionResult response = Unauthorized();
            var caseBPC=await _context.caseBPCs.Where(p => p.caseBPCStatus != CaseBPC.CaseBPCStatus.Complete).ToListAsync();
            response = Ok(caseBPC);
            return response;
        }

        public async Task<IActionResult> GenDataToken()
        {
            /*Check Session */
            IActionResult response = Unauthorized();
            var LineApi =  await _context.LineAPIs.Where(p=>p.Id==1).ToListAsync();
            response = Ok(LineApi);
            return response;
        }


    }
}
