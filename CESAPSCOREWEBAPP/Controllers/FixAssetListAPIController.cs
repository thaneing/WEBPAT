using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;
using CESAPSCOREWEBAPP.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace CESAPSCOREWEBAPP.Controllers
{



    [Route("api/[controller]")]
    [ApiController]
    public class FixAssetListAPIController : ControllerBase
    {
        private readonly NAVContext _navcontext;
        private readonly DatabaseContext _context;


        public FixAssetListAPIController(NAVContext navcontext, DatabaseContext context)
        {
            _context = context;
            _navcontext = navcontext;
        }


        // GET: api/FixAssetListAPI
        [HttpGet]
        public async Task<ActionResult<IEnumerable<FixAssetList>>> GetFixAssetList(string date1,string site)
        {
            IActionResult response = Unauthorized();

            var queryData = "SELECT b.ItemNo,b.Description,b.LocationCode,b.Quantity,ROW_NUMBER() OVER (ORDER BY b.ItemNo) AS ListNo,'' as Detail,'' as Status,'' as Status1,'' as Status2,'' as Curren,'' as Etc " +
                " FROM( SELECT a.ItemNo,a.Description,a.LocationCode," +
                "	(select sum(dbo."+ Environment.GetEnvironmentVariable("Company") +"Item Ledger Entry].Quantity) from dbo."+ Environment.GetEnvironmentVariable("Company") +"Item Ledger Entry]" +
                "	 WHERE [Location Code] = a.LocationCode and [Item No_] = a.ItemNo AND dbo."+ Environment.GetEnvironmentVariable("Company") +"Item Ledger Entry].[Posting Date]>='2018-10-23 00:00:00' " +
                " 		and dbo."+ Environment.GetEnvironmentVariable("Company") +"Item Ledger Entry].[Posting Date]<={0}) as Quantity" +
                " 	FROM( SELECT DISTINCT " +
                " 	dbo."+ Environment.GetEnvironmentVariable("Company") +"Item Ledger Entry].[Item No_] AS ItemNo," +
                " 	dbo."+ Environment.GetEnvironmentVariable("Company") +"Item].Description AS Description," +
                " 	dbo."+ Environment.GetEnvironmentVariable("Company") +"Item Ledger Entry].[Location Code] AS LocationCode" +
                " 	FROM dbo."+ Environment.GetEnvironmentVariable("Company") +"Item Ledger Entry]" +
                " 	LEFT JOIN dbo."+ Environment.GetEnvironmentVariable("Company") +"Item] ON dbo."+ Environment.GetEnvironmentVariable("Company") +"Item Ledger Entry].[Item No_] = dbo."+ Environment.GetEnvironmentVariable("Company") +"Item].No_" +
                " 	WHERE  dbo."+ Environment.GetEnvironmentVariable("Company") +"Item Ledger Entry].[Posting Date]>='2018-10-23 00:00:00' and dbo."+ Environment.GetEnvironmentVariable("Company") +"Item Ledger Entry].[Posting Date]<={0} " +
                "  and [Item No_] LIKE 'PC%'" +
                " 	) as a" +
                " ) as b  WHERE b.Quantity<>0 and b.LocationCode={1}  Order by b.ItemNo asc";


            //SqlParameter parameterDate = new SqlParameter("@date1", date1);
            //SqlParameter parameterSite = new SqlParameter("@site", site);

    



            return await _navcontext.fixAssetLists.FromSqlRaw(queryData, date1, site.ToUpper()).ToListAsync();
        }

    }
}