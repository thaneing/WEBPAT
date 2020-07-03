using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;
using CESAPSCOREWEBAPP.Helpers;
using CESAPSCOREWEBAPP.Models;
using DevExtreme.AspNet.Data;
using DevExtreme.AspNet.Mvc;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using static CESAPSCOREWEBAPP.Models.Enums;

namespace CESAPSCOREWEBAPP.Controllers
{
    public class ItemCodesController : BaseController
    {
        private readonly NAVContext _navcontext;
        private readonly DatabaseContext _context;


        public ItemCodesController(NAVContext navcontext, DatabaseContext context)
        {
            _context = context;
            _navcontext = navcontext;
        }


        public IActionResult Index()
        {

            /*Check Session */
            var page = "157";
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

            var queryData = "SELECT Description as code,Description as name FROM dbo."+ Environment.GetEnvironmentVariable("Company") +"Inventory Posting Group] order by Description asc ";
            var sourceAutoCompletes = _navcontext.sourceAutoCompletes.FromSqlRaw(queryData).ToList();
            ViewData["SourceAutoCompletes"] = sourceAutoCompletes;




            //var table = "<table id='dt1' class='table'><thead><tr>"
            // + "<th align ='center'>POSTING</th>"
            // + "<th align ='center'>EntryNo</th>"
            // + "<th align ='center'>VendorEntry</th>"
            // + "<th align ='center'>DocumentNo</th>"
            // + "<th align ='center'>VendorName</th>"
            // + "<th align ='center'>Group</th>"
            // + "<th align ='center'>Type</th>"
            // + "<th align ='center'>Amount</th>"
            // + "<th align ='center'>AmountLCY</th>"
            // + "<th align ='center'>UserId</th>"
            // + "<th align ='center'>Code</th>"
            // + "<th align ='center'>DueDate</th>"
            // + "<th align ='center'>Dim1</th>"
            // + "<th align ='center'>Dim2</th>"
            //+ "<th align ='center'>count</th>"
            //+ "<th align ='center'>DocuemntPay</th>"
            // + "</tr>"
            // + "</thead>"
            // + "<tbody>";


            //foreach (var std in APPVDATA as IList<V_APPV>)
            //{

            //    table += "<tr>"
            //              + "<td>" + std.PostingDate + "</td>"
            //              + "<td>" + std.EntryNo + "</td>"
            //              + "<td>" + std.VendorLedgerEntry + "</td>"
            //              + "<td>" + std.DocumentNo + "</td>"
            //              + "<td>" + std.VendorName + "</td>"
            //              + "<td>" + std.VendorPostingGroup + "</td>"
            //              + "<td>" + std.DocumentType + "</td>"
            //              + "<td align ='right'>" + SetFontRed(std.Amount, 3) + "</td>"
            //              + "<td align ='right'>" + SetFontRed(std.AmountLCY, 3) + "</td >"
            //              + "<td>" + std.UserId + "</td>"
            //              + "<td>" + std.SourceCode + "</td>"
            //              + "<td>" + std.InitialEntryDueDate + "</td>"
            //              + "<td>" + std.InitialEntryGlobalDim1 + "</td>"
            //              + "<td>" + std.InitialEntryGlobalDim2 + "</td>"
            //              + "<td>" + std.CountDocument + "</td>"
            //               + "<td>" + std.Documentname + "</td>"
            //              + "</tr>";
            //}

            //table += "</tbody></table>";

            //ViewBag.table = table;

            return View();
        }

