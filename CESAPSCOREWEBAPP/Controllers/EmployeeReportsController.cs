using System;
using System.Collections.Generic;
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
    public class EmployeeReportsController : BaseController
    {
        private readonly DatabaseContext _context;
        private const string XlsxContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";

        public EmployeeReportsController(DatabaseContext context)
        {
            _context = context;
        }

        public IActionResult Index()
        {
            /*Check Session */
            var page = "208";
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

            ViewBag.Departments = _context.Departments.ToList();
            ViewBag.branchs = _context.Branchs.ToList();
            ViewBag.statusUsers = _context.statusUsers.ToList();
            ViewBag.typeOfEmp = _context.typeOfEmployee.ToList();
         

            return View();
        }

        public IActionResult ShowEmployee()
        {
            /*Check Session */
            var page = "208";
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

          var  users = _context.Users
                      .Include(p => p.TitleOfUsers)
                      .Include(p => p.StatusUser)
                      .Include(p => p.Branchs)
                      .Include(p => p.Bloods)
                      .Include(p => p.nationality)
                      .Include(p => p.povince)
                      .Include(p => p.TypeCongrates)
                      .Include(p => p.religions)
                      .Include(p => p.typeOfEmployee)
         
                     .ToList();


            var header = "<table id='example' class='table table-striped table-bordered table-hover dataTables-example ' style='width:100%'>" +
                             " <thead> " +
                             "<tr> " +
                             "<th>รหัส</th> " +
                            "<th>ชื่อ - สกุล</th> " +
                            "<th>ชื่อเล่น</th> " +
                            "<th>อีเมลล์</th> " +
                            "<th>เบอร์ติดต่อ</th> " +
                            "<th>วันที่เริ่มงาน</th> " +
                            "<th>หน่วยงาน</th> " +
                            "<th>ประเภท</th> " +
                            "<th>สถานะ</th> " +
                             "</tr>" +
                             "</thead>" +
                             "<tbody>";

            var footer = "</tbody>" +
                         "</table>";
            var body = "";
            int i = 0;
            foreach (var user in users as IList<User>) //เช็คค่าตาม Location
            {
                body += "<tr>";
                body += "<td>" + user.EmpId + "</td>";
                body += "<td>" + user.TitleOfUsers.TitleOfUserName + user.Firstname + " " + user.Lastname + "</td>";
                body += "<td>" + user.Nickname + "</td>";
                body += "<td>" + user.EmailContact + "</td>";
                body += "<td>" + user.ExtTel + "</td>";
                body += "<td>" + user.UserCreateDate.ToString("dd/MM/yyyy") + "</td>";
                body += "<td>" + user.Branchs.BranchName + "</td>";
                body += "<td>" + user.typeOfEmployee.TypeOfEmployeeName + "</td>";
                body += "<td>" + user.StatusUser.StatusUserName + "</td>";

                body += "</tr>";
                
            }


            var tableReport = header + body + footer;
            response = Ok(new{data = tableReport, sumdata = i });

            return response;
        }

            public IActionResult ReportEmployees(int ddlTypeOfEmp, int ddlDep, int ddlDep1s, int ddlPosition, int ddlBranch,int ddlLevels,int ddlStatus)
        {
            /*Check Session */
            var page = "208";
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
            char[] charsToTrim = { '*', ' ', '\'' };
            List<User> users;
            try
            {
                IActionResult response = Unauthorized();
                users = _context.Users
                      .Include(p => p.TitleOfUsers)
                      .Include(p => p.StatusUser)
                      .Include(p => p.Branchs)
                      .Include(p => p.Bloods)
                      .Include(p => p.nationality)
                      .Include(p => p.povince)
                      .Include(p => p.TypeCongrates)
                      .Include(p => p.religions)
                      .Include(p => p.typeOfEmployee)
                
                     .ToList();

                int i = 0;

                if (ddlTypeOfEmp != -1)
                {
                    users = users.Where(p =>
                        p.typeOfEmployee.TypeOfEmployeeId == ddlTypeOfEmp).ToList();
                }


                if (ddlBranch != -1)
                {
                    users = users.Where(p =>
                        p.Branchs.BranchId == ddlBranch).ToList();
                }
     
                if (ddlStatus != -1)
                {
                    users = users.Where(p =>
                         p.StatusUser.StatusUserId == ddlStatus).ToList();

                }

                var header = "<table id='example' class='table table-striped table-bordered table-hover dataTables-example' style='width:100%'>" +
                             " <thead> " +
                             "<tr> " +
                             "<th>รหัส</th> " +
                            "<th>ชื่อ - สกุล</th> " +
                            "<th>ชื่อเล่น</th> " +
                            "<th>อีเมลล์</th> " +
                            "<th>เบอร์ติดต่อ</th> " +
                            "<th>วันที่เริ่มงาน</th> " +
                            "<th>หน่วยงาน</th> " +
                            "<th>ประเภท</th> " +
                            "<th>สถานะ</th> " +
                             "</tr>" +
                             "</thead>" +
                             "<tbody>";

                var footer = "</tbody>" +
                             "</table>";
                var body = "";
                foreach (var user in users as IList<User>) //เช็คค่าตาม Location
                {
                    body += "<tr>";
                    body += "<td>" + user.EmpId + "</td>";
                    body += "<td>" + user.TitleOfUsers.TitleOfUserName + user.Firstname + " " + user.Lastname + "</td>";
                    body += "<td>" + user.Nickname + "</td>";
                    body += "<td>" + user.EmailContact + "</td>";
                    body += "<td>" + user.ExtTel + "</td>";
                    body += "<td>" + user.UserCreateDate.ToString("dd/MM/yyyy") + "</td>";
                        body += "<td>" + user.Branchs.BranchName + "</td>";
                    body += "<td>" + user.typeOfEmployee.TypeOfEmployeeName + "</td>";
                    body += "<td>" + user.StatusUser.StatusUserName + "</td>";

                    body += "</tr>";
                    i = i + 1;
                }


                var tableReport = header + body + footer;
                //ViewBag.tableReport = tableReport;
                response = Ok(new { tableReport = tableReport, sumdata = i });
                return response;
                //return View();

            }
            catch
            {
                return BadRequest();
            }

            //return View(users);
        }



    }
}