using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using CESAPSCOREWEBAPP.Models;

namespace CESAPSCOREWEBAPP.Controllers
{
    public class AppointmentsController : Controller
    {
        private readonly DatabaseContext _context;

        public AppointmentsController(DatabaseContext context)
        {
            _context = context;
        }

        // GET: Appointments
        public async Task<IActionResult> Index()
        {
            var databaseContext = _context.appointments.Include(a => a.appEtcs).Include(a => a.appResults).Include(a => a.appStatuses).Include(a => a.appSuccesses).Include(a => a.appTelTypes);
            return View(await databaseContext.ToListAsync());
        }

        // GET: Appointments/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var appointment = await _context.appointments
                .Include(a => a.appEtcs)
                .Include(a => a.appResults)
                .Include(a => a.appStatuses)
                .Include(a => a.appSuccesses)
                .Include(a => a.appTelTypes)
                .FirstOrDefaultAsync(m => m.AppId == id);
            if (appointment == null)
            {
                return NotFound();
            }

            return View(appointment);
        }

        // GET: Appointments/Create
        public IActionResult Create()
        {
            ViewData["AppEtcId"] = new SelectList(_context.appEtcs, "AppEtcId", "AppEtcName");
            ViewData["AppResultId"] = new SelectList(_context.appResults, "AppResultId", "AppResultName");
            ViewData["AppStatusId"] = new SelectList(_context.appStatuses, "AppStatusId", "AppStatusName");
            ViewData["AppSuccessId"] = new SelectList(_context.appSuccesses, "AppSuccessId", "AppSuccessName");
            ViewData["AppTelTypeId"] = new SelectList(_context.appTelTypes, "AppTelTypeId", "AppTelTypeName");
            return View();
        }

        // POST: Appointments/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("AppId,HRRecruiteID,AppTelTypeId,AppTelDate,AppStatusId,AppDate,AppResultId,AppEtcId,AppSuccessId,AppCreateBy,AppCreateDate,AppUpdateBy,AppUpdateDate")] Appointment appointment)
        {
            if (ModelState.IsValid)
            {
                _context.Add(appointment);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["AppEtcId"] = new SelectList(_context.appEtcs, "AppEtcId", "AppEtcName", appointment.AppEtcId);
            ViewData["AppResultId"] = new SelectList(_context.appResults, "AppResultId", "AppResultName", appointment.AppResultId);
            ViewData["AppStatusId"] = new SelectList(_context.appStatuses, "AppStatusId", "AppStatusName", appointment.AppStatusId);
            ViewData["AppSuccessId"] = new SelectList(_context.appSuccesses, "AppSuccessId", "AppSuccessName", appointment.AppSuccessId);
            ViewData["AppTelTypeId"] = new SelectList(_context.appTelTypes, "AppTelTypeId", "AppTelTypeName", appointment.AppTelTypeId);


            return View(appointment);
        }

        // GET: Appointments/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var appointment = await _context.appointments.FindAsync(id);
            if (appointment == null)
            {
                return NotFound();
            }
            ViewData["AppEtcId"] = new SelectList(_context.appEtcs, "AppEtcId", "AppEtcName", appointment.AppEtcId);
            ViewData["AppResultId"] = new SelectList(_context.appResults, "AppResultId", "AppResultName", appointment.AppResultId);
            ViewData["AppStatusId"] = new SelectList(_context.appStatuses, "AppStatusId", "AppStatusName", appointment.AppStatusId);
            ViewData["AppSuccessId"] = new SelectList(_context.appSuccesses, "AppSuccessId", "AppSuccessName", appointment.AppSuccessId);
            ViewData["AppTelTypeId"] = new SelectList(_context.appTelTypes, "AppTelTypeId", "AppTelTypeName", appointment.AppTelTypeId);
            return View(appointment);
        }

        // POST: Appointments/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("AppId,HRRecruiteID,AppTelTypeId,AppTelDate,AppStatusId,AppDate,AppResultId,AppEtcId,AppSuccessId,AppCreateBy,AppCreateDate,AppUpdateBy,AppUpdateDate")] Appointment appointment)
        {
            if (id != appointment.AppId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(appointment);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!AppointmentExists(appointment.AppId))
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
            ViewData["AppEtcId"] = new SelectList(_context.appEtcs, "AppEtcId", "AppEtcName", appointment.AppEtcId);
            ViewData["AppResultId"] = new SelectList(_context.appResults, "AppResultId", "AppResultName", appointment.AppResultId);
            ViewData["AppStatusId"] = new SelectList(_context.appStatuses, "AppStatusId", "AppStatusName", appointment.AppStatusId);
            ViewData["AppSuccessId"] = new SelectList(_context.appSuccesses, "AppSuccessId", "AppSuccessName", appointment.AppSuccessId);
            ViewData["AppTelTypeId"] = new SelectList(_context.appTelTypes, "AppTelTypeId", "AppTelTypeName", appointment.AppTelTypeId);
            return View(appointment);
        }

        // GET: Appointments/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var appointment = await _context.appointments
                .Include(a => a.appEtcs)
                .Include(a => a.appResults)
                .Include(a => a.appStatuses)
                .Include(a => a.appSuccesses)
                .Include(a => a.appTelTypes)
                .FirstOrDefaultAsync(m => m.AppId == id);
            if (appointment == null)
            {
                return NotFound();
            }

            return View(appointment);
        }

        // POST: Appointments/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var appointment = await _context.appointments.FindAsync(id);
            _context.appointments.Remove(appointment);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool AppointmentExists(int id)
        {
            return _context.appointments.Any(e => e.AppId == id);
        }

        // GET: Appointments/Details/5
        public async Task<Appointment> GetData(int? id)
        {

            return await _context.appointments.FirstOrDefaultAsync(m => m.AppId == id);
            

        }






    }
}
