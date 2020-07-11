using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using CESAPSCOREWEBAPP.Models;
using Microsoft.AspNetCore.Http;
using System.Globalization;
using Microsoft.AspNetCore.Authorization;

namespace CESAPSCOREWEBAPP.Controllers
{
    [Authorize]
    public class AccountsReceivablesController : Controller
    {
        private readonly NAVContext _navcontext;
        private readonly DatabaseContext _context;

        public AccountsReceivablesController(NAVContext navcontext,DatabaseContext context)
        {
            _navcontext = navcontext;
            _context = context;
        }

        // GET: AccountsReceivables
        public IActionResult Index()
        {
            ViewBag.group = _context.RegisGroupJobs.ToList();

            var queryData1 = "SELECT dbo." + Environment.GetEnvironmentVariable("Company") + "Location].Code AS name,dbo." + Environment.GetEnvironmentVariable("Company") + "Location].Name as code FROM [dbo]." + Environment.GetEnvironmentVariable("Company") + "Location] order by dbo." + Environment.GetEnvironmentVariable("Company") + "Location].Code asc ";
            var sourceAutoCompletes = _navcontext.sourceAutoCompletes.FromSqlRaw(queryData1).ToList();
            ViewData["SourceAutoCompletes"] = sourceAutoCompletes;

            ViewBag.StartDate = DateTime.Now.ToString("dd-MM-yyyy", new CultureInfo("en-US"));

            return View();
        }


        public IActionResult Gendata()
        {
            IActionResult response = Unauthorized();
            var queryData = "SELECT " +
                "ROW_NUMBER() OVER (ORDER BY a.Doc) AS ID," +
                "(select top 1 convert(varchar,[Posting Date],23) FROM dbo."+ Environment.GetEnvironmentVariable("Company") +"Cust_ Ledger Entry] where [Document No_] LIKE '%'+a.Doc+'%') as PostingDate," +
                "a.Doc," +
                "(select top 1 [Global Dimension 1 Code] FROM dbo."+ Environment.GetEnvironmentVariable("Company") +"Cust_ Ledger Entry] where [Document No_] LIKE '%'+a.Doc+'%') as JobNo, " +
                "(select top 1  Description FROM dbo."+ Environment.GetEnvironmentVariable("Company") +"Cust_ Ledger Entry] where [Document No_] LIKE '%'+a.Doc+'%') as Description, " +
                "(select top 1  [Customer Posting Group] FROM dbo."+ Environment.GetEnvironmentVariable("Company") +"Cust_ Ledger Entry] where [Document No_] LIKE '%'+a.Doc+'%') as CustomerGroup, " +
                "((select isnull(SUM([Sales (LCY)]),0)*107/100 FROM dbo."+ Environment.GetEnvironmentVariable("Company") +"Cust_ Ledger Entry] where [Document No_] LIKE a.Doc)+ (select isnull(sum([Sales (LCY)]),0) FROM dbo."+ Environment.GetEnvironmentVariable("Company") +"Cust_ Ledger Entry] where [Document No_] LIKE a.Doc+'/R%')*7/100) as SumTotal," +
                "(((select isnull(SUM([Sales (LCY)]),0)*107/100 FROM dbo."+ Environment.GetEnvironmentVariable("Company") +"Cust_ Ledger Entry] where [Document No_] LIKE a.Doc)+ (select isnull(sum([Sales (LCY)]),0) FROM dbo."+ Environment.GetEnvironmentVariable("Company") +"Cust_ Ledger Entry] where [Document No_] LIKE a.Doc+'/R%')*7/100) " +
                "-(((select isnull(SUM([Sales (LCY)]),0) FROM dbo."+ Environment.GetEnvironmentVariable("Company") +"Cust_ Ledger Entry] where [Document No_] LIKE a.Doc) +  " +
                "(select isnull(sum([Sales (LCY)]),0) FROM dbo."+ Environment.GetEnvironmentVariable("Company") +"Cust_ Ledger Entry] where [Document No_] LIKE a.Doc+'/R%'))*3/100)) as SumTotalVat, " +
                "(((select isnull(SUM([Sales (LCY)]),0) FROM dbo."+ Environment.GetEnvironmentVariable("Company") +"Cust_ Ledger Entry] where [Document No_] LIKE a.Doc) +  " +
                "(select isnull(sum([Sales (LCY)]),0) FROM dbo."+ Environment.GetEnvironmentVariable("Company") +"Cust_ Ledger Entry] where [Document No_] LIKE a.Doc+'/R%'))*107/100) as TotalNAV, " +
                "(select isnull(sum([Sales (LCY)]),0) FROM dbo."+ Environment.GetEnvironmentVariable("Company") +"Cust_ Ledger Entry] where [Document No_] LIKE a.Doc+'/R%') as Retention " +

                "FROM( " +
                "SELECT " +
                "dbo."+ Environment.GetEnvironmentVariable("Company") +"Cust_ Ledger Entry].[Document No_] as Doc " +
                "FROM " +
                "dbo."+ Environment.GetEnvironmentVariable("Company") +"Cust_ Ledger Entry] " +
                "WHERE [Open]=1 and [Document Type]=2 and [Document No_] Not Like '%/R%' and [Customer No_] Not Like '%R%' AND [Customer Posting Group]<>'003' " +
                "GROUP BY  " +
                "dbo."+ Environment.GetEnvironmentVariable("Company") +"Cust_ Ledger Entry].[Document No_]) as a order by PostingDate";

            var AccountsReceivable = _navcontext.AccountsReceivables.FromSqlRaw(queryData).ToList();
            AccountsReceivable AccountsRec;

            var DocumentReceivables = _context.DocumentReceivables.Where(p=>p.Statuss==0).ToList();
            foreach(var std in DocumentReceivables)
            {
                AccountsRec = new AccountsReceivable();
                AccountsRec.PostingDate = std.PostingDate.ToString("yyyy-MM-dd");
                AccountsRec.Doc = std.DocNo;
                AccountsRec.JobNo = std.JobNo;
                AccountsRec.Description = std.Detail;
                AccountsRec.CustomerGroup = "";
                AccountsRec.SumTotal = std.Outstanding;
                AccountsRec.SumTotalVat = std.Amount;
                AccountsRec.TotalNAV = std.Amount;
                AccountsRec.TotalNAV = 0.0M;

                AccountsReceivable.Add(AccountsRec);
            }
            response = Ok(new { data = AccountsReceivable });


            return response;
        }


