using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using CESAPSCOREWEBAPP.Models;
using Microsoft.AspNetCore.Http;
using CESAPSCOREWEBAPP.Helpers;
using System.Text;
using static CESAPSCOREWEBAPP.Models.Enums;
using CESAPSCOREWEBAPP.Controllers;
using System.Net.NetworkInformation;
using System.Runtime.InteropServices;
namespace CESAPSCOREWEBAPP.Models
{
    
    public class AccountsController : BaseController
    {
        private readonly DatabaseContext _context;
        private IHttpContextAccessor _accessor;

        public AccountsController(DatabaseContext context, IHttpContextAccessor accessor)
        {
            _accessor = accessor;
            _context = context;
        }

        public IActionResult Index()
        {


            return View();
        }




        private static string GetMachineNameFromIPAddress(string ipAdress)
        {
            string machineName = string.Empty;
            try
            {
                System.Net.IPHostEntry hostEntry = System.Net.Dns.GetHostEntry(ipAdress);
                machineName = hostEntry.HostName;
            }
            catch (Exception ex)
            {
                //log here
            }
            return machineName;
        }




        public IActionResult Login()
        {


            var ip = _accessor.HttpContext.Connection.RemoteIpAddress?.ToString();
            var MacAddress = GetClientData.GetMACAddress();
            ViewBag.MacAddress = MacAddress;
            var userAgent = Request.Headers["User-Agent"];
            UserAgent.UserAgent ua = new UserAgent.UserAgent(userAgent);



            HttpContext.Session.SetString("Macaddress", MacAddress);
            HttpContext.Session.SetString("OS", ua.OS.Name);
            HttpContext.Session.SetString("OSVersion", ua.OS.Version);
            HttpContext.Session.SetString("Browser", ua.Browser.Name);
            HttpContext.Session.SetString("BrowserVersion", ua.Browser.Version);
            HttpContext.Session.SetString("IPAddress", ip);
            HttpContext.Session.SetString("LoginDate", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"));




            //ViewBag.MacAddress = MacAddress;
            //ViewBag.OS = ua.OS.Name;
            //ViewBag.Browser = ua.Browser.Name;
            //ViewBag.BrowserVersion = ua.Browser.Version;
            //ViewBag.IPAddress = ip;
            //ViewBag.LoginDate = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");


            if ( Request.Cookies["user"] != null )
            {
                ViewBag.user= Request.Cookies["user"];
                ViewBag.pass=Request.Cookies["pass"];
                ViewBag.remember=Request.Cookies["remember"];
               
            }

       

            return View(ua);


        }

        // POST: Permisions/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Login(string username, string password,bool remember, string Latitude, string Longitude)
        {

          


            if (username != null && password != null)
            {
                string hash = EncryptionHelper.Encrypt(password);


                //IQueryable<User> query = _context.Users;
                var login = _context.Logins
                    .Include(p=>p.Users)
     
                    .Include(p => p.Users.TitleOfUsers)
                    .Include(p=>p.TypeOfUsers)
                    .Include(p=>p.Permisions)
                    .Include(p=>p.CheckUsers)
                    .FirstOrDefault( p => p.Username.Equals(username) && p.Password.Equals(hash));
                if (login == null)
                {
                    Alert("รหัสผ่านผิดพลาด", NotificationType.error);
                    //TempData["SweetAlertTempdata"] = "ทดสอบ";
                    return View();
                    //return RedirectToAction("Login", "Accounts");
                }
                else
                {
                    if (login.CheckUsers.CheckUserId == 1)
                    {
                        Alert("กรุณาติดต่อ Admin เพื่อยืนยันสิทธิ์การใช้งาน", NotificationType.info);
                        //ViewBag.error = "กรุณาติดต่อ Admin เพื่อยืนยันสิทธิ์การใช้งาน";
                        return View();
                    }
                    else if (login.CheckUsers.CheckUserId == 3)
                    {
                        Alert("สิทธิ์การใช้งานของท่านถูกระงับ", NotificationType.error);
                        return View();
                    }
                    else
                    {

                        HttpContext.Session.SetInt32("Userid", login.UserId);
                        HttpContext.Session.SetString("Username", login.Username);
                        HttpContext.Session.SetString("Firstname", login.Users.Firstname);
                        HttpContext.Session.SetString("Lastname", login.Users.Lastname);
                        HttpContext.Session.SetString("Pic", login.Users.Pic);
                        HttpContext.Session.SetString("TypeOfUserId", login.TypeOfUserId.ToString());
                        HttpContext.Session.SetString("TitleOfUserName", login.Users.TitleOfUsers.TitleOfUserName);
                        HttpContext.Session.SetString("TypeOfUserName", login.TypeOfUsers.TypeOfUserName);
                        HttpContext.Session.SetString("CheckUserName", login.CheckUsers.CheckUserName);
                        HttpContext.Session.SetString("PermisionName", login.Permisions.PermisionName);
                        HttpContext.Session.SetString("PermisionAction", login.Permisions.PermisionAction);

                        CookieOptions option = new CookieOptions();
                        int? expireTime=600000;

                        if (expireTime.HasValue)
                            option.Expires = DateTime.Now.AddMinutes(expireTime.Value);
                        else
                            option.Expires = DateTime.Now.AddMilliseconds(600000);

                        if(remember==true){
                            Response.Cookies.Append("user", username, option);
                            Response.Cookies.Append("pass",password, option);
                            Response.Cookies.Append("remember","true", option);
                        }else{
                           Response.Cookies.Delete("user");
                            Response.Cookies.Delete("pass");
                            Response.Cookies.Delete("remember");
                        }

                       

                        var ip = _accessor.HttpContext.Connection.RemoteIpAddress?.ToString();
                        var MacAddress = GetClientData.GetMACAddress();
                        var userAgent = Request.Headers["User-Agent"];
                        UserAgent.UserAgent ua = new UserAgent.UserAgent(userAgent);


                        Monitor Monitors = new Monitor();
                        Monitors.Date = DateTime.Now;
                        Monitors.IP = ip;
                        Monitors.Mac = MacAddress;
                        Monitors.OS = ua.OS.Name;
                        Monitors.OSVersion = ua.OS.Version;
                        Monitors.Browser = ua.Browser.Name;
                        Monitors.BrowserVersion = ua.Browser.Version;
                        Monitors.Latitude = Latitude;
                        Monitors.Longitude = Longitude;
                        Monitors.Username = username;

                        _context.Add(Monitors);
                        _context.SaveChanges();


                        //LineAlert.LineNotify(username+" เข้าระบบเมื่อ  :" +DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss"));
                        Alert("Login", NotificationType.success);
                  
                    }

                    return RedirectToAction("Index", "Home");
                    //return Redirect("Home/Index");
                }
            }
            else
            {

                Alert("Invalid Account", NotificationType.error);
                return View(); ;
            }

        }

        public IActionResult Logout()
        {


            HttpContext.Session.Remove("Userid");
            HttpContext.Session.Remove("Username");
            HttpContext.Session.Remove("Firstname");
            HttpContext.Session.Remove("Lastname");
            HttpContext.Session.Remove("Pic");
            HttpContext.Session.Remove("PositionName");
            HttpContext.Session.Remove("TypeOfUserId");
            HttpContext.Session.Remove("TitleOfUserName");
            HttpContext.Session.Remove("TypeOfUserName");
            HttpContext.Session.Remove("LevelName");
            HttpContext.Session.Remove("CheckUserName");
            HttpContext.Session.Remove("PermisionAction");

            //Detail Data
            HttpContext.Session.Remove("Macaddress");
            HttpContext.Session.Remove("OS");
            HttpContext.Session.Remove("OSVersion");
            HttpContext.Session.Remove("Browser");
            HttpContext.Session.Remove("BrowserVersion");
            HttpContext.Session.Remove("IPAddress");
            HttpContext.Session.Remove("LoginDate");


            return RedirectToAction("Login", "Accounts");
        }



    }
}