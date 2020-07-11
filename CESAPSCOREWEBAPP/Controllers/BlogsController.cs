using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using CESAPSCOREWEBAPP.Models;
using Microsoft.AspNetCore.Http;
using System.IO;
using CESAPSCOREWEBAPP.Helpers;
using static CESAPSCOREWEBAPP.Models.Enums;
using System.Globalization;
using Microsoft.AspNetCore.Authorization;

namespace CESAPSCOREWEBAPP.Controllers
{
    [Authorize]
    public class BlogsController : BaseController
    {
        private readonly DatabaseContext _context;

        public BlogsController(DatabaseContext context)
        {
            _context = context;
        }

        // GET: Blogs
        public async Task<IActionResult> Index()
        {

            /*Check Session */
            var page = "1";
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

            ViewBag.StartDate = DateTime.Now.ToString("01-MM-yyyy", new CultureInfo("en-US"));
            ViewBag.EndDate = DateTime.Now.ToString("dd-MM-yyyy", new CultureInfo("en-US"));

            return View(await _context.Blogs.ToListAsync());
        }

        // GET: Blogs/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            /*Check Session */
            var page = "4";
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

            if (id == null)
            {
                return NotFound();
            }

            if (string.IsNullOrEmpty(HttpContext.Session.GetString("Username")))
            {
                Alert("กรุณา Login เข้าสู่ระบบ", NotificationType.error);
                return RedirectToAction("Login", "Accounts");
            }


            var blogPic = _context.BlogPics.Where(s => s.BlogId == id).ToList();

            ViewData["BlogPic"] = blogPic;

            var blog = _context.Blogs.Where(s => s.BlogId == id).ToList();

            ViewData["blog"] = blog;


            var blogFile = _context.BlogFiles
                            .Where(s => s.BlogId == id)
                            .ToList();
            ViewData["blogFile"] = blogFile;



            return View();
        }

        // GET: Blogs/Create
        public IActionResult Create()
        {
            /*Check Session */
            var page = "2";
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


            ViewData["BlogCatId"] = new SelectList(_context.BlogCats, "BlogCatId", "BlogCatName");
            return View();
        }