        public async Task<IActionResult> GendataR()
        {
            IActionResult response = Unauthorized();


            var queryData = "SELECT " +
                "ROW_NUMBER() OVER (ORDER BY a.Doc) AS ID," +
                "(select top 1 convert(varchar,[Posting Date],23) FROM dbo."+ Environment.GetEnvironmentVariable("Company") +"Cust_ Ledger Entry] where [Document No_] LIKE '%'+a.Doc+'%') as PostingDate," +
                "(select top 1  dbo."+ Environment.GetEnvironmentVariable("Company") +"Cust_ Ledger Entry].[External Document No_] FROM dbo."+ Environment.GetEnvironmentVariable("Company") +"Cust_ Ledger Entry] where [Document No_] LIKE '%'+a.Doc+'%') as Doc, " +
                "(select top 1 [Global Dimension 1 Code] FROM dbo."+ Environment.GetEnvironmentVariable("Company") +"Cust_ Ledger Entry] where [Document No_] LIKE '%'+a.Doc+'%') as JobNo, " +
                "(select top 1  Description FROM dbo."+ Environment.GetEnvironmentVariable("Company") +"Cust_ Ledger Entry] where [Document No_] LIKE '%'+a.Doc+'%') as Description, " +
                "(select top 1  [Customer Posting Group] FROM dbo."+ Environment.GetEnvironmentVariable("Company") +"Cust_ Ledger Entry] where [Document No_] LIKE '%'+a.Doc+'%') as CustomerGroup, " +
                "CONVERT(DECIMAL(16,4),0.00)  as SumTotal," +
                "CONVERT(DECIMAL(16,4),0.00) as SumTotalVat, " +
                "CONVERT(DECIMAL(16,4),0.00) as TotalNAV, " +
                "(select isnull(SUM([Sales (LCY)]),0) FROM dbo."+ Environment.GetEnvironmentVariable("Company") +"Cust_ Ledger Entry] where [Document No_] LIKE a.Doc)+(select isnull(sum([Sales (LCY)]),0) FROM dbo."+ Environment.GetEnvironmentVariable("Company") +"Cust_ Ledger Entry] where [Document No_] LIKE a.Doc+'/R%') as Retention " +

                "FROM( " +
                "SELECT " +
                "dbo."+ Environment.GetEnvironmentVariable("Company") +"Cust_ Ledger Entry].[Document No_] as Doc " +
                "FROM " +
                "dbo."+ Environment.GetEnvironmentVariable("Company") +"Cust_ Ledger Entry] " +
                "WHERE [Open]=1 and [Document Type]=2 and [Document No_] Not Like '%/R%' and [Customer No_] Like '%R%' AND [Customer Posting Group]<>'003' " +
                "GROUP BY  " +
                "dbo."+ Environment.GetEnvironmentVariable("Company") +"Cust_ Ledger Entry].[Document No_]) as a order by Description";

            var AccountsReceivable =  await _navcontext.AccountsReceivables.FromSqlRaw(queryData).ToListAsync();

           
            //var acc = new List<AccountsReceivable>();
            List<Tmp_AccGroup> instances = new List<Tmp_AccGroup>();
            Tmp_AccGroup tmp = null;
            GroupJob group;
            var name = "";
            RegisGroupJob RegisGroupJobs;
           //acc = AccountsReceivable.ToList();


            foreach (var std in AccountsReceivable)
            {
              //group = new GroupJob();
              group = await _context.GroupJobs.Where(p => p.DocumentNo == std.Doc).FirstOrDefaultAsync();
                if (group == null) {
                    name = "";
                }
                else
                {
                    RegisGroupJobs= await _context.RegisGroupJobs.Where(p=>p.GroupCode==group.GroupNo).FirstOrDefaultAsync();
                    if (RegisGroupJobs == null)
                    {
                        name = "";
                    }
                    else
                    {
                        name = RegisGroupJobs.GroupName;
                    }
                   
                }
            

                tmp = new Tmp_AccGroup();
                tmp.Doc = std.Doc;
                tmp.Description = std.Description;
                tmp.PostingDate = std.PostingDate;
                tmp.JobNo = std.JobNo;
                tmp.CustomerGroup = std.CustomerGroup;
                tmp.SumTotal = std.SumTotal;
                tmp.SumTotalVat = std.SumTotalVat;
                tmp.TotalNAV = std.TotalNAV;
                tmp.Retention = std.Retention;
                tmp.GroupName = name;

                
                instances.Add(tmp);
            }

            response = Ok(new { data = instances});


            return response;
        }


 

        public IActionResult GendataShirt()
        {
            IActionResult response = Unauthorized();
            var queryData = "SELECT " +
                "ROW_NUMBER() OVER (ORDER BY a.Doc) AS ID," +
                "(select top 1 convert(varchar,[Posting Date],23) FROM dbo."+ Environment.GetEnvironmentVariable("Company") +"Cust_ Ledger Entry] where [External Document No_] LIKE '%'+a.Doc+'%') as PostingDate," +
                "a.Doc," +
                "(select top 1 [Global Dimension 1 Code] FROM dbo."+ Environment.GetEnvironmentVariable("Company") +"Cust_ Ledger Entry] where [External Document No_] LIKE '%'+a.Doc+'%') as JobNo, " +
                "(select top 1  Description FROM dbo."+ Environment.GetEnvironmentVariable("Company") +"Cust_ Ledger Entry] where [External Document No_] LIKE '%'+a.Doc+'%') as Description,  " +
                "(select top 1  [Customer Posting Group] FROM dbo."+ Environment.GetEnvironmentVariable("Company") +"Cust_ Ledger Entry] where [External Document No_] LIKE '%'+a.Doc+'%') as CustomerGroup,  " +
                "0.00  as SumTotal," +
                "0.00 as SumTotalVat, " +
                "0.00 as TotalNAV, " +
                "(((select isnull(SUM([Sales (LCY)]),0) FROM dbo."+ Environment.GetEnvironmentVariable("Company") +"Cust_ Ledger Entry] where [External Document No_] LIKE a.Doc)+(select isnull(sum([Sales (LCY)]),0) FROM dbo."+ Environment.GetEnvironmentVariable("Company") +"Cust_ Ledger Entry] where [External Document No_] LIKE a.Doc+'/R%'))*107/100) as Retention " +

                "FROM( " +
                "SELECT " +
                "dbo."+ Environment.GetEnvironmentVariable("Company") +"Cust_ Ledger Entry].[External Document No_] as Doc " +
                "FROM " +
                "dbo."+ Environment.GetEnvironmentVariable("Company") +"Cust_ Ledger Entry] " +
                "WHERE [Open]=1 and [Document Type]=2 and [External Document No_] Like '%ข%'   AND [Customer Posting Group]='003' " +
                "GROUP BY  " +
                "dbo."+ Environment.GetEnvironmentVariable("Company") +"Cust_ Ledger Entry].[External Document No_]) as a order by PostingDate";

            var AccountsReceivable = _navcontext.AccountsReceivables.FromSqlRaw(queryData).ToList();


            response = Ok(new { data = AccountsReceivable });


            return response;
        }



