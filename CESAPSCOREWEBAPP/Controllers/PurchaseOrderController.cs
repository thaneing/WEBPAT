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
    public class PurchaseOrderController : BaseController
    {

        private readonly NAVContext _navcontext;
        private readonly DatabaseContext _context;


        public PurchaseOrderController(NAVContext navcontext, DatabaseContext context)
        {
            _context = context;
            _navcontext = navcontext;
        }

        public IActionResult Index()
        {

            /*Check Session */
            var page = "211";
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


            var queryData = "SELECT DISTINCT dbo."+ Environment.GetEnvironmentVariable("Company") +"Purchase Line].[Location Code] as name, dbo."+ Environment.GetEnvironmentVariable("Company") +"Purchase Line].[Location Code] as code FROM dbo."+ Environment.GetEnvironmentVariable("Company") +"Purchase Line] WHERE dbo."+ Environment.GetEnvironmentVariable("Company") +"Purchase Line].[Location Code]!='' ORDER BY dbo."+ Environment.GetEnvironmentVariable("Company") +"Purchase Line].[Location Code]";
            var sourceAutoCompletes = _navcontext.sourceAutoCompletes.FromSqlRaw(queryData).ToList();
            var CountSource=sourceAutoCompletes.Count;




            string[] terms = new string[CountSource];
            var i = 0;
            foreach (var std in sourceAutoCompletes as IList<SourceAutoComplete>)
            {
                terms[i] = std.name;
                i++;
            }



            var queryData1 = "SELECT DISTINCT dbo."+ Environment.GetEnvironmentVariable("Company") +"Job Planning Line].[Job Task No_] as name,dbo."+ Environment.GetEnvironmentVariable("Company") +"Job Planning Line].[Job Task No_] as code FROM dbo."+ Environment.GetEnvironmentVariable("Company") +"Job Planning Line] ORDER BY dbo."+ Environment.GetEnvironmentVariable("Company") +"Job Planning Line].[Job Task No_]";
            var sourceAutoCompletes1 = _navcontext.sourceAutoCompletes.FromSqlRaw(queryData1).ToList();
            var CountSource1 = sourceAutoCompletes1.Count;

            string[] terms1 = new string[CountSource1];
            var j = 0;
            foreach (var std in sourceAutoCompletes1 as IList<SourceAutoComplete>)
            {
                terms1[j] = std.name;
                j++;
            }


            ViewBag.CountSource1 = CountSource;

            ViewBag.SourceAutoCompletes1 = terms1;


            ViewBag.CountSource = CountSource;

            ViewBag.SourceAutoCompletes = terms;

            return View();
        }

        public IActionResult GetData()
        {
            /*Check Session */
            var page = "211";
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
                          + " dbo."+ Environment.GetEnvironmentVariable("Company") +"Purchase Line].[Ref_ PR No_] As RefPR," +
                          " dbo."+ Environment.GetEnvironmentVariable("Company") +"Item].[Inventory Posting Group] AS InventoryPostingGroupCode," +
                          " dbo."+ Environment.GetEnvironmentVariable("Company") +"Inventory Posting Group].Description as InventoryPostingGroupName, " +
                          " dbo."+ Environment.GetEnvironmentVariable("Company") +"Purchase Line].[Quantity Received]*dbo."+ Environment.GetEnvironmentVariable("Company") +"Purchase Line].[Direct Unit Cost] as TotalReceive"
                          + " FROM dbo."+ Environment.GetEnvironmentVariable("Company") +"Purchase Line] "
                          + " LEFT JOIN dbo."+ Environment.GetEnvironmentVariable("Company") +"Vendor] ON dbo."+ Environment.GetEnvironmentVariable("Company") +"Purchase Line].[Buy-from Vendor No_] = dbo."+ Environment.GetEnvironmentVariable("Company") +"Vendor].No_"
                          + " LEFT JOIN dbo."+ Environment.GetEnvironmentVariable("Company") +"Purchase Header] ON dbo."+ Environment.GetEnvironmentVariable("Company") +"Purchase Header].No_ = dbo."+ Environment.GetEnvironmentVariable("Company") +"Purchase Line].[Document No_]"
                          + " LEFT JOIN dbo."+ Environment.GetEnvironmentVariable("Company") +"Item] ON dbo."+ Environment.GetEnvironmentVariable("Company") +"Purchase Line].No_ = dbo."+ Environment.GetEnvironmentVariable("Company") +"Item].No_ "
                          + " LEFT JOIN dbo."+ Environment.GetEnvironmentVariable("Company") +"Inventory Posting Group] ON dbo."+ Environment.GetEnvironmentVariable("Company") +"Item].[Inventory Posting Group] = dbo."+ Environment.GetEnvironmentVariable("Company") +"Inventory Posting Group].Code "
                          + " WHERE  dbo."+ Environment.GetEnvironmentVariable("Company") +"Purchase Line].No_ !='' and dbo."+ Environment.GetEnvironmentVariable("Company") +"Item].[Inventory Posting Group] >='01' and dbo."+ Environment.GetEnvironmentVariable("Company") +"Item].[Inventory Posting Group]<='15'" +
                          " ORDER BY dbo."+ Environment.GetEnvironmentVariable("Company") +"Vendor].[timestamp] DESC";


            //ViewBag.sql = queryData;
            var V_OrderPurchaseLines = _navcontext.v_OrderPurchaseLines.FromSqlRaw(queryData).ToList();


            //response = Ok(new { data = V_OrderPurchaseLines });

            response = Ok(V_OrderPurchaseLines);


            return response;
        }




        public object Get(DataSourceLoadOptions loadOptions)
        {
         
         
            var queryData = "SELECT ROW_NUMBER() OVER (ORDER BY dbo." + Environment.GetEnvironmentVariable("Company") + "Purchase Header].[Order Date]) as ID,"
                          + " convert(varchar, dbo."+ Environment.GetEnvironmentVariable("Company") +"Purchase Header].[Order Date] , 23) as OrderDate,"
                          + " dbo."+ Environment.GetEnvironmentVariable("Company") +"Purchase Line].[Document No_] as DocumentNo,"
                          + " dbo."+ Environment.GetEnvironmentVariable("Company") +"Purchase Line].[Buy-from Vendor No_] As VendorNo,"
                          + " dbo."+ Environment.GetEnvironmentVariable("Company") +"Vendor].Name as VendorName,"
                          + " dbo."+ Environment.GetEnvironmentVariable("Company") +"Purchase Line].No_ As ItemNo,"
                          + " dbo."+ Environment.GetEnvironmentVariable("Company") +"Purchase Header].[Shortcut Dimension 1 Code] As Location,"
                          + " dbo."+ Environment.GetEnvironmentVariable("Company") +"Purchase Line].Description As Description,"
                          + " dbo."+ Environment.GetEnvironmentVariable("Company") +"Purchase Line].[Description 2] As Description2,"
                          + " dbo."+ Environment.GetEnvironmentVariable("Company") +"Purchase Line].[Unit of Measure] As UnitOfMeasure,"
                          + " dbo."+ Environment.GetEnvironmentVariable("Company") +"Purchase Line].Quantity  as Quantity,"
                          + " dbo."+ Environment.GetEnvironmentVariable("Company") +"Purchase Line].[Quantity Received]  AS Receive,"
                          + " dbo."+ Environment.GetEnvironmentVariable("Company") +"Purchase Line].[Direct Unit Cost] as UnitCost,"
                          + " dbo."+ Environment.GetEnvironmentVariable("Company") +"Purchase Line].Amount  as Amount,"
                          + " dbo."+ Environment.GetEnvironmentVariable("Company") +"Purchase Line].[Amount Including VAT] as AmountInludVat,"
                          + " dbo."+ Environment.GetEnvironmentVariable("Company") +"Purchase Header].[Shortcut Dimension 1 Code] As JobNo, "
                          + " dbo."+ Environment.GetEnvironmentVariable("Company") +"Purchase Line].[Job Task No_] As JobTaskNo,"
                          + " dbo."+ Environment.GetEnvironmentVariable("Company") +"Purchase Line].[Ref_ PR No_] As RefPR," +
                          " dbo."+ Environment.GetEnvironmentVariable("Company") +"Item].[Inventory Posting Group] AS InventoryPostingGroupCode," +
                          " dbo."+ Environment.GetEnvironmentVariable("Company") +"Inventory Posting Group].Description as InventoryPostingGroupName, " +
                          " dbo."+ Environment.GetEnvironmentVariable("Company") +"Purchase Line].[Quantity Received]*dbo."+ Environment.GetEnvironmentVariable("Company") +"Purchase Line].[Direct Unit Cost] as TotalReceive"
                          + " FROM dbo."+ Environment.GetEnvironmentVariable("Company") +"Purchase Line] "
                          + " LEFT JOIN dbo."+ Environment.GetEnvironmentVariable("Company") +"Vendor] ON dbo."+ Environment.GetEnvironmentVariable("Company") +"Purchase Line].[Buy-from Vendor No_] = dbo."+ Environment.GetEnvironmentVariable("Company") +"Vendor].No_"
                          + " LEFT JOIN dbo."+ Environment.GetEnvironmentVariable("Company") +"Purchase Header] ON dbo."+ Environment.GetEnvironmentVariable("Company") +"Purchase Header].No_ = dbo."+ Environment.GetEnvironmentVariable("Company") +"Purchase Line].[Document No_]"
                          + " LEFT JOIN dbo."+ Environment.GetEnvironmentVariable("Company") +"Item] ON dbo."+ Environment.GetEnvironmentVariable("Company") +"Purchase Line].No_ = dbo."+ Environment.GetEnvironmentVariable("Company") +"Item].No_ "
                          + " LEFT JOIN dbo."+ Environment.GetEnvironmentVariable("Company") +"Inventory Posting Group] ON dbo."+ Environment.GetEnvironmentVariable("Company") +"Item].[Inventory Posting Group] = dbo."+ Environment.GetEnvironmentVariable("Company") +"Inventory Posting Group].Code "
                          + " WHERE  dbo."+ Environment.GetEnvironmentVariable("Company") +"Purchase Line].No_ !='' and dbo."+ Environment.GetEnvironmentVariable("Company") +"Item].[Inventory Posting Group] >='01' and dbo."+ Environment.GetEnvironmentVariable("Company") +"Item].[Inventory Posting Group]<='15' " +
                          " ORDER BY dbo."+ Environment.GetEnvironmentVariable("Company") +"Vendor].[timestamp] DESC";


            //ViewBag.sql = queryData;
            var V_OrderPurchaseLines = _navcontext.v_OrderPurchaseLines.FromSqlRaw(queryData).ToList();

            return DataSourceLoader.Load(V_OrderPurchaseLines, loadOptions);

        }


        public IActionResult POByGroup()
        {
            /*Check Session */
            var page = "212";
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


        public IActionResult POByVendor()
        {
            /*Check Session */
            var page = "213";
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

        public IActionResult POByItem()
        {
            /*Check Session */
            var page = "214";
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



        public object GetDataFilter(DataSourceLoadOptions loadOptions)
        {


            var queryData = "SELECT ROW_NUMBER() OVER (ORDER BY dbo." + Environment.GetEnvironmentVariable("Company") + "Purchase Header].[Order Date]) as ID,"
                          + " dbo."+ Environment.GetEnvironmentVariable("Company") +"Purchase Header].[Order Date]  as OrderDate,"
                          + " dbo."+ Environment.GetEnvironmentVariable("Company") +"Purchase Line].[Document No_] as DocumentNo,"
                          + " dbo."+ Environment.GetEnvironmentVariable("Company") +"Purchase Line].[Buy-from Vendor No_] As VendorNo,"
                          + " dbo."+ Environment.GetEnvironmentVariable("Company") +"Vendor].Name as VendorName,"
                          + " dbo."+ Environment.GetEnvironmentVariable("Company") +"Purchase Line].No_ As ItemNo,"
                          + " dbo."+ Environment.GetEnvironmentVariable("Company") +"Purchase Header].[Shortcut Dimension 1 Code] As Location,"
                          + " dbo."+ Environment.GetEnvironmentVariable("Company") +"Purchase Line].Description As Description,"
                          + " dbo."+ Environment.GetEnvironmentVariable("Company") +"Purchase Line].[Description 2] As Description2,"
                          + " dbo."+ Environment.GetEnvironmentVariable("Company") +"Purchase Line].[Unit of Measure] As UnitOfMeasure,"
                          + " dbo."+ Environment.GetEnvironmentVariable("Company") +"Purchase Line].Quantity  as Quantity,"
                          + " dbo."+ Environment.GetEnvironmentVariable("Company") +"Purchase Line].[Quantity Received]  AS Receive,"
                          + " dbo."+ Environment.GetEnvironmentVariable("Company") +"Purchase Line].[Direct Unit Cost] as UnitCost,"
                          + " dbo."+ Environment.GetEnvironmentVariable("Company") +"Purchase Line].Amount  as Amount,"
                          + " dbo."+ Environment.GetEnvironmentVariable("Company") +"Purchase Line].[Amount Including VAT] as AmountInludVat,"
                          + " dbo."+ Environment.GetEnvironmentVariable("Company") +"Purchase Header].[Shortcut Dimension 1 Code] As JobNo, "
                          + " dbo."+ Environment.GetEnvironmentVariable("Company") +"Purchase Line].[Job Task No_] As JobTaskNo,"
                          + " dbo."+ Environment.GetEnvironmentVariable("Company") +"Purchase Line].[Ref_ PR No_] As RefPR," +
                          " dbo."+ Environment.GetEnvironmentVariable("Company") +"Item].[Inventory Posting Group] AS InventoryPostingGroupCode," +
                          " dbo."+ Environment.GetEnvironmentVariable("Company") +"Inventory Posting Group].Description as InventoryPostingGroupName, " +
                          " dbo."+ Environment.GetEnvironmentVariable("Company") +"Purchase Line].[Quantity Received]*dbo."+ Environment.GetEnvironmentVariable("Company") +"Purchase Line].[Direct Unit Cost] as TotalReceive"
                          + " FROM dbo."+ Environment.GetEnvironmentVariable("Company") +"Purchase Line] "
                          + " LEFT JOIN dbo."+ Environment.GetEnvironmentVariable("Company") +"Vendor] ON dbo."+ Environment.GetEnvironmentVariable("Company") +"Purchase Line].[Buy-from Vendor No_] = dbo."+ Environment.GetEnvironmentVariable("Company") +"Vendor].No_"
                          + " LEFT JOIN dbo."+ Environment.GetEnvironmentVariable("Company") +"Purchase Header] ON dbo."+ Environment.GetEnvironmentVariable("Company") +"Purchase Header].No_ = dbo."+ Environment.GetEnvironmentVariable("Company") +"Purchase Line].[Document No_]"
                          + " LEFT JOIN dbo."+ Environment.GetEnvironmentVariable("Company") +"Item] ON dbo."+ Environment.GetEnvironmentVariable("Company") +"Purchase Line].No_ = dbo."+ Environment.GetEnvironmentVariable("Company") +"Item].No_ "
                          + " LEFT JOIN dbo."+ Environment.GetEnvironmentVariable("Company") +"Inventory Posting Group] ON dbo."+ Environment.GetEnvironmentVariable("Company") +"Item].[Inventory Posting Group] = dbo."+ Environment.GetEnvironmentVariable("Company") +"Inventory Posting Group].Code "
                          + " WHERE  dbo."+ Environment.GetEnvironmentVariable("Company") +"Purchase Line].No_ !='' and dbo."+ Environment.GetEnvironmentVariable("Company") +"Item].[Inventory Posting Group] >='01' and dbo."+ Environment.GetEnvironmentVariable("Company") +"Item].[Inventory Posting Group]<='15'" +
                          " ORDER BY dbo."+ Environment.GetEnvironmentVariable("Company") +"Vendor].[timestamp] DESC";


            //ViewBag.sql = queryData;
            var V_OrderPurchaseLines = _navcontext.orderPurchaseLines.FromSqlRaw(queryData).ToList();

            return DataSourceLoader.Load(V_OrderPurchaseLines, loadOptions);

        }

    }
}