        // POST: Blogs/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("BlogId,BlogTitle,BlogDate,BlogEndDate,BlogDetail,BlogPicTitle,BlogCatId,BlogStatus,BlogCreateDate,BlogCreateBy")] Blog blog, List<IFormFile> files, IFormFile uploadPic, List<IFormFile> filesUpload, string Startdate, string EndDate)
        {
            /*Check Session */
            var page = "2";
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

            var date1 = Startdate.Substring(6, 4) + "-" + Startdate.Substring(3, 2) + "-" + Startdate.Substring(0, 2) + " 00:00:00";
            var date2 = EndDate.Substring(6, 4) + "-" + EndDate.Substring(3, 2) + "-" + EndDate.Substring(0, 2) + " 23:59:59";
            var sdate1 = Startdate;
            var sdate2 = EndDate;
            var rdate1 = Startdate;
            var rdate2 = EndDate;




            if (ModelState.IsValid)
            {
                blog.BlogCreateBy = HttpContext.Session.GetString("Username");
                blog.BlogCreateDate = DateTime.Now;


                var check = 0;
                if (uploadPic == null || uploadPic.Length == 0)
                {
                    blog.BlogPicTitle = "NoImage2019-04-09-09-05-46.jpg";
                    check = 1;
                }
                if (check == 0)
                {
                    if (uploadPic.ContentType.IndexOf("image", StringComparison.OrdinalIgnoreCase) < 0)
                    {
                        blog.BlogPicTitle = "NoImage2019-04-09-09-05-46.jpg";
                        check = 1;
                    }
                }
                if (check == 0)
                {
                    string pathImage = "/images/BlogTitle/";
                    string pathSave = $"wwwroot{pathImage}";
                    if (!Directory.Exists(pathSave))
                    {
                        Directory.CreateDirectory(pathSave);
                    }
                    if (!Directory.Exists(pathSave + "/32/"))
                    {
                        Directory.CreateDirectory(pathSave + "/32/");
                    }
                    if (!Directory.Exists(pathSave + "/64/"))
                    {
                        Directory.CreateDirectory(pathSave + "/64/");
                    }
                    if (!Directory.Exists(pathSave + "/128/"))
                    {
                        Directory.CreateDirectory(pathSave + "/128/");
                    }
                    if (!Directory.Exists(pathSave + "/256/"))
                    {
                        Directory.CreateDirectory(pathSave + "/256/");
                    }
                    if (!Directory.Exists(pathSave + "/512/"))
                    {
                        Directory.CreateDirectory(pathSave + "/512/");
                    }
                    if (!Directory.Exists(pathSave + "/1028/"))
                    {
                        Directory.CreateDirectory(pathSave + "/1028/");
                    }

                    string fileName = Path.GetFileNameWithoutExtension(uploadPic.FileName);
                    string extension = Path.GetExtension(uploadPic.FileName);
                    fileName = fileName + DateTime.Now.ToString("yyyy-MM-dd-hh-mm-ss") + extension;
                    var path = Path.Combine(Directory.GetCurrentDirectory(), pathSave, fileName);
                    using (var stream = new FileStream(path, FileMode.Create))
                    {
                        await uploadPic.CopyToAsync(stream);
                    }
                    ImageResizeHelper.Image_resize(pathSave + fileName, pathSave + "/32/" + fileName, 32);
                    ImageResizeHelper.Image_resize(pathSave + fileName, pathSave + "/64/" + fileName, 64);
                    ImageResizeHelper.Image_resize(pathSave + fileName, pathSave + "/128/" + fileName, 128);
                    ImageResizeHelper.Image_resize(pathSave + fileName, pathSave + "/256/" + fileName, 256);
                    ImageResizeHelper.Image_resize(pathSave + fileName, pathSave + "/512/" + fileName, 512);
                    ImageResizeHelper.Image_resize(pathSave + fileName, pathSave + "/1028/" + fileName, 1028);

                    blog.BlogPicTitle = fileName;
                }

                blog.BlogDate =Convert.ToDateTime(date1);
                blog.BlogEndDate = Convert.ToDateTime(date2);

                _context.Add(blog);
                await _context.SaveChangesAsync();
                var IDBLOG = blog.BlogId;

                if (files != null && files.Count > 0)
                {

                    //Upload file
                    string pathImage = "/images/Board/";
                    string pathSave = $"wwwroot{pathImage}";
                    if (!Directory.Exists(pathSave))
                    {
                        Directory.CreateDirectory(pathSave);
                    }
                    if (!Directory.Exists(pathSave + "/32/"))
                    {
                        Directory.CreateDirectory(pathSave + "/32/");
                    }
                    if (!Directory.Exists(pathSave + "/64/"))
                    {
                        Directory.CreateDirectory(pathSave + "/64/");
                    }
                    if (!Directory.Exists(pathSave + "/128/"))
                    {
                        Directory.CreateDirectory(pathSave + "/128/");
                    }
                    if (!Directory.Exists(pathSave + "/256/"))
                    {
                        Directory.CreateDirectory(pathSave + "/256/");
                    }
                    if (!Directory.Exists(pathSave + "/512/"))
                    {
                        Directory.CreateDirectory(pathSave + "/512/");
                    }
                    if (!Directory.Exists(pathSave + "/1028/"))
                    {
                        Directory.CreateDirectory(pathSave + "/1028/");
                    }

                    foreach (IFormFile item in files)
                    {
                        if (item.Length > 0)
                        {
                            string fileName = Path.GetFileNameWithoutExtension(item.FileName);
                            string extension = Path.GetExtension(item.FileName);
                            fileName = fileName + DateTime.Now.ToString("yyyy-MM-dd-hh-mm-ss") + extension;
                            var path = Path.Combine(Directory.GetCurrentDirectory(), pathSave, fileName);
                            using (var stream = new FileStream(path, FileMode.Create))
                            {
                                await item.CopyToAsync(stream);
                            }
                            ImageResizeHelper.Image_resize(pathSave + fileName, pathSave + "/32/" + fileName, 32);
                            ImageResizeHelper.Image_resize(pathSave + fileName, pathSave + "/64/" + fileName, 64);
                            ImageResizeHelper.Image_resize(pathSave + fileName, pathSave + "/128/" + fileName, 128);
                            ImageResizeHelper.Image_resize(pathSave + fileName, pathSave + "/256/" + fileName, 256);
                            ImageResizeHelper.Image_resize(pathSave + fileName, pathSave + "/512/" + fileName, 512);
                            ImageResizeHelper.Image_resize(pathSave + fileName, pathSave + "/1028/" + fileName, 1028);
                            var blogPic = new BlogPic { BlogPicName = fileName ,BlogId= IDBLOG };
                            _context.BlogPics.Add(blogPic);
                            _context.SaveChanges();
                        }
                    }
                }


                //upload File

                if (filesUpload != null && filesUpload.Count > 0)
                {
                    //Upload file
                    string pathImage = "/File/Board/";
                    string pathSave = $"wwwroot{pathImage}";
                    if (!Directory.Exists(pathSave))
                    {
                        Directory.CreateDirectory(pathSave);
                    }
                    foreach (IFormFile item in filesUpload)
                    {
                        if (item.Length > 0)
                        {
                            string fileName = Path.GetFileNameWithoutExtension(item.FileName);
                            string extension = Path.GetExtension(item.FileName);
                            fileName = fileName + DateTime.Now.ToString("yyyy-MM-dd-hh-mm-ss") + extension;
                            var path = Path.Combine(Directory.GetCurrentDirectory(), pathSave, fileName);
                            using (var stream = new FileStream(path, FileMode.Create))
                            {
                                await item.CopyToAsync(stream);
                            }
                            var blogFile = new BlogFile{ BlogFileName = fileName, BlogFileType= extension, BlogId = IDBLOG };
                            _context.BlogFiles.Add(blogFile);
                            _context.SaveChanges();
                        }
                    }
                }



                return RedirectToAction(nameof(Index));
            }
            return View(blog);
        }