        public IActionResult GendataRetention(string doc)
        {
            IActionResult response = Unauthorized();
            var check = 0;
            var groupjob = _context.GroupJobs.Where(p => p.DocumentNo == doc).ToList();
            if(groupjob.Count>0)
            {

                check = 1;
                response = Ok(new { data = groupjob, check=check });
            }
            else { 

                var queryData = "SELECT " +
                    "ROW_NUMBER() OVER (ORDER BY a.Doc) AS ID," +
                    "(select top 1 convert(varchar,[Posting Date],23) FROM dbo."+ Environment.GetEnvironmentVariable("Company") +"Cust_ Ledger Entry] where [Document No_] LIKE '%'+a.Doc+'%') as PostingDate," +
                    "(select top 1  dbo."+ Environment.GetEnvironmentVariable("Company") +"Cust_ Ledger Entry].[External Document No_] FROM dbo."+ Environment.GetEnvironmentVariable("Company") +"Cust_ Ledger Entry] where [Document No_] LIKE '%'+a.Doc+'%') as Doc, " +
                    "(select top 1 [Global Dimension 1 Code] FROM dbo."+ Environment.GetEnvironmentVariable("Company") +"Cust_ Ledger Entry] where [Document No_] LIKE '%'+a.Doc+'%') as JobNo, " +
                    "(select top 1  Description FROM dbo."+ Environment.GetEnvironmentVariable("Company") +"Cust_ Ledger Entry] where [Document No_] LIKE '%'+a.Doc+'%') as Description, " +
                    "(select top 1  [Customer Posting Group] FROM dbo."+ Environment.GetEnvironmentVariable("Company") +"Cust_ Ledger Entry] where [Document No_] LIKE '%'+a.Doc+'%') as CustomerGroup, " +
                    "0.00  as SumTotal," +
                    "0.00 as SumTotalVat, " +
                    "0.00 as TotalNAV, " +
                    "(select isnull(SUM([Sales (LCY)]),0) FROM dbo."+ Environment.GetEnvironmentVariable("Company") +"Cust_ Ledger Entry] where [Document No_] LIKE a.Doc)+(select isnull(sum([Sales (LCY)]),0) FROM dbo."+ Environment.GetEnvironmentVariable("Company") +"Cust_ Ledger Entry] where [Document No_] LIKE a.Doc+'/R%') as Retention " +

                    "FROM( " +
                    "SELECT " +
                    "dbo."+ Environment.GetEnvironmentVariable("Company") +"Cust_ Ledger Entry].[Document No_] as Doc " +
                    "FROM " +
                    "dbo."+ Environment.GetEnvironmentVariable("Company") +"Cust_ Ledger Entry] " +
                    "WHERE [Open]=1 and [Document Type]=2 and [Document No_] Not Like '%/R%' and [Customer No_] Like '%R%' AND [Customer Posting Group]<>'003' " +
                    "GROUP BY  " +
                    "dbo."+ Environment.GetEnvironmentVariable("Company") +"Cust_ Ledger Entry].[Document No_]) as a order by Description";

                var AccountsReceivable = _navcontext.AccountsReceivables.FromSqlRaw(queryData).ToList();

                var dataall =AccountsReceivable.Where(p => p.Doc == doc).ToList();


                    response = Ok(new { data = dataall ,check=check});
            }

            return response;
        }



        public IActionResult SaveGroup(string date,string doc, string job, string amount, string group, string groupdetail)
        {
            IActionResult response = Unauthorized();
            var check = 0;
            var groupjob = _context.GroupJobs.Where(p => p.DocumentNo == doc).FirstOrDefault();
            if (groupjob != null)
            {
                groupjob.GroupNo = group;
                groupjob.GroupName = groupdetail;
                groupjob.EditDate = DateTime.Now;
                groupjob.EditBy= HttpContext.Session.GetString("Username");
                _context.Update(groupjob);
                _context.SaveChanges();
            }
            else
            {

                var queryData = "SELECT " +
                    "ROW_NUMBER() OVER (ORDER BY a.Doc) AS ID," +
                    "(select top 1 convert(varchar,[Posting Date],23) FROM dbo."+ Environment.GetEnvironmentVariable("Company") +"Cust_ Ledger Entry] where [Document No_] LIKE '%'+a.Doc+'%') as PostingDate," +
                    "(select top 1  dbo."+ Environment.GetEnvironmentVariable("Company") +"Cust_ Ledger Entry].[External Document No_] FROM dbo."+ Environment.GetEnvironmentVariable("Company") +"Cust_ Ledger Entry] where [Document No_] LIKE '%'+a.Doc+'%') as Doc, " +
                    "(select top 1 [Global Dimension 1 Code] FROM dbo."+ Environment.GetEnvironmentVariable("Company") +"Cust_ Ledger Entry] where [Document No_] LIKE '%'+a.Doc+'%') as JobNo, " +
                    "(select top 1  Description FROM dbo."+ Environment.GetEnvironmentVariable("Company") +"Cust_ Ledger Entry] where [Document No_] LIKE '%'+a.Doc+'%') as Description, " +
                    "(select top 1  [Customer Posting Group] FROM dbo."+ Environment.GetEnvironmentVariable("Company") +"Cust_ Ledger Entry] where [Document No_] LIKE '%'+a.Doc+'%') as CustomerGroup, " +
                    "0.00  as SumTotal," +
                    "0.00 as SumTotalVat, " +
                    "0.00 as TotalNAV, " +
                    "(select isnull(SUM([Sales (LCY)]),0) FROM dbo."+ Environment.GetEnvironmentVariable("Company") +"Cust_ Ledger Entry] where [Document No_] LIKE a.Doc)+(select isnull(sum([Sales (LCY)]),0) FROM dbo."+ Environment.GetEnvironmentVariable("Company") +"Cust_ Ledger Entry] where [Document No_] LIKE a.Doc+'/R%') as Retention " +

                    "FROM( " +
                    "SELECT " +
                    "dbo."+ Environment.GetEnvironmentVariable("Company") +"Cust_ Ledger Entry].[Document No_] as Doc " +
                    "FROM " +
                    "dbo."+ Environment.GetEnvironmentVariable("Company") +"Cust_ Ledger Entry] " +
                    "WHERE [Open]=1 and [Document Type]=2 and [Document No_] Not Like '%/R%' and [Customer No_] Like '%R%' AND [Customer Posting Group]<>'003' " +
                    "GROUP BY  " +
                    "dbo."+ Environment.GetEnvironmentVariable("Company") +"Cust_ Ledger Entry].[Document No_]) as a order by Description";

                var AccountsReceivable = _navcontext.AccountsReceivables.FromSqlRaw(queryData).ToList();

              
                var std = AccountsReceivable.Where(p => p.Doc == doc).FirstOrDefault();
                GroupJob groupJobdata = new GroupJob();
                groupJobdata.DocumentNo = std.Doc;
                groupJobdata.JobNo = std.JobNo;
                groupJobdata.Amount = std.Retention;
                groupJobdata.Description = std.Description;
                groupJobdata.CreateBy = HttpContext.Session.GetString("Username");
                groupJobdata.CreateDate = DateTime.Now;
                groupJobdata.GroupNo = group;
                groupJobdata.PostingDate = std.PostingDate;
                groupJobdata.GroupName = groupdetail;
                _context.Add(groupJobdata);
                _context.SaveChanges();



                response = Ok(new { data = std, check = check });
            }

            return response;
        }




