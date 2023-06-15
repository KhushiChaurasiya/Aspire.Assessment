using Assignment.Contracts.DTO;

namespace Assignment.Contracts.Data.Repositories
{
    public interface IRepository<T>
    {
        IEnumerable<T> GetAll();
        T Get(object id);
        T GetByEmail(object email);
        void Add(T entity);
        void Update(T entity);
        void Delete(object id);
        int Count();
        IEnumerable<DownloadReportDTO> GetDownloadedReport(DateTime? fromDate, DateTime? toDate);
        IEnumerable<LogsDTO> getLogReportWiseDate(DateTime? logwisedate);
    }
}