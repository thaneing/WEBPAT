using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using CESAPSCOREWEBAPP.Helpers;
using CESAPSCOREWEBAPP.Models;
using DevExpress.Compatibility.System.Web;
using DevExpress.XtraReports.UI;
using DevExtreme.AspNet.Data;
using DevExtreme.AspNet.Mvc;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using static CESAPSCOREWEBAPP.Models.Enums;

namespace CESAPSCOREWEBAPP.Controllers
{
    public class FixAssetStraightController : BaseController
    {
        private readonly DatabaseContext _context;

        private readonly NAVContext _navcontext;


        public FixAssetStraightController(DatabaseContext context, NAVContext navcontext)
        {
            _context = context;
            _navcontext = navcontext;
        }


        public IActionResult Index()
        {
            /*Check Session */
            var page = "251";
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

            ViewBag.StartDate = DateTime.Now.ToString("01-01-yyyy", new CultureInfo("en-US"));

            ViewBag.StartEnd = DateTime.Now.ToString("31-12-yyyy", new CultureInfo("en-US"));

            return View();
        }
        public async Task<IActionResult> Gendata(string date1, string date2)
        {
            /*Check Session */
            var page = "251";
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








            var StartDate = Convert.ToDateTime(date1.Substring(6, 4) + "-" + date1.Substring(3, 2) + "-" + date1.Substring(0, 2) + " 00:00:00");
            var EndDate1 = Convert.ToDateTime(date2.Substring(6, 4) + "-" + date2.Substring(3, 2) + "-" + date2.Substring(0, 2) + " 00:00:00");
            TimeSpan difference;
            var EndDate = EndDate1;
            IActionResult response = Unauthorized();
            //var StraightLineAll = _context.FixAssetStraightLines.ToList();
            var StraightLineAll = _context.FixAssetStraightLines.ToList();

            FixAssetStraightLine Straight;

            List<FixAssetStraightLine> Straights = new List<FixAssetStraightLine>();

            var querydata = "SELECT  " +
                "ROW_NUMBER() OVER (ORDER BY dbo.[C_E_S_ CO_, LTD_$FA Ledger Entry].[FA No_]) AS ID, " +
                "dbo.[C_E_S_ CO_, LTD_$FA Ledger Entry].[FA No_] as FANO, " +
                "dbo.[C_E_S_ CO_, LTD_$FA Ledger Entry].Description, " +
                "dbo.[C_E_S_ CO_, LTD_$FA Ledger Entry].[FA Posting Date] as StartDate, " +
                "dbo.[C_E_S_ CO_, LTD_$FA Ledger Entry].[FA Posting Group] AS FAPostingGroup, " +
                "sum(dbo.[C_E_S_ CO_, LTD_$FA Ledger Entry].Amount) as Amount, " +
                "sum(dbo.[C_E_S_ CO_, LTD_$FA Ledger Entry].Qty_) as Quantity " +
                "FROM " +
                "dbo.[C_E_S_ CO_, LTD_$FA Ledger Entry] " +
                "LEFT JOIN dbo.[C_E_S_ CO_, LTD_$Fixed Asset] ON dbo.[C_E_S_ CO_, LTD_$FA Ledger Entry].[FA No_] = dbo.[C_E_S_ CO_, LTD_$Fixed Asset].No_ " +
                "WHERE dbo.[C_E_S_ CO_, LTD_$FA Ledger Entry].[FA Posting Type]=0 and [Reason Code] in('AC DISP','AC DONATIO','AC LOST')   " +
                "GROUP BY  " +
                "dbo.[C_E_S_ CO_, LTD_$FA Ledger Entry].[FA No_], " +
                "dbo.[C_E_S_ CO_, LTD_$FA Ledger Entry].Description, " +
                "dbo.[C_E_S_ CO_, LTD_$FA Ledger Entry].[FA Posting Date], " +
                "dbo.[C_E_S_ CO_, LTD_$FA Ledger Entry].[FA Posting Group] " +
                "order by dbo.[C_E_S_ CO_, LTD_$FA Ledger Entry].[FA No_]  ";

            var FixAssetMisses = await _navcontext.FixAssetMisses.FromSqlRaw(querydata).ToListAsync();




            foreach (var std in StraightLineAll)
            {
                Straight = new FixAssetStraightLine();
                Straight = std;
                EndDate = EndDate1;
                var StartDate1 = StartDate;


                var FixAssetData = FixAssetMisses.Where(p => p.FANO == std.FANO).ToList();
                if (FixAssetData.Count > 0)
                {


                    Straight.StraightLine = 0;
                    var amount = Math.Round(std.Amount, 2);
                    var endprice = Math.Round(std.PriceEnd * -1, 2);
                    decimal total = Math.Round(0.00M, 2);
                    var CutQuantity = 0M;
                    var CutAmount = 0M;


                    if (std.EndDate <= StartDate)  //วันที่สิ้นสุด น้อยกว่าเท่ากับ วันที่ เริ่ม
                    {

                    }
                    else if (std.StartDate >= EndDate) //วันที่เริ่ม มากกว่าเท่ากับ วันที่สิ้นสุด
                    {

                    }
                    else
                    {



                        //เช็คว่ามีรายการเกิดก่อน วันหรือไม่เพื่อลดค่า amount และ Quantity
                        CutQuantity = FixAssetData.Where(p => p.StartDate < StartDate).Sum(a => a.Quantity);
                        CutAmount = FixAssetData.Where(p => p.StartDate < StartDate).Sum(a => a.Amount);
                        amount = amount + CutAmount;
                        endprice = endprice + CutQuantity;


                        if (amount <= 0)
                        {

                        }
                        else
                        {



                            //เช็คตั้งแต่ Start ถึง End มีรายการหรือไม่ คิดค่าใช้จ่ายระหว่างทาง
                            var checkcurrent = FixAssetData.Where(p => p.StartDate >= StartDate && p.StartDate <= EndDate).ToList();
                            if (checkcurrent.Count > 0)
                            {
                                for (var i = 0; i < checkcurrent.Count; i++)
                                {
                                    if (i == 0 && (checkcurrent.Count != i + 1))
                                    {
                                        if (std.StartDate >= StartDate1 && std.EndDate >= EndDate)  //กรณีทรัพย์สินเกิดหลังวันที่กรองข้อมูล
                                        {
                                            StartDate1 = std.StartDate;
                                            difference = checkcurrent[i].StartDate.AddDays(-1) - StartDate1;
                                        }
                                        else if (std.StartDate <= StartDate1 && std.EndDate <= EndDate) //กรณีทรัพย์สินมีอายุการใช้งานไม่ถึง
                                        {
                                            if (checkcurrent[i].StartDate.AddDays(-1) <= EndDate)
                                            {
                                                EndDate = std.EndDate;
                                                difference = checkcurrent[i].StartDate.AddDays(-1) - StartDate1;
                                            }
                                            else
                                            {
                                                EndDate = std.EndDate;
                                                difference = EndDate - StartDate1;
                                            }
                                        }
                                        else if (std.StartDate <= StartDate && std.EndDate >= EndDate) //กรณีปกติ
                                        {
                                            if (checkcurrent[i].StartDate.AddDays(-1) <= EndDate)
                                            {
                                                difference = checkcurrent[i].StartDate.AddDays(-1) - StartDate1;
                                            }
                                            else
                                            {
                                                difference = EndDate - StartDate1;
                                            }
                                        }
                                        else
                                        {
                                            if (checkcurrent[i].StartDate.AddDays(-1) >= StartDate)
                                            {
                                                difference = checkcurrent[i].StartDate.AddDays(-1) - StartDate1;
                                            }
                                            else
                                            {
                                                difference = EndDate - StartDate1;
                                            }
                                        }


                                        var percen = Math.Round(std.Percen, 2);
                                        //difference = FixAssetData[i].StartDate.AddDays(-1) - StartDate1; //create TimeSpan object
                                        total += ((((amount - endprice) * 0.20M) * (difference.Days + 1)) / 360);
                                        StartDate1 = checkcurrent[i].StartDate;
                                    }



                                    if (checkcurrent.Count == i + 1)
                                    {
                                        var percen = Math.Round(std.Percen, 2);
                                        if ((amount + Math.Round(checkcurrent[i].Amount, 2)) == 0)
                                        {
                                            amount = amount;
                                            difference = checkcurrent[i].StartDate - StartDate1; //create TimeSpan object
                                        }
                                        else
                                        {
                                            amount = amount + Math.Round(checkcurrent[i].Amount, 2);
                                            difference = EndDate - StartDate1; //create TimeSpan object
                                        }

                                        endprice = endprice + Math.Round(checkcurrent[i].Quantity, 2);

                                        Console.WriteLine(EndDate + " * " + StartDate1);
                                        Console.WriteLine(amount + "-" + endprice + "*" + percen + "*" + (difference.Days + 1) + "//" + ((((amount - endprice) * (percen / 100)) * (difference.Days + 1)) / 360));
                                        total += ((((amount - endprice) * (percen / 100)) * (difference.Days + 1)) / 360);
                                    }
                                    else
                                    {
                                        var percen = Math.Round(std.Percen, 2);
                                        amount = amount + Math.Round(checkcurrent[i].Amount, 2);
                                        endprice = endprice + Math.Round(checkcurrent[i].Quantity, 2);
                                        difference = checkcurrent[i + 1].StartDate.AddDays(-1) - StartDate1; //create TimeSpan object
                                        total += ((((amount - endprice) * (percen / 100)) * (difference.Days + 1)) / 360);
                                        StartDate1 = checkcurrent[i + 1].StartDate;

                                    }

                                }///จบระหว่างทาง
                            }
                            else
                            {

                                if (std.EndDate <= StartDate)  //วันที่สิ้นสุด น้อยกว่าเท่ากับ วันที่ เริ่ม
                                {

                                }
                                else if (std.StartDate >= EndDate) //วันที่เริ่ม มากกว่าเท่ากับ วันที่สิ้นสุด
                                {

                                }
                                else if (std.StartDate <= StartDate && std.EndDate >= EndDate) //วันที่เริ่ม เริ่ม น้อยกว่าเท่ากับ วันที่เริ่ม  และ วันที่สิ้นสุด มากกว่าเท่ากับ วันที่สิ้นสุด
                                {


                                    difference = EndDate - StartDate; //create TimeSpan object
                                    total = (((((amount - (endprice)) * (std.Percen / 100))) * (difference.Days + 1)) / 360);
                                }
                                else if (std.StartDate >= StartDate && std.EndDate >= EndDate) //วันที เริ่ม มากกว่าเท่ากับ วันที่เริ่ม และ วันที่สิ้นสุด มากกว่าเท่ากับ วันที่สิ้นสุด
                                {

                                    difference = EndDate - std.StartDate; //create TimeSpan object
                                    total = (((((amount - (endprice)) * (std.Percen / 100))) * (difference.Days + 1)) / 360);
                                }
                                else if (std.StartDate <= StartDate && std.EndDate <= EndDate) //วันที่เริ่ม เริ่ม น้อยกว่าเท่ากับ วันที่เริ่ม  และ วันที่สิ้นสุด มากกว่าเท่ากับ วันที่สิ้นสุด
                                {

                                    difference = std.EndDate - StartDate; //create TimeSpan object
                                    total = (((((amount - (endprice)) * (std.Percen / 100))) * (difference.Days + 1)) / 360);
                                }

                            }

                            Straight.StraightLine = Convert.ToDecimal(total);
                            total = 0;
                            StartDate1 = StartDate;
                        }
                    }
                }
                else
                {

                    if (std.EndDate <= StartDate)  //วันที่สิ้นสุด น้อยกว่าเท่ากับ วันที่ เริ่ม
                    {

                    }
                    else if (std.StartDate >= EndDate) //วันที่เริ่ม มากกว่าเท่ากับ วันที่สิ้นสุด
                    {

                    }
                    else if (std.StartDate <= StartDate && std.EndDate >= EndDate) //วันที่เริ่ม เริ่ม น้อยกว่าเท่ากับ วันที่เริ่ม  และ วันที่สิ้นสุด มากกว่าเท่ากับ วันที่สิ้นสุด
                    {


                        difference = EndDate - StartDate; //create TimeSpan object
                        Straight.StraightLine = (((((std.Amount - (std.PriceEnd * -1)) * (std.Percen / 100))) * (difference.Days + 1)) / 360);
                    }
                    else if (std.StartDate >= StartDate && std.EndDate >= EndDate) //วันที เริ่ม มากกว่าเท่ากับ วันที่เริ่ม และ วันที่สิ้นสุด มากกว่าเท่ากับ วันที่สิ้นสุด
                    {

                        difference = EndDate - std.StartDate; //create TimeSpan object
                        Straight.StraightLine = (((((std.Amount - (std.PriceEnd * -1)) * (std.Percen / 100))) * (difference.Days + 1)) / 360);
                    }
                    else if (std.StartDate <= StartDate && std.EndDate <= EndDate) //วันที่เริ่ม เริ่ม น้อยกว่าเท่ากับ วันที่เริ่ม  และ วันที่สิ้นสุด มากกว่าเท่ากับ วันที่สิ้นสุด
                    {

                        difference = std.EndDate - StartDate; //create TimeSpan object
                        Straight.StraightLine = (((((std.Amount - (std.PriceEnd * -1)) * (std.Percen / 100))) * (difference.Days + 1)) / 360);
                    }
                }


                Straights.Add(Straight);

            }


            response = Ok(new { data = Straights });
            return response;
        }