        public IActionResult AutoComplete()
        {
            IActionResult response = Unauthorized();

            var std = _context.GroupJobs
                .GroupBy(g => 
                new { g.GroupNo,
                    g.GroupName 
                }).OrderBy(o => o.Key.GroupNo)
                .Select(s => new SourceAutoComplete() 
                { 
                    code=s.Key.GroupNo,
                    name=s.Key.GroupNo 
                }
               );

           response = Ok(new { data = std});
          

            return response;
        }


        //Register Group
        public IActionResult GetdataGroup()
        {
            IActionResult response = Unauthorized();

            var regisGroupJob = new List<RegisGroupJob>();
            regisGroupJob = _context.RegisGroupJobs.ToList();

            response = Ok(new { data = regisGroupJob });
            return response;
        }

        [HttpPost, ActionName("AddGroup")]
        public IActionResult AddGroup(string group,string code,string des,string site)
        {


            RegisGroupJob regisGroupJob = new RegisGroupJob();

            regisGroupJob.GroupName = group;
            regisGroupJob.GroupCode = code;
            regisGroupJob.GroupDes = des;
            regisGroupJob.GroupSite = site;
            regisGroupJob.RegisBy = HttpContext.Session.GetString("Username");
            regisGroupJob.RegisDate = DateTime.Now;

            _context.RegisGroupJobs.Add(regisGroupJob);
            _context.SaveChanges();
            return Ok(regisGroupJob);
        }

        public IActionResult getGroup(int id)
        {
            IActionResult response = Unauthorized();
            var regisGroupJob = _context.RegisGroupJobs.FirstOrDefault(m => m.ID == id);
            var name = regisGroupJob.GroupName;
            var ID = regisGroupJob.ID;
            var code = regisGroupJob.GroupCode;
            var site = regisGroupJob.GroupSite;
            var des = regisGroupJob.GroupDes;
            response = Ok(new { name = name,ID=ID,site=site,code=code,des=des });
            return response;
        }
        public IActionResult EditGroup(int id, string group, string code, string des, string site)
        {
            IActionResult response = Unauthorized();


            var regisGroupJob = _context.RegisGroupJobs.FirstOrDefault(m => m.ID == id);

           regisGroupJob.GroupName = group;
            regisGroupJob.GroupCode = code;
            regisGroupJob.GroupSite =site;
            regisGroupJob.GroupDes =des;

            _context.RegisGroupJobs.Update(regisGroupJob);
            _context.SaveChanges();

            return Ok(regisGroupJob);


        }
        [HttpPost, ActionName("remove")]

        public async Task<IActionResult> remove(int id)
        {
            
            IActionResult response = Unauthorized();
            var group = await _context.RegisGroupJobs.FindAsync(id);
            var name = group.GroupName;


            _context.RegisGroupJobs.Remove(group);
            await _context.SaveChangesAsync();

            response = Ok(new { name = name });
            return response;
        }


