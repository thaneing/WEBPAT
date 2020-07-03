using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CESAPSCOREWEBAPP.Models
{
    public class DatabaseInit
    {
        public static void INIT(IServiceProvider ServiceProvider)
        {
            var context = new DatabaseContext(ServiceProvider.GetRequiredService<DbContextOptions<DatabaseContext>>());
            // If database does not exist then the database and all its schema are created
            context.Database.EnsureCreated();

            var NAVcontext = new NAVContext(ServiceProvider.GetRequiredService<DbContextOptions<NAVContext>>());


            var context1 = new UpdateContext(ServiceProvider.GetRequiredService<DbContextOptions<UpdateContext>>());
            // If database does not exist then the database and all its schema are created

            var NASuperVcontext = new NAVSuperContext(ServiceProvider.GetRequiredService<DbContextOptions<NAVSuperContext>>());

        }
    }
}
