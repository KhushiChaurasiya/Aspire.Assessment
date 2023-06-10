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

        public IEnumerable<DownloadReportDTO> GetDownloadedReport()
        {
            try 
            { 
                var downList = _context.AppDownload.ToList();
               var  customDownList = downList.GroupBy(x => new { x.AppId, x.DownloadedDate }).Select(g => new { NumberOfDownloads = g.Count(), DownloadedDate = g.Key.DownloadedDate, AppId = g.Key.AppId }).ToList();
                var today = DateTime.Today;
                var finalDownloadedList = customDownList.Where(x => today >= x.DownloadedDate && today <= x.DownloadedDate).ToList();


                var ft = (from d in finalDownloadedList
                          join a in _context.App
                          on d.AppId equals a.Id into gj
                          from subpet in gj.DefaultIfEmpty()

                          select new DownloadReportDTO
                          {
                              AppName = subpet.Name,
                              DownlodedDate = d.DownloadedDate,
                              NumberOfDownloads = d.NumberOfDownloads
                          }).ToList();

                return (IEnumerable<DownloadReportDTO>)ft;
            }catch(Exception ex)
            {
                throw;
            }
        }

        //public IEnumerable<int> GetUserReport()
        //{
        //    int repList = _context.User.Count();
        //    return repList;
        //}
    }
}