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
using System.Globalization;

namespace CESAPSCOREWEBAPP.Controllers
{
    public class CHQDirectController : BaseController
    {


        private readonly NAVContext _navcontext;
        private readonly DatabaseContext _context;


        public CHQDirectController(NAVContext navcontext, DatabaseContext context)
        {
            _context = context;
            _navcontext = navcontext;
        }


        public IActionResult Index()
        {


            /*Check Session */
            var page = "255";
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


            ViewBag.StartDate = DateTime.Now.ToString("01-01-yyyy", new CultureInfo("en-US"));
            ViewBag.EndDate = DateTime.Now.ToString("dd-MM-yyyy", new CultureInfo("en-US"));


            return View();
        }



        public IActionResult Gendata(string Startdate, string EndDate)
        {
            /*Check Session */
            var page = "255";
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


            var date1 = Startdate.Substring(6, 4) + "-" + Startdate.Substring(3, 2) + "-" + Startdate.Substring(0, 2) + " 00:00:00";
            var date2 = EndDate.Substring(6, 4) + "-" + EndDate.Substring(3, 2) + "-" + EndDate.Substring(0, 2) + " 23:59:59";
            var sdate1 = Startdate;
            var sdate2 = EndDate;
            var rdate1 = Startdate;
            var rdate2 = EndDate;



            IActionResult response = Unauthorized();

            var query = "SELECT ROW_NUMBER() OVER (ORDER BY a.PostingDate) AS ID, * , " +
                "CASE  " +
                "WHEN a.MonthDate='01' THEN 'มกราคม' " +
                "WHEN a.MonthDate='02' THEN 'กุมภาพันธ์' " +
                "WHEN a.MonthDate='03' THEN 'มีนาคม' " +
                "WHEN a.MonthDate='04' THEN 'เมษายน' " +
                "WHEN a.MonthDate='05' THEN 'พฤษภาคม' " +
                "WHEN a.MonthDate='06' THEN 'มิถุนายน' " +
                "WHEN a.MonthDate='07' THEN 'กรกฏาคม' " +
                "WHEN a.MonthDate='08' THEN 'สิงหาคม' " +
                "WHEN a.MonthDate='09' THEN 'กันยายน' " +
                "WHEN a.MonthDate='10' THEN 'ตุลาคม' " +
                "WHEN a.MonthDate='11' THEN 'พฤศจิกายน' " +
                "WHEN a.MonthDate='12' THEN 'ธันวาคม' " +
                "ELSE '' END as MonthTH " +
                "FROM ( " +
                "SELECT " +
                "CONVERT(varchar,Year(dbo.[C_E_S_ CO_, LTD_$Check Ledger Entry].[Posting Date])+543) as YearDate, " +
                "CASE WHEN Month(dbo.[C_E_S_ CO_, LTD_$Check Ledger Entry].[Posting Date])<'10' THEN CONCAT('0',Month(dbo.[C_E_S_ CO_, LTD_$Check Ledger Entry].[Posting Date])) " +
                "ELSE CONVERT(VARCHAR,Month(dbo.[C_E_S_ CO_, LTD_$Check Ledger Entry].[Posting Date])) END as MonthDate, " +
                "dbo.[C_E_S_ CO_, LTD_$Check Ledger Entry].[Posting Date] as PostingDate, " +
                "Count(dbo.[C_E_S_ CO_, LTD_$Check Ledger Entry].[Posting Date]) as CountData, " +
                "SUM(dbo.[C_E_S_ CO_, LTD_$Check Ledger Entry].Amount) as SumTotal " +
                "FROM [dbo].[C_E_S_ CO_, LTD_$Check Ledger Entry] " +
                "WHERE [Document No_] LIKE 'PVB%' " +
                "GROUP BY dbo.[C_E_S_ CO_, LTD_$Check Ledger Entry].[Posting Date]  " +
                ")as a WHERE a.PostingDate >={0} and a.PostingDate<={1}" +
                " ORDER BY a.PostingDate ";


            //var head = "<table id='itemmetrix' class='table'>"
            //      + "<thead>"
            //      + "<tr>"
            //          + "<th>เดือน</th>"
            //          + "<th style=\'text-align :right;\'>งวดที่ 1 ฉบับ</th>"
            //          + "<th style=\'text-align :right;\'>จำนวนเงิน</th>"
            //          + "<th style=\'text-align :right;\'>งวดที่ 2 ฉบับ</th>"
            //          + "<th style=\'text-align :right;\'>จำนวนเงิน</th>"
            //          + "<th style=\'text-align :right;\'>รวมฉบับ</th>"
            //          + "<th style=\'text-align :right;\'>รวมทั้งหมด</th>" +
            //      "</tr>" +
            //      "</thead>" +
            //      "<tbody>";

       

            var checkmonth = "";
            var checkyear = "";
            var table = "";
            var count = 0;
            var countdata = 0;
            var i = 1;



            List<CHQDirectReportGen> cHQDirectReportGens = new List<CHQDirectReportGen>();
            CHQDirectReportGen cHQDirectReportGen =new CHQDirectReportGen();

            var CHQDirects = _navcontext.CHQDirects.FromSqlRaw(query,date1,date2).ToList();
            foreach(var std in CHQDirects)
            {


          
                if (checkyear == std.YearDate && checkmonth == std.MonthDate)   //ถ้าเป็นรอบที่ 2 
                {

                    //cHQDirectReportGen = new CHQDirectReportGen();
                        i++;
                        countdata = 0;
                }
                else
                {
                    cHQDirectReportGen = new CHQDirectReportGen();
                    i = 1;
                    table += "<tr>";

                    checkyear = std.YearDate;
                    checkmonth = std.MonthDate;
                    countdata = 1;
                    table += "<td>" + std.MonthTH + "-" + std.YearDate + "</td>";
                    cHQDirectReportGen.Month = std.MonthTH + " " + std.YearDate;


                    count = CHQDirects.Where(p => p.MonthDate == std.MonthDate && p.YearDate == std.YearDate).Count();
                    if (count > 2)
                    {
                        cHQDirectReportGen.Q1 = CHQDirects.Where(p => p.MonthDate == std.MonthDate && p.YearDate == std.YearDate && p.PostingDate.Day < 13).Sum(p => p.CountData);
                        cHQDirectReportGen.TotalQ1 = Math.Round(CHQDirects.Where(p => p.MonthDate == std.MonthDate && p.YearDate == std.YearDate && p.PostingDate.Day < 13).Sum(p => p.SumTotal), 2);
                        //table += "<td style=\'text-align :right;\'>" + CHQDirects.Where(p => p.MonthDate == std.MonthDate && p.YearDate == std.YearDate && p.PostingDate.Day < 10).Sum(p => p.CountData) + "</td>";
                        //table += "<td style=\'text-align :right;\'>" + Math.Round(CHQDirects.Where(p => p.MonthDate == std.MonthDate && p.YearDate == std.YearDate && p.PostingDate.Day < 10).Sum(p => p.SumTotal), 2) + "</td>";
                    }
                    else if(count==1)
                    {
                        cHQDirectReportGen.Q1 = std.CountData;
                        cHQDirectReportGen.TotalQ1 = Math.Round(std.SumTotal, 2);
                        cHQDirectReportGen.Q2= 0;
                        cHQDirectReportGen.TotalQ2 = 0.00M;
                        cHQDirectReportGen.QTotal = CHQDirects.Where(p => p.MonthDate == std.MonthDate && p.YearDate == std.YearDate).Sum(p => p.CountData);
                        cHQDirectReportGen.QAmount = Math.Round(CHQDirects.Where(p => p.MonthDate == std.MonthDate && p.YearDate == std.YearDate).Sum(p => p.SumTotal), 2);
                        cHQDirectReportGens.Add(cHQDirectReportGen);

                    }
                    else
                    {
                        cHQDirectReportGen.Q1 = std.CountData;
                        cHQDirectReportGen.TotalQ1 = Math.Round(std.SumTotal, 2);
                        //table += "<td style=\'text-align :right;\'>" + std.CountData + "</td>";
                        //table += "<td style=\'text-align :right;\'>" + Math.Round(std.SumTotal, 2) + "</td>";
                     }
                }




                if (i == 3)
                {

                }
                else
                {
                   
                    if (countdata != 1)
                    {
                        
                            count = CHQDirects.Where(p => p.MonthDate == std.MonthDate && p.YearDate == std.YearDate).Count();
                            if (count > 2)
                            {

                                cHQDirectReportGen.Q2 = CHQDirects.Where(p => p.MonthDate == std.MonthDate && p.YearDate == std.YearDate && p.PostingDate.Day >= 13).Sum(p => p.CountData);
                                cHQDirectReportGen.TotalQ2 = Math.Round(CHQDirects.Where(p => p.MonthDate == std.MonthDate && p.YearDate == std.YearDate && p.PostingDate.Day >= 13).Sum(p => p.SumTotal), 2);
                                cHQDirectReportGen.QTotal = CHQDirects.Where(p => p.MonthDate == std.MonthDate && p.YearDate == std.YearDate).Sum(p => p.CountData);
                                cHQDirectReportGen.QAmount = Math.Round(CHQDirects.Where(p => p.MonthDate == std.MonthDate && p.YearDate == std.YearDate).Sum(p => p.SumTotal), 2);



                                //table += "<td style=\'text-align :right;\'>" + CHQDirects.Where(p => p.MonthDate == std.MonthDate && p.YearDate == std.YearDate &&p.PostingDate.Day>=10 ).Sum(p => p.CountData) + "</td>";
                                //table += "<td style=\'text-align :right;\'>" + Math.Round(CHQDirects.Where(p => p.MonthDate == std.MonthDate && p.YearDate == std.YearDate && p.PostingDate.Day >= 10).Sum(p => p.SumTotal), 2) + "</td>";
                                //table += "<td style=\'text-align :right;\'>" + CHQDirects.Where(p => p.MonthDate == std.MonthDate && p.YearDate == std.YearDate).Sum(p => p.CountData) + "</td>";
                                //table += "<td style=\'text-align :right;\'>" + Math.Round(CHQDirects.Where(p => p.MonthDate == std.MonthDate && p.YearDate == std.YearDate).Sum(p => p.SumTotal), 2) + "</td>";
                                //table += "</tr>";
                                countdata = 0;
                            }
                            else 
                            {
                                cHQDirectReportGen.Q2 = std.CountData;
                                cHQDirectReportGen.TotalQ2 = Math.Round(std.SumTotal, 2);
                                cHQDirectReportGen.QTotal = CHQDirects.Where(p => p.MonthDate == std.MonthDate && p.YearDate == std.YearDate).Sum(p => p.CountData);
                                cHQDirectReportGen.QAmount = Math.Round(CHQDirects.Where(p => p.MonthDate == std.MonthDate && p.YearDate == std.YearDate).Sum(p => p.SumTotal), 2);

                                //table += "<td style=\'text-align :right;\'>" + std.CountData + "</td>";
                                //table += "<td style=\'text-align :right;\'>" + Math.Round(std.SumTotal,2) + "</td>";
                                //table += "<td style=\'text-align :right;\'>" + CHQDirects.Where(p => p.MonthDate == std.MonthDate && p.YearDate == std.YearDate).Sum(p=>p.CountData)+ "</td>";
                                //table += "<td style=\'text-align :right;\'>" + Math.Round(CHQDirects.Where(p => p.MonthDate == std.MonthDate && p.YearDate == std.YearDate).Sum(p => p.SumTotal),2) + "</td>";
                                //table += "</tr>";
                                countdata = 0;
                            }

                            cHQDirectReportGens.Add(cHQDirectReportGen);
                       

                        //if (CHQDirects.Where(p => p.MonthDate == std.MonthDate && p.YearDate == std.YearDate && p.PostingDate.Day >= 13).Count() > 0)
                        //{


                        //}

                        //else
                        //{
                        //    cHQDirectReportGens.Add(cHQDirectReportGen);
                        //}
                    }


                  
                }

            }
       
            //var footer = "<tfoot>"
            //     + "<tr>"
            //     + "<td></td>" +
            //     "<td></td>" +
            //       "<td></td>" +
            //         "<td></td>" +
            //     "<td style=\'text-align :right;\'>Total</td>" +
            //     "<td style=\'text-align :right;\'>"+ CHQDirects.Sum(p => p.CountData) + "</td>" +
            //     "<td style=\'text-align :right;\'>" + Math.Round(CHQDirects.Sum(p => p.SumTotal),2) + "</td>" +
            //     "</tr>";








           // var table1 = "</tbody></table>";
           //var total_table = head + table + footer + table1;


            response = Ok(new { data= cHQDirectReportGens });
            //response = Ok(new { table = query });
            //response = Ok(new { table = queryLocation });
            return response;

        }



    }
}