        public static double CalPercent(decimal value1)
        {


            Double result = 0;
            try
            {
                //Percen((((amount - (endprice * -1)) * (Percen(std.Percen, 100))) * difference.Days), 360);
                result = 0.01 * Convert.ToDouble(value1);
            }
            catch
            {
                result = 0;
            }
            return result;
        }




        public async Task<IActionResult> UpdateData()
        {

            /*Check Session */
            var page = "260";
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
            var querydata = "SELECT ROW_NUMBER() OVER (ORDER BY a.FANO) AS ID,*, " +
                "(SELECT top 1 Amount FROM dbo.[C_E_S_ CO_, LTD_$FA Ledger Entry] WHERE [FA Posting Type]=7 and [FA No_]=a.FANO and Amount<0 and [FA No_]!='' ORDER BY [Entry No_] DESC) as PriceEnd, " +
                "isnull((SELECT top 1 [FA Posting Category] FROM dbo.[C_E_S_ CO_, LTD_$FA Ledger Entry] WHERE [FA Posting Category]=1 and [FA No_]=a.FANO  ORDER BY [Entry No_] DESC),0) as Sale, " +
                "(SELECT top 1 [FA Posting Date] FROM dbo.[C_E_S_ CO_, LTD_$FA Ledger Entry] WHERE [FA Posting Category]=1 and [FA No_]=a.FANO   ORDER BY [Entry No_] DESC) as SaleDate, " +
                "(SELECT top 1 dbo.[C_E_S_ CO_, LTD_$FA Ledger Entry].[Straight-Line _] FROM dbo.[C_E_S_ CO_, LTD_$FA Ledger Entry] WHERE  [FA No_]=a.FANO and [Straight-Line _]<>0 ORDER BY [Entry No_] DESC) as Percen, " +
                "(100/(SELECT top 1 dbo.[C_E_S_ CO_, LTD_$FA Ledger Entry].[Straight-Line _] FROM dbo.[C_E_S_ CO_, LTD_$FA Ledger Entry] WHERE  [FA No_]=a.FANO and [Straight-Line _]<>0 ORDER BY [Entry No_] DESC)) as Life, " +
                "DATEADD(year,100/(SELECT top 1 dbo.[C_E_S_ CO_, LTD_$FA Ledger Entry].[Straight-Line _] FROM dbo.[C_E_S_ CO_, LTD_$FA Ledger Entry] WHERE  [FA No_]=a.FANO and [Straight-Line _]<>0 ORDER BY [Entry No_] DESC),a.StartDate) as EndDate," +
                "0.00 as StraightLine " +
                " FROM( " +
                "SELECT  " +
                "dbo.[C_E_S_ CO_, LTD_$FA Ledger Entry].[FA No_] as FANO, " +
                "dbo.[C_E_S_ CO_, LTD_$FA Ledger Entry].Description, " +
                "dbo.[C_E_S_ CO_, LTD_$FA Ledger Entry].[FA Posting Date] as StartDate, " +
                "dbo.[C_E_S_ CO_, LTD_$FA Ledger Entry].Amount, " +
                "dbo.[C_E_S_ CO_, LTD_$FA Ledger Entry].[FA Posting Group] AS FAPostingGroup," +
                "dbo.[C_E_S_ CO_, LTD_$FA Ledger Entry].Qty_ as Quantity " +
                "FROM " +
                "dbo.[C_E_S_ CO_, LTD_$FA Ledger Entry] " +
                "LEFT JOIN dbo.[C_E_S_ CO_, LTD_$Fixed Asset] ON dbo.[C_E_S_ CO_, LTD_$FA Ledger Entry].[FA No_] = dbo.[C_E_S_ CO_, LTD_$Fixed Asset].No_ " +
                "WHERE [FA Posting Type]=0 and Amount>=0   " +
                ")as a ";

            var StraightLines = await _navcontext.FixAssetStraightLines.FromSqlRaw(querydata).ToListAsync();


            var StraightLineAll = _context.FixAssetStraightLines.ToList();



            List<FixAssetStraightLine> fixAssetStraightLine = new List<FixAssetStraightLine>();
            FixAssetStraightLine Straight;
            foreach (var std in StraightLines)
            {
                Straight = new FixAssetStraightLine();
                Straight.Amount = std.Amount;
                Straight.EndDate = std.EndDate;
                Straight.FANO = std.FANO;
                Straight.Description = std.Description;
                Straight.Life = std.Life;
                Straight.FAPostingGroup = std.FAPostingGroup;
                Straight.Percen = std.Percen;
                Straight.PriceEnd = std.PriceEnd;
                Straight.Quantity = std.Quantity;
                Straight.Sale = std.Sale;
                Straight.SaleDate = std.SaleDate;
                Straight.StartDate = std.StartDate;
                Straight.StraightLine = std.StraightLine;
                fixAssetStraightLine.Add(Straight);


            }

            _context.FixAssetStraightLines.RemoveRange(StraightLineAll);
            _context.FixAssetStraightLines.AddRange(fixAssetStraightLine);
            await _context.SaveChangesAsync();




            response = Ok(new { name = StraightLines });
            return response;
        }