        public async Task<IActionResult> SaveGroupMulti(string doc,string group)
        {
            IActionResult response = Unauthorized();

            var queryData = "SELECT " +
            "ROW_NUMBER() OVER (ORDER BY a.Doc) AS ID," +
            "(select top 1 convert(varchar,[Posting Date],23) FROM dbo."+ Environment.GetEnvironmentVariable("Company") +"Cust_ Ledger Entry] where [Document No_] LIKE '%'+a.Doc+'%') as PostingDate," +
            "(select top 1  dbo."+ Environment.GetEnvironmentVariable("Company") +"Cust_ Ledger Entry].[External Document No_] FROM dbo."+ Environment.GetEnvironmentVariable("Company") +"Cust_ Ledger Entry] where [Document No_] LIKE '%'+a.Doc+'%') as Doc, " +
            "(select top 1 [Global Dimension 1 Code] FROM dbo."+ Environment.GetEnvironmentVariable("Company") +"Cust_ Ledger Entry] where [Document No_] LIKE '%'+a.Doc+'%') as JobNo, " +
            "(select top 1  Description FROM dbo."+ Environment.GetEnvironmentVariable("Company") +"Cust_ Ledger Entry] where [Document No_] LIKE '%'+a.Doc+'%') as Description, " +
            "(select top 1  [Customer Posting Group] FROM dbo."+ Environment.GetEnvironmentVariable("Company") +"Cust_ Ledger Entry] where [Document No_] LIKE '%'+a.Doc+'%') as CustomerGroup, " +
            "0.00  as SumTotal," +
            "0.00 as SumTotalVat, " +
            "0.00 as TotalNAV, " +
            "(select isnull(SUM([Sales (LCY)]),0) FROM dbo."+ Environment.GetEnvironmentVariable("Company") +"Cust_ Ledger Entry] where [Document No_] LIKE a.Doc)+(select isnull(sum([Sales (LCY)]),0) FROM dbo."+ Environment.GetEnvironmentVariable("Company") +"Cust_ Ledger Entry] where [Document No_] LIKE a.Doc+'/R%') as Retention " +

            "FROM( " +
            "SELECT " +
            "dbo."+ Environment.GetEnvironmentVariable("Company") +"Cust_ Ledger Entry].[Document No_] as Doc " +
            "FROM " +
            "dbo."+ Environment.GetEnvironmentVariable("Company") +"Cust_ Ledger Entry] " +
            "WHERE [Open]=1 and [Document Type]=2 and [Document No_] Not Like '%/R%' and [Customer No_] Like '%R%' AND [Customer Posting Group]<>'003' " +
            "GROUP BY  " +
            "dbo."+ Environment.GetEnvironmentVariable("Company") +"Cust_ Ledger Entry].[Document No_]) as a order by Description";

            var AccountsReceivables = await _navcontext.AccountsReceivables.FromSqlRaw(queryData).ToListAsync();

            var RegisGroupJobs = await _context.RegisGroupJobs.Where(p => p.GroupCode == group).FirstAsync();
            string[] codelist = doc.Split(",");
            var DocNo = "";
            List<GroupJob> groupJobs = new List<GroupJob>();
            GroupJob groupJob = new GroupJob();
            AccountsReceivable accountsReceivable = null;
            for (var i = 0; i < codelist.Length; i++)
            {
                DocNo = codelist[i];
                groupJob = await _context.GroupJobs.Where(p => p.DocumentNo == DocNo).FirstOrDefaultAsync();
                if (groupJob != null)
                {
                    groupJob.GroupNo = group;
                    groupJob.GroupName = RegisGroupJobs.GroupName;
                    groupJob.EditDate = DateTime.Now;
                    groupJob.EditBy = HttpContext.Session.GetString("Username");
                    _context.GroupJobs.Update(groupJob);
                    _context.SaveChanges();
                    groupJobs.Add(groupJob);
                }
                else
                {
                    accountsReceivable = AccountsReceivables.Where(p => p.Doc == DocNo).FirstOrDefault();
                    groupJob = new GroupJob();
                    groupJob.DocumentNo = accountsReceivable.Doc;
                    groupJob.JobNo = accountsReceivable.JobNo;
                    groupJob.Amount = accountsReceivable.Retention;
                    groupJob.Description = accountsReceivable.Description;
                    groupJob.CreateBy = HttpContext.Session.GetString("Username");
                    groupJob.CreateDate = DateTime.Now;
                    groupJob.PostingDate = accountsReceivable.PostingDate;
                    groupJob.GroupNo = group;
                    groupJob.GroupName = RegisGroupJobs.GroupName;
                    _context.GroupJobs.Add(groupJob);
                    _context.SaveChanges();
                    //groupJobs.Add(groupJob);
                }
                
            }



               
                response = Ok(new { test= groupJobs });

       
           
            return response;
        }
        public IActionResult GenDocuement()
        {
            IActionResult response = Unauthorized();

            var documentReceivable = new List<DocumentReceivable>();
            documentReceivable = _context.DocumentReceivables.ToList();

            response = Ok(new { data = documentReceivable });
            return response;
        }

        //[HttpPost, ActionName("AddDoc")]
        public IActionResult AddDoc(string date, string job, string status, string detail,decimal amount,decimal outstanding,decimal totalNAV,string doc)
        {
            IActionResult response = Unauthorized();
          
            var date1 = Convert.ToDateTime(date.Substring(6, 4) + "-" + date.Substring(3, 2) + "-" + date.Substring(0, 2) + " 00:00:00");
            DocumentReceivable documentReceivable = new DocumentReceivable();

            var statuss = Int32.Parse(status);
            documentReceivable.Detail = detail;
            documentReceivable.JobNo = job;
            documentReceivable.PostingDate = date1;
            documentReceivable.DocNo = doc;

            if(statuss == 0)
            {

                documentReceivable.Statuss = DocumentReceivable.Status.Open;
            }
            else
            {
                documentReceivable.Statuss = DocumentReceivable.Status.Close;
            }
           

            documentReceivable.Amount = amount;
            documentReceivable.Outstanding = outstanding;
            documentReceivable.TotalNAV = totalNAV;

            documentReceivable.CreateBy = HttpContext.Session.GetString("Username");
            documentReceivable.CreateDate = DateTime.Now;

            _context.DocumentReceivables.Add(documentReceivable);
            _context.SaveChanges();
            return Ok(documentReceivable);

            //response = Ok(new { data = documentReceivable });
            //return response;
        }
        public IActionResult getDoc(int id)
        {
            IActionResult response = Unauthorized();
            var documentReceivable = _context.DocumentReceivables.FirstOrDefault(m => m.ID == id);
            
            var ID = documentReceivable.ID;
            var detail = documentReceivable.Detail;
            var job = documentReceivable.JobNo;
            var amount = documentReceivable.Amount;
            var outstanding = documentReceivable.Outstanding;
            var totalNav = documentReceivable.TotalNAV;
            var postingdate = documentReceivable.PostingDate.ToString("dd/MM/yyyy");
            var status = documentReceivable.Statuss;
            var doc = documentReceivable.DocNo;

            response = Ok(new { ID=ID,detail=detail,job=job,amount=amount,outstanding=outstanding,totalNav=totalNav,postingdate=postingdate,status=status,doc=doc});
            return response;
        }

        [HttpPost, ActionName("EditDoc")]
        public IActionResult EditDoc(int id, string date, string job, int status, string detail, decimal amount, decimal outstanding, decimal totalNAV, string doc)
        {
            IActionResult response = Unauthorized();
            var date1 = Convert.ToDateTime(date.Substring(6, 4) + "-" + date.Substring(3, 2) + "-" + date.Substring(0, 2) + " 00:00:00");

            var documentReceivable = _context.DocumentReceivables.FirstOrDefault(m => m.ID == id);

            documentReceivable.Detail = detail;
            documentReceivable.JobNo = job;
            documentReceivable.PostingDate = date1;
            documentReceivable.DocNo = doc;

            if (status == 0)
            {

                documentReceivable.Statuss = DocumentReceivable.Status.Open;
            }
            else
            {
                documentReceivable.Statuss = DocumentReceivable.Status.Close;
            }


            documentReceivable.Amount = amount;
            documentReceivable.Outstanding = outstanding;
            documentReceivable.TotalNAV = totalNAV;
            documentReceivable.EditBy = HttpContext.Session.GetString("Username");
            documentReceivable.EditDate = DateTime.Now;

            _context.DocumentReceivables.Update(documentReceivable);
            _context.SaveChanges();

            return Ok(documentReceivable);


        }

       // [HttpPost, ActionName("removeDoc")]

