using Assignment.Contracts.Data.Entities;
using Assignment.Contracts.Data.Repositories;
using Assignment.Core.Data.Repositories;
using Assignment.Migrations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assignment.Infrastructure.Data.Repositories
{
    public class AppdownloadRepository : Repository<AppDownload>, IAppdownloadRepository
    {
        public AppdownloadRepository(DatabaseContext context) : base(context)
        {
        }
    }
}