        public static decimal Percen(decimal value1, decimal value2)
        {
            decimal result = 0;
            try
            {
                result = Math.Round((value1 / value2), 2);
            }
            catch
            {
                result = 0;
            }
            return result;
        }







        public async Task<IActionResult> UpdateDataAll()
        {
            var page = "275";
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

            var FixAssetStraightAlls = await _context.FixAssetStraightAlls.ToListAsync();

            _context.FixAssetStraightAlls.RemoveRange(FixAssetStraightAlls);

            await _context.SaveChangesAsync();


            DateTime Checkdate;
            var detailall = _context.FixAssetStraightLines.ToList();


            var querydata = "SELECT  " +
                "ROW_NUMBER() OVER (ORDER BY dbo.[C_E_S_ CO_, LTD_$FA Ledger Entry].[FA No_]) AS ID, " +
                "dbo.[C_E_S_ CO_, LTD_$FA Ledger Entry].[FA No_] as FANO, " +
                "dbo.[C_E_S_ CO_, LTD_$FA Ledger Entry].Description, " +
                "dbo.[C_E_S_ CO_, LTD_$FA Ledger Entry].[FA Posting Date] as StartDate, " +
                "dbo.[C_E_S_ CO_, LTD_$FA Ledger Entry].[FA Posting Group] AS FAPostingGroup, " +
                "sum(dbo.[C_E_S_ CO_, LTD_$FA Ledger Entry].Amount) as Amount, " +
                "sum(dbo.[C_E_S_ CO_, LTD_$FA Ledger Entry].Qty_) as Quantity " +
                "FROM " +
                "dbo.[C_E_S_ CO_, LTD_$FA Ledger Entry] " +
                "LEFT JOIN dbo.[C_E_S_ CO_, LTD_$Fixed Asset] ON dbo.[C_E_S_ CO_, LTD_$FA Ledger Entry].[FA No_] = dbo.[C_E_S_ CO_, LTD_$Fixed Asset].No_ " +
                "WHERE dbo.[C_E_S_ CO_, LTD_$FA Ledger Entry].[FA Posting Type]=0 and [Reason Code] in('AC DISP','AC DONATIO','AC LOST')   " +
                "GROUP BY  " +
                "dbo.[C_E_S_ CO_, LTD_$FA Ledger Entry].[FA No_], " +
                "dbo.[C_E_S_ CO_, LTD_$FA Ledger Entry].Description, " +
                "dbo.[C_E_S_ CO_, LTD_$FA Ledger Entry].[FA Posting Date], " +
                "dbo.[C_E_S_ CO_, LTD_$FA Ledger Entry].[FA Posting Group] " +
                "order by dbo.[C_E_S_ CO_, LTD_$FA Ledger Entry].[FA No_]  ";
            var FixAssetMisses = await _navcontext.FixAssetMisses.FromSqlRaw(querydata).ToListAsync();



            List<FixAssetStraightAll> fixAssetStraightAll = new List<FixAssetStraightAll>();
            FixAssetStraightAll Straight;

            foreach (var dt in detailall)
            {
                //if (dt.Sale == 1)
                //{
                //    dt.EndDate = dt.SaleDate ?? DateTime.Now;
                //}


                var datestart = dt.StartDate;
                var dateend = dt.EndDate;
                var amount = Math.Round(dt.Amount, 2);
                var qty = Math.Round(dt.Quantity, 2);
                var endprice = Math.Round(dt.PriceEnd, 2);
                decimal total = Math.Round(0.00M, 2);
                var CutQuantity = 0M;
                var CutAmount = 0M;

                //เช็คว่ามีรายการเกิดก่อน วันหรือไม่เพื่อลดค่า amount และ Quantity
                CutQuantity = FixAssetMisses.Where(p => p.StartDate < datestart && p.FANO == dt.FANO).Sum(a => a.Quantity);
                CutAmount = FixAssetMisses.Where(p => p.StartDate < datestart && p.FANO == dt.FANO).Sum(a => a.Amount);
                amount = amount + CutAmount;
                endprice = endprice - CutQuantity;
                qty = qty - CutQuantity;

                var checksale = 0;

                while (datestart <= dateend) //วนลูปจนกว่า Datestart มากกว่า Dateend
                {
                    if (datestart.Month + 1 > 12)
                    {   //เช็คเงื่อนไขถ้าเดือนมากกว่า 12 เริ่มปีใหม่
                        Checkdate = new DateTime(datestart.Year + 1, 1, 1, 0, 0, 0);
                    }
                    else
                    {
                        Checkdate = new DateTime(datestart.Year, datestart.Month + 1, 1, 0, 0, 0);
                    }


                    var CheckMount = Checkdate.AddDays(-1);

                    //เช็คตั้งแต่ Start ถึง End มีรายการหรือไม่ คิดค่าใช้จ่ายระหว่างทาง
                    var checkcurrent = FixAssetMisses.Where(p => p.StartDate >= datestart && p.StartDate <= CheckMount && p.FANO == dt.FANO).ToList();
                    if (checkcurrent.Count > 0)
                    {


                        CutQuantity = checkcurrent.Sum(a => a.Quantity);
                        CutAmount = checkcurrent.Sum(a => a.Amount);

                        if ((amount + CutAmount) == 0)
                        {
                            amount = amount;
                            qty = qty;
                            CheckMount = checkcurrent[0].StartDate;
                            checksale = 1;
                        }
                        else
                        {
                            amount = amount + CutAmount;
                            endprice = endprice - CutQuantity;
                            qty = qty + CutQuantity;
                            checksale = 0;
                        }


                    }


                    if (CheckMount <= dt.EndDate)
                    {
                        Straight = new FixAssetStraightAll();
                        Straight.Amount = amount;
                        //Straight.Amount = dt.Amount;
                        Straight.EndDate = dt.EndDate;
                        Straight.FANO = dt.FANO;
                        Straight.Description = dt.Description;
                        Straight.Life = dt.Life;
                        Straight.FAPostingGroup = dt.FAPostingGroup;
                        Straight.Percen = dt.Percen;
                        Straight.PriceEnd = endprice;
                        Straight.Quantity = qty;
                        // Straight.PriceEnd = dt.PriceEnd;
                        //Straight.Quantity = dt.Quantity;
                        Straight.Sale = dt.Sale;
                        Straight.SaleDate = dt.SaleDate;
                        Straight.StartDate = dt.StartDate;
                        Straight.DateInMount = CheckMount;

                        //Straight.StraightLine = ((((dt.Amount - (dt.PriceEnd * -1)) * ((dt.Percen / 100))) * (CheckMount - datestart).Days) / 360);

                        Straight.StraightLine = ((((amount + endprice) * ((dt.Percen / 100))) * ((CheckMount - datestart).Days + 1) / 360));
                        Console.WriteLine(CheckMount + " * " + datestart);
                        Console.WriteLine(amount + "-" + endprice + "*" + dt.Percen / 100 + "*" + ((CheckMount - datestart).Days + 1) + "//" + ((((amount + endprice) * ((dt.Percen / 100))) * ((CheckMount - datestart).Days + 1) / 360)));

                        datestart = CheckMount.AddDays(+1);
                        fixAssetStraightAll.Add(Straight);

                        var json = new JavaScriptSerializer().Serialize(Straight);
                        //Console.WriteLine(json);
                        //Console.Beep();
                    }
                    else
                    {
                        Straight = new FixAssetStraightAll();
                        Straight.Amount = amount;
                        //Straight.Amount = dt.Amount;
                        Straight.EndDate = dt.EndDate;
                        Straight.FANO = dt.FANO;
                        Straight.Description = dt.Description;
                        Straight.Life = dt.Life;
                        Straight.FAPostingGroup = dt.FAPostingGroup;
                        Straight.Percen = dt.Percen;
                        Straight.PriceEnd = endprice;
                        Straight.Quantity = qty;
                        //Straight.Quantity = dt.Quantity;
                        Straight.Sale = dt.Sale;
                        Straight.SaleDate = dt.SaleDate;
                        Straight.StartDate = dt.StartDate;
                        Straight.DateInMount = dt.EndDate;

                        Straight.StraightLine = ((((amount + endprice) * ((dt.Percen / 100))) * ((dt.EndDate - datestart).Days + 1) / 360));

                        Console.WriteLine(CheckMount + " * " + datestart);
                        Console.WriteLine(amount + "-" + endprice + "*" + dt.Percen / 100 + "*" + ((dt.EndDate - datestart).Days + 1) + "//" + ((((amount + endprice) * ((dt.Percen / 100))) * ((dt.EndDate - datestart).Days + 1) / 360)));

                        datestart = CheckMount.AddDays(+1);
                        fixAssetStraightAll.Add(Straight);

                        var json = new JavaScriptSerializer().Serialize(Straight);
                        //Console.WriteLine(json);


                    }
                    if (checksale == 1)
                    {
                        amount = 0;
                        endprice = 0;
                        qty = 0;
                    }
                }

            }



            _context.FixAssetStraightAlls.AddRange(fixAssetStraightAll);
            await _context.SaveChangesAsync();

            response = Ok(new { data = fixAssetStraightAll });
            return response;
        }