        public async Task<IActionResult> removeDoc(int id)
        {

            IActionResult response = Unauthorized();
            var document = await _context.DocumentReceivables.Where(p=>p.ID == id).FirstOrDefaultAsync();
            var name = document.DocNo;


           _context.DocumentReceivables.Remove(document);
           await _context.SaveChangesAsync();

            response = Ok(new { name = name });
            return response;
        }

        public IActionResult GenRetention()
        {
            IActionResult response = Unauthorized();

            var retentions = new List<Retention>();
            retentions = _context.Retentions.Where(p=>p.Statuss == Retention.Status.Open).ToList();

            response = Ok(new { data = retentions });
            return response;
        }


        public IActionResult GenRetentionAll()
        {
            IActionResult response = Unauthorized();

            var retentions = new List<Retention>();
            retentions = _context.Retentions.ToList();

            response = Ok(new { data = retentions });
            return response;
        }
        public IActionResult CheckGroup(string job)
        {
            IActionResult response = Unauthorized();
            var group = new List<RegisGroupJob>();
           
            group =  _context.RegisGroupJobs.Where(p => p.GroupSite == job).ToList();

                response = Ok(new { group=  group});

            return response;
        }
        public IActionResult AddRetention(string date, string job, int statuss, string detail, decimal amount, string doc,string group)
        {
            IActionResult response = Unauthorized();

            var date1 = Convert.ToDateTime(date.Substring(6, 4) + "-" + date.Substring(3, 2) + "-" + date.Substring(0, 2) + " 00:00:00");
                Retention retention   = new Retention();


            retention.Detail = detail;
            retention.JobNo = job;
            retention.PostingDate = date1;
            retention.DocNo = doc;

            if (statuss == 0)
            {

                retention.Statuss = Retention.Status.Open;
            }
            else
            {
                retention.Statuss = Retention.Status.Close;
            }


           retention.Amount = amount;
            retention.Group = group;

            retention.CreateBy = HttpContext.Session.GetString("Username");
            retention.CreateDate = DateTime.Now;

            _context.Retentions.Add(retention);
            _context.SaveChanges();
            return Ok(retention);

            //response = Ok(new { data = documentReceivable });
            //return response;
        }
        public IActionResult getRetention(int id)
        {
            IActionResult response = Unauthorized();
            var retention = _context.Retentions.FirstOrDefault(m => m.ID == id);

            var ID =retention.ID;
            var detail = retention.Detail;
            var job = retention.JobNo;
            var amount = retention.Amount;
            var postingdate = retention.PostingDate.ToString("dd/MM/yyyy");
            var status = retention.Statuss;
            var doc = retention.DocNo;
            var group = retention.Group;

            response = Ok(new { ID = ID, detail = detail, job = job, amount = amount,  postingdate = postingdate, status = status, doc = doc,group=group });
            return response;
        }
        public IActionResult editRetention(int id,string date, string job, int status, string detail, decimal amount, string doc, string group)
        {
            IActionResult response = Unauthorized();

            var date1 = Convert.ToDateTime(date.Substring(6, 4) + "-" + date.Substring(3, 2) + "-" + date.Substring(0, 2) + " 00:00:00");
           
            Retention retention = new Retention();
            retention = _context.Retentions.Where(p => p.ID == id).FirstOrDefault();

            retention.Detail = detail;
            retention.JobNo = job;
            retention.PostingDate = DateTime.Now;
            retention.DocNo = doc;

            if (status == 0)
            {

                retention.Statuss = Retention.Status.Open;
            }
            else
            {
                retention.Statuss = Retention.Status.Close;
            }

            retention.Amount = amount;
            retention.Group = group;
            retention.EditBy = HttpContext.Session.GetString("Username");
            retention.EditDate = DateTime.Now;

            _context.Retentions.Update(retention);
            _context.SaveChanges();
            return Ok(retention);

            //response = Ok(new { data = retention});
            //return response;


        }




        [HttpPost, ActionName("removeRetention")]

        public async Task<IActionResult> removeRetention(int id)
        {

            IActionResult response = Unauthorized();
            var  retentions =  await _context.Retentions.Where(p => p.ID == id).FirstOrDefaultAsync();
            var name = retentions.DocNo;


            _context.Retentions.Remove(retentions);
            await _context.SaveChangesAsync();

            response = Ok(new { name = name });
            return response;
        }



        public async Task<IActionResult> SelectGroup(string site)
        {

            IActionResult response = Unauthorized();
            List<RegisGroupJob> RegisGroupJobs = new List<RegisGroupJob>();
            if (site ==null)
            {
                RegisGroupJobs = await _context.RegisGroupJobs.ToListAsync();
            }
            else
            {
                RegisGroupJobs = await _context.RegisGroupJobs.Where(p => p.GroupSite == site).ToListAsync();

            }
        
         

            response = Ok(new { data = RegisGroupJobs });
            return response;
        }



