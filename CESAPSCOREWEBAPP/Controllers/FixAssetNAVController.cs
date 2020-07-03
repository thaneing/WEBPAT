using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;
using CESAPSCOREWEBAPP.Helpers;
using CESAPSCOREWEBAPP.Models;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using static CESAPSCOREWEBAPP.Models.Enums;

namespace CESAPSCOREWEBAPP.Controllers
{
    public class FixAssetNAVController : BaseController
    {
        private readonly DatabaseContext _context;
        private readonly NAVContext _navcontext;


        private const string XlsxContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
        private readonly IHostingEnvironment _hostingEnvironment;


        public FixAssetNAVController(DatabaseContext context, NAVContext navcontext, IHostingEnvironment hostingEnvironment)
        {
            _context = context;
            _navcontext = navcontext;
            _hostingEnvironment = hostingEnvironment;
        }

        public IActionResult Index()
        {

            /*Check Session */
            var page = "179";
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

            ViewData["JobNo"] = new SelectList(_context.UserJobs.Where(s => s.UserId == HttpContext.Session.GetInt32("Userid")).OrderBy(s => s.UserJobDetail).ToList(), "UserJobDetail", "UserJobDetail");
           

            return View();
        }
        public async Task<IActionResult> UpdateDataAsync()
        {
            /*Check Session */
            var page = "179";
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


            IActionResult response = Unauthorized();
            var sql = "SELECT DISTINCT a.FixAccNo,a.Description,a.Description2,a.FALocation, CONCAT('<button data-animal-type=''',a.RefPC,''' data-item-type=''', a.RefPCDetail ,''' onclick=''selectDetail(this)'' class=''btn-link''><u>',a.RefPC , '</u></button>') as RefPC,a.RefPCDetail,(SELECT Sum(dbo."+ Environment.GetEnvironmentVariable("Company") +"FA Ledger Entry].Qty_) FROM [dbo]."+ Environment.GetEnvironmentVariable("Company") +"FA Ledger Entry] WHERE [FA No_]=a.FixAccNo AND [FA Posting Type]=0 GROUP BY [FA No_]) as FAQty," +
                " CONCAT('<button data-fixaccno-type=''',a.FixAccNo,''' data-fixaccessdetail-type=''', a.Description ,''' data-fixaccessdetail2-type=''',a.Description2,''' data-qty-type=''',(SELECT Sum(dbo."+ Environment.GetEnvironmentVariable("Company") +"FA Ledger Entry].Qty_) FROM [dbo]."+ Environment.GetEnvironmentVariable("Company") +"FA Ledger Entry] WHERE [FA No_]=a.FixAccNo AND [FA Posting Type]=0 GROUP BY [FA No_]),'''  onclick=''selectData(this)'' class=''btn btn-sm btn-info''><u>Action</u></button>') as ActionData " +
                " FROM(SELECT dbo.FA_card.No_ As FixAccNo,dbo.FA_card.Description as Description,dbo.FA_card.[Description 2] as Description2,dbo.FA_card.[Refer Item FA No_] as RefPC ,dbo.FA_card.[Refer Item FA Description] as RefPCDetail,dbo.FA_card.[FA Location Code] as FALocation FROM dbo.FA_card) as a";
            //var sql = "SELECT a.FixAccNo,a.FALocation,a.Description,a.Description2, CONCAT('<button data-animal-type=''' , a.RefPC, ''' data-itemname-type=''' ,a.RefPCDetail , ''' data-date1-type=''' , a.RefPC, ''' onclick=''selectDetail(this)'' class=''btn btn-link''><u>' , a.RefPC , '</u></button>') as RefPC,a.RefPCDetail, (SELECT Sum(dbo."+ Environment.GetEnvironmentVariable("Company") +"FA Ledger Entry].Qty_) FROM [dbo]."+ Environment.GetEnvironmentVariable("Company") +"FA Ledger Entry] WHERE [FA No_]=a.FixAccNo AND [FA Posting Type]=0 GROUP BY [FA No_]) as FAQty FROM(SELECT dbo.FA_card.No_ As FixAccNo,dbo.FA_card.Description as Description,dbo.FA_card.[Description 2] as Description2,dbo.FA_card.[Refer Item FA No_] as RefPC ,dbo.FA_card.[Refer Item FA Description] as RefPCDetail,dbo.FA_card.[FA Location Code] as FALocation FROM dbo.FA_card) as a";
            var FANav = _navcontext.fixAccessDatas.FromSqlRaw(sql).ToList();
            //var checkdata = new FixAccessDataTmp();
            //var fixAccessDataTmp = new FixAccessDataTmp();


            foreach (var std in FANav as IList<FixAccessData>)
            {

                var checkdata = _context.fixAccessDataTmps.Where(a => a.FixAccNo == std.FixAccNo).ToList();

                if (checkdata.Count() == 0)
                {
                    var fixAccessDataTmp = new FixAccessDataTmp
                    {
                        FixAssId=0,
                        FixAccNo = std.FixAccNo,
                        Description = std.Description,
                        Description2 = std.Description2,
                        FALocation = std.FALocation,
                        RefPC = std.RefPC,
                        RefPCDetail = std.RefPCDetail,
                        FAQty = std.FAQty,
                        LastModifi = null,
                        FATransfer = 0,
                        ActionData=std.ActionData

                    };
                    _context.Add(fixAccessDataTmp);
                    await _context.SaveChangesAsync();

                }
                else
                {
                    foreach (var std1 in checkdata as IList<FixAccessDataTmp>)
                    {

                        if (std.FixAccNo == std1.FixAccNo)
                        {


                            std1.FixAccNo = std.FixAccNo;
                            std1.Description = std.Description;
                            std1.Description2 = std.Description2;
                            std1.FALocation = std.FALocation;
                            std1.RefPC = std.RefPC;
                            std1.RefPCDetail = std.RefPCDetail;
                            std1.FAQty = std.FAQty;
         
                        

                            try
                            {


                                _context.Update(std1);
                                await _context.SaveChangesAsync();
                            }
                            catch (DbUpdateConcurrencyException)
                            {

                            }
                        }
                    }
                }


            }



            var fixAccessDataAll = _context.fixAccessDataTmps.ToList();

            response = Ok(new { data = fixAccessDataAll });
            return response;
            //return View();
        }




