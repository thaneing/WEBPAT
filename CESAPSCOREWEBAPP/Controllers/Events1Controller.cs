using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using CESAPSCOREWEBAPP.Models;
using CESAPSCOREWEBAPP.Helpers;
using Microsoft.AspNetCore.Http;
using static CESAPSCOREWEBAPP.Models.Enums;
using System.Net;

namespace CESAPSCOREWEBAPP.Controllers
{
    public class Events1Controller : BaseController
    {
        private readonly DatabaseContext _context;

        public Events1Controller(DatabaseContext context)
        {
            _context = context;
        }

    


        // POST: Events1/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.

        public async Task<IActionResult> Create([Bind("Id,Subject,Description,Start,End,ThemeColor,IsFullDay")] Event @event)
        {

            IActionResult response = Unauthorized();

            /*Check Session */
            var page = "77";
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

            if (@event.Id == null)
            {
                _context.Add(@event);
                await _context.SaveChangesAsync();
            }
            else
            {
                _context.Update(@event);
                await _context.SaveChangesAsync();
            }
            response = Ok(new { table = "OK" });
            return response;
        }

        // POST: Events1/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            IActionResult response = Unauthorized();


            /*Check Session */
            var page = "78";
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


            var @event = await _context.Events.FindAsync(id);
            _context.Events.Remove(@event);
            await _context.SaveChangesAsync();
            //total_table = query;
            response = Ok(new { table = "OK" });
            //response = Ok(new { table = Job });
            //response = Ok(new { table = queryLocation });
            return response;
        }

        private bool EventExists(int id)
        {
            return _context.Events.Any(e => e.Id == id);
        }
    }
}
