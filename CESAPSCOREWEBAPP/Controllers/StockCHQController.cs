using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;
using CESAPSCOREWEBAPP.Helpers;
using CESAPSCOREWEBAPP.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using static CESAPSCOREWEBAPP.Models.Enums;

namespace CESAPSCOREWEBAPP.Controllers
{
    [Authorize]
    public class StockCHQController : BaseController
    {
        private readonly NAVContext _navcontext;
        private readonly DatabaseContext _context;


        public StockCHQController(NAVContext navcontext, DatabaseContext context)
        {
            _context = context;
            _navcontext = navcontext;
        }

        public IActionResult Index()
        {
            /*Check Session */
            var page = "256";
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
            var bankdata = "SELECT dbo." + Environment.GetEnvironmentVariable("Company") + "Bank Account].No_ AS name,dbo." + Environment.GetEnvironmentVariable("Company") + "Bank Account].Name as code FROM dbo." + Environment.GetEnvironmentVariable("Company") + "Bank Account]  where  dbo." + Environment.GetEnvironmentVariable("Company") + "Bank Account].Name != 'เงินสดในมือ'  order by dbo." + Environment.GetEnvironmentVariable("Company") + "Bank Account].Name asc ";
            var bank = _navcontext.sourceAutoCompletes.FromSqlRaw(bankdata).ToList();
            ViewData["bank"] = bank;

            var bankRatios = _context.BankRatios.ToList();
            ViewData["bankRatios"] = bankRatios;

            var inventory = _context.InventoryCHQs.ToList();
            ViewData["inventory"] = inventory;

            var chqfail = _context.CHQloses.ToList();
            ViewData["chqfail"] = chqfail;


            var bank2 = new SelectList(_context.BankRatios.OrderBy(s => s.BankName).ToList(), "BankName", "BankName");
            ViewData["bank2"] = bank2;

            ViewBag.StartDate1 = DateTime.Now.ToString("dd/MM/yyyy", new CultureInfo("en-US"));
            ViewBag.StartDate = DateTime.Now.ToString("01-MM-yyyy", new CultureInfo("en-US"));
            ViewBag.EndDate = DateTime.Now.ToString("dd-MM-yyyy", new CultureInfo("en-US"));

            ViewBag.banksearch = _context.BankRatios.ToList();

            return View();
        }
        //check bank regit
        public IActionResult CheckBank(string bankname)
        {
            IActionResult response = Unauthorized();

            var bankdata = "SELECT dbo." + Environment.GetEnvironmentVariable("Company") + "Bank Account].No_ AS name,dbo." + Environment.GetEnvironmentVariable("Company") + "Bank Account].Name as code FROM dbo." + Environment.GetEnvironmentVariable("Company") + "Bank Account] where  dbo." + Environment.GetEnvironmentVariable("Company") + "Bank Account].Name = {0} order by dbo." + Environment.GetEnvironmentVariable("Company") + "Bank Account].Name asc ";

            var bank = _navcontext.sourceAutoCompletes.FromSqlRaw(bankdata, bankname).ToList();

            //var HouseRentals = _context.HouseRentals.Where(p => p.EmpId == bankname).OrderByDescending(s => s.ID).ToList();

            response = Ok(new { data=bank});

            //return Ok(DetailEmpHouseReports);
            return response;
        }

        //check bank to inventory and CHqLose
        public IActionResult CheckBank2(string bankname)
        {
            IActionResult response = Unauthorized();

            var bank = _context.BankRatios.Where(p => p.BankName == bankname).ToList();

            //var bankdata= "SELECT dbo.BankRatios.BankName AS Name,dbo.BankRatios.BankCode AS Code " +
            //  "FROM dbo.BankRatios" +
            //  "WHERE dbo.BankRatios.BankName = {0}";

            //var bank = _context.DetailBanks.FromSqlRaw(bankdata, bankname).ToList();

            response = Ok(new { bank = bank });
            return response;
        }

