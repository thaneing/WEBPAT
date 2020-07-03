using System;
using System.Collections;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;
using CESAPSCOREWEBAPP.Helpers;
using CESAPSCOREWEBAPP.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using static CESAPSCOREWEBAPP.Models.Enums;

namespace CESAPSCOREWEBAPP.Controllers
{
    public class ItemLedgerEntrysController : BaseController
    {
        private readonly NAVContext _navcontext;
        private readonly DatabaseContext _context;

        public ItemLedgerEntrysController(NAVContext navcontext,DatabaseContext context)
        {
            _context = context;
            _navcontext = navcontext;
        }

        // GET: ItemLedgerEntrys
        public IActionResult Index()
        {



            /*Check Session */
            var page = "41";
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





            var query = "SELECT DISTINCT dbo."+ Environment.GetEnvironmentVariable("Company") +"Item Ledger Entry].[Location Code] AS JobNo,dbo."+ Environment.GetEnvironmentVariable("Company") +"Item Ledger Entry].[Location Code]AS LocationCode FROM dbo."+ Environment.GetEnvironmentVariable("Company") +"Item Ledger Entry] ORDER BY dbo."+ Environment.GetEnvironmentVariable("Company") +"Item Ledger Entry].[Location Code]";
            List<V_Job> jobNo = _navcontext.v_Job.FromSqlRaw(query).ToList();

            //var JobNo = _context.UserJobs
            //.Where(s => s.UserId == HttpContext.Session.GetInt32("Userid"))
            //.ToList();

            ViewData["JobNo"] = jobNo;
            ViewBag.StartDate = DateTime.Now.ToString("dd/MM/yyyy");


            return View();
        }



        // POST: ItemLedgerEntrys/Deletefile/5

