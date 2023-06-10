using Assignment.Contracts.Data;
using Assignment.Contracts.DTO;
using Assignment.Core.Exceptions;
using Assignment.Providers.Handlers.Queries;
using AutoMapper;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata;
using System.Text;
using System.Threading.Tasks;

namespace Assignment.Core.Handlers.Queries
{
    public class GetDownloadedReportDetailsQuery : IRequest<IEnumerable<DownloadReportDTO>>
    {
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

        //public void Handle(GetDownloadedReportDetailsQuery request, CancellationToken cancellationToken)
        //{
        //   var data = await Task.FromResult(_repository.App.GetDownloadedReport());

        //}

        public async Task<IEnumerable<DownloadReportDTO>> Handle(GetDownloadedReportDetailsQuery request, CancellationToken cancellationToken)
        {
            var data = await Task.FromResult(_repository.App.GetDownloadedReport());
            return (IEnumerable<DownloadReportDTO>)data;
        }
    }
}