        public IActionResult GetData()
        {

            /*Check Session */
            var page = "157";
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
            var queryData = " SELECT ROW_NUMBER() OVER (ORDER BY dbo." + Environment.GetEnvironmentVariable("Company") + "Item].No_ ) as ID,CONCAT('<a data-animal-type=''',dbo." + Environment.GetEnvironmentVariable("Company") +"Item].No_ ,''' data-item-type=''', dbo."+ Environment.GetEnvironmentVariable("Company") +"Item].Description ,''' onclick=''selectDetail(this)'' class=''btn-link fullscreen''><u>',dbo."+ Environment.GetEnvironmentVariable("Company") +"Item].No_ , '</u></button>') as ItemNo,"

                + "dbo."+ Environment.GetEnvironmentVariable("Company") +"Item].[No_ 2] as ItemNo2,dbo."+ Environment.GetEnvironmentVariable("Company") +"Item].Description as Description,dbo."+ Environment.GetEnvironmentVariable("Company") +"Item].[Description 2] as Description2,dbo."+ Environment.GetEnvironmentVariable("Company") +"Item].[Base Unit of Measure] as Unit,dbo."+ Environment.GetEnvironmentVariable("Company") +"Inventory Posting Group].Description as Groupcode, dbo."+ Environment.GetEnvironmentVariable("Company") +"Item].[Global Dimension 2 Code] as Costcode FROM dbo."+ Environment.GetEnvironmentVariable("Company") +"Item] " +
                " LEFT JOIN dbo."+ Environment.GetEnvironmentVariable("Company") +"Inventory Posting Group] ON dbo."+ Environment.GetEnvironmentVariable("Company") +"Item].[Inventory Posting Group] = dbo."+ Environment.GetEnvironmentVariable("Company") +"Inventory Posting Group].Code ";


            //ViewBag.sql = queryData;
            var v_ItemCodes = _navcontext.v_ItemCodes.FromSqlRaw(queryData).ToList();


            response = Ok(new { data = v_ItemCodes });


            return response;

        }



        public IActionResult GetItem(string Item)
        {

            /*Check Session */
            var page = "157";
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
            var queryData = "SELECT ROW_NUMBER() OVER (ORDER BY dbo." + Environment.GetEnvironmentVariable("Company") + "Purchase Header].[Order Date]) as ID,"
                          + " convert(varchar, dbo."+ Environment.GetEnvironmentVariable("Company") +"Purchase Header].[Order Date] , 23) as OrderDate,"
                          + " dbo."+ Environment.GetEnvironmentVariable("Company") +"Purchase Line].[Document No_] as DocumentNo,"
                          + " dbo."+ Environment.GetEnvironmentVariable("Company") +"Purchase Line].[Buy-from Vendor No_] As VendorNo,"
                          + " dbo."+ Environment.GetEnvironmentVariable("Company") +"Vendor].Name as VendorName,"
                          + " dbo."+ Environment.GetEnvironmentVariable("Company") +"Purchase Line].No_ As ItemNo,"
                          + " dbo."+ Environment.GetEnvironmentVariable("Company") +"Purchase Line].[Location Code] As Location,"
                          + " dbo."+ Environment.GetEnvironmentVariable("Company") +"Purchase Line].Description As Description,"
                          + " dbo."+ Environment.GetEnvironmentVariable("Company") +"Purchase Line].[Description 2] As Description2,"
                          + " dbo."+ Environment.GetEnvironmentVariable("Company") +"Purchase Line].[Unit of Measure] As UnitOfMeasure,"
                          + " dbo."+ Environment.GetEnvironmentVariable("Company") +"Purchase Line].Quantity  as Quantity,"
                          + " dbo."+ Environment.GetEnvironmentVariable("Company") +"Purchase Line].[Quantity Received]  AS Receive,"
                          + " dbo."+ Environment.GetEnvironmentVariable("Company") +"Purchase Line].[Direct Unit Cost] as UnitCost,"
                          + " dbo."+ Environment.GetEnvironmentVariable("Company") +"Purchase Line].Amount  as Amount,"
                          + " dbo."+ Environment.GetEnvironmentVariable("Company") +"Purchase Line].[Amount Including VAT] as AmountInludVat,"
                          + " dbo."+ Environment.GetEnvironmentVariable("Company") +"Purchase Line].[Job No_] As JobNo, "
                          + " dbo."+ Environment.GetEnvironmentVariable("Company") +"Purchase Line].[Job Task No_] As JobTaskNo,"
                          + " dbo."+ Environment.GetEnvironmentVariable("Company") +"Purchase Header].[Posting Description] As RefPR," +
                          " dbo."+ Environment.GetEnvironmentVariable("Company") +"Item].[Inventory Posting Group] AS InventoryPostingGroupCode," +
                          " dbo."+ Environment.GetEnvironmentVariable("Company") +"Inventory Posting Group].Description as InventoryPostingGroupName, " +
                          " dbo."+ Environment.GetEnvironmentVariable("Company") +"Purchase Line].[Quantity Received]*dbo."+ Environment.GetEnvironmentVariable("Company") +"Purchase Line].[Direct Unit Cost] as TotalReceive"
                          + " FROM dbo."+ Environment.GetEnvironmentVariable("Company") +"Purchase Line] "
                          + " LEFT JOIN dbo."+ Environment.GetEnvironmentVariable("Company") +"Vendor] ON dbo."+ Environment.GetEnvironmentVariable("Company") +"Purchase Line].[Buy-from Vendor No_] = dbo."+ Environment.GetEnvironmentVariable("Company") +"Vendor].No_"
                          + " LEFT JOIN dbo."+ Environment.GetEnvironmentVariable("Company") +"Purchase Header] ON dbo."+ Environment.GetEnvironmentVariable("Company") +"Purchase Header].No_ = dbo."+ Environment.GetEnvironmentVariable("Company") +"Purchase Line].[Document No_]"
                          + " LEFT JOIN dbo."+ Environment.GetEnvironmentVariable("Company") +"Item] ON dbo."+ Environment.GetEnvironmentVariable("Company") +"Purchase Line].No_ = dbo."+ Environment.GetEnvironmentVariable("Company") +"Item].No_ "
                          + " LEFT JOIN dbo."+ Environment.GetEnvironmentVariable("Company") +"Inventory Posting Group] ON dbo."+ Environment.GetEnvironmentVariable("Company") +"Item].[Inventory Posting Group] = dbo."+ Environment.GetEnvironmentVariable("Company") +"Inventory Posting Group].Code "

                            + " WHERE dbo."+ Environment.GetEnvironmentVariable("Company") +"Purchase Line].No_ = {0}"
                            + " ORDER BY dbo."+ Environment.GetEnvironmentVariable("Company") +"Vendor].[timestamp] DESC";

            //SqlParameter parameterItem = new SqlParameter("@item", Item);

            //ViewBag.sql = queryData;
            var V_OrderPurchaseLines = _navcontext.v_OrderPurchaseLines.FromSqlRaw(queryData,Item).ToList();


            response = Ok(new { data = V_OrderPurchaseLines });


            return response;

        }





