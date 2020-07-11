using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CESAPSCOREWEBAPP.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Rotativa.AspNetCore;

namespace CESAPSCOREWEBAPP.Controllers
{
    [Authorize]
    public class TestPDFController : Controller
    {
        private readonly DatabaseContext _context;

        private readonly NAVContext _navcontext;

        public TestPDFController(DatabaseContext context, NAVContext navcontext)
        {
            _context = context;
            _navcontext = navcontext;
        }

        public IActionResult Index()
        {
       
                IActionResult response = Unauthorized();
                var TotalEmployee = _context.Users.Where(p => p.StatusUserId == 1).Count();
                var TotalMale = _context.Users.Where(p => p.StatusUserId == 1 && p.TitleOfUserId == 1).Count();
                var TotalFemale = _context.Users.Where(p => p.StatusUserId == 1 && (p.TitleOfUserId == 2 || p.TitleOfUserId == 3)).Count();

                var Monly = _context.Users.Where(p => p.StatusUserId == 1 && p.TypeOfEmployeeId == 1).Count();
                var Dayly = _context.Users.Where(p => p.StatusUserId == 1 && p.TypeOfEmployeeId == 2).Count();

                var Ho = _context.Users.Where(p => p.StatusUserId == 1 && p.BranchId == 3).Count();
                var Store = _context.Users.Where(p => p.StatusUserId == 1 && p.BranchId == 10).Count();
                var Sites = _context.Users.Where(p => p.StatusUserId == 1 && (p.BranchId != 10 && p.BranchId != 3)).Count();

                decimal percenMale = Percen(TotalMale, TotalEmployee);
                decimal percenFemale = Percen(TotalFemale, TotalEmployee);
                decimal percenMonly = Percen(Monly, TotalEmployee);
                decimal percenDayly = Percen(Dayly, TotalEmployee);
                decimal percenHo = Percen(Ho, TotalEmployee);
                decimal percenStores = Percen(Store, TotalEmployee);
                decimal percenSites = Percen(Sites, TotalEmployee);


            ViewData["TotalEmployee"] = ""+ TotalEmployee + "";
            ViewData["TotalMale"] = "" + TotalMale + "";
            ViewData["TotalFemale"] = "" + TotalFemale + "";
            ViewData["Monly"] = "" + Monly + "";
            ViewData["Dayly"] = "" + Dayly + "";
            ViewData["Ho"] = "" + Ho + "";
            ViewData["Store"] = "" + Store + "";
            ViewData["Sites"] = "" + Sites + "";

            ViewData["percenMale"] = "" + percenMale + " %";
            ViewData["percenFemale"] = "" + percenFemale + " %";
            ViewData["percenMonly"] = "" + percenMonly + " %";
            ViewData["percenDayly"] = "" + percenDayly + " %";
            ViewData["percenHo"] = "" + percenHo + " %";
            ViewData["percenStores"] = "" + percenStores + " %";
            ViewData["percenMale"] = "" + percenMale + " %";
            ViewData["percenSites"] = "" + percenSites + " %";

            //Generation
            var queryGen = "SELECT 'Baby Boomer (2489-2507)' As Head,'มีอายุระหว่าง 57-73 ปี' As Etc,COUNT(*) AS CountGen,'' AS Progressbar " +
                " FROM dbo.Users " +
                " WHERE YEAR ( dbo.Users.BirthName ) +543 >= 2489 AND YEAR ( dbo.Users.BirthName ) +543 <= 2507 AND dbo.Users.StatusUserId = 1  " +
                " UNION SELECT 'Gen X (2508-2522)' As Head,'มีอายุระหว่าง 40-50 ปี' As Etc,COUNT(*) AS  CountGen ,'' AS Progressbar " +
                " FROM dbo.Users " +
                " WHERE YEAR ( dbo.Users.BirthName ) +543 >= 2508 AND YEAR ( dbo.Users.BirthName ) +543 <= 2522 AND dbo.Users.StatusUserId = 1 " +
                " UNION SELECT 'Gen Y (2523-2540)' As Head,'มีอายุระหว่าง 22-39 ปี' As Etc,COUNT(*) AS  CountGen, '' AS Progressbar " +
                " FROM dbo.Users " +
                " WHERE YEAR ( dbo.Users.BirthName ) +543 >= 2523 AND YEAR ( dbo.Users.BirthName ) +543 <= 2540 AND dbo.Users.StatusUserId = 1 " +
                " UNION SELECT 'Gen Z (2540 +)' As Head,'มีอายุน้อยกว่า 22 ปี' As Etc,COUNT(*) AS  CountGen, '' AS Progressbar " +
                " FROM dbo.Users " +
                " WHERE YEAR ( dbo.Users.BirthName ) +543 > 2540 AND dbo.Users.StatusUserId = 1 ";

            var reportGens = _context.ReportGens.FromSqlRaw(queryGen).ToList();
            var totaleGens = reportGens.Sum(p => p.CountGen);
            var header = "<table>" +
                " <thead> " +
                "<tr> " +
                "<th width='300px'> " +
                "</th> " +
                "<th width='40px' style='text-align:center'>" +
                "<sapn>คน</sapn>" +
                "</th>" +
                "<th style='text-align:right' width='400px'>" +
                "<sapn>%</sapn>" +
                "</th>" +
                "</tr>" +
                "</thead>" +
                "<tbody>";

            var footer = "</tbody>" +
                         "</table>";
            var body = "";
            decimal percent = 0;
            var countgen = 0;
            var totalemp = 0;
            foreach (var std in reportGens as IList<ReportGen>)
            {
                countgen = std.CountGen;
                totalemp = totaleGens;


                percent = Percen(countgen, totalemp);


                body += " <tr>";
                body += " <td valign='top'>";
                body += " <div>" + std.Head + "</div>" +
                    "<small>" + std.Etc + "</small>";
                body += " </td>";
                body += " <td valign='top' style='text-align:right;padding-right:6px;'>";
                body += " <span>" + std.CountGen + "</span>";
                body += " </td>";
                body += " <td style='text-align:right;' valign='top'>";
                body += " <div class='progress'>";
                body += "<div class='progress-bar' role='progress-bar' aria-valuenow='40' style='width:" + percent + "%' id='bg-progressGen'><span style='color:#4d4f53;font-weight: bold; font-size:10px;'>" + percent + "%</span>";
                body += "</div>";
                body += " </td>";
                body += " </tr>";

            }
            var tableGen = header + body + footer;

            ViewData["tableGen"] = header + body + footer;

            //Age ช่วงอายุ
            var queryAge = "SELECT '21 - 25' AS Range,COUNT(*) AS countAge,'' AS progressbarAge " +
                " FROM dbo.Users " +
                " WHERE YEAR(getdate())- YEAR(dbo.Users.BirthName) >=21 AND YEAR(getdate())- YEAR(dbo.Users.BirthName) <=25 AND dbo.Users.StatusUserId = 1 " +
                " UNION SELECT '26 - 30' AS Range,COUNT(*) AS CountAge,'' AS progressbarAge " +
                " FROM dbo.Users " +
                " WHERE YEAR(getdate())- YEAR(dbo.Users.BirthName) >=26 AND YEAR(getdate())- YEAR(dbo.Users.BirthName) <=30 AND dbo.Users.StatusUserId = 1 " +
                " UNION SELECT '31 - 35' AS Range,COUNT(*) AS CountAge,'' AS progressbar " +
                " FROM dbo.Users " +
                " WHERE YEAR(getdate())- YEAR(dbo.Users.BirthName) >=31 AND YEAR(getdate())- YEAR(dbo.Users.BirthName) <=35 AND dbo.Users.StatusUserId = 1 " +
                " UNION SELECT '36 - 40' AS Range,COUNT(*) AS CountAge,'' AS progressbarAge " +
                " FROM dbo.Users " +
                " WHERE YEAR(getdate())- YEAR(dbo.Users.BirthName) >=36 AND YEAR(getdate())- YEAR(dbo.Users.BirthName) <=40 AND dbo.Users.StatusUserId = 1 " +
                " UNION SELECT '41 - 45' AS Range,COUNT(*) AS CountAge,'' AS progressbarAge " +
                " FROM dbo.Users " +
                " WHERE YEAR(getdate())- YEAR(dbo.Users.BirthName) >=41 AND YEAR(getdate())- YEAR(dbo.Users.BirthName) <=45 AND dbo.Users.StatusUserId = 1 " +
                " UNION SELECT '46 - 50' AS Range,COUNT(*) AS CountAge,'' AS progressbarAge " +
                " FROM dbo.Users " +
                " WHERE YEAR(getdate())- YEAR(dbo.Users.BirthName) >=46 AND YEAR(getdate())- YEAR(dbo.Users.BirthName) <=50 AND dbo.Users.StatusUserId = 1 " +
                " UNION SELECT '51 - 55' AS Range,COUNT(*) AS CountAge,'' AS progressbarAge " +
                " FROM dbo.Users " +
                " WHERE YEAR(getdate())- YEAR(dbo.Users.BirthName) >=51 AND YEAR(getdate())- YEAR(dbo.Users.BirthName) <=55 AND dbo.Users.StatusUserId = 1 " +
                " UNION SELECT '56 - 60' AS Range,COUNT(*) AS CountAge,'' AS progressbarAge " +
                " FROM dbo.Users " +
                " WHERE YEAR(getdate())- YEAR(dbo.Users.BirthName) >=56 AND YEAR(getdate())- YEAR(dbo.Users.BirthName) <=60 AND dbo.Users.StatusUserId = 1 " +
                " UNION SELECT '60 ปีขึ้นไป' AS Range,COUNT(*) AS CountAge,'' AS progressbarAge " +
                " FROM dbo.Users " +
                " WHERE YEAR(getdate())- YEAR(dbo.Users.BirthName) >60 ";

            var reportAge = _context.ReportAges.FromSqlRaw(queryAge).ToList();
            var totaleAge = reportAge.Sum(p => p.CountAge);

            var headerAge = "<table>" +
                " <thead> " +
                "<tr> " +
                "<th width='100px'> " +
                "</th> " +
                "<th width='20px' style='text-align:center'>" +
                "<sapn>คน</sapn>" +
                "</th>" +
                "<th style='text-align:right' width='400px'>" +
                "<sapn>%</sapn>" +
                "</th>" +
                "</tr>" +
                "</thead>" +
                "<tbody>";
            var footerAge = "</tbody>" +
                            "</table>";
            var bodyAge = "";
            decimal percentAge = 0;
            var countAge = 0;
            var totalempAge = 0;


            foreach (var std in reportAge as IList<ReportAge>)
            {
                countAge = std.CountAge;
                totalempAge = totaleAge;


                percentAge = Percen(countAge, totalempAge);


                bodyAge += " <tr>";
                bodyAge += " <td valign='top'>";
                bodyAge += " <span>" + std.Range + "</span>";
                bodyAge += " </td>";
                bodyAge += " <td style='text-align:right;padding-right:10px;' valign='top'>";
                bodyAge += " <span>" + std.CountAge + "</span>";
                bodyAge += " </td>";
                bodyAge += " <td style='text-align:right' >";
                bodyAge += " <div class='progress'>";
                bodyAge += "<div class='progress-bar progress-bar-warning' role='progress-bar' aria-valuenow='40' style='width:" + percentAge + "%' id='bg-progressAge'><span style='color:#4d4f53;font-weight: bold;font-size:10px;'>" + percentAge + "%</span>";
                bodyAge += "</div>";
                bodyAge += " </td>";
                bodyAge += " </tr>";

            }

            ViewData["tableAge"] = headerAge + bodyAge + footerAge;

            //Position PC Level
            var queryLevels = "SELECT 'PC-1' AS HeadLevel,Count(*) AS CountLevels,'' AS ProgressbarLevel" +
                " FROM dbo.Users " +
                " WHERE dbo.Users.StatusUserId = 1 AND dbo.Users.LevelId = 1 " +
                " UNION SELECT 'PC-2' AS HeadLevel,Count(*) AS CountLevels,'' AS ProgressbarLevel " +
                " FROM dbo.Users " +
                " WHERE dbo.Users.StatusUserId = 1 AND dbo.Users.LevelId = 2 " +
                " UNION SELECT 'PC-3' AS HeadLevel,Count(*) AS CountLevels,'' AS ProgressbarLevel" +
                " FROM dbo.Users " +
                " WHERE dbo.Users.StatusUserId = 1 AND dbo.Users.LevelId = 3 " +
                " UNION SELECT 'PC-4' AS HeadLevel,Count(*) AS CountLevels,'' AS ProgressbarLevel " +
                " FROM dbo.Users " +
                " WHERE dbo.Users.StatusUserId = 1 AND dbo.Users.LevelId = 4 " +
                " UNION SELECT 'PC-5' AS HeadLevel,Count(*) AS CountLevels,'' AS ProgressbarLevel " +
                " FROM dbo.Users " +
                " WHERE dbo.Users.StatusUserId = 1 AND dbo.Users.LevelId = 5 " +
                " UNION SELECT 'PC-6' AS HeadLevel, Count(*) AS CountLevels,'' AS ProgressbarLevel " +
                " FROM dbo.Users " +
                " WHERE dbo.Users.StatusUserId = 1 AND dbo.Users.LevelId = 6 " +
                " UNION SELECT 'PC-7' AS HeadLevel,Count(*) AS CountLevels,'' AS ProgressbarLevel" +
                " FROM dbo.Users " +
                " WHERE dbo.Users.StatusUserId = 1 AND dbo.Users.LevelId = 7 " +
                " UNION SELECT 'PC-8' AS HeadLevel,Count(*) AS CountLevels,'' AS ProgressbarLevel " +
                " FROM dbo.Users " +
                " WHERE dbo.Users.StatusUserId = 1 AND dbo.Users.LevelId = 8 " +
                " UNION SELECT 'PC-9' AS HeadLevel,Count(*) AS CountLevels,'' AS ProgressbarLevel " +
                " FROM dbo.Users " +
                " WHERE dbo.Users.StatusUserId = 1 AND dbo.Users.LevelId = 9 " +
                " UNION SELECT 'Un' AS HeadLevel,Count(*) AS CountLevelsม,'' AS ProgressbarLevel " +
                " FROM dbo.Users " +
                " WHERE dbo.Users.StatusUserId = 1 AND dbo.Users.LevelId = 10";

            var reportLevel = _context.ReportLevels.FromSqlRaw(queryLevels).ToList();
            var totaleLevel = reportLevel.Sum(p => p.CountLevels);

            var headerLevel = "<table>" +
                " <thead> " +
                "<tr> " +
                "<th width='100px'> " +
                "</th> " +
                "<th width='20px' style='text-align:center'>" +
                "<sapn>คน</sapn>" +
                "</th>" +
                "<th style='text-align:right' width='400px'>" +
                "<sapn>%</sapn>" +
                "</th>" +
                "</tr>" +
                "</thead>" +
                "<tbody>";
            var footerLevel = "</tbody>" +
                            "</table>";
            var bodyLevel = "";
            decimal percentLevel = 0;
            var countlevel = 0;
            var totalLavelemp = 0;

            foreach (var std in reportLevel as IList<ReportLevel>)
            {
                countlevel = std.CountLevels;
                totalLavelemp = totaleGens;
                percentLevel = Percen(countlevel, totalLavelemp);

                bodyLevel += " <tr valign='top'>";
                bodyLevel += " <td>";
                bodyLevel += " <span>" + std.HeadLevel + "</span>";
                bodyLevel += " </td>";
                bodyLevel += " <td style='text-align:right;padding-right:10px;'>";
                bodyLevel += " <span>" + std.CountLevels + "</span>";
                bodyLevel += " </td>";
                bodyLevel += " <td>";
                bodyLevel += " <div class='progress'>";
                bodyLevel += "<div class='progress-bar progress-bar-warning' role='progress-bar' aria-valuenow='40' style='width:" + percentLevel + "%' id='bg-progressPC'><span style='color:#4d4f53;font-weight: bold;font-size:10px;'>" + percentLevel + "%</span>";
                bodyLevel += "</div>";
                bodyLevel += " </td>";
                bodyLevel += " </tr>";

            }

            ViewData["tableLevel"] = headerLevel + bodyLevel + footerLevel;


            //Management to staff Ratio
            var queryManagement = " SELECT 'Employee count (PC1-6)' AS LevelHead,COUNT(*) AS CountMnagement,'' AS ProgressbarManagement " +
                " FROM dbo.Users " +
                " WHERE dbo.Users.LevelId >=1 AND  dbo.Users.LevelId <=6 AND dbo.Users.StatusUserId = 1  " +
                " UNION SELECT 'Manager count (PC7 +)' AS LevelHead,COUNT(*) AS CountMnagement,'' AS ProgressbarManagement" +
                " FROM dbo.Users " +
                " WHERE dbo.Users.LevelId >=7 AND dbo.Users.StatusUserId = 1 ";

            var reportMn = _context.ReportManagements.FromSqlRaw(queryManagement).ToList();
            var totaleMn = reportMn.Sum(p => p.CountMnagement);

            var headerMn = "<table>" +
                " <thead> " +
                "<tr> " +
                "<th width='300px'> " +
                "</th> " +
                "<th width='20px' style='text-align:center'>" +
                "<sapn>คน</sapn>" +
                "</th>" +
                "<th style='text-align:right' width='400px'>" +
                "<sapn>%</sapn>" +
                "</th>" +
                "</tr>" +
                "</thead>" +
                "<tbody>";
            var footerMn = "</tbody>" +
                            "</table>";
            var bodyMn = "";
            decimal percentMn = 0;
            var countMn = 0;
            var totalempMn = 0;

            foreach (var std in reportMn as IList<ReportManagement>)
            {
                countMn = std.CountMnagement;
                totalempMn = totaleAge;

                percentMn = Percen(countMn, totalempMn);

                bodyMn += " <tr valign='top'>";
                bodyMn += " <td>";
                bodyMn += " <i class='fa fa-user-secret'></i> <span>" + std.LevelHead + "</span>";
                bodyMn += " </td>";
                bodyMn += " <td style='text-align:right;padding-right:10px;'>";
                bodyMn += " <span>" + std.CountMnagement + "</span>";
                bodyMn += " </td>";
                bodyMn += " <td>";
                bodyMn += " <div class='progress'>";
                bodyMn += "<div class='progress-bar' role='progress-bar' aria-valuenow='40' style='width:" + percentMn + "%' id='bg-progressMn'><span style='color:#4d4f53;font-weight: bold;font-size:10px;'>" + percentMn + "%</span>";
                bodyMn += "</div>";
                bodyMn += " </td>";
                bodyMn += " </tr>";

            }

            ViewData["tableManagement"] = headerMn + bodyMn + footerMn;

            //HeadCount --สายงาน
            //Construction
            var queryConstruction = "SELECT 'สายงานก่อสร้าง' as HeadDepartment,COUNT(*) AS CountDepartment, " +
                " (SELECT count(*) FROM dbo.Users WHERE dbo.Users.StatusUserId =1) AS TotalEmployee" +
                " FROM dbo.Users AS u " +
                " INNER JOIN dbo.Organizs AS o ON u.organizId = o.organizId " +
                " INNER JOIN dbo.Departments AS d ON o.DepartmentId = d.DepartmentId " +
                " WHERE (d.DepartmentId=2 OR d.DepartmentId =3 OR d.DepartmentId=9) AND u.StatusUserId =1 ";

            var reportCon = _context.ReportDeparments.FromSqlRaw(queryConstruction).ToList();

            var headerCon = "<table>" +
                        " <thead> " +
                        "<tr> " +
                        "<th width='250px'> " +
                        "</th> " +
                        "<th width='100px'> " +
                        "</th> " +
                        "</tr>" +
                        "</thead>" +
                        "<tbody>";
            var footerCon = "</tbody>" +
                            "</table>";

            var bodyCon = "";
            decimal percentCon = 0;
            var countCon = 0;
            var totalempCon = 0;

            foreach (var std in reportCon as IList<ReportDeparment>)
            {
                countCon = std.CountDepartment;
                totalempCon = std.TotalEmployee;
                percentCon = Percen(countCon, totalempCon);

                bodyCon += " <tr><td><h1></h1></td><tr>";
                bodyCon += " <tr>";
                bodyCon += " <td>";
                bodyCon += " <i class='fa fa-cubes fa-2x'></i> <span class='info - box - text'>" + std.HeadDepartment + "</span>";
                bodyCon += " <p>Construction</p>";
                bodyCon += " </td>";
                bodyCon += " <td>";
                bodyCon += " <span><h3>" + std.CountDepartment + "</h3></span>";
                bodyCon += " <p>" + percentCon + "%</p>";
                bodyCon += " </td>";
                bodyCon += " </tr>";

            }

            ViewData["tableCon"] = headerCon + bodyCon + footerCon;

            //admin&financial
            var queryAdminFinancail = "SELECT 'สายงานบริหารและการเงิน' as HeadDepartment,COUNT(*) AS CountDepartment, " +
               " (SELECT count(*) FROM dbo.Users WHERE dbo.Users.StatusUserId =1) AS TotalEmployee" +
               " FROM dbo.Users AS u " +
               " INNER JOIN dbo.Organizs AS o ON u.organizId = o.organizId " +
               " INNER JOIN dbo.Departments AS d ON o.DepartmentId = d.DepartmentId " +
               " WHERE (d.DepartmentId=5 OR d.DepartmentId =8 OR d.DepartmentId=11 OR d.DepartmentId=12 OR d.DepartmentId=10) AND u.StatusUserId =1 ";

            var reportAdminF = _context.ReportDeparments.FromSqlRaw(queryAdminFinancail).ToList();

            var headerAF = "<table>" +
                        " <thead> " +
                        "<tr> " +
                        "<th width='250px'> " +
                        "</th> " +
                        "<th width='100px'> " +
                        "</th> " +
                        "</tr>" +
                        "</thead>" +
                        "<tbody>";
            var footerAF = "</tbody>" +
                            "</table>";

            var bodyAF = "";
            decimal percentAF = 0;
            var countAF = 0;
            var totalempAF = 0;

            foreach (var std in reportAdminF as IList<ReportDeparment>)
            {
                countAF = std.CountDepartment;
                totalempAF = std.TotalEmployee;
                percentAF = Percen(countAF, totalempAF);

                bodyAF += " <tr><td><h2></h2></td><tr>";
                bodyAF += " <tr>";
                bodyAF += " <td>";
                bodyAF += " <i class='fa fa-money fa-2x'></i> <span class='info-box-text'>" + std.HeadDepartment + "</span>";
                bodyAF += " <p>Admin & Financail</p>";
                bodyAF += " </td>";
                bodyAF += " <td colspan='2'>";
                bodyAF += " <span><h3>" + std.CountDepartment + "</h3></span>";
                bodyAF += " <p>" + percentAF + "%</p>";
                bodyAF += " </td>";
                bodyAF += " </tr>";

            }

            ViewData["tableAdminFinancail"] = headerAF + bodyAF + footerAF;

            //Tech&Est
            var queryTechEst = "SELECT 'สายงานเทคนิคและประเมินราคา' as HeadDepartment,COUNT(*) AS CountDepartment, " +
               " (SELECT count(*) FROM dbo.Users WHERE dbo.Users.StatusUserId =1) AS TotalEmployee" +
               " FROM dbo.Users AS u " +
               " INNER JOIN dbo.Organizs AS o ON u.organizId = o.organizId " +
               " INNER JOIN dbo.Departments AS d ON o.DepartmentId = d.DepartmentId " +
               " WHERE (d.DepartmentId=4 OR d.DepartmentId =6 OR d.DepartmentId =7 OR d.DepartmentId=13 OR d.DepartmentId=14 OR d.DepartmentId=15) AND u.StatusUserId =1 ";

            var reportTechEst = _context.ReportDeparments.FromSqlRaw(queryTechEst).ToList();

            var headerTE = "<table>" +
                        " <thead> " +
                        "<tr> " +
                        "<th width='250px'> " +
                        "</th> " +
                        "<th width='100px'> " +
                        "</th> " +
                        "</tr>" +
                        "</thead>" +
                        "<tbody>";
            var footerTE = "</tbody>" +
                            "</table>";

            var bodyTE = "";
            decimal percentTE = 0;
            var countTE = 0;
            var totalempTE = 0;

            foreach (var std in reportTechEst as IList<ReportDeparment>)
            {
                countTE = std.CountDepartment;
                totalempTE = std.TotalEmployee;
                percentTE = Percen(countTE, totalempTE);

                bodyTE += " <tr><td><h2></h2></td><tr>";
                bodyTE += " <tr>";
                bodyTE += " <td>";
                bodyTE += " <i class='fa fa-tags fa-2x'></i> <span'>" + std.HeadDepartment + "</span>";
                bodyTE += " <p>Tech & Est</p>";
                bodyTE += " </td>";
                bodyTE += " <td colspan='2'>";
                bodyTE += " <span><h3>" + std.CountDepartment + "</h3></span>";
                bodyTE += " <p>" + percentTE + "%</p>";
                bodyTE += " </td>";
                bodyTE += " </tr>";

            }

            ViewData["tableTechEst"] = headerTE + bodyTE + footerTE;

            //Deparment ฝ่าย
            //Construct Factory
            var queryDepCon = "SELECT 'Construction' as HeadDepartment1,COUNT(*) as CountDepartment1," +
               " (SELECT count(*) FROM dbo.Users WHERE dbo.Users.StatusUserId =1) AS TotalEmployee1 ,'' AS ProgressbarDepartment1 " +
               " FROM dbo.Users AS u " +
               " INNER JOIN dbo.Organizs AS o ON u.organizId = o.organizId " +
               " INNER JOIN dbo.Departments AS d ON o.DepartmentId = d.DepartmentId " +
               " WHERE d.DepartmentId=2 AND u.StatusUserId =1 " +
               " UNION SELECT 'Facrory +' as HeadDepartment1,COUNT(*) as CountDepartment1," +
               " (SELECT count(*) FROM dbo.Users WHERE dbo.Users.StatusUserId =1) as TotalEmployee ,'' AS ProgressbarDepartment1 " +
               " FROM dbo.Users AS u " +
               " INNER JOIN dbo.Organizs AS o ON u.organizId = o.organizId " +
               " INNER JOIN dbo.Departments AS d ON o.DepartmentId = d.DepartmentId " +
               " WHERE d.DepartmentId=9 AND u.StatusUserId =1 ";

            var reportDepCon = _context.ReportDeparment1s.FromSqlRaw(queryDepCon).ToList();

            var headerDepCon = "<table>" +
                    " <thead> " +
                    "<tr> " +
                    "<th width='180px'>" +
                    "<sapn><h2></h2></sapn>" +
                    "</th>" +
                    "<th width='50px'>" +
                    "<sapn>คน</sapn>" +
                    "</th>" +
                    "<th style='text-align:right' width='200px'>" +
                    "<sapn>%</sapn>" +
                    "</th>" +
                    "</tr>" +
                    "</thead>" +
                    "<tbody>";
            var footerDepCon = "</tbody>" +
                            "</table>";

            var bodyDepCon = "";
            decimal percentDepCon = 0;
            var countDepCon = 0;
            var totalempDepCon = 0;

            foreach (var std in reportDepCon as IList<ReportDepartment1>)
            {
                countDepCon = std.CountDepartment1;
                totalempDepCon = std.TotalEmployee1;
                percentDepCon = Percen(countDepCon, totalempDepCon);

                bodyDepCon += " <tr valign='top'>";
                bodyDepCon += " <td style='text-align:right;padding-right:10px'>";
                bodyDepCon += " <span>" + std.HeadDepartment1 + "</span>          ";
                bodyDepCon += " </td>";
                bodyDepCon += " <td>";
                bodyDepCon += " <span>" + std.CountDepartment1 + "</span>";
                bodyDepCon += " </td>";
                bodyDepCon += " <td>";
                bodyDepCon += " <div class='progress'>";
                bodyDepCon += "<div class='progress-bar' role='progress-bar' aria-valuenow='40' style='width:" + percentDepCon + "%' id='bg-progressDep'><span style='color:#4d4f53;font-weight: bold;font-size:10px;'>" + percentDepCon + "%</span>";
                bodyDepCon += "</div>";
                bodyDepCon += "</td>";
                bodyDepCon += " </tr>";
            }

            ViewData["tableDepCon"] = headerDepCon + bodyDepCon + footerDepCon;

            //Account HR IT
            var queryDepAF = "SELECT 'Accounting & Financial +' as HeadDepartment1,COUNT(*) as CountDepartment1," +
               " (SELECT count(*) FROM dbo.Users WHERE dbo.Users.StatusUserId =1) AS TotalEmployee1 ,'' AS ProgressbarDepartment1 " +
               " FROM dbo.Users AS u " +
               " INNER JOIN dbo.Organizs AS o ON u.organizId = o.organizId " +
               " INNER JOIN dbo.Departments AS d ON o.DepartmentId = d.DepartmentId " +
               " WHERE d.DepartmentId=11 AND u.StatusUserId =1 " +
               " UNION SELECT 'HR & Admin +' as HeadDepartment1,COUNT(*) as CountDepartment1," +
               " (SELECT count(*) FROM dbo.Users WHERE dbo.Users.StatusUserId =1) as TotalEmployee ,'' AS ProgressbarDepartment1 " +
               " FROM dbo.Users AS u " +
               " INNER JOIN dbo.Organizs AS o ON u.organizId = o.organizId " +
               " INNER JOIN dbo.Departments AS d ON o.DepartmentId = d.DepartmentId " +
               " WHERE d.DepartmentId=12 AND u.StatusUserId =1 " +
               " UNION SELECT 'IT +' as HeadDepartment1,COUNT(*) as CountDepartment1," +
               " (SELECT count(*) FROM dbo.Users WHERE dbo.Users.StatusUserId =1) as TotalEmployee ,'' AS ProgressbarDepartment1 " +
               " FROM dbo.Users AS u " +
               " INNER JOIN dbo.Organizs AS o ON u.organizId = o.organizId " +
               " INNER JOIN dbo.Departments AS d ON o.DepartmentId = d.DepartmentId " +
               " WHERE d.DepartmentId=8 AND u.StatusUserId =1 ";



            var reportDepAF = _context.ReportDeparment1s.FromSqlRaw(queryDepAF).ToList();

            var headerDepAF = "<table>" +
                    " <thead> " +
                    "<tr> " +
                    "<th width='180px'>" +
                    "<sapn><h2></h2></sapn>" +
                    "</th>" +
                    "<th width='50px'>" +
                    "<sapn></sapn>" +
                    "</th>" +
                    "<th style='text-align:right' width='200px'>" +
                    "<sapn></sapn>" +
                    "</th>" +
                    "</tr>" +
                    "</thead>" +
                    "<tbody>";
            var footerDepAF = "</tbody>" +
                            "</table>";

            var bodyDepAF = "";
            decimal percentDepAF = 0;
            var countDepAF = 0;
            var totalempDepAF = 0;

            foreach (var std in reportDepAF as IList<ReportDepartment1>)
            {
                countDepAF = std.CountDepartment1;
                totalempDepAF = std.TotalEmployee1;
                percentDepAF = Percen(countDepCon, totalempDepCon);

                bodyDepAF += " <tr valign='top'>";
                bodyDepAF += " <td style='text-align:right;padding-right:10px'>";
                bodyDepAF += " <span>" + std.HeadDepartment1 + "</span>          ";
                bodyDepAF += " </td>";
                bodyDepAF += " <td>";
                bodyDepAF += " <span>" + std.CountDepartment1 + "</span>";
                bodyDepAF += " </td>";
                bodyDepAF += " <td>";
                bodyDepAF += " <div class='progress'>";
                bodyDepAF += "<div class='progress-bar' role='progress-bar' aria-valuenow='40' style='width:" + percentDepAF + "%' id='bg-progressDep'><span style='color:#4d4f53;font-weight: bold;font-size:10px;'>" + percentDepAF + "%</span>";
                bodyDepAF += "</div>";
                bodyDepAF += "</td>";
                bodyDepAF += " </tr>";

            }

            ViewData["tableDepAF"] = headerDepAF + bodyDepAF + footerDepAF;

            //Purchasing---Design
            var queryDepTE = "SELECT 'Puchasing' as HeadDepartment1,COUNT(*) as CountDepartment1," +
               " (SELECT count(*) FROM dbo.Users WHERE dbo.Users.StatusUserId =1) AS TotalEmployee1 ,'' AS ProgressbarDepartment1 " +
               " FROM dbo.Users AS u " +
               " INNER JOIN dbo.Organizs AS o ON u.organizId = o.organizId " +
               " INNER JOIN dbo.Departments AS d ON o.DepartmentId = d.DepartmentId " +
               " WHERE d.DepartmentId=4 AND u.StatusUserId =1 " +
               " UNION SELECT 'Cost Management' as HeadDepartment1,COUNT(*) as CountDepartment1," +
               " (SELECT count(*) FROM dbo.Users WHERE dbo.Users.StatusUserId =1) as TotalEmployee ,'' AS ProgressbarDepartment1 " +
               " FROM dbo.Users AS u " +
               " INNER JOIN dbo.Organizs AS o ON u.organizId = o.organizId " +
               " INNER JOIN dbo.Departments AS d ON o.DepartmentId = d.DepartmentId " +
               " WHERE d.DepartmentId=15 AND u.StatusUserId =1 " +
               " UNION SELECT 'Technical' as HeadDepartment1,COUNT(*) as CountDepartment1," +
               " (SELECT count(*) FROM dbo.Users WHERE dbo.Users.StatusUserId =1) as TotalEmployee ,'' AS ProgressbarDepartment1 " +
               " FROM dbo.Users AS u " +
               " INNER JOIN dbo.Organizs AS o ON u.organizId = o.organizId " +
               " INNER JOIN dbo.Departments AS d ON o.DepartmentId = d.DepartmentId " +
               " WHERE d.DepartmentId=6 AND u.StatusUserId =1 " +
               " UNION SELECT 'Estimation' as HeadDepartment1,COUNT(*) as CountDepartment1," +
               " (SELECT count(*) FROM dbo.Users WHERE dbo.Users.StatusUserId =1) as TotalEmployee ,'' AS ProgressbarDepartment1 " +
               " FROM dbo.Users AS u " +
               " INNER JOIN dbo.Organizs AS o ON u.organizId = o.organizId " +
               " INNER JOIN dbo.Departments AS d ON o.DepartmentId = d.DepartmentId " +
               " WHERE d.DepartmentId=13 AND u.StatusUserId =1 " +
               " UNION SELECT 'Design' as HeadDepartment1,COUNT(*) as CountDepartment1," +
               " (SELECT count(*) FROM dbo.Users WHERE dbo.Users.StatusUserId =1) as TotalEmployee ,'' AS ProgressbarDepartment1 " +
               " FROM dbo.Users AS u " +
               " INNER JOIN dbo.Organizs AS o ON u.organizId = o.organizId " +
               " INNER JOIN dbo.Departments AS d ON o.DepartmentId = d.DepartmentId " +
               " WHERE d.DepartmentId=14 AND u.StatusUserId =1 ";

            var reportDepTE = _context.ReportDeparment1s.FromSqlRaw(queryDepTE).ToList();

            var headerDepTE = "<table>" +
                    " <thead> " +
                    "<tr> " +
                    "<th width='180px'>" +
                    "<sapn><h2></h2></sapn>" +
                    "</th>" +
                    "<th width='50px'>" +
                    "<sapn></sapn>" +
                    "</th>" +
                    "<th style='text-align:right' width='200px'>" +
                    "<sapn></sapn>" +
                    "</th>" +
                    "</tr>" +
                    "</thead>" +
                    "<tbody>";
            var footerDepTE = "</tbody>" +
                            "</table>";

            var bodyDepTE = "";
            decimal percentDepTE = 0;
            var countDepTE = 0;
            var totalempDepTE = 0;


            foreach (var std in reportDepTE as IList<ReportDepartment1>)
            {
                countDepTE = std.CountDepartment1;
                totalempDepTE = std.TotalEmployee1;
                percentDepTE = Percen(countDepTE, totalempDepTE);

                bodyDepTE += " <tr valign='top'>";
                bodyDepTE += " <td style='text-align:right;padding-right:10px'>";
                bodyDepTE += " <span>" + std.HeadDepartment1 + "</span>          ";
                bodyDepTE += " </td>";
                bodyDepTE += " <td>";
                bodyDepTE += " <span>" + std.CountDepartment1 + "</span>";
                bodyDepTE += " </td>";
                bodyDepTE += " <td>";
                bodyDepTE += " <div class='progress'>";
                bodyDepTE += "<div class='progress-bar' role='progress-bar' aria-valuenow='40' style='width:" + percentDepTE + "%' id='bg-progressDep'>" +
                              "<span style='color:#4d4f53;font-weight: bold;font-size:10px;'>" + percentDepTE + "%</span>";
                bodyDepTE += "</div>";
                bodyDepTE += " </td>";
                bodyDepTE += " </tr>";

            }

            ViewData["tableDepTE"] = headerDepTE + bodyDepTE + footerDepTE;


            //Department1
            //--1--
            var queryDep1Cons = "SELECT 'Construction' as HeadDepartment1,COUNT(*) as CountDepartment1," +
               " (SELECT count(*) FROM dbo.Users WHERE dbo.Users.StatusUserId =1) AS TotalEmployee1 ,'' AS ProgressbarDepartment1 " +
               " FROM dbo.Users AS u " +
               " INNER JOIN dbo.Organizs AS o ON u.organizId = o.organizId " +
               " INNER JOIN dbo.Department1s AS dp ON o.Department1Id = dp.Department1Id " +
               " WHERE dp.Department1Id=3 AND u.StatusUserId =1 " +
               " UNION SELECT 'Wherehouse' as HeadDepartment1,COUNT(*) as CountDepartment1," +
               " (SELECT count(*) FROM dbo.Users WHERE dbo.Users.StatusUserId =1) as TotalEmployee ,'' AS ProgressbarDepartment1 " +
               " FROM dbo.Users AS u " +
               " INNER JOIN dbo.Organizs AS o ON u.organizId = o.organizId " +
               " INNER JOIN dbo.Department1s AS dp ON o.Department1Id = dp.Department1Id " +
               " WHERE dp.Department1Id=12 AND u.StatusUserId =1" +
               " UNION SELECT 'Machinery & Equipment' as HeadDepartment1,COUNT(*) as CountDepartment1," +
               " (SELECT count(*) FROM dbo.Users WHERE dbo.Users.StatusUserId =1) as TotalEmployee ,'' AS ProgressbarDepartment1 " +
               " FROM dbo.Users AS u " +
               " INNER JOIN dbo.Organizs AS o ON u.organizId = o.organizId " +
               " INNER JOIN dbo.Department1s AS dp ON o.Department1Id = dp.Department1Id " +
               " WHERE dp.Department1Id=6 AND u.StatusUserId =1 ";

            var reportDep1Cons = _context.ReportDeparment1s.FromSqlRaw(queryDep1Cons).ToList();

            var headerDep1Cons = "<table>" +
                    " <thead> " +
                    "<tr> " +
                    "<th width='180px'>" +
                    "<sapn><h2></h2></sapn>" +
                    "</th>" +
                    "<th width='50px'>" +
                    "<sapn>คน</sapn>" +
                    "</th>" +
                    "<th style='text-align:right' width='200px'>" +
                    "<sapn>%</sapn>" +
                    "</th>" +
                    "</tr>" +
                    "</thead>" +
                    "<tbody>";
            var footerDep1Cons = "</tbody>" +
                            "</table>";

            var bodyDep1Cons = "";
            decimal percentDep1Cons = 0;
            var countDep1Cons = 0;
            var totalempDep1Cons = 0;

            foreach (var std in reportDep1Cons as IList<ReportDepartment1>)
            {
                countDep1Cons = std.CountDepartment1;
                totalempDep1Cons = std.TotalEmployee1;
                percentDep1Cons = Percen(countDep1Cons, totalempDep1Cons);

                bodyDep1Cons += " <tr valign='top'>";
                bodyDep1Cons += " <td style='text-align:right;padding-right:10px'>";
                bodyDep1Cons += " <span>" + std.HeadDepartment1 + "</span>";
                bodyDep1Cons += " </td>";
                bodyDep1Cons += " <td>";
                bodyDep1Cons += " <span>" + std.CountDepartment1 + "</span>";
                bodyDep1Cons += " </td>";
                bodyDep1Cons += " <td>";
                bodyDep1Cons += " <div class='progress'>";
                bodyDep1Cons += "<div class='progress-bar' role='progress-bar' aria-valuenow='40' style='width:" + percentDep1Cons + "%' id='bg-progressDep1'>" +
                                "<span style='color:#4d4f53;font-weight: bold;font-size:10px;'>" + percentDep1Cons + "%</span>";
                bodyDep1Cons += "</div>";
                bodyDep1Cons += " </td>";
                bodyDep1Cons += " </tr>";

            }

            ViewData["tableDep1Cons"] = headerDep1Cons + bodyDep1Cons + footerDep1Cons;

            //--2--
            var queryDep1AdminF = "SELECT 'Acounting' as HeadDepartment1,COUNT(*) as CountDepartment1," +
               " (SELECT count(*) FROM dbo.Users WHERE dbo.Users.StatusUserId =1) AS TotalEmployee1 ,'' AS ProgressbarDepartment1 " +
               " FROM dbo.Users AS u " +
               " INNER JOIN dbo.Organizs AS o ON u.organizId = o.organizId " +
               " INNER JOIN dbo.Department1s AS dp ON o.Department1Id = dp.Department1Id " +
               " WHERE dp.Department1Id=13 AND u.StatusUserId =1 " +
               " UNION SELECT 'Financial' as HeadDepartment1,COUNT(*) as CountDepartment1," +
               " (SELECT count(*) FROM dbo.Users WHERE dbo.Users.StatusUserId =1) as TotalEmployee ,'' AS ProgressbarDepartment1 " +
               " FROM dbo.Users AS u " +
               " INNER JOIN dbo.Organizs AS o ON u.organizId = o.organizId " +
               " INNER JOIN dbo.Department1s AS dp ON o.Department1Id = dp.Department1Id " +
               " WHERE dp.Department1Id=5 AND u.StatusUserId =1" +
               " UNION SELECT 'HR' as HeadDepartment1,COUNT(*) as CountDepartment1," +
               " (SELECT count(*) FROM dbo.Users WHERE dbo.Users.StatusUserId =1) as TotalEmployee ,'' AS ProgressbarDepartment1 " +
               " FROM dbo.Users AS u " +
               " INNER JOIN dbo.Organizs AS o ON u.organizId = o.organizId " +
               " INNER JOIN dbo.Department1s AS dp ON o.Department1Id = dp.Department1Id " +
               " WHERE dp.Department1Id=14 AND u.StatusUserId =1 " +
               " UNION SELECT 'Admin' as HeadDepartment1,COUNT(*) as CountDepartment1," +
               " (SELECT count(*) FROM dbo.Users WHERE dbo.Users.StatusUserId =1) as TotalEmployee ,'' AS ProgressbarDepartment1 " +
               " FROM dbo.Users AS u " +
               " INNER JOIN dbo.Organizs AS o ON u.organizId = o.organizId " +
               " INNER JOIN dbo.Department1s AS dp ON o.Department1Id = dp.Department1Id " +
               " WHERE dp.Department1Id=11 AND u.StatusUserId =1 " +
               " UNION SELECT 'Law' as HeadDepartment1,COUNT(*) as CountDepartment1," +
               " (SELECT count(*) FROM dbo.Users WHERE dbo.Users.StatusUserId =1) as TotalEmployee ,'' AS ProgressbarDepartment1 " +
               " FROM dbo.Users AS u " +
               " INNER JOIN dbo.Organizs AS o ON u.organizId = o.organizId " +
               " INNER JOIN dbo.Department1s AS dp ON o.Department1Id = dp.Department1Id " +
               " WHERE dp.Department1Id=2 AND u.StatusUserId =1 " +
               " UNION SELECT 'NSS' as HeadDepartment1,COUNT(*) as CountDepartment1," +
               " (SELECT count(*) FROM dbo.Users WHERE dbo.Users.StatusUserId =1) as TotalEmployee ,'' AS ProgressbarDepartment1 " +
               " FROM dbo.Users AS u " +
               " INNER JOIN dbo.Organizs AS o ON u.organizId = o.organizId " +
               " INNER JOIN dbo.Department1s AS dp ON o.Department1Id = dp.Department1Id " +
               " WHERE dp.Department1Id=17 AND u.StatusUserId =1 " +
               " UNION SELECT 'SD & Im' as HeadDepartment1,COUNT(*) as CountDepartment1," +
               " (SELECT count(*) FROM dbo.Users WHERE dbo.Users.StatusUserId =1) as TotalEmployee ,'' AS ProgressbarDepartment1 " +
               " FROM dbo.Users AS u " +
               " INNER JOIN dbo.Organizs AS o ON u.organizId = o.organizId " +
               " INNER JOIN dbo.Department1s AS dp ON o.Department1Id = dp.Department1Id " +
               " WHERE dp.Department1Id=16 AND u.StatusUserId =1 ";



            var reportDep1AdminF = _context.ReportDeparment1s.FromSqlRaw(queryDep1AdminF).ToList();

            var headerDep1AdminF = "<table>" +
                    " <thead> " +
                    "<tr> " +
                    "<th width='180px'>" +
                    "<sapn><h2></h2></sapn>" +
                    "</th>" +
                    "<th width='50px'>" +
                    "</th>" +
                    "<th style='text-align:right' width='200px'>" +
                    "</th>" +
                    "</tr>" +
                    "</thead>" +
                    "<tbody>";
            var footerDep1AdminF = "</tbody>" +
                            "</table>";

            var bodyDep1AdminF = "";
            decimal percentDep1AdminF = 0;
            var countDep1AdminF = 0;
            var totalempDep1AdminF = 0;


            foreach (var std in reportDep1AdminF as IList<ReportDepartment1>)
            {
                countDep1AdminF = std.CountDepartment1;
                totalempDep1AdminF = std.TotalEmployee1;
                percentDep1AdminF = Percen(countDep1AdminF, totalempDep1AdminF);

                bodyDep1AdminF += " <tr valign='top'>";
                bodyDep1AdminF += " <td style='text-align:right;padding-right:10px'>";
                bodyDep1AdminF += " <span>" + std.HeadDepartment1 + "</span>          ";
                bodyDep1AdminF += " </td>";
                bodyDep1AdminF += " <td>";
                bodyDep1AdminF += " <span>" + std.CountDepartment1 + "</span>";
                bodyDep1AdminF += " <td>";
                bodyDep1AdminF += " <div class='progress'>";
                bodyDep1AdminF += "<div class='progress-bar' role='progress-bar' aria-valuenow='40' style='width:" + percentDep1AdminF + "%' id='bg-progressDep1'>" +
                                  "<span style='color:#4d4f53;font-weight: bold;font-size:10px;'>" + percentDep1AdminF + "%</span>";
                bodyDep1AdminF += "</div>";
                bodyDep1AdminF += " </td>";
                bodyDep1AdminF += " </tr>";

            }

            ViewData["tableDep1AdminF"] = headerDep1AdminF + bodyDep1AdminF + footerDep1AdminF;

            //--3--
            var queryDep1TechE = "SELECT 'Purchasing' as HeadDepartment1,COUNT(*) as CountDepartment1," +
               " (SELECT count(*) FROM dbo.Users WHERE dbo.Users.StatusUserId =1) AS TotalEmployee1 ,'' AS ProgressbarDepartment1 " +
               " FROM dbo.Users AS u " +
               " INNER JOIN dbo.Organizs AS o ON u.organizId = o.organizId " +
               " INNER JOIN dbo.Department1s AS dp ON o.Department1Id = dp.Department1Id " +
               " WHERE dp.Department1Id=7 OR dp.Department1Id=8 AND u.StatusUserId =1 " +
               " UNION SELECT 'Cost Management' as HeadDepartment1,COUNT(*) as CountDepartment1," +
               " (SELECT count(*) FROM dbo.Users WHERE dbo.Users.StatusUserId =1) as TotalEmployee ,'' AS ProgressbarDepartment1 " +
               " FROM dbo.Users AS u " +
               " INNER JOIN dbo.Organizs AS o ON u.organizId = o.organizId " +
               " INNER JOIN dbo.Department1s AS dp ON o.Department1Id = dp.Department1Id " +
               " WHERE dp.Department1Id=19 AND u.StatusUserId =1" +
               " UNION SELECT 'Technical' as HeadDepartment1,COUNT(*) as CountDepartment1," +
               " (SELECT count(*) FROM dbo.Users WHERE dbo.Users.StatusUserId =1) as TotalEmployee ,'' AS ProgressbarDepartment1 " +
               " FROM dbo.Users AS u " +
               " INNER JOIN dbo.Organizs AS o ON u.organizId = o.organizId " +
               " INNER JOIN dbo.Department1s AS dp ON o.Department1Id = dp.Department1Id " +
               " WHERE dp.Department1Id=10   AND u.StatusUserId =1 " +
               " UNION SELECT 'Estimation' as HeadDepartment1,COUNT(*) as CountDepartment1," +
               " (SELECT count(*) FROM dbo.Users WHERE dbo.Users.StatusUserId =1) as TotalEmployee ,'' AS ProgressbarDepartment1 " +
               " FROM dbo.Users AS u " +
               " INNER JOIN dbo.Organizs AS o ON u.organizId = o.organizId " +
               " INNER JOIN dbo.Department1s AS dp ON o.Department1Id = dp.Department1Id " +
               " WHERE dp.Department1Id=15 AND u.StatusUserId =1 " +
               " UNION SELECT 'Design' as HeadDepartment1,COUNT(*) as CountDepartment1," +
               " (SELECT count(*) FROM dbo.Users WHERE dbo.Users.StatusUserId =1) as TotalEmployee ,'' AS ProgressbarDepartment1 " +
               " FROM dbo.Users AS u " +
               " INNER JOIN dbo.Organizs AS o ON u.organizId = o.organizId " +
               " INNER JOIN dbo.Department1s AS dp ON o.Department1Id = dp.Department1Id " +
               " WHERE dp.Department1Id=18 AND u.StatusUserId =1 ";



            var reportDep1TechE = _context.ReportDeparment1s.FromSqlRaw(queryDep1TechE).ToList();

            var headerDep1TechE = "<table>" +
                    " <thead> " +
                    "<tr> " +
                    "<th width='180px'>" +
                    "<sapn><h2></h2></sapn>" +
                    "</th>" +
                    "<th width='50px'>" +
                    "</th>" +
                    "<th style='text-align:right' width='150px'>" +
                    "</th>" +
                    "</tr>" +
                    "</thead>" +
                    "<tbody>";
            var footerDep1TechE = "</tbody>" +
                            "</table>";

            var bodyDep1TechE = "";
            decimal percentDep1TechE = 0;
            var countDep1TechE = 0;
            var totalempDep1TechE = 0;

            foreach (var std in reportDep1TechE as IList<ReportDepartment1>)
            {
                countDep1TechE = std.CountDepartment1;
                totalempDep1TechE = std.TotalEmployee1;
                percentDep1TechE = Percen(countDep1TechE, totalempDep1TechE);

                bodyDep1TechE += " <tr valign='top'>";
                bodyDep1TechE += " <td style='text-align:right;padding-right:10px'>";
                bodyDep1TechE += " <span>" + std.HeadDepartment1 + "</span> ";
                bodyDep1TechE += " </td>";
                bodyDep1TechE += " <td >";
                bodyDep1TechE += " <span>" + std.CountDepartment1 + "</span>";
                bodyDep1TechE += " </td>";
                bodyDep1TechE += " <td>";
                bodyDep1TechE += " <div class='progress'>";
                bodyDep1TechE += " <div class='progress-bar' role='progress-bar' aria-valuenow='40' style='width:" + percentDep1TechE + "%' id='bg-progressDep1'> " +
                                "  <span style='color:#4d4f53;font-weight: bold;font-size:10px;'>" + percentDep1TechE + "%</span>";
                bodyDep1TechE += " </div>";
                bodyDep1TechE += " </td>";
                bodyDep1TechE += " </tr>";

            }

            ViewData["tableDep1TechE"] = headerDep1TechE + bodyDep1TechE + footerDep1TechE;
     

            return new ViewAsPdf("~/Views/TestPDF/Index.cshtml",viewData:ViewData)
            {
                PageSize = Rotativa.AspNetCore.Options.Size.A4,
                PageOrientation = Rotativa.AspNetCore.Options.Orientation.Landscape,
                CustomSwitches =
            "--footer-right \"   " +
          DateTime.Now.Date.ToString("dd/MM/yyyy") + " \"" +
          " --footer-font-size \"8\" --footer-spacing 1 --footer-font-name \"Segoe UI\""


            };
        }
        public static decimal Percen(decimal value1, decimal value2)
        {
            decimal result = 0;
            try
            {
                result = Math.Round(((value1 / value2) * 100), 2);
            }
            catch
            {
                result = 0;
            }
            return result;
        }



    }
}