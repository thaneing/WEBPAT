using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CESAPSCOREWEBAPP.Models;
using DevExpress.Data.Linq.Helpers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace CESAPSCOREWEBAPP.Controllers
{
    [Authorize]
    public class ShowManualController : BaseController
    {

        private readonly DatabaseContext _context;
        private readonly NAVContext _navcontext;

        public ShowManualController(DatabaseContext context)
        {
            _context = context;
        }

        public IActionResult Index()
        {

            IActionResult response = Unauthorized();
            var manuals = _context.Manuals.ToList();


            List<SourceAutoComplete> sourceAutoCompletesnew = new List<SourceAutoComplete>();  //table
            SourceAutoComplete CurrentsourceAutoComplete = null;  //row

    

            foreach (var std in manuals as IList<Manual>)
            {
                CurrentsourceAutoComplete = new SourceAutoComplete();
                CurrentsourceAutoComplete.code = std.ManualName;
                CurrentsourceAutoComplete.name = std.ManualName;
                sourceAutoCompletesnew.Add(CurrentsourceAutoComplete);
            }

            ViewData["SourceAutoCompletes"] = sourceAutoCompletesnew;

            var manualCat = _context.ManualCats.OrderBy(p => p.ManualCatName).ToList();
            ViewData["manualcat"] = manualCat;

            var cat = _context.ManualCats.OrderBy(m=>m.ManualCatName).ToList();
            List<Tmp_MannualCat> instances = new List<Tmp_MannualCat>();
            Tmp_MannualCat tmp = null;
            foreach (var std in cat as IList<ManualCat>)
            {
                var MID = std.ManualCatId;

                tmp = new Tmp_MannualCat();
                tmp.ManualCatId = std.ManualCatId;
                tmp.ManualCatName = std.ManualCatName;
                tmp.CountData = _context.Manuals.Where(p => p.ManualCatId == MID).Count();
                instances.Add(tmp);
            }

            ViewData["count"] = instances;

            return View();
      
        }

      

        public IActionResult showdata(int id)
        {
            IActionResult response = Unauthorized();

            var manuals = _context.Manuals
                .Include(c => c.ManualCats)
                .Where(c => c.ManualCatId == id).ToList();

            var bodylist = "";

            foreach (var std in manuals as IList<Manual>)
            {
                //bodylist += "<div class='col-lg-12'>";
                bodylist += "<div class='col-lg-8'>";
                bodylist += "<a style='color:#1c79c0;font-size: 15px;' href='../Manuals/Details/" + std.ManualId+ "'>" + std.ManualCats.ManualCatName+" - " + std.ManualName + "</a>";
                bodylist += "<p id='textover'>" + std.ManuaDetail + "</p>  ";
                bodylist += "<p class='fcolor'>โดย : " + std.ManualUser + " เมื่อวันที่ : " + std.ManualDate.ToString("yyyy-MM-dd") + "</p>";
                bodylist += "</div>";
                bodylist += "<div class='col-lg-2'>";
                bodylist += "<p><b>ดาวโหลดไฟล์คู่มือ <i class='fa fa-download'></i>  </b></p>";
                //bodylist += "<button class='btn' type='button'data-toggle='collapse' data-target='#demo'> ดาวน์โหลดคู่มือ <i class='fa fa-download'></i></button>";
                //bodylist += "</div>";



                var file = _context.FileManals.Where(c => c.ManualId == std.ManualId).ToList();
                foreach (var stf in file as IList<FileManal>)
                {

                    var type = stf.FileManalType;
                    if (type == ".pdf")
                    {
                        bodylist += " <a href='/File/Manual/" + stf.FileManalName + "' data-toggle='tooltip' data-placement='top' title='" + stf.FileManalName + "'><img src='/Images/pdf.png' width='25px'></span></a> ";
                    }
                    if (type == ".xlsx")
                    {
                        bodylist += " <a href='/File/Manual/" + stf.FileManalName + "' data-toggle='tooltip' data-placement='top' title='" + stf.FileManalName + "'><img src='/Images/xls.png' width='25px'></span></a> ";
                    }
                    if (type == ".pptx")
                    {
                        bodylist += " <a href='/File/Manual/" + stf.FileManalName + "' data-toggle='tooltip' data-placement='top' title='" + stf.FileManalName + "'><img src='/Images/ppt.png' width='25px'></span></a> ";
                    }
                    if (type == ".docx")
                    {
                        bodylist += " <a href='/File/Manual/" + stf.FileManalName + "' data-toggle='tooltip' data-placement='top' title='" + stf.FileManalName + "'><img src='/Images/doc.png' width='25px'></span></a> ";
                    }


                }
                bodylist += "</div>";
                bodylist += "<div class='col-lg-2'>";
                bodylist += "<p><b>รูปภาพคู่มือ <i class='glyphicon glyphicon-picture'></i>  </b></p>";
                
                var pic = _context.PictureManuals.Where(c => c.ManualId == std.ManualId).ToList();

                foreach (var stp in pic as IList<PictureManual>)
                {
                    //bodylist += "<span><img alt = 'image' class='img - circle' src='/Images/Manual/" + stp.PictureManualName + "' width='25px' /></span> ";
                    bodylist += "<span><a href = '/Images/Manual/" + stp.PictureManualName + "'  class='modaal-image gallery-thumb' data-modaal-desc='test' id='abc'><img alt = 'image' class='img - circle' src='/Images/Manual/" + stp.PictureManualName + "' width='25px' /></a></span> ";

                }
               bodylist += "</div>";
                bodylist += "<div class='col-lg-12'><hr></div>";
            }

            var tableMannual = bodylist;

            response = Ok(new
            {
                tableMannual = tableMannual,
          

            });
            return response;
        }
        public IActionResult getdata(int id)
        {
            IActionResult response = Unauthorized();

            var manuals = _context.Manuals
                .Include(c => c.ManualCats).ToList();

            var bodylist = "";

            foreach (var std in manuals as IList<Manual>)
            {
                //bodylist += "<div class='col-lg-12'>";
                bodylist += "<div class='col-lg-8'>";
                bodylist += "<a style='color:#1c79c0;font-size: 15px;' href='../Manuals/Details/" + std.ManualId + "'>" + std.ManualCats.ManualCatName + " - " + std.ManualName + "</a>";
                bodylist += "<p id='textover'>" + std.ManuaDetail + "</p>  ";
                bodylist += "<p class='fcolor'>โดย : " + std.ManualUser + " เมื่อวันที่ : " + std.ManualDate.ToString("yyyy-MM-dd") + "</p>";
                bodylist += "</div>";
                bodylist += "<div class='col-lg-4'>";
                bodylist += "<p><b>ดาวโหลดไฟล์คู่มือ <i class='fa fa-download'></i> </b></p>";
                //bodylist += "<button class='btn' type='button'data-toggle='collapse' data-target='#demo'> ดาวน์โหลดคู่มือ <i class='fa fa-download'></i></button>";
                //bodylist += "</div>";



                var file = _context.FileManals.Where(c => c.ManualId == std.ManualId).ToList();
                foreach (var stf in file as IList<FileManal>)
                {

                    ////bodylist += "<div id='demo' class='collapse'>";
                    //bodylist += " <a href='/File/Manual/" + stf.FileManalName + "'  class='btn btn-primary' data-toggle='tooltip' data-placement='top' title='" + stf.FileManalName + "'><span class='fa fa-book'></span></a> ";
                    ////bodylist += "</div>";
                   var type = stf.FileManalType;
                    if (type == ".pdf")
                    {
                        bodylist += " <a href='/File/Manual/" + stf.FileManalName + "' data-toggle='tooltip' data-placement='top' title='" + stf.FileManalName + "'><img src='/Images/pdf.png' width='25px'></span></a> ";
                    }
                    if (type == ".xlsx")
                    {
                        bodylist += " <a href='/File/Manual/" + stf.FileManalName + "' data-toggle='tooltip' data-placement='top' title='" + stf.FileManalName + "'><img src='/Images/xls.png' width='25px'></span></a> ";
                    }
                    if (type == ".pptx")
                    {
                        bodylist += " <a href='/File/Manual/" + stf.FileManalName + "' data-toggle='tooltip' data-placement='top' title='" + stf.FileManalName + "'><img src='/Images/ppt.png' width='25px'></span></a> ";
                    }
                    if (type == ".docx")
                    {
                        bodylist += " <a href='/File/Manual/" + stf.FileManalName + "' data-toggle='tooltip' data-placement='top' title='" + stf.FileManalName + "'><img src='/Images/doc.png' width='25px'></span></a> ";
                    }


                }
            bodylist += "</div>";
                bodylist += "<div class='col-lg-12'><hr></div>";
               
            }

            var ListMannual = bodylist;

            response = Ok(new
            {
                ListMannual = ListMannual

            });
            return response;
        }


        public IActionResult search(/*string manuals*/)
        {
            char[] charsToTrim = { '*', ' ', '\'' };
            string manualname = HttpContext.Request.Query["manualname"].ToString().Trim(charsToTrim);

            var manuals = _context.Manuals
                .Include(c => c.ManualCats).ToList();

            if (manualname != "")
            {
             
                manuals = manuals.Where(c => c.ManualName.Contains(manualname)).ToList();
            }

            var bodylist = "";

            foreach (var std in manuals as IList<Manual>)
            {
                //bodylist += "<div class='col-lg-12'>";
                bodylist += "<div class='col-lg-8'>";
                bodylist += "<a style='color:#1c79c0;font-size: 15px;' href='../Manuals/Details/" + std.ManualId + "'>" + std.ManualCats.ManualCatName + " - " + std.ManualName + "</a>";
                bodylist += "<p id='textover'>" + std.ManuaDetail + "</p>  ";
                bodylist += "<p class='fcolor'>โดย : " + std.ManualUser + " เมื่อวันที่ : " + std.ManualDate.ToString("yyyy-MM-dd") + "</p>";
                bodylist += "</div>";
                bodylist += "<div class='col-lg-4'>";
                bodylist += "<p><b>ดาวโหลดไฟล์คู่มือ <i class='fa fa-download'></i> </b></p>";
                //bodylist += "<button class='btn' type='button'data-toggle='collapse' data-target='#demo'> ดาวน์โหลดคู่มือ <i class='fa fa-download'></i></button>";
                //bodylist += "</div>";



                var file = _context.FileManals.Where(c => c.ManualId == std.ManualId).ToList();
                foreach (var stf in file as IList<FileManal>)
                {
                    var type = stf.FileManalType;
                    if (type == ".pdf")
                    {
                        bodylist += " <a href='/File/Manual/" + stf.FileManalName + "' data-toggle='tooltip' data-placement='top' title='" + stf.FileManalName + "'><img src='/Images/pdf.png' width='25px'></span></a> ";
                    }
                    if (type == ".xlsx")
                    {
                        bodylist += " <a href='/File/Manual/" + stf.FileManalName + "' data-toggle='tooltip' data-placement='top' title='" + stf.FileManalName + "'><img src='/Images/xls.png' width='25px'></span></a> ";
                    }
                    if (type == ".pptx")
                    {
                        bodylist += " <a href='/File/Manual/" + stf.FileManalName + "' data-toggle='tooltip' data-placement='top' title='" + stf.FileManalName + "'><img src='/Images/ppt.png' width='25px'></span></a> ";
                    }
                    if (type == ".docx")
                    {
                        bodylist += " <a href='/File/Manual/" + stf.FileManalName + "' data-toggle='tooltip' data-placement='top' title='" + stf.FileManalName + "'><img src='/Images/doc.png' width='25px'></span></a> ";
                    }


                }
                bodylist += "</div>";
                bodylist += "<div class='col-lg-12'><hr></div>";

            }

            var ListMannual = bodylist;

            return Ok(new { ListMannual = ListMannual});
        }
    }
}