        public IActionResult ShowData(string Job,string date1)
        {

            /*Check Session */
            var page = "41";
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

            int rowdata = 0;
            var columdata = 0;
            var head = "";
            var table = "";
            var checkrow = "Null";
            var queryLocation = "";
            var query = "";
            var footer = "";
            // Creating an ArrayList
            ArrayList myList = new ArrayList(1000);



            if (Job==null)
            {
                head = "<div class='table-responsive'><table id=\'itemmetrix\' class=\'table2excel table2excel_with_colors\'>"
                 + "<thead>"
               + "<tr>"
               + "<th>ItemNo</th><th>Decription</th><th style=\'text-align :right;\'>Total</th>";
                head += "</tr></thead>";
                columdata = 3;

                 footer = "<tfoot>"
              + "<tr>"
              + "<td>ItemNo</td><td>Decription</td><td style=\'text-align :right;\'>Total</td>";
                footer += "</tr></tfoot>";

                query = "SELECT ROW_NUMBER() OVER (ORDER BY a.ItemNo) as ID ,a.ItemNo,(REPLACE(REPLACE(a.Description,'\"','*'),'>','มากกว่า')) as Description,a.LocationCode,"
               + " (select sum(dbo."+ Environment.GetEnvironmentVariable("Company") +"Item Ledger Entry].Quantity) from dbo."+ Environment.GetEnvironmentVariable("Company") +"Item Ledger Entry]  WHERE [Item No_] = a.ItemNo AND dbo."+ Environment.GetEnvironmentVariable("Company") +"Item Ledger Entry].[Posting Date]>='2018-10-23 00:00:00' and dbo."+ Environment.GetEnvironmentVariable("Company") +"Item Ledger Entry].[Posting Date]<='" + date1 + "') as TotalItem,"
               + " (select sum(dbo."+ Environment.GetEnvironmentVariable("Company") +"Item Ledger Entry].Quantity) from dbo."+ Environment.GetEnvironmentVariable("Company") +"Item Ledger Entry] WHERE [Location Code] = a.LocationCode and [Item No_] = a.ItemNo AND dbo."+ Environment.GetEnvironmentVariable("Company") +"Item Ledger Entry].[Posting Date]>='2018-10-23 00:00:00' "
               + " and dbo."+ Environment.GetEnvironmentVariable("Company") +"Item Ledger Entry].[Posting Date]<='" + date1 + "') as Quantity"
               + " FROM "
               + " (SELECT DISTINCT dbo."+ Environment.GetEnvironmentVariable("Company") +"Item Ledger Entry].[Item No_] AS ItemNo,dbo."+ Environment.GetEnvironmentVariable("Company") +"Item].Description as Description, dbo."+ Environment.GetEnvironmentVariable("Company") +"Item Ledger Entry].[Location Code] AS LocationCode FROM dbo."+ Environment.GetEnvironmentVariable("Company") +"Item Ledger Entry]"
               + " LEFT JOIN dbo."+ Environment.GetEnvironmentVariable("Company") +"Item] ON dbo."+ Environment.GetEnvironmentVariable("Company") +"Item Ledger Entry].[Item No_] = dbo."+ Environment.GetEnvironmentVariable("Company") +"Item].No_ WHERE  dbo."+ Environment.GetEnvironmentVariable("Company") +"Item Ledger Entry].[Posting Date]>='2018-10-23 00:00:00' and dbo."+ Environment.GetEnvironmentVariable("Company") +"Item Ledger Entry].[Posting Date]<='" + date1 + "') as a ORDER BY a.ItemNo,a.LocationCode";

                var itemLedgerEntry = _navcontext.ItemLedgerEntry.FromSqlRaw(query).ToList();
                table = "<tbody>";
                int countDes = 0;
              
                int countDesrowNo = 1;
                foreach (var itm in itemLedgerEntry as IList<ItemLedgerEntry>)
                {
                 
                    //itemLedgerEntry.Count(itm.Quantity)
                    if (checkrow != itm.ItemNo)
                    { //เช็คหาก Decription ไม่เหมือนกันให้สร้าง Row ใหม่ทันที

                        rowdata = rowdata + 1;
                        table += "<tr>";
                        checkrow = itm.Description;
                        table += "<td>" + itm.ItemNo + "</td>";
                        table += "<td>" + itm.Description + "</td>";
                        table += "<td align =\'right\'><button data-animal-type=\'" + itm.ItemNo + "\' data-itemname-type=\'" + itm.Description + "\' data-date1-type=\'" + date1 + "\' onclick=\'selectDetail(this)\' class=\'btn btn-link\'><u>" + SetFontRed(itm.TotalItem, 1) + "</u></button></td>";
                        checkrow = itm.ItemNo; //Adsign ค้่่าให้เท่ากัน
                        countDes = (from x in itemLedgerEntry where x.ItemNo == itm.ItemNo select x).Count();
                    }

                    if (countDes == countDesrowNo)
                    {
                       
                        table += "</tr>";

                    }
                    countDes -= 1;
                }








            }
            else
            {


                string str = "";
                // Split authors separated by a comma followed by space  
                string[] authorsList = Job.Split(",");
                foreach (string author in authorsList)
                {
                    str += "'" + author + "',";
                }

                str = str.Substring(0, str.Length - 1);
                queryLocation = "SELECT DISTINCT dbo."+ Environment.GetEnvironmentVariable("Company") +"Item Ledger Entry].[Location Code] AS LocationID,dbo."+ Environment.GetEnvironmentVariable("Company") +"Item Ledger Entry].[Location Code] AS LocationCode FROM dbo."+ Environment.GetEnvironmentVariable("Company") +"Item Ledger Entry] "
                + "WHERE dbo."+ Environment.GetEnvironmentVariable("Company") +"Item Ledger Entry].[Posting Date]>='2018-10-23 00:00:00' "
                + "and dbo."+ Environment.GetEnvironmentVariable("Company") +"Item Ledger Entry].[Posting Date]<='" + date1 + "' AND dbo."+ Environment.GetEnvironmentVariable("Company") +"Item Ledger Entry].[Location Code] in (" + str + ") "
                + "ORDER BY [Location Code]";
                var locations = _navcontext.Locations.FromSqlRaw(queryLocation).ToList();
                head = "<table id='itemmetrix'>"
                    + "<thead>"
                    + "<tr>"
                    + "<th>ItemNo</th><th>Decription</th><th style=\'text-align :right;\'>Total</th>";

                footer = "<tfoot>"
                       + "<tr>"
                       + "<td>ItemNo</td><td>Decription</td><td style=\'text-align :right;\'>Total</td>";

                columdata = 3;
                foreach (var location in locations as IList<Location>)
                {
                    columdata=columdata + 1;
                    head += "<th style=\'text-align :right;\'>" + location.LocationCode + "</th>";
                    footer += "<td style=\'text-align :right;\'>" + location.LocationCode + "</td>";
                    myList.Add(location.LocationCode);

                }

                head += "</tr></thead>";
                footer += "</tr></tfoot></div>";

                ViewBag.head = head;


                query = "SELECT ROW_NUMBER() OVER (ORDER BY a.ItemNo) as ID ,a.ItemNo,(REPLACE(REPLACE(a.Description,'\"','*'),'>','มากกว่า')) as Description,a.LocationCode,"
                      + " (select sum(dbo."+ Environment.GetEnvironmentVariable("Company") +"Item Ledger Entry].Quantity) from dbo."+ Environment.GetEnvironmentVariable("Company") +"Item Ledger Entry]  WHERE [Item No_] = a.ItemNo AND dbo."+ Environment.GetEnvironmentVariable("Company") +"Item Ledger Entry].[Posting Date]>='2018-10-23 00:00:00' and dbo."+ Environment.GetEnvironmentVariable("Company") +"Item Ledger Entry].[Posting Date]<='" + date1 + "') as TotalItem,"
                    + " (select sum(dbo."+ Environment.GetEnvironmentVariable("Company") +"Item Ledger Entry].Quantity) from dbo."+ Environment.GetEnvironmentVariable("Company") +"Item Ledger Entry] WHERE [Location Code] = a.LocationCode and [Item No_] = a.ItemNo AND dbo."+ Environment.GetEnvironmentVariable("Company") +"Item Ledger Entry].[Posting Date]>='2018-10-23 00:00:00' "
                    + " and dbo."+ Environment.GetEnvironmentVariable("Company") +"Item Ledger Entry].[Posting Date]<='" + date1 + "' AND dbo."+ Environment.GetEnvironmentVariable("Company") +"Item Ledger Entry].[Location Code] in (" + str + ")) as Quantity"
                    + " FROM "
                    + " (SELECT DISTINCT dbo."+ Environment.GetEnvironmentVariable("Company") +"Item Ledger Entry].[Item No_] AS ItemNo,dbo."+ Environment.GetEnvironmentVariable("Company") +"Item].Description as Description, dbo."+ Environment.GetEnvironmentVariable("Company") +"Item Ledger Entry].[Location Code] AS LocationCode FROM dbo."+ Environment.GetEnvironmentVariable("Company") +"Item Ledger Entry]"
                    + " LEFT JOIN dbo."+ Environment.GetEnvironmentVariable("Company") +"Item] ON dbo."+ Environment.GetEnvironmentVariable("Company") +"Item Ledger Entry].[Item No_] = dbo."+ Environment.GetEnvironmentVariable("Company") +"Item].No_ WHERE  dbo."+ Environment.GetEnvironmentVariable("Company") +"Item Ledger Entry].[Posting Date]>='2018-10-23 00:00:00' and dbo."+ Environment.GetEnvironmentVariable("Company") +"Item Ledger Entry].[Posting Date]<='" + date1 + "' AND dbo."+ Environment.GetEnvironmentVariable("Company") +"Item Ledger Entry].[Location Code] in (" + str + ")) as a ORDER BY a.ItemNo,a.LocationCode";


                string[] intArray = new string[myList.Count];



                var itemLedgerEntry = _navcontext.ItemLedgerEntry.FromSqlRaw(query).ToList();
                table = "<tbody>";
                int countDes = 0;
                int countDesrowNo = 1;
                foreach (var itm in itemLedgerEntry as IList<ItemLedgerEntry>)
                {
                    if (itm.TotalItem != 0) { 
                        //itemLedgerEntry.Count(itm.Quantity)
                        if (checkrow != itm.ItemNo)
                        { //เช็คหาก Decription ไม่เหมือนกันให้สร้าง Row ใหม่ทันที

                            rowdata = rowdata + 1;
                            table += "<tr>";
                            checkrow = itm.Description;
                            table += "<td>" + itm.ItemNo + "</td>";
                            table += "<td>" + itm.Description + "</td>";
                            table += "<td  style=\'text-align :right;\'><button data-animal-type=\'" + itm.ItemNo + "\' data-itemname-type=\'" + itm.Description + "\' data-date1-type=\'" + date1 + "\' onclick=\'selectDetail(this)\' class=\'btn btn-link\'><u>" + SetFontRed(itm.TotalItem, 1) + "</u></button></td>";
                            checkrow = itm.ItemNo; //Adsign ค้่่าให้เท่ากัน
                            countDes = (from x in itemLedgerEntry where x.ItemNo == itm.ItemNo select x).Count();


                            //clear table in row
                            for (int i = 0; i < intArray.Length; i++)  //Clear Array แถว
                            {
                                intArray[i] = "<td align =\'right\'>" + SetFontRed(0, 1) + "</td>";
                            }
                        }

                        int j = 0;
                        foreach (var lo in locations as IList<Location>) //เช็คค่าตาม Location
                        {
                            if (lo.LocationCode == itm.LocationCode)
                            {
                                intArray[j] = "<td align =\'right\'>" + SetFontRed(itm.Quantity, 1) + "</td>";
                            }
                            j++; //เพิ่มค่าเช็คช่อง Array
                        }


                        if (countDes == countDesrowNo)
                        {
                            //return value to row
                            for (int i = 0; i < intArray.Length; i++)
                            {
                                table += intArray[i];
                            }
                            table += "</tr>";

                        }

                        countDes -= 1;
                    }
                }



            }


            

            var  table1 = "</tbody></table>";
            var total_table = head + table+ footer+ table1;
            //total_table = query;
            response = Ok(new { table = total_table,rowdata= rowdata,columdata= columdata });
            //response = Ok(new { table = query });
            //response = Ok(new { table = queryLocation });
            return response;

        }