        //อัตราส่วน
        //[HttpPost, ActionName("Add")]
        public IActionResult Add(string code, int qty,string bank)
        {
           
            IActionResult response = Unauthorized();
            var bankcheck = _context.BankRatios.Where(p => p.BankCode == code).Count();
            //var check = 0;
            if (bankcheck > 0)
            {
              
                var bankRatio =  _context.BankRatios.FirstOrDefault(m => m.BankCode == code);
                bankRatio.IssueQty = qty;
                bankRatio.EditBy = HttpContext.Session.GetString("Username");
                bankRatio.EditDate = DateTime.Now;

                _context.Update(bankRatio);
                _context.SaveChanges();

            }
            else
            {
                /*Check Session */
           
                BankRatio bankRatio = new BankRatio();

                bankRatio.BankName = bank;
                bankRatio.IssueQty = qty;
                bankRatio.BankCode = code;
                bankRatio.CreateBy = HttpContext.Session.GetString("Username");
                bankRatio.CreateDate = DateTime.Now;

                _context.BankRatios.Add(bankRatio);
                _context.SaveChanges();
            }

            response = Ok(new { bankcheck = bankcheck});
            return response;
        }


        // POST: UserJob/remov/5
        [HttpPost, ActionName("remove")]
        public async Task<IActionResult> remove(int id)
        {
            /*Check Session */
            var page = "259";
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
            var bank = await _context.BankRatios.FindAsync(id);
            _context.BankRatios.RemoveRange(bank);
            _context.SaveChanges();

            
            return Ok(bank);
        }
        public async Task<IActionResult> Edit(int id)
        {
            IActionResult response = Unauthorized();
            var bankRatio = await _context.BankRatios.FirstOrDefaultAsync(m => m.ID == id);

            var ID =   bankRatio.ID;
            var name = bankRatio.BankName;
            var qty = bankRatio.IssueQty;
            var code = bankRatio.BankCode;



            response = Ok(new { name = name, qty = qty, code = code,ID = ID });
            return response;
        }

        [HttpPost]
        //[ValidateAntiForgeryToken]
        public async Task<IActionResult> edit(int qty,int id)
        {
            /*Check Session */
            var page = "258";
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
            //BankRatio bankRatio = new BankRatio();

            var bankRatio = await _context.BankRatios.FirstOrDefaultAsync(m => m.ID == id);

            //bankRatio.BankName = bank;
            //bankRatio.BankCode = code;
            bankRatio.IssueQty = qty;
            bankRatio.EditBy = HttpContext.Session.GetString("Username");
            bankRatio.EditDate = DateTime.Now;

            _context.Update(bankRatio);
            await _context.SaveChangesAsync();
            return Ok(bankRatio);
        }


        //ซื้อมา
        
        [HttpPost, ActionName("Add2")]
        public IActionResult Add2(string bank, int qty, string code,string etc,string startNo,string endNo,string post)
        {
            var date1 = Convert.ToDateTime(post.Substring(6, 4) + " " + post.Substring(3, 2) + " " + post.Substring(0, 2) + " 00:00:00");


            InventoryCHQ inventory = new InventoryCHQ();

            inventory.BankCode = code;
            inventory.BankName = bank;
            inventory.StartNo = startNo;
            inventory.EndNo = endNo;
            inventory.Qty = qty;
            inventory.PostingDate = date1;
            inventory.CreateBy = HttpContext.Session.GetString("Username");
            inventory.Etc = etc;
            inventory.CreateDate = DateTime.Now;

            _context.InventoryCHQs.Add(inventory);
            _context.SaveChanges();
            return Ok(inventory);
        }
        // POST: UserJob/remov/5
        [HttpPost, ActionName("remove2")]
        public async Task<IActionResult> remove2(int id)
        {

            var inventory = await _context.InventoryCHQs.FindAsync(id);
            _context.InventoryCHQs.RemoveRange(inventory);
            _context.SaveChanges();


            return Ok(inventory);
        }

        public async Task<IActionResult> Edit2(int id)
        {
            IActionResult response = Unauthorized();
            var inventory = await _context.InventoryCHQs.FirstOrDefaultAsync(m => m.ID == id);

            var ID = inventory.ID;
            var name = inventory.BankName;
            var qty = inventory.Qty;
            var code = inventory.BankCode;
            var etc = inventory.Etc;
            var sno = inventory.StartNo;
            var eno = inventory.EndNo;
            var post = inventory.PostingDate.ToString("dd/MM/yyyy", new CultureInfo("en-US"));
            
            
            response = Ok(new { name=name,qty=qty,code=code,etc=etc,sno=sno,eno=eno,post=post,ID=ID });
            return response;
        }

