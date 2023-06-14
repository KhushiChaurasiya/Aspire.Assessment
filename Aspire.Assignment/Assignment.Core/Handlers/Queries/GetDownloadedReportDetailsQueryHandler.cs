using Assignment.Contracts.Data;
using Assignment.Contracts.DTO;
using Assignment.Core.Exceptions;
using Assignment.Providers.Handlers.Queries;
using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata;
using System.Text;
using System.Threading.Tasks;

namespace Assignment.Core.Handlers.Queries
{
    //public class GetDownloadedReportDetailsQuery : IRequest<IEnumerable<DownloadReportDTO>>
    //{
    //}

    public class GetDownloadedReportDetailsQuery : IRequest<IEnumerable<DownloadReportDTO>>
    {
        public DateTime? _fromDate { get; }
        public DateTime? _toDate { get; }
        public GetDownloadedReportDetailsQuery(DateTime? fromDate, DateTime? toDate)
        {
           this._fromDate = fromDate;
           this._toDate = toDate;
        }

    }
    public class GetDownloadedReportDetailsQueryHandler : IRequestHandler<GetDownloadedReportDetailsQuery, IEnumerable<DownloadReportDTO>>
    {
        private readonly IUnitOfWork _repository;
        private readonly IMapper _mapper;

        public GetDownloadedReportDetailsQueryHandler(IUnitOfWork repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        public async Task<IEnumerable<DownloadReportDTO>> Handle(GetDownloadedReportDetailsQuery request, CancellationToken cancellationToken)
        {
            DateTime? fromdatetime = request._fromDate;
            DateTime? todatetime = request._toDate;

            var data = await Task.FromResult(_repository.App.GetDownloadedReport(fromdatetime, todatetime));
            return (IEnumerable<DownloadReportDTO>)data;
        }
    }
}
