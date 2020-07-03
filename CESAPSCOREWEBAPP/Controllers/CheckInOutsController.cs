using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CESAPSCOREWEBAPP.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace CESAPSCOREWEBAPP.Controllers
{
    public class CheckInOutsController : Controller
    {
        private readonly DatabaseContext _context;
        public CheckInOutsController(DatabaseContext context)
        {
            _context = context;
        }


        public IActionResult Index()
        {
            return View();
        }



        public async Task<IActionResult> GetNumberrow(string Site)
        {
            IActionResult response = Unauthorized();
            var CheckRow = 0;
            var CheckRowInOuts = await _context.CheckRowInOuts.FirstOrDefaultAsync(s => s.Site == Site); //เช็คก่อนว่าลำดับที่เท่าไหร่
            if (CheckRowInOuts == null)
            {

            }
            else
            {
                CheckRow = CheckRowInOuts.LastRow;
            }


             response = Ok(CheckRow);
       

            return response;
        }






        public async Task<IActionResult> SendData(string ID, string Data1, string Data2, string Site,string CheckRow)
        {
            IActionResult response = Unauthorized();
            //CheckInOut Inout;
            //foreach (var std in checkInOuts as IList<CheckInOut>)
            //{
            //Inout = new CheckInOut();
            //var row = 0;
            CheckRowInOut CheckRowInOuts1 = new CheckRowInOut();

            var CheckRowInOuts = await _context.CheckRowInOuts.FirstOrDefaultAsync(s => s.Site == Site); //เช็คก่อนว่าลำดับที่เท่าไหร่
            if (CheckRowInOuts == null)
            {
                //CheckRowInOuts1.ID = 0;
                CheckRowInOuts1.Site = Site;
                CheckRowInOuts1.LastRow = Int32.Parse(CheckRow);
                CheckRowInOuts1.UpdateDate = DateTime.Now;

                _context.Add(CheckRowInOuts1);
                await _context.SaveChangesAsync();
            }
            else
            {
                CheckRowInOuts.LastRow = Int32.Parse(CheckRow);
                CheckRowInOuts.UpdateDate = DateTime.Now;
                _context.Update(CheckRowInOuts);
                await _context.SaveChangesAsync();

            }




            CheckInOut checkInOuts1 = new CheckInOut();
            var Inouts = await _context.CheckInOuts.FirstOrDefaultAsync(s => s.Data1 == Data1 && s.Data2 == Data2 && s.Site == Site); //เช็คว่าเคยมีการลงบันทึกข้อมูลหรือยัง
           


            



            if (Inouts == null)  //ไม่เคยลงให้ Create
            {
           
                //heckInOuts1.ID = 0;
                checkInOuts1.Data1 = Data1;
                checkInOuts1.Data2 = Data2;
                checkInOuts1.Site = Site;


                _context.Add(checkInOuts1);
                await _context.SaveChangesAsync();
                response = Ok(checkInOuts1);
            }
            else  //เคยลงให้ Update
            {
                _context.Update(Inouts);
                await _context.SaveChangesAsync();
                response = Ok(Inouts);
            }




    

  

            return response;
        }
    }
}