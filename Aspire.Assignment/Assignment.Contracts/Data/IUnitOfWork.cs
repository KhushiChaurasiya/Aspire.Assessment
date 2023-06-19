using Assignment.Contracts.Data.Repositories;

namespace Assignment.Contracts.Data
{
    public interface IUnitOfWork
    {
        IAppRepository App { get; }
        IUserRepository User { get; }
        IAppdownloadRepository Appdownload { get; }
        IErrorLogRepository Errorlog { get; }
        Task CommitAsync();
    }
}