using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CESAPSCOREWEBAPP.Models
{
    public partial class NAVSuperContext : DbContext
    {
        public NAVSuperContext(DbContextOptions<NAVSuperContext> options)
            : base(options)
        {


        }
    }
}

