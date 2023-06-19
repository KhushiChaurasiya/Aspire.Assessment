using Assignment.Contracts.Data;
using Assignment.Contracts.DTO;
using Assignment.Core.Exceptions;
using AutoMapper;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assignment.Core.Handlers.Queries
{

    public class GetAllLogReportQuery : IRequest<IEnumerable<LogsDTO>>
    {
        public DateTime _logdate { get; }
        public string _level { get; }
        public GetAllLogReportQuery(DateTime logdate, string level)
        {
            this._logdate = logdate;
            _level = level;
        }
    }
    public class GetAllLogReportQueryHandler : IRequestHandler<GetAllLogReportQuery, IEnumerable<LogsDTO>>
    {
        private readonly IUnitOfWork _repository;
        private readonly IMapper _mapper;

        public GetAllLogReportQueryHandler(IUnitOfWork repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        public async Task<IEnumerable<LogsDTO>> Handle(GetAllLogReportQuery request, CancellationToken cancellationToken)
        {
            DateTime logdatereport = request._logdate;
            string level = request._level;
            var entities = await Task.FromResult(_repository.Errorlog.getLogReportWiseDate(logdatereport, level));
            if (entities == null)
            {
                throw new EntityNotFoundException($"There are no any data!");
            }
            return _mapper.Map<IEnumerable<LogsDTO>>(entities);
            
        }
    }
}