        [HttpGet]
        public JsonResult GetProvinceOfBirth(string fetch)
        {
           

            var query = "SELECT DISTINCT dbo."+ Environment.GetEnvironmentVariable("Company") +"Item Ledger Entry].[Location Code] AS JobNo,dbo."+ Environment.GetEnvironmentVariable("Company") +"Item Ledger Entry].[Location Code]AS LocationCode FROM dbo."+ Environment.GetEnvironmentVariable("Company") +"Item Ledger Entry] ORDER BY dbo."+ Environment.GetEnvironmentVariable("Company") +"Item Ledger Entry].[Location Code]";
            List<V_Job> jobNo = _navcontext.v_Job.FromSqlRaw(query).ToList();
           
            return Json(jobNo);
        }




        [HttpGet]
        public IActionResult ItemBySite(string itemno,string date1)
        {

            /*Check Session */
            var page = "41";
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
            var query = " SELECT DISTINCT dbo."+ Environment.GetEnvironmentVariable("Company") + "Item Ledger Entry].[Location Code] AS LocationCode,ROW_NUMBER() OVER (ORDER BY dbo." + Environment.GetEnvironmentVariable("Company") + "Item Ledger Entry].[Location Code]) as ID,convert(varchar,FORMAT(sum(dbo." + Environment.GetEnvironmentVariable("Company") +"Item Ledger Entry].Quantity),'###,###,###.00','en-US')) as Total FROM dbo."+ Environment.GetEnvironmentVariable("Company") +"Item Ledger Entry] "
                     + "LEFT JOIN dbo."+ Environment.GetEnvironmentVariable("Company") +"Item] ON dbo."+ Environment.GetEnvironmentVariable("Company") +"Item Ledger Entry].[Item No_] = dbo."+ Environment.GetEnvironmentVariable("Company") +"Item].No_ "
                     + " WHERE  dbo."+ Environment.GetEnvironmentVariable("Company") +"Item Ledger Entry].[Posting Date]>='2018-10-23 00:00:00' and dbo."+ Environment.GetEnvironmentVariable("Company") +"Item Ledger Entry].[Posting Date]<={1} and [Item No_]={0} "
                     + " GROUP BY dbo."+ Environment.GetEnvironmentVariable("Company") +"Item Ledger Entry].[Location Code] HAVING sum(dbo."+ Environment.GetEnvironmentVariable("Company") +"Item Ledger Entry].Quantity)<>0 ";

            //SqlParameter parameterItem = new SqlParameter("@item", itemno);
            //SqlParameter parameterDate1 = new SqlParameter("@date1", date1);





            //ViewBag.sql = queryData;
            var itemBySites = _navcontext.ItemBySites.FromSqlRaw(query, itemno, date1).ToList();

            response = Ok(new { data = itemBySites });


            return response;
        }





        public static string SetFontRed(decimal value1, int level) //Check Color
        {
            string result = "";

            if (level == 1)
            {

                if (value1 < 0)
                {
                    result = "<font color='red'><b>" + value1.ToString("#,##0.00") + "</b></font>";
                }
                else
                {
                    result = "<font color='black'><b>" + value1.ToString("#,##0.00") + "</b></font>";
                }
            }
            else if (level == 3)
            {
                if (value1 < 0)
                {
                    result = "<font color='red'>" + value1.ToString("#,##0.00") + "</font>";
                }
                else
                {
                    result = "<font color='black'>" + value1.ToString("#,##0.00") + "</font>";
                }

            }
            return result;
        }

    }
}