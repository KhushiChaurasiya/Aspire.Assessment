using Microsoft.EntityFrameworkCore;
using Assignment.Contracts.Data.Repositories;
using Assignment.Migrations;
using Microsoft.VisualBasic;
using Assignment.Contracts.DTO;
using Assignment.Contracts.Data.Entities;
using System.Linq;
using System.Globalization;

namespace Assignment.Core.Data.Repositories
{
    public class Repository<T> : IRepository<T> where T : class
    {
        private readonly DatabaseContext _context;
        private readonly DbSet<T> _dbSet;

        public Repository(DatabaseContext context)
        {
            _context = context;
            _dbSet = _context.Set<T>();
        }

        public void Add(T entity)
        {
            _dbSet.Add(entity);
        }

        public int Count()
        {
            return _dbSet.Count();
        }

        public void Delete(object id)
        {
            var entity = Get(id);
            if (entity != null)
            {
                if (_context.Entry(entity).State == EntityState.Detached)
                {
                    _dbSet.Attach(entity);
                }
                _dbSet.Remove(entity);
            }
        }

        public T Get(object id)
        {
            var x = _dbSet.Find(id);
            return x;
        }

        public T GetByEmail(object email)
        {
            var x = _dbSet.Find();
            return x;
        }
        

        public IEnumerable<T> GetAll()
        {
            return _dbSet.AsEnumerable();
        }

        public void Update(T entity)
        {
            _dbSet.Attach(entity);
            _context.Entry(entity).State = EntityState.Modified;
        }

        public IEnumerable<DownloadReportDTO> GetDownloadedReport(DateTime? fromDate, DateTime? toDate)
        {
            try 
            { 
                var downList = _context.AppDownload.ToList();
                var  customDownList = downList.GroupBy(x => new { x.AppId, x.DownloadedDate }).Select(g => new { NumberOfDownloads = g.Count(), DownloadedDate = g.Key.DownloadedDate, AppId = g.Key.AppId }).ToList();
                if (fromDate == null)
                {
                    fromDate = DateTime.Today;
                }
                if (toDate == null)
                {
                    toDate = DateTime.Today;
                }
                var DownloadedList = customDownList.Where(x => x.DownloadedDate >= fromDate &&  x.DownloadedDate <= toDate).ToList();
                var finalDownloadedList = DownloadedList.GroupBy(x => x.AppId).Select(g => new { AppId = g.Key, NumberOfDownloads = g.Sum(c => c.NumberOfDownloads) }).ToList();


                var ft = (from d in finalDownloadedList
                          join a in _context.App
                          on d.AppId equals a.Id into gj
                          from subpet in gj.DefaultIfEmpty()

                          select new DownloadReportDTO
                          {
                              AppName = subpet.Name,
                              NumberOfDownloads = d.NumberOfDownloads
                          }).ToList();

                return (IEnumerable<DownloadReportDTO>)ft;
            }
            catch(Exception ex)
            {
                throw;
            }
        }

        public IEnumerable<LogsDTO> getLogReportWiseDate(DateTime? logwisedate)
        {
            var logreport = _context.Logs.ToList();
            //var finalLogReport = logreport.Where(x=>x.CreatedOn.)
            return (IEnumerable<LogsDTO>)logreport;
        }
    }
}