        public async Task<IActionResult> SelectGroupAmount(string Group)
        {
            
            IActionResult response = Unauthorized();
            var total = 0.0M;
            var SumAfter = _context.Retentions.Where(p => p.Group == Group).Sum(s=>s.Amount);



            var queryData = "SELECT " +
             "ROW_NUMBER() OVER (ORDER BY a.Doc) AS ID," +
             "(select top 1 convert(varchar,[Posting Date],23) FROM dbo."+ Environment.GetEnvironmentVariable("Company") +"Cust_ Ledger Entry] where [Document No_] LIKE '%'+a.Doc+'%') as PostingDate," +
             "(select top 1  dbo."+ Environment.GetEnvironmentVariable("Company") +"Cust_ Ledger Entry].[External Document No_] FROM dbo."+ Environment.GetEnvironmentVariable("Company") +"Cust_ Ledger Entry] where [Document No_] LIKE '%'+a.Doc+'%') as Doc, " +
             "(select top 1 [Global Dimension 1 Code] FROM dbo."+ Environment.GetEnvironmentVariable("Company") +"Cust_ Ledger Entry] where [Document No_] LIKE '%'+a.Doc+'%') as JobNo, " +
             "(select top 1  Description FROM dbo."+ Environment.GetEnvironmentVariable("Company") +"Cust_ Ledger Entry] where [Document No_] LIKE '%'+a.Doc+'%') as Description, " +
             "(select top 1  [Customer Posting Group] FROM dbo."+ Environment.GetEnvironmentVariable("Company") +"Cust_ Ledger Entry] where [Document No_] LIKE '%'+a.Doc+'%') as CustomerGroup, " +
             "CONVERT(DECIMAL(16,4),0.00)  as SumTotal," +
             "CONVERT(DECIMAL(16,4),0.00) as SumTotalVat, " +
             "CONVERT(DECIMAL(16,4),0.00) as TotalNAV, " +
             "(select isnull(SUM([Sales (LCY)]),0) FROM dbo."+ Environment.GetEnvironmentVariable("Company") +"Cust_ Ledger Entry] where [Document No_] LIKE a.Doc)+(select isnull(sum([Sales (LCY)]),0) FROM dbo."+ Environment.GetEnvironmentVariable("Company") +"Cust_ Ledger Entry] where [Document No_] LIKE a.Doc+'/R%') as Retention " +

             "FROM( " +
             "SELECT " +
             "dbo."+ Environment.GetEnvironmentVariable("Company") +"Cust_ Ledger Entry].[Document No_] as Doc " +
             "FROM " +
             "dbo."+ Environment.GetEnvironmentVariable("Company") +"Cust_ Ledger Entry] " +
             "WHERE [Open]=1 and [Document Type]=2 and [Document No_] Not Like '%/R%' and [Customer No_] Like '%R%' AND [Customer Posting Group]<>'003' " +
             "GROUP BY  " +
             "dbo."+ Environment.GetEnvironmentVariable("Company") +"Cust_ Ledger Entry].[Document No_]) as a order by Description";

            var AccountsReceivable = await _navcontext.AccountsReceivables.FromSqlRaw(queryData).ToListAsync();


            //var acc = new List<AccountsReceivable>();
            List<Tmp_AccGroup> instances = new List<Tmp_AccGroup>();
            Tmp_AccGroup tmp = null;
            GroupJob group;
            var name = "";
            RegisGroupJob RegisGroupJobs;
            //acc = AccountsReceivable.ToList();


            foreach (var std in AccountsReceivable)
            {
                //group = new GroupJob();
                group = await _context.GroupJobs.Where(p => p.DocumentNo == std.Doc).FirstOrDefaultAsync();
                if (group == null)
                {
                    name = "";
                }
                else
                {
                    RegisGroupJobs = await _context.RegisGroupJobs.Where(p => p.GroupCode == group.GroupNo).FirstOrDefaultAsync();
                    if (RegisGroupJobs == null)
                    {
                        name = "";
                    }
                    else
                    {
                        name = RegisGroupJobs.GroupCode;
                    }

                }


                tmp = new Tmp_AccGroup();
                tmp.Doc = std.Doc;
                tmp.Description = std.Description;
                tmp.PostingDate = std.PostingDate;
                tmp.JobNo = std.JobNo;
                tmp.CustomerGroup = std.CustomerGroup;
                tmp.SumTotal = std.SumTotal;
                tmp.SumTotalVat = std.SumTotalVat;
                tmp.TotalNAV = std.TotalNAV;
                tmp.Retention = std.Retention;
                tmp.GroupName = name;


                instances.Add(tmp);
            }


            total = instances.Where(p => p.GroupName == Group).Sum(s => s.Retention);
            total=total-SumAfter;





            response = Ok(new { data = total });
            return response;
        }


        public async Task<IActionResult> SelectEditGroupAmount(string Group)
        {

            IActionResult response = Unauthorized();
            var total = 0.0M;
            var SumAfter = _context.Retentions.Where(p => p.Group == Group).Sum(s => s.Amount);



            var queryData = "SELECT " +
             "ROW_NUMBER() OVER (ORDER BY a.Doc) AS ID," +
             "(select top 1 convert(varchar,[Posting Date],23) FROM dbo."+ Environment.GetEnvironmentVariable("Company") +"Cust_ Ledger Entry] where [Document No_] LIKE '%'+a.Doc+'%') as PostingDate," +
             "(select top 1  dbo."+ Environment.GetEnvironmentVariable("Company") +"Cust_ Ledger Entry].[External Document No_] FROM dbo."+ Environment.GetEnvironmentVariable("Company") +"Cust_ Ledger Entry] where [Document No_] LIKE '%'+a.Doc+'%') as Doc, " +
             "(select top 1 [Global Dimension 1 Code] FROM dbo."+ Environment.GetEnvironmentVariable("Company") +"Cust_ Ledger Entry] where [Document No_] LIKE '%'+a.Doc+'%') as JobNo, " +
             "(select top 1  Description FROM dbo."+ Environment.GetEnvironmentVariable("Company") +"Cust_ Ledger Entry] where [Document No_] LIKE '%'+a.Doc+'%') as Description, " +
             "(select top 1  [Customer Posting Group] FROM dbo."+ Environment.GetEnvironmentVariable("Company") +"Cust_ Ledger Entry] where [Document No_] LIKE '%'+a.Doc+'%') as CustomerGroup, " +
             "CONVERT(DECIMAL(16,4),0.00)  as SumTotal," +
             "CONVERT(DECIMAL(16,4),0.00) as SumTotalVat, " +
             "CONVERT(DECIMAL(16,4),0.00) as TotalNAV, " +
             "(select isnull(SUM([Sales (LCY)]),0) FROM dbo."+ Environment.GetEnvironmentVariable("Company") +"Cust_ Ledger Entry] where [Document No_] LIKE a.Doc)+(select isnull(sum([Sales (LCY)]),0) FROM dbo."+ Environment.GetEnvironmentVariable("Company") +"Cust_ Ledger Entry] where [Document No_] LIKE a.Doc+'/R%') as Retention " +

             "FROM( " +
             "SELECT " +
             "dbo."+ Environment.GetEnvironmentVariable("Company") +"Cust_ Ledger Entry].[Document No_] as Doc " +
             "FROM " +
             "dbo."+ Environment.GetEnvironmentVariable("Company") +"Cust_ Ledger Entry] " +
             "WHERE [Open]=1 and [Document Type]=2 and [Document No_] Not Like '%/R%' and [Customer No_] Like '%R%' AND [Customer Posting Group]<>'003' " +
             "GROUP BY  " +
             "dbo."+ Environment.GetEnvironmentVariable("Company") +"Cust_ Ledger Entry].[Document No_]) as a order by Description";

            var AccountsReceivable = await _navcontext.AccountsReceivables.FromSqlRaw(queryData).ToListAsync();


            //var acc = new List<AccountsReceivable>();
            List<Tmp_AccGroup> instances = new List<Tmp_AccGroup>();
            Tmp_AccGroup tmp = null;
            GroupJob group;
            var name = "";
            RegisGroupJob RegisGroupJobs;
            //acc = AccountsReceivable.ToList();


            foreach (var std in AccountsReceivable)
            {
                //group = new GroupJob();
                group = await _context.GroupJobs.Where(p => p.DocumentNo == std.Doc).FirstOrDefaultAsync();
                if (group == null)
                {
                    name = "";
                }
                else
                {
                    RegisGroupJobs = await _context.RegisGroupJobs.Where(p => p.GroupCode == group.GroupNo).FirstOrDefaultAsync();
                    if (RegisGroupJobs == null)
                    {
                        name = "";
                    }
                    else
                    {
                        name = RegisGroupJobs.GroupCode;
                    }

                }


                tmp = new Tmp_AccGroup();
                tmp.Doc = std.Doc;
                tmp.Description = std.Description;
                tmp.PostingDate = std.PostingDate;
                tmp.JobNo = std.JobNo;
                tmp.CustomerGroup = std.CustomerGroup;
                tmp.SumTotal = std.SumTotal;
                tmp.SumTotalVat = std.SumTotalVat;
                tmp.TotalNAV = std.TotalNAV;
                tmp.Retention = std.Retention;
                tmp.GroupName = name;


                instances.Add(tmp);
            }


            total = instances.Where(p => p.GroupName == Group).Sum(s => s.Retention);
            //total = total - SumAfter;





            response = Ok(new { data = total });
            return response;
        }


