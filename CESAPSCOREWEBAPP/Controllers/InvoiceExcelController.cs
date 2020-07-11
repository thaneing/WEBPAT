using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using CESAPSCOREWEBAPP.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using OfficeOpenXml;

namespace CESAPSCOREWEBAPP.Controllers
{
    [Authorize]
    public class InvoiceExcelController : Controller
    {
        private readonly DatabaseContext _context;
        public InvoiceExcelController(DatabaseContext context)
        {
            _context = context;
        }
        public IActionResult Index()
        {

            return View();
        }




        public IActionResult Gendata()
        {
            IActionResult response = Unauthorized();
            var check = _context.InvoiceExcelTMPs.ToList();

            response = Ok(new { data = check });
            return response;
        }




        [HttpPost]
        public async Task<IActionResult> Index(IFormFile formFile)
        {
            IActionResult response = Unauthorized();
    

            var list = new List<InvoiceExcelTMP>();
            var DocumentReceivables = new List<DocumentReceivable>();
           
            var EtcTax = "";
            var CusNo = "";
            var Site = "";
            DateTime PostingDate;
            var CusName = "";
            var InvoiceId = "";
            var Detail = "";
            var Period = "";
            decimal TotalExcVat = 0;
            decimal  TotalCut = 0;
            decimal RetentionTotal = 0;
            decimal AdvanceTotal = 0;
            decimal TotalConstruction = 0;
            decimal IncomeExtra = 0;
            decimal IncomeMaterial = 0;
            decimal IncomeEtc = 0;
            decimal IncomeAdvanceExtra = 0;
            decimal Vat7 = 0;
            decimal CustInVat = 0;
            decimal Vat3 = 0;
            decimal TotalIncome = 0;
            DateTime? DueDate;
            var Ref = "";
            decimal IncomeTotalBank = 0;
            var Bankname = "";
            var ChqNo = "";
            decimal InvoiceTotal = 0l;
            decimal PayTotal = 0;
            decimal BankTotal =0;

            using (var stream = new MemoryStream())
            {
                await formFile.CopyToAsync(stream);

                using (var package = new ExcelPackage(stream))
                {
                    ExcelWorksheet worksheet = package.Workbook.Worksheets[1];
                    var rowCount = worksheet.Dimension.Rows;
                   
                    for (int row = 2; row <= rowCount-1; row++)
                    {
                      

                        try 
                        {
                            EtcTax = EncodeUTF8(worksheet.Cells[row, 1].Value.ToString());
                        }
                        catch
                        {
                            EtcTax = "";
                        }

                        try
                        {
                            CusNo = EncodeUTF8(worksheet.Cells[row, 2].Value.ToString().Trim());
                        }
                        catch
                        {
                            CusNo = "";
                        }

                        try
                        {
                            Site = EncodeUTF8(worksheet.Cells[row, 3].Value.ToString().Trim());
                        }
                        catch
                        {
                            Site = "";
                        }
                        try
                        {
                            PostingDate = DateTime.FromOADate(Convert.ToDouble(EncodeUTF8(worksheet.Cells[row, 4].Value.ToString().Trim())));
                        }
                        catch
                        {
                            PostingDate = Convert.ToDateTime("01/01/1993");
                        }
                       

                        try
                        {
                            CusName = EncodeUTF8(worksheet.Cells[row, 5].Value.ToString().Trim());
                        }
                        catch
                        {
                            CusName = "";
                        }
                        try
                        {
                            InvoiceId = EncodeUTF8(worksheet.Cells[row, 6].Value.ToString().Trim());
                        }
                        catch
                        {
                            InvoiceId = "";
                        }
                        try
                        {
                            Detail = EncodeUTF8(worksheet.Cells[row, 8].Value.ToString().Trim());
                        }
                        catch
                        {
                            Detail = "";
                        }
                        try
                        {
                            Period = EncodeUTF8(worksheet.Cells[row, 9].Value.ToString().Trim());
                        }
                        catch
                        {
                            Period = "";
                        }
                        try
                        {
                            TotalExcVat = Decimal.Parse(EncodeUTF8(worksheet.Cells[row, 10].Value.ToString().Trim()));
                        }
                        catch
                        {
                            TotalExcVat = 0;
                        }
                        try
                        {
                            TotalCut = Decimal.Parse(EncodeUTF8(worksheet.Cells[row, 11].Value.ToString().Trim()));
                        }
                        catch
                        {
                            TotalCut = 0;
                        }
                        try
                        {
                            RetentionTotal = Decimal.Parse(EncodeUTF8(worksheet.Cells[row, 12].Value.ToString().Trim()));
                        }
                        catch
                        {
                            RetentionTotal = 0;
                        }
                        try
                        {
                            AdvanceTotal = Decimal.Parse(EncodeUTF8(worksheet.Cells[row, 13].Value.ToString().Trim()));
                        }
                        catch
                        {
                            AdvanceTotal = 0;
                        }
                        try
                        {
                            TotalConstruction = Decimal.Parse(EncodeUTF8(worksheet.Cells[row, 14].Value.ToString().Trim()));
                        }
                        catch
                        {
                            TotalConstruction = 0;
                        }
                        try
                        {
                            IncomeExtra = Decimal.Parse(EncodeUTF8(worksheet.Cells[row, 15].Value.ToString().Trim()));
                        }
                        catch
                        {
                            IncomeExtra = 0;
                        }
                        try
                        {
                            IncomeMaterial = Decimal.Parse(EncodeUTF8(worksheet.Cells[row, 16].Value.ToString().Trim()));
                        }
                        catch
                        {
                            IncomeMaterial = 0;
                        }
                        try
                        {
                            IncomeEtc = Decimal.Parse(EncodeUTF8(worksheet.Cells[row, 17].Value.ToString().Trim()));
                        }
                        catch
                        {
                            IncomeEtc = 0;
                        }
                        try
                        {
                            IncomeAdvanceExtra = Decimal.Parse(EncodeUTF8(worksheet.Cells[row, 18].Value.ToString().Trim()));
                        }
                        catch
                        {
                            IncomeAdvanceExtra = 0;
                        }
                        try
                        {
                            Vat7 = Decimal.Parse(EncodeUTF8(worksheet.Cells[row, 19].Value.ToString().Trim()));
                        }
                        catch
                        {
                            Vat7 = 0;
                        }
                        try
                        {
                            CustInVat = Decimal.Parse(EncodeUTF8(worksheet.Cells[row, 20].Value.ToString().Trim()));
                        }
                        catch
                        {
                            CustInVat = 0;
                        }
                        try
                        {
                            Vat3 = Decimal.Parse(EncodeUTF8(worksheet.Cells[row, 21].Value.ToString().Trim()));
                        }
                        catch
                        {
                            Vat3 = 0;
                        }
                        try
                        {
                            TotalIncome = Decimal.Parse(EncodeUTF8(worksheet.Cells[row, 22].Value.ToString().Trim()));
                        }
                        catch
                        {
                            TotalIncome = 0;
                        }
                        try
                        {
                            DueDate = DateTime.FromOADate(Convert.ToDouble(EncodeUTF8(worksheet.Cells[row, 23].Value.ToString().Trim())));
                            //DueDate = Convert.ToDateTime(EncodeUTF8(worksheet.Cells[row, 23].Value.ToString().Trim()));
                        }
                        catch
                        {
                            DueDate = Convert.ToDateTime("01/01/1993");
                        }
                        try
                        {
                            Ref = EncodeUTF8(worksheet.Cells[row, 24].Value.ToString().Trim());
                        }
                        catch
                        {
                            Ref = "";
                        }
                        try
                        {
                            IncomeTotalBank = Decimal.Parse(EncodeUTF8(worksheet.Cells[row, 25].Value.ToString().Trim()));
                        }
                        catch
                        {
                            IncomeTotalBank = 0;
                        }
                        try
                        {
                            Bankname = EncodeUTF8(worksheet.Cells[row, 26].Value.ToString().Trim());
                        }
                        catch
                        {
                            Bankname = "";
                        }
                        try
                        {
                            ChqNo = EncodeUTF8(worksheet.Cells[row, 27].Value.ToString().Trim());
                        }
                        catch
                        {
                            ChqNo = "";
                        }
                        try
                        {
                            InvoiceTotal = Decimal.Parse(EncodeUTF8(worksheet.Cells[row, 28].Value.ToString().Trim()));
                        }
                        catch
                        {
                            InvoiceTotal = 0;
                        }
                        try
                        {
                            PayTotal = Decimal.Parse(EncodeUTF8(worksheet.Cells[row, 29].Value.ToString().Trim()));
                        }
                        catch
                        {
                            PayTotal = 0;
                        }
                        try
                        {
                            BankTotal = Decimal.Parse(EncodeUTF8(worksheet.Cells[row, 30].Value.ToString().Trim()));
                        }
                        catch
                        {
                            BankTotal = 0;
                        }

                        var check = _context.InvoiceExcelTMPs.Where(c => c.InvoiceId == InvoiceId).ToList();

                        if (check.Count == 0) 
                        {
                            var Invoicedata=_context.InvoiceExcelTMPs.Where(p => p.InvoiceId == InvoiceId).ToList();

                            if (Invoicedata.Count <= 0) {
                                list.Add(new InvoiceExcelTMP
                                {
                                    EtcTax = EtcTax,
                                    CusNo = CusNo,
                                    Site = Site,
                                    PostingDate = PostingDate,
                                    CusName = CusName,
                                    InvoiceId = InvoiceId,
                                    Detail = Detail,
                                    Period = Period,
                                    TotalExcVat = TotalExcVat,
                                    TotalCut = TotalCut,
                                    RetentionTotal =RetentionTotal,
                                    AdvanceTotal =AdvanceTotal, 
                                    TotalConstruction =TotalConstruction,
                                    IncomeExtra = IncomeExtra,
                                    IncomeMaterial = IncomeMaterial,
                                    IncomeEtc = IncomeEtc,
                                    IncomeAdvanceExtra = IncomeAdvanceExtra,
                                    Vat7 = Vat7,
                                    CustInVat = CustInVat,
                                    Vat3 = Vat3,
                                    TotalIncome = TotalIncome,
                                    DueDate = DueDate,
                                    Ref = Ref,
                                    IncomeTotalBank = IncomeTotalBank,
                                    Bankname = Bankname,
                                    ChqNo = ChqNo,
                                    InvoiceTotal = InvoiceTotal,
                                    PayTotal = PayTotal,
                                    BankTotal = BankTotal,
                                    TypeOfPay = "0",
                                    ImportBy = HttpContext.Session.GetString("Username"),
                                    ImportDate = DateTime.Now

                                }) ;
                            }

                            var DocumentData = _context.DocumentReceivables.Where(p => p.DocNo == InvoiceId).ToList();

                            if (DocumentData.Count <= 0) { 

                            DocumentReceivables.Add(new DocumentReceivable
                            {
                                PostingDate= PostingDate,
                                DocNo= InvoiceId,
                                JobNo=Site,
                                Detail=Detail,
                                Outstanding= InvoiceTotal,
                                Amount=BankTotal,
                                TotalNAV=0.00M,
                                Statuss=DocumentReceivable.Status.Open,
                                CreateBy = HttpContext.Session.GetString("Username"),
                                CreateDate = DateTime.Now
                            });
                            }
                        }


                    }
                }
            }

            _context.DocumentReceivables.AddRange(DocumentReceivables);
            _context.InvoiceExcelTMPs.AddRange(list);
            await _context.SaveChangesAsync();

            //response = Ok(new { data = list });
            return RedirectToAction("Index", "AccountsReceivables");
        }

        public static string EncodeUTF8(string strPlanText)
        {
            UTF8Encoding utf8 = new UTF8Encoding();
            string unicodeString = strPlanText;
            byte[] encodedBytes = utf8.GetBytes(unicodeString);
            return Encoding.UTF8.GetString(encodedBytes);
        }
    }
}