        [HttpGet]
        public IActionResult GetFixAccess()
        {

            IActionResult response = Unauthorized();
            var fixAccessDataAll = _context.fixAccessDataTmps.ToList();

            response = Ok(new { data = fixAccessDataAll });
            return response;
   
        }





        [HttpGet]
        public IActionResult ItemBySite(string itemno)
        {


            IActionResult response = Unauthorized();
            var query = " SELECT DISTINCT dbo."+ Environment.GetEnvironmentVariable("Company") + "Item Ledger Entry].[Location Code] AS LocationCode,ROW_NUMBER() OVER (ORDER BY dbo." + Environment.GetEnvironmentVariable("Company") + "Item Ledger Entry].[Location Code]) as ID,convert(varchar,FORMAT(sum(dbo." + Environment.GetEnvironmentVariable("Company") +"Item Ledger Entry].Quantity),'###,###,###.00','en-US')) as Total FROM dbo."+ Environment.GetEnvironmentVariable("Company") +"Item Ledger Entry] "
                     + "LEFT JOIN dbo."+ Environment.GetEnvironmentVariable("Company") +"Item] ON dbo."+ Environment.GetEnvironmentVariable("Company") +"Item Ledger Entry].[Item No_] = dbo."+ Environment.GetEnvironmentVariable("Company") +"Item].No_ "
                     + " WHERE  dbo."+ Environment.GetEnvironmentVariable("Company") +"Item Ledger Entry].[Posting Date]>='2018-10-23 00:00:00' and dbo."+ Environment.GetEnvironmentVariable("Company") +"Item Ledger Entry].[Posting Date]<=GETDATE() and [Item No_]={0} "
                     + " GROUP BY dbo."+ Environment.GetEnvironmentVariable("Company") +"Item Ledger Entry].[Location Code] HAVING sum(dbo."+ Environment.GetEnvironmentVariable("Company") +"Item Ledger Entry].Quantity)<>0 ";

            //SqlParameter parameterItem = new SqlParameter("@item", itemno);





            //ViewBag.sql = queryData;
            var itemBySites = _navcontext.ItemBySites.FromSqlRaw(query, itemno).ToList();

            response = Ok(new { data = itemBySites });


            return response;
        }