        // GET: Blogs/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            /*Check Session */
            var page = "3";
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



            ViewData["BlogCatId"] = new SelectList(_context.BlogCats, "BlogCatId", "BlogCatName");
            if (id == null)
            {
                return NotFound();
            }

            var blog = await _context.Blogs.FindAsync(id);
            if (blog == null)
            {
                return NotFound();
            }
            var blogPic = _context.BlogPics
            .Where(s => s.BlogId == id)
            .ToList();

            var blogFile = _context.BlogFiles
                .Where(s => s.BlogId == id)
                .ToList();


            ViewData["BlogPic"] = blogPic;
            ViewData["blogFile"] = blogFile;
            ViewBag.BlogPicTitle = blog.BlogPicTitle;
            var StartDate = blog.BlogDate;
            var EndDate = blog.BlogEndDate;

            ViewBag.StartDate = StartDate.ToString("dd/MM/yyyy");
            ViewBag.EndDate = EndDate.ToString("dd/MM/yyyy"); ;
            ViewBag.BlogStatus=blog.BlogStatus;
            return View(blog);
        }

        // POST: Blogs/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("BlogId,BlogTitle,BlogDate,BlogEndDate,BlogDetail,BlogPicTitle,BlogCatId,BlogStatus,BlogCreateDate,BlogCreateBy")] Blog blog, List<IFormFile> files, IFormFile uploadPic,string picold, List<IFormFile> filesUpload, string Startdate, string EndDate)
        {
            /*Check Session */
            var page = "3";
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

            var date1 = Startdate.Substring(6, 4) + "-" + Startdate.Substring(3, 2) + "-" + Startdate.Substring(0, 2) + " 00:00:00";
            var date2 = EndDate.Substring(6, 4) + "-" + EndDate.Substring(3, 2) + "-" + EndDate.Substring(0, 2) + " 23:59:59";
            var sdate1 = Startdate;
            var sdate2 = EndDate;
            var rdate1 = Startdate;
            var rdate2 = EndDate;

            if (id != blog.BlogId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {

                    blog.BlogUpdateBy = HttpContext.Session.GetString("Username");
                    blog.BlogUpdateDate = DateTime.Now;
                    var check = 0;
                    if (uploadPic == null || uploadPic.Length == 0)
                    {
                        blog.BlogPicTitle = picold;
                        check = 1;
                    }
                    if (check == 0)
                    {
                        if (uploadPic.ContentType.IndexOf("image", StringComparison.OrdinalIgnoreCase) < 0)
                        {
                            blog.BlogPicTitle = picold;
                            check = 1;
                        }
                    }
                    if (check == 0)
                    {
                        string pathImage = "/images/BlogTitle/";
                        string pathSave = $"wwwroot{pathImage}";
                        if (!Directory.Exists(pathSave))
                        {
                            Directory.CreateDirectory(pathSave);
                        }
                        if (!Directory.Exists(pathSave + "/32/"))
                        {
                            Directory.CreateDirectory(pathSave + "/32/");
                        }
                        if (!Directory.Exists(pathSave + "/64/"))
                        {
                            Directory.CreateDirectory(pathSave + "/64/");
                        }
                        if (!Directory.Exists(pathSave + "/128/"))
                        {
                            Directory.CreateDirectory(pathSave + "/128/");
                        }
                        if (!Directory.Exists(pathSave + "/256/"))
                        {
                            Directory.CreateDirectory(pathSave + "/256/");
                        }
                        if (!Directory.Exists(pathSave + "/512/"))
                        {
                            Directory.CreateDirectory(pathSave + "/512/");
                        }
                        if (!Directory.Exists(pathSave + "/1028/"))
                        {
                            Directory.CreateDirectory(pathSave + "/1028/");
                        }

                        string fileName = Path.GetFileNameWithoutExtension(uploadPic.FileName);
                        string extension = Path.GetExtension(uploadPic.FileName);
                        fileName = fileName + DateTime.Now.ToString("yyyy-MM-dd-hh-mm-ss") + extension;
                        var path = Path.Combine(Directory.GetCurrentDirectory(), pathSave, fileName);
                        using (var stream = new FileStream(path, FileMode.Create))
                        {
                            await uploadPic.CopyToAsync(stream);
                        }
                        ImageResizeHelper.Image_resize(pathSave + fileName, pathSave + "/32/" + fileName, 32);
                        ImageResizeHelper.Image_resize(pathSave + fileName, pathSave + "/64/" + fileName, 64);
                        ImageResizeHelper.Image_resize(pathSave + fileName, pathSave + "/128/" + fileName, 128);
                        ImageResizeHelper.Image_resize(pathSave + fileName, pathSave + "/256/" + fileName, 256);
                        ImageResizeHelper.Image_resize(pathSave + fileName, pathSave + "/512/" + fileName, 512);
                        ImageResizeHelper.Image_resize(pathSave + fileName, pathSave + "/1028/" + fileName, 1028);

                        blog.BlogPicTitle = fileName;
                    }

                    blog.BlogDate = Convert.ToDateTime(date1);
                    blog.BlogEndDate = Convert.ToDateTime(date2);
                    _context.Update(blog);
                    await _context.SaveChangesAsync();
                    var IDBLOG = blog.BlogId;

                    if (files != null && files.Count > 0)
                    {

                        //Upload file
                        string pathImage = "/images/Board/";
                        string pathSave = $"wwwroot{pathImage}";
                        if (!Directory.Exists(pathSave))
                        {
                            Directory.CreateDirectory(pathSave);
                        }
                        if (!Directory.Exists(pathSave + "/32/"))
                        {
                            Directory.CreateDirectory(pathSave + "/32/");
                        }
                        if (!Directory.Exists(pathSave + "/64/"))
                        {
                            Directory.CreateDirectory(pathSave + "/64/");
                        }
                        if (!Directory.Exists(pathSave + "/128/"))
                        {
                            Directory.CreateDirectory(pathSave + "/128/");
                        }
                        if (!Directory.Exists(pathSave + "/256/"))
                        {
                            Directory.CreateDirectory(pathSave + "/256/");
                        }
                        if (!Directory.Exists(pathSave + "/512/"))
                        {
                            Directory.CreateDirectory(pathSave + "/512/");
                        }
                        if (!Directory.Exists(pathSave + "/1028/"))
                        {
                            Directory.CreateDirectory(pathSave + "/1028/");
                        }

                        foreach (IFormFile item in files)
                        {
                            if (item.Length > 0)
                            {
                                string fileName = Path.GetFileNameWithoutExtension(item.FileName);
                                string extension = Path.GetExtension(item.FileName);
                                fileName = fileName + DateTime.Now.ToString("yyyy-MM-dd-hh-mm-ss") + extension;
                                var path = Path.Combine(Directory.GetCurrentDirectory(), pathSave, fileName);
                                using (var stream = new FileStream(path, FileMode.Create))
                                {
                                    await item.CopyToAsync(stream);
                                }
                                ImageResizeHelper.Image_resize(pathSave + fileName, pathSave + "/32/" + fileName, 32);
                                ImageResizeHelper.Image_resize(pathSave + fileName, pathSave + "/64/" + fileName, 64);
                                ImageResizeHelper.Image_resize(pathSave + fileName, pathSave + "/128/" + fileName, 128);
                                ImageResizeHelper.Image_resize(pathSave + fileName, pathSave + "/256/" + fileName, 256);
                                ImageResizeHelper.Image_resize(pathSave + fileName, pathSave + "/512/" + fileName, 512);
                                ImageResizeHelper.Image_resize(pathSave + fileName, pathSave + "/1028/" + fileName, 1028);
                                var blogPic = new BlogPic { BlogPicName = fileName, BlogId = IDBLOG };
                                _context.BlogPics.Add(blogPic);
                                _context.SaveChanges();
                            }
                        }
                    }
                    //upload File

                    if (filesUpload != null && filesUpload.Count > 0)
                    {
                        //Upload file
                        string pathImage = "/File/Board/";
                        string pathSave = $"wwwroot{pathImage}";
                        if (!Directory.Exists(pathSave))
                        {
                            Directory.CreateDirectory(pathSave);
                        }
                        foreach (IFormFile item in filesUpload)
                        {
                            if (item.Length > 0)
                            {
                                string fileName = Path.GetFileNameWithoutExtension(item.FileName);
                                string extension = Path.GetExtension(item.FileName);
                                fileName = fileName + DateTime.Now.ToString("yyyy-MM-dd-hh-mm-ss") + extension;
                                var path = Path.Combine(Directory.GetCurrentDirectory(), pathSave, fileName);
                                using (var stream = new FileStream(path, FileMode.Create))
                                {
                                    await item.CopyToAsync(stream);
                                }
                                var blogFile = new BlogFile { BlogFileName = fileName, BlogFileType=extension,BlogId = IDBLOG };
                                _context.BlogFiles.Add(blogFile);
                                _context.SaveChanges();
                            }
                        }
                    }




                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!BlogExists(blog.BlogId))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            return View(blog);
        }

        // GET: Blogs/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {

            /*Check Session */
            var page = "5";
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

            if (id == null)
            {
                return NotFound();
            }

            if (string.IsNullOrEmpty(HttpContext.Session.GetString("Username")))
            {
                Alert("กรุณา Login เข้าสู่ระบบ", NotificationType.error);
                return RedirectToAction("Login", "Accounts");
            }


            var blogPic = _context.BlogPics.Where(s => s.BlogId == id).ToList();

            ViewData["BlogPic"] = blogPic;

            var blog = _context.Blogs.Where(s => s.BlogId == id).ToList();

            ViewData["blog"] = blog;


            var blogFile = _context.BlogFiles
                            .Where(s => s.BlogId == id)
                            .ToList();
            ViewData["blogFile"] = blogFile;



            return View();
        }

        // POST: Blogs/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            /*Check Session */
            var page = "5";
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




            List<BlogPic> blogPics = _context.BlogPics
                .Where(s => s.BlogId == id)
                .ToList();

              _context.BlogPics.RemoveRange(blogPics);
              _context.SaveChanges();
         

            var blog = await _context.Blogs.FindAsync(id);
            _context.Blogs.Remove(blog);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        // POST: Blogs/Deletepic/5
        [HttpPost, ActionName("Deletepic")]
        public async Task<IActionResult> Deletepic(int id)
        {

            var blogPics= await _context.BlogPics.FindAsync(id);
            _context.BlogPics.Remove(blogPics);
            await _context.SaveChangesAsync();
            return Ok(blogPics);
        }

        // POST: Blogs/Deletefile/5
        [HttpPost, ActionName("Deletefile")]
        public async Task<IActionResult> Deletefile(int id)
        {
            var blogFiles = await _context.BlogFiles.FindAsync(id);
            _context.BlogFiles.Remove(blogFiles);
            await _context.SaveChangesAsync();
            return Ok(blogFiles);
        }


        private bool BlogExists(int id)
        {
            return _context.Blogs.Any(e => e.BlogId == id);
        }
    }
}