        public object GetItemChart(string Item, DataSourceLoadOptions loadOptions)
        {

            /*Check Session */
            var page = "157";
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
            var queryData = "SELECT ROW_NUMBER() OVER (ORDER BY dbo." + Environment.GetEnvironmentVariable("Company") + "Purchase Header].[Order Date]) as ID,"
                          + " LEFT(CONVERT(varchar, dbo."+ Environment.GetEnvironmentVariable("Company") +"Purchase Header].[Order Date] ,112),6) as OrderDate,"
                          + " dbo."+ Environment.GetEnvironmentVariable("Company") +"Purchase Line].[Document No_] as DocumentNo,"
                          + " dbo."+ Environment.GetEnvironmentVariable("Company") +"Purchase Line].[Buy-from Vendor No_] As VendorNo,"
                          + " dbo."+ Environment.GetEnvironmentVariable("Company") +"Vendor].Name as VendorName,"
                          + " dbo."+ Environment.GetEnvironmentVariable("Company") +"Purchase Line].No_ As ItemNo,"
                          + " dbo."+ Environment.GetEnvironmentVariable("Company") +"Purchase Line].[Location Code] As Location,"
                          + " dbo."+ Environment.GetEnvironmentVariable("Company") +"Purchase Line].Description As Description,"
                          + " dbo."+ Environment.GetEnvironmentVariable("Company") +"Purchase Line].[Description 2] As Description2,"
                          + " dbo."+ Environment.GetEnvironmentVariable("Company") +"Purchase Line].[Unit of Measure] As UnitOfMeasure,"
                          + " dbo."+ Environment.GetEnvironmentVariable("Company") +"Purchase Line].Quantity  as Quantity,"
                          + " dbo."+ Environment.GetEnvironmentVariable("Company") +"Purchase Line].[Quantity Received]  AS Receive,"
                          + " dbo."+ Environment.GetEnvironmentVariable("Company") +"Purchase Line].[Direct Unit Cost] as UnitCost,"
                          + " dbo."+ Environment.GetEnvironmentVariable("Company") +"Purchase Line].Amount  as Amount,"
                          + " dbo."+ Environment.GetEnvironmentVariable("Company") +"Purchase Line].[Amount Including VAT] as AmountInludVat,"
                          + " dbo."+ Environment.GetEnvironmentVariable("Company") +"Purchase Line].[Job No_] As JobNo, "
                          + " dbo."+ Environment.GetEnvironmentVariable("Company") +"Purchase Line].[Job Task No_] As JobTaskNo,"
                          + " dbo."+ Environment.GetEnvironmentVariable("Company") +"Purchase Header].[Posting Description] As RefPR," +
                          " dbo."+ Environment.GetEnvironmentVariable("Company") +"Item].[Inventory Posting Group] AS InventoryPostingGroupCode," +
                          " dbo."+ Environment.GetEnvironmentVariable("Company") +"Inventory Posting Group].Description as InventoryPostingGroupName, " +
                          " dbo."+ Environment.GetEnvironmentVariable("Company") +"Purchase Line].[Quantity Received]*dbo."+ Environment.GetEnvironmentVariable("Company") +"Purchase Line].[Direct Unit Cost] as TotalReceive"
                          + " FROM dbo."+ Environment.GetEnvironmentVariable("Company") +"Purchase Line] "
                          + " LEFT JOIN dbo."+ Environment.GetEnvironmentVariable("Company") +"Vendor] ON dbo."+ Environment.GetEnvironmentVariable("Company") +"Purchase Line].[Buy-from Vendor No_] = dbo."+ Environment.GetEnvironmentVariable("Company") +"Vendor].No_"
                          + " LEFT JOIN dbo."+ Environment.GetEnvironmentVariable("Company") +"Purchase Header] ON dbo."+ Environment.GetEnvironmentVariable("Company") +"Purchase Header].No_ = dbo."+ Environment.GetEnvironmentVariable("Company") +"Purchase Line].[Document No_]"
                          + " LEFT JOIN dbo."+ Environment.GetEnvironmentVariable("Company") +"Item] ON dbo."+ Environment.GetEnvironmentVariable("Company") +"Purchase Line].No_ = dbo."+ Environment.GetEnvironmentVariable("Company") +"Item].No_ "
                          + " LEFT JOIN dbo."+ Environment.GetEnvironmentVariable("Company") +"Inventory Posting Group] ON dbo."+ Environment.GetEnvironmentVariable("Company") +"Item].[Inventory Posting Group] = dbo."+ Environment.GetEnvironmentVariable("Company") +"Inventory Posting Group].Code "