        [HttpPost]
        //[ValidateAntiForgeryToken]
        public async Task<IActionResult> edit2(string etc, string startNo, string endNo, string post,int id,int qty)
        {

            //BankRatio bankRatio = new BankRatio();
            var date1 = Convert.ToDateTime(post.Substring(6, 4) + " " + post.Substring(3, 2) + " " + post.Substring(0, 2) + " 00:00:00");

            var inventory = await _context.InventoryCHQs.FirstOrDefaultAsync(m => m.ID == id);


            inventory.StartNo = startNo;
            inventory.EndNo = endNo;
            inventory.Qty = qty;
            inventory.PostingDate = date1;
           inventory.EditBy = HttpContext.Session.GetString("Username");
            inventory.EditDate = DateTime.Now;
            inventory.Etc = etc;

            _context.Update(inventory);
            await _context.SaveChangesAsync();
            return Ok(inventory);
        }

        //เสีย

       [HttpPost, ActionName("Add3")]
        //public IActionResult Add3(string bank, string code, string etc, string chqNo, string post)a
        public IActionResult Add3(string bank, string code, string etc, string chqNo, string post)
        {
           var date1 = Convert.ToDateTime(post.Substring(6, 4) + " " + post.Substring(3, 2) + " " + post.Substring(0, 2) + " 00:00:00");

            IActionResult response = Unauthorized();
            //var checkInventory = _context.InventoryCHQs.Where(p => p.BankCode == code).ToList();

            var inventory = _context.InventoryCHQs.Where(p => p.BankCode == code).ToList();
            var check = 0;

                       
            foreach (var i in inventory)
            {
                //inventory = new List<InventoryCHQ>();
               var data = inventory.Where(p => Int64.Parse(p.StartNo) <= Int64.Parse(chqNo) && Int64.Parse(p.EndNo) >= Int64.Parse(chqNo)).ToList();

                if (data.Count > 0)
                {
                    check = 1;

                    CHQlose cHQlose = new CHQlose();
                    cHQlose.BankCode = code;
                    cHQlose.BankName = bank;
                    cHQlose.CHQNo = chqNo;
                    cHQlose.PostingDate = date1;
                    cHQlose.CreateBy = HttpContext.Session.GetString("Username");
                    cHQlose.Etc = etc;
                    cHQlose.CreateDate = DateTime.Now;


                    _context.CHQloses.Add(cHQlose);
                    _context.SaveChanges();
                    break;
                }
                else
                {
                    check = 0;
                    //break;
                }
                
                
            }

            response = Ok(new { check = check});

            return response;
        }
        

        // POST: UserJob/remov/5
        [HttpPost, ActionName("remove3")]
        public async Task<IActionResult> remove3(int id)
        {

            var cHQlose = await _context.CHQloses.FindAsync(id);
            _context.CHQloses.RemoveRange(cHQlose);
            _context.SaveChanges();


            return Ok(cHQlose);
        }

        public IActionResult Edit3(int id)
        {
            IActionResult response = Unauthorized();

            var cHQlose = _context.CHQloses.FirstOrDefault(m => m.ID == id);

            var ID = cHQlose.ID;
            var name = cHQlose.BankName;
            var code = cHQlose.BankCode;
            var etc = cHQlose.Etc;
            var chqNo = cHQlose.CHQNo;
            var post = cHQlose.PostingDate.ToString("dd/MM/yyyy", new CultureInfo("en-US"));


            response = Ok(new { name = name, chqNo = chqNo, code = code, etc = etc, post = post, ID = ID });
            return response;
        }

        [HttpPost]
        //[ValidateAntiForgeryToken]
        public IActionResult edit3(string etc, string chqNo, string post, int id,string code)
        {
            IActionResult response = Unauthorized();
            //BankRatio bankRatio = new BankRatio();
            var date1 = Convert.ToDateTime(post.Substring(6, 4) + " " + post.Substring(3, 2) + " " + post.Substring(0, 2) + " 00:00:00");


                var cHQlose = _context.CHQloses.FirstOrDefault(m => m.ID == id);
                cHQlose.CHQNo = chqNo;
                cHQlose.PostingDate = date1;
                cHQlose.EditBy = HttpContext.Session.GetString("Username");
                cHQlose.Etc = etc;
                cHQlose.EditDate = DateTime.Now;

                _context.Update(cHQlose);
                _context.SaveChanges();
         
            return Ok(cHQlose);
        }