        [HttpGet]
        public IActionResult GenDetailItem(string id)
        {
            IActionResult response = Unauthorized();
            var fixAccessDataAll = _context.tranSectionFixAsset.Where(p=>p.FixAccNo==id).ToList();
            var sumdata = _context.tranSectionFixAsset.Where(p => p.FixAccNo == id).Sum(p => p.FixAssetQty);

            response = Ok(new { data = fixAccessDataAll,sumdata=sumdata });
            return response;

        }








        [HttpGet]
        public async Task<IActionResult> TransectionData(string FixAccNo,string FixAssetName,string FixAssetName1,string SiteName,int qty,string TransectionType,string TransectionEtc)
        {
            IActionResult response = Unauthorized();


            TranSectionFixAsset tran  = new TranSectionFixAsset();
           var now= DateTime.Now;
            tran.FixAccNo = FixAccNo;
            tran.FixAssetQty = qty;
            tran.FixAssetItem = FixAssetName;
            tran.FixAssetItem2 = FixAssetName1;
            tran.site = SiteName;
            tran.TransectionDate = now;
            tran.ActionData= "<button data-fixaccno-type='"+ FixAccNo + "' data-qty-type='"+ qty + "' data-site-type='"+ SiteName + "' onclick='reclassdata(this)' class='btn btn-info btn-sm'>Move</button>";
            tran.TransectionType = TransectionType;
            tran.TransectionBy = HttpContext.Session.GetString("Username");
            tran.TransectionEtc = TransectionEtc;

             _context.tranSectionFixAsset.Add(tran);
             await _context.SaveChangesAsync();
            var username = HttpContext.Session.GetString("Username");



            var editdata = _context.tranSectionFixAsset.Where(a => a.FixAccNo == FixAccNo && a.TransectionBy== username && a.TransectionEtc == TransectionEtc  && a.FixAssetQty == qty && a.site==SiteName && a.TransectionDate==now).ToList();
            try
            {
                TranSectionFixAsset tranedit = new TranSectionFixAsset();
                if (editdata != null)
                {
                    foreach (var data in editdata as IList<TranSectionFixAsset>)
                    {
                        tranedit = data;
                        tranedit.ActionData = "<button data-dataId-type='" + data.TranSectionFixAssetId + "'  data-fixaccno-type='" + FixAccNo + "' data-qty-type='" + qty + "' data-site-type='" + SiteName + "' onclick='reclassdata(this)' class='btn btn-info btn-sm'>Move</button>";
                    }
                    _context.Update(tranedit);
                    await _context.SaveChangesAsync();
           
                }

            }
            catch
            {

            }
            



            var sumdata=_context.tranSectionFixAsset.Where(c => c.FixAccNo == FixAccNo).Sum(p => p.FixAssetQty);
            var checkdata = _context.fixAccessDataTmps.Where(a => a.FixAccNo == FixAccNo).ToList();
            foreach (var std1 in checkdata as IList<FixAccessDataTmp>)
            {

                if (FixAccNo == std1.FixAccNo)
                {

                    std1.FATransfer = sumdata;
                    std1.LastModifi = DateTime.Now;
        

                    try
                    {
                        _context.Update(std1);
                        await _context.SaveChangesAsync();
                    }
                    catch (DbUpdateConcurrencyException)
                    {

                    }
                }
            }

            return Ok(tran);


        }



        [HttpGet]
        public IActionResult TransferBysite(int id,string site, string item,decimal qty)
        {
            IActionResult response = Unauthorized();

            TranSectionFixAsset tran = new TranSectionFixAsset();
            var checkdata = _context.tranSectionFixAsset.Where(a => a.TranSectionFixAssetId==id&&a.FixAccNo == item &&a.site==site && a.FixAssetQty==qty).ToList();
            if (checkdata == null)
            {

            }
            else {
                foreach(var std in checkdata as IList<TranSectionFixAsset>)
                {
                    tran = std;
                }

            }

                return Ok(tran);


        }