                            + " WHERE dbo."+ Environment.GetEnvironmentVariable("Company") +"Purchase Line].No_ = {0}"
                            + " ORDER BY dbo."+ Environment.GetEnvironmentVariable("Company") +"Purchase Header].[Order Date]  ASC";

            //SqlParameter parameterItem = new SqlParameter("@item", Item);

            //ViewBag.sql = queryData;
            var V_OrderPurchaseLines = _navcontext.v_OrderPurchaseLines.FromSqlRaw(queryData, Item).ToList();


            //response = Ok(V_OrderPurchaseLines);

            return DataSourceLoader.Load(V_OrderPurchaseLines, loadOptions);
            //return response;

        }


        public IActionResult Vendor()
        {

            /*Check Session */
            var page = "222";
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



        public IActionResult GetDataVendor()
        {

            /*Check Session */
            var page = "222";
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
            var queryData = " SELECT DISTINCT CONCAT('<a data-animal-type=''',dbo."+ Environment.GetEnvironmentVariable("Company") +"Purchase Header].[Buy-from Vendor No_] ,''' data-item-type=''', dbo."+ Environment.GetEnvironmentVariable("Company") +"Purchase Header].[Buy-from Vendor Name] ,''' onclick=''selectDetail(this)'' class=''btn-link fullscreen''><u>',dbo."+ Environment.GetEnvironmentVariable("Company") +"Purchase Header].[Buy-from Vendor No_], '</u></button>') AS ItemNo,"

                + " '' AS ItemNo2,ROW_NUMBER() OVER (ORDER BY dbo." + Environment.GetEnvironmentVariable("Company") + "Purchase Header].[Buy-from Vendor Name]) AS ID,dbo." + Environment.GetEnvironmentVariable("Company") +"Purchase Header].[Buy-from Vendor Name] AS Description,'' AS Description2,'' AS Unit,convert(varchar,FORMAT(sum(dbo."+ Environment.GetEnvironmentVariable("Company") +"Purchase Header].[Amount Receipt]),'###,###,###.00','en-US')) AS Groupcode,'' AS Costcode FROM dbo."+ Environment.GetEnvironmentVariable("Company") +"Purchase Header]  " +
                " group by	dbo."+ Environment.GetEnvironmentVariable("Company") +"Purchase Header].[Buy-from Vendor No_],dbo."+ Environment.GetEnvironmentVariable("Company") +"Purchase Header].[Buy-from Vendor Name] " +
                " HAVING(dbo."+ Environment.GetEnvironmentVariable("Company") +"Purchase Header].[Buy-from Vendor Name]<>'') ";


            //ViewBag.sql = queryData;
            var v_ItemCodes = _navcontext.v_ItemCodes.FromSqlRaw(queryData).ToList();


            response = Ok(new { data = v_ItemCodes });


            return response;

        }


        public IActionResult GetVendor(string Item)
        {

            /*Check Session */
            var page = "222";
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
            var queryData = "SELECT " +
                "ROW_NUMBER() OVER ( ORDER BY dbo." + Environment.GetEnvironmentVariable("Company") + "Purchase Header].[Order Date]) AS ID,"
                          + " convert(varchar, dbo."+ Environment.GetEnvironmentVariable("Company") +"Purchase Header].[Order Date] , 23) as OrderDate,"
                          + " dbo."+ Environment.GetEnvironmentVariable("Company") +"Purchase Line].[Document No_] as DocumentNo,"
                          + " dbo."+ Environment.GetEnvironmentVariable("Company") +"Purchase Line].[Buy-from Vendor No_] As VendorNo,"
                          + " dbo."+ Environment.GetEnvironmentVariable("Company") +"Vendor].Name as VendorName,"
                          + " dbo."+ Environment.GetEnvironmentVariable("Company") +"Purchase Line].No_ As ItemNo,"
                          + " dbo."+ Environment.GetEnvironmentVariable("Company") +"Purchase Line].[Location Code] As Location,"
                          + " dbo."+ Environment.GetEnvironmentVariable("Company") +"Purchase Line].Description As Description,"
                          + " dbo."+ Environment.GetEnvironmentVariable("Company") +"Purchase Line].[Description 2] As Description2,"
                          + " dbo."+ Environment.GetEnvironmentVariable("Company") +"Purchase Line].[Unit of Measure] As UnitOfMeasure,"
                          + " dbo."+ Environment.GetEnvironmentVariable("Company") +"Purchase Line].Quantity  as Quantity,"
                          + " dbo."+ Environment.GetEnvironmentVariable("Company") +"Purchase Line].[Quantity Received]  AS Receive,"
                          + " dbo."+ Environment.GetEnvironmentVariable("Company") +"Purchase Line].[Direct Unit Cost] as UnitCost,"
                          + " dbo."+ Environment.GetEnvironmentVariable("Company") +"Purchase Line].Amount  as Amount,"
                          + " dbo."+ Environment.GetEnvironmentVariable("Company") +"Purchase Line].[Amount Including VAT] as AmountInludVat,"
                          + " dbo."+ Environment.GetEnvironmentVariable("Company") +"Purchase Line].[Job No_] As JobNo, "
                          + " dbo."+ Environment.GetEnvironmentVariable("Company") +"Purchase Line].[Job Task No_] As JobTaskNo,"
                          + " dbo."+ Environment.GetEnvironmentVariable("Company") +"Purchase Header].[Posting Description] As RefPR," +
                          " dbo."+ Environment.GetEnvironmentVariable("Company") +"Item].[Inventory Posting Group] AS InventoryPostingGroupCode," +
                          " dbo."+ Environment.GetEnvironmentVariable("Company") +"Inventory Posting Group].Description as InventoryPostingGroupName, " +
                          " dbo."+ Environment.GetEnvironmentVariable("Company") +"Purchase Line].[Quantity Received]*dbo."+ Environment.GetEnvironmentVariable("Company") +"Purchase Line].[Direct Unit Cost] as TotalReceive"
                          + " FROM dbo."+ Environment.GetEnvironmentVariable("Company") +"Purchase Line] "
                          + " LEFT JOIN dbo."+ Environment.GetEnvironmentVariable("Company") +"Vendor] ON dbo."+ Environment.GetEnvironmentVariable("Company") +"Purchase Line].[Buy-from Vendor No_] = dbo."+ Environment.GetEnvironmentVariable("Company") +"Vendor].No_"
                          + " LEFT JOIN dbo."+ Environment.GetEnvironmentVariable("Company") +"Purchase Header] ON dbo."+ Environment.GetEnvironmentVariable("Company") +"Purchase Header].No_ = dbo."+ Environment.GetEnvironmentVariable("Company") +"Purchase Line].[Document No_]"
                          + " LEFT JOIN dbo."+ Environment.GetEnvironmentVariable("Company") +"Item] ON dbo."+ Environment.GetEnvironmentVariable("Company") +"Purchase Line].No_ = dbo."+ Environment.GetEnvironmentVariable("Company") +"Item].No_ "
                          + " LEFT JOIN dbo."+ Environment.GetEnvironmentVariable("Company") +"Inventory Posting Group] ON dbo."+ Environment.GetEnvironmentVariable("Company") +"Item].[Inventory Posting Group] = dbo."+ Environment.GetEnvironmentVariable("Company") +"Inventory Posting Group].Code "

                            + " WHERE dbo."+ Environment.GetEnvironmentVariable("Company") +"Purchase Line].[Buy-from Vendor No_] = {0}"
                            + " ORDER BY dbo."+ Environment.GetEnvironmentVariable("Company") +"Vendor].[timestamp] DESC";

            //SqlParameter parameterItem = new SqlParameter("@item", Item);

            //ViewBag.sql = queryData;
            var V_OrderPurchaseLines = _navcontext.v_OrderPurchaseLines.FromSqlRaw(queryData, Item).ToList();


            response = Ok(new { data = V_OrderPurchaseLines });


            return response;

        }


        public IActionResult AllPurchase()
        {
            /*Check Session */
            var page = "224";
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

       public IActionResult GetAllPurchase()
        {

            /*Check Session */
            var page = "224";
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
            var queryData = "SELECT ROW_NUMBER() OVER (ORDER BY dbo." + Environment.GetEnvironmentVariable("Company") + "Purchase Header].[Order Date]) as ID,"
                          + " convert(varchar, dbo." + Environment.GetEnvironmentVariable("Company") + "Purchase Header].[Order Date] , 23) as OrderDate,"
                          + " dbo." + Environment.GetEnvironmentVariable("Company") + "Purchase Line].[Document No_] as DocumentNo,"
                          + " dbo." + Environment.GetEnvironmentVariable("Company") + "Purchase Line].[Buy-from Vendor No_] As VendorNo,"
                          + " dbo." + Environment.GetEnvironmentVariable("Company") + "Vendor].Name as VendorName,"
                          + " dbo." + Environment.GetEnvironmentVariable("Company") + "Purchase Line].No_ As ItemNo,"
                          + " dbo." + Environment.GetEnvironmentVariable("Company") + "Purchase Line].[Location Code] As Location,"
                          + " dbo." + Environment.GetEnvironmentVariable("Company") + "Purchase Line].Description As Description,"
                          + " dbo." + Environment.GetEnvironmentVariable("Company") + "Purchase Line].[Description 2] As Description2,"
                          + " dbo." + Environment.GetEnvironmentVariable("Company") + "Purchase Line].[Unit of Measure] As UnitOfMeasure,"
                          + " dbo." + Environment.GetEnvironmentVariable("Company") + "Purchase Line].Quantity  as Quantity,"
                          + " dbo." + Environment.GetEnvironmentVariable("Company") + "Purchase Line].[Quantity Received]  AS Receive,"
                          + " dbo." + Environment.GetEnvironmentVariable("Company") + "Purchase Line].[Direct Unit Cost] as UnitCost,"
                          + " dbo." + Environment.GetEnvironmentVariable("Company") + "Purchase Line].Amount  as Amount,"
                          + " dbo." + Environment.GetEnvironmentVariable("Company") + "Purchase Line].[Amount Including VAT] as AmountInludVat,"
                          + " dbo." + Environment.GetEnvironmentVariable("Company") + "Purchase Line].[Job No_] As JobNo, "
                          + " dbo." + Environment.GetEnvironmentVariable("Company") + "Purchase Line].[Job Task No_] As JobTaskNo,"
                          + " dbo." + Environment.GetEnvironmentVariable("Company") + "Purchase Header].[Posting Description] As RefPR," +
                          " dbo." + Environment.GetEnvironmentVariable("Company") + "Item].[Inventory Posting Group] AS InventoryPostingGroupCode," +
                          " dbo." + Environment.GetEnvironmentVariable("Company") + "Inventory Posting Group].Description as InventoryPostingGroupName, " +
                          " dbo." + Environment.GetEnvironmentVariable("Company") + "Purchase Line].[Quantity Received]*dbo." + Environment.GetEnvironmentVariable("Company") + "Purchase Line].[Direct Unit Cost] as TotalReceive"
                          + " FROM dbo." + Environment.GetEnvironmentVariable("Company") + "Purchase Line] "
                          + " LEFT JOIN dbo." + Environment.GetEnvironmentVariable("Company") + "Vendor] ON dbo." + Environment.GetEnvironmentVariable("Company") + "Purchase Line].[Buy-from Vendor No_] = dbo." + Environment.GetEnvironmentVariable("Company") + "Vendor].No_"
                          + " LEFT JOIN dbo." + Environment.GetEnvironmentVariable("Company") + "Purchase Header] ON dbo." + Environment.GetEnvironmentVariable("Company") + "Purchase Header].No_ = dbo." + Environment.GetEnvironmentVariable("Company") + "Purchase Line].[Document No_]"
                          + " LEFT JOIN dbo." + Environment.GetEnvironmentVariable("Company") + "Item] ON dbo." + Environment.GetEnvironmentVariable("Company") + "Purchase Line].No_ = dbo." + Environment.GetEnvironmentVariable("Company") + "Item].No_ "
                          + " LEFT JOIN dbo." + Environment.GetEnvironmentVariable("Company") + "Inventory Posting Group] ON dbo." + Environment.GetEnvironmentVariable("Company") + "Item].[Inventory Posting Group] = dbo." + Environment.GetEnvironmentVariable("Company") + "Inventory Posting Group].Code "

                            + " WHERE dbo.[C_E_S_ CO_, LTD_$Purchase Line].Description !='' and dbo.[C_E_S_ CO_, LTD_$Purchase Line].[Description 2]!='' and dbo.[C_E_S_ CO_, LTD_$Purchase Line].[Document No_]!='' "
                            + " ORDER BY dbo." + Environment.GetEnvironmentVariable("Company") + "Vendor].[timestamp] DESC";

            //SqlParameter parameterItem = new SqlParameter("@item", Item);

            //ViewBag.sql = queryData;
            var V_OrderPurchaseLines = _navcontext.v_OrderPurchaseLines.FromSqlRaw(queryData).ToList();


            response = Ok(new { data = V_OrderPurchaseLines });


            return response;

        }











    }
}
