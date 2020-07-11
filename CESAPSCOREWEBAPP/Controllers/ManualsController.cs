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
using Microsoft.AspNetCore.Authorization;

namespace CESAPSCOREWEBAPP.Controllers
{
    [Authorize]
    public class ManualsController : BaseController
    {
        private readonly DatabaseContext _context;

        public ManualsController(DatabaseContext context)
        {
            _context = context;
        }

        // GET: Manuals
        public async Task<IActionResult> Index()
        {
            /*Check Session */
            var page = "202";
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


            var databaseContext = _context.Manuals.Include(m => m.ManualCats);
            return View(await databaseContext.ToListAsync());
        }

        // GET: Manuals/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            /*Check Session */
            var page = "205";
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

            var manual = await _context.Manuals
                .Include(m => m.ManualCats)
                .FirstOrDefaultAsync(m => m.ManualId == id);


            var manualPic = _context.PictureManuals
                .Where(s => s.ManualId == id)
                .ToList();

            ViewData["manualPic"] = manualPic;



            var manualFile = _context.FileManals
                .Where(s => s.ManualId == id)
                .ToList();
            ViewData["manaulFile"] = manualFile;


            if (manual == null)
            {
                return NotFound();
            }
            

            return View(manual);
        }

        // GET: Manuals/Create
        public IActionResult Create()
        {
            /*Check Session */
            var page = "203";
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
            ViewData["ManualCatId"] = new SelectList(_context.ManualCats, "ManualCatId", "ManualCatName");
            return View();
        }

        // POST: Manuals/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("ManualId,ManualName,ManualLink,ManuaDetail,ManualDate,ManuaEditLastDate,ManualHits,ManualEnables,ManualUser,ManualCatId,ManualUserEdit")] Manual manual, List<IFormFile> files, List<IFormFile> filesUpload)
        {
            /*Check Session */
            var page = "203";
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
            if (ModelState.IsValid)
            {
              
                manual.ManualDate = DateTime.Now;
                manual.ManualUser = HttpContext.Session.GetString("Username");
            
                _context.Add(manual);
                await _context.SaveChangesAsync();

                var IDMANUAL = manual.ManualId;

                //upload PictureManual
                if (files != null && files.Count > 0)
                {

                    //Upload file
                    string pathImage = "/images/Manual/";
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

                            var manualPic = new PictureManual { PictureManualName = fileName, ManualId = IDMANUAL };
                            _context.PictureManuals.Add(manualPic);
                            _context.SaveChanges();
                        }
                    }
                }
                //upload File

                if (filesUpload != null && filesUpload.Count > 0)
                {
                    //Upload file
                    string pathImage = "/File/Manual/";
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
                            var fileManual = new FileManal { FileManalName = fileName, FileManalType = extension, ManualId = IDMANUAL };


                            _context.FileManals.Add(fileManual);
                            _context.SaveChanges();
                        }
                    }
                }

                return RedirectToAction(nameof(Index));


            }
            ViewData["ManualCatId"] = new SelectList(_context.ManualCats, "ManualCatId", "ManualCatName", manual.ManualCatId);
            return View(manual);
        }

        // GET: Manuals/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            /*Check Session */
            var page = "204";
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

            var manual = await _context.Manuals.FindAsync(id);
            if (manual == null)
            {
                return NotFound();
            }


            var manualPic = _context.PictureManuals
            .Where(s => s.ManualId == id)
            .ToList();

            var manualFile = _context.FileManals
                .Where(s => s.ManualId == id)
                .ToList();


            ViewData["manaulPic"] = manualPic;
            ViewData["manaulFile"] = manualFile;

            ViewData["ManualCatId"] = new SelectList(_context.ManualCats, "ManualCatId", "ManualCatName", manual.ManualCatId);
            return View(manual);
        }

        // POST: Manuals/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("ManualId,ManualName,ManualLink,ManuaDetail,ManualDate,ManuaEditLastDate,ManualHits,ManualEnables,ManualUser,ManualCatId,ManualUserEdit")] Manual manual, List<IFormFile> files, List<IFormFile> filesUpload)
        {
            /*Check Session */
            var page = "204";
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

            if (id != manual.ManualId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    manual.ManuaEditLastDate = DateTime.Now;
                    manual.ManualUserEdit = HttpContext.Session.GetString("Username");

                    _context.Update(manual);
                    await _context.SaveChangesAsync();
                    var IDMANUAL = manual.ManualId;

                    //upload PictureManual
                    if (files != null && files.Count > 0)
                    {

                        //Upload file
                        string pathImage = "/images/Manual/";
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

                                var manualPic = new PictureManual { PictureManualName = fileName, ManualId = IDMANUAL };
                                _context.PictureManuals.Add(manualPic);
                                _context.SaveChanges();
                            }
                        }
                    }
                    //upload File

                    if (filesUpload != null && filesUpload.Count > 0)
                    {
                        //Upload file
                        string pathImage = "/File/Manual/";
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
                                var fileManual = new FileManal { FileManalName = fileName, FileManalType = extension, ManualId = IDMANUAL };


                                _context.FileManals.Add(fileManual);
                                _context.SaveChanges();
                            }
                        }
                    }

                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ManualExists(manual.ManualId))
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
            ViewData["ManualCatId"] = new SelectList(_context.ManualCats, "ManualCatId", "ManualCatName", manual.ManualCatId);
            return View(manual);
        }

        // GET: Manuals/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            /*Check Session */
            var page = "206";
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

            var manual = await _context.Manuals
                .Include(m => m.ManualCats)
                .FirstOrDefaultAsync(m => m.ManualId == id);
            if (manual == null)
            {
                return NotFound();
            }

            return View(manual);
        }

        // POST: Manuals/Delete/5
        [HttpPost, ActionName("remove")]
 
        public async Task<IActionResult> remove(int id)
        {
            /*Check Session */
            var page = "206";
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
            var manual = await _context.Manuals.FindAsync(id);
            var name = manual.ManualName;

            _context.Manuals.Remove(manual);
            await _context.SaveChangesAsync();

            response = Ok(new{ name=name});
            return response;
        }

        // POST: Blogs/Deletepic/5
        [HttpPost, ActionName("Deletepic")]
        public async Task<IActionResult> Deletepic(int id)
        {
            IActionResult response = Unauthorized();
            var manualPics = await _context.PictureManuals.FindAsync(id);
            var name = manualPics.PictureManualName;

            _context.PictureManuals.Remove(manualPics);
            await _context.SaveChangesAsync();

            response = Ok(new { name = name });
            return response;
        }

        // POST: Blogs/Deletefile/5
        [HttpPost, ActionName("Deletefile")]
        public async Task<IActionResult> Deletefile(int id)
        {
            IActionResult response = Unauthorized();

      
            var manualFiles = await _context.FileManals.FindAsync(id);
            var name = manualFiles.FileManalName;

            _context.FileManals.Remove(manualFiles);
            await _context.SaveChangesAsync();

            response = Ok(new { name = name });
            return response;
        }

        private bool ManualExists(int id)
        {
            return _context.Manuals.Any(e => e.ManualId == id);
        }
    }
}