        [HttpGet]
        public async Task<IActionResult> TransferBysiteConfirm(int id, string site, decimal qty,string etc,string selectType)
        {
            IActionResult response = Unauthorized();

            
            var transectionold = await _context.tranSectionFixAsset.Where(a => a.TranSectionFixAssetId==id).FirstAsync();

            TranSectionFixAsset transectionNew =new TranSectionFixAsset();
            if (transectionold == null)
            {

            }
            else
            {
                var now = DateTime.Now;

                transectionNew.FixAccNo = transectionold.FixAccNo;
                transectionNew.FixAssetQty = qty;
                transectionNew.FixAssetItem = transectionold.FixAssetItem;
                transectionNew.FixAssetItem2 = transectionold.FixAssetItem2;
                transectionNew.site = site;
                transectionNew.TransectionDate = now;
                transectionNew.ActionData = "<button data-fixaccno-type='" + transectionold.FixAccNo + "' data-qty-type='" + qty + "' data-site-type='" + site + "' onclick='reclassdata(this)' class='btn btn-info btn-sm'>Move</button>";
                transectionNew.TransectionType = selectType;
                transectionNew.TransectionBy = HttpContext.Session.GetString("Username");
                transectionNew.TransectionEtc = etc;



                try
                {
                    transectionold.ActionData="<button data-dataId-type='" + transectionold.TranSectionFixAssetId + "'  data-fixaccno-type='" + transectionold.FixAccNo + "' data-qty-type='" + (transectionold.FixAssetQty - qty) + "' data-site-type='" + transectionold.site + "' onclick='reclassdata(this)' class='btn btn-info btn-sm'>Move</button>";
                    transectionold.FixAssetQty = transectionold.FixAssetQty - qty;
                    _context.Update(transectionold);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {

                }

                try
                {
                    _context.tranSectionFixAsset.Add(transectionNew);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {

                }


                var username= HttpContext.Session.GetString("Username");

                var editdata = _context.tranSectionFixAsset.Where(a => a.FixAccNo == transectionNew.FixAccNo && a.TransectionBy == username && a.TransectionEtc == etc && a.FixAssetQty == qty && a.site == site && a.TransectionDate == now).ToList();
                try
                {
                    TranSectionFixAsset tranedit = new TranSectionFixAsset();
                    if (editdata != null)
                    {
                        foreach (var data in editdata as IList<TranSectionFixAsset>)
                        {
                            tranedit = data;
                            tranedit.ActionData = "<button data-dataId-type='" + data.TranSectionFixAssetId + "'  data-fixaccno-type='" + data.FixAccNo + "' data-qty-type='" + qty + "' data-site-type='" + data.site + "' onclick='reclassdata(this)' class='btn btn-info btn-sm'>Move</button>";
                        }
                        _context.Update(tranedit);
                        await _context.SaveChangesAsync();

                    }

                }
                catch
                {

                }



            }

       

          

            return Ok();


        }





        public IActionResult Report()
        {
            /*Check Session */
            var page = "184";
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
        public IActionResult GenReportTable()
        {
            IActionResult response = Unauthorized();
            var fixAccessDataAll = _context.tranSectionFixAsset.Where(p=>p.FixAssetQty!=0).ToList();

            response = Ok(new { data = fixAccessDataAll });
            return response;

        }



        [HttpGet]
        public IActionResult GenReportPercen()
        {
            IActionResult response = Unauthorized();
            var total = _context.fixAccessDataTmps.Sum(p => p.FAQty);
            var actuan = _context.tranSectionFixAsset.Sum(p=>p.FixAssetQty);

            var percen = Math.Round(((actuan / total) * 100),2);
            var percentotal = 100 - percen;
            List<DataXYDecimal> instances = new List<DataXYDecimal>();
            DataXYDecimal current = null;


            current = new DataXYDecimal();
            current.X = "Process";
            current.Y = percen;
            instances.Add(current);

            current = new DataXYDecimal();
            current.X = "UnProcess";
            current.Y = percentotal;
            instances.Add(current);



            response = Ok(instances);

            return response;

        }


        public IActionResult ReportFixAsset()
        {
            /*Check Session */
            var page = "184";
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

            var queryData = "SELECT dbo."+ Environment.GetEnvironmentVariable("Company") +"Location].Code AS name,dbo."+ Environment.GetEnvironmentVariable("Company") +"Location].Name as code FROM [dbo]."+ Environment.GetEnvironmentVariable("Company") +"Location] order by dbo."+ Environment.GetEnvironmentVariable("Company") +"Location].Code asc ";
            var sourceAutoCompletes = _navcontext.sourceAutoCompletes.FromSqlRaw(queryData).ToList();
            ViewData["SourceAutoCompletes"] = sourceAutoCompletes;

            ViewBag.StartDate= DateTime.Now.ToString("dd-MM-yyyy", new CultureInfo("en-US"));

            ViewBag.StartEnd = DateTime.Now.ToString("23-10-2018", new CultureInfo("en-US"));

            ViewData["JobNo"] = new SelectList(_context.UserJobs.Where(s => s.UserId == HttpContext.Session.GetInt32("Userid")).OrderBy(s => s.UserJobDetail).ToList(), "UserJobDetail", "UserJobDetail");


            return View();
        }



        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ReportFixAsset(string date1,string date2, string site,string checkdata)
        {
            /*Check Session */
            var page = "184";
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

            var queryData1 = "SELECT dbo."+ Environment.GetEnvironmentVariable("Company") +"Location].Code AS name,dbo."+ Environment.GetEnvironmentVariable("Company") +"Location].Name as code FROM [dbo]."+ Environment.GetEnvironmentVariable("Company") +"Location] order by dbo."+ Environment.GetEnvironmentVariable("Company") +"Location].Code asc ";
            var sourceAutoCompletes = _navcontext.sourceAutoCompletes.FromSqlRaw(queryData1).ToList();
            ViewData["SourceAutoCompletes"] = sourceAutoCompletes;


            ViewBag.StartDate = DateTime.Now.ToString("dd-MM-yyyy", new CultureInfo("en-US"));
     

            ViewData["JobNo"] = new SelectList(_context.UserJobs.Where(s => s.UserId == HttpContext.Session.GetInt32("Userid")).OrderBy(s => s.UserJobDetail).ToList(), "UserJobDetail", "UserJobDetail");

            var StartDate = date1.Substring(6, 4) + "-" + date1.Substring(3, 2) + "-" + date1.Substring(0, 2) + " 23:59:59";
            var DateEnd = date2.Substring(6, 4) + "-" + date2.Substring(3, 2) + "-" + date2.Substring(0, 2) + " 00:00:00";

            ViewBag.StartEnd = date2.Substring(0, 2) + "/" + date2.Substring(3, 2) + "/" + date2.Substring(6, 4);
            ViewBag.StartDate = date1.Substring(0, 2)+"/"+ date1.Substring(3, 2)+"/"+ date1.Substring(6, 4);


            var queryData = "SELECT b.ItemNo,b.Description,b.LocationCode,b.Quantity,ROW_NUMBER() OVER (ORDER BY b.ItemNo) AS ListNo,'' as Detail,'' as Status,'' as Status1,'' as Status2,'' as Curren,'' as Etc " +
                " FROM( SELECT a.ItemNo,a.Description,a.LocationCode," +
                "	(select sum(dbo."+ Environment.GetEnvironmentVariable("Company") +"Item Ledger Entry].Quantity) from dbo."+ Environment.GetEnvironmentVariable("Company") +"Item Ledger Entry]" +
                "	 WHERE [Location Code] = a.LocationCode and [Item No_] = a.ItemNo AND dbo."+ Environment.GetEnvironmentVariable("Company") +"Item Ledger Entry].[Posting Date]>={1} " +
                " 		and dbo."+ Environment.GetEnvironmentVariable("Company") +"Item Ledger Entry].[Posting Date]<={0}) as Quantity" +
                " 	FROM( SELECT DISTINCT " +
                " 	dbo."+ Environment.GetEnvironmentVariable("Company") +"Item Ledger Entry].[Item No_] AS ItemNo," +
                " 	dbo."+ Environment.GetEnvironmentVariable("Company") +"Item].Description AS Description," +
                " 	dbo."+ Environment.GetEnvironmentVariable("Company") +"Item Ledger Entry].[Location Code] AS LocationCode" +
                " 	FROM dbo."+ Environment.GetEnvironmentVariable("Company") +"Item Ledger Entry]" +
                " 	LEFT JOIN dbo."+ Environment.GetEnvironmentVariable("Company") +"Item] ON dbo."+ Environment.GetEnvironmentVariable("Company") +"Item Ledger Entry].[Item No_] = dbo."+ Environment.GetEnvironmentVariable("Company") +"Item].No_" +
                " 	WHERE  dbo."+ Environment.GetEnvironmentVariable("Company") + "Item Ledger Entry].[Posting Date]>={1} and dbo." + Environment.GetEnvironmentVariable("Company") +"Item Ledger Entry].[Posting Date]<={0} " +
                //"  and [Item No_] LIKE 'PC%'" +
                " 	) as a" +
                " ) as b  WHERE b.Quantity<>0 and b.LocationCode={2}   Order by b.ItemNo asc";


            //SqlParameter parameterDate = new SqlParameter("@date1", StartDate);
            //SqlParameter parameterDate2 = new SqlParameter("@DateEnd", DateEnd);
            //SqlParameter parameterSite = new SqlParameter("@site", site.ToUpper());


            var vfixAssetLists = await _navcontext.fixAssetLists.FromSqlRaw(queryData, StartDate, DateEnd, site.ToUpper()).ToListAsync();
            ViewBag.Site = site.ToUpper();


            var table = "<table id='example' class='table table-striped table-bordered table-hover dataTables-example' style='width:100%'>" +
                "<thead><tr>" +
                "<th align='center'>รหัส</th>" +
                "<th align='center'>รหัสสืนค้า</th>" +
                "<th align='center'>ชื่อรายการ</th>" +
                "<th align='center'>รายละเอียดเพิ่มเติม</th>" +
                "<th align='center'>หน่วยงาน</th>" +
                "<th align='center'>จำนวนทั้งหมด</th>" +
                "<th align='center'>จำนวนที่พบ</th>" +
                "<th align='center'>ของดี</th>" +
                "<th align='center'>ของเสีย</th>" +
                "<th align='center'>หมายเหตุ</th></tr></thead><tbody>";
            //Item Code ที่ยกเว้นการ Flash array
            string[] Itemcodes = { "PC720100",
                "PC651500",
                "PC670800",
                "PC610200",
                "PC651400",
                "PC610400",
                "PC620100",
                "PC640100",
                "PC687000",
                "PC610700",
                "PC640200",
                "PC601500",
                "PC651100",
                "PC671100",
                "PC700100",
                "PC671000",
                "PC610100",
                "PC600100",
                "PC610800",
                "PC600200",
                "PC601600",
                "PC650400",
                "PC601700",
                "PC651600",
                "PC612000",
                "PC640400",
                "PC651000",
                "PC650200",
                "PC650900",
                "PC650100",
                "PC685000",
                "PC681000",
                "PC610600",
                "PC650500",
                "PC688000",
                "PC750900",
                "PC610500",
                "PC652900",
                "PC650700",
                "PC650800",
                "PC740700",
                "PC650600",
                "PC651300",
                "PC610300",
                "PC690600",
                "PC600400",
                "PC741000",
                "PC741400",
                "PC601200",
                "PC601100",
                "PC601400",
                "PC601300",
                "PC740400",
                "PC683000",
                "PC651200",
                "PC691000",
                "PC600500",
                "PC650300",
                "PC640510",
                "PC600300",
                "PC661300",
                "PC661600",
                "PC751300",
                "PC740500",
                "PC690300",
                "PC661800",
                "PC740100",
                "PC740600",
                "PC741200",
                "PC710900",
                "PC740200",
                "PC690700",
                "PC740300",
                "PC741100",
                "PC740900","PC741300",
                "PC691100","PC691400",
                "PC740800","PC752100","PC750200", "PC751400", "PC752800", "PC661700","PC750400","PC661100", "PC753900", "PC840400","PC600600",
            "PC640300","PC652800","PC652300","PC652100","PC652000","PC651700",
            "PC640520","PC751000","PC750800","PC750700","PC750600","PC750500","PC750300","PC750100","PC741500","PC740600","PC720300","PC720200",
            "PC651800","PC752000","PC751900","PC751800","PC751700","PC751600","PC751500","PC751200","PC751100","PC711000","PC710700","PC691500","PC690100","PC670300","PC652700","PC652500",
            "PC600700","PC600800", "PC620500","PC651900","PC652400", "PC652600", "PC670900", "PC69010","PC752600","PC752500","PC752400","PC752300","PC752200","PC752100",
            "PC690500","PC691600","PC753800","PC753700","PC753600","PC753400","PC753500","PC753300","PC753200","PC753100","PC753000","PC752900","PC752800","PC752700"

            };

            Array.Sort<string>(Itemcodes, new Comparison<string>(
                 (i1, i2) => i2.CompareTo(i1)));
            var k = 0;
            var j = 0;
            var check = 0;
            var no = 1;
            foreach (var std in vfixAssetLists as IList<FixAssetList>)
            {

                if (checkdata =="on")
                {
                    if (std.ItemNo.Substring(0, 2) == "PC")
                    {
                        for (j = 0; j < Itemcodes.Length; j++)
                        {
                            if (std.ItemNo == Itemcodes[j])
                            {
                                table += "<tr><td>"+ no + "</td>" +
                               "<td>" + std.ItemNo + "</td>" +
                               "<td>" + std.Description + "</td>" +
                               "<td>" + std.Detail + "</td>" +
                               "<td>" + std.LocationCode + "</td>" +
                               "<td style='text-align:right'>" + std.Quantity.ToString("##,###.00") + "</td>" +
                               "<td>" + std.Status + "</td>" +
                               "<td>" + std.Status1 + "</td>" +
                               "<td>" + std.Status2 + "</td>" +
                               "<td>" + std.Etc + "</td></tr>";
                                j = Itemcodes.Length;
                                check = 1;
                                no = no + 1;
                            }
                            else
                            {
                                check = 0;
                            }
                        }
                        if (check == 0)
                        {

                            for (var i = 0; i < std.Quantity; i++)
                            {
                                table += "<tr><td>"+no+"</td>" +
                                    "<td>" + std.ItemNo + "</td>" +
                                    "<td>" + std.Description + "</td>" +
                                    "<td>" + std.Detail + "</td>" +
                                    "<td>" + std.LocationCode + "</td>" +
                                    "<td style='text-align:right'>" + "1.00" + "</td>" +
                                    "<td>" + std.Status + "</td>" +
                                    "<td>" + std.Status1 + "</td>" +
                                    "<td>" + std.Status2 + "</td>" +
                                    "<td>" + std.Etc + "</td></tr>";
                                no = no + 1;
                            }
                        }

                    }
                    else
                    {
                        table += "<tr><td>"+no+"</td>" +
                              "<td>" + std.ItemNo + "</td>" +
                              "<td>" + std.Description + "</td>" +
                              "<td>" + std.Detail + "</td>" +
                              "<td>" + std.LocationCode + "</td>" +
                              "<td style='text-align:right'>" + std.Quantity.ToString("##,###.00") + "</td>" +
                              "<td>" + std.Status + "</td>" +
                              "<td>" + std.Status1 + "</td>" +
                              "<td>" + std.Status2 + "</td>" +
                              "<td>" + std.Etc + "</td></tr>";
                              no = no + 1;
                    }
                }
                else
                {
                    table += "<tr><td>" + no + "</td>" +
                        "<td>" + std.ItemNo + "</td>" +
                        "<td>" + std.Description + "</td>" +
                        "<td>" + std.Detail + "</td>" +
                        "<td>" + std.LocationCode + "</td>" +
                        "<td style='text-align:right'>" + std.Quantity.ToString("##,###.00") + "</td>" +
                        "<td>" + std.Status + "</td>" +
                        "<td>" + std.Status1 + "</td>" +
                        "<td>" + std.Status2 + "</td>" +
                        "<td>" + std.Etc + "</td></tr>";
                    no = no + 1;
                }
            }

            table += "</tbody></table>";
            ViewBag.Table = table;
            ViewBag.Check = checkdata;

            return View();
        }

        public IActionResult ReportDiffPages()
        {
            /*Check Session */
            var page = "184";
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


            ViewBag.StartDate = DateTime.Now.ToString("dd-MM-yyyy", new CultureInfo("en-US"));
            ViewBag.EndDate = DateTime.Now.ToString("dd-MM-yyyy", new CultureInfo("en-US"));
            var queryData = "SELECT dbo." + Environment.GetEnvironmentVariable("Company") + "Location].Code AS name,dbo." + Environment.GetEnvironmentVariable("Company") + "Location].Name as code FROM [dbo]." + Environment.GetEnvironmentVariable("Company") + "Location] order by dbo." + Environment.GetEnvironmentVariable("Company") + "Location].Code asc ";
            var sourceAutoCompletes = _navcontext.sourceAutoCompletes.FromSqlRaw(queryData).ToList();
            ViewData["SourceAutoCompletes"] = sourceAutoCompletes;
            return View();

        }



            public async Task<IActionResult> ReportDiff(string date1,string date2 , string site)
        {
            IActionResult response = Unauthorized();
            var StartDate = date1.Substring(6, 4) + "-" + date1.Substring(3, 2) + "-" + date1.Substring(0, 2) + " 00:00:00";
            var EndDate = date2.Substring(6, 4) + "-" + date2.Substring(3, 2) + "-" + date2.Substring(0, 2) + " 23:59:59";

            var queryData = "SELECT ROW_NUMBER() OVER (ORDER BY b.ItemNo) AS ListNo,b.ItemNo,b.Description,b.LocationCode, " +
                "ISNULL(b.StartDay,0) as StartDay, " +
                "ISNULL(b.Cutoff,0) as Cutoff, " +
                "ISNULL(b.Diff,0) as Diff " +
                " FROM( " +
                "SELECT a.ItemNo,a.Description,a.LocationCode, " +
                "(select sum(dbo.[C_E_S_ CO_, LTD_$Item Ledger Entry].Quantity) from dbo.[C_E_S_ CO_, LTD_$Item Ledger Entry] " +
                "WHERE [Location Code] = a.LocationCode and [Item No_] = a.ItemNo AND dbo.[C_E_S_ CO_, LTD_$Item Ledger Entry].[Posting Date]>='2018-10-23 00:00:00' and dbo.[C_E_S_ CO_, LTD_$Item Ledger Entry].[Posting Date]<={0}) as StartDay, " +
                "(select sum(dbo.[C_E_S_ CO_, LTD_$Item Ledger Entry].Quantity) from dbo.[C_E_S_ CO_, LTD_$Item Ledger Entry] " +
                "WHERE [Location Code] = a.LocationCode and [Item No_] = a.ItemNo AND dbo.[C_E_S_ CO_, LTD_$Item Ledger Entry].[Posting Date]>='2018-10-23 00:00:00' and dbo.[C_E_S_ CO_, LTD_$Item Ledger Entry].[Posting Date]<={1}) as Cutoff, " +
                "(select sum(dbo.[C_E_S_ CO_, LTD_$Item Ledger Entry].Quantity) from dbo.[C_E_S_ CO_, LTD_$Item Ledger Entry] " +
                "WHERE [Location Code] = a.LocationCode and [Item No_] = a.ItemNo AND dbo.[C_E_S_ CO_, LTD_$Item Ledger Entry].[Posting Date]>={0} and dbo.[C_E_S_ CO_, LTD_$Item Ledger Entry].[Posting Date]<={1}) as Diff " +
                "FROM( SELECT DISTINCT  " +
                "dbo.[C_E_S_ CO_, LTD_$Item Ledger Entry].[Item No_] AS ItemNo, " +
                "dbo.[C_E_S_ CO_, LTD_$Item].Description AS Description, " +
                "dbo.[C_E_S_ CO_, LTD_$Item Ledger Entry].[Location Code] AS LocationCode " +
                "FROM dbo.[C_E_S_ CO_, LTD_$Item Ledger Entry] " +
                "LEFT JOIN dbo.[C_E_S_ CO_, LTD_$Item] ON dbo.[C_E_S_ CO_, LTD_$Item Ledger Entry].[Item No_] = dbo.[C_E_S_ CO_, LTD_$Item].No_ " +
                "WHERE  dbo.[C_E_S_ CO_, LTD_$Item Ledger Entry].[Posting Date]>='2018-10-23 00:00:00' and dbo.[C_E_S_ CO_, LTD_$Item Ledger Entry].[Posting Date]<={1} " +
                ") as a " +
                ") as b  WHERE (b.StartDay <>0 or b.Cutoff<>0 or b.Diff<>0) and b.LocationCode={2} "+
                " Order by b.ItemNo asc ";



            //SqlParameter parameterStartDate = new SqlParameter("@startdate", StartDate);
            //SqlParameter parameterEndDate = new SqlParameter("@enddate", EndDate);
            //SqlParameter parameterSite = new SqlParameter("@site", site.ToUpper());


            var FAChangeStatuses = await _navcontext.FAChangeStatuses.FromSqlRaw(queryData, StartDate, EndDate, site.ToUpper()).ToListAsync();


            response = Ok(new { data = FAChangeStatuses });


            return response;
        }


        }
}