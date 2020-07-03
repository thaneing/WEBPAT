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
    public class RateOfRentalController : BaseController
    {
        private readonly NAVContext _navcontext;
        private readonly DatabaseContext _context;

        public RateOfRentalController(NAVContext navcontext, DatabaseContext context)
        {
            _context = context;
            _navcontext = navcontext;
        }
        public IActionResult Index()
        {
            /*Check Session */
            var page = "237";
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
         

            return View();
        }
        public IActionResult Getdata(string site)
        {
            /*Check Session */
            var page = "237";
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
            var queryData1 = "SELECT dbo." + Environment.GetEnvironmentVariable("Company") + "Location].Code AS name,dbo." + Environment.GetEnvironmentVariable("Company") + "Location].Name as code FROM [dbo]." + Environment.GetEnvironmentVariable("Company") + "Location] order by dbo." + Environment.GetEnvironmentVariable("Company") + "Location].Code asc ";
            var sourceAutoCompletes = _navcontext.sourceAutoCompletes.FromSqlRaw(queryData1).ToList();
            ViewData["SourceAutoCompletes"] = sourceAutoCompletes;

            IActionResult response = Unauthorized();
            List<RateOfRental> RateOfRentalsDATA;
            var querydata = "";

            if (site == null)
            {
                querydata = "SELECT ROW_NUMBER() OVER (ORDER BY a.ItemNo) as ID,*," +
                   " (SELECT convert(varchar,AVG(dbo." + Environment.GetEnvironmentVariable("Company") + "Job Ledger Entry].[Unit Cost])) FROM" +
                   " dbo." + Environment.GetEnvironmentVariable("Company") + "Job Ledger Entry]" +
                   " INNER JOIN dbo." + Environment.GetEnvironmentVariable("Company") + "Resource] ON dbo." + Environment.GetEnvironmentVariable("Company") + "Job Ledger Entry].No_ = dbo." + Environment.GetEnvironmentVariable("Company") + "Resource].No_" +
                   " WHERE" +
                   " dbo." + Environment.GetEnvironmentVariable("Company") + "Job Ledger Entry].No_ = a.ItemNo and" +
                   " dbo." + Environment.GetEnvironmentVariable("Company") + "Resource].Name = a.Description and" +
                   " dbo." + Environment.GetEnvironmentVariable("Company") + "Job Ledger Entry].[Unit of Measure Code] = a.UnitOfMesure) as Avg" +
                   " FROM(SELECT dbo." + Environment.GetEnvironmentVariable("Company") + "Job Ledger Entry].No_ AS ItemNo," +
                   " dbo." + Environment.GetEnvironmentVariable("Company") + "Resource].Name as Description," +
                   " dbo." + Environment.GetEnvironmentVariable("Company") + "Job Ledger Entry].[Unit of Measure Code] AS UnitOfMesure" +
                   " FROM" +
                   " dbo." + Environment.GetEnvironmentVariable("Company") + "Job Ledger Entry]" +
                   " INNER JOIN dbo." + Environment.GetEnvironmentVariable("Company") + "Resource] ON dbo." + Environment.GetEnvironmentVariable("Company") + "Job Ledger Entry].No_ = dbo." + Environment.GetEnvironmentVariable("Company") + "Resource].No_" +
                   " WHERE" +
                   " dbo." + Environment.GetEnvironmentVariable("Company") + "Job Ledger Entry].[Type of task] = 2 AND" +
                   " dbo." + Environment.GetEnvironmentVariable("Company") + "Job Ledger Entry].[Document No_] NOT LIKE 'CAL%' AND " +
                   " dbo." + Environment.GetEnvironmentVariable("Company") + "Job Ledger Entry].No_ NOT LIKE 'CR%' " +
                   " GROUP BY dbo." + Environment.GetEnvironmentVariable("Company") + "Job Ledger Entry].No_, Name,[Name 2], dbo." + Environment.GetEnvironmentVariable("Company") + "Job Ledger Entry].[Unit of Measure Code]) as a"; ;

                RateOfRentalsDATA = _navcontext.RateOfRentals.FromSqlRaw(querydata).ToList();

            }
            else
            {
                querydata = "SELECT ROW_NUMBER() OVER (ORDER BY a.ItemNo) as ID,*," +
                   " (SELECT convert(varchar,AVG(dbo." + Environment.GetEnvironmentVariable("Company") + "Job Ledger Entry].[Unit Cost])) FROM" +
                   " dbo." + Environment.GetEnvironmentVariable("Company") + "Job Ledger Entry]" +
                   " INNER JOIN dbo." + Environment.GetEnvironmentVariable("Company") + "Resource] ON dbo." + Environment.GetEnvironmentVariable("Company") + "Job Ledger Entry].No_ = dbo." + Environment.GetEnvironmentVariable("Company") + "Resource].No_" +
                   " WHERE" +
                   " dbo." + Environment.GetEnvironmentVariable("Company") + "Job Ledger Entry].No_ = a.ItemNo and" +
                   " dbo." + Environment.GetEnvironmentVariable("Company") + "Resource].Name = a.Description and" +
                   " dbo." + Environment.GetEnvironmentVariable("Company") + "Job Ledger Entry].[Unit of Measure Code] = a.UnitOfMesure) as Avg" +
                   " FROM(SELECT dbo." + Environment.GetEnvironmentVariable("Company") + "Job Ledger Entry].No_ AS ItemNo," +
                   " dbo." + Environment.GetEnvironmentVariable("Company") + "Resource].Name as Description," +
                   " dbo." + Environment.GetEnvironmentVariable("Company") + "Job Ledger Entry].[Unit of Measure Code] AS UnitOfMesure" +
                   " FROM" +
                   " dbo." + Environment.GetEnvironmentVariable("Company") + "Job Ledger Entry]" +
                   " INNER JOIN dbo." + Environment.GetEnvironmentVariable("Company") + "Resource] ON dbo." + Environment.GetEnvironmentVariable("Company") + "Job Ledger Entry].No_ = dbo." + Environment.GetEnvironmentVariable("Company") + "Resource].No_" +
                   " WHERE" +
                   " dbo." + Environment.GetEnvironmentVariable("Company") + "Job Ledger Entry].[Type of task] = 2 AND" +
                   " dbo." + Environment.GetEnvironmentVariable("Company") + "Job Ledger Entry].[Document No_] NOT LIKE 'CAL%' AND" +
                   " dbo." + Environment.GetEnvironmentVariable("Company") + "Job Ledger Entry].No_ NOT LIKE 'CR%' AND " +
                   " dbo." + Environment.GetEnvironmentVariable("Company") + "Job Ledger Entry].[Job No_] = @site " +
                   " GROUP BY dbo." + Environment.GetEnvironmentVariable("Company") + "Job Ledger Entry].No_, Name,[Name 2], dbo." + Environment.GetEnvironmentVariable("Company") + "Job Ledger Entry].[Unit of Measure Code]) as a";

                SqlParameter parameterSite = new SqlParameter("@site", site);
                RateOfRentalsDATA = _navcontext.RateOfRentals.FromSqlRaw(querydata,parameterSite).ToList();

            }

            response = Ok(new { data = RateOfRentalsDATA });

            return response;

         
        }
        public IActionResult Get()
        {
            /*Check Session */
            var page = "237";
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
            IActionResult response = Unauthorized();
            List<RateOfRental> RateOfRentalsDATA;
            var querydata = "";

                querydata = "SELECT ROW_NUMBER() OVER (ORDER BY dbo." + Environment.GetEnvironmentVariable("Company") + "Resource].No_) as ID,dbo." + Environment.GetEnvironmentVariable("Company") + "Resource].No_ AS ItemNo," +
                " dbo." + Environment.GetEnvironmentVariable("Company") + "Resource].Name AS Description," +
                " convert(varchar,dbo." + Environment.GetEnvironmentVariable("Company") + "Rental Price].[Price _ Area : Bkk]) AS RentalPrice," +
                " dbo." + Environment.GetEnvironmentVariable("Company") + "Rental Price].[Rental Unit] AS RentalUnit" +
                " FROM dbo." + Environment.GetEnvironmentVariable("Company") + "Rental Price]" +
                " INNER JOIN dbo." + Environment.GetEnvironmentVariable("Company") + "Resource] ON dbo." + Environment.GetEnvironmentVariable("Company") + "Resource].No_ = dbo." + Environment.GetEnvironmentVariable("Company") + "Rental Price].[Resource No_]" +
                " WHERE" +
                " dbo." + Environment.GetEnvironmentVariable("Company") + "Resource].No_ NOT LIKE 'CR%' AND" +
                " dbo." + Environment.GetEnvironmentVariable("Company") + "Resource].No_ NOT LIKE 'CI%'"; 


                RateOfRentalsDATA = _navcontext.RateOfRentals.FromSqlRaw(querydata).ToList();

       

            response = Ok(new { data = RateOfRentalsDATA });

            return response;
        }
    }
}