        public async Task<IActionResult> GendataGroupR()
        {
            IActionResult response = Unauthorized();


            var queryData = "SELECT " +
                "ROW_NUMBER() OVER (ORDER BY a.Doc) AS ID," +
                "(select top 1 convert(varchar,[Posting Date],23) FROM dbo."+ Environment.GetEnvironmentVariable("Company") +"Cust_ Ledger Entry] where [Document No_] LIKE '%'+a.Doc+'%') as PostingDate," +
                "(select top 1  dbo."+ Environment.GetEnvironmentVariable("Company") +"Cust_ Ledger Entry].[External Document No_] FROM dbo."+ Environment.GetEnvironmentVariable("Company") +"Cust_ Ledger Entry] where [Document No_] LIKE '%'+a.Doc+'%') as Doc, " +
                "(select top 1 [Global Dimension 1 Code] FROM dbo."+ Environment.GetEnvironmentVariable("Company") +"Cust_ Ledger Entry] where [Document No_] LIKE '%'+a.Doc+'%') as JobNo, " +
                "(select top 1  Description FROM dbo."+ Environment.GetEnvironmentVariable("Company") +"Cust_ Ledger Entry] where [Document No_] LIKE '%'+a.Doc+'%') as Description, " +
                "(select top 1  [Customer Posting Group] FROM dbo."+ Environment.GetEnvironmentVariable("Company") +"Cust_ Ledger Entry] where [Document No_] LIKE '%'+a.Doc+'%') as CustomerGroup, " +
                "CONVERT(DECIMAL(16,4),0.00)  as SumTotal," +
                "CONVERT(DECIMAL(16,4),0.00) as SumTotalVat, " +
                "CONVERT(DECIMAL(16,4),0.00) as TotalNAV, " +
                "(select isnull(SUM([Sales (LCY)]),0) FROM dbo."+ Environment.GetEnvironmentVariable("Company") +"Cust_ Ledger Entry] where [Document No_] LIKE a.Doc)+(select isnull(sum([Sales (LCY)]),0) FROM dbo."+ Environment.GetEnvironmentVariable("Company") +"Cust_ Ledger Entry] where [Document No_] LIKE a.Doc+'/R%') as Retention " +

                "FROM( " +
                "SELECT " +
                "dbo."+ Environment.GetEnvironmentVariable("Company") +"Cust_ Ledger Entry].[Document No_] as Doc " +
                "FROM " +
                "dbo."+ Environment.GetEnvironmentVariable("Company") +"Cust_ Ledger Entry] " +
                "WHERE [Open]=1 and [Document Type]=2 and [Document No_] Not Like '%/R%' and [Customer No_] Like '%R%' AND [Customer Posting Group]<>'003' " +
                "GROUP BY  " +
                "dbo."+ Environment.GetEnvironmentVariable("Company") +"Cust_ Ledger Entry].[Document No_]) as a order by Description";

            var AccountsReceivable = await _navcontext.AccountsReceivables.FromSqlRaw(queryData).ToListAsync();


            //var acc = new List<AccountsReceivable>();
            List<Tmp_AccGroup> instances = new List<Tmp_AccGroup>();
            Tmp_AccGroup tmp = null;
            GroupJob group;
            var name = "";
            var code = "";
            var groupdes = "";
            var paydata= 0.0M;
            RegisGroupJob RegisGroupJobs;
            //acc = AccountsReceivable.ToList();
    

            foreach (var std in AccountsReceivable)
            {
                //group = new GroupJob();
                group = await _context.GroupJobs.Where(p => p.DocumentNo == std.Doc).FirstOrDefaultAsync();
                if (group == null)
                {
                    code = "";
                    name = "";

                }
                else
                {
                    RegisGroupJobs = await _context.RegisGroupJobs.Where(p => p.GroupCode == group.GroupNo).FirstOrDefaultAsync();
                    if (RegisGroupJobs == null)
                    {
                        code = "";
                        name = "";
                        groupdes ="";
                    }
                    else
                    {
                        code = RegisGroupJobs.GroupCode;
                        name = RegisGroupJobs.GroupName;
                        groupdes = RegisGroupJobs.GroupDes;
                    }

                }
               

                tmp = new Tmp_AccGroup();
                tmp.Doc = std.Doc;
                tmp.Description = groupdes;
                tmp.PostingDate = std.PostingDate;
                tmp.JobNo = std.JobNo;
                tmp.CustomerGroup =code;
                tmp.SumTotal = std.SumTotal;
                tmp.SumTotalVat = std.SumTotalVat;
                tmp.TotalNAV = std.TotalNAV;
                tmp.Retention = std.Retention;
                tmp.GroupName = name;


                instances.Add(tmp);
            }



            var dataall = instances.GroupBy(l => l.GroupName)
                           .Select(cl => new Tmp_AccGroup
                           {
                               GroupName = cl.First().GroupName,
                               JobNo = cl.First().JobNo,
                               CustomerGroup = cl.First().CustomerGroup,
                               Description = cl.First().Description,
                               Retention = cl.Sum(c => c.Retention) -_context.Retentions.Where(p => p.Group == cl.First().CustomerGroup).Sum(s => s.Amount),
                           }).ToList();

            response = Ok(new { data =dataall.Where(p => p.Retention > 0).ToList() });


            return response;
        }







    }
}
