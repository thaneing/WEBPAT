using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;
using CESAPSCOREWEBAPP.Helpers;
using CESAPSCOREWEBAPP.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using static CESAPSCOREWEBAPP.Models.Enums;

namespace CESAPSCOREWEBAPP.Controllers
{
    public class CheckGRErrorsController : BaseController
    {
        private readonly NAVContext _navcontext;
        private readonly DatabaseContext _context;


        public CheckGRErrorsController(NAVContext navcontext, DatabaseContext context)
        {
            _context = context;
            _navcontext = navcontext;
        }

        public IActionResult Index()
        {
            /*Check Session */
            var page = "235";
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
            
            var queryData1 = "SELECT dbo." + Environment.GetEnvironmentVariable("Company") + "Location].Code AS name,dbo." + Environment.GetEnvironmentVariable("Company") + "Location].Name as code FROM [dbo]." + Environment.GetEnvironmentVariable("Company") + "Location] order by dbo." + Environment.GetEnvironmentVariable("Company") + "Location].Code asc ";
            var sourceAutoCompletes = _navcontext.sourceAutoCompletes.FromSqlRaw(queryData1).ToList();
            ViewData["SourceAutoCompletes"] = sourceAutoCompletes;

            return View();
        }


        [HttpGet]
        public IActionResult GetData(string site)
        {

            /*Check Session */
            var page = "235";
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
            var query = "";
            var CheckGRErrors = new List<CheckGRError>();
            if (site == null)
            {
               query = " SELECT ROW_NUMBER() OVER (ORDER BY b.PostingDate) AS ID," +
                    "b.ItemNo,b.PostingDate,b.DocumentNo,sum(b.Sumdata) as ValueDiff,b.Location " +
                    " FROM( " +
                    "	SELECT * , " +
                    "		(SELECT Sum([Cost Amount (Actual)]) FROM dbo." + Environment.GetEnvironmentVariable("Company") + "Value Entry] WHERE [Item Ledger Entry No_]=a.EntryNo) as Sumdata" +
                    "	FROM( " +
                    "		SELECT " +
                    "		[Entry No_] as EntryNo, " +
                    "		[Item No_] as ItemNo, " +
                    "		CONVERT(varchar,[Posting Date],23) as PostingDate, " +
                    "		[Document No_] as DocumentNo," +
                    "		[Location Code] as Location " +
                    "		FROM " +
                    "		dbo." + Environment.GetEnvironmentVariable("Company") + "Item Ledger Entry]" +
                    "  WHERE [Document Type]=5 and ([Location Code]='2071' or [Location Code]='1040' or [Location Code]='2070' or [Location Code]='1041')  and ([Job No_]!='' or [Job Task No_]!='')  and [Item No_] Not Like 'PC%'" +
                    "	)as a  " +
                    ")as b " +
                    "GROUP BY  " +
                    "b.ItemNo, " +
                    "b.DocumentNo, " +
                    "b.Location, " +
                    "b.PostingDate " +
                    "HAVING sum(b.Sumdata)!=0 " +
                    "ORDER BY b.PostingDate DESC,b.DocumentNo DESC ";
                //ViewBag.sql = queryData;
                CheckGRErrors = _navcontext.CheckGRErrors.FromSqlRaw(query).ToList();
            }
            else
            {
                query = " SELECT ROW_NUMBER() OVER (ORDER BY b.PostingDate) AS ID," +
                 "b.ItemNo,b.PostingDate,b.DocumentNo,sum(b.Sumdata) as ValueDiff,b.Location " +
                 " FROM( " +
                 "	SELECT * , " +
                 "		(SELECT Sum([Cost Amount (Actual)]) FROM dbo." + Environment.GetEnvironmentVariable("Company") + "Value Entry] WHERE [Item Ledger Entry No_]=a.EntryNo) as Sumdata" +
                 "	FROM( " +
                 "		SELECT " +
                 "		[Entry No_] as EntryNo, " +
                 "		[Item No_] as ItemNo, " +
                 "		CONVERT(varchar,[Posting Date],23) as PostingDate, " +
                 "		[Document No_] as DocumentNo," +
                 "		[Location Code] as Location " +
                 "		FROM " +
                 "		dbo." + Environment.GetEnvironmentVariable("Company") + "Item Ledger Entry]" +
                 "  WHERE [Document Type]=5 and [Location Code]={0}   and ([Job No_]!='' or [Job Task No_]!='')  and [Item No_] Not Like 'PC%'" +
                 "	)as a  " +
                 ")as b " +
                 "GROUP BY  " +
                 "b.ItemNo, " +
                 "b.DocumentNo, " +
                 "b.Location, " +
                 "b.PostingDate " +
                 "HAVING sum(b.Sumdata)!=0 " +
                 "ORDER BY b.PostingDate DESC,b.DocumentNo DESC ";

                //SqlParameter parameterSite = new SqlParameter("@Site", site);
                CheckGRErrors = _navcontext.CheckGRErrors.FromSqlRaw(query, site).ToList();

            }



            response = Ok(new { data = CheckGRErrors });


            return response;
        }




        [HttpGet]
        public IActionResult GetCountGRErrors()
        {
            IActionResult response = Unauthorized();
            var query = " SELECT ROW_NUMBER() OVER (ORDER BY b.ItemNo) as ID," +
                "b.ItemNo,b.PostingDate,b.DocumentNo,sum(b.Sumdata) as ValueDiff,b.Location " +
                " FROM( " +
                "	SELECT * , " +
                "		(SELECT Sum([Cost Amount (Actual)]) FROM dbo." + Environment.GetEnvironmentVariable("Company") + "Value Entry] WHERE [Item Ledger Entry No_]=a.EntryNo) as Sumdata" +
                "	FROM( " +
                "		SELECT " +
                "		[Entry No_] as EntryNo, " +
                "		[Item No_] as ItemNo, " +
                "		CONVERT(varchar,[Posting Date],23) as PostingDate, " +
                "		[Document No_] as DocumentNo," +
                "		[Location Code] as Location " +
                "		FROM " +
                "		dbo." + Environment.GetEnvironmentVariable("Company") + "Item Ledger Entry]" +
                "  WHERE [Document Type]=5 and ([Location Code]='2071' or [Location Code]='1040' or [Location Code]='2070' or [Location Code]='1041')  and ([Job No_]!='' or [Job Task No_]!='')  and [Item No_] Not Like 'PC%'" +
                "	)as a  " +
                ")as b " +
                "GROUP BY  " +
                "b.ItemNo, " +
                "b.DocumentNo, " +
                "b.Location, " +
                "b.PostingDate " +
                "HAVING sum(b.Sumdata)!=0 " +
                "ORDER BY b.PostingDate DESC,b.DocumentNo DESC ";






            //ViewBag.sql = queryData;
            var CheckGRErrors = _navcontext.CheckGRErrors.FromSqlRaw(query).ToList();

            response = Ok(new { countdata = CheckGRErrors.Count });


            return response;
        }

    }
}