        public IActionResult GetRetroi()
        {
            IActionResult response = Unauthorized();

            var bankRatios = _context.BankRatios.ToList();

            response = Ok(new { data = bankRatios});
            return response;
        }
        public IActionResult GetInventory(string name)
        {
            IActionResult response = Unauthorized();

            var inventories = new List<InventoryCHQ>();
            if (name == "All")
            {
                inventories = _context.InventoryCHQs.ToList();
            }
            else
            {
                inventories = _context.InventoryCHQs.Where(p => p.BankCode == name).ToList();
            }


            response = Ok(new { data = inventories });
            return response;
        }
        public IActionResult GetCHQfail(string name)
        {
            IActionResult response = Unauthorized();


            var cHQloses = new List<CHQlose>();
            if (name == "All")
            {
                cHQloses = _context.CHQloses.ToList();
            }
            else
            {
                cHQloses = _context.CHQloses.Where(p => p.BankCode == name).ToList();
            }


            response = Ok(new { data = cHQloses });
            return response;
        }

        //data chart and table
        public IActionResult GenData(string Startdate, string EndDate)
        {


            var date1 = Startdate.Substring(6, 4) + "-" + Startdate.Substring(3, 2) + "-" + Startdate.Substring(0, 2) + " 00:00:00";
            var date2 = EndDate.Substring(6, 4) + "-" + EndDate.Substring(3, 2) + "-" + EndDate.Substring(0, 2) + " 23:59:59";



            var date3 = Startdate.Substring(6, 4) + "-01-01 00:00:00";
            var date4 = EndDate.Substring(6, 4) + "-" + EndDate.Substring(3, 2) + "-" + EndDate.Substring(0, 2) + " 23:59:59";

            var ddate3 = Convert.ToDateTime(date3);
            var ddate4 = Convert.ToDateTime(date4);

            var sdate1 = Startdate;
            var sdate2 = EndDate;
            var rdate1 = Startdate;
            var rdate2 = EndDate;
            var startdate = Convert.ToDateTime(date1);
            var enddate = Convert.ToDateTime(date2);


            IActionResult response = Unauthorized();

            var query = "SELECT ROW_NUMBER() OVER (ORDER BY a.PostingDate) AS ID, * , CASE  " +
                " WHEN a.MonthDate='01' THEN 'มกราคม' " +
                "WHEN a.MonthDate='02' THEN 'กุมภาพันธ์'  " +
                "WHEN a.MonthDate='03' THEN 'มีนาคม'  " +
                "WHEN a.MonthDate='04' THEN 'เมษายน'  " +
                "WHEN a.MonthDate='05' THEN 'พฤษภาคม'  " +
                "WHEN a.MonthDate='06' THEN 'มิถุนายน'  " +
                "WHEN a.MonthDate='07' THEN 'กรกฏาคม'  " +
                "WHEN a.MonthDate='08' THEN 'สิงหาคม'  " +
                "WHEN a.MonthDate='09' THEN 'กันยายน'  " +
                "WHEN a.MonthDate='10' THEN 'ตุลาคม'  " +
                "WHEN a.MonthDate='11' THEN 'พฤศจิกายน'  " +
                "WHEN a.MonthDate='12' THEN 'ธันวาคม'  " +
                "ELSE '' END as MonthTH  " +
                "FROM ( " +
                "SELECT " +
                "CONVERT(varchar,YEAR(dbo."+ Environment.GetEnvironmentVariable("Company") +"Bank Account Ledger Entry].[Posting Date]) +543) as YearData, " +
                "CASE WHEN Month(dbo."+ Environment.GetEnvironmentVariable("Company") +"Bank Account Ledger Entry].[Posting Date])<'10' THEN CONCAT('0',Month(dbo."+ Environment.GetEnvironmentVariable("Company") +"Bank Account Ledger Entry].[Posting Date])) " +
                "ELSE CONVERT(VARCHAR,Month(dbo."+ Environment.GetEnvironmentVariable("Company") +"Bank Account Ledger Entry].[Posting Date])) END as MonthDate, " +
                "dbo."+ Environment.GetEnvironmentVariable("Company") +"Bank Account Ledger Entry].[Posting Date] as PostingDate, " +
                "dbo."+ Environment.GetEnvironmentVariable("Company") +"Bank Account Ledger Entry].[Bank Account No_] as BankCode, " +
                "dbo."+ Environment.GetEnvironmentVariable("Company") +"Bank Account Ledger Entry].[External Document No_] as CHQNo " +
                "FROM " +
                "dbo."+ Environment.GetEnvironmentVariable("Company") +"Bank Account Ledger Entry] " +
                "WHERE dbo."+ Environment.GetEnvironmentVariable("Company") +"Bank Account Ledger Entry].[Print Check]=1 " +
                ")as a";

            var CHQCurrents = _navcontext.CHQCurrents.FromSqlRaw(query).ToList();


            var bankRatios = _context.BankRatios.OrderBy(p=>p.BankName).ToList();
            var CHQloses = _context.CHQloses.ToList();
            var inventoryCHQs = _context.InventoryCHQs.ToList();



            List<CHQInventoryReport> cHQInventoryReports = new List<CHQInventoryReport>();
            CHQInventoryReport cHQInventoryReport;
            var i = 1;
            var usechqCurrent = 0;
            var usechqOld = 0;
            var losechqCurrent = 0;
            var losechqOld = 0;
            var stockchq = 0;
            var buychq = 0;
            var totalchq = 0;
            decimal total = 0.00M;
            var usingSum = 0;

            foreach (var std in bankRatios)
            {
                cHQInventoryReport = new CHQInventoryReport();
                cHQInventoryReport.ID = i;
                cHQInventoryReport.BankCode = std.BankCode;
                cHQInventoryReport.BankName = std.BankName;
                cHQInventoryReport.QTYOfOne = 1;
                cHQInventoryReport.BankIssueQty = std.IssueQty;

                usechqCurrent = CHQCurrents.Where(p => p.BankCode == std.BankCode && p.PostingDate >= startdate && p.PostingDate <= enddate).Count(); //CHQ ใช้ระหว่างเดือน
                usechqOld = CHQCurrents.Where(p => p.BankCode == std.BankCode && p.PostingDate < startdate).Count(); //CHQ ใช้ Opening


                losechqCurrent = CHQloses.Where(p => p.BankCode == std.BankCode && p.PostingDate >= startdate && p.PostingDate <= enddate).Count(); //CHQ เสียระหว่างเดือน
                losechqOld = CHQloses.Where(p => p.BankCode == std.BankCode && p.PostingDate < startdate).Count(); //CHQ เสีย Openning



                stockchq = inventoryCHQs.Where(p => p.BankCode == std.BankCode && p.PostingDate < startdate).Sum(c => c.Qty); //ปริมาณใน Stokk
                buychq = inventoryCHQs.Where(p => p.BankCode == std.BankCode && p.PostingDate >= startdate && p.PostingDate <= enddate).Sum(c => c.Qty); //ปริมาณซื้อเพิ่มระหว่างเดือน


                usingSum = (CHQCurrents.Where(p => p.BankCode == std.BankCode && p.PostingDate >= ddate3 && p.PostingDate <= ddate4).Count()) + CHQloses.Where(p => p.BankCode == std.BankCode && p.PostingDate >= ddate3  && p.PostingDate <=ddate4).Count();

                totalchq = (stockchq + buychq) - (usechqCurrent + usechqOld + losechqCurrent + losechqOld);



                total = totalchq / std.IssueQty;
                cHQInventoryReport.CHQInventoryHead = Decimal.ToInt32(Math.Floor(total)); //ทำการหารตัดเศษ จำนวนเล่ม
                cHQInventoryReport.CHQInventorySub = totalchq % std.IssueQty;

                cHQInventoryReport.CHQInventoryTotal = totalchq;

                cHQInventoryReport.BuyInPeriod = buychq;
                cHQInventoryReport.TotalUseInPeriod = usechqCurrent + losechqCurrent;

                cHQInventoryReport.TotalInstock = totalchq;
                cHQInventoryReport.TotalUsing = usingSum;
                i++;

                cHQInventoryReports.Add(cHQInventoryReport);
            }




            response = Ok(new { data = cHQInventoryReports });

            return response;
        }
    }
}