        public IActionResult DataInLine()
        {
            /*Check Session */
            var page = "251";
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



        public async Task<IActionResult> UpdateDataAllInLine()
        {
            /*Check Session */
            var page = "251";
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

            DateTime Checkdate;
            var detailall = _context.FixAssetStraightLines.ToList();





            var querydata = "SELECT  " +
                "ROW_NUMBER() OVER (ORDER BY dbo.[C_E_S_ CO_, LTD_$FA Ledger Entry].[FA No_]) AS ID, " +
                "dbo.[C_E_S_ CO_, LTD_$FA Ledger Entry].[FA No_] as FANO, " +
                "dbo.[C_E_S_ CO_, LTD_$FA Ledger Entry].Description, " +
                "dbo.[C_E_S_ CO_, LTD_$FA Ledger Entry].[FA Posting Date] as StartDate, " +
                "dbo.[C_E_S_ CO_, LTD_$FA Ledger Entry].[FA Posting Group] AS FAPostingGroup, " +
                "sum(dbo.[C_E_S_ CO_, LTD_$FA Ledger Entry].Amount) as Amount, " +
                "sum(dbo.[C_E_S_ CO_, LTD_$FA Ledger Entry].Qty_) as Quantity " +
                "FROM " +
                "dbo.[C_E_S_ CO_, LTD_$FA Ledger Entry] " +
                "LEFT JOIN dbo.[C_E_S_ CO_, LTD_$Fixed Asset] ON dbo.[C_E_S_ CO_, LTD_$FA Ledger Entry].[FA No_] = dbo.[C_E_S_ CO_, LTD_$Fixed Asset].No_ " +
                "WHERE dbo.[C_E_S_ CO_, LTD_$FA Ledger Entry].[FA Posting Type]=0 and [Reason Code] in('AC DISP','AC DONATIO','AC LOST')   " +
                "GROUP BY  " +
                "dbo.[C_E_S_ CO_, LTD_$FA Ledger Entry].[FA No_], " +
                "dbo.[C_E_S_ CO_, LTD_$FA Ledger Entry].Description, " +
                "dbo.[C_E_S_ CO_, LTD_$FA Ledger Entry].[FA Posting Date], " +
                "dbo.[C_E_S_ CO_, LTD_$FA Ledger Entry].[FA Posting Group] " +
                "order by dbo.[C_E_S_ CO_, LTD_$FA Ledger Entry].[FA No_]  ";
            var FixAssetMisses = await _navcontext.FixAssetMisses.FromSqlRaw(querydata).ToListAsync();



            List<FixAssetStraightAll> fixAssetStraightAll = new List<FixAssetStraightAll>();
            FixAssetStraightAll Straight;

            foreach (var dt in detailall)
            {
                //if (dt.Sale == 1)
                //{
                //    dt.EndDate = dt.SaleDate ?? DateTime.Now;
                //}


                var datestart = dt.StartDate;
                var dateend = dt.EndDate;
                var amount = Math.Round(dt.Amount, 2);
                var qty = Math.Round(dt.Quantity, 2);
                var endprice = Math.Round(dt.PriceEnd, 2);
                decimal total = Math.Round(0.00M, 2);
                var CutQuantity = 0M;
                var CutAmount = 0M;

                //เช็คว่ามีรายการเกิดก่อน วันหรือไม่เพื่อลดค่า amount และ Quantity
                CutQuantity = FixAssetMisses.Where(p => p.StartDate < datestart && p.FANO == dt.FANO).Sum(a => a.Quantity);
                CutAmount = FixAssetMisses.Where(p => p.StartDate < datestart && p.FANO == dt.FANO).Sum(a => a.Amount);
                amount = amount + CutAmount;
                endprice = endprice - CutQuantity;
                qty = qty - CutQuantity;

                var checksale = 0;

                while (datestart <= dateend) //วนลูปจนกว่า Datestart มากกว่า Dateend
                {
                    if (datestart.Month + 1 > 12)
                    {   //เช็คเงื่อนไขถ้าเดือนมากกว่า 12 เริ่มปีใหม่
                        Checkdate = new DateTime(datestart.Year + 1, 1, 1, 0, 0, 0);
                    }
                    else
                    {
                        Checkdate = new DateTime(datestart.Year, datestart.Month + 1, 1, 0, 0, 0);
                    }


                    var CheckMount = Checkdate.AddDays(-1);

                    //เช็คตั้งแต่ Start ถึง End มีรายการหรือไม่ คิดค่าใช้จ่ายระหว่างทาง
                    var checkcurrent = FixAssetMisses.Where(p => p.StartDate >= datestart && p.StartDate <= CheckMount && p.FANO == dt.FANO).ToList();
                    if (checkcurrent.Count > 0)
                    {


                        CutQuantity = checkcurrent.Sum(a => a.Quantity);
                        CutAmount = checkcurrent.Sum(a => a.Amount);

                        if ((amount + CutAmount) == 0)
                        {
                            amount = amount;
                            qty = qty;
                            CheckMount = checkcurrent[0].StartDate;
                            checksale = 1;
                        }
                        else
                        {
                            amount = amount + CutAmount;
                            endprice = endprice - CutQuantity;
                            qty = qty + CutQuantity;
                            checksale = 0;
                        }


                    }


                    if (CheckMount <= dt.EndDate)
                    {
                        Straight = new FixAssetStraightAll();
                        Straight.Amount = amount;
                        //Straight.Amount = dt.Amount;
                        Straight.EndDate = dt.EndDate;
                        Straight.FANO = dt.FANO;
                        Straight.Description = dt.Description;
                        Straight.Life = dt.Life;
                        Straight.FAPostingGroup = dt.FAPostingGroup;
                        Straight.Percen = dt.Percen;
                        Straight.PriceEnd = endprice;
                        Straight.Quantity = qty;
                        // Straight.PriceEnd = dt.PriceEnd;
                        //Straight.Quantity = dt.Quantity;
                        Straight.Sale = dt.Sale;
                        Straight.SaleDate = dt.SaleDate;
                        Straight.StartDate = dt.StartDate;
                        Straight.DateInMount = CheckMount;

                        //Straight.StraightLine = ((((dt.Amount - (dt.PriceEnd * -1)) * ((dt.Percen / 100))) * (CheckMount - datestart).Days) / 360);

                        Straight.StraightLine = ((((amount + endprice) * ((dt.Percen / 100))) * ((CheckMount - datestart).Days + 1) / 360));
                        Console.WriteLine(CheckMount + " * " + datestart);
                        Console.WriteLine(amount + "-" + endprice + "*" + dt.Percen / 100 + "*" + ((CheckMount - datestart).Days + 1) + "//" + ((((amount + endprice) * ((dt.Percen / 100))) * ((CheckMount - datestart).Days + 1) / 360)));

                        datestart = CheckMount.AddDays(+1);
                        fixAssetStraightAll.Add(Straight);

                        var json = new JavaScriptSerializer().Serialize(Straight);
                        //Console.WriteLine(json);
                        //Console.Beep();
                    }
                    else
                    {
                        Straight = new FixAssetStraightAll();
                        Straight.Amount = amount;
                        //Straight.Amount = dt.Amount;
                        Straight.EndDate = dt.EndDate;
                        Straight.FANO = dt.FANO;
                        Straight.Description = dt.Description;
                        Straight.Life = dt.Life;
                        Straight.FAPostingGroup = dt.FAPostingGroup;
                        Straight.Percen = dt.Percen;
                        Straight.PriceEnd = endprice;
                        Straight.Quantity = qty;
                        //Straight.Quantity = dt.Quantity;
                        Straight.Sale = dt.Sale;
                        Straight.SaleDate = dt.SaleDate;
                        Straight.StartDate = dt.StartDate;
                        Straight.DateInMount = dt.EndDate;

                        Straight.StraightLine = ((((amount + endprice) * ((dt.Percen / 100))) * ((dt.EndDate - datestart).Days + 1) / 360));

                        Console.WriteLine(CheckMount + " * " + datestart);
                        Console.WriteLine(amount + "-" + endprice + "*" + dt.Percen / 100 + "*" + ((dt.EndDate - datestart).Days + 1) + "//" + ((((amount + endprice) * ((dt.Percen / 100))) * ((dt.EndDate - datestart).Days + 1) / 360)));

                        datestart = CheckMount.AddDays(+1);
                        fixAssetStraightAll.Add(Straight);

                        var json = new JavaScriptSerializer().Serialize(Straight);
                        //Console.WriteLine(json);


                    }
                    if (checksale == 1)
                    {
                        amount = 0;
                        endprice = 0;
                        qty = 0;
                    }
                }

            }




            response = Ok(new { data = fixAssetStraightAll });
            return response;
        }


        public async Task<IActionResult> GenDataAllInLine()
        {
            /*Check Session */
            var page = "251";
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

            var fixAssetStraightAll = await _context.FixAssetStraightAlls.ToListAsync();

            response = Ok(new { data = fixAssetStraightAll });
            return response;

        }

        public async Task<IActionResult> AllInLineReports(string date1,string date2)
        {
            
            IActionResult response = Unauthorized();

            var StartDate = Convert.ToDateTime(date1.Substring(6, 4) + "-" + date1.Substring(3, 2) + "-" + date1.Substring(0, 2) + " 00:00:00");
            var EndDate1 = Convert.ToDateTime(date2.Substring(6, 4) + "-" + date2.Substring(3, 2) + "-" + date2.Substring(0, 2) + " 00:00:00");

            var fixAssetStraightAll = await _context.FixAssetStraightAlls.Where(p=>p.DateInMount >= StartDate && p.DateInMount<= EndDate1).ToListAsync();

            //response = Ok(new { data = fixAssetStraightAll });
            //return response;

            XtraReport report = XtraReport.FromFile("reports\\ReportAllInLine.repx");
            report.DataSource = fixAssetStraightAll;



            report.CreateDocument(true);
            var @out = new MemoryStream();
            report.ExportToPdf(@out);
            @out.Position = 0;



            response = Ok(new { data = fixAssetStraightAll });


            //return response;
            return new FileStreamResult(@out, "application/pdf");

        }
        public object  GenDataChartLine(DataSourceLoadOptions loadOptions)
        {
            var fixAssetStraightAll = _context.FixAssetStraightAlls.ToList();

            return DataSourceLoader.Load(fixAssetStraightAll, loadOptions);
        